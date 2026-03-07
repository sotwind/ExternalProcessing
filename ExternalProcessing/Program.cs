using ExternalProcessing.Forms;

namespace ExternalProcessing;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // 显示登录窗体
        using var loginForm = new LoginForm();
        var result = loginForm.ShowDialog();

        if (result != DialogResult.OK || !loginForm.IsLoggedIn)
        {
            Application.Exit();
            return;
        }

        // 登录成功后，登录窗体自动关闭，显示主窗体
        var mainForm = new MainForm(loginForm.CurrentUser!);
        Application.Run(mainForm);
    }
}
