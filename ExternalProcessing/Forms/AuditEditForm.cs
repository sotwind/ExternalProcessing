using System;
using System.Windows.Forms;
using ExternalProcessing.Models;
using ExternalProcessing.Services;

namespace ExternalProcessing.Forms;

public partial class AuditEditForm : Form
{
    private readonly ExternalProcessingApplication _application;
    private readonly ExternalProcessingAuditService _auditService = new();
    private readonly User _currentUser;

    public AuditEditForm(ExternalProcessingApplication application, User currentUser)
    {
        _application = application;
        _currentUser = currentUser;
        InitializeComponent();
        LoadData();
    }

    private void InitializeComponent()
    {
        this.LblApplicationNo = new Label();
        this.TxtApplicationNo = new TextBox();
        this.LblProcessorName = new Label();
        this.TxtProcessorName = new TextBox();
        this.LblProcessingContent = new Label();
        this.TxtProcessingContent = new TextBox();
        this.LblAuditResult = new Label();
        this.CboAuditResult = new ComboBox();
        this.LblAuditRemark = new Label();
        this.TxtAuditRemark = new TextBox();
        this.BtnSave = new Button();
        this.BtnCancel = new Button();
        this.SuspendLayout();

        // LblApplicationNo
        this.LblApplicationNo.AutoSize = true;
        this.LblApplicationNo.Location = new System.Drawing.Point(30, 30);
        this.LblApplicationNo.Name = "LblApplicationNo";
        this.LblApplicationNo.Size = new System.Drawing.Size(70, 17);
        this.LblApplicationNo.Text = "申请编号：";

        // TxtApplicationNo
        this.TxtApplicationNo.Location = new System.Drawing.Point(110, 27);
        this.TxtApplicationNo.Name = "TxtApplicationNo";
        this.TxtApplicationNo.Size = new System.Drawing.Size(250, 23);
        this.TxtApplicationNo.ReadOnly = true;

        // LblProcessorName
        this.LblProcessorName.AutoSize = true;
        this.LblProcessorName.Location = new System.Drawing.Point(30, 70);
        this.LblProcessorName.Name = "LblProcessorName";
        this.LblProcessorName.Size = new System.Drawing.Size(70, 17);
        this.LblProcessorName.Text = "加工商：";

        // TxtProcessorName
        this.TxtProcessorName.Location = new System.Drawing.Point(110, 67);
        this.TxtProcessorName.Name = "TxtProcessorName";
        this.TxtProcessorName.Size = new System.Drawing.Size(250, 23);
        this.TxtProcessorName.ReadOnly = true;

        // LblProcessingContent
        this.LblProcessingContent.AutoSize = true;
        this.LblProcessingContent.Location = new System.Drawing.Point(30, 110);
        this.LblProcessingContent.Name = "LblProcessingContent";
        this.LblProcessingContent.Size = new System.Drawing.Size(70, 17);
        this.LblProcessingContent.Text = "加工内容：";

        // TxtProcessingContent
        this.TxtProcessingContent.Location = new System.Drawing.Point(110, 107);
        this.TxtProcessingContent.Multiline = true;
        this.TxtProcessingContent.Name = "TxtProcessingContent";
        this.TxtProcessingContent.Size = new System.Drawing.Size(250, 60);
        this.TxtProcessingContent.ReadOnly = true;

        // LblAuditResult
        this.LblAuditResult.AutoSize = true;
        this.LblAuditResult.Location = new System.Drawing.Point(30, 190);
        this.LblAuditResult.Name = "LblAuditResult";
        this.LblAuditResult.Size = new System.Drawing.Size(70, 17);
        this.LblAuditResult.Text = "审批结果：";

        // CboAuditResult
        this.CboAuditResult.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboAuditResult.FormattingEnabled = true;
        this.CboAuditResult.Location = new System.Drawing.Point(110, 187);
        this.CboAuditResult.Name = "CboAuditResult";
        this.CboAuditResult.Size = new System.Drawing.Size(250, 23);

        // LblAuditRemark
        this.LblAuditRemark.AutoSize = true;
        this.LblAuditRemark.Location = new System.Drawing.Point(30, 230);
        this.LblAuditRemark.Name = "LblAuditRemark";
        this.LblAuditRemark.Size = new System.Drawing.Size(70, 17);
        this.LblAuditRemark.Text = "审批意见：";

        // TxtAuditRemark
        this.TxtAuditRemark.Location = new System.Drawing.Point(110, 227);
        this.TxtAuditRemark.Multiline = true;
        this.TxtAuditRemark.Name = "TxtAuditRemark";
        this.TxtAuditRemark.Size = new System.Drawing.Size(250, 80);

        // BtnSave
        this.BtnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        this.BtnSave.ForeColor = System.Drawing.Color.White;
        this.BtnSave.Location = new System.Drawing.Point(80, 330);
        this.BtnSave.Name = "BtnSave";
        this.BtnSave.Size = new System.Drawing.Size(100, 35);
        this.BtnSave.Text = "保存";
        this.BtnSave.UseVisualStyleBackColor = false;
        this.BtnSave.Click += new EventHandler(this.BtnSave_Click);

        // BtnCancel
        this.BtnCancel.Location = new System.Drawing.Point(210, 330);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(100, 35);
        this.BtnCancel.Text = "取消";
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // AuditEditForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 400);
        this.Controls.Add(this.LblApplicationNo);
        this.Controls.Add(this.TxtApplicationNo);
        this.Controls.Add(this.LblProcessorName);
        this.Controls.Add(this.TxtProcessorName);
        this.Controls.Add(this.LblProcessingContent);
        this.Controls.Add(this.TxtProcessingContent);
        this.Controls.Add(this.LblAuditResult);
        this.Controls.Add(this.CboAuditResult);
        this.Controls.Add(this.LblAuditRemark);
        this.Controls.Add(this.TxtAuditRemark);
        this.Controls.Add(this.BtnSave);
        this.Controls.Add(this.BtnCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "AuditEditForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "审批";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private Label LblApplicationNo = null!;
    private TextBox TxtApplicationNo = null!;
    private Label LblProcessorName = null!;
    private TextBox TxtProcessorName = null!;
    private Label LblProcessingContent = null!;
    private TextBox TxtProcessingContent = null!;
    private Label LblAuditResult = null!;
    private ComboBox CboAuditResult = null!;
    private Label LblAuditRemark = null!;
    private TextBox TxtAuditRemark = null!;
    private Button BtnSave = null!;
    private Button BtnCancel = null!;

    private void LoadData()
    {
        TxtApplicationNo.Text = _application.ApplicationNo ?? "";
        TxtProcessorName.Text = _application.ProcessorName ?? "";
        TxtProcessingContent.Text = _application.ProcessingContent ?? "";

        // 加载审批结果选项
        CboAuditResult.Items.Clear();
        CboAuditResult.Items.Add(new ComboBoxItem("通过", 2));
        CboAuditResult.Items.Add(new ComboBoxItem("拒绝", 3));
        CboAuditResult.DisplayMember = "Text";
        CboAuditResult.ValueMember = "Value";
        CboAuditResult.SelectedIndex = 0;
    }

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
                AuditorId = _currentUser.UserID,
                AuditorName = _currentUser.Username,
                AuditResult = auditResult,
                AuditRemark = TxtAuditRemark.Text.Trim(),
                OperatorId = _currentUser.UserID
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
