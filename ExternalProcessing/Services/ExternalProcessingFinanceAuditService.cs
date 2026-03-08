using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class ExternalProcessingFinanceAuditService
{
    public int AddFinanceAudit(ExternalProcessingFinanceAudit financeAudit)
    {
        financeAudit.AuditDate = DateTime.Now;
        financeAudit.OperatorTime = DateTime.Now;

        var sql = @"INSERT INTO ExternalProcessingFinanceAudits 
            (ReconciliationId, FinanceAuditorId, FinanceAuditorName, AuditDate, AuditResult, AuditRemark, PaymentStatus, OperatorId, OperatorTime) 
            VALUES (@ReconciliationId, @FinanceAuditorId, @FinanceAuditorName, @AuditDate, @AuditResult, @AuditRemark, @PaymentStatus, @OperatorId, @OperatorTime); 
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ReconciliationId", financeAudit.ReconciliationId);
        command.Parameters.AddWithValue("@FinanceAuditorId", financeAudit.FinanceAuditorId);
        command.Parameters.AddWithValue("@FinanceAuditorName", financeAudit.FinanceAuditorName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AuditDate", financeAudit.AuditDate);
        command.Parameters.AddWithValue("@AuditResult", financeAudit.AuditResult);
        command.Parameters.AddWithValue("@AuditRemark", financeAudit.AuditRemark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@PaymentStatus", financeAudit.PaymentStatus);
        command.Parameters.AddWithValue("@OperatorId", financeAudit.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", financeAudit.OperatorTime);

        var result = command.ExecuteScalar();
        return result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    public bool UpdateApplicationStatus(int applicationId, int status)
    {
        var sql = "UPDATE ExternalProcessingApplications SET Status = @Status, OperatorTime = @OperatorTime WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);
        command.Parameters.AddWithValue("@Status", status);
        command.Parameters.AddWithValue("@OperatorTime", DateTime.Now);

        return command.ExecuteNonQuery() > 0;
    }

    public bool DeleteFinanceAuditByApplicationId(int applicationId)
    {
        var sql = "DELETE FROM ExternalProcessingFinanceAudits WHERE ReconciliationId IN (SELECT ReconciliationId FROM ExternalProcessingReconciliations WHERE ApplicationId = @ApplicationId)";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        return command.ExecuteNonQuery() > 0;
    }
}
