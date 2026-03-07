using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Services;
using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class ReportForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private List<ExternalProcessingApplication> _applications = new();

    public ReportForm()
    {
        InitializeComponent();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnExport = new Button();
        this.BtnRefresh = new Button();
        this.TxtSearch = new TextBox();
        this.BtnSearch = new Button();
        this.Label1 = new Label();
        this.CboStatus = new ComboBox();
        this.Label2 = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).BeginInit();
        this.SuspendLayout();

        this.Label1.Location = new System.Drawing.Point(30, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 0;
        this.Label1.Text = "状态：";

        this.Label2.Location = new System.Drawing.Point(220, 28);
        this.Label2.Name = "Label2";
        this.Label2.Size = new System.Drawing.Size(60, 23);
        this.Label2.TabIndex = 1;
        this.Label2.Text = "搜索：";

        this.CboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboStatus.FormattingEnabled = true;
        this.CboStatus.Location = new System.Drawing.Point(90, 26);
        this.CboStatus.Name = "CboStatus";
        this.CboStatus.Size = new System.Drawing.Size(120, 23);
        this.CboStatus.TabIndex = 2;
        this.CboStatus.SelectedIndexChanged += new EventHandler(this.CboStatus_SelectedIndexChanged);

        this.TxtSearch.Location = new System.Drawing.Point(280, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(200, 23);
        this.TxtSearch.TabIndex = 3;

        this.BtnSearch.Location = new System.Drawing.Point(490, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(93, 30);
        this.BtnSearch.TabIndex = 4;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        this.DgvApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvApplications.Location = new System.Drawing.Point(30, 70);
        this.DgvApplications.Name = "DgvApplications";
        this.DgvApplications.Size = new System.Drawing.Size(940, 480);
        this.DgvApplications.TabIndex = 5;

        this.BtnExport.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnExport.BackColor = System.Drawing.Color.FromArgb(220, 53, 69);
        this.BtnExport.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnExport.ForeColor = Color.White;
        this.BtnExport.Location = new System.Drawing.Point(990, 70);
        this.BtnExport.Name = "BtnExport";
        this.BtnExport.Size = new System.Drawing.Size(100, 35);
        this.BtnExport.TabIndex = 6;
        this.BtnExport.Text = "导出";
        this.BtnExport.UseVisualStyleBackColor = false;
        this.BtnExport.Click += new EventHandler(this.BtnExport_Click);

        this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnRefresh.Location = new System.Drawing.Point(990, 120);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(100, 35);
        this.BtnRefresh.TabIndex = 7;
        this.BtnRefresh.Text = "刷新";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.Label2);
        this.Controls.Add(this.CboStatus);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.DgvApplications);
        this.Controls.Add(this.BtnExport);
        this.Controls.Add(this.BtnRefresh);
        this.Name = "ReportForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "统计报表";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

        LoadStatusOptions();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnExport = null!;
    private Button BtnRefresh = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Label Label1 = null!;
    private Label Label2 = null!;
    private ComboBox CboStatus = null!;

    private void LoadStatusOptions()
    {
        CboStatus.Items.Clear();
        CboStatus.Items.Add(new ComboBoxItem("全部", null));
        CboStatus.Items.Add(new ComboBoxItem("待审批", 1));
        CboStatus.Items.Add(new ComboBoxItem("已审批", 2));
        CboStatus.Items.Add(new ComboBoxItem("已拒绝", 3));
        CboStatus.Items.Add(new ComboBoxItem("已外发", 4));
        CboStatus.Items.Add(new ComboBoxItem("已验收", 5));
        CboStatus.Items.Add(new ComboBoxItem("已对账", 6));
        CboStatus.Items.Add(new ComboBoxItem("已财务审核", 7));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void LoadApplications(int? status = null, string searchText = "")
    {
        try
        {
            _applications = _service.GetAllApplications(status);

            if (!string.IsNullOrEmpty(searchText))
            {
                _applications = _applications.FindAll(a =>
                    (a.ApplicationNo?.Contains(searchText) ?? false) ||
                    (a.OrderNo?.Contains(searchText) ?? false) ||
                    (a.ProcessorName?.Contains(searchText) ?? false));
            }

            DgvApplications.DataSource = null;
            DgvApplications.DataSource = _applications;

            if (DgvApplications.Columns.Count > 0)
            {
                DgvApplications.Columns["ApplicationNo"].HeaderText = "申请编号";
                DgvApplications.Columns["OrderNo"].HeaderText = "订单编号";
                DgvApplications.Columns["ApplicantName"].HeaderText = "申请人";
                DgvApplications.Columns["ApplicationDate"].HeaderText = "申请日期";
                DgvApplications.Columns["ProcessorName"].HeaderText = "加工商";
                DgvApplications.Columns["ProcessingContent"].HeaderText = "加工内容";
                DgvApplications.Columns["Status"].HeaderText = "状态";
                DgvApplications.Columns["ExpectedReturnDate"].HeaderText = "预计归还日期";
                DgvApplications.Columns["OperatorTime"].HeaderText = "操作时间";
                DgvApplications.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("加载数据失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnExport_Click(object sender, EventArgs e)
    {
        MessageBox.Show("导出功能开发中...", "提示");
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
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
