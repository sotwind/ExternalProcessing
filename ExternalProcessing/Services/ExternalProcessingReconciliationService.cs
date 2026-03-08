using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class ExternalProcessingReconciliationService
{
    public int AddReconciliation(ExternalProcessingReconciliation reconciliation)
    {
        reconciliation.ReconciliationDate = DateTime.Now;
        reconciliation.OperatorTime = DateTime.Now;

        var sql = @"INSERT INTO ExternalProcessingReconciliations 
            (ApplicationId, ReconciliationDate, ReconciliationAmount, ReconciliationRemark, Status, OperatorId, OperatorTime) 
            VALUES (@ApplicationId, @ReconciliationDate, @ReconciliationAmount, @ReconciliationRemark, @Status, @OperatorId, @OperatorTime); 
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", reconciliation.ApplicationId);
        command.Parameters.AddWithValue("@ReconciliationDate", reconciliation.ReconciliationDate);
        command.Parameters.AddWithValue("@ReconciliationAmount", reconciliation.ReconciliationAmount);
        command.Parameters.AddWithValue("@ReconciliationRemark", reconciliation.ReconciliationRemark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Status", reconciliation.Status);
        command.Parameters.AddWithValue("@OperatorId", reconciliation.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", reconciliation.OperatorTime);

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

    public bool DeleteReconciliationByApplicationId(int applicationId)
    {
        var sql = "DELETE FROM ExternalProcessingReconciliations WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        return command.ExecuteNonQuery() > 0;
    }
}
