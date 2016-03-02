namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
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
            this.uxSidebarArticleNumberGo = new System.Windows.Forms.Button();
            this.uxSidebarArticleNumber = new System.Windows.Forms.TextBox();
            this.uxPreviewArticle = new System.Windows.Forms.Button();
            this.uxRetrieveArticle = new System.Windows.Forms.Button();
            this.uxInsertIntoArticle = new System.Windows.Forms.Button();
            this.uxPreviewPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ReferencedHeaderPanel = new System.Windows.Forms.Panel();
            this.uxResultsLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.uxArticlePreviewTable = new ArticlePreviewControl();
            this.uxPreviewPanel.SuspendLayout();
            this.ReferencedHeaderPanel.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // uxRelatedArticleNumberGo
            // 
            this.uxRelatedArticleNumberGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxRelatedArticleNumberGo.AutoSize = true;
            this.uxRelatedArticleNumberGo.Location = new System.Drawing.Point(264, 20);
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
            this.uxRelatedArticleNumber.Location = new System.Drawing.Point(6, 21);
            this.uxRelatedArticleNumber.Name = "uxRelatedArticleNumber";
            this.uxRelatedArticleNumber.Size = new System.Drawing.Size(252, 22);
            this.uxRelatedArticleNumber.TabIndex = 11;
            this.uxRelatedArticleNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxRelatedArticleNumber_KeyDown);
            // 
            // uxSidebarArticleNumberGo
            // 
            this.uxSidebarArticleNumberGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSidebarArticleNumberGo.AutoSize = true;
            this.uxSidebarArticleNumberGo.Location = new System.Drawing.Point(264, 20);
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
            this.uxSidebarArticleNumber.Location = new System.Drawing.Point(6, 21);
            this.uxSidebarArticleNumber.Name = "uxSidebarArticleNumber";
            this.uxSidebarArticleNumber.Size = new System.Drawing.Size(255, 22);
            this.uxSidebarArticleNumber.TabIndex = 19;
            this.uxSidebarArticleNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxSidebarArticleNumber_KeyDown);
            // 
            // uxPreviewArticle
            // 
            this.uxPreviewArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.uxPreviewArticle.Enabled = false;
            this.uxPreviewArticle.Location = new System.Drawing.Point(61, 365);
            this.uxPreviewArticle.Name = "uxPreviewArticle";
            this.uxPreviewArticle.Size = new System.Drawing.Size(185, 23);
            this.uxPreviewArticle.TabIndex = 22;
            this.uxPreviewArticle.Text = "Preview Article";
            this.uxPreviewArticle.UseVisualStyleBackColor = true;
            this.uxPreviewArticle.Click += new System.EventHandler(this.uxPreviewArticle_Click);
            // 
            // uxRetrieveArticle
            // 
            this.uxRetrieveArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.uxRetrieveArticle.Location = new System.Drawing.Point(61, 394);
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
            this.uxInsertIntoArticle.Location = new System.Drawing.Point(61, 394);
            this.uxInsertIntoArticle.Name = "uxInsertIntoArticle";
            this.uxInsertIntoArticle.Size = new System.Drawing.Size(185, 23);
            this.uxInsertIntoArticle.TabIndex = 24;
            this.uxInsertIntoArticle.Text = "Insert";
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
            this.uxPreviewPanel.Location = new System.Drawing.Point(4, 185);
            this.uxPreviewPanel.Name = "uxPreviewPanel";
            this.uxPreviewPanel.Size = new System.Drawing.Size(300, 155);
            this.uxPreviewPanel.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Image = global::InformaSitecoreWord.Properties.Resources.articlessidebars_browser;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(310, 24);
            this.label1.TabIndex = 8;
            // 
            // ReferencedHeaderPanel
            // 
            this.ReferencedHeaderPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ReferencedHeaderPanel.Controls.Add(this.label1);
            this.ReferencedHeaderPanel.Location = new System.Drawing.Point(0, 0);
            this.ReferencedHeaderPanel.Name = "ReferencedHeaderPanel";
            this.ReferencedHeaderPanel.Size = new System.Drawing.Size(315, 27);
            this.ReferencedHeaderPanel.TabIndex = 22;
            // 
            // uxResultsLabel
            // 
            this.uxResultsLabel.AutoSize = true;
            this.uxResultsLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.uxResultsLabel.Location = new System.Drawing.Point(1, 163);
            this.uxResultsLabel.Name = "uxResultsLabel";
            this.uxResultsLabel.Size = new System.Drawing.Size(47, 13);
            this.uxResultsLabel.TabIndex = 26;
            this.uxResultsLabel.Text = "Results:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.uxRelatedArticleNumber);
            this.groupBox1.Controls.Add(this.uxRelatedArticleNumberGo);
            this.groupBox1.Location = new System.Drawing.Point(4, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(304, 53);
            this.groupBox1.TabIndex = 27;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Article Number :";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uxSidebarArticleNumber);
            this.groupBox2.Controls.Add(this.uxSidebarArticleNumberGo);
            this.groupBox2.Location = new System.Drawing.Point(4, 92);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(304, 60);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Sidebars Article Number :";
            // 
            // uxArticlePreviewTable
            // 
            this.uxArticlePreviewTable.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxArticlePreviewTable.ColumnCount = 2;
            this.uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxArticlePreviewTable.Location = new System.Drawing.Point(-3, 2);
            this.uxArticlePreviewTable.Name = "uxArticlePreviewTable";
            this.uxArticlePreviewTable.RowCount = 4;
            this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.66667F));
            this.uxArticlePreviewTable.Size = new System.Drawing.Size(300, 150);
            this.uxArticlePreviewTable.TabIndex = 25;
            // 
            // ArticlesSidebarsControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.uxResultsLabel);
            this.Controls.Add(this.ReferencedHeaderPanel);
            this.Controls.Add(this.uxPreviewPanel);
            this.Controls.Add(this.uxInsertIntoArticle);
            this.Controls.Add(this.uxRetrieveArticle);
            this.Controls.Add(this.uxPreviewArticle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "ArticlesSidebarsControl";
            this.Size = new System.Drawing.Size(315, 550);
            this.uxPreviewPanel.ResumeLayout(false);
            this.ReferencedHeaderPanel.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button uxRelatedArticleNumberGo;
        private System.Windows.Forms.TextBox uxRelatedArticleNumber;
        private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button uxSidebarArticleNumberGo;
		private System.Windows.Forms.TextBox uxSidebarArticleNumber;
		private System.Windows.Forms.Button uxPreviewArticle;
		private System.Windows.Forms.Button uxRetrieveArticle;
		private System.Windows.Forms.Button uxInsertIntoArticle;
		private ArticlePreviewControl uxArticlePreviewTable;
        private System.Windows.Forms.Panel uxPreviewPanel;
		private System.Windows.Forms.Panel ReferencedHeaderPanel;
        private System.Windows.Forms.Label uxResultsLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;

	}
}
