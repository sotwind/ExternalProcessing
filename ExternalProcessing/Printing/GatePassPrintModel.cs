using System;
using System.Collections.Generic;

namespace ExternalProcessing.Printing;

public class GatePassPrintModel
{
    // 公司抬头
    public string CompanyName { get; set; } = "森林包装集团股份有限公司";

    // 出门单号
    public string GatePassNo { get; set; } = "";

    // 打印日期
    public DateTime PrintDate { get; set; } = DateTime.Now;

    // 订单明细列表
    public List<GatePassOrderItem> OrderItems { get; set; } = new();

    // 申请人列表（去重后的所有申请人）
    public List<string> ApplicantNames { get; set; } = new();

    // 审批人
    public string AuditorName { get; set; } = "";

    // 审批日期
    public DateTime AuditDate { get; set; }
}

public class GatePassOrderItem
{
    public string OrderNo { get; set; } = "";
    public string ProcessingContent { get; set; } = "";
    public int Quantity { get; set; }
    public string Unit { get; set; } = "个";
}
