namespace SitecoreTreeWalker.UI
{
    partial class Login
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
			this.uxUsername = new System.Windows.Forms.TextBox();
			this.uxPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.uxLoginButton = new System.Windows.Forms.Button();
			this.uxRememberPassword = new System.Windows.Forms.CheckBox();
			this.SuspendLayout();
			// 
			// uxUsername
			// 
			this.uxUsername.Location = new System.Drawing.Point(39, 85);
			this.uxUsername.Name = "uxUsername";
			this.uxUsername.Size = new System.Drawing.Size(234, 20);
			this.uxUsername.TabIndex = 0;
			// 
			// uxPassword
			// 
			this.uxPassword.Location = new System.Drawing.Point(39, 167);
			this.uxPassword.Name = "uxPassword";
			this.uxPassword.PasswordChar = '*';
			this.uxPassword.Size = new System.Drawing.Size(234, 20);
			this.uxPassword.TabIndex = 1;
			this.uxPassword.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxPassword_KeyDown);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Location = new System.Drawing.Point(34, 54);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(93, 19);
			this.label1.TabIndex = 2;
			this.label1.Text = "Username:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label2.ForeColor = System.Drawing.Color.Gray;
			this.label2.Location = new System.Drawing.Point(35, 136);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(92, 19);
			this.label2.TabIndex = 3;
			this.label2.Text = "Password:";
			// 
			// uxLoginButton
			// 
			this.uxLoginButton.Location = new System.Drawing.Point(198, 230);
			this.uxLoginButton.Name = "uxLoginButton";
			this.uxLoginButton.Size = new System.Drawing.Size(75, 23);
			this.uxLoginButton.TabIndex = 4;
			this.uxLoginButton.Text = "Login";
			this.uxLoginButton.UseVisualStyleBackColor = true;
			this.uxLoginButton.Click += new System.EventHandler(this.uxLoginButton_Click);
			// 
			// uxRememberPassword
			// 
			this.uxRememberPassword.AutoSize = true;
			this.uxRememberPassword.Location = new System.Drawing.Point(38, 193);
			this.uxRememberPassword.Name = "uxRememberPassword";
			this.uxRememberPassword.Size = new System.Drawing.Size(131, 17);
			this.uxRememberPassword.TabIndex = 5;
			this.uxRememberPassword.Text = "Remember password?";
			this.uxRememberPassword.UseVisualStyleBackColor = true;
			// 
			// Login
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(358, 302);
			this.Controls.Add(this.uxRememberPassword);
			this.Controls.Add(this.uxLoginButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uxPassword);
			this.Controls.Add(this.uxUsername);
			this.Name = "Login";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Login";
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox uxUsername;
        private System.Windows.Forms.TextBox uxPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button uxLoginButton;
        private System.Windows.Forms.CheckBox uxRememberPassword;
    }
}