using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class ExternalProcessingAuditService
{
    public List<ExternalProcessingAudit> GetAuditsByApplicationId(int applicationId)
    {
        var audits = new List<ExternalProcessingAudit>();
        var sql = "SELECT * FROM ExternalProcessingAudits WHERE ApplicationId = @ApplicationId ORDER BY AuditDate DESC";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            audits.Add(MapToAudit(reader));
        }

        return audits;
    }

    public int AddAudit(ExternalProcessingAudit audit)
    {
        audit.AuditDate = DateTime.Now;
        audit.OperatorTime = DateTime.Now;

        var sql = @"INSERT INTO ExternalProcessingAudits 
            (ApplicationId, AuditorId, AuditorName, AuditDate, AuditResult, AuditRemark, OperatorId, OperatorTime) 
            VALUES (@ApplicationId, @AuditorId, @AuditorName, @AuditDate, @AuditResult, @AuditRemark, @OperatorId, @OperatorTime); 
            SELECT SCOPE_IDENTITY();";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", audit.ApplicationId);
        command.Parameters.AddWithValue("@AuditorId", audit.AuditorId);
        command.Parameters.AddWithValue("@AuditorName", audit.AuditorName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@AuditDate", audit.AuditDate);
        command.Parameters.AddWithValue("@AuditResult", audit.AuditResult);
        command.Parameters.AddWithValue("@AuditRemark", audit.AuditRemark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@OperatorId", audit.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", audit.OperatorTime);

        return Convert.ToInt32(command.ExecuteScalar());
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

    private ExternalProcessingAudit MapToAudit(SqlDataReader reader)
    {
        return new ExternalProcessingAudit
        {
            AuditId = Convert.ToInt32(reader["AuditId"]),
            ApplicationId = Convert.ToInt32(reader["ApplicationId"]),
            AuditorId = Convert.ToInt32(reader["AuditorId"]),
            AuditorName = reader["AuditorName"] == DBNull.Value ? null : reader["AuditorName"].ToString(),
            AuditDate = Convert.ToDateTime(reader["AuditDate"]),
            AuditResult = Convert.ToInt32(reader["AuditResult"]),
            AuditRemark = reader["AuditRemark"] == DBNull.Value ? null : reader["AuditRemark"].ToString(),
            OperatorId = Convert.ToInt32(reader["OperatorId"]),
            OperatorTime = Convert.ToDateTime(reader["OperatorTime"])
        };
    }
}
