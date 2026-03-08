using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using ExternalProcessing.Services;
using ExternalProcessing.Models;
using ExternalProcessing.Printing;
using System.Drawing.Printing;

namespace ExternalProcessing.Forms;

public partial class AuditForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private readonly ExternalProcessingAuditService _auditService = new();
    private List<ExternalProcessingApplication> _applications = new();
    private readonly User _currentUser;

    public AuditForm(User currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        LoadStatusOptions();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnAudit = new Button();
        this.BtnUnAudit = new Button();
        this.BtnRefresh = new Button();
        this.CboStatus = new ComboBox();
        this.TxtSearch = new TextBox();
        this.BtnSearch = new Button();
        this.Label1 = new Label();
        this.Label2 = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).BeginInit();
        this.SuspendLayout();

        this.Label1.Location = new System.Drawing.Point(30, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 0;
        this.Label1.Text = "状态：";

        this.Label2.Location = new System.Drawing.Point(310, 28);
        this.Label2.Name = "Label2";
        this.Label2.Size = new System.Drawing.Size(60, 23);
        this.Label2.TabIndex = 1;
        this.Label2.Text = "搜索：";

        this.CboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboStatus.FormattingEnabled = true;
        this.CboStatus.Location = new System.Drawing.Point(90, 26);
        this.CboStatus.Name = "CboStatus";
        this.CboStatus.Size = new System.Drawing.Size(150, 23);
        this.CboStatus.TabIndex = 2;
        this.CboStatus.SelectedIndexChanged += new EventHandler(this.CboStatus_SelectedIndexChanged);

        this.TxtSearch.Location = new System.Drawing.Point(370, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(233, 23);
        this.TxtSearch.TabIndex = 3;

        this.BtnSearch.Location = new System.Drawing.Point(620, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(93, 30);
        this.BtnSearch.TabIndex = 4;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        this.BtnRefresh.Location = new System.Drawing.Point(720, 23);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(93, 30);
        this.BtnRefresh.TabIndex = 5;
        this.BtnRefresh.Text = "重置";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        this.DgvApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvApplications.Location = new System.Drawing.Point(30, 70);
        this.DgvApplications.Name = "DgvApplications";
        this.DgvApplications.Size = new System.Drawing.Size(940, 460);
        this.DgvApplications.TabIndex = 4;
        this.DgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.DgvApplications.MultiSelect = true;
        this.DgvApplications.ReadOnly = true;
        this.DgvApplications.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvApplications_CellDoubleClick);

        // LblMultiSelectHint
        this.LblMultiSelectHint = new Label();
        this.LblMultiSelectHint.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        this.LblMultiSelectHint.Location = new System.Drawing.Point(30, 535);
        this.LblMultiSelectHint.Name = "LblMultiSelectHint";
        this.LblMultiSelectHint.Size = new System.Drawing.Size(940, 20);
        this.LblMultiSelectHint.TabIndex = 8;
        this.LblMultiSelectHint.Text = "提示：按住 Ctrl 键或拖动鼠标可选择多个订单进行合并打印";
        this.LblMultiSelectHint.ForeColor = System.Drawing.Color.Gray;
        this.LblMultiSelectHint.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Italic);

        // BtnAudit
        this.BtnAudit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnAudit.BackColor = System.Drawing.Color.FromArgb(255, 255, 153);
        this.BtnAudit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnAudit.Location = new System.Drawing.Point(990, 70);
        this.BtnAudit.Name = "BtnAudit";
        this.BtnAudit.Size = new System.Drawing.Size(100, 35);
        this.BtnAudit.TabIndex = 5;
        this.BtnAudit.Text = "审批";
        this.BtnAudit.UseVisualStyleBackColor = false;
        this.BtnAudit.Click += new EventHandler(this.BtnAudit_Click);

        // BtnUnAudit
        this.BtnUnAudit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnUnAudit.BackColor = System.Drawing.Color.FromArgb(255, 99, 71);
        this.BtnUnAudit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnUnAudit.ForeColor = System.Drawing.Color.White;
        this.BtnUnAudit.Location = new System.Drawing.Point(990, 120);
        this.BtnUnAudit.Name = "BtnUnAudit";
        this.BtnUnAudit.Size = new System.Drawing.Size(100, 35);
        this.BtnUnAudit.TabIndex = 6;
        this.BtnUnAudit.Text = "反审";
        this.BtnUnAudit.UseVisualStyleBackColor = false;
        this.BtnUnAudit.Click += new EventHandler(this.BtnUnAudit_Click);

        // BtnPrintGatePass
        this.BtnPrintGatePass = new Button();
        this.BtnPrintGatePass.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnPrintGatePass.BackColor = System.Drawing.Color.FromArgb(70, 130, 180);
        this.BtnPrintGatePass.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnPrintGatePass.ForeColor = System.Drawing.Color.White;
        this.BtnPrintGatePass.Location = new System.Drawing.Point(990, 170);
        this.BtnPrintGatePass.Name = "BtnPrintGatePass";
        this.BtnPrintGatePass.Size = new System.Drawing.Size(100, 35);
        this.BtnPrintGatePass.TabIndex = 7;
        this.BtnPrintGatePass.Text = "打印出门单";
        this.BtnPrintGatePass.UseVisualStyleBackColor = false;
        this.BtnPrintGatePass.Click += new EventHandler(this.BtnPrintGatePass_Click);

        // AuditForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.Label2);
        this.Controls.Add(this.CboStatus);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.BtnRefresh);
        this.Controls.Add(this.DgvApplications);
        this.Controls.Add(this.LblMultiSelectHint);
        this.Controls.Add(this.BtnAudit);
        this.Controls.Add(this.BtnUnAudit);
        this.Controls.Add(this.BtnPrintGatePass);
        this.Name = "AuditForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "审批管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnAudit = null!;
    private Button BtnUnAudit = null!;
    private Button BtnPrintGatePass = null!;
    private Button BtnRefresh = null!;
    private ComboBox CboStatus = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Label Label1 = null!;
    private Label Label2 = null!;
    private Label LblMultiSelectHint = null!;

    private void LoadStatusOptions()
    {
        CboStatus.Items.Clear();
        CboStatus.Items.Add(new ComboBoxItem("全部", null));
        CboStatus.Items.Add(new ComboBoxItem("待审批", 1));
        CboStatus.Items.Add(new ComboBoxItem("已审批", 2));
        CboStatus.Items.Add(new ComboBoxItem("已拒绝", 3));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void LoadApplications(int? status = null, string searchText = "")
    {
        try
        {
            var allApplications = _service.GetAllApplications();
            _applications = allApplications.FindAll(a => a.Status == 1 || a.Status == 2 || a.Status == 3);

            if (status.HasValue)
            {
                _applications = _applications.FindAll(a => a.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                _applications = _applications.FindAll(a =>
                    (a.ApplicationNo?.Contains(searchText) ?? false) ||
                    (a.OrderNo?.Contains(searchText) ?? false) ||
                    (a.ProcessorName?.Contains(searchText) ?? false) ||
                    (a.ApplicantName?.Contains(searchText) ?? false));
            }

            DgvApplications.DataSource = null;
            DgvApplications.DataSource = _applications;

            if (DgvApplications.Columns.Count > 0)
            {
                DgvApplications.Columns["ApplicationId"].Visible = false;
                DgvApplications.Columns["OrderId"].Visible = false;
                DgvApplications.Columns["ApplicantId"].Visible = false;
                DgvApplications.Columns["ProcessorId"].Visible = false;
                DgvApplications.Columns["OperatorId"].Visible = false;
                DgvApplications.Columns["Status"].Visible = false;
                DgvApplications.Columns["LatestAuditRemark"].Visible = false;

                DgvApplications.Columns["ApplicationNo"].HeaderText = "申请编号";
                DgvApplications.Columns["OrderNo"].HeaderText = "订单编号";
                DgvApplications.Columns["ApplicantName"].HeaderText = "申请人";
                DgvApplications.Columns["ApplicationDate"].HeaderText = "申请日期";
                DgvApplications.Columns["ProcessorName"].HeaderText = "加工商";
                DgvApplications.Columns["ProcessingContent"].HeaderText = "加工内容";
                DgvApplications.Columns["TotalQuantity"].HeaderText = "数量";
                DgvApplications.Columns["ExpectedReturnDate"].HeaderText = "预计归还日期";
                DgvApplications.Columns["StatusText"].HeaderText = "状态";
                DgvApplications.Columns["Remark"].HeaderText = "备注";
                DgvApplications.Columns["OperatorTime"].HeaderText = "操作时间";

                DgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("加载数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnAudit_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要审批的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

        if (application.Status != 1)
        {
            MessageBox.Show("只有待审批的记录才能进行审批", "提示");
            return;
        }

        using var form = new AuditEditForm(application, _currentUser);
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadApplications();
        }
    }

    private void BtnUnAudit_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要反审的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

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
                _auditService.DeleteAuditByApplicationId(application.ApplicationId);

                if (_auditService.UpdateApplicationStatus(application.ApplicationId, 1))
                {
                    MessageBox.Show("反审成功", "提示");
                    LoadApplications();
                }
                else
                {
                    MessageBox.Show("更新申请状态失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("反审失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnPrintGatePass_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要打印出门单的记录", "提示");
            return;
        }

        var selectedApplications = new List<ExternalProcessingApplication>();
        foreach (DataGridViewRow row in DgvApplications.SelectedRows)
        {
            var application = (ExternalProcessingApplication)row.DataBoundItem;
            if (application.Status == 2)
            {
                selectedApplications.Add(application);
            }
        }

        if (selectedApplications.Count == 0)
        {
            MessageBox.Show("请至少选择一条已审批的记录", "提示");
            return;
        }

        var gatePassModel = CreateGatePassModel(selectedApplications);

        try
        {
            var printDoc = new GatePassPrintDocument(gatePassModel);

            var previewDlg = new PrintPreviewDialog();
            previewDlg.Document = printDoc;
            previewDlg.WindowState = FormWindowState.Maximized;

            if (previewDlg.ShowDialog() == DialogResult.OK)
            {
                printDoc.Print();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("打印失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private GatePassPrintModel CreateGatePassModel(List<ExternalProcessingApplication> applications)
    {
        var model = new GatePassPrintModel
        {
            GatePassNo = $"GT-{DateTime.Now:yyyyMMdd}-{new Random().Next(1000, 9999)}",
            PrintDate = DateTime.Now
        };

        foreach (var app in applications)
        {
            // 使用申请中的数量
            model.OrderItems.Add(new GatePassOrderItem
            {
                OrderNo = app.OrderNo ?? app.ApplicationNo ?? "",
                ProcessingContent = app.ProcessingContent ?? "",
                Quantity = app.TotalQuantity > 0 ? app.TotalQuantity : 1,
                Unit = "个"
            });

            var applicantName = app.ApplicantName ?? "未知";
            if (!model.ApplicantNames.Contains(applicantName))
            {
                model.ApplicantNames.Add(applicantName);
            }

            var audits = _auditService.GetAuditsByApplicationId(app.ApplicationId);
            var latestAudit = audits.FirstOrDefault();
            if (latestAudit != null)
            {
                model.AuditorName = latestAudit.AuditorName ?? "";
                model.AuditDate = latestAudit.AuditDate;
            }
        }

        return model;
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        CboStatus.SelectedIndex = 0;
        TxtSearch.Text = "";
        LoadApplications();
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        var selectedItem = CboStatus.SelectedItem as ComboBoxItem;
        var status = selectedItem?.Value as int?;
        var searchText = TxtSearch.Text.Trim();
        LoadApplications(status, searchText);
    }

    private void CboStatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        BtnSearch_Click(sender, e);
    }

    private void DgvApplications_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnAudit_Click(sender, e);
        }
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
