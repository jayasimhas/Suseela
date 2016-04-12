namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleLockInfo
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
			this.label88 = new System.Windows.Forms.Label();
			this.uxRefreshStatus = new System.Windows.Forms.Button();
			this.uxLockStatusLabel = new System.Windows.Forms.Label();
			this.uxLockUser = new System.Windows.Forms.Label();
			this.uxLockButton = new System.Windows.Forms.Button();
			this.uxUnlockButton = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// label88
			// 
			this.label88.AutoSize = true;
			this.label88.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.label88.Location = new System.Drawing.Point(33, 34);
			this.label88.Margin = new System.Windows.Forms.Padding(0);
			this.label88.Name = "label88";
			this.label88.Size = new System.Drawing.Size(23, 13);
			this.label88.TabIndex = 50;
			this.label88.Text = "By:";
			// 
			// uxRefreshStatus
			// 
			this.uxRefreshStatus.Location = new System.Drawing.Point(36, 70);
			this.uxRefreshStatus.Name = "uxRefreshStatus";
			this.uxRefreshStatus.Size = new System.Drawing.Size(105, 21);
			this.uxRefreshStatus.TabIndex = 52;
			this.uxRefreshStatus.Text = "Refresh Status";
			this.uxRefreshStatus.UseVisualStyleBackColor = true;
			this.uxRefreshStatus.Click += new System.EventHandler(this.uxRefreshStatus_Click);
			// 
			// uxLockStatusLabel
			// 
			this.uxLockStatusLabel.AutoSize = true;
			this.uxLockStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.uxLockStatusLabel.Location = new System.Drawing.Point(33, 19);
			this.uxLockStatusLabel.Margin = new System.Windows.Forms.Padding(0);
			this.uxLockStatusLabel.Name = "uxLockStatusLabel";
			this.uxLockStatusLabel.Size = new System.Drawing.Size(56, 13);
			this.uxLockStatusLabel.TabIndex = 49;
			this.uxLockStatusLabel.Text = "Unlocked";
			// 
			// uxLockUser
			// 
			this.uxLockUser.AutoSize = true;
			this.uxLockUser.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxLockUser.Location = new System.Drawing.Point(55, 35);
			this.uxLockUser.Margin = new System.Windows.Forms.Padding(0);
			this.uxLockUser.Name = "uxLockUser";
			this.uxLockUser.Size = new System.Drawing.Size(58, 13);
			this.uxLockUser.TabIndex = 51;
			this.uxLockUser.Text = "Username";
			// 
			// uxLockButton
			// 
			this.uxLockButton.Location = new System.Drawing.Point(147, 70);
			this.uxLockButton.Name = "uxLockButton";
			this.uxLockButton.Size = new System.Drawing.Size(75, 21);
			this.uxLockButton.TabIndex = 53;
			this.uxLockButton.Text = "Lock";
			this.uxLockButton.UseVisualStyleBackColor = true;
			this.uxLockButton.Click += new System.EventHandler(this.uxLockButton_Click);
			// 
			// uxUnlockButton
			// 
			this.uxUnlockButton.Location = new System.Drawing.Point(147, 70);
			this.uxUnlockButton.Name = "uxUnlockButton";
			this.uxUnlockButton.Size = new System.Drawing.Size(75, 21);
			this.uxUnlockButton.TabIndex = 54;
			this.uxUnlockButton.Text = "Unlock";
			this.uxUnlockButton.UseVisualStyleBackColor = true;
			this.uxUnlockButton.Click += new System.EventHandler(this.uxUnlockButton_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.SystemColors.HotTrack;
			this.label1.Location = new System.Drawing.Point(60, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 55;
			this.label1.Click += new System.EventHandler(this.label1_Click);
			// 
			// ArticleLockInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 100);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.uxUnlockButton);
			this.Controls.Add(this.label88);
			this.Controls.Add(this.uxRefreshStatus);
			this.Controls.Add(this.uxLockStatusLabel);
			this.Controls.Add(this.uxLockUser);
			this.Controls.Add(this.uxLockButton);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ArticleLockInfo";
			this.ShowIcon = false;
			this.Text = "Article Lock and Unlock";
			this.Load += new System.EventHandler(this.ArticleLockInfo_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

        private System.Windows.Forms.Label label88;
        private System.Windows.Forms.Button uxRefreshStatus;
        private System.Windows.Forms.Label uxLockStatusLabel;
        private System.Windows.Forms.Label uxLockUser;
        private System.Windows.Forms.Button uxLockButton;
        private System.Windows.Forms.Button uxUnlockButton;
		private System.Windows.Forms.Label label1;
	}
}