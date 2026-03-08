using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Services;
using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class ReconciliationForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private readonly ExternalProcessingReconciliationService _reconciliationService = new();
    private List<ExternalProcessingApplication> _applications = new();

    public ReconciliationForm()
    {
        InitializeComponent();
        LoadStatusOptions();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnReconcile = new Button();
        this.BtnUnReconcile = new Button();
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

        // 对账按钮
        this.BtnReconcile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnReconcile.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
        this.BtnReconcile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnReconcile.ForeColor = System.Drawing.Color.White;
        this.BtnReconcile.Location = new System.Drawing.Point(990, 70);
        this.BtnReconcile.Name = "BtnReconcile";
        this.BtnReconcile.Size = new System.Drawing.Size(100, 35);
        this.BtnReconcile.TabIndex = 5;
        this.BtnReconcile.Text = "对账";
        this.BtnReconcile.UseVisualStyleBackColor = false;
        this.BtnReconcile.Click += new EventHandler(this.BtnReconcile_Click);

        // 反审按钮
        this.BtnUnReconcile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnUnReconcile.BackColor = System.Drawing.Color.FromArgb(255, 99, 71);
        this.BtnUnReconcile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnUnReconcile.ForeColor = System.Drawing.Color.White;
        this.BtnUnReconcile.Location = new System.Drawing.Point(990, 120);
        this.BtnUnReconcile.Name = "BtnUnReconcile";
        this.BtnUnReconcile.Size = new System.Drawing.Size(100, 35);
        this.BtnUnReconcile.TabIndex = 6;
        this.BtnUnReconcile.Text = "反审";
        this.BtnUnReconcile.UseVisualStyleBackColor = false;
        this.BtnUnReconcile.Click += new EventHandler(this.BtnUnReconcile_Click);

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
        this.Controls.Add(this.BtnReconcile);
        this.Controls.Add(this.BtnUnReconcile);
        this.Name = "ReconciliationForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "对账管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnReconcile = null!;
    private Button BtnUnReconcile = null!;
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
        CboStatus.Items.Add(new ComboBoxItem("已验收", 4));
        CboStatus.Items.Add(new ComboBoxItem("已对账", 5));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void LoadApplications(int? status = null, string searchText = "")
    {
        try
        {
            var allApplications = _service.GetAllApplications();
            _applications = allApplications.FindAll(a => a.Status == 4 || a.Status == 5);

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

    private void BtnReconcile_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要对账的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

        if (application.Status != 4)
        {
            MessageBox.Show("只有已验收的记录才能进行对账", "提示");
            return;
        }

        if (MessageBox.Show("确定要对账选中的记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                var reconciliation = new ExternalProcessingReconciliation
                {
                    ApplicationId = application.ApplicationId,
                    ReconciliationAmount = 0,
                    Status = 1,
                    OperatorId = 1
                };

                var reconciliationId = _reconciliationService.AddReconciliation(reconciliation);
                if (reconciliationId > 0)
                {
                    if (_reconciliationService.UpdateApplicationStatus(application.ApplicationId, 5))
                    {
                        MessageBox.Show("对账成功", "提示");
                        LoadApplications();
                    }
                    else
                    {
                        MessageBox.Show("更新申请状态失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("添加对账记录失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("对账失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void BtnUnReconcile_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要反审的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;

        if (application.Status != 5)
        {
            MessageBox.Show("只有已对账的记录才能反审", "提示");
            return;
        }

        if (MessageBox.Show("确定要反审选中的记录吗？反审后将回到已验收状态。", "确认",
            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            try
            {
                _reconciliationService.DeleteReconciliationByApplicationId(application.ApplicationId);

                if (_reconciliationService.UpdateApplicationStatus(application.ApplicationId, 4))
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
