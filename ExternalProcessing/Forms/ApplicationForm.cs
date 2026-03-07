using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ExternalProcessing.Services;
using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class ApplicationForm : Form
{
    private readonly ExternalProcessingService _service = new();
    private List<ExternalProcessingApplication> _applications = new();

    public ApplicationForm()
    {
        InitializeComponent();
        LoadStatusOptions();
        LoadApplications();
    }

    private void InitializeComponent()
    {
        this.DgvApplications = new DataGridView();
        this.BtnAdd = new Button();
        this.BtnEdit = new Button();
        this.BtnDelete = new Button();
        this.BtnRefresh = new Button();
        this.CboStatus = new ComboBox();
        this.TxtSearch = new TextBox();
        this.BtnSearch = new Button();
        this.Label1 = new Label();
        this.Label2 = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).BeginInit();
        this.SuspendLayout();

        // Label1
        this.Label1.Location = new System.Drawing.Point(30, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 0;
        this.Label1.Text = "状态：";

        // Label2
        this.Label2.Location = new System.Drawing.Point(310, 28);
        this.Label2.Name = "Label2";
        this.Label2.Size = new System.Drawing.Size(60, 23);
        this.Label2.TabIndex = 1;
        this.Label2.Text = "搜索：";

        // CboStatus
        this.CboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboStatus.FormattingEnabled = true;
        this.CboStatus.Location = new System.Drawing.Point(90, 26);
        this.CboStatus.Name = "CboStatus";
        this.CboStatus.Size = new System.Drawing.Size(150, 23);
        this.CboStatus.TabIndex = 2;
        this.CboStatus.SelectedIndexChanged += new EventHandler(this.CboStatus_SelectedIndexChanged);

        // TxtSearch
        this.TxtSearch.Location = new System.Drawing.Point(370, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(233, 23);
        this.TxtSearch.TabIndex = 3;

        // BtnSearch
        this.BtnSearch.Location = new System.Drawing.Point(620, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(93, 30);
        this.BtnSearch.TabIndex = 4;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        // DgvApplications
        this.DgvApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvApplications.Location = new System.Drawing.Point(30, 70);
        this.DgvApplications.Name = "DgvApplications";
        this.DgvApplications.Size = new System.Drawing.Size(940, 480);
        this.DgvApplications.TabIndex = 5;
        this.DgvApplications.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvApplications_CellDoubleClick);

        // BtnAdd
        this.BtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnAdd.BackColor = System.Drawing.Color.FromArgb(144, 238, 144);
        this.BtnAdd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnAdd.Location = new System.Drawing.Point(990, 70);
        this.BtnAdd.Name = "BtnAdd";
        this.BtnAdd.Size = new System.Drawing.Size(100, 35);
        this.BtnAdd.TabIndex = 6;
        this.BtnAdd.Text = "新增";
        this.BtnAdd.UseVisualStyleBackColor = false;
        this.BtnAdd.Click += new EventHandler(this.BtnAdd_Click);

        // BtnEdit
        this.BtnEdit.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnEdit.BackColor = System.Drawing.Color.FromArgb(255, 255, 153);
        this.BtnEdit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnEdit.Location = new System.Drawing.Point(990, 120);
        this.BtnEdit.Name = "BtnEdit";
        this.BtnEdit.Size = new System.Drawing.Size(100, 35);
        this.BtnEdit.TabIndex = 7;
        this.BtnEdit.Text = "编辑";
        this.BtnEdit.UseVisualStyleBackColor = false;
        this.BtnEdit.Click += new EventHandler(this.BtnEdit_Click);

        // BtnDelete
        this.BtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnDelete.Location = new System.Drawing.Point(990, 170);
        this.BtnDelete.Name = "BtnDelete";
        this.BtnDelete.Size = new System.Drawing.Size(100, 35);
        this.BtnDelete.TabIndex = 8;
        this.BtnDelete.Text = "删除";
        this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);

        // BtnRefresh
        this.BtnRefresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnRefresh.Location = new System.Drawing.Point(990, 220);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(100, 35);
        this.BtnRefresh.TabIndex = 9;
        this.BtnRefresh.Text = "刷新";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        // ApplicationForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.Label2);
        this.Controls.Add(this.CboStatus);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.DgvApplications);
        this.Controls.Add(this.BtnAdd);
        this.Controls.Add(this.BtnEdit);
        this.Controls.Add(this.BtnDelete);
        this.Controls.Add(this.BtnRefresh);
        this.Name = "ApplicationForm";
        this.StartPosition = FormStartPosition.CenterScreen;
        this.Text = "申请管理";
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();
    }

    private DataGridView DgvApplications = null!;
    private Button BtnAdd = null!;
    private Button BtnEdit = null!;
    private Button BtnDelete = null!;
    private Button BtnRefresh = null!;
    private ComboBox CboStatus = null!;
    private TextBox TxtSearch = null!;
    private Button BtnSearch = null!;
    private Label Label1 = null!;
    private Label Label2 = null!;

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
                    (a.ProcessorName?.Contains(searchText) ?? false) ||
                    (a.ApplicantName?.Contains(searchText) ?? false));
            }

            DgvApplications.DataSource = null;
            DgvApplications.DataSource = _applications;

            if (DgvApplications.Columns.Count > 0)
            {
                // 隐藏不需要显示的列
                DgvApplications.Columns["ApplicationId"].Visible = false;
                DgvApplications.Columns["OrderId"].Visible = false;
                DgvApplications.Columns["ApplicantId"].Visible = false;
                DgvApplications.Columns["ProcessorId"].Visible = false;
                DgvApplications.Columns["OperatorId"].Visible = false;
                DgvApplications.Columns["Status"].Visible = false;
                
                // 设置列标题
                DgvApplications.Columns["ApplicationNo"].HeaderText = "申请编号";
                DgvApplications.Columns["OrderNo"].HeaderText = "订单编号";
                DgvApplications.Columns["ApplicantName"].HeaderText = "申请人";
                DgvApplications.Columns["ApplicationDate"].HeaderText = "申请日期";
                DgvApplications.Columns["ProcessorName"].HeaderText = "加工商";
                DgvApplications.Columns["ProcessingContent"].HeaderText = "加工内容";
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

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        using var form = new ApplicationEditForm();
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadApplications();
        }
    }

    private void BtnEdit_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要编辑的记录", "提示");
            return;
        }

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
        using var form = new ApplicationEditForm(application);
        if (form.ShowDialog() == DialogResult.OK)
        {
            LoadApplications();
        }
    }

    private void BtnDelete_Click(object sender, EventArgs e)
    {
        if (DgvApplications.SelectedRows.Count == 0)
        {
            MessageBox.Show("请选择要删除的记录", "提示");
            return;
        }

        if (MessageBox.Show("确定要删除选中的记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
            try
            {
                if (_service.DeleteApplication(application.ApplicationId))
                {
                    MessageBox.Show("删除成功", "提示");
                    LoadApplications();
                }
                else
                {
                    MessageBox.Show("删除失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("删除失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
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

    private void DgvApplications_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            BtnEdit_Click(sender, e);
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
