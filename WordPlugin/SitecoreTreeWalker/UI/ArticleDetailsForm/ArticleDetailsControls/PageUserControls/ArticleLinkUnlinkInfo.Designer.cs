namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleLinkUnlinkInfo
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
			this.uxLinkedTo = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.uxUnlinkButton = new System.Windows.Forms.Button();
			this.uxLinkDocument = new System.Windows.Forms.Button();
			this.uxArticleNumberToLink = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.uxPanelLink = new System.Windows.Forms.Panel();
			this.uxPanelUnlink = new System.Windows.Forms.Panel();
			this.uxPanelLink.SuspendLayout();
			this.uxPanelUnlink.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxLinkedTo
			// 
			this.uxLinkedTo.AutoSize = true;
			this.uxLinkedTo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxLinkedTo.Location = new System.Drawing.Point(70, 25);
			this.uxLinkedTo.Name = "uxLinkedTo";
			this.uxLinkedTo.Size = new System.Drawing.Size(27, 13);
			this.uxLinkedTo.TabIndex = 1;
			this.uxLinkedTo.Text = "xxxx";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.label1.Location = new System.Drawing.Point(3, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(61, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Linked to :";
			// 
			// uxUnlinkButton
			// 
			this.uxUnlinkButton.Location = new System.Drawing.Point(124, 52);
			this.uxUnlinkButton.Name = "uxUnlinkButton";
			this.uxUnlinkButton.Size = new System.Drawing.Size(75, 23);
			this.uxUnlinkButton.TabIndex = 2;
			this.uxUnlinkButton.Text = "Unlink";
			this.uxUnlinkButton.UseVisualStyleBackColor = true;
			this.uxUnlinkButton.Click += new System.EventHandler(this.uxUnlinkButton_Click);
			// 
			// uxLinkDocument
			// 
			this.uxLinkDocument.Location = new System.Drawing.Point(208, 34);
			this.uxLinkDocument.Name = "uxLinkDocument";
			this.uxLinkDocument.Size = new System.Drawing.Size(92, 21);
			this.uxLinkDocument.TabIndex = 20;
			this.uxLinkDocument.Text = "Link Document";
			this.uxLinkDocument.UseVisualStyleBackColor = true;
			this.uxLinkDocument.Click += new System.EventHandler(this.uxLinkDocument_Click);
			// 
			// uxArticleNumberToLink
			// 
			this.uxArticleNumberToLink.Location = new System.Drawing.Point(102, 35);
			this.uxArticleNumberToLink.Name = "uxArticleNumberToLink";
			this.uxArticleNumberToLink.Size = new System.Drawing.Size(100, 20);
			this.uxArticleNumberToLink.TabIndex = 19;
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
			this.label6.Location = new System.Drawing.Point(3, 38);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(93, 14);
			this.label6.TabIndex = 18;
			this.label6.Text = "Article Number:";
			// 
			// uxPanelLink
			// 
			this.uxPanelLink.Controls.Add(this.uxArticleNumberToLink);
			this.uxPanelLink.Controls.Add(this.label6);
			this.uxPanelLink.Controls.Add(this.uxLinkDocument);
			this.uxPanelLink.Location = new System.Drawing.Point(31, 12);
			this.uxPanelLink.Name = "uxPanelLink";
			this.uxPanelLink.Size = new System.Drawing.Size(312, 100);
			this.uxPanelLink.TabIndex = 21;
			// 
			// uxPanelUnlink
			// 
			this.uxPanelUnlink.Controls.Add(this.uxUnlinkButton);
			this.uxPanelUnlink.Controls.Add(this.uxLinkedTo);
			this.uxPanelUnlink.Controls.Add(this.label1);
			this.uxPanelUnlink.Location = new System.Drawing.Point(34, 12);
			this.uxPanelUnlink.Name = "uxPanelUnlink";
			this.uxPanelUnlink.Size = new System.Drawing.Size(309, 78);
			this.uxPanelUnlink.TabIndex = 22;
			// 
			// ArticleLinkUnlinkInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(388, 120);
			this.Controls.Add(this.uxPanelUnlink);
			this.Controls.Add(this.uxPanelLink);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ArticleLinkUnlinkInfo";
			this.ShowIcon = false;
			this.Text = "Article Link / Unlink";
			this.Load += new System.EventHandler(this.ArticleLinkUnlinkInfo_Load);
			this.uxPanelLink.ResumeLayout(false);
			this.uxPanelLink.PerformLayout();
			this.uxPanelUnlink.ResumeLayout(false);
			this.uxPanelUnlink.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label uxLinkedTo;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button uxUnlinkButton;
        private System.Windows.Forms.Button uxLinkDocument;
        private System.Windows.Forms.TextBox uxArticleNumberToLink;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel uxPanelLink;
        private System.Windows.Forms.Panel uxPanelUnlink;
	}
}