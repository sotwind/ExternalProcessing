using System;

namespace ExternalProcessing.Models;

public class ExternalProcessingApplication
{
    public int ApplicationId { get; set; }
    public string? ApplicationNo { get; set; }
    public int? OrderId { get; set; }
    public string? OrderNo { get; set; }
    public int? ApplicantId { get; set; }
    public string? ApplicantName { get; set; }
    public DateTime ApplicationDate { get; set; }
    public int? ProcessorId { get; set; }
    public string? ProcessorName { get; set; }
    public string? ProcessingContent { get; set; }
    public DateTime ExpectedReturnDate { get; set; }
    public int Status { get; set; }  // 1:待审批, 2:已审批, 3:已拒绝, 4:已验收, 5:已对账, 6:已财务审核
    public string? Remark { get; set; }
    public int TotalQuantity { get; set; }  // 总数量
    public int OperatorId { get; set; }
    public DateTime OperatorTime { get; set; }

    public string StatusText => Status switch
    {
        1 => "待审批",
        2 => "已审批",
        3 => "已拒绝",
        4 => "已验收",
        5 => "已对账",
        6 => "已财务审核",
        _ => "未知"
    };

    // 用于显示最新审批/拒绝理由（不映射到数据库）
    public string? LatestAuditRemark { get; set; }
}

public class ExternalProcessingApplicationDetail
{
    public int DetailId { get; set; }
    public int ApplicationId { get; set; }
    public int ItemId { get; set; }
    public string? ItemName { get; set; }
    public string? Specification { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalAmount { get; set; }
    public string? Remark { get; set; }
    public int OperatorId { get; set; }
    public DateTime OperatorTime { get; set; }
}
