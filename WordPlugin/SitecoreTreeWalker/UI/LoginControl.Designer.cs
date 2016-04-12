namespace InformaSitecoreWord.UI
{
    partial class LoginControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginControl));
			this.uxLoginButton = new System.Windows.Forms.Button();
			this.uxUsername = new System.Windows.Forms.TextBox();
			this.uxPassword = new System.Windows.Forms.TextBox();
			this.uxRememberPassword = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.uxConnectionError = new System.Windows.Forms.Panel();
			this.uxErrorMessage = new System.Windows.Forms.Label();
			this.uxVersionNumber = new System.Windows.Forms.Label();
			this.uxForgotPassword = new System.Windows.Forms.LinkLabel();
			this.AdvancedSettingsLbl = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.uxConnectionError.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxLoginButton
			// 
			this.uxLoginButton.Location = new System.Drawing.Point(299, 363);
			this.uxLoginButton.Name = "uxLoginButton";
			this.uxLoginButton.Size = new System.Drawing.Size(75, 23);
			this.uxLoginButton.TabIndex = 2;
			this.uxLoginButton.Text = "Login";
			this.uxLoginButton.UseVisualStyleBackColor = true;
			this.uxLoginButton.Click += new System.EventHandler(this.uxLoginButton_Click);
			// 
			// uxUsername
			// 
			this.uxUsername.Location = new System.Drawing.Point(110, 273);
			this.uxUsername.Name = "uxUsername";
			this.uxUsername.Size = new System.Drawing.Size(264, 22);
			this.uxUsername.TabIndex = 0;
			this.uxUsername.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxUsername_KeyDown);
			// 
			// uxPassword
			// 
			this.uxPassword.Location = new System.Drawing.Point(110, 304);
			this.uxPassword.Name = "uxPassword";
			this.uxPassword.Size = new System.Drawing.Size(264, 22);
			this.uxPassword.TabIndex = 1;
			this.uxPassword.UseSystemPasswordChar = true;
			this.uxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxPassword_KeyDown);
			// 
			// uxRememberPassword
			// 
			this.uxRememberPassword.AutoSize = true;
			this.uxRememberPassword.Location = new System.Drawing.Point(110, 332);
			this.uxRememberPassword.Name = "uxRememberPassword";
			this.uxRememberPassword.Size = new System.Drawing.Size(99, 17);
			this.uxRememberPassword.TabIndex = 3;
			this.uxRememberPassword.TabStop = false;
			this.uxRememberPassword.Text = "Remember Me";
			this.uxRememberPassword.UseVisualStyleBackColor = true;
			this.uxRememberPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxRememberPassword_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Location = new System.Drawing.Point(3, 276);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(87, 19);
			this.label1.TabIndex = 4;
			this.label1.Text = "Username";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Gray;
			this.label2.Location = new System.Drawing.Point(4, 307);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(86, 19);
			this.label2.TabIndex = 5;
			this.label2.Text = "Password";
			// 
			// uxConnectionError
			// 
			this.uxConnectionError.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
			this.uxConnectionError.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxConnectionError.Controls.Add(this.uxErrorMessage);
			this.uxConnectionError.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
			this.uxConnectionError.Location = new System.Drawing.Point(110, 27);
			this.uxConnectionError.Name = "uxConnectionError";
			this.uxConnectionError.Size = new System.Drawing.Size(264, 44);
			this.uxConnectionError.TabIndex = 7;
			this.uxConnectionError.Visible = false;
			// 
			// uxErrorMessage
			// 
			this.uxErrorMessage.AutoSize = true;
			this.uxErrorMessage.Font = new System.Drawing.Font("Segoe UI", 8.75F, System.Drawing.FontStyle.Bold);
			this.uxErrorMessage.Location = new System.Drawing.Point(18, 13);
			this.uxErrorMessage.MaximumSize = new System.Drawing.Size(225, 0);
			this.uxErrorMessage.Name = "uxErrorMessage";
			this.uxErrorMessage.Size = new System.Drawing.Size(213, 15);
			this.uxErrorMessage.TabIndex = 0;
			this.uxErrorMessage.Text = "Sitecore server cannot be contacted.";
			// 
			// uxVersionNumber
			// 
			this.uxVersionNumber.AutoSize = true;
			this.uxVersionNumber.Location = new System.Drawing.Point(298, 427);
			this.uxVersionNumber.Name = "uxVersionNumber";
			this.uxVersionNumber.Size = new System.Drawing.Size(76, 13);
			this.uxVersionNumber.TabIndex = 8;
			this.uxVersionNumber.Text = "xxx.xxx.xxx.xxx";
			// 
			// uxForgotPassword
			// 
			this.uxForgotPassword.AutoSize = true;
			this.uxForgotPassword.Location = new System.Drawing.Point(275, 336);
			this.uxForgotPassword.Name = "uxForgotPassword";
			this.uxForgotPassword.Size = new System.Drawing.Size(99, 13);
			this.uxForgotPassword.TabIndex = 9;
			this.uxForgotPassword.TabStop = true;
			this.uxForgotPassword.Text = "Forgot Password?";
			this.uxForgotPassword.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.uxForgotPassword_LinkClicked);
			// 
			// AdvancedSettingsLbl
			// 
			this.AdvancedSettingsLbl.AutoSize = true;
			this.AdvancedSettingsLbl.Location = new System.Drawing.Point(199, 427);
			this.AdvancedSettingsLbl.Name = "AdvancedSettingsLbl";
			this.AdvancedSettingsLbl.Size = new System.Drawing.Size(102, 13);
			this.AdvancedSettingsLbl.TabIndex = 10;
			this.AdvancedSettingsLbl.TabStop = true;
			this.AdvancedSettingsLbl.Text = "Advanced Options";
			this.AdvancedSettingsLbl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AdvancedSettingsLbl_LinkClicked);
			// 
			// label3
			// 
			this.label3.Image = ((System.Drawing.Image)(resources.GetObject("label3.Image")));
			this.label3.Location = new System.Drawing.Point(40, 157);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(400, 150);
			this.label3.TabIndex = 6;
			// 
			// LoginControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.AdvancedSettingsLbl);
			this.Controls.Add(this.uxForgotPassword);
			this.Controls.Add(this.uxVersionNumber);
			this.Controls.Add(this.uxConnectionError);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uxRememberPassword);
			this.Controls.Add(this.uxPassword);
			this.Controls.Add(this.uxUsername);
			this.Controls.Add(this.uxLoginButton);
			this.Controls.Add(this.label3);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "LoginControl";
			this.Size = new System.Drawing.Size(443, 440);
			this.Load += new System.EventHandler(this.LoginControl_Load);
			this.uxConnectionError.ResumeLayout(false);
			this.uxConnectionError.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button uxLoginButton;
        private System.Windows.Forms.TextBox uxUsername;
        private System.Windows.Forms.TextBox uxPassword;
        private System.Windows.Forms.CheckBox uxRememberPassword;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Panel uxConnectionError;
		private System.Windows.Forms.Label uxErrorMessage;
		private System.Windows.Forms.Label uxVersionNumber;
		private System.Windows.Forms.LinkLabel uxForgotPassword;
        private System.Windows.Forms.LinkLabel AdvancedSettingsLbl;
    }
}
