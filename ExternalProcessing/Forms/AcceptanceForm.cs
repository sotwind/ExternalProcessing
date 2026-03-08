using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Services;
using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class AcceptanceForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private readonly ExternalProcessingAcceptanceService _acceptanceService = new();
    private List<ExternalProcessingApplication> _applications = new();

    public AcceptanceForm()
    {
        InitializeComponent();
        LoadStatusOptions();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnAccept = new Button();
        this.BtnUnAccept = new Button();
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
        this.DgvApplications.Size = new System.Drawing.Size(940, 480);
        this.DgvApplications.TabIndex = 4;
        this.DgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.DgvApplications.MultiSelect = false;
        this.DgvApplications.ReadOnly = true;

        // 验收按钮
        this.BtnAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnAccept.BackColor = System.Drawing.Color.FromArgb(144, 238, 144);
        this.BtnAccept.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnAccept.Location = new System.Drawing.Point(990, 70);
        this.BtnAccept.Name = "BtnAccept";
        this.BtnAccept.Size = new System.Drawing.Size(100, 35);
        this.BtnAccept.TabIndex = 6;
        this.BtnAccept.Text = "验收";
        this.BtnAccept.UseVisualStyleBackColor = false;
        this.BtnAccept.Click += new EventHandler(this.BtnAccept_Click);

        // 反审按钮
        this.BtnUnAccept.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnUnAccept.BackColor = System.Drawing.Color.FromArgb(255, 99, 71);
        this.BtnUnAccept.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnUnAccept.ForeColor = System.Drawing.Color.White;
        this.BtnUnAccept.Location = new System.Drawing.Point(990, 120);
        this.BtnUnAccept.Name = "BtnUnAccept";
        this.BtnUnAccept.Size = new System.Drawing.Size(100, 35);
        this.BtnUnAccept.TabIndex = 7;
        this.BtnUnAccept.Text = "反审";
        this.BtnUnAccept.UseVisualStyleBackColor = false;
        this.BtnUnAccept.Click += new EventHandler(this.BtnUnAccept_Click);

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
        this.Controls.Add(this.BtnAccept);
        this.Controls.Add(this.BtnUnAccept);
        this.Name = "AcceptanceForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "验收管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnAccept = null!;
    private Button BtnUnAccept = null!;
    private Button BtnRefresh = null!;
    private ComboBox CboStatus = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Label Label1 = null!;
    private Label Label2 = null!;

    private void LoadStatusOptions()
    {
        CboStatus.Items.Clear();
        CboStatus.Items.Add(new ComboBoxItem("全部", null));
        CboStatus.Items.Add(new ComboBoxItem("已审批", 2));
        CboStatus.Items.Add(new ComboBoxItem("已验收", 4));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void LoadApplications(int? status = null, string searchText = "")
    {
        try
        {
            var allApplications = _service.GetAllApplications();
            _applications = allApplications.FindAll(a => a.Status == 2 || a.Status == 4);

            if (status.HasValue)
            {
                _applications = _applications.FindAll(a => a.Status == status.Value);
            }

            if (!string.IsNullOrEmpty(searchText))
            {
                _applications = _applications.FindAll(a =>
                    (a.ApplicationNo?.Contains(searchText) ?? false) ||
                    (a.OrderNo?.Contains(searchText) ?? false));
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

    private void BtnAccept_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要验收的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

        if (application.Status != 2)
        {
            MessageBox.Show("只有已审批的记录才能进行验收", "提示");
            return;
        }

        if (MessageBox.Show("确定要验收选中的记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                var acceptance = new ExternalProcessingAcceptance
                {
                    ApplicationId = application.ApplicationId,
                    AcceptanceResult = 1,
                    AcceptanceRemark = "",
                    OperatorId = 1
                };

                var acceptanceId = _acceptanceService.AddAcceptance(acceptance);
                if (acceptanceId > 0)
                {
                    if (_acceptanceService.UpdateApplicationStatus(application.ApplicationId, 4))
                    {
                        MessageBox.Show("验收成功", "提示");
                        LoadApplications();
                    }
                    else
                    {
                        MessageBox.Show("更新申请状态失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("添加验收记录失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("验收失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnUnAccept_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要反审的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

        if (application.Status != 4)
        {
            MessageBox.Show("只有已验收的记录才能反审", "提示");
            return;
        }

        if (MessageBox.Show("确定要反审选中的记录吗？反审后将回到已审批状态。", "确认",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                _acceptanceService.DeleteAcceptanceByApplicationId(application.ApplicationId);

                if (_acceptanceService.UpdateApplicationStatus(application.ApplicationId, 2))
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
