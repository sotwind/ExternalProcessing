using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Services;
using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class ReconciliationForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private List<ExternalProcessingApplication> _applications = new();

    public ReconciliationForm()
    {
        InitializeComponent();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnReconcile = new Button();
        this.BtnRefresh = new Button();
        this.TxtSearch = new TextBox();
        this.BtnSearch = new Button();
        this.Label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).BeginInit();
        this.SuspendLayout();

        this.Label1.Location = new System.Drawing.Point(30, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 0;
        this.Label1.Text = "搜索：";

        this.TxtSearch.Location = new System.Drawing.Point(90, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(233, 23);
        this.TxtSearch.TabIndex = 1;

        this.BtnSearch.Location = new System.Drawing.Point(340, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(93, 30);
        this.BtnSearch.TabIndex = 2;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        this.DgvApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvApplications.Location = new System.Drawing.Point(30, 70);
        this.DgvApplications.Name = "DgvApplications";
        this.DgvApplications.Size = new System.Drawing.Size(940, 480);
        this.DgvApplications.TabIndex = 3;

        this.BtnReconcile.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnReconcile.BackColor = System.Drawing.Color.FromArgb(108, 117, 125);
        this.BtnReconcile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnReconcile.ForeColor = Color.White;
        this.BtnReconcile.Location = new System.Drawing.Point(990, 70);
        this.BtnReconcile.Name = "BtnReconcile";
        this.BtnReconcile.Size = new System.Drawing.Size(100, 35);
        this.BtnReconcile.TabIndex = 4;
        this.BtnReconcile.Text = "对账";
        this.BtnReconcile.UseVisualStyleBackColor = false;
        this.BtnReconcile.Click += new EventHandler(this.BtnReconcile_Click);

        this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnRefresh.Location = new System.Drawing.Point(990, 120);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(100, 35);
        this.BtnRefresh.TabIndex = 5;
        this.BtnRefresh.Text = "刷新";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.DgvApplications);
        this.Controls.Add(this.BtnReconcile);
        this.Controls.Add(this.BtnRefresh);
        this.Name = "ReconciliationForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "对账管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnReconcile = null!;
    private Button BtnRefresh = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Label Label1 = null!;

    private void LoadApplications(string searchText = "")
    {
        try
        {
            _applications = _service.GetAllApplications(5);

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
                DgvApplications.Columns["ApplicationNo"].HeaderText = "申请编号";
                DgvApplications.Columns["OrderNo"].HeaderText = "订单编号";
                DgvApplications.Columns["ProcessorName"].HeaderText = "加工商";
                DgvApplications.Columns["ProcessingContent"].HeaderText = "加工内容";
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
        MessageBox.Show("对账功能开发中...", "提示");
    }

    private void BtnRefresh_Click(object sender, EventArgs e)
    {
        LoadApplications();
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        LoadApplications(TxtSearch.Text.Trim());
    }
}
