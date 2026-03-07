using System;

namespace ExternalProcessing.Models;

public class ExternalProcessingAudit
{
    public int AuditId { get; set; }
    public int ApplicationId { get; set; }
    public int AuditorId { get; set; }
    public string? AuditorName { get; set; }
    public DateTime AuditDate { get; set; }
    public int AuditResult { get; set; }  // 1:已通过, 2:已拒绝
    public string? AuditRemark { get; set; }
    public int OperatorId { get; set; }
    public DateTime OperatorTime { get; set; }

    public string AuditResultText => AuditResult switch
    {
        1 => "已通过",
        2 => "已拒绝",
        _ => "未知"
    };
}
