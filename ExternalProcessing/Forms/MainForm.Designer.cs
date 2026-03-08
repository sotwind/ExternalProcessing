namespace ExternalProcessing.Forms;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private Panel PnlHeader = null!;
    private Panel PnlContent = null!;
    private Panel PnlFooter = null!;
    private Label LblUserInfo = null!;
    private Button BtnLogout = null!;
    private Button BtnApplication = null!;
    private Button BtnAudit = null!;
    private Button BtnAcceptance = null!;
    private Button BtnReconciliation = null!;
    private Button BtnFinanceAudit = null!;
    private Button BtnReport = null!;
    private Button BtnUserManagement = null!;
    private Label LblCopyright = null!;

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
        this.PnlHeader = new Panel();
        this.LblUserInfo = new Label();
        this.BtnLogout = new Button();
        this.PnlContent = new Panel();
        this.BtnApplication = new Button();
        this.BtnAudit = new Button();
        this.BtnAcceptance = new Button();
        this.BtnReconciliation = new Button();
        this.BtnFinanceAudit = new Button();
        this.BtnReport = new Button();
        this.BtnUserManagement = new Button();
        this.PnlFooter = new Panel();
        this.LblCopyright = new Label();
        this.PnlHeader.SuspendLayout();
        this.PnlContent.SuspendLayout();
        this.PnlFooter.SuspendLayout();
        this.SuspendLayout();

        // PnlHeader
        this.PnlHeader.BackColor = Color.FromArgb(0, 120, 215);
        this.PnlHeader.Controls.Add(this.LblUserInfo);
        this.PnlHeader.Controls.Add(this.BtnLogout);
        this.PnlHeader.Dock = DockStyle.Top;
        this.PnlHeader.Location = new Point(0, 0);
        this.PnlHeader.Name = "PnlHeader";
        this.PnlHeader.Size = new Size(800, 60);
        this.PnlHeader.TabIndex = 0;

        // LblUserInfo
        this.LblUserInfo.AutoSize = true;
        this.LblUserInfo.Font = new Font("Microsoft YaHei", 12F);
        this.LblUserInfo.ForeColor = Color.White;
        this.LblUserInfo.Location = new Point(20, 20);
        this.LblUserInfo.Name = "LblUserInfo";
        this.LblUserInfo.Size = new Size(100, 20);
        this.LblUserInfo.TabIndex = 0;
        this.LblUserInfo.Text = "当前用户：";

        // BtnLogout
        this.BtnLogout.BackColor = Color.FromArgb(220, 53, 69);
        this.BtnLogout.FlatStyle = FlatStyle.Flat;
        this.BtnLogout.Font = new Font("Microsoft YaHei", 10F);
        this.BtnLogout.ForeColor = Color.White;
        this.BtnLogout.Location = new Point(680, 15);
        this.BtnLogout.Name = "BtnLogout";
        this.BtnLogout.Size = new Size(100, 30);
        this.BtnLogout.TabIndex = 1;
        this.BtnLogout.Text = "退出登录";
        this.BtnLogout.UseVisualStyleBackColor = false;
        this.BtnLogout.Click += new EventHandler(this.BtnLogout_Click);

        // PnlContent
        this.PnlContent.BackColor = Color.FromArgb(240, 240, 240);
        this.PnlContent.Controls.Add(this.BtnApplication);
        this.PnlContent.Controls.Add(this.BtnAudit);
        this.PnlContent.Controls.Add(this.BtnAcceptance);
        this.PnlContent.Controls.Add(this.BtnReconciliation);
        this.PnlContent.Controls.Add(this.BtnFinanceAudit);
        this.PnlContent.Controls.Add(this.BtnReport);
        this.PnlContent.Controls.Add(this.BtnUserManagement);
        this.PnlContent.Dock = DockStyle.Fill;
        this.PnlContent.Location = new Point(0, 60);
        this.PnlContent.Name = "PnlContent";
        this.PnlContent.Size = new Size(800, 440);
        this.PnlContent.TabIndex = 1;

        // BtnApplication - 申请管理
        this.BtnApplication.BackColor = Color.FromArgb(40, 167, 69);
        this.BtnApplication.FlatStyle = FlatStyle.Flat;
        this.BtnApplication.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnApplication.ForeColor = Color.White;
        this.BtnApplication.Location = new Point(80, 50);
        this.BtnApplication.Name = "BtnApplication";
        this.BtnApplication.Size = new Size(180, 120);
        this.BtnApplication.TabIndex = 0;
        this.BtnApplication.Text = "申请管理";
        this.BtnApplication.UseVisualStyleBackColor = false;
        this.BtnApplication.Click += new EventHandler(this.BtnApplication_Click);

        // BtnAudit - 审批管理
        this.BtnAudit.BackColor = Color.FromArgb(255, 193, 7);
        this.BtnAudit.FlatStyle = FlatStyle.Flat;
        this.BtnAudit.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnAudit.ForeColor = Color.White;
        this.BtnAudit.Location = new Point(310, 50);
        this.BtnAudit.Name = "BtnAudit";
        this.BtnAudit.Size = new Size(180, 120);
        this.BtnAudit.TabIndex = 1;
        this.BtnAudit.Text = "审批管理";
        this.BtnAudit.UseVisualStyleBackColor = false;
        this.BtnAudit.Click += new EventHandler(this.BtnAudit_Click);

        // BtnAcceptance - 验收管理
        this.BtnAcceptance.BackColor = Color.FromArgb(23, 162, 184);
        this.BtnAcceptance.FlatStyle = FlatStyle.Flat;
        this.BtnAcceptance.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnAcceptance.ForeColor = Color.White;
        this.BtnAcceptance.Location = new Point(540, 50);
        this.BtnAcceptance.Name = "BtnAcceptance";
        this.BtnAcceptance.Size = new Size(180, 120);
        this.BtnAcceptance.TabIndex = 2;
        this.BtnAcceptance.Text = "验收管理";
        this.BtnAcceptance.UseVisualStyleBackColor = false;
        this.BtnAcceptance.Click += new EventHandler(this.BtnAcceptance_Click);

        // BtnReconciliation - 对账管理
        this.BtnReconciliation.BackColor = Color.FromArgb(108, 117, 125);
        this.BtnReconciliation.FlatStyle = FlatStyle.Flat;
        this.BtnReconciliation.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnReconciliation.ForeColor = Color.White;
        this.BtnReconciliation.Location = new Point(80, 220);
        this.BtnReconciliation.Name = "BtnReconciliation";
        this.BtnReconciliation.Size = new Size(180, 120);
        this.BtnReconciliation.TabIndex = 3;
        this.BtnReconciliation.Text = "对账管理";
        this.BtnReconciliation.UseVisualStyleBackColor = false;
        this.BtnReconciliation.Click += new EventHandler(this.BtnReconciliation_Click);

        // BtnFinanceAudit - 财务审核
        this.BtnFinanceAudit.BackColor = Color.FromArgb(102, 16, 242);
        this.BtnFinanceAudit.FlatStyle = FlatStyle.Flat;
        this.BtnFinanceAudit.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnFinanceAudit.ForeColor = Color.White;
        this.BtnFinanceAudit.Location = new Point(310, 220);
        this.BtnFinanceAudit.Name = "BtnFinanceAudit";
        this.BtnFinanceAudit.Size = new Size(180, 120);
        this.BtnFinanceAudit.TabIndex = 4;
        this.BtnFinanceAudit.Text = "财务审核";
        this.BtnFinanceAudit.UseVisualStyleBackColor = false;
        this.BtnFinanceAudit.Click += new EventHandler(this.BtnFinanceAudit_Click);

        // BtnReport - 统计报表
        this.BtnReport.BackColor = Color.FromArgb(220, 53, 69);
        this.BtnReport.FlatStyle = FlatStyle.Flat;
        this.BtnReport.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnReport.ForeColor = Color.White;
        this.BtnReport.Location = new Point(540, 220);
        this.BtnReport.Name = "BtnReport";
        this.BtnReport.Size = new Size(180, 120);
        this.BtnReport.TabIndex = 5;
        this.BtnReport.Text = "统计报表";
        this.BtnReport.UseVisualStyleBackColor = false;
        this.BtnReport.Click += new EventHandler(this.BtnReport_Click);

        // BtnUserManagement - 用户管理
        this.BtnUserManagement.BackColor = Color.FromArgb(111, 66, 193);
        this.BtnUserManagement.FlatStyle = FlatStyle.Flat;
        this.BtnUserManagement.Font = new Font("Microsoft YaHei", 14F, FontStyle.Bold);
        this.BtnUserManagement.ForeColor = Color.White;
        this.BtnUserManagement.Location = new Point(310, 350);
        this.BtnUserManagement.Name = "BtnUserManagement";
        this.BtnUserManagement.Size = new Size(180, 60);
        this.BtnUserManagement.TabIndex = 6;
        this.BtnUserManagement.Text = "用户管理";
        this.BtnUserManagement.UseVisualStyleBackColor = false;
        this.BtnUserManagement.Click += new EventHandler(this.BtnUserManagement_Click);

        // PnlFooter
        this.PnlFooter.BackColor = Color.FromArgb(233, 236, 239);
        this.PnlFooter.Controls.Add(this.LblCopyright);
        this.PnlFooter.Dock = DockStyle.Bottom;
        this.PnlFooter.Location = new Point(0, 500);
        this.PnlFooter.Name = "PnlFooter";
        this.PnlFooter.Size = new Size(800, 40);
        this.PnlFooter.TabIndex = 2;

        // LblCopyright
        this.LblCopyright.AutoSize = true;
        this.LblCopyright.Font = new Font("Microsoft YaHei", 9F);
        this.LblCopyright.ForeColor = Color.FromArgb(108, 117, 125);
        this.LblCopyright.Location = new Point(250, 12);
        this.LblCopyright.Name = "LblCopyright";
        this.LblCopyright.Size = new Size(200, 15);
        this.LblCopyright.TabIndex = 0;
        this.LblCopyright.Text = "© 2026 外发加工管理系统 v1.0";

        // MainForm
        this.AutoScaleDimensions = new SizeF(8F, 20F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(800, 540);
        this.Controls.Add(this.PnlContent);
        this.Controls.Add(this.PnlFooter);
        this.Controls.Add(this.PnlHeader);
        this.Name = "MainForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "外发加工管理系统";
        this.PnlHeader.ResumeLayout(false);
        this.PnlHeader.PerformLayout();
        this.PnlContent.ResumeLayout(false);
        this.PnlFooter.ResumeLayout(false);
        this.PnlFooter.PerformLayout();
        this.ResumeLayout(false);
    }
}
