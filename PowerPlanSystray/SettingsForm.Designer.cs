namespace PowerPlanSystray
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBoxVisibleSettings = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbLockPlan = new System.Windows.Forms.ComboBox();
            this.cmbUnlockPlan = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listBoxVisibleSettings
            // 
            this.listBoxVisibleSettings.CheckOnClick = true;
            this.listBoxVisibleSettings.FormattingEnabled = true;
            this.listBoxVisibleSettings.Location = new System.Drawing.Point(12, 25);
            this.listBoxVisibleSettings.Name = "listBoxVisibleSettings";
            this.listBoxVisibleSettings.Size = new System.Drawing.Size(495, 139);
            this.listBoxVisibleSettings.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(126, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Show these power plans:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 185);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(261, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "When workstation is locked, change power setting to:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(273, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "When workstation is unlocked, change power setting to:";
            // 
            // cmbLockPlan
            // 
            this.cmbLockPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLockPlan.FormattingEnabled = true;
            this.cmbLockPlan.Location = new System.Drawing.Point(15, 201);
            this.cmbLockPlan.MaxDropDownItems = 20;
            this.cmbLockPlan.Name = "cmbLockPlan";
            this.cmbLockPlan.Size = new System.Drawing.Size(239, 21);
            this.cmbLockPlan.TabIndex = 4;
            // 
            // cmbUnlockPlan
            // 
            this.cmbUnlockPlan.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUnlockPlan.FormattingEnabled = true;
            this.cmbUnlockPlan.Location = new System.Drawing.Point(15, 252);
            this.cmbUnlockPlan.MaxDropDownItems = 20;
            this.cmbUnlockPlan.Name = "cmbUnlockPlan";
            this.cmbUnlockPlan.Size = new System.Drawing.Size(239, 21);
            this.cmbUnlockPlan.TabIndex = 5;
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 307);
            this.Controls.Add(this.cmbUnlockPlan);
            this.Controls.Add(this.cmbLockPlan);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBoxVisibleSettings);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SettingsForm";
            this.Text = "PowerPlanSystray Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox listBoxVisibleSettings;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbLockPlan;
        private System.Windows.Forms.ComboBox cmbUnlockPlan;
    }
}

