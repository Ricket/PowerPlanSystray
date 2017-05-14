using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerPlanSystray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MyApplicationContext());
        }
    }

    class MyApplicationContext : ApplicationContext
    {
        SettingsForm settingsForm;
        SysTrayIcon sysTrayIcon;
        PowerPlans powerPlans;

        public MyApplicationContext()
        {
            powerPlans = PowerPlans.Instance;
            settingsForm = null;
            sysTrayIcon = new SysTrayIcon();

            sysTrayIcon.SetActivePlan += SysTrayIcon_SetActivePlan;
            sysTrayIcon.OpenSettings += SysTrayIcon_OpenSettings;
            sysTrayIcon.Exit += SysTrayIcon_Exit;
            Application.ApplicationExit += Application_ApplicationExit;

            Microsoft.Win32.SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;
        }

        private void SystemEvents_SessionSwitch(object sender, Microsoft.Win32.SessionSwitchEventArgs e)
        {
            string guid = null;
            if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionLock)
            {
                // Locked
                Console.WriteLine("Workstation locked");
                guid = Properties.Settings.Default.LockPlan;
            }
            else if (e.Reason == Microsoft.Win32.SessionSwitchReason.SessionUnlock)
            {
                // Unlocked
                Console.WriteLine("Workstation unlocked");
                guid = Properties.Settings.Default.UnlockPlan;
            }

            if (guid != null && powerPlans.ActivePlan.Guid != guid)
            {
                PowerPlan plan = powerPlans.Plans.Find(p => p.Guid == guid);
                if (plan != null)
                {
                    powerPlans.SetActivePlan(plan);
                }
            }
        }

        private void Application_ApplicationExit(object sender, EventArgs e)
        {
            // If Close isn't called on sysTrayIcon, then the icon tends to linger in the system tray.
            sysTrayIcon.Close();
        }

        public void Exit()
        {
            sysTrayIcon.Close();
            ExitThread();
        }

        private void SysTrayIcon_SetActivePlan(object sender, SetActivePlanEventArgs e)
        {
            PowerPlan newActive = powerPlans.Plans.Find(plan => plan.Guid == e.Guid);
            if (newActive != powerPlans.ActivePlan)
            {
                powerPlans.SetActivePlan(newActive);
            }
        }

        private void CreateSettingsForm()
        {
            if (settingsForm == null || settingsForm.IsDisposed)
            {
                settingsForm = new SettingsForm();
            }
        }

        private void SysTrayIcon_OpenSettings(object sender, EventArgs e)
        {
            CreateSettingsForm();
            settingsForm.Show();
        }

        private void SysTrayIcon_Exit(object sender, EventArgs e)
        {
            Exit();
        }
    }
}
