using System.Configuration;
using Microsoft.Data.SqlClient;

namespace ExternalProcessing.Helpers;

public static class DbHelper
{
    private static string? _connectionString;

    public static string ConnectionString
    {
        get
        {
            if (_connectionString == null)
            {
                _connectionString = ConfigurationManager.ConnectionStrings["CartonQuoteDB"]?.ConnectionString
                    ?? throw new InvalidOperationException("数据库连接字符串未配置");
            }
            return _connectionString;
        }
    }

    public static SqlConnection CreateConnection()
    {
        return new SqlConnection(ConnectionString);
    }

    public static object? ExecuteScalar(string sql, params SqlParameter[] parameters)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand(sql, connection);
        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }
        connection.Open();
        return command.ExecuteScalar();
    }

    public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
    {
        using var connection = CreateConnection();
        using var command = new SqlCommand(sql, connection);
        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }
        connection.Open();
        return command.ExecuteNonQuery();
    }

    public static SqlDataReader ExecuteReader(string sql, SqlConnection connection, params SqlParameter[] parameters)
    {
        using var command = new SqlCommand(sql, connection);
        if (parameters.Length > 0)
        {
            command.Parameters.AddRange(parameters);
        }
        return command.ExecuteReader();
    }
}
