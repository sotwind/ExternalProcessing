namespace ExternalProcessing.Models;

public class User
{
    public int UserID { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public DateTime CreateTime { get; set; }

    public List<string> GetPermissionList()
    {
        if (string.IsNullOrEmpty(Permissions))
            return new List<string>();
        
        return Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries)
                         .Select(p => p.Trim())
                         .ToList();
    }

    public bool HasPermission(string permission)
    {
        return GetPermissionList().Contains(permission);
    }
}

public static class PermissionKeys
{
    public const string Application = "外发申请";
    public const string Audit = "外发审批";
    public const string Acceptance = "外发验收";
    public const string Reconciliation = "外发对账";
    public const string FinanceAudit = "财务审核";
    public const string Report = "统计报表";
}
