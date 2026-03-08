using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using ExternalProcessing.Helpers;
using ExternalProcessing.Models;

namespace ExternalProcessing.Services;

public class UserService
{
    /// <summary>
    /// 获取所有用户列表
    /// </summary>
    public List<User> GetAllUsers()
    {
        var users = new List<User>();
        var sql = "SELECT UserID, Username, Password, Permissions, IsActive, CreateTime FROM EP_User ORDER BY UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            users.Add(MapToUser(reader));
        }

        return users;
    }

    /// <summary>
    /// 根据ID获取用户
    /// </summary>
    public User? GetUserById(int userId)
    {
        var sql = "SELECT UserID, Username, Password, Permissions, IsActive, CreateTime FROM EP_User WHERE UserID = @UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserID", userId);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapToUser(reader);
        }

        return null;
    }

    /// <summary>
    /// 根据用户名获取用户
    /// </summary>
    public User? GetUserByUsername(string username)
    {
        var sql = "SELECT UserID, Username, Password, Permissions, IsActive, CreateTime FROM EP_User WHERE Username = @Username";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", username);
        using var reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapToUser(reader);
        }

        return null;
    }

    /// <summary>
    /// 创建新用户
    /// </summary>
    public int CreateUser(User user)
    {
        var sql = @"INSERT INTO EP_User (Username, Password, Permissions, IsActive, CreateTime) 
                     VALUES (@Username, @Password, @Permissions, @IsActive, @CreateTime);
                     SELECT CAST(SCOPE_IDENTITY() AS INT);";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", user.Username);
        command.Parameters.AddWithValue("@Password", user.Password);
        command.Parameters.AddWithValue("@Permissions", user.Permissions);
        command.Parameters.AddWithValue("@IsActive", user.IsActive);
        command.Parameters.AddWithValue("@CreateTime", DateTime.Now);

        var result = command.ExecuteScalar();
        return result == DBNull.Value ? 0 : Convert.ToInt32(result);
    }

    /// <summary>
    /// 更新用户信息
    /// </summary>
    public bool UpdateUser(User user)
    {
        var sql = @"UPDATE EP_User SET 
                     Permissions = @Permissions, 
                     IsActive = @IsActive
                     WHERE UserID = @UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserID", user.UserID);
        command.Parameters.AddWithValue("@Permissions", user.Permissions);
        command.Parameters.AddWithValue("@IsActive", user.IsActive);

        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// 更新用户密码
    /// </summary>
    public bool UpdatePassword(int userId, string newPassword)
    {
        var sql = "UPDATE EP_User SET Password = @Password WHERE UserID = @UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserID", userId);
        command.Parameters.AddWithValue("@Password", newPassword);

        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// 删除用户
    /// </summary>
    public bool DeleteUser(int userId)
    {
        var sql = "DELETE FROM EP_User WHERE UserID = @UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@UserID", userId);

        return command.ExecuteNonQuery() > 0;
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    public bool ResetPassword(int userId, string newPassword)
    {
        return UpdatePassword(userId, newPassword);
    }

    /// <summary>
    /// 检查用户名是否已存在
    /// </summary>
    public bool IsUsernameExists(string username)
    {
        var sql = "SELECT COUNT(*) FROM EP_User WHERE Username = @Username";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", username);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    /// <summary>
    /// 检查用户名是否已存在（排除指定用户ID）
    /// </summary>
    public bool IsUsernameExists(string username, int excludeUserId)
    {
        var sql = "SELECT COUNT(*) FROM EP_User WHERE Username = @Username AND UserID != @UserID";

        using var connection = DbHelper.CreateConnection();
        connection.Open();
        using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@Username", username);
        command.Parameters.AddWithValue("@UserID", excludeUserId);

        return Convert.ToInt32(command.ExecuteScalar()) > 0;
    }

    private User MapToUser(SqlDataReader reader)
    {
        return new User
        {
            UserID = reader["UserID"] == DBNull.Value ? 0 : Convert.ToInt32(reader["UserID"]),
            Username = reader["Username"] == DBNull.Value ? "" : reader["Username"].ToString() ?? "",
            Password = reader["Password"] == DBNull.Value ? "" : reader["Password"].ToString() ?? "",
            Permissions = reader["Permissions"] == DBNull.Value ? "" : reader["Permissions"].ToString() ?? "",
            IsActive = reader["IsActive"] == DBNull.Value ? true : Convert.ToBoolean(reader["IsActive"]),
            CreateTime = reader["CreateTime"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["CreateTime"])
        };
    }
}
