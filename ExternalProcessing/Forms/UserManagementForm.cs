using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Models;
using ExternalProcessing.Services;

namespace ExternalProcessing.Forms;

public partial class UserManagementForm : Form
{
    private readonly UserService _userService = new();
    private List<User> _users = new();

    public UserManagementForm()
    {
        InitializeComponent();
        LoadUsers();
    }

    private void InitializeComponent()
    {
        this.DgvUsers = new DataGridView();
        this.BtnAdd = new Button();
        this.BtnEdit = new Button();
        this.BtnDelete = new Button();
        this.BtnResetPassword = new Button();
        this.TxtSearch = new TextBox();
        this.BtnSearch = new Button();
        this.BtnRefresh = new Button();
        this.Label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvUsers)).BeginInit();
        this.SuspendLayout();

        // Label1
        this.Label1.Location = new System.Drawing.Point(30, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 0;
        this.Label1.Text = "搜索：";

        // TxtSearch
        this.TxtSearch.Location = new System.Drawing.Point(90, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(233, 23);
        this.TxtSearch.TabIndex = 1;

        // BtnSearch
        this.BtnSearch.Location = new System.Drawing.Point(340, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(93, 30);
        this.BtnSearch.TabIndex = 2;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        // BtnRefresh
        this.BtnRefresh.Location = new System.Drawing.Point(440, 23);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(93, 30);
        this.BtnRefresh.TabIndex = 3;
        this.BtnRefresh.Text = "重置";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        // DgvUsers
        this.DgvUsers.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvUsers.Location = new System.Drawing.Point(30, 70);
        this.DgvUsers.Name = "DgvUsers";
        this.DgvUsers.Size = new System.Drawing.Size(940, 480);
        this.DgvUsers.TabIndex = 4;
        this.DgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.DgvUsers.MultiSelect = false;
        this.DgvUsers.ReadOnly = true;

        // BtnAdd
        this.BtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnAdd.BackColor = System.Drawing.Color.FromArgb(144, 238, 144);
        this.BtnAdd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnAdd.Location = new System.Drawing.Point(990, 70);
        this.BtnAdd.Name = "BtnAdd";
        this.BtnAdd.Size = new System.Drawing.Size(100, 35);
        this.BtnAdd.TabIndex = 5;
        this.BtnAdd.Text = "新增";
        this.BtnAdd.UseVisualStyleBackColor = false;
        this.BtnAdd.Click += new EventHandler(this.BtnAdd_Click);

        // BtnEdit
        this.BtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnEdit.BackColor = System.Drawing.Color.FromArgb(255, 255, 153);
        this.BtnEdit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnEdit.Location = new System.Drawing.Point(990, 120);
        this.BtnEdit.Name = "BtnEdit";
        this.BtnEdit.Size = new System.Drawing.Size(100, 35);
        this.BtnEdit.TabIndex = 6;
        this.BtnEdit.Text = "编辑";
        this.BtnEdit.UseVisualStyleBackColor = false;
        this.BtnEdit.Click += new EventHandler(this.BtnEdit_Click);

        // BtnDelete
        this.BtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnDelete.Location = new System.Drawing.Point(990, 170);
        this.BtnDelete.Name = "BtnDelete";
        this.BtnDelete.Size = new System.Drawing.Size(100, 35);
        this.BtnDelete.TabIndex = 7;
        this.BtnDelete.Text = "删除";
        this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);

        // BtnResetPassword
        this.BtnResetPassword.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnResetPassword.Location = new System.Drawing.Point(990, 220);
        this.BtnResetPassword.Name = "BtnResetPassword";
        this.BtnResetPassword.Size = new System.Drawing.Size(100, 35);
        this.BtnResetPassword.TabIndex = 8;
        this.BtnResetPassword.Text = "重置密码";
        this.BtnResetPassword.Click += new EventHandler(this.BtnResetPassword_Click);

        // UserManagementForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.BtnRefresh);
        this.Controls.Add(this.DgvUsers);
        this.Controls.Add(this.BtnAdd);
        this.Controls.Add(this.BtnEdit);
        this.Controls.Add(this.BtnDelete);
        this.Controls.Add(this.BtnResetPassword);
        this.Name = "UserManagementForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "用户管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvUsers)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvUsers = null!;
    private Button BtnAdd = null!;
    private Button BtnEdit = null!;
    private Button BtnDelete = null!;
    private Button BtnResetPassword = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Button BtnRefresh = null!;
    private Label Label1 = null!;

    private void LoadUsers(string searchText = "")
    {
        try
        {
            _users = _userService.GetAllUsers();

            if (!string.IsNullOrEmpty(searchText))
            {
                _users = _users.FindAll(u =>
                    u.Username.Contains(searchText, StringComparison.OrdinalIgnoreCase));
            }

            DgvUsers.DataSource = null;
            DgvUsers.DataSource = _users;

            if (DgvUsers.Columns.Count > 0)
            {
                // 隐藏密码列
                DgvUsers.Columns["Password"].Visible = false;

                // 设置列标题
                DgvUsers.Columns["UserID"].HeaderText = "用户ID";
                DgvUsers.Columns["Username"].HeaderText = "用户名";
                DgvUsers.Columns["Permissions"].HeaderText = "权限";
                DgvUsers.Columns["IsActive"].HeaderText = "状态";
                DgvUsers.Columns["CreateTime"].HeaderText = "创建时间";

                DgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("加载数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        using var form = new UserEditForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadUsers();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (DgvUsers.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要编辑的用户", "提示");
            return;
        }

        var user = (User)DgvUsers.SelectedRows[0].DataBoundItem;
        using var form = new UserEditForm(user);
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadUsers();
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (DgvUsers.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要删除的用户", "提示");
            return;
        }

        var user = (User)DgvUsers.SelectedRows[0].DataBoundItem;

        // 不允许删除自己
        if (user.Username == "admin")
        {
            MessageBox.Show("不能删除管理员账号", "提示");
            return;
        }

        if (MessageBox.Show($"确定要删除用户 [{user.Username}] 吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                if (_userService.DeleteUser(user.UserID))
                {
                    MessageBox.Show("删除成功", "提示");
                    LoadUsers();
                }
                else
                {
                    MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnResetPassword_Click(object sender, EventArgs e)
    {
        if (DgvUsers.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要重置密码的用户", "提示");
            return;
        }

        var user = (User)DgvUsers.SelectedRows[0].DataBoundItem;

        // 弹出输入框让用户输入新密码
        using var inputForm = new PasswordResetForm(user.Username);
        if (inputForm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                if (_userService.ResetPassword(user.UserID, inputForm.NewPassword))
                {
                    MessageBox.Show("密码重置成功", "提示");
                }
                else
                {
                    MessageBox.Show("密码重置失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("密码重置失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        var searchText = TxtSearch.Text.Trim();
        LoadUsers(searchText);
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        TxtSearch.Text = "";
        LoadUsers();
    }
}

/// <summary>
/// 密码重置输入窗体
/// </summary>
public partial class PasswordResetForm : Form
{
    public string NewPassword { get; private set; } = "";

    public PasswordResetForm(string username)
    {
        InitializeComponent(username);
    }

    private void InitializeComponent(string username)
    {
        this.LblMessage = new Label();
        this.TxtPassword = new TextBox();
        this.TxtConfirmPassword = new TextBox();
        this.BtnOK = new Button();
        this.BtnCancel = new Button();
        this.SuspendLayout();

        // LblMessage
        this.LblMessage.AutoSize = true;
        this.LblMessage.Location = new System.Drawing.Point(30, 20);
        this.LblMessage.Name = "LblMessage";
        this.LblMessage.Size = new System.Drawing.Size(300, 17);
        this.LblMessage.Text = $"正在重置用户 [{username}] 的密码";

        // TxtPassword
        this.TxtPassword.Location = new System.Drawing.Point(30, 50);
        this.TxtPassword.Name = "TxtPassword";
        this.TxtPassword.Size = new System.Drawing.Size(250, 23);
        this.TxtPassword.UseSystemPasswordChar = true;
        this.TxtPassword.PlaceholderText = "请输入新密码";

        // TxtConfirmPassword
        this.TxtConfirmPassword.Location = new System.Drawing.Point(30, 80);
        this.TxtConfirmPassword.Name = "TxtConfirmPassword";
        this.TxtConfirmPassword.Size = new System.Drawing.Size(250, 23);
        this.TxtConfirmPassword.UseSystemPasswordChar = true;
        this.TxtConfirmPassword.PlaceholderText = "请确认新密码";

        // BtnOK
        this.BtnOK.Location = new System.Drawing.Point(60, 120);
        this.BtnOK.Name = "BtnOK";
        this.BtnOK.Size = new System.Drawing.Size(80, 30);
        this.BtnOK.Text = "确定";
        this.BtnOK.Click += new EventHandler(this.BtnOK_Click);

        // BtnCancel
        this.BtnCancel.Location = new System.Drawing.Point(170, 120);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(80, 30);
        this.BtnCancel.Text = "取消";
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // PasswordResetForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(320, 170);
        this.Controls.Add(this.LblMessage);
        this.Controls.Add(this.TxtPassword);
        this.Controls.Add(this.TxtConfirmPassword);
        this.Controls.Add(this.BtnOK);
        this.Controls.Add(this.BtnCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "PasswordResetForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "重置密码";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Label LblMessage = null!;
    private TextBox TxtPassword = null!;
    private TextBox TxtConfirmPassword = null!;
    private Button BtnOK = null!;
    private Button BtnCancel = null!;

    private void BtnOK_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtPassword.Text))
        {
            MessageBox.Show("请输入新密码", "提示");
            TxtPassword.Focus();
            return;
        }

        if (TxtPassword.Text != TxtConfirmPassword.Text)
        {
            MessageBox.Show("两次输入的密码不一致", "提示");
            TxtConfirmPassword.Focus();
            return;
        }

        NewPassword = TxtPassword.Text.Trim();
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }
}
