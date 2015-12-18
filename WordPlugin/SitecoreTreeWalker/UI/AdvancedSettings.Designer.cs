namespace SitecoreTreeWalker.UI
{
    partial class AdvancedSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AdvancedSettings));
            this.EnvironmentLbl = new System.Windows.Forms.Label();
            this.uxEnvironmentPicker = new System.Windows.Forms.ComboBox();
            this.AdvancedSettingCancelBtn = new System.Windows.Forms.Button();
            this.AdvancedSettingOkayBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EnvironmentLbl
            // 
            this.EnvironmentLbl.AutoSize = true;
            this.EnvironmentLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EnvironmentLbl.Location = new System.Drawing.Point(39, 37);
            this.EnvironmentLbl.Name = "EnvironmentLbl";
            this.EnvironmentLbl.Size = new System.Drawing.Size(98, 20);
            this.EnvironmentLbl.TabIndex = 0;
            this.EnvironmentLbl.Text = "Environment";
            // 
            // uxEnvironmentPicker
            // 
            this.uxEnvironmentPicker.FormattingEnabled = true;
            this.uxEnvironmentPicker.Location = new System.Drawing.Point(143, 39);
            this.uxEnvironmentPicker.Name = "uxEnvironmentPicker";
            this.uxEnvironmentPicker.Size = new System.Drawing.Size(190, 21);
            this.uxEnvironmentPicker.TabIndex = 1;
            // 
            // AdvancedSettingCancelBtn
            // 
            this.AdvancedSettingCancelBtn.Location = new System.Drawing.Point(143, 82);
            this.AdvancedSettingCancelBtn.Name = "AdvancedSettingCancelBtn";
            this.AdvancedSettingCancelBtn.Size = new System.Drawing.Size(75, 23);
            this.AdvancedSettingCancelBtn.TabIndex = 2;
            this.AdvancedSettingCancelBtn.Text = "Cancel";
            this.AdvancedSettingCancelBtn.UseVisualStyleBackColor = true;
            this.AdvancedSettingCancelBtn.Click += new System.EventHandler(this.AdvancedSettingCancelBtn_Click);
            // 
            // AdvancedSettingOkayBtn
            // 
            this.AdvancedSettingOkayBtn.Location = new System.Drawing.Point(258, 82);
            this.AdvancedSettingOkayBtn.Name = "AdvancedSettingOkayBtn";
            this.AdvancedSettingOkayBtn.Size = new System.Drawing.Size(75, 23);
            this.AdvancedSettingOkayBtn.TabIndex = 3;
            this.AdvancedSettingOkayBtn.Text = "OK";
            this.AdvancedSettingOkayBtn.UseVisualStyleBackColor = true;
            this.AdvancedSettingOkayBtn.Click += new System.EventHandler(this.AdvancedSettingOkayBtn_Click);
            // 
            // AdvancedSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(380, 137);
            this.Controls.Add(this.AdvancedSettingOkayBtn);
            this.Controls.Add(this.AdvancedSettingCancelBtn);
            this.Controls.Add(this.uxEnvironmentPicker);
            this.Controls.Add(this.EnvironmentLbl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AdvancedSettings";
            this.ShowIcon = false;
            this.Text = "Advanced Options";
            this.Load += new System.EventHandler(this.AdvancedSettings_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label EnvironmentLbl;
        private System.Windows.Forms.ComboBox uxEnvironmentPicker;
        private System.Windows.Forms.Button AdvancedSettingCancelBtn;
        private System.Windows.Forms.Button AdvancedSettingOkayBtn;
    }
}