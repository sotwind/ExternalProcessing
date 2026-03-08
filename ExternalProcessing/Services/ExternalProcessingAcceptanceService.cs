using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class ExternalProcessingAcceptanceService
{
    public int AddAcceptance(ExternalProcessingAcceptance acceptance)
    {
        acceptance.AcceptanceDate = DateTime.Now;
        acceptance.OperatorTime = DateTime.Now;

        var sql = @"INSERT INTO ExternalProcessingAcceptances 
            (ApplicationId, AcceptanceDate, AcceptanceResult, AcceptanceRemark, OperatorId, OperatorTime) 
            VALUES (@ApplicationId, @AcceptanceDate, @AcceptanceResult, @AcceptanceRemark, @OperatorId, @OperatorTime); 
            SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", acceptance.ApplicationId);
        command.Parameters.AddWithValue("@AcceptanceDate", acceptance.AcceptanceDate);
        command.Parameters.AddWithValue("@AcceptanceResult", acceptance.AcceptanceResult);
        command.Parameters.AddWithValue("@AcceptanceRemark", acceptance.AcceptanceRemark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@OperatorId", acceptance.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", acceptance.OperatorTime);

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

    public bool DeleteAcceptanceByApplicationId(int applicationId)
    {
        var sql = "DELETE FROM ExternalProcessingAcceptances WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        return command.ExecuteNonQuery() > 0;
    }
}
