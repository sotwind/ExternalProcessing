using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class ExternalProcessingService
{
    public List<ExternalProcessingApplication> GetAllApplications(int? status = null)
    {
        var applications = new List<ExternalProcessingApplication>();
        var sql = "SELECT * FROM ExternalProcessingApplications WHERE 1=1";
        var parameters = new List<SqlParameter>();

        if (status.HasValue)
        {
            sql += " AND Status = @Status";
            parameters.Add(new SqlParameter("@Status", status.Value));
        }

        sql += " ORDER BY ApplicationDate DESC";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddRange(parameters.ToArray());

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            applications.Add(MapToApplication(reader));
        }

        // 加载每个申请的最新审批意见
        foreach (var app in applications)
        {
            app.LatestAuditRemark = GetLatestAuditRemark(app.ApplicationId);
        }

        return applications;
    }

    private string? GetLatestAuditRemark(int applicationId)
    {
        var sql = @"SELECT TOP 1 AuditRemark FROM ExternalProcessingAudits 
                     WHERE ApplicationId = @ApplicationId 
                     ORDER BY AuditDate DESC";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        var result = command.ExecuteScalar();
        return result == DBNull.Value ? null : result?.ToString();
    }

    public ExternalProcessingApplication? GetApplicationById(int applicationId)
    {
        var sql = "SELECT * FROM ExternalProcessingApplications WHERE ApplicationId = @ApplicationId";
        
        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return MapToApplication(reader);
        }

        return null;
    }

    public List<ExternalProcessingApplicationDetail> GetApplicationDetails(int applicationId)
    {
        var details = new List<ExternalProcessingApplicationDetail>();
        var sql = "SELECT * FROM ExternalProcessingApplicationDetails WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        using var reader = command.ExecuteReader();
        while (reader.Read())
        {
            details.Add(MapToDetail(reader));
        }

        return details;
    }

    public int CreateApplication(ExternalProcessingApplication application, List<ExternalProcessingApplicationDetail> details)
    {
        application.ApplicationNo = GenerateApplicationNo();
        application.Status = 1;
        application.OperatorTime = DateTime.Now;

        var sql = @"INSERT INTO ExternalProcessingApplications 
            (ApplicationNo, OrderId, OrderNo, ApplicantId, ApplicantName, ApplicationDate, ProcessorId, ProcessorName, ProcessingContent, ExpectedReturnDate, Status, Remark, TotalQuantity, OperatorId, OperatorTime) 
            VALUES (@ApplicationNo, @OrderId, @OrderNo, @ApplicantId, @ApplicantName, @ApplicationDate, @ProcessorId, @ProcessorName, @ProcessingContent, @ExpectedReturnDate, @Status, @Remark, @TotalQuantity, @OperatorId, @OperatorTime); 
            SELECT SCOPE_IDENTITY();";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationNo", application.ApplicationNo ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@OrderId", application.OrderId.HasValue ? application.OrderId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@OrderNo", application.OrderNo ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicantId", application.ApplicantId.HasValue ? application.ApplicantId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicantName", application.ApplicantName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
        command.Parameters.AddWithValue("@ProcessorId", application.ProcessorId.HasValue ? application.ProcessorId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@ProcessorName", application.ProcessorName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ProcessingContent", application.ProcessingContent ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ExpectedReturnDate", application.ExpectedReturnDate);
        command.Parameters.AddWithValue("@Status", application.Status);
        command.Parameters.AddWithValue("@Remark", application.Remark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@TotalQuantity", application.TotalQuantity);
        command.Parameters.AddWithValue("@OperatorId", application.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", application.OperatorTime);

        var applicationId = Convert.ToInt32(command.ExecuteScalar());

        foreach (var detail in details)
        {
            detail.ApplicationId = applicationId;
            detail.OperatorTime = DateTime.Now;
            AddDetail(detail);
        }

        return applicationId;
    }

    public bool UpdateApplication(ExternalProcessingApplication application, List<ExternalProcessingApplicationDetail> details)
    {
        application.OperatorTime = DateTime.Now;

        var sql = @"UPDATE ExternalProcessingApplications SET 
            ApplicationNo = @ApplicationNo, OrderId = @OrderId, OrderNo = @OrderNo, 
            ApplicantId = @ApplicantId, ApplicantName = @ApplicantName, ApplicationDate = @ApplicationDate, 
            ProcessorId = @ProcessorId, ProcessorName = @ProcessorName, ProcessingContent = @ProcessingContent, 
            ExpectedReturnDate = @ExpectedReturnDate, Status = @Status, Remark = @Remark, TotalQuantity = @TotalQuantity, 
            OperatorId = @OperatorId, OperatorTime = @OperatorTime 
            WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", application.ApplicationId);
        command.Parameters.AddWithValue("@ApplicationNo", application.ApplicationNo ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@OrderId", application.OrderId.HasValue ? application.OrderId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@OrderNo", application.OrderNo ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicantId", application.ApplicantId.HasValue ? application.ApplicantId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicantName", application.ApplicantName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ApplicationDate", application.ApplicationDate);
        command.Parameters.AddWithValue("@ProcessorId", application.ProcessorId.HasValue ? application.ProcessorId.Value : (object)DBNull.Value);
        command.Parameters.AddWithValue("@ProcessorName", application.ProcessorName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ProcessingContent", application.ProcessingContent ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@ExpectedReturnDate", application.ExpectedReturnDate);
        command.Parameters.AddWithValue("@Status", application.Status);
        command.Parameters.AddWithValue("@Remark", application.Remark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@TotalQuantity", application.TotalQuantity);
        command.Parameters.AddWithValue("@OperatorId", application.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", application.OperatorTime);

        var result = command.ExecuteNonQuery() > 0;

        if (result)
        {
            DeleteDetailsByApplicationId(application.ApplicationId);
            foreach (var detail in details)
            {
                detail.ApplicationId = application.ApplicationId;
                detail.OperatorTime = DateTime.Now;
                AddDetail(detail);
            }
        }

        return result;
    }

    public bool DeleteApplication(int applicationId)
    {
        DeleteDetailsByApplicationId(applicationId);

        var sql = "DELETE FROM ExternalProcessingApplications WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        return command.ExecuteNonQuery() > 0;
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

    private void AddDetail(ExternalProcessingApplicationDetail detail)
    {
        var sql = @"INSERT INTO ExternalProcessingApplicationDetails 
            (ApplicationId, ItemId, ItemName, Specification, Quantity, UnitPrice, TotalAmount, Remark, OperatorId, OperatorTime) 
            VALUES (@ApplicationId, @ItemId, @ItemName, @Specification, @Quantity, @UnitPrice, @TotalAmount, @Remark, @OperatorId, @OperatorTime)";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", detail.ApplicationId);
        command.Parameters.AddWithValue("@ItemId", detail.ItemId);
        command.Parameters.AddWithValue("@ItemName", detail.ItemName ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Specification", detail.Specification ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Quantity", detail.Quantity);
        command.Parameters.AddWithValue("@UnitPrice", detail.UnitPrice);
        command.Parameters.AddWithValue("@TotalAmount", detail.TotalAmount);
        command.Parameters.AddWithValue("@Remark", detail.Remark ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@OperatorId", detail.OperatorId);
        command.Parameters.AddWithValue("@OperatorTime", detail.OperatorTime);

        command.ExecuteNonQuery();
    }

    private void DeleteDetailsByApplicationId(int applicationId)
    {
        var sql = "DELETE FROM ExternalProcessingApplicationDetails WHERE ApplicationId = @ApplicationId";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@ApplicationId", applicationId);

        command.ExecuteNonQuery();
    }

    private string GenerateApplicationNo()
    {
        var yearMonth = DateTime.Now.ToString("yyyyMM");
        var sql = $"SELECT TOP 1 ApplicationNo FROM ExternalProcessingApplications WHERE ApplicationNo LIKE 'EPA{yearMonth}%' ORDER BY ApplicationNo DESC";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);

        var result = command.ExecuteScalar();
        if (result != null)
        {
            var lastNo = result.ToString();
            var seq = Convert.ToInt32(lastNo?.Substring(10)) + 1;
            return $"EPA{yearMonth}{seq:D4}";
        }

        return $"EPA{yearMonth}0001";
    }

    private ExternalProcessingApplication MapToApplication(SqlDataReader reader)
    {
        return new ExternalProcessingApplication
        {
            ApplicationId = reader["ApplicationId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicationId"]),
            ApplicationNo = reader["ApplicationNo"] == DBNull.Value ? null : reader["ApplicationNo"].ToString(),
            OrderId = reader["OrderId"] == DBNull.Value ? null : Convert.ToInt32(reader["OrderId"]),
            OrderNo = reader["OrderNo"] == DBNull.Value ? null : reader["OrderNo"].ToString(),
            ApplicantId = reader["ApplicantId"] == DBNull.Value ? null : Convert.ToInt32(reader["ApplicantId"]),
            ApplicantName = reader["ApplicantName"] == DBNull.Value ? null : reader["ApplicantName"].ToString(),
            ApplicationDate = reader["ApplicationDate"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["ApplicationDate"]),
            ProcessorId = reader["ProcessorId"] == DBNull.Value ? null : Convert.ToInt32(reader["ProcessorId"]),
            ProcessorName = reader["ProcessorName"] == DBNull.Value ? null : reader["ProcessorName"].ToString(),
            ProcessingContent = reader["ProcessingContent"] == DBNull.Value ? null : reader["ProcessingContent"].ToString(),
            ExpectedReturnDate = reader["ExpectedReturnDate"] == DBNull.Value ? DateTime.Now.AddDays(7) : Convert.ToDateTime(reader["ExpectedReturnDate"]),
            Status = reader["Status"] == DBNull.Value ? 1 : Convert.ToInt32(reader["Status"]),
            Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
            TotalQuantity = reader["TotalQuantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["TotalQuantity"]),
            OperatorId = reader["OperatorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OperatorId"]),
            OperatorTime = reader["OperatorTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["OperatorTime"])
        };
    }

    private ExternalProcessingApplicationDetail MapToDetail(SqlDataReader reader)
    {
        return new ExternalProcessingApplicationDetail
        {
            DetailId = reader["DetailId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["DetailId"]),
            ApplicationId = reader["ApplicationId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ApplicationId"]),
            ItemId = reader["ItemId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["ItemId"]),
            ItemName = reader["ItemName"] == DBNull.Value ? null : reader["ItemName"].ToString(),
            Specification = reader["Specification"] == DBNull.Value ? null : reader["Specification"].ToString(),
            Quantity = reader["Quantity"] == DBNull.Value ? 0 : Convert.ToInt32(reader["Quantity"]),
            UnitPrice = reader["UnitPrice"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["UnitPrice"]),
            TotalAmount = reader["TotalAmount"] == DBNull.Value ? 0 : Convert.ToDecimal(reader["TotalAmount"]),
            Remark = reader["Remark"] == DBNull.Value ? null : reader["Remark"].ToString(),
            OperatorId = reader["OperatorId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["OperatorId"]),
            OperatorTime = reader["OperatorTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["OperatorTime"])
        };
    }
}
