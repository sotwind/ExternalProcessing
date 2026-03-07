namespace ExternalProcessing.Forms;

partial class LoginForm
{
    private System.ComponentModel.IContainer components = null;
    private TextBox TxtUsername = null!;
    private TextBox TxtPassword = null!;
    private Button BtnLogin = null!;
    private Button BtnCancel = null!;
    private Label LblUsername = null!;
    private Label LblPassword = null!;
    private Label LblTitle = null!;
    private Panel PnlMain = null!;
    private CheckBox ChkRememberPassword = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        this.PnlMain = new Panel();
        this.LblTitle = new Label();
        this.LblUsername = new Label();
        this.TxtUsername = new TextBox();
        this.LblPassword = new Label();
        this.TxtPassword = new TextBox();
        this.ChkRememberPassword = new CheckBox();
        this.BtnLogin = new Button();
        this.BtnCancel = new Button();
        this.PnlMain.SuspendLayout();
        this.SuspendLayout();

        // PnlMain
        this.PnlMain.BackColor = Color.White;
        this.PnlMain.Controls.Add(this.LblTitle);
        this.PnlMain.Controls.Add(this.LblUsername);
        this.PnlMain.Controls.Add(this.TxtUsername);
        this.PnlMain.Controls.Add(this.LblPassword);
        this.PnlMain.Controls.Add(this.TxtPassword);
        this.PnlMain.Controls.Add(this.ChkRememberPassword);
        this.PnlMain.Controls.Add(this.BtnLogin);
        this.PnlMain.Controls.Add(this.BtnCancel);
        this.PnlMain.Dock = DockStyle.Fill;
        this.PnlMain.Location = new Point(0, 0);
        this.PnlMain.Name = "PnlMain";
        this.PnlMain.Size = new Size(400, 330);
        this.PnlMain.TabIndex = 0;

        // LblTitle
        this.LblTitle.Font = new Font("Microsoft YaHei", 16F, FontStyle.Bold);
        this.LblTitle.ForeColor = Color.FromArgb(64, 64, 64);
        this.LblTitle.Location = new Point(0, 30);
        this.LblTitle.Name = "LblTitle";
        this.LblTitle.Size = new Size(400, 40);
        this.LblTitle.TabIndex = 0;
        this.LblTitle.Text = "外发加工管理系统";
        this.LblTitle.TextAlign = ContentAlignment.MiddleCenter;

        // LblUsername
        this.LblUsername.AutoSize = true;
        this.LblUsername.Location = new Point(50, 90);
        this.LblUsername.Name = "LblUsername";
        this.LblUsername.Size = new Size(56, 20);
        this.LblUsername.TabIndex = 1;
        this.LblUsername.Text = "用户名：";

        // TxtUsername
        this.TxtUsername.Location = new Point(50, 115);
        this.TxtUsername.Name = "TxtUsername";
        this.TxtUsername.Size = new Size(300, 26);
        this.TxtUsername.TabIndex = 2;

        // LblPassword
        this.LblPassword.AutoSize = true;
        this.LblPassword.Location = new Point(50, 155);
        this.LblPassword.Name = "LblPassword";
        this.LblPassword.Size = new Size(56, 20);
        this.LblPassword.TabIndex = 3;
        this.LblPassword.Text = "密码：";

        // TxtPassword
        this.TxtPassword.Location = new Point(50, 180);
        this.TxtPassword.Name = "TxtPassword";
        this.TxtPassword.PasswordChar = '*';
        this.TxtPassword.Size = new Size(300, 26);
        this.TxtPassword.TabIndex = 4;
        this.TxtPassword.KeyPress += new KeyPressEventHandler(this.TxtPassword_KeyPress);

        // ChkRememberPassword
        this.ChkRememberPassword.AutoSize = true;
        this.ChkRememberPassword.Location = new Point(50, 220);
        this.ChkRememberPassword.Name = "ChkRememberPassword";
        this.ChkRememberPassword.Size = new Size(90, 24);
        this.ChkRememberPassword.TabIndex = 5;
        this.ChkRememberPassword.Text = "记住密码";
        this.ChkRememberPassword.UseVisualStyleBackColor = true;

        // BtnLogin
        this.BtnLogin.BackColor = Color.FromArgb(0, 120, 215);
        this.BtnLogin.FlatStyle = FlatStyle.Flat;
        this.BtnLogin.ForeColor = Color.White;
        this.BtnLogin.Location = new Point(80, 260);
        this.BtnLogin.Name = "BtnLogin";
        this.BtnLogin.Size = new Size(100, 35);
        this.BtnLogin.TabIndex = 6;
        this.BtnLogin.Text = "登录";
        this.BtnLogin.UseVisualStyleBackColor = false;
        this.BtnLogin.Click += new EventHandler(this.BtnLogin_Click);

        // BtnCancel
        this.BtnCancel.BackColor = Color.Gray;
        this.BtnCancel.FlatStyle = FlatStyle.Flat;
        this.BtnCancel.ForeColor = Color.White;
        this.BtnCancel.Location = new Point(220, 260);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new Size(100, 35);
        this.BtnCancel.TabIndex = 7;
        this.BtnCancel.Text = "取消";
        this.BtnCancel.UseVisualStyleBackColor = false;
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // LoginForm
        this.AcceptButton = this.BtnLogin;
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(400, 330);
        this.Controls.Add(this.PnlMain);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "LoginForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "登录 - 外发加工管理系统";
        this.PnlMain.ResumeLayout(false);
        this.PnlMain.PerformLayout();
        this.ResumeLayout(false);
    }
}
