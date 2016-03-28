namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class RelatedArticlesControl
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
			this.label53 = new System.Windows.Forms.Label();
			this.panel1 = new System.Windows.Forms.Panel();
			this.uxRetrieveArticle = new System.Windows.Forms.Button();
			this.uxAddToRelated = new System.Windows.Forms.Button();
			this.uxPreviewPanel = new System.Windows.Forms.Panel();
			this._uxArticlePreviewTable = new ArticlePreviewControl();
			this.uxViewArticle = new System.Windows.Forms.Button();
			this.uxArticleNumber = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.uxArticleInformationHeaderLabel = new System.Windows.Forms.Label();
			this.uxSelectedPanel = new System.Windows.Forms.Panel();
			this.uxSelectedLayout = new SelectedRelatedArticles();
			this.label5 = new System.Windows.Forms.Label();
			this.panel1.SuspendLayout();
			this.uxPreviewPanel.SuspendLayout();
			this.uxSelectedPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// label53
			// 
			this.label53.AutoSize = true;
			this.label53.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label53.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label53.Location = new System.Drawing.Point(4, 37);
			this.label53.Name = "label53";
			this.label53.Size = new System.Drawing.Size(54, 19);
			this.label53.TabIndex = 21;
			this.label53.Text = "Search";
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left)| System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.uxRetrieveArticle);
			this.panel1.Controls.Add(this.uxAddToRelated);
			this.panel1.Controls.Add(this.uxPreviewPanel);
			this.panel1.Controls.Add(this.uxViewArticle);
			this.panel1.Controls.Add(this.uxArticleNumber);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Location = new System.Drawing.Point(0, 56);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(610, 168);
			this.panel1.TabIndex = 22;
			// 
			// uxRetrieveArticle
			// 
			this.uxRetrieveArticle.Location = new System.Drawing.Point(543, 20);
			this.uxRetrieveArticle.Name = "uxRetrieveArticle";
			this.uxRetrieveArticle.Size = new System.Drawing.Size(101, 23);
			this.uxRetrieveArticle.TabIndex = 22;
			this.uxRetrieveArticle.Text = "Search";
			this.uxRetrieveArticle.UseVisualStyleBackColor = true;
			this.uxRetrieveArticle.Click += new System.EventHandler(this.uxRetrieveArticle_Click);
			// 
			// uxAddToRelated
			// 
			this.uxAddToRelated.Location = new System.Drawing.Point(543, 79);
			this.uxAddToRelated.Name = "uxAddToRelated";
			this.uxAddToRelated.Size = new System.Drawing.Size(101, 23);
			this.uxAddToRelated.TabIndex = 26;
			this.uxAddToRelated.Text = "Add";
			this.uxAddToRelated.UseVisualStyleBackColor = true;
			this.uxAddToRelated.Visible = false;
			this.uxAddToRelated.Click += new System.EventHandler(this.uxAddToRelated_Click);
			// 
			// uxPreviewPanel
			// 
			this.uxPreviewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.uxPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPreviewPanel.Controls.Add(this._uxArticlePreviewTable);
			this.uxPreviewPanel.Location = new System.Drawing.Point(19, 49);
			this.uxPreviewPanel.Name = "uxPreviewPanel";
			this.uxPreviewPanel.Size = new System.Drawing.Size(469, 111);
			this.uxPreviewPanel.TabIndex = 25;
			// 
			// _uxArticlePreviewTable
			// 
			this._uxArticlePreviewTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this._uxArticlePreviewTable.BackColor = System.Drawing.Color.White;
			this._uxArticlePreviewTable.ColumnCount = 2;
			this._uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
			this._uxArticlePreviewTable.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
			this._uxArticlePreviewTable.Location = new System.Drawing.Point(-1, 2);
			this._uxArticlePreviewTable.Name = "_uxArticlePreviewTable";
			this._uxArticlePreviewTable.RowCount = 4;
			this._uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this._uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
			this._uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
			this._uxArticlePreviewTable.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 13F));
			this._uxArticlePreviewTable.Size = new System.Drawing.Size(469, 111);
			this._uxArticlePreviewTable.TabIndex = 24;
			// 
			// uxViewArticle
			// 
			this.uxViewArticle.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.uxViewArticle.Location = new System.Drawing.Point(494, 50);
			this.uxViewArticle.Name = "uxViewArticle";
			this.uxViewArticle.Size = new System.Drawing.Size(101, 23);
			this.uxViewArticle.TabIndex = 23;
			this.uxViewArticle.Text = "View";
			this.uxViewArticle.UseVisualStyleBackColor = true;
			this.uxViewArticle.Click += new System.EventHandler(this.uxViewArticle_Click);
			// 
			// uxArticleNumber
			// 
			this.uxArticleNumber.Location = new System.Drawing.Point(19, 21);
			this.uxArticleNumber.Name = "uxArticleNumber";
			this.uxArticleNumber.Size = new System.Drawing.Size(518, 22);
			this.uxArticleNumber.TabIndex = 17;
			this.uxArticleNumber.TextChanged += new System.EventHandler(this.uxArticleNumber_TextChanged);
			this.uxArticleNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxArticleNumber_KeyDown);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.label6.Location = new System.Drawing.Point(16, 4);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(142, 13);
			this.label6.TabIndex = 16;
			this.label6.Text = "Search by Article Number:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
			this.label4.Location = new System.Drawing.Point(259, 4);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(42, 13);
			this.label4.TabIndex = 21;
			this.label4.Text = "Result:";
			// 
			// uxArticleInformationHeaderLabel
			// 
			this.uxArticleInformationHeaderLabel.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
			this.uxArticleInformationHeaderLabel.ForeColor = System.Drawing.Color.Gray;
			this.uxArticleInformationHeaderLabel.Image = global::InformaSitecoreWord.Properties.Resources.relatedarticles_tabheader;
			this.uxArticleInformationHeaderLabel.Location = new System.Drawing.Point(0, 0);
			this.uxArticleInformationHeaderLabel.Name = "uxArticleInformationHeaderLabel";
			this.uxArticleInformationHeaderLabel.Size = new System.Drawing.Size(610, 30);
			this.uxArticleInformationHeaderLabel.TabIndex = 12;
			this.uxArticleInformationHeaderLabel.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_Paint);
			// 
			// uxSelectedPanel
			// 
			this.uxSelectedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
			| System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this.uxSelectedPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxSelectedPanel.Controls.Add(this.uxSelectedLayout);
			this.uxSelectedPanel.Location = new System.Drawing.Point(0, 247);
			this.uxSelectedPanel.Name = "uxSelectedPanel";
			this.uxSelectedPanel.Size = new System.Drawing.Size(610, 202);
			this.uxSelectedPanel.TabIndex = 24;
			// 
			// uxSelectedLayout
			// 
			this.uxSelectedLayout.AutoScroll = true;
			this.uxSelectedLayout.AutoSize = true;
			this.uxSelectedLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.uxSelectedLayout.ColumnCount = 6;
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.72727F));
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.45454F));
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.18182F));
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.63636F));
			this.uxSelectedLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 50F));
			this.uxSelectedLayout.Dock = System.Windows.Forms.DockStyle.Fill;
			this.uxSelectedLayout.Location = new System.Drawing.Point(0, 0);
			this.uxSelectedLayout.Name = "uxSelectedLayout";
			this.uxSelectedLayout.Padding = new System.Windows.Forms.Padding(0, 0, 17, 0);
			this.uxSelectedLayout.RowCount = 1;
			this.uxSelectedLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 200F));
			this.uxSelectedLayout.Size = new System.Drawing.Size(608, 200);
			this.uxSelectedLayout.TabIndex = 0;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
			this.label5.ForeColor = System.Drawing.SystemColors.GrayText;
			this.label5.Location = new System.Drawing.Point(4, 227);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(128, 19);
			this.label5.TabIndex = 23;
			this.label5.Text = "Selected Articles :";
			// 
			// RelatedArticlesControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.uxSelectedPanel);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.uxArticleInformationHeaderLabel);
			this.Controls.Add(this.label53);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "RelatedArticlesControl";
			this.Size = new System.Drawing.Size(610, 449);
			this.Load += new System.EventHandler(this.RelatedArticlesControl_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.uxPreviewPanel.ResumeLayout(false);
			this.uxSelectedPanel.ResumeLayout(false);
			this.uxSelectedPanel.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label uxArticleInformationHeaderLabel;
		private System.Windows.Forms.Label label53;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button uxViewArticle;
		private System.Windows.Forms.Button uxRetrieveArticle;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox uxArticleNumber;
        private System.Windows.Forms.Label label6;
		private ArticlePreviewControl _uxArticlePreviewTable;
		private System.Windows.Forms.Panel uxPreviewPanel;
		private System.Windows.Forms.Button uxAddToRelated;
		private System.Windows.Forms.Panel uxSelectedPanel;
		private SelectedRelatedArticles uxSelectedLayout;
        private System.Windows.Forms.Label label5;
	}
}
