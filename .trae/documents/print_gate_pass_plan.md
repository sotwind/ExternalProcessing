# 审批后打印出门单功能计划

## 需求概述

审批通过后，需要打印出门单（也叫放行单、出门证），用于物品出厂/出门的凭证。

## 出门单内容设计

出门单应包含以下信息：

### 表头信息
- 标题：外发加工出门单
- 单号：申请编号（ApplicationNo）
- 日期：审批日期
- 出门单号：可单独生成（如 GT-20240307-001）

### 基本信息
- 申请编号
- 订单编号
- 申请人
- 申请日期
- 加工商
- 加工内容
- 预计归还日期
- 审批人
- 审批日期
- 审批意见

### 明细信息（如果有）
- 物品名称
- 规格型号
- 数量
- 单位
- 备注

### 底部信息
- 出门时间（留空，由门卫填写）
- 门卫签字（留空）
- 备注

---

## 实现方案

### 方案一：使用 PrintDocument 直接打印（推荐）

使用 .NET 的 `System.Drawing.Printing.PrintDocument` 类直接绘制打印内容。

**优点**：
- 无需第三方库
- 轻量级
- 可精确控制打印格式

**缺点**：
- 需要手动绘制每个元素
- 代码量较多

### 方案二：使用 Word 模板打印

使用 Word 模板，通过代码填充数据后打印。

**优点**：
- 模板易于修改
- 可视化设计

**缺点**：
- 需要安装 Word
- 依赖 Office Interop

### 方案三：使用报表工具（如 RDLC）

使用 Visual Studio 的报表设计器创建 RDLC 报表。

**优点**：
- 专业报表功能
- 支持预览

**缺点**：
- 需要额外学习
- 增加项目复杂度

---

## 推荐实施方案：方案一（PrintDocument）

### 实施步骤

#### 1. 创建 GatePassPrintDocument 类

创建一个新的打印文档类，专门用于打印出门单：

