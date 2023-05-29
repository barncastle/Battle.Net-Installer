using System;
using System.Threading;
using System.Windows.Forms;
using Dark.Net;

namespace BNetInstaller
{
    internal static class ProgramLauncher
    {
        static Mutex mutex = new Mutex(true, "D4 Launcher");
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                IDarkNet darkNet = DarkNet.Instance;
                Theme processTheme = Theme.Dark;
                darkNet.SetCurrentProcessTheme(processTheme);
                Form mainForm = new Form1();
                Theme windowTheme = Theme.Dark;
                darkNet.SetWindowThemeForms(mainForm, windowTheme);
                darkNet.UserDefaultAppThemeIsDarkChanged += (_, isSystemDarkTheme) => Console.WriteLine($"System theme is {(isSystemDarkTheme ? "Dark" : "Light")}");
                darkNet.UserTaskbarThemeIsDarkChanged += (_, isTaskbarDarkTheme) => Console.WriteLine($"Taskbar theme is {(isTaskbarDarkTheme ? "Dark" : "Light")}");
                Application.Run(mainForm);
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("D4 Launcher уже запущен");
            }

        }
    }
}
