using ExternalProcessing.Models;

namespace ExternalProcessing.Forms;

public partial class MainForm : Form
{
    private readonly User _currentUser;

    public MainForm(User currentUser)
    {
        _currentUser = currentUser;
        InitializeComponent();
        LblUserInfo.Text = $"当前用户：{currentUser.Username}";
        ApplyPermissions();
    }

    private void ApplyPermissions()
    {
        var permissions = _currentUser.GetPermissionList();

        BtnApplication.Enabled = permissions.Contains(PermissionKeys.Application);
        BtnAudit.Enabled = permissions.Contains(PermissionKeys.Audit);
        BtnAcceptance.Enabled = permissions.Contains(PermissionKeys.Acceptance);
        BtnReconciliation.Enabled = permissions.Contains(PermissionKeys.Reconciliation);
        BtnFinanceAudit.Enabled = permissions.Contains(PermissionKeys.FinanceAudit);
        BtnReport.Enabled = permissions.Contains(PermissionKeys.Report);
        BtnUserManagement.Enabled = permissions.Contains(PermissionKeys.UserManagement);
    }

    private void BtnApplication_Click(object sender, EventArgs e)
    {
        using var form = new ApplicationForm(_currentUser);
        form.ShowDialog();
    }

    private void BtnAudit_Click(object sender, EventArgs e)
    {
        using var form = new AuditForm(_currentUser);
        form.ShowDialog();
    }

    private void BtnAcceptance_Click(object sender, EventArgs e)
    {
        using var form = new AcceptanceForm();
        form.ShowDialog();
    }

    private void BtnReconciliation_Click(object sender, EventArgs e)
    {
        using var form = new ReconciliationForm();
        form.ShowDialog();
    }

    private void BtnFinanceAudit_Click(object sender, EventArgs e)
    {
        using var form = new FinanceAuditForm();
        form.ShowDialog();
    }

    private void BtnReport_Click(object sender, EventArgs e)
    {
        using var form = new ReportForm();
        form.ShowDialog();
    }

    private void BtnUserManagement_Click(object sender, EventArgs e)
    {
        using var form = new UserManagementForm();
        form.ShowDialog();
    }

    private void BtnLogout_Click(object sender, EventArgs e)
    {
        this.Close();
    }
}
