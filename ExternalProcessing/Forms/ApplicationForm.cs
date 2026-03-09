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
    private readonly User _currentUser;

    public ApplicationForm(User currentUser)
    {
        _currentUser = currentUser;
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
        this.LblStartDate = new Label();
        this.LblEndDate = new Label();
        this.DtpStartDate = new DateTimePicker();
        this.DtpEndDate = new DateTimePicker();
        this.LabelTo = new Label();
        ((System.ComponentModel.ISupportInitialize)(this.DgvApplications)).BeginInit();
        this.SuspendLayout();

        // LblStartDate
        this.LblStartDate.Location = new System.Drawing.Point(30, 28);
        this.LblStartDate.Name = "LblStartDate";
        this.LblStartDate.Size = new System.Drawing.Size(70, 23);
        this.LblStartDate.TabIndex = 0;
        this.LblStartDate.Text = "开始日期：";

        // DtpStartDate
        this.DtpStartDate.Location = new System.Drawing.Point(100, 26);
        this.DtpStartDate.Name = "DtpStartDate";
        this.DtpStartDate.Size = new System.Drawing.Size(120, 23);
        this.DtpStartDate.TabIndex = 1;
        this.DtpStartDate.Format = DateTimePickerFormat.Short;
        this.DtpStartDate.ShowCheckBox = true;
        this.DtpStartDate.Checked = false;

        // LabelTo
        this.LabelTo.Location = new System.Drawing.Point(225, 28);
        this.LabelTo.Name = "LabelTo";
        this.LabelTo.Size = new System.Drawing.Size(20, 23);
        this.LabelTo.TabIndex = 2;
        this.LabelTo.Text = "至";

        // DtpEndDate
        this.DtpEndDate.Location = new System.Drawing.Point(250, 26);
        this.DtpEndDate.Name = "DtpEndDate";
        this.DtpEndDate.Size = new System.Drawing.Size(120, 23);
        this.DtpEndDate.TabIndex = 3;
        this.DtpEndDate.Format = DateTimePickerFormat.Short;
        this.DtpEndDate.ShowCheckBox = true;
        this.DtpEndDate.Checked = false;

        // Label1
        this.Label1.Location = new System.Drawing.Point(380, 28);
        this.Label1.Name = "Label1";
        this.Label1.Size = new System.Drawing.Size(60, 23);
        this.Label1.TabIndex = 4;
        this.Label1.Text = "状态：";

        // CboStatus
        this.CboStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        this.CboStatus.FormattingEnabled = true;
        this.CboStatus.Location = new System.Drawing.Point(440, 26);
        this.CboStatus.Name = "CboStatus";
        this.CboStatus.Size = new System.Drawing.Size(120, 23);
        this.CboStatus.TabIndex = 5;
        this.CboStatus.SelectedIndexChanged += new EventHandler(this.CboStatus_SelectedIndexChanged);

        // Label2
        this.Label2.Location = new System.Drawing.Point(570, 28);
        this.Label2.Name = "Label2";
        this.Label2.Size = new System.Drawing.Size(60, 23);
        this.Label2.TabIndex = 6;
        this.Label2.Text = "搜索：";

        // TxtSearch
        this.TxtSearch.Location = new System.Drawing.Point(630, 26);
        this.TxtSearch.Name = "TxtSearch";
        this.TxtSearch.Size = new System.Drawing.Size(180, 23);
        this.TxtSearch.TabIndex = 7;

        // BtnSearch
        this.BtnSearch.Location = new System.Drawing.Point(820, 23);
        this.BtnSearch.Name = "BtnSearch";
        this.BtnSearch.Size = new System.Drawing.Size(75, 30);
        this.BtnSearch.TabIndex = 8;
        this.BtnSearch.Text = "搜索";
        this.BtnSearch.Click += new EventHandler(this.BtnSearch_Click);

        // BtnRefresh
        this.BtnRefresh.Location = new System.Drawing.Point(900, 23);
        this.BtnRefresh.Name = "BtnRefresh";
        this.BtnRefresh.Size = new System.Drawing.Size(75, 30);
        this.BtnRefresh.TabIndex = 9;
        this.BtnRefresh.Text = "重置";
        this.BtnRefresh.Click += new EventHandler(this.BtnRefresh_Click);

        // DgvApplications
        this.DgvApplications.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        this.DgvApplications.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        this.DgvApplications.Location = new System.Drawing.Point(30, 70);
        this.DgvApplications.Name = "DgvApplications";
        this.DgvApplications.Size = new System.Drawing.Size(940, 480);
        this.DgvApplications.TabIndex = 10;
        this.DgvApplications.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        this.DgvApplications.MultiSelect = false;
        this.DgvApplications.ReadOnly = true;
        this.DgvApplications.CellDoubleClick += new DataGridViewCellEventHandler(this.DgvApplications_CellDoubleClick);

        // BtnAdd
        this.BtnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnAdd.BackColor = System.Drawing.Color.FromArgb(144, 238, 144);
        this.BtnAdd.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
        this.BtnAdd.Location = new System.Drawing.Point(990, 70);
        this.BtnAdd.Name = "BtnAdd";
        this.BtnAdd.Size = new System.Drawing.Size(100, 35);
        this.BtnAdd.TabIndex = 11;
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
        this.BtnEdit.TabIndex = 12;
        this.BtnEdit.Text = "编辑";
        this.BtnEdit.UseVisualStyleBackColor = false;
        this.BtnEdit.Click += new EventHandler(this.BtnEdit_Click);

        // BtnDelete
        this.BtnDelete.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        this.BtnDelete.Location = new System.Drawing.Point(990, 170);
        this.BtnDelete.Name = "BtnDelete";
        this.BtnDelete.Size = new System.Drawing.Size(100, 35);
        this.BtnDelete.TabIndex = 13;
        this.BtnDelete.Text = "删除";
        this.BtnDelete.Click += new EventHandler(this.BtnDelete_Click);

        // ApplicationForm
        this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(1120, 600);
        this.Controls.Add(this.LblStartDate);
        this.Controls.Add(this.DtpStartDate);
        this.Controls.Add(this.LabelTo);
        this.Controls.Add(this.DtpEndDate);
        this.Controls.Add(this.Label1);
        this.Controls.Add(this.CboStatus);
        this.Controls.Add(this.Label2);
        this.Controls.Add(this.TxtSearch);
        this.Controls.Add(this.BtnSearch);
        this.Controls.Add(this.BtnRefresh);
        this.Controls.Add(this.DgvApplications);
        this.Controls.Add(this.BtnAdd);
        this.Controls.Add(this.BtnEdit);
        this.Controls.Add(this.BtnDelete);
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
    private Label LblStartDate = null!;
    private Label LblEndDate = null!;
    private DateTimePicker DtpStartDate = null!;
    private DateTimePicker DtpEndDate = null!;
    private Label LabelTo = null!;

    private void LoadApplications(int? status = null, string searchText = "", DateTime? startDate = null, DateTime? endDate = null)
    {
        try
        {
            _applications = _service.GetAllApplications(status, null, startDate, endDate);

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
                DgvApplications.Columns["TotalQuantity"].HeaderText = "数量";
                DgvApplications.Columns["ExpectedReturnDate"].HeaderText = "预计归还日期";
                DgvApplications.Columns["StatusText"].HeaderText = "状态";
                DgvApplications.Columns["Remark"].HeaderText = "备注";
                DgvApplications.Columns["OperatorTime"].HeaderText = "操作时间";
                
                // 添加审批意见列
                if (DgvApplications.Columns.Contains("LatestAuditRemark"))
                {
                    DgvApplications.Columns["LatestAuditRemark"].HeaderText = "审批意见";
                    DgvApplications.Columns["LatestAuditRemark"].DisplayIndex = 8;
                }

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
        CboStatus.Items.Add(new ComboBoxItem("已验收", 4));
        CboStatus.Items.Add(new ComboBoxItem("已对账", 5));
        CboStatus.Items.Add(new ComboBoxItem("已财务审核", 6));
        CboStatus.DisplayMember = "Text";
        CboStatus.ValueMember = "Value";
        CboStatus.SelectedIndex = 0;
    }

    private void BtnAdd_Click(object sender, EventArgs e)
    {
        using var form = new ApplicationEditForm(_currentUser);
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
        
        // 检查状态：只有待审批(1)或已拒绝(3)才能编辑
        if (application.Status != 1 && application.Status != 3)
        {
            MessageBox.Show("只有待审批或已拒绝的记录才能编辑，如需修改请先反审", "提示");
            return;
        }
        
        using var form = new ApplicationEditForm(application, _currentUser);
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

        var application = (ExternalProcessingApplication)DgvApplications.SelectedRows[0].DataBoundItem;
        
        // 检查状态：只有待审批(1)或已拒绝(3)才能删除
        if (application.Status != 1 && application.Status != 3)
        {
            MessageBox.Show("只有待审批或已拒绝的记录才能删除，如需删除请先反审", "提示");
            return;
        }

        if (MessageBox.Show("确定要删除选中的记录吗？", "确认", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
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
        // 重置搜索条件
        CboStatus.SelectedIndex = 0;
        TxtSearch.Text = "";
        DtpStartDate.Checked = false;
        DtpEndDate.Checked = false;
        // 重新加载数据
        LoadApplications();
    }

    private void BtnSearch_Click(object sender, EventArgs e)
    {
        var selectedItem = CboStatus.SelectedItem as ComboBoxItem;
        var status = selectedItem?.Value as int?;
        var searchText = TxtSearch.Text.Trim();
        var startDate = DtpStartDate.Checked ? DtpStartDate.Value : (DateTime?)null;
        var endDate = DtpEndDate.Checked ? DtpEndDate.Value : (DateTime?)null;
        LoadApplications(status, searchText, startDate, endDate);
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
