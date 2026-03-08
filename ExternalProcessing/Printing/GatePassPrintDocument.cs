using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Linq;

namespace ExternalProcessing.Printing;

public class GatePassPrintDocument : PrintDocument
{
    private readonly GatePassPrintModel _model;

    public GatePassPrintDocument(GatePassPrintModel model)
    {
        _model = model;
        DocumentName = $"出门单_{_model.GatePassNo}";
    }

    protected override void OnPrintPage(PrintPageEventArgs e)
    {
        base.OnPrintPage(e);

        var g = e.Graphics;
        var pageBounds = e.PageBounds;

        // 设置字体
        var companyFont = new Font("宋体", 16, FontStyle.Bold);
        var titleFont = new Font("宋体", 14, FontStyle.Bold);
        var headerFont = new Font("宋体", 11, FontStyle.Bold);
        var normalFont = new Font("宋体", 10);
        var smallFont = new Font("宋体", 9);

        // 打印公司抬头
        var companyTitle = _model.CompanyName;
        var companySize = g.MeasureString(companyTitle, companyFont);
        g.DrawString(companyTitle, companyFont, Brushes.Black,
            (pageBounds.Width - companySize.Width) / 2, 30);

        // 打印标题
        var title = "外发加工出门单";
        var titleSize = g.MeasureString(title, titleFont);
        g.DrawString(title, titleFont, Brushes.Black,
            (pageBounds.Width - titleSize.Width) / 2, 55);

        // 打印日期和出门单号
        g.DrawString($"日期：{_model.PrintDate:yyyy-MM-dd}", normalFont, Brushes.Black, 50, 90);
        g.DrawString($"出门单号：{_model.GatePassNo}", normalFont, Brushes.Black, 400, 90);

        // 打印分隔线
        g.DrawLine(Pens.Black, 50, 115, pageBounds.Width - 50, 115);

        // 打印表头
        var y = 130;
        var lineHeight = 25;

        g.DrawString("加工订单号", headerFont, Brushes.Black, 50, y);
        g.DrawString("加工工艺", headerFont, Brushes.Black, 250, y);
        g.DrawString("数量", headerFont, Brushes.Black, 450, y);
        y += lineHeight;

        // 打印分隔线
        g.DrawLine(Pens.Black, 50, y - 5, pageBounds.Width - 50, y - 5);

        // 打印订单明细
        foreach (var item in _model.OrderItems)
        {
            g.DrawString(item.OrderNo, normalFont, Brushes.Black, 50, y);
            g.DrawString(item.ProcessingContent, normalFont, Brushes.Black, 250, y);
            g.DrawString($"{item.Quantity} {item.Unit}", normalFont, Brushes.Black, 450, y);
            y += lineHeight;
        }

        // 打印分隔线
        y += 10;
        g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
        y += 15;

        // 打印申请人（多个申请人用顿号分隔，去重）
        var applicants = string.Join("、", _model.ApplicantNames.Distinct());
        g.DrawString($"申请人：{applicants}", headerFont, Brushes.Black, 50, y);

        // 打印审批人
        g.DrawString($"审批人：{_model.AuditorName}", headerFont, Brushes.Black, 300, y);
        y += lineHeight + 10;

        g.DrawString($"审批日期：{_model.AuditDate:yyyy-MM-dd}", headerFont, Brushes.Black, 50, y);

        // 动态计算底部起始位置 - 在审批日期下方30像素开始
        y += lineHeight + 30;

        // 打印分隔线
        g.DrawLine(Pens.Black, 50, y, pageBounds.Width - 50, y);
        y += 15;

        // 打印出门时间、签字等
        g.DrawString("出门时间：_______年_____月_____日 _____时_____分", normalFont, Brushes.Black, 50, y);
        y += lineHeight + 10;

        g.DrawString("门卫签字：__________________", normalFont, Brushes.Black, 50, y);
        g.DrawString("经办人签字：__________________", normalFont, Brushes.Black, 300, y);
        y += lineHeight + 10;

        // 打印备注
        g.DrawString("备注：", headerFont, Brushes.Black, 50, y);
        g.DrawLine(Pens.Black, 90, y + 18, pageBounds.Width - 50, y + 18);
        y += lineHeight + 10;

        // 打印页脚 - 确保在备注下方
        var footerY = y + 10;
        // 确保页脚不会超出页面
        if (footerY > pageBounds.Height - 30)
        {
            footerY = pageBounds.Height - 30;
        }
        g.DrawString("此联由门卫留存", smallFont, Brushes.Gray, 50, footerY);
        g.DrawString($"打印时间：{DateTime.Now:yyyy-MM-dd HH:mm:ss}", smallFont, Brushes.Gray,
            pageBounds.Width - 200, footerY);
    }
}
