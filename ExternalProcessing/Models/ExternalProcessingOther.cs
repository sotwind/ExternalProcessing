using System;

namespace ExternalProcessing.Models;

public class ExternalProcessingReconciliation
{
    public int ReconciliationId { get; set; }
    public int ApplicationId { get; set; }
    public DateTime ReconciliationDate { get; set; }
    public decimal ReconciliationAmount { get; set; }
    public string? ReconciliationRemark { get; set; }
    public int Status { get; set; }  // 1:待财务审核, 2:已财务审核
    public int OperatorId { get; set; }
    public DateTime OperatorTime { get; set; }

    public string StatusText => Status switch
    {
        1 => "待财务审核",
        2 => "已财务审核",
        _ => "未知"
    };
}

public class ExternalProcessingFinanceAudit
{
    public int FinanceAuditId { get; set; }
    public int ReconciliationId { get; set; }
    public int FinanceAuditorId { get; set; }
    public string? FinanceAuditorName { get; set; }
    public DateTime AuditDate { get; set; }
    public int AuditResult { get; set; }  // 1:已通过, 2:已拒绝
    public string? AuditRemark { get; set; }
    public int PaymentStatus { get; set; }  // 1:已开票, 2:已付款
    public int OperatorId { get; set; }
    public DateTime OperatorTime { get; set; }

    public string AuditResultText => AuditResult switch
    {
        1 => "已通过",
        2 => "已拒绝",
        _ => "未知"
    };

    public string PaymentStatusText => PaymentStatus switch
    {
        1 => "已开票",
        2 => "已付款",
        _ => "未处理"
    };
}
