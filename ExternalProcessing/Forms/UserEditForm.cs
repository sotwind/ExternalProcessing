using System;
using System.Windows.Forms;
using ExternalProcessing.Models;
using ExternalProcessing.Services;

namespace ExternalProcessing.Forms;

public partial class UserEditForm : Form
{
    private readonly UserService _userService = new();
    private User? _user;
    private bool _isEdit = false;

    public UserEditForm()
    {
        InitializeComponent();
        _isEdit = false;
        this.Text = "新增用户";
    }

    public UserEditForm(User user)
    {
        InitializeComponent();
        _user = user;
        _isEdit = true;
        this.Text = "编辑用户";
        LoadData();
    }

    private void InitializeComponent()
    {
        this.LblUsername = new Label();
        this.TxtUsername = new TextBox();
        this.LblPassword = new Label();
        this.TxtPassword = new TextBox();
        this.LblPermissions = new Label();
        this.ChkApplication = new CheckBox();
        this.ChkAudit = new CheckBox();
        this.ChkAcceptance = new CheckBox();
        this.ChkReconciliation = new CheckBox();
        this.ChkFinanceAudit = new CheckBox();
        this.ChkReport = new CheckBox();
        this.ChkUserManagement = new CheckBox();
        this.LblStatus = new Label();
        this.CboStatus = new ComboBox();
        this.BtnSave = new Button();
        this.BtnCancel = new Button();
        this.SuspendLayout();

        // LblUsername
        this.LblUsername.AutoSize = true;
        this.LblUsername.Location = new System.Drawing.Point(30, 20);
        this.LblUsername.Name = "LblUsername";
        this.LblUsername.Size = new System.Drawing.Size(70, 17);
        this.LblUsername.Text = "用户名：";

        // TxtUsername
        this.TxtUsername.Location = new System.Drawing.Point(110, 17);
        this.TxtUsername.Name = "TxtUsername";
        this.TxtUsername.Size = new System.Drawing.Size(250, 23);

        // LblPassword
        this.LblPassword.AutoSize = true;
        this.LblPassword.Location = new System.Drawing.Point(30, 50);
        this.LblPassword.Name = "LblPassword";
        this.LblPassword.Size = new System.Drawing.Size(200, 17);
        this.LblPassword.Text = "密码：";

        // TxtPassword
        this.TxtPassword.Location = new System.Drawing.Point(110, 47);
        this.TxtPassword.Name = "TxtPassword";
        this.TxtPassword.Size = new System.Drawing.Size(250, 23);
        this.TxtPassword.UseSystemPasswordChar = true;

        // LblPermissions
        this.LblPermissions.AutoSize = true;
        this.LblPermissions.Location = new System.Drawing.Point(30, 85);
        this.LblPermissions.Name = "LblPermissions";
        this.LblPermissions.Size = new System.Drawing.Size(70, 17);
        this.LblPermissions.Text = "权限：";

        // ChkApplication
        this.ChkApplication.AutoSize = true;
        this.ChkApplication.Location = new System.Drawing.Point(110, 85);
        this.ChkApplication.Name = "ChkApplication";
        this.ChkApplication.Size = new System.Drawing.Size(90, 21);
        this.ChkApplication.Text = "申请管理";

        // ChkAudit
        this.ChkAudit.AutoSize = true;
        this.ChkAudit.Location = new System.Drawing.Point(210, 85);
        this.ChkAudit.Name = "ChkAudit";
        this.ChkAudit.Size = new System.Drawing.Size(90, 21);
        this.ChkAudit.Text = "审批管理";

        // ChkAcceptance
        this.ChkAcceptance.AutoSize = true;
        this.ChkAcceptance.Location = new System.Drawing.Point(110, 110);
        this.ChkAcceptance.Name = "ChkAcceptance";
        this.ChkAcceptance.Size = new System.Drawing.Size(90, 21);
        this.ChkAcceptance.Text = "验收管理";

        // ChkReconciliation
        this.ChkReconciliation.AutoSize = true;
        this.ChkReconciliation.Location = new System.Drawing.Point(210, 110);
        this.ChkReconciliation.Name = "ChkReconciliation";
        this.ChkReconciliation.Size = new System.Drawing.Size(90, 21);
        this.ChkReconciliation.Text = "对账管理";

        // ChkFinanceAudit
        this.ChkFinanceAudit.AutoSize = true;
        this.ChkFinanceAudit.Location = new System.Drawing.Point(110, 135);
        this.ChkFinanceAudit.Name = "ChkFinanceAudit";
        this.ChkFinanceAudit.Size = new System.Drawing.Size(90, 21);
        this.ChkFinanceAudit.Text = "财务审核";

        // ChkReport
        this.ChkReport.AutoSize = true;
        this.ChkReport.Location = new System.Drawing.Point(210, 135);
        this.ChkReport.Name = "ChkReport";
        this.ChkReport.Size = new System.Drawing.Size(90, 21);
        this.ChkReport.Text = "统计报表";

        // ChkUserManagement
        this.ChkUserManagement.AutoSize = true;
        this.ChkUserManagement.Location = new System.Drawing.Point(110, 160);
        this.ChkUserManagement.Name = "ChkUserManagement";
        this.ChkUserManagement.Size = new System.Drawing.Size(90, 21);
        this.ChkUserManagement.Text = "用户管理";

        // LblStatus
        this.LblStatus.AutoSize = true;
        this.LblStatus.Location = new System.Drawing.Point(30, 195);
        this.LblStatus.Name = "LblStatus";
        this.LblStatus.Size = new System.Drawing.Size(70, 17);
        this.LblStatus.Text = "状态：";

        // CboStatus
        this.CboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboStatus.FormattingEnabled = true;
        this.CboStatus.Location = new System.Drawing.Point(110, 192);
        this.CboStatus.Name = "CboStatus";
        this.CboStatus.Size = new System.Drawing.Size(250, 23);

        // BtnSave
        this.BtnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        this.BtnSave.ForeColor = System.Drawing.Color.White;
        this.BtnSave.Location = new System.Drawing.Point(80, 240);
        this.BtnSave.Name = "BtnSave";
        this.BtnSave.Size = new System.Drawing.Size(100, 35);
        this.BtnSave.Text = "保存";
        this.BtnSave.UseVisualStyleBackColor = false;
        this.BtnSave.Click += new EventHandler(this.BtnSave_Click);

        // BtnCancel
        this.BtnCancel.Location = new System.Drawing.Point(210, 240);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(100, 35);
        this.BtnCancel.Text = "取消";
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // UserEditForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 310);
        this.Controls.Add(this.LblUsername);
        this.Controls.Add(this.TxtUsername);
        this.Controls.Add(this.LblPassword);
        this.Controls.Add(this.TxtPassword);
        this.Controls.Add(this.LblPermissions);
        this.Controls.Add(this.ChkApplication);
        this.Controls.Add(this.ChkAudit);
        this.Controls.Add(this.ChkAcceptance);
        this.Controls.Add(this.ChkReconciliation);
        this.Controls.Add(this.ChkFinanceAudit);
        this.Controls.Add(this.ChkReport);
        this.Controls.Add(this.ChkUserManagement);
        this.Controls.Add(this.LblStatus);
        this.Controls.Add(this.CboStatus);
        this.Controls.Add(this.BtnSave);
        this.Controls.Add(this.BtnCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "UserEditForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "用户编辑";
        this.ResumeLayout(false);
        this.PerformLayout();

        LoadStatusOptions();
    }

    private Label LblUsername = null!;
    private TextBox TxtUsername = null!;
    private Label LblPassword = null!;
    private TextBox TxtPassword = null!;
    private Label LblPermissions = null!;
    private CheckBox ChkApplication = null!;
    private CheckBox ChkAudit = null!;
    private CheckBox ChkAcceptance = null!;
    private CheckBox ChkReconciliation = null!;
    private CheckBox ChkFinanceAudit = null!;
    private CheckBox ChkReport = null!;
    private CheckBox ChkUserManagement = null!;
    private Label LblStatus = null!;
    private ComboBox CboStatus = null!;
    private Button BtnSave = null!;
    private Button BtnCancel = null!;

    private void LoadStatusOptions()
    {
        CboStatus.Items.Clear();
        CboStatus.Items.Add(new ComboBoxItem("启用", true));
        CboStatus.Items.Add(new ComboBoxItem("禁用", false));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void LoadData()
    {
        if (_user != null)
        {
            TxtUsername.Text = _user.Username;
            TxtUsername.ReadOnly = true;
            TxtPassword.Text = ""; // 编辑时不显示密码
            LblPassword.Text = "密码（留空不修改）：";

            // 加载权限
            var permissions = _user.GetPermissionList();
            ChkApplication.Checked = permissions.Contains(PermissionKeys.Application);
            ChkAudit.Checked = permissions.Contains(PermissionKeys.Audit);
            ChkAcceptance.Checked = permissions.Contains(PermissionKeys.Acceptance);
            ChkReconciliation.Checked = permissions.Contains(PermissionKeys.Reconciliation);
            ChkFinanceAudit.Checked = permissions.Contains(PermissionKeys.FinanceAudit);
            ChkReport.Checked = permissions.Contains(PermissionKeys.Report);
            ChkUserManagement.Checked = permissions.Contains(PermissionKeys.UserManagement);

            // 加载状态
            CboStatus.SelectedIndex = _user.IsActive ? 0 : 1;
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        // 验证
        if (string.IsNullOrWhiteSpace(TxtUsername.Text))
        {
            MessageBox.Show("请输入用户名", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtUsername.Focus();
            return;
        }

        if (!_isEdit && string.IsNullOrWhiteSpace(TxtPassword.Text))
        {
            MessageBox.Show("请输入密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtPassword.Focus();
            return;
        }

        try
        {
            // 收集权限
            var permissions = new List<string>();
            if (ChkApplication.Checked) permissions.Add(PermissionKeys.Application);
            if (ChkAudit.Checked) permissions.Add(PermissionKeys.Audit);
            if (ChkAcceptance.Checked) permissions.Add(PermissionKeys.Acceptance);
            if (ChkReconciliation.Checked) permissions.Add(PermissionKeys.Reconciliation);
            if (ChkFinanceAudit.Checked) permissions.Add(PermissionKeys.FinanceAudit);
            if (ChkReport.Checked) permissions.Add(PermissionKeys.Report);
            if (ChkUserManagement.Checked) permissions.Add(PermissionKeys.UserManagement);

            var selectedStatus = CboStatus.SelectedItem as ComboBoxItem;
            var isActive = selectedStatus?.Value as bool? ?? true;

            if (_isEdit && _user != null)
            {
                // 编辑用户
                _user.Permissions = string.Join(",", permissions);
                _user.IsActive = isActive;

                if (_userService.UpdateUser(_user))
                {
                    // 如果输入了新密码，则更新密码
                    if (!string.IsNullOrWhiteSpace(TxtPassword.Text))
                    {
                        _userService.UpdatePassword(_user.UserID, TxtPassword.Text.Trim());
                    }

                    MessageBox.Show("更新成功", "提示");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("更新失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // 新增用户
                // 检查用户名是否已存在
                if (_userService.IsUsernameExists(TxtUsername.Text.Trim()))
                {
                    MessageBox.Show("用户名已存在", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    TxtUsername.Focus();
                    return;
                }

                var newUser = new User
                {
                    Username = TxtUsername.Text.Trim(),
                    Password = TxtPassword.Text.Trim(),
                    Permissions = string.Join(",", permissions),
                    IsActive = isActive
                };

                var userId = _userService.CreateUser(newUser);
                if (userId > 0)
                {
                    MessageBox.Show("创建成功", "提示");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("创建失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("保存失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private class ComboBoxItem
    {
        public string Text { get; }
        public object? Value { get; }

        public ComboBoxItem(string text, object? value)
        {
            Text = text;
            Value = value;
        }
    }
}
