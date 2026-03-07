using System;
using System.Windows.Forms;
using ExternalProcessing.Models;
using ExternalProcessing.Services;

namespace ExternalProcessing.Forms;

public partial class ApplicationEditForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private ExternalProcessingApplication? _application;
    private bool _isEdit = false;

    public ApplicationEditForm()
    {
        InitializeComponent();
        _isEdit = false;
        this.Text = "新增申请";
    }

    public ApplicationEditForm(ExternalProcessingApplication application)
    {
        InitializeComponent();
        _application = application;
        _isEdit = true;
        this.Text = "编辑申请";
        LoadData();
    }

    private void InitializeComponent()
    {
        this.TxtOrderNo = new TextBox();
        this.TxtApplicantName = new TextBox();
        this.TxtProcessorName = new TextBox();
        this.TxtProcessingContent = new TextBox();
        this.DtpExpectedReturnDate = new DateTimePicker();
        this.TxtRemark = new TextBox();
        this.BtnSave = new Button();
        this.BtnCancel = new Button();
        this.LblOrderNo = new Label();
        this.LblApplicantName = new Label();
        this.LblProcessorName = new Label();
        this.LblProcessingContent = new Label();
        this.LblExpectedReturnDate = new Label();
        this.LblRemark = new Label();
        this.SuspendLayout();

        // LblOrderNo
        this.LblOrderNo.AutoSize = true;
        this.LblOrderNo.Location = new System.Drawing.Point(30, 30);
        this.LblOrderNo.Name = "LblOrderNo";
        this.LblOrderNo.Size = new System.Drawing.Size(70, 17);
        this.LblOrderNo.Text = "订单编号：";

        // TxtOrderNo
        this.TxtOrderNo.Location = new System.Drawing.Point(110, 27);
        this.TxtOrderNo.Name = "TxtOrderNo";
        this.TxtOrderNo.Size = new System.Drawing.Size(250, 23);

        // LblApplicantName
        this.LblApplicantName.AutoSize = true;
        this.LblApplicantName.Location = new System.Drawing.Point(30, 70);
        this.LblApplicantName.Name = "LblApplicantName";
        this.LblApplicantName.Size = new System.Drawing.Size(70, 17);
        this.LblApplicantName.Text = "申请人：";

        // TxtApplicantName
        this.TxtApplicantName.Location = new System.Drawing.Point(110, 67);
        this.TxtApplicantName.Name = "TxtApplicantName";
        this.TxtApplicantName.Size = new System.Drawing.Size(250, 23);

        // LblProcessorName
        this.LblProcessorName.AutoSize = true;
        this.LblProcessorName.Location = new System.Drawing.Point(30, 110);
        this.LblProcessorName.Name = "LblProcessorName";
        this.LblProcessorName.Size = new System.Drawing.Size(70, 17);
        this.LblProcessorName.Text = "加工商：";

        // TxtProcessorName
        this.TxtProcessorName.Location = new System.Drawing.Point(110, 107);
        this.TxtProcessorName.Name = "TxtProcessorName";
        this.TxtProcessorName.Size = new System.Drawing.Size(250, 23);

        // LblProcessingContent
        this.LblProcessingContent.AutoSize = true;
        this.LblProcessingContent.Location = new System.Drawing.Point(30, 150);
        this.LblProcessingContent.Name = "LblProcessingContent";
        this.LblProcessingContent.Size = new System.Drawing.Size(70, 17);
        this.LblProcessingContent.Text = "加工内容：";

        // TxtProcessingContent
        this.TxtProcessingContent.Location = new System.Drawing.Point(110, 147);
        this.TxtProcessingContent.Multiline = true;
        this.TxtProcessingContent.Name = "TxtProcessingContent";
        this.TxtProcessingContent.Size = new System.Drawing.Size(250, 60);

        // LblExpectedReturnDate
        this.LblExpectedReturnDate.AutoSize = true;
        this.LblExpectedReturnDate.Location = new System.Drawing.Point(30, 230);
        this.LblExpectedReturnDate.Name = "LblExpectedReturnDate";
        this.LblExpectedReturnDate.Size = new System.Drawing.Size(70, 17);
        this.LblExpectedReturnDate.Text = "预计归还：";

        // DtpExpectedReturnDate
        this.DtpExpectedReturnDate.Location = new System.Drawing.Point(110, 227);
        this.DtpExpectedReturnDate.Name = "DtpExpectedReturnDate";
        this.DtpExpectedReturnDate.Size = new System.Drawing.Size(250, 23);
        this.DtpExpectedReturnDate.Value = DateTime.Now.AddDays(7);

        // LblRemark
        this.LblRemark.AutoSize = true;
        this.LblRemark.Location = new System.Drawing.Point(30, 270);
        this.LblRemark.Name = "LblRemark";
        this.LblRemark.Size = new System.Drawing.Size(70, 17);
        this.LblRemark.Text = "备注：";

        // TxtRemark
        this.TxtRemark.Location = new System.Drawing.Point(110, 267);
        this.TxtRemark.Multiline = true;
        this.TxtRemark.Name = "TxtRemark";
        this.TxtRemark.Size = new System.Drawing.Size(250, 60);

        // BtnSave
        this.BtnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        this.BtnSave.ForeColor = System.Drawing.Color.White;
        this.BtnSave.Location = new System.Drawing.Point(80, 350);
        this.BtnSave.Name = "BtnSave";
        this.BtnSave.Size = new System.Drawing.Size(100, 35);
        this.BtnSave.Text = "保存";
        this.BtnSave.UseVisualStyleBackColor = false;
        this.BtnSave.Click += new EventHandler(this.BtnSave_Click);

        // BtnCancel
        this.BtnCancel.Location = new System.Drawing.Point(210, 350);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(100, 35);
        this.BtnCancel.Text = "取消";
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // ApplicationEditForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 420);
        this.Controls.Add(this.LblOrderNo);
        this.Controls.Add(this.TxtOrderNo);
        this.Controls.Add(this.LblApplicantName);
        this.Controls.Add(this.TxtApplicantName);
        this.Controls.Add(this.LblProcessorName);
        this.Controls.Add(this.TxtProcessorName);
        this.Controls.Add(this.LblProcessingContent);
        this.Controls.Add(this.TxtProcessingContent);
        this.Controls.Add(this.LblExpectedReturnDate);
        this.Controls.Add(this.DtpExpectedReturnDate);
        this.Controls.Add(this.LblRemark);
        this.Controls.Add(this.TxtRemark);
        this.Controls.Add(this.BtnSave);
        this.Controls.Add(this.BtnCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "ApplicationEditForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "申请编辑";
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private TextBox TxtOrderNo = null!;
    private TextBox TxtApplicantName = null!;
    private TextBox TxtProcessorName = null!;
    private TextBox TxtProcessingContent = null!;
    private DateTimePicker DtpExpectedReturnDate = null!;
    private TextBox TxtRemark = null!;
    private Button BtnSave = null!;
    private Button BtnCancel = null!;
    private Label LblOrderNo = null!;
    private Label LblApplicantName = null!;
    private Label LblProcessorName = null!;
    private Label LblProcessingContent = null!;
    private Label LblExpectedReturnDate = null!;
    private Label LblRemark = null!;

    private void LoadData()
    {
        if (_application != null)
        {
            TxtOrderNo.Text = _application.OrderNo ?? "";
            TxtApplicantName.Text = _application.ApplicantName ?? "";
            TxtProcessorName.Text = _application.ProcessorName ?? "";
            TxtProcessingContent.Text = _application.ProcessingContent ?? "";
            DtpExpectedReturnDate.Value = _application.ExpectedReturnDate;
            TxtRemark.Text = _application.Remark ?? "";
        }
    }

    private void BtnSave_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TxtProcessorName.Text))
        {
            MessageBox.Show("请输入加工商", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtProcessorName.Focus();
            return;
        }

        try
        {
            var application = new ExternalProcessingApplication
            {
                OrderNo = TxtOrderNo.Text.Trim(),
                ApplicantName = TxtApplicantName.Text.Trim(),
                ProcessorName = TxtProcessorName.Text.Trim(),
                ProcessingContent = TxtProcessingContent.Text.Trim(),
                ExpectedReturnDate = DtpExpectedReturnDate.Value,
                Remark = TxtRemark.Text.Trim(),
                Status = 1, // 待审批
                OperatorId = 1, // 当前用户ID
                ApplicationDate = DateTime.Now
            };

            if (_isEdit && _application != null)
            {
                application.ApplicationId = _application.ApplicationId;
                application.ApplicationNo = _application.ApplicationNo;
                application.OrderId = _application.OrderId;
                application.ApplicantId = _application.ApplicantId;
                application.ProcessorId = _application.ProcessorId;
                application.Status = _application.Status;
                
                if (_service.UpdateApplication(application, new System.Collections.Generic.List<ExternalProcessingApplicationDetail>()))
                {
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
                var id = _service.CreateApplication(application, new System.Collections.Generic.List<ExternalProcessingApplicationDetail>());
                if (id > 0)
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
}
