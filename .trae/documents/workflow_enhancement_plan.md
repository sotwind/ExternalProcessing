# 外发加工管理系统工作流增强计划

## 问题概述

当前系统存在以下问题需要解决：

1. **状态控制不严格**：已审批、已外发、已验收等状态的单据仍可在申请管理中编辑和删除
2. **缺乏审批历史查看**：无法查看审批意见、拒绝理由等历史记录
3. **拒绝后无法重新提交**：被拒绝的单据修改后无法正确重新提交
4. **验收管理数据问题**：已审批的单据在验收管理中找不到

## 解决方案

### 1. 状态控制与编辑删除权限

**目标**：只有"待审批"和"已拒绝"状态的单据才允许编辑和删除

**实现步骤**：

#### 1.1 修改 ApplicationForm.cs

* 在 `BtnEdit_Click` 方法中添加状态检查

* 在 `BtnDelete_Click` 方法中添加状态检查

* 只有 Status = 1 (待审批) 或 Status = 3 (已拒绝) 时才允许操作

```csharp
// 编辑按钮点击事件
private void BtnEdit_Click(object sender, EventArgs e)
{
    if (DgvApplications.SelectedRows.Count == 0)
    {
        MessageBox.Show("请选择要编辑的记录", "提示");
        return;
    }

    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    // 检查状态：只有待审批(1)或已拒绝(3)才能编辑
    if (application.Status != 1 && application.Status != 3)
    {
        MessageBox.Show("只有待审批或已拒绝的记录才能编辑，如需修改请先反审", "提示");
        return;
    }
    
    using var form = new ApplicationEditForm(application);
    if (form.ShowDialog() == DialogResult.OK)
    {
        LoadApplications();
    }
}

// 删除按钮点击事件
private void BtnDelete_Click(object sender, EventArgs e)
{
    if (DgvApplications.SelectedRows.Count == 0)
    {
        MessageBox.Show("请选择要删除的记录", "提示");
        return;
    }

    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    // 检查状态：只有待审批(1)或已拒绝(3)才能删除
    if (application.Status != 1 && application.Status != 3)
    {
        MessageBox.Show("只有待审批或已拒绝的记录才能删除，如需删除请先反审", "提示");
        return;
    }
    
    // ... 原有删除逻辑
}
```

#### 1.2 修改 ApplicationEditForm.cs

* 在保存时，如果被拒绝的单据重新提交，将状态重置为"待审批"(1)

```csharp
// 保存按钮点击事件中的编辑逻辑
if (_isEdit && _application != null)
{
    application.ApplicationId = _application.ApplicationId;
    application.ApplicationNo = _application.ApplicationNo;
    application.OrderId = _application.OrderId;
    application.ApplicantId = _application.ApplicantId;
    application.ProcessorId = _application.ProcessorId;
    
    // 如果被拒绝的单据重新提交，重置为待审批状态
    if (_application.Status == 3) // 已拒绝
    {
        application.Status = 1; // 重置为待审批
    }
    else
    {
        application.Status = _application.Status;
    }
    
    // ... 原有更新逻辑
}
```

***

### 2. 查看审批/拒绝理由

**目标**：在申请管理中能够查看审批历史记录和拒绝理由

**实现步骤**：

#### 2.1 修改 ExternalProcessingApplication 模型

添加一个属性用于显示最新的审批意见：

```csharp
public class ExternalProcessingApplication
{
    // ... 原有属性
    
    // 用于显示最新审批/拒绝理由（不映射到数据库）
    public string? LatestAuditRemark { get; set; }
}
```

#### 2.2 修改 ExternalProcessingService.cs

在 `GetAllApplications` 和 `GetApplicationById` 方法中，加载最新的审批记录：

```csharp
public List<ExternalProcessingApplication> GetAllApplications(int? status = null)
{
    var applications = new List<ExternalProcessingApplication>();
    // ... 原有查询逻辑
    
    // 加载每个申请的最新审批意见
    foreach (var app in applications)
    {
        app.LatestAuditRemark = GetLatestAuditRemark(app.ApplicationId);
    }
    
    return applications;
}

private string? GetLatestAuditRemark(int applicationId)
{
    var sql = @"SELECT TOP 1 AuditRemark FROM ExternalProcessingAudits 
                 WHERE ApplicationId = @ApplicationId 
                 ORDER BY AuditDate DESC";
    // ... 执行查询并返回结果
}
```

#### 2.3 修改 ApplicationForm.cs

在数据网格中添加"审批意见"列：

```csharp
// 在 LoadApplications 方法中
if (DgvApplications.Columns.Count > 0)
{
    // ... 原有列设置
    
    // 添加审批意见列
    if (DgvApplications.Columns.Contains("LatestAuditRemark"))
    {
        DgvApplications.Columns["LatestAuditRemark"].HeaderText = "审批意见";
        DgvApplications.Columns["LatestAuditRemark"].DisplayIndex = 8; // 调整显示位置
    }
}
```

***

### 3. 各层级反审功能

**目标**：在每个管理环节添加"反审"按钮，允许将单据退回上一级状态

**状态流转规则**：

* 已审批(2) → 反审 → 待审批(1)

* 已外发(4) → 反审 → 已审批(2)

* 已验收(5) → 反审 → 已外发(4)

* 已对账(6) → 反审 → 已验收(5)

* 已财务审核(7) → 反审 → 已对账(6)

**实现步骤**：

#### 3.1 修改 AuditForm.cs（审批管理）

添加"反审"按钮，将已审批的单据退回待审批：

