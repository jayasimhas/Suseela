namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
	partial class ArticlesSidebarsControl
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
			this.uxRelatedArticleNumberGo = new System.Windows.Forms.Button();
			this.uxRelatedArticleNumber = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.uxSidebarArticleNumberGo = new System.Windows.Forms.Button();
			this.uxSidebarArticleNumber = new System.Windows.Forms.TextBox();
			this.uxPreviewArticle = new System.Windows.Forms.Button();
			this.uxRetrieveArticle = new System.Windows.Forms.Button();
			this.uxInsertIntoArticle = new System.Windows.Forms.Button();
			this.uxPreviewPanel = new System.Windows.Forms.Panel();
			this.uxArticlePreviewTable = new SitecoreTreeWalker.UI.ArticlePreviewControl();
			this.label5 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.HeaderPanel1 = new System.Windows.Forms.Panel();
			this.ReferencedHeaderPanel = new System.Windows.Forms.Panel();
			this.uxPreviewPanel.SuspendLayout();
			this.HeaderPanel1.SuspendLayout();
			this.ReferencedHeaderPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxRelatedArticleNumberGo
			// 
			this.uxRelatedArticleNumberGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxRelatedArticleNumberGo.AutoSize = true;
			this.uxRelatedArticleNumberGo.Location = new System.Drawing.Point(155, 49);
			this.uxRelatedArticleNumberGo.Name = "uxRelatedArticleNumberGo";
			this.uxRelatedArticleNumberGo.Size = new System.Drawing.Size(34, 23);
			this.uxRelatedArticleNumberGo.TabIndex = 12;
			this.uxRelatedArticleNumberGo.Text = "GO";
			this.uxRelatedArticleNumberGo.UseVisualStyleBackColor = true;
			this.uxRelatedArticleNumberGo.Click += new System.EventHandler(this.uxRelatedArticleNumberGo_Click);
			// 
			// uxRelatedArticleNumber
			// 
			this.uxRelatedArticleNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uxRelatedArticleNumber.Location = new System.Drawing.Point(4, 51);
			this.uxRelatedArticleNumber.Name = "uxRelatedArticleNumber";
			this.uxRelatedArticleNumber.Size = new System.Drawing.Size(146, 22);
			this.uxRelatedArticleNumber.TabIndex = 11;
			this.uxRelatedArticleNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxRelatedArticleNumber_KeyDown);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.label3.Location = new System.Drawing.Point(1, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(117, 13);
			this.label3.TabIndex = 10;
			this.label3.Text = "Look-Up by Article #:";
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.label7.Location = new System.Drawing.Point(1, 115);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(117, 13);
			this.label7.TabIndex = 18;
			this.label7.Text = "Look-Up by Article #:";
			// 
			// uxSidebarArticleNumberGo
			// 
			this.uxSidebarArticleNumberGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSidebarArticleNumberGo.AutoSize = true;
			this.uxSidebarArticleNumberGo.Location = new System.Drawing.Point(155, 130);
			this.uxSidebarArticleNumberGo.Name = "uxSidebarArticleNumberGo";
			this.uxSidebarArticleNumberGo.Size = new System.Drawing.Size(34, 23);
			this.uxSidebarArticleNumberGo.TabIndex = 20;
			this.uxSidebarArticleNumberGo.Text = "GO";
			this.uxSidebarArticleNumberGo.UseVisualStyleBackColor = true;
			this.uxSidebarArticleNumberGo.Click += new System.EventHandler(this.uxSidebarArticleNumberGo_Click);
			// 
			// uxSidebarArticleNumber
			// 
			this.uxSidebarArticleNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uxSidebarArticleNumber.Location = new System.Drawing.Point(4, 132);
			this.uxSidebarArticleNumber.Name = "uxSidebarArticleNumber";
			this.uxSidebarArticleNumber.Size = new System.Drawing.Size(146, 22);
			this.uxSidebarArticleNumber.TabIndex = 19;
			this.uxSidebarArticleNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxSidebarArticleNumber_KeyDown);
			// 
			// uxPreviewArticle
			// 
			this.uxPreviewArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxPreviewArticle.Enabled = false;
			this.uxPreviewArticle.Location = new System.Drawing.Point(4, 321);
			this.uxPreviewArticle.Name = "uxPreviewArticle";
			this.uxPreviewArticle.Size = new System.Drawing.Size(185, 23);
			this.uxPreviewArticle.TabIndex = 22;
			this.uxPreviewArticle.Text = "Preview Referenced Article";
			this.uxPreviewArticle.UseVisualStyleBackColor = true;
			this.uxPreviewArticle.Click += new System.EventHandler(this.uxPreviewArticle_Click);
			// 
			// uxRetrieveArticle
			// 
			this.uxRetrieveArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxRetrieveArticle.Location = new System.Drawing.Point(5, 350);
			this.uxRetrieveArticle.Name = "uxRetrieveArticle";
			this.uxRetrieveArticle.Size = new System.Drawing.Size(185, 23);
			this.uxRetrieveArticle.TabIndex = 23;
			this.uxRetrieveArticle.Text = "Retrieve Article";
			this.uxRetrieveArticle.UseVisualStyleBackColor = true;
			this.uxRetrieveArticle.Click += new System.EventHandler(this.uxRetrieveArticle_Click);
			// 
			// uxInsertIntoArticle
			// 
			this.uxInsertIntoArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxInsertIntoArticle.Location = new System.Drawing.Point(4, 350);
			this.uxInsertIntoArticle.Name = "uxInsertIntoArticle";
			this.uxInsertIntoArticle.Size = new System.Drawing.Size(185, 23);
			this.uxInsertIntoArticle.TabIndex = 24;
			this.uxInsertIntoArticle.Text = "Insert into Article";
			this.uxInsertIntoArticle.UseVisualStyleBackColor = true;
			this.uxInsertIntoArticle.Visible = false;
			this.uxInsertIntoArticle.Click += new System.EventHandler(this.uxInsertIntoArticle_Click);
			// 
			// uxPreviewPanel
			// 
			this.uxPreviewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uxPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPreviewPanel.Controls.Add(this.uxArticlePreviewTable);
			this.uxPreviewPanel.Location = new System.Drawing.Point(4, 160);
			this.uxPreviewPanel.Name = "uxPreviewPanel";
			this.uxPreviewPanel.Size = new System.Drawing.Size(185, 155);
			this.uxPreviewPanel.TabIndex = 0;
			// 
			// uxArticlePreviewTable
			// 
			this.uxArticlePreviewTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.uxArticlePreviewTable.ColumnCount = 2;
			this.uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
			this.uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.uxArticlePreviewTable.Location = new System.Drawing.Point(0, -1);
			this.uxArticlePreviewTable.Name = "uxArticlePreviewTable";
			this.uxArticlePreviewTable.RowCount = 4;
			this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
			this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
			this.uxArticlePreviewTable.Size = new System.Drawing.Size(185, 150);
			this.uxArticlePreviewTable.TabIndex = 25;
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = System.Drawing.Color.Gray;
			this.label5.Image = global::SitecoreTreeWalker.Properties.Resources.TabHeaders_sidebars;
			this.label5.Location = new System.Drawing.Point(0, 0);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(310, 24);
			this.label5.TabIndex = 16;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Image = global::SitecoreTreeWalker.Properties.Resources.articlessidebars_browser;
			this.label1.Location = new System.Drawing.Point(0, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(310, 24);
			this.label1.TabIndex = 8;
			// 
			// HeaderPanel1
			// 
			this.HeaderPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.HeaderPanel1.Controls.Add(this.label5);
			this.HeaderPanel1.Location = new System.Drawing.Point(0, 85);
			this.HeaderPanel1.Name = "HeaderPanel1";
			this.HeaderPanel1.Size = new System.Drawing.Size(200, 27);
			this.HeaderPanel1.TabIndex = 25;
			// 
			// ReferencedHeaderPanel
			// 
			this.ReferencedHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.ReferencedHeaderPanel.Controls.Add(this.label1);
			this.ReferencedHeaderPanel.Location = new System.Drawing.Point(0, 0);
			this.ReferencedHeaderPanel.Name = "ReferencedHeaderPanel";
			this.ReferencedHeaderPanel.Size = new System.Drawing.Size(200, 27);
			this.ReferencedHeaderPanel.TabIndex = 22;
			// 
			// ArticlesSidebarsControl
			// 
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.ReferencedHeaderPanel);
			this.Controls.Add(this.HeaderPanel1);
			this.Controls.Add(this.uxPreviewPanel);
			this.Controls.Add(this.uxInsertIntoArticle);
			this.Controls.Add(this.uxRetrieveArticle);
			this.Controls.Add(this.uxPreviewArticle);
			this.Controls.Add(this.uxSidebarArticleNumberGo);
			this.Controls.Add(this.uxSidebarArticleNumber);
			this.Controls.Add(this.label7);
			this.Controls.Add(this.uxRelatedArticleNumberGo);
			this.Controls.Add(this.uxRelatedArticleNumber);
			this.Controls.Add(this.label3);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "ArticlesSidebarsControl";
			this.Size = new System.Drawing.Size(200, 550);
			this.uxPreviewPanel.ResumeLayout(false);
			this.HeaderPanel1.ResumeLayout(false);
			this.ReferencedHeaderPanel.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button uxRelatedArticleNumberGo;
		private System.Windows.Forms.TextBox uxRelatedArticleNumber;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Button uxSidebarArticleNumberGo;
		private System.Windows.Forms.TextBox uxSidebarArticleNumber;
		private System.Windows.Forms.Button uxPreviewArticle;
		private System.Windows.Forms.Button uxRetrieveArticle;
		private System.Windows.Forms.Button uxInsertIntoArticle;
		private ArticlePreviewControl uxArticlePreviewTable;
		private System.Windows.Forms.Panel uxPreviewPanel;
		private System.Windows.Forms.Panel HeaderPanel1;
		private System.Windows.Forms.Panel ReferencedHeaderPanel;

	}
}
