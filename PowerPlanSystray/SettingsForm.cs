using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PowerPlanSystray
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
            this.listBoxVisibleSettings.ItemCheck += this.listBoxVisibleSettings_ItemCheck;

            var planNames = PowerPlans.Instance.Plans.Select(plan => plan.Name).ToArray();
            listBoxVisibleSettings.Items.AddRange(planNames);
            cmbLockPlan.Items.Add("(no change)");
            cmbLockPlan.Items.AddRange(planNames);
            cmbLockPlan.SelectedIndex = 0;
            cmbUnlockPlan.Items.Add("(no change)");
            cmbUnlockPlan.Items.AddRange(planNames);
            cmbUnlockPlan.SelectedIndex = 0;

            if (Properties.Settings.Default.LockPlan != null)
            {
                var lockPlan = PowerPlans.Instance.Plans.FindIndex(pl => pl.Guid == Properties.Settings.Default.LockPlan);
                if (lockPlan != -1)
                {
                    // add 1 for the no change entry
                    cmbLockPlan.SelectedIndex = lockPlan + 1;
                }
            }

            if (Properties.Settings.Default.UnlockPlan != null)
            {
                var unlockPlan = PowerPlans.Instance.Plans.FindIndex(pl => pl.Guid == Properties.Settings.Default.UnlockPlan);
                if (unlockPlan != -1)
                {
                    // add 1 for the no change entry
                    cmbUnlockPlan.SelectedIndex = unlockPlan + 1;
                }
            }

            cmbLockPlan.SelectedIndexChanged += CmbLockPlan_SelectedIndexChanged;
            cmbUnlockPlan.SelectedIndexChanged += CmbUnlockPlan_SelectedIndexChanged;

            // --- Visible plans ---

            var visiblePlans = Properties.Settings.Default.VisiblePlans;

            // if this is the user's first time opening the app, then initialize the visible set with all the plans
            if (visiblePlans == null || visiblePlans.Count == 0)
            {
                visiblePlans = new StringCollection();
                Properties.Settings.Default.VisiblePlans = visiblePlans;
                foreach (var plan in PowerPlans.Instance.Plans)
                {
                    visiblePlans.Add(plan.Guid);
                }
            }

            // dedupe the properties file by adding it to a set
            var visiblePlansSet = visiblePlans.ToHashSet();

            // remove any nonexistent guids
            var entriesToRemove = new HashSet<string>();
            foreach (var visiblePlan in visiblePlansSet)
            {
                var plan = PowerPlans.Instance.Plans.Find(p => p.Guid == visiblePlan);
                if (plan == null)
                {
                    entriesToRemove.Add(visiblePlan);
                }
            }
            visiblePlansSet.RemoveWhere(vp => entriesToRemove.Contains(vp));

            // save the new set of visible plans
            var newVisiblePlans = new StringCollection();
            newVisiblePlans.AddRange(visiblePlansSet.ToArray());
            Properties.Settings.Default.VisiblePlans = newVisiblePlans;
            Properties.Settings.Default.Save();

            UpdateCheckboxes();
        }

        private void CmbLockPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLockPlan.SelectedIndex == 0)
            {
                Properties.Settings.Default.LockPlan = "";
            }
            else
            {
                Properties.Settings.Default.LockPlan = PowerPlans.Instance.Plans[cmbLockPlan.SelectedIndex - 1].Guid;
            }
            Properties.Settings.Default.Save();
        }

        private void CmbUnlockPlan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbUnlockPlan.SelectedIndex == 0)
            {
                Properties.Settings.Default.UnlockPlan = "";
            }
            else
            {
                Properties.Settings.Default.UnlockPlan = PowerPlans.Instance.Plans[cmbUnlockPlan.SelectedIndex - 1].Guid;
            }
            Properties.Settings.Default.Save();
        }
        
        private void UpdateCheckboxes()
        {
            this.listBoxVisibleSettings.ItemCheck -= this.listBoxVisibleSettings_ItemCheck;

            var visiblePlans = Properties.Settings.Default.VisiblePlans;
            for (var i = 0; i < PowerPlans.Instance.Plans.Count; i++)
            {
                this.listBoxVisibleSettings.SetItemChecked(i, visiblePlans.Contains(PowerPlans.Instance.Plans[i].Guid));
            }

            this.listBoxVisibleSettings.ItemCheck += this.listBoxVisibleSettings_ItemCheck;
        }

        private void listBoxVisibleSettings_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            var plan = PowerPlans.Instance.Plans[e.Index];
            if (e.NewValue == CheckState.Checked)
            {
                Properties.Settings.Default.VisiblePlans.Add(plan.Guid);
                Properties.Settings.Default.Save();
            }
            else if (e.NewValue == CheckState.Unchecked)
            {
                Properties.Settings.Default.VisiblePlans.Remove(plan.Guid);
                Properties.Settings.Default.Save();
            }
            else
            {
                throw new ArgumentException("Unknown checkstate " + e.NewValue, "e.NewValue");
            }
        }
    }

    static class SettingsFormExtensionMethods
    {
        public static HashSet<string> ToHashSet(this StringCollection coll)
        {
            return new HashSet<string>(coll.ToStringArray());
        }

        public static string[] ToStringArray(this StringCollection coll)
        {
            var ret = new string[coll.Count];
            for (int i = 0; i < coll.Count; i++)
            {
                ret[i] = coll[i];
            }
            return ret;
        }

        public static void DebugPrint(this StringCollection coll, string prefix)
        {
            Console.WriteLine(prefix + ": (" + coll.Count + ") " + string.Join(",", Properties.Settings.Default.VisiblePlans.ToStringArray()));
        }

    }
}