```csharp
// 添加反审按钮
this.BtnUnAudit = new Button();
this.BtnUnAudit.Text = "反审";
this.BtnUnAudit.Click += new EventHandler(this.BtnUnAudit_Click);

// 反审按钮点击事件
private void BtnUnAudit_Click(object sender, EventArgs e)
{
    if (DgvApplications.SelectedRows.Count == 0)
    {
        MessageBox.Show("请选择要反审的记录", "提示");
        return;
    }

    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    // 只有已审批(2)的单据才能反审
    if (application.Status != 2)
    {
        MessageBox.Show("只有已审批的记录才能反审", "提示");
        return;
    }
    
    if (MessageBox.Show("确定要反审选中的记录吗？反审后将回到待审批状态。", "确认", 
        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
    {
        try
        {
            // 删除审批记录
            _auditService.DeleteAuditByApplicationId(application.ApplicationId);
            
            // 更新状态为待审批
            if (_auditService.UpdateApplicationStatus(application.ApplicationId, 1))
            {
                MessageBox.Show("反审成功", "提示");
                LoadApplications();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("反审失败：" + ex.Message, "错误");
        }
    }
}
```

#### 3.2 修改 ExternalProcessingAuditService.cs

添加删除审批记录的方法：

```csharp
public bool DeleteAuditByApplicationId(int applicationId)
{
    var sql = "DELETE FROM ExternalProcessingAudits WHERE ApplicationId = @ApplicationId";
    // ... 执行删除
}
```

#### 3.3 修改 AcceptanceForm.cs（验收管理）

添加"反审"按钮，将已验收的单据退回已外发：

```csharp
// 添加反审按钮
this.BtnUnAccept = new Button();
this.BtnUnAccept.Text = "反审";
this.BtnUnAccept.Click += new EventHandler(this.BtnUnAccept_Click);

// 修改 LoadApplications 方法，加载状态为 4(已外发) 和 5(已验收) 的单据
_applications = _service.GetAllApplications(); // 加载所有状态
// 然后筛选出 4 和 5 的

// 反审按钮点击事件
private void BtnUnAccept_Click(object sender, EventArgs e)
{
    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    if (application.Status != 5) // 已验收
    {
        MessageBox.Show("只有已验收的记录才能反审", "提示");
        return;
    }
    
    // 删除验收记录，更新状态为已外发(4)
    _acceptanceService.DeleteAcceptanceByApplicationId(application.ApplicationId);
    _acceptanceService.UpdateApplicationStatus(application.ApplicationId, 4);
}
```

#### 3.4 类似地修改 ReconciliationForm.cs 和 FinanceAuditForm.cs

添加相应的反审功能。

***

### 4. 修复验收管理数据加载问题

**问题**：AcceptanceForm 只加载状态为 4(已外发) 的单据，但用户希望看到已审批(2)的单据也可以进行外发和验收

**解决方案**：

#### 4.1 修改 AcceptanceForm.cs

* 修改 `LoadApplications` 方法，加载状态为 2(已审批)、4(已外发)、5(已验收) 的单据

* 添加"外发"按钮，用于将已审批的单据标记为已外发

```csharp
private void LoadApplications(string searchText = "")
{
    try
    {
        // 加载已审批(2)、已外发(4)、已验收(5)的单据
        var allApplications = _service.GetAllApplications();
        _applications = allApplications.FindAll(a => a.Status == 2 || a.Status == 4 || a.Status == 5);
        
        // ... 原有逻辑
    }
}

// 添加外发按钮
this.BtnDispatch = new Button();
this.BtnDispatch.Text = "外发";
this.BtnDispatch.Click += new EventHandler(this.BtnDispatch_Click);

// 外发按钮点击事件
private void BtnDispatch_Click(object sender, EventArgs e)
{
    var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
    
    if (application.Status != 2) // 已审批
    {
        MessageBox.Show("只有已审批的记录才能进行外发", "提示");
        return;
    }
    
    // 更新状态为已外发(4)
    if (_service.UpdateApplicationStatus(application.ApplicationId, 4))
    {
        MessageBox.Show("外发成功", "提示");
        LoadApplications();
    }
}
```

***

## 数据库表结构确认

当前数据库表结构已支持以上功能，无需修改：

* **ExternalProcessingApplications**：主表，Status 字段控制流程状态

* **ExternalProcessingAudits**：审批记录表，存储审批意见

* **ExternalProcessingAcceptances**：验收记录表

* **ExternalProcessingReconciliations**：对账记录表

* **ExternalProcessingFinanceAudits**：财务审核记录表

***

## 实施顺序

1. **第一步**：修改 ApplicationForm.cs，添加状态控制（编辑/删除权限）
2. **第二步**：修改 ApplicationEditForm.cs，支持被拒绝单据重新提交
3. **第三步**：修改 ExternalProcessingService.cs 和 ApplicationForm.cs，显示审批意见
4. **第四步**：修改 AcceptanceForm.cs，修复数据加载问题，添加外发按钮
5. **第五步**：在各管理窗体添加反审功能（AuditForm、AcceptanceForm、ReconciliationForm、FinanceAuditForm）

***

## 预期效果

1. 用户只能编辑/删除待审批或已拒绝的单据
2. 用户可以在申请管理中查看审批意见和拒绝理由
3. 被拒绝的单据修改后可以重新提交，状态正确重置
4. 已审批的单据可以在验收管理中看到，并可以进行外发操作
5. 各层级都可以进行反审操作，将单据退回上一级