```csharp
using System;
using System.Drawing;
using System.Drawing.Printing;
using ExternalProcessing.Models;

namespace ExternalProcessing.Printing;

public class GatePassPrintDocument : PrintDocument
{
    private readonly ExternalProcessingApplication _application;
    private readonly ExternalProcessingAudit _audit;
    private readonly List<ExternalProcessingApplicationDetail> _details;
    
    public GatePassPrintDocument(ExternalProcessingApplication application, 
        ExternalProcessingAudit audit, 
        List<ExternalProcessingApplicationDetail> details)
    {
        _application = application;
        _audit = audit;
        _details = details;
        DocumentName = $"出门单_{application.ApplicationNo}";
    }
    
    protected override void OnPrintPage(PrintPageEventArgs e)
    {
        base.OnPrintPage(e);
        
        var g = e.Graphics;
        var pageBounds = e.PageBounds;
        
        // 设置字体
        var titleFont = new Font("宋体", 18, FontStyle.Bold);
        var headerFont = new Font("宋体", 12, FontStyle.Bold);
        var normalFont = new Font("宋体", 10);
        var smallFont = new Font("宋体", 9);
        
        // 打印标题
        var title = "外发加工出门单";
        var titleSize = g.MeasureString(title, titleFont);
        g.DrawString(title, titleFont, Brushes.Black, 
            (pageBounds.Width - titleSize.Width) / 2, 40);
        
        // 打印单号和日期
        g.DrawString($"单号：{_application.ApplicationNo}", normalFont, Brushes.Black, 50, 80);
        g.DrawString($"日期：{_audit.AuditDate:yyyy-MM-dd HH:mm}", normalFont, Brushes.Black, 400, 80);
        
        // 打印分隔线
        g.DrawLine(Pens.Black, 50, 105, pageBounds.Width - 50, 105);
        
        // 打印基本信息
        var y = 120;
        var lineHeight = 25;
        
        g.DrawString("申请编号：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_application.ApplicationNo ?? "", normalFont, Brushes.Black, 120, y);
        
        g.DrawString("订单编号：", headerFont, Brushes.Black, 300, y);
        g.DrawString(_application.OrderNo ?? "", normalFont, Brushes.Black, 370, y);
        y += lineHeight;
        
        g.DrawString("申请人：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_application.ApplicantName ?? "", normalFont, Brushes.Black, 120, y);
        
        g.DrawString("申请日期：", headerFont, Brushes.Black, 300, y);
        g.DrawString(_application.ApplicationDate.ToString("yyyy-MM-dd"), normalFont, Brushes.Black, 370, y);
        y += lineHeight;
        
        g.DrawString("加工商：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_application.ProcessorName ?? "", normalFont, Brushes.Black, 120, y);
        y += lineHeight;
        
        g.DrawString("预计归还：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_application.ExpectedReturnDate.ToString("yyyy-MM-dd"), normalFont, Brushes.Black, 120, y);
        y += lineHeight + 10;
        
        // 打印加工内容
        g.DrawString("加工内容：", headerFont, Brushes.Black, 50, y);
        y += lineHeight;
        
        // 处理长文本换行
        var contentRect = new RectangleF(50, y, pageBounds.Width - 100, 60);
        g.DrawString(_application.ProcessingContent ?? "", normalFont, Brushes.Black, contentRect);
        y += 70;
        
        // 打印审批信息
        g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
        y += 10;
        
        g.DrawString("审批信息", headerFont, Brushes.Black, 50, y);
        y += lineHeight;
        
        g.DrawString("审批人：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_audit.AuditorName ?? "", normalFont, Brushes.Black, 120, y);
        
        g.DrawString("审批日期：", headerFont, Brushes.Black, 300, y);
        g.DrawString(_audit.AuditDate.ToString("yyyy-MM-dd HH:mm"), normalFont, Brushes.Black, 370, y);
        y += lineHeight;
        
        g.DrawString("审批结果：", headerFont, Brushes.Black, 50, y);
        g.DrawString(_audit.AuditResultText, normalFont, Brushes.Black, 120, y);
        y += lineHeight;
        
        g.DrawString("审批意见：", headerFont, Brushes.Black, 50, y);
        y += lineHeight;
        
        var remarkRect = new RectangleF(50, y, pageBounds.Width - 100, 40);
        g.DrawString(_audit.AuditRemark ?? "", normalFont, Brushes.Black, remarkRect);
        y += 50;
        
        // 打印明细（如果有）
        if (_details.Count > 0)
        {
            g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
            y += 10;
            
            g.DrawString("物品明细", headerFont, Brushes.Black, 50, y);
            y += lineHeight + 5;
            
            // 打印表头
            g.DrawString("序号", headerFont, Brushes.Black, 50, y);
            g.DrawString("物品名称", headerFont, Brushes.Black, 100, y);
            g.DrawString("规格", headerFont, Brushes.Black, 250, y);
            g.DrawString("数量", headerFont, Brushes.Black, 350, y);
            g.DrawString("备注", headerFont, Brushes.Black, 420, y);
            y += lineHeight;
            
            // 打印数据
            for (int i = 0; i < _details.Count; i++)
            {
                var detail = _details[i];
                g.DrawString((i + 1).ToString(), normalFont, Brushes.Black, 50, y);
                g.DrawString(detail.ItemName ?? "", normalFont, Brushes.Black, 100, y);
                g.DrawString(detail.Specification ?? "", normalFont, Brushes.Black, 250, y);
                g.DrawString(detail.Quantity.ToString(), normalFont, Brushes.Black, 350, y);
                g.DrawString(detail.Remark ?? "", normalFont, Brushes.Black, 420, y);
                y += lineHeight;
            }
        }
        
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

#### 2. 修改 AuditEditForm.cs

在审批保存成功后，询问用户是否打印出门单：

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

        var auditId = _auditService.AddAudit(audit);
        audit.AuditId = auditId;
        audit.AuditDate = DateTime.Now;

        // 更新申请状态
        _auditService.UpdateApplicationStatus(_application.ApplicationId, auditResult);

        MessageBox.Show("审批成功", "提示");
        
        // 如果审批通过，询问是否打印出门单
        if (auditResult == 2) // 已通过
        {
            var result = MessageBox.Show("审批通过，是否打印出门单？", "打印", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            
            if (result == DialogResult.Yes)
            {
                PrintGatePass(audit);
            }
        }
        
        this.DialogResult = DialogResult.OK;
        this.Close();
    }
    catch (Exception ex)
    {
        MessageBox.Show("审批失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
}

private void PrintGatePass(ExternalProcessingAudit audit)
{
    try
    {
        // 获取申请明细
        var details = _auditService.GetApplicationDetails(_application.ApplicationId);
        
        // 创建打印文档
        var printDoc = new GatePassPrintDocument(_application, audit, details);
        
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
```

#### 3. 添加 GetApplicationDetails 方法到 ExternalProcessingAuditService

```csharp
public List<ExternalProcessingApplicationDetail> GetApplicationDetails(int applicationId)
{
    var details = new List<ExternalProcessingApplicationDetail>();
    var sql = "SELECT * FROM ExternalProcessingApplicationDetails WHERE ApplicationId = @ApplicationId";

    using var connection = DbHelper.CreateConnection();
    connection.Open();
    using var command = new SqlCommand(sql, connection);
    command.Parameters.AddWithValue("@ApplicationId", applicationId);

    using var reader = command.ExecuteReader();
    while (reader.Read())
    {
        details.Add(MapToDetail(reader));
    }

    return details;
}

private ExternalProcessingApplicationDetail MapToDetail(SqlDataReader reader)
{
    return new ExternalProcessingApplicationDetail
    {
        DetailId = reader["DetailId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DetailId"]),
        ApplicationId = reader["ApplicationId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicationId"]),
        ItemId = reader["ItemId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ItemId"]),
        ItemName = reader["ItemName"] == DBNull.Value ? null : reader["ItemName"].ToString(),
        Specification = reader["Specification"] == DBNull.Value ? null : reader["Specification"].ToString(),
        Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
        UnitPrice = reader["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["UnitPrice"]),
        TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalAmount"]),
        Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
        OperatorId = reader["OperatorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OperatorId"]),
        OperatorTime = reader["OperatorTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["OperatorTime"])
    };
}
```

#### 4. 在 AuditForm 中也添加打印功能

在 AuditForm 中添加一个"打印出门单"按钮，允许对已审批的单据补打出门单：

```csharp
// 在 InitializeComponent 中添加打印按钮
this.BtnPrintGatePass = new Button();
this.BtnPrintGatePass.Text = "打印出门单";
this.BtnPrintGatePass.Click += new EventHandler(this.BtnPrintGatePass_Click);

private void BtnPrintGatePass_Click(object sender, EventArgs e)
{
    if (DgvApplications.SelectedRows.Count == 0)
    {
        MessageBox.Show("请选择要打印出门单的记录", "提示");
        return;
    }

    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    // 只有已审批的单据才能打印出门单
    if (application.Status != 2)
    {
        MessageBox.Show("只有已审批的记录才能打印出门单", "提示");
        return;
    }
    
    // 获取审批记录
    var audits = _auditService.GetAuditsByApplicationId(application.ApplicationId);
    var latestAudit = audits.FirstOrDefault();
    
    if (latestAudit == null)
    {
        MessageBox.Show("未找到审批记录", "错误");
        return;
    }
    
    // 打印出门单
    PrintGatePass(application, latestAudit);
}
```

---

## 打印效果预览

```
                    外发加工出门单
    单号：EPA202403070001          日期：2024-03-07 14:30
    ─────────────────────────────────────────────────
    
    申请编号：EPA202403070001        订单编号：ORD202403070001
    申请人：张三                      申请日期：2024-03-07
    加工商：鑫源包装厂
    预计归还：2024-03-14
    
    加工内容：
    纸箱印刷加工，数量5000个，要求防水防潮处理
    
    ─────────────────────────────────────────────────
    审批信息
    审批人：李四                      审批日期：2024-03-07 15:00
    审批结果：已通过
    审批意见：同意，请按时归还
    
    ─────────────────────────────────────────────────
    出门时间：_______年_____月_____日 _____时_____分
    门卫签字：__________________    经办人签字：__________________
    
    备注：_______________________________________________
    
    此联由门卫留存                                    打印时间：2024-03-07 15:01:30
```

---

## 实施顺序

1. **第一步**：创建 `GatePassPrintDocument.cs` 打印文档类
2. **第二步**：添加 `GetApplicationDetails` 方法到 `ExternalProcessingAuditService.cs`
3. **第三步**：修改 `AuditEditForm.cs`，在审批通过后添加打印询问
4. **第四步**：修改 `AuditForm.cs`，添加补打出门单按钮

---

## 预期效果

1. 审批通过后，系统自动询问是否打印出门单
2. 显示打印预览窗口，用户可以查看效果后决定是否打印
3. 支持对已审批的单据补打出门单
4. 出门单格式规范，包含所有必要信息
5. 打印的出门单有门卫签字栏和出门时间栏，方便现场使用
