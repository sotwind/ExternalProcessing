using ExternalProcessing.Helpers;
using ExternalProcessing.Models;
using Microsoft.Data.SqlClient;

namespace ExternalProcessing.Forms;

public partial class LoginForm : Form
{
    public bool IsLoggedIn { get; private set; }
    public User? CurrentUser { get; private set; }

    // 配置文件路径
    private readonly string _configFilePath;

    public LoginForm()
    {
        InitializeComponent();
        this.StartPosition = FormStartPosition.CenterScreen;
        
        // 设置配置文件路径
        _configFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ExternalProcessing",
            "login.config"
        );
        
        // 加载保存的登录信息
        LoadSavedLoginInfo();
    }

    private void LoadSavedLoginInfo()
    {
        try
        {
            if (File.Exists(_configFilePath))
            {
                var lines = File.ReadAllLines(_configFilePath);
                if (lines.Length >= 3)
                {
                    string savedUsername = lines[0];
                    string savedPassword = lines[1];
                    bool rememberPassword = bool.Parse(lines[2]);

                    TxtUsername.Text = savedUsername;
                    
                    if (rememberPassword)
                    {
                        TxtPassword.Text = savedPassword;
                        ChkRememberPassword.Checked = true;
                    }
                }
            }
        }
        catch
        {
            // 如果读取失败，忽略错误
        }
    }

    private void SaveLoginInfo()
    {
        try
        {
            // 确保目录存在
            var directory = Path.GetDirectoryName(_configFilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // 保存登录信息
            var lines = new List<string>
            {
                TxtUsername.Text.Trim(),
                ChkRememberPassword.Checked ? TxtPassword.Text.Trim() : "",
                ChkRememberPassword.Checked.ToString()
            };

            File.WriteAllLines(_configFilePath, lines);
        }
        catch
        {
            // 如果保存失败，忽略错误
        }
    }

    private void BtnLogin_Click(object sender, EventArgs e)
    {
        string username = TxtUsername.Text.Trim();
        string password = TxtPassword.Text.Trim();

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
        {
            MessageBox.Show("请输入用户名和密码", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        try
        {
            using var connection = DbHelper.CreateConnection();
            connection.Open();

            string sql = "SELECT UserID, Username, Password, Permissions, IsActive, CreateTime FROM EP_User WHERE Username = @Username";
            using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Username", username);

            using var reader = command.ExecuteReader();
            if (reader.Read())
            {
                string dbPassword = reader["Password"].ToString() ?? "";
                bool isActive = reader["IsActive"] != DBNull.Value && Convert.ToBoolean(reader["IsActive"]);

                if (!isActive)
                {
                    MessageBox.Show("账号已被禁用", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (password == dbPassword)
                {
                    CurrentUser = new User
                    {
                        UserID = Convert.ToInt32(reader["UserID"]),
                        Username = reader["Username"].ToString() ?? "",
                        Password = dbPassword,
                        Permissions = reader["Permissions"].ToString() ?? "",
                        IsActive = isActive,
                        CreateTime = reader["CreateTime"] != DBNull.Value ? Convert.ToDateTime(reader["CreateTime"]) : DateTime.Now
                    };

                    // 保存登录信息
                    SaveLoginInfo();

                    IsLoggedIn = true;
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("密码错误", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TxtPassword.Clear();
                    TxtPassword.Focus();
                }
            }
            else
            {
                MessageBox.Show("用户不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                TxtUsername.Clear();
                TxtPassword.Clear();
                TxtUsername.Focus();
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show("登录失败：" + ex.Message, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void BtnCancel_Click(object sender, EventArgs e)
    {
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void TxtPassword_KeyPress(object sender, KeyPressEventArgs e)
    {
        if (e.KeyChar == (char)Keys.Enter)
        {
            BtnLogin_Click(sender, e);
        }
    }
}
