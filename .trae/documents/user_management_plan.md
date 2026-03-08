# 用户管理功能开发计划

## 需求分析

1. 在主界面添加"用户管理"按钮
2. 创建用户管理窗体，用于管理员管理其他用户
3. 功能包括：新增用户、编辑用户、删除用户、重置密码
4. 权限控制：只有特定权限的用户才能访问用户管理功能

## 任务清单

### 任务1: 添加用户管理权限常量

**文件**: [PermissionKeys](file:///h:/TraeDev/ExternalProcessing/ExternalProcessing/Models/User.cs)

* 在 PermissionKeys 类中添加 `UserManagement = "用户管理"` 常量

### 任务2: 修改主窗体添加用户管理按钮

**文件**: [MainForm.Designer.cs](file:///h:/TraeDev/ExternalProcessing/ExternalProcessing/Forms/MainForm.Designer.cs)

* 添加 BtnUserManagement 按钮（位置放在统计报表下方或调整布局）

* 按钮样式与其他按钮保持一致

* 添加按钮点击事件

**文件**: [MainForm.cs](file:///h:/TraeDev/ExternalProcessing/ExternalProcessing/Forms/MainForm.cs)

* 在 ApplyPermissions 方法中添加用户管理按钮的权限控制

* 实现 BtnUserManagement\_Click 方法打开用户管理窗体

### 任务3: 创建用户服务类

**文件**: 新建 `Services/UserService.cs`

* GetAllUsers() - 获取所有用户列表

* GetUserById(int userId) - 根据ID获取用户

* CreateUser(User user) - 创建新用户

* UpdateUser(User user) - 更新用户信息

* DeleteUser(int userId) - 删除用户

* ResetPassword(int userId, string newPassword) - 重置密码

* IsUsernameExists(string username) - 检查用户名是否已存在

### 任务4: 创建用户管理窗体

**文件**: 新建 `Forms/UserManagementForm.cs`

* 界面布局：

  * 顶部：搜索框 + 搜索按钮 + 重置按钮

  * 中间：DataGridView 显示用户列表（用户ID、用户名、权限、状态、创建时间）

  * 右侧：功能按钮（新增、编辑、删除、重置密码）

* 功能实现：

  * 加载用户列表

  * 搜索功能（按用户名搜索）

  * 整行选中模式

  * 列名中文化

### 任务5: 创建用户编辑窗体

**文件**: 新建 `Forms/UserEditForm.cs`

* 用于新增和编辑用户

* 界面元素：

  * 用户名（新增时 editable，编辑时 readonly）

  * 密码（新增时必填，编辑时可选）

  * 权限选择（CheckBox 列表：外发申请、外发审批、外发验收、外发对账、财务审核、统计报表、用户管理）

  * 状态（启用/禁用）

  * 保存/取消按钮

* 验证逻辑：

  * 用户名不能为空

  * 用户名不能重复

  * 新增时密码不能为空

### 任务6: 初始化管理员权限

**文件**: [Database/Init.sql](file:///h:/TraeDev/ExternalProcessing/Database/Init.sql)

* 修改初始化脚本，给 admin 用户添加"用户管理"权限

## 实施步骤

1. 任务1: 添加权限常量
2. 任务3: 创建用户服务类
3. 任务5: 创建用户编辑窗体
4. 任务4: 创建用户管理窗体
5. 任务2: 修改主窗体
6. 任务6: 更新数据库初始化脚本
7. 编译验证

## 界面设计参考

### 主窗体布局调整

当前是 2行3列 的按钮布局，可以调整为：

* 第一行：申请管理、审批管理、验收管理

* 第二行：对账管理、财务审核、统计报表

* 第三行：用户管理（居中显示，或放在合适位置）

### 用户管理窗体布局

参考 ApplicationForm 的布局风格：

* 搜索区域在顶部

* 数据网格在中间

* 操作按钮在右侧

### 用户编辑窗体布局

参考 ApplicationEditForm 的布局风格：

* 表单字段垂直排列

* 保存/取消按钮在底部

