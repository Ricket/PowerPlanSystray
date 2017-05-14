using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerPlanSystray
{
    public class SetActivePlanEventArgs : EventArgs
    {
        private string guid;

        public SetActivePlanEventArgs(string guid)
        {
            this.guid = guid;
        }

        public string Guid
        {
            get { return guid; }
        }
    }

    public delegate void SetActivePlanEventHandler(object sender, SetActivePlanEventArgs e);

    public class SysTrayIcon
    {
        public event SetActivePlanEventHandler SetActivePlan;
        public event EventHandler OpenSettings;
        public event EventHandler Exit;

        private NotifyIcon notifyIcon;
        private ContextMenuStrip contextMenuStrip;
        private ToolStripSeparator tsSeparator;
        private ToolStripMenuItem tsmiSettings;
        private ToolStripMenuItem tsmiExit;

        public SysTrayIcon()
        {
            var components = new Container();
            this.notifyIcon = new NotifyIcon(components);
            this.contextMenuStrip = new ContextMenuStrip(components);
            this.tsSeparator = new ToolStripSeparator();
            this.tsmiSettings = new ToolStripMenuItem();
            this.tsmiExit = new ToolStripMenuItem();

            this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
            this.notifyIcon.Icon = Properties.Resources.sysTrayIcon;
            this.notifyIcon.Text = "Loading";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            this.notifyIcon.BalloonTipTitle = "Balloon Title";
            this.notifyIcon.BalloonTipText = "Balloon Text";

            this.tsSeparator.Name = "tsSeparator";
            this.tsSeparator.Size = new System.Drawing.Size(113, 6);
            this.contextMenuStrip.Items.Add(this.tsSeparator);

            this.tsmiSettings.Name = "tsmiSettings";
            this.tsmiSettings.Size = new System.Drawing.Size(116, 22);
            this.tsmiSettings.Text = "Settings";
            this.tsmiSettings.Click += TsmiSettings_Click;
            this.contextMenuStrip.Items.Add(this.tsmiSettings);

            this.tsmiExit.Name = "tsmiExit";
            this.tsmiExit.Size = new System.Drawing.Size(116, 22);
            this.tsmiExit.Text = "Exit";
            this.tsmiExit.Click += TsmiExit_Click;
            this.contextMenuStrip.Items.Add(this.tsmiExit);

            RefreshMenuOptions();

            PowerPlans.Instance.Reloaded += Instance_Reloaded;
            Properties.Settings.Default.SettingsSaving += Default_SettingsSaving;
        }

        private void Instance_Reloaded(object sender, EventArgs e)
        {
            RefreshMenuOptions();
        }

        private void Default_SettingsSaving(object sender, CancelEventArgs e)
        {
            RefreshMenuOptions();
        }

        public void Close()
        {
            notifyIcon.Dispose();
        }

        private void TsmiSettings_Click(object sender, EventArgs e)
        {
            if (OpenSettings != null)
            {
                OpenSettings(sender, e);
            }
        }

        private void TsmiExit_Click(object sender, EventArgs e)
        {
            if (Exit != null)
            {
                Exit(sender, e);
            }
        }

        private void RefreshMenuOptions()
        {
            notifyIcon.Text = PowerPlans.Instance.ActivePlan.Name;

            while (contextMenuStrip.Items[0] != tsSeparator)
            {
                contextMenuStrip.Items.RemoveAt(0);
            }

            var visiblePlans = Properties.Settings.Default.VisiblePlans;

            var idx = 0;
            foreach (var plan in PowerPlans.Instance.Plans)
            {
                if (visiblePlans != null && visiblePlans.Count > 0 && !visiblePlans.Contains(plan.Guid))
                {
                    continue;
                }

                var tsmi = new ToolStripMenuItem(plan.Name);
                tsmi.Name = plan.Guid;
                tsmi.Click += Tsmi_Click;
                if (plan == PowerPlans.Instance.ActivePlan)
                {
                    tsmi.Checked = true;
                    tsmi.CheckState = CheckState.Checked;
                }
                contextMenuStrip.Items.Insert(idx++, tsmi);
            }
        }

        private void Tsmi_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem source = (ToolStripMenuItem)sender;
            SetActivePlan(sender, new SetActivePlanEventArgs(source.Name));
        }

        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (OpenSettings != null)
            {
                OpenSettings(sender, e);
            }
        }
    }
}
