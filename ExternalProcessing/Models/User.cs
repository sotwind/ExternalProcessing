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
    public const string Application = "申请管理";
    public const string Audit = "审批管理";
    public const string Acceptance = "验收管理";
    public const string Reconciliation = "对账管理";
    public const string FinanceAudit = "财务审核";
    public const string Report = "统计报表";
    public const string UserManagement = "用户管理";
}
