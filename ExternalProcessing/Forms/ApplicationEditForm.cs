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
    private readonly User _currentUser;

    public ApplicationEditForm(User currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        _isEdit = false;
        this.Text = "新增申请";
        TxtApplicantName.Text = _currentUser.Username;
    }

    public ApplicationEditForm(ExternalProcessingApplication application, User currentUser)
    {
        _currentUser = currentUser;
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
        this.TxtQuantity = new TextBox();
        this.DtpExpectedReturnDate = new DateTimePicker();
        this.TxtRemark = new TextBox();
        this.BtnSave = new Button();
        this.BtnCancel = new Button();
        this.LblOrderNo = new Label();
        this.LblApplicantName = new Label();
        this.LblProcessorName = new Label();
        this.LblProcessingContent = new Label();
        this.LblQuantity = new Label();
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
        this.TxtApplicantName.ReadOnly = true;

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
        this.TxtProcessingContent.Size = new System.Drawing.Size(250, 50);

        // LblQuantity
        this.LblQuantity.AutoSize = true;
        this.LblQuantity.Location = new System.Drawing.Point(30, 210);
        this.LblQuantity.Name = "LblQuantity";
        this.LblQuantity.Size = new System.Drawing.Size(70, 17);
        this.LblQuantity.Text = "数量：";

        // TxtQuantity
        this.TxtQuantity.Location = new System.Drawing.Point(110, 207);
        this.TxtQuantity.Name = "TxtQuantity";
        this.TxtQuantity.Size = new System.Drawing.Size(250, 23);
        this.TxtQuantity.Text = "1";

        // LblExpectedReturnDate
        this.LblExpectedReturnDate.AutoSize = true;
        this.LblExpectedReturnDate.Location = new System.Drawing.Point(30, 250);
        this.LblExpectedReturnDate.Name = "LblExpectedReturnDate";
        this.LblExpectedReturnDate.Size = new System.Drawing.Size(70, 17);
        this.LblExpectedReturnDate.Text = "预计归还：";

        // DtpExpectedReturnDate
        this.DtpExpectedReturnDate.Location = new System.Drawing.Point(110, 247);
        this.DtpExpectedReturnDate.Name = "DtpExpectedReturnDate";
        this.DtpExpectedReturnDate.Size = new System.Drawing.Size(250, 23);
        this.DtpExpectedReturnDate.Value = DateTime.Now.AddDays(7);

        // LblRemark
        this.LblRemark.AutoSize = true;
        this.LblRemark.Location = new System.Drawing.Point(30, 290);
        this.LblRemark.Name = "LblRemark";
        this.LblRemark.Size = new System.Drawing.Size(70, 17);
        this.LblRemark.Text = "备注：";

        // TxtRemark
        this.TxtRemark.Location = new System.Drawing.Point(110, 287);
        this.TxtRemark.Multiline = true;
        this.TxtRemark.Name = "TxtRemark";
        this.TxtRemark.Size = new System.Drawing.Size(250, 50);

        // BtnSave
        this.BtnSave.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
        this.BtnSave.ForeColor = System.Drawing.Color.White;
        this.BtnSave.Location = new System.Drawing.Point(80, 360);
        this.BtnSave.Name = "BtnSave";
        this.BtnSave.Size = new System.Drawing.Size(100, 35);
        this.BtnSave.Text = "保存";
        this.BtnSave.UseVisualStyleBackColor = false;
        this.BtnSave.Click += new EventHandler(this.BtnSave_Click);

        // BtnCancel
        this.BtnCancel.Location = new System.Drawing.Point(210, 360);
        this.BtnCancel.Name = "BtnCancel";
        this.BtnCancel.Size = new System.Drawing.Size(100, 35);
        this.BtnCancel.Text = "取消";
        this.BtnCancel.Click += new EventHandler(this.BtnCancel_Click);

        // ApplicationEditForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(400, 430);
        this.Controls.Add(this.LblOrderNo);
        this.Controls.Add(this.TxtOrderNo);
        this.Controls.Add(this.LblApplicantName);
        this.Controls.Add(this.TxtApplicantName);
        this.Controls.Add(this.LblProcessorName);
        this.Controls.Add(this.TxtProcessorName);
        this.Controls.Add(this.LblProcessingContent);
        this.Controls.Add(this.TxtProcessingContent);
        this.Controls.Add(this.LblQuantity);
        this.Controls.Add(this.TxtQuantity);
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
    private TextBox TxtQuantity = null!;
    private DateTimePicker DtpExpectedReturnDate = null!;
    private TextBox TxtRemark = null!;
    private Button BtnSave = null!;
    private Button BtnCancel = null!;
    private Label LblOrderNo = null!;
    private Label LblApplicantName = null!;
    private Label LblProcessorName = null!;
    private Label LblProcessingContent = null!;
    private Label LblQuantity = null!;
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
            TxtQuantity.Text = _application.TotalQuantity > 0 ? _application.TotalQuantity.ToString() : "1";
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

        if (!int.TryParse(TxtQuantity.Text.Trim(), out int quantity) || quantity <= 0)
        {
            MessageBox.Show("请输入有效的数量", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            TxtQuantity.Focus();
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
                TotalQuantity = quantity,
                ExpectedReturnDate = DtpExpectedReturnDate.Value,
                Remark = TxtRemark.Text.Trim(),
                Status = 1,
                OperatorId = _currentUser.UserID,
                ApplicationDate = DateTime.Now,
                OrderId = null,
                ApplicantId = _currentUser.UserID,
                ProcessorId = null
            };

            if (_isEdit && _application != null)
            {
                application.ApplicationId = _application.ApplicationId;
                application.ApplicationNo = _application.ApplicationNo;
                application.OrderId = _application.OrderId;
                application.ApplicantId = _application.ApplicantId;
                application.ProcessorId = _application.ProcessorId;
                
                if (_application.Status == 3)
                {
                    application.Status = 1;
                }
                else
                {
                    application.Status = _application.Status;
                }
                
                try
                {
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
                catch (Exception ex)
                {
                    MessageBox.Show("更新失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
