# 五个窗体统一优化计划

## 目标窗体
1. AuditForm (审批管理)
2. AcceptanceForm (验收管理)
3. ReconciliationForm (对账管理)
4. FinanceAuditForm (财务审核)
5. ReportForm (统计报表)

## 任务清单

### 任务1: 刷新按钮改造
将所有窗体的"刷新"按钮：
- 位置：从右侧按钮组移动到搜索按钮后面
- 名称：从"刷新"改为"重置"
- 功能：点击时清空搜索条件（状态下拉框重置为"全部"、搜索文本框清空），然后重新加载数据

**涉及文件：**
- AuditForm.cs
- AcceptanceForm.cs
- ReconciliationForm.cs
- FinanceAuditForm.cs
- ReportForm.cs

### 任务2: 数据网格选择模式
为所有窗体的 DataGridView 添加：
- `SelectionMode = DataGridViewSelectionMode.FullRowSelect` - 整行选中
- `MultiSelect = false` - 禁止多选

**涉及文件：**
- AuditForm.cs
- AcceptanceForm.cs
- ReconciliationForm.cs
- FinanceAuditForm.cs
- ReportForm.cs

### 任务3: 功能按钮开发
为各窗体的功能按钮添加实际功能（目前都是"开发中"提示）：

#### AuditForm (审批管理)
- 审批按钮：实现审批功能，更新申请状态

#### AcceptanceForm (验收管理)
- 验收按钮：实现验收功能，更新申请状态

#### ReconciliationForm (对账管理)
- 对账按钮：实现对账功能，更新申请状态

#### FinanceAuditForm (财务审核)
- 审核按钮：实现财务审核功能，更新申请状态

#### ReportForm (统计报表)
- 导出按钮：实现数据导出功能（如Excel导出）

**涉及文件：**
- AuditForm.cs
- AcceptanceForm.cs
- ReconciliationForm.cs
- FinanceAuditForm.cs
- ReportForm.cs

### 任务4: 数据网格列名中文化
检查所有窗体的 DataGridView 列名，将英文列名改为中文：

需要处理的列名（根据各窗体实际情况）：
- ApplicationId → 申请ID（或隐藏）
- ApplicationNo → 申请编号
- OrderId → 订单ID（或隐藏）
- OrderNo → 订单编号
- ApplicantId → 申请人ID（或隐藏）
- ApplicantName → 申请人
- ApplicationDate → 申请日期
- ProcessorId → 加工商ID（或隐藏）
- ProcessorName → 加工商
- ProcessingContent → 加工内容
- ExpectedReturnDate → 预计归还日期
- Status → 状态（或隐藏）
- StatusText → 状态
- Remark → 备注
- OperatorId → 操作人ID（或隐藏）
- OperatorTime → 操作时间

**涉及文件：**
- AuditForm.cs
- AcceptanceForm.cs
- ReconciliationForm.cs
- FinanceAuditForm.cs
- ReportForm.cs

## 实施步骤

1. 先完成任务1（刷新按钮改造）- 5个窗体
2. 再完成任务2（数据网格选择模式）- 5个窗体
3. 然后完成任务4（列名中文化）- 5个窗体
4. 最后完成任务3（功能按钮开发）- 5个窗体

## 注意事项
- 修改前先查看各窗体现有代码结构
- 保持代码风格一致
- 确保编译通过
- 各功能按钮需要调用 Service 层更新状态
