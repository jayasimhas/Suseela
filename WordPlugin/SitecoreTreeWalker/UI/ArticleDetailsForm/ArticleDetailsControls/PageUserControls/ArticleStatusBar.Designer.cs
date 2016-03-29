namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleStatusBar
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ArticleStatusBar));
            this.uxArticleStatusBar = new System.Windows.Forms.ToolStrip();
            this.uxArticleNumberLabel = new System.Windows.Forms.ToolStripLabel();
            this.uxArticleNumber = new System.Windows.Forms.ToolStripTextBox();
            this.uxLinkUnlinkButton = new System.Windows.Forms.ToolStripButton();
            this.uxLockStateButton = new System.Windows.Forms.ToolStripButton();
            this.uxVersionStateButton = new System.Windows.Forms.ToolStripButton();
            this.uxWorkflowButton = new System.Windows.Forms.ToolStripButton();
            this.uxArticleStatusBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxArticleStatusBar
            // 
            this.uxArticleStatusBar.AllowMerge = false;
            this.uxArticleStatusBar.BackColor = System.Drawing.Color.Transparent;
            this.uxArticleStatusBar.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.uxArticleStatusBar.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.uxArticleStatusBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxArticleNumberLabel,
            this.uxArticleNumber,
            this.uxLinkUnlinkButton,
            this.uxLockStateButton,
            this.uxVersionStateButton,
            this.uxWorkflowButton});
            this.uxArticleStatusBar.Location = new System.Drawing.Point(0, 0);
            this.uxArticleStatusBar.Name = "uxArticleStatusBar";
            this.uxArticleStatusBar.Size = new System.Drawing.Size(1059, 27);
            this.uxArticleStatusBar.TabIndex = 0;
            this.uxArticleStatusBar.Text = "Status Bar";
            // 
            // uxArticleNumberLabel
            // 
            this.uxArticleNumberLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxArticleNumberLabel.Margin = new System.Windows.Forms.Padding(10, 1, 0, 2);
            this.uxArticleNumberLabel.Name = "uxArticleNumberLabel";
            this.uxArticleNumberLabel.Size = new System.Drawing.Size(112, 24);
            this.uxArticleNumberLabel.Text = "Article Number : ";
            // 
            // uxArticleNumber
            // 
            this.uxArticleNumber.BackColor = this.BackColor;
            this.uxArticleNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uxArticleNumber.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxArticleNumber.Name = "uxArticleNumber";
            this.uxArticleNumber.ReadOnly = true;
            this.uxArticleNumber.Size = new System.Drawing.Size(125, 27);
            this.uxArticleNumber.Text = "Document Unlinked";
            // 
            // uxLinkUnlinkButton
            // 
            this.uxLinkUnlinkButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uxLinkUnlinkButton.Image = global::InformaSitecoreWord.Properties.Resources.link_32;
            this.uxLinkUnlinkButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxLinkUnlinkButton.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.uxLinkUnlinkButton.Name = "uxLinkUnlinkButton";
            this.uxLinkUnlinkButton.Size = new System.Drawing.Size(102, 24);
            this.uxLinkUnlinkButton.Text = "Link/Unlink";
            this.uxLinkUnlinkButton.Click += new System.EventHandler(this.uxLinkUnlinkButton_Click);
            // 
            // uxLockStateButton
            // 
            this.uxLockStateButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uxLockStateButton.Image = global::InformaSitecoreWord.Properties.Resources.lockedIcon;
            this.uxLockStateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxLockStateButton.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.uxLockStateButton.Name = "uxLockStateButton";
            this.uxLockStateButton.Size = new System.Drawing.Size(75, 24);
            this.uxLockStateButton.Text = "Unlock";
            this.uxLockStateButton.Click += new System.EventHandler(this.uxLockStateButton_Click);
            // 
            // uxVersionStateButton
            // 
            this.uxVersionStateButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uxVersionStateButton.Image = ((System.Drawing.Image)(resources.GetObject("uxVersionStateButton.Image")));
            this.uxVersionStateButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxVersionStateButton.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.uxVersionStateButton.Name = "uxVersionStateButton";
            this.uxVersionStateButton.Size = new System.Drawing.Size(79, 24);
            this.uxVersionStateButton.Text = "Version";
            this.uxVersionStateButton.Click += new System.EventHandler(this.uxVersionStateButton_Click);
            // 
            // uxWorkflowButton
            // 
            this.uxWorkflowButton.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uxWorkflowButton.Image = ((System.Drawing.Image)(resources.GetObject("uxWorkflowButton.Image")));
            this.uxWorkflowButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.uxWorkflowButton.Margin = new System.Windows.Forms.Padding(6, 1, 0, 2);
            this.uxWorkflowButton.Name = "uxWorkflowButton";
            this.uxWorkflowButton.Size = new System.Drawing.Size(126, 24);
            this.uxWorkflowButton.Text = "Workflow State";
            this.uxWorkflowButton.Click += new System.EventHandler(this.uxWorkflowButton_Click);
            // 
            // ArticleStatusBar
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.uxArticleStatusBar);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "ArticleStatusBar";
            this.Size = new System.Drawing.Size(1059, 50);
            this.uxArticleStatusBar.ResumeLayout(false);
            this.uxArticleStatusBar.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ToolStrip uxArticleStatusBar;
		private System.Windows.Forms.ToolStripLabel uxArticleNumberLabel;
		private System.Windows.Forms.ToolStripTextBox uxArticleNumber;
		private System.Windows.Forms.ToolStripButton uxLinkUnlinkButton;
		private System.Windows.Forms.ToolStripButton uxLockStateButton;
		public System.Windows.Forms.ToolStripButton uxVersionStateButton;
		private System.Windows.Forms.ToolStripButton uxWorkflowButton;

	}
}
