# 出门单多订单合并打印功能计划

## 需求概述

根据用户反馈，需要改进出门单打印功能：
1. 支持多个单子合并打印（一张出门单上显示多个订单）
2. 添加公司抬头：森林包装集团股份有限公司
3. 显示元素：日期、加工订单号列表、加工工艺列表、数量列表
4. 底部显示：申请人（多个申请人需要去重显示）、审批人
5. **审批通过后不询问，审批人在审批管理界面勾选多个订单后打印**

## 关键理解

- 多个订单合并打印时，每个订单的申请人可能不同
- 底部显示的申请人需要将所有不同申请人列出来（去重）
- 不是按部门分组，只是简单列出所有不同的申请人

## 出门单新格式设计

```
                    森林包装集团股份有限公司
                        外发加工出门单
    
    日期：2024-03-07                                    出门单号：GT-20240307-001
    ─────────────────────────────────────────────────────────────────
    
    加工订单号          加工工艺          数量
    ─────────────────────────────────────────────────────────────────
    ORD202403070001     纸箱印刷加工      5000个
    ORD202403070002     纸盒折叠加工      3000个
    ORD202403070003     标签粘贴加工      10000个
    ...
    
    ─────────────────────────────────────────────────────────────────
    
    申请人：张三、李四、王五              审批人：赵六
    
    ─────────────────────────────────────────────────────────────────
    出门时间：_______年_____月_____日 _____时_____分
    门卫签字：__________________    经办人签字：__________________
    
    备注：___________________________________________________________
    
    此联由门卫留存                                    打印时间：2024-03-07 15:01:30
```

---

## 实现方案

### 1. 创建新的数据模型 GatePassPrintModel

用于存储合并打印的数据：

```csharp
namespace ExternalProcessing.Printing;

public class GatePassPrintModel
{
    // 公司抬头
    public string CompanyName { get; set; } = "森林包装集团股份有限公司";
    
    // 出门单号
    public string GatePassNo { get; set; } = "";
    
    // 打印日期
    public DateTime PrintDate { get; set; } = DateTime.Now;
    
    // 订单明细列表
    public List<GatePassOrderItem> OrderItems { get; set; } = new();
    
    // 申请人列表（去重后的所有申请人）
    public List<string> ApplicantNames { get; set; } = new();
    
    // 审批人
    public string AuditorName { get; set; } = "";
    
    // 审批日期
    public DateTime AuditDate { get; set; }
}

public class GatePassOrderItem
{
    public string OrderNo { get; set; } = "";
    public string ProcessingContent { get; set; } = "";
    public int Quantity { get; set; }
    public string Unit { get; set; } = "个";
}
```

### 2. 修改 GatePassPrintDocument 类

重写打印逻辑，支持多订单合并打印：

```csharp
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;
using ExternalProcessing.Models;

namespace ExternalProcessing.Printing;

public class GatePassPrintDocument : PrintDocument
{
    private readonly GatePassPrintModel _model;

    public GatePassPrintDocument(GatePassPrintModel model)
    {
        _model = model;
        DocumentName = $"出门单_{_model.GatePassNo}";
    }

    protected override void OnPrintPage(PrintPageEventArgs e)
    {
        base.OnPrintPage(e);

        var g = e.Graphics;
        var pageBounds = e.PageBounds;

        // 设置字体
        var companyFont = new Font("宋体", 16, FontStyle.Bold);
        var titleFont = new Font("宋体", 14, FontStyle.Bold);
        var headerFont = new Font("宋体", 11, FontStyle.Bold);
        var normalFont = new Font("宋体", 10);
        var smallFont = new Font("宋体", 9);

        // 打印公司抬头
        var companyTitle = _model.CompanyName;
        var companySize = g.MeasureString(companyTitle, companyFont);
        g.DrawString(companyTitle, companyFont, Brushes.Black,
            (pageBounds.Width - companySize.Width) / 2, 30);

        // 打印标题
        var title = "外发加工出门单";
        var titleSize = g.MeasureString(title, titleFont);
        g.DrawString(title, titleFont, Brushes.Black,
            (pageBounds.Width - titleSize.Width) / 2, 55);

        // 打印日期和出门单号
        g.DrawString($"日期：{_model.PrintDate:yyyy-MM-dd}", normalFont, Brushes.Black, 50, 90);
        g.DrawString($"出门单号：{_model.GatePassNo}", normalFont, Brushes.Black, 400, 90);

        // 打印分隔线
        g.DrawLine(Pens.Black, 50, 115, pageBounds.Width - 50, 115);

        // 打印表头
        var y = 130;
        var lineHeight = 25;

        g.DrawString("加工订单号", headerFont, Brushes.Black, 50, y);
        g.DrawString("加工工艺", headerFont, Brushes.Black, 250, y);
        g.DrawString("数量", headerFont, Brushes.Black, 450, y);
        y += lineHeight;

        // 打印分隔线
        g.DrawLine(Pens.Black, 50, y - 5, pageBounds.Width - 50, y - 5);

        // 打印订单明细
        foreach (var item in _model.OrderItems)
        {
            g.DrawString(item.OrderNo, normalFont, Brushes.Black, 50, y);
            g.DrawString(item.ProcessingContent, normalFont, Brushes.Black, 250, y);
            g.DrawString($"{item.Quantity} {item.Unit}", normalFont, Brushes.Black, 450, y);
            y += lineHeight;
        }

        // 打印分隔线
        y += 10;
        g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
        y += 15;

        // 打印申请人（多个申请人用顿号分隔，去重）
        var applicants = string.Join("、", _model.ApplicantNames.Distinct());
        g.DrawString($"申请人：{applicants}", headerFont, Brushes.Black, 50, y);

        // 打印审批人
        g.DrawString($"审批人：{_model.AuditorName}", headerFont, Brushes.Black, 300, y);
        y += lineHeight + 10;

        g.DrawString($"审批日期：{_model.AuditDate:yyyy-MM-dd}", headerFont, Brushes.Black, 50, y);

        // 打印底部信息
        y = pageBounds.Height - 150;
        g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
        y += 15;

        g.DrawString("出门时间：_______年_____月_____日 _____时_____分", normalFont, Brushes.Black, 50, y);
        y += lineHeight + 10;

        g.DrawString("门卫签字：__________________", normalFont, Brushes.Black, 50, y);
        g.DrawString("经办人签字：__________________", normalFont, Brushes.Black, 300, y);
        y += lineHeight + 10;

        g.DrawString("备注：", headerFont, Brushes.Black, 50, y);
        g.DrawLine(Pens.Black, 90, y + 18, pageBounds.Width - 50, y + 18);

        // 打印页脚
        g.DrawString("此联由门卫留存", smallFont, Brushes.Gray, 50, pageBounds.Height - 50);
        g.DrawString($"打印时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}", smallFont, Brushes.Gray,
            pageBounds.Width - 200, pageBounds.Height - 50);
    }
}
```

### 3. 修改 AuditForm，支持多选和合并打印

#### 3.1 修改 DataGridView 允许多选

```csharp
// 修改 DgvApplications 配置
this.DgvApplications.MultiSelect = true;  // 允许多选
this.DgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
```

#### 3.2 修改打印出门单按钮逻辑

```csharp
private void BtnPrintGatePass_Click(object sender, EventArgs e)
{
    if (DgvApplications.SelectedRows.Count == 0)
    {
        MessageBox.Show("请选择要打印出门单的记录", "提示");
        return;
    }

    // 获取所有选中的已审批记录
    var selectedApplications = new List<ExternalProcessingApplication>();
    foreach (DataGridViewRow row in DgvApplications.SelectedRows)
    {
        var application = (ExternalProcessingApplication)row.DataBoundItem;
        if (application.Status == 2) // 已审批
        {
            selectedApplications.Add(application);
        }
    }

    if (selectedApplications.Count == 0)
    {
        MessageBox.Show("请至少选择一条已审批的记录", "提示");
        return;
    }

    // 生成出门单数据模型
    var gatePassModel = CreateGatePassModel(selectedApplications);

    try
    {
        // 创建打印文档
        var printDoc = new GatePassPrintDocument(gatePassModel);

        // 显示打印预览对话框
        var previewDlg = new PrintPreviewDialog();
        previewDlg.Document = printDoc;
        previewDlg.WindowState = FormWindowState.Maximized;

        if (previewDlg.ShowDialog() == DialogResult.OK)
        {
            // 用户确认后执行打印
            printDoc.Print();
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("打印失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private GatePassPrintModel CreateGatePassModel(List<ExternalProcessingApplication> applications)
{
    var model = new GatePassPrintModel
    {
        GatePassNo = $"GT-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
        PrintDate = DateTime.Now
    };

    // 收集所有订单信息
    foreach (var app in applications)
    {
        // 获取申请明细
        var details = _auditService.GetApplicationDetails(app.ApplicationId);
        
        if (details.Count > 0)
        {
            foreach (var detail in details)
            {
                model.OrderItems.Add(new GatePassOrderItem
                {
                    OrderNo = app.OrderNo ?? app.ApplicationNo ?? "",
                    ProcessingContent = detail.ItemName ?? app.ProcessingContent ?? "",
                    Quantity = detail.Quantity,
                    Unit = "个"
                });
            }
        }
        else
        {
            // 如果没有明细，使用申请主信息
            model.OrderItems.Add(new GatePassOrderItem
            {
                OrderNo = app.OrderNo ?? app.ApplicationNo ?? "",
                ProcessingContent = app.ProcessingContent ?? "",
                Quantity = 0,
                Unit = "个"
            });
        }

        // 收集申请人（去重）
        var applicantName = app.ApplicantName ?? "未知";
        if (!model.ApplicantNames.Contains(applicantName))
        {
            model.ApplicantNames.Add(applicantName);
        }

        // 获取审批记录
        var audits = _auditService.GetAuditsByApplicationId(app.ApplicationId);
        var latestAudit = audits.FirstOrDefault();
        if (latestAudit != null)
        {
            model.AuditorName = latestAudit.AuditorName ?? "";
            model.AuditDate = latestAudit.AuditDate;
        }
    }

    return model;
}
```

### 4. 修改 AuditEditForm（审批通过后不询问）

```csharp
private void BtnSave_Click(object sender, EventArgs e)
{
    try
    {
        var selectedItem = CboAuditResult.SelectedItem as ComboBoxItem;
        var auditResult = selectedItem?.Value as int? ?? 2;

        // 添加审批记录
        var audit = new ExternalProcessingAudit
        {
            ApplicationId = _application.ApplicationId,
            AuditorId = 1, // 当前用户ID
            AuditorName = "当前用户",
            AuditResult = auditResult,
            AuditRemark = TxtAuditRemark.Text.Trim(),
            OperatorId = 1
        };

        _auditService.AddAudit(audit);

        // 更新申请状态
        _auditService.UpdateApplicationStatus(_application.ApplicationId, auditResult);

        MessageBox.Show("审批成功", "提示");
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show("审批失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}
```

---

## 实施顺序

1. **第一步**：创建 `GatePassPrintModel.cs` 数据模型类
2. **第二步**：修改 `GatePassPrintDocument.cs`，使用新的数据模型
3. **第三步**：修改 `AuditForm.cs`：
   - 允许多选
   - 修改打印逻辑支持合并打印
   - 添加生成出门单数据模型的方法（申请人去重）
4. **第四步**：修改 `AuditEditForm.cs`，移除审批后的打印询问

---

## 预期效果

1. 出门单显示公司抬头"森林包装集团股份有限公司"
2. 支持在审批管理中选择多个已审批订单进行合并打印（按住Ctrl键多选）
3. 出门单格式为表格形式，显示订单号、加工工艺、数量
4. 底部显示所有不同的申请人（去重，用顿号分隔）和审批人信息
5. 审批通过后不询问，审批人自己在审批管理界面操作打印
