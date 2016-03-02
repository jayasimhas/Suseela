namespace InformaSitecoreWord
{
	partial class TreeBrowser
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
			this.uxSupportingDocumentsList = new System.Windows.Forms.TreeView();
			this.uxPreview = new System.Windows.Forms.PictureBox();
			this.label1 = new System.Windows.Forms.Label();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.uxImages = new System.Windows.Forms.TabPage();
			this.button2 = new System.Windows.Forms.Button();
			this.button1 = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.uxBrowseImages = new System.Windows.Forms.TreeView();
			this.label2 = new System.Windows.Forms.Label();
			this.uxSupportingDocuments = new System.Windows.Forms.TabPage();
			this.button4 = new System.Windows.Forms.Button();
			this.uxDocsDownload = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.uxRelatedArticlesTab = new System.Windows.Forms.TabPage();
			this.button3 = new System.Windows.Forms.Button();
			this.uxArticlesInsertIntoArticleById = new System.Windows.Forms.Button();
			this.uxArticlesViewFile = new System.Windows.Forms.Button();
			this.uxSearchResults = new System.Windows.Forms.ListBox();
			this.label11 = new System.Windows.Forms.Label();
			this.uxSearchByPublication = new System.Windows.Forms.ComboBox();
			this.uxSearchByIssue = new System.Windows.Forms.ComboBox();
			this.uxSearchByAuthor = new System.Windows.Forms.ComboBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.uxRelatedArticleURL = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.uxRelatedArticleID = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.uxImages.SuspendLayout();
			this.uxSupportingDocuments.SuspendLayout();
			this.uxRelatedArticlesTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxSupportingDocumentsList
			// 
			this.uxSupportingDocumentsList.Location = new System.Drawing.Point(19, 69);
			this.uxSupportingDocumentsList.Name = "uxSupportingDocumentsList";
			this.uxSupportingDocumentsList.Size = new System.Drawing.Size(297, 628);
			this.uxSupportingDocumentsList.TabIndex = 1;
			this.uxSupportingDocumentsList.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanded);
			this.uxSupportingDocumentsList.DoubleClick += new System.EventHandler(this.SitecoreTreeView_DoubleClick);
			// 
			// uxPreview
			// 
			this.uxPreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.uxPreview.Location = new System.Drawing.Point(19, 417);
			this.uxPreview.Name = "uxPreview";
			this.uxPreview.Size = new System.Drawing.Size(297, 280);
			this.uxPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.uxPreview.TabIndex = 1;
			this.uxPreview.TabStop = false;
			this.uxPreview.Visible = false;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.ForeColor = System.Drawing.Color.Gray;
			this.label1.Location = new System.Drawing.Point(15, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(99, 22);
			this.label1.TabIndex = 0;
			this.label1.Text = "Images";
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.uxImages);
			this.tabControl1.Controls.Add(this.uxSupportingDocuments);
			this.tabControl1.Controls.Add(this.uxRelatedArticlesTab);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl1.HotTrack = true;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(335, 926);
			this.tabControl1.TabIndex = 3;
			// 
			// uxImages
			// 
			this.uxImages.Controls.Add(this.button2);
			this.uxImages.Controls.Add(this.button1);
			this.uxImages.Controls.Add(this.label3);
			this.uxImages.Controls.Add(this.uxBrowseImages);
			this.uxImages.Controls.Add(this.label2);
			this.uxImages.Controls.Add(this.uxPreview);
			this.uxImages.Controls.Add(this.label1);
			this.uxImages.Location = new System.Drawing.Point(4, 25);
			this.uxImages.Name = "uxImages";
			this.uxImages.Padding = new System.Windows.Forms.Padding(3);
			this.uxImages.Size = new System.Drawing.Size(327, 897);
			this.uxImages.TabIndex = 0;
			this.uxImages.Text = "Images";
			this.uxImages.UseVisualStyleBackColor = true;
			// 
			// button2
			// 
			this.button2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button2.Location = new System.Drawing.Point(19, 732);
			this.button2.Name = "button2";
			this.button2.Size = new System.Drawing.Size(194, 23);
			this.button2.TabIndex = 6;
			this.button2.Text = "Insert Into Article";
			this.button2.UseVisualStyleBackColor = true;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(19, 703);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(194, 23);
			this.button1.TabIndex = 5;
			this.button1.Text = "View Full Size";
			this.button1.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(16, 397);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(112, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Selected Image:";
			// 
			// uxBrowseImages
			// 
			this.uxBrowseImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxBrowseImages.Location = new System.Drawing.Point(19, 69);
			this.uxBrowseImages.Name = "uxBrowseImages";
			this.uxBrowseImages.Size = new System.Drawing.Size(297, 325);
			this.uxBrowseImages.TabIndex = 3;
			this.uxBrowseImages.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.NodeExpanded);
			this.uxBrowseImages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.SitecoreTreeView_AfterSelect);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(16, 50);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(93, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Browse Files:";
			// 
			// uxSupportingDocuments
			// 
			this.uxSupportingDocuments.Controls.Add(this.button4);
			this.uxSupportingDocuments.Controls.Add(this.uxDocsDownload);
			this.uxSupportingDocuments.Controls.Add(this.label5);
			this.uxSupportingDocuments.Controls.Add(this.label4);
			this.uxSupportingDocuments.Controls.Add(this.uxSupportingDocumentsList);
			this.uxSupportingDocuments.Location = new System.Drawing.Point(4, 25);
			this.uxSupportingDocuments.Name = "uxSupportingDocuments";
			this.uxSupportingDocuments.Padding = new System.Windows.Forms.Padding(3);
			this.uxSupportingDocuments.Size = new System.Drawing.Size(327, 897);
			this.uxSupportingDocuments.TabIndex = 1;
			this.uxSupportingDocuments.Text = "Supporting Documents";
			this.uxSupportingDocuments.UseVisualStyleBackColor = true;
			// 
			// button4
			// 
			this.button4.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button4.Location = new System.Drawing.Point(19, 732);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(194, 23);
			this.button4.TabIndex = 5;
			this.button4.Text = "Insert into Article";
			this.button4.UseVisualStyleBackColor = true;
			// 
			// uxDocsDownload
			// 
			this.uxDocsDownload.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxDocsDownload.Location = new System.Drawing.Point(19, 703);
			this.uxDocsDownload.Name = "uxDocsDownload";
			this.uxDocsDownload.Size = new System.Drawing.Size(194, 23);
			this.uxDocsDownload.TabIndex = 4;
			this.uxDocsDownload.Text = "Download / View File";
			this.uxDocsDownload.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 50);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(93, 16);
			this.label5.TabIndex = 3;
			this.label5.Text = "Browse Files:";
			// 
			// label4
			// 
			this.label4.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.ForeColor = System.Drawing.Color.Gray;
			this.label4.Location = new System.Drawing.Point(15, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(222, 22);
			this.label4.TabIndex = 2;
			this.label4.Text = "Supporting Documents";
			// 
			// uxRelatedArticlesTab
			// 
			this.uxRelatedArticlesTab.Controls.Add(this.button3);
			this.uxRelatedArticlesTab.Controls.Add(this.uxArticlesInsertIntoArticleById);
			this.uxRelatedArticlesTab.Controls.Add(this.uxArticlesViewFile);
			this.uxRelatedArticlesTab.Controls.Add(this.uxSearchResults);
			this.uxRelatedArticlesTab.Controls.Add(this.label11);
			this.uxRelatedArticlesTab.Controls.Add(this.uxSearchByPublication);
			this.uxRelatedArticlesTab.Controls.Add(this.uxSearchByIssue);
			this.uxRelatedArticlesTab.Controls.Add(this.uxSearchByAuthor);
			this.uxRelatedArticlesTab.Controls.Add(this.label10);
			this.uxRelatedArticlesTab.Controls.Add(this.label9);
			this.uxRelatedArticlesTab.Controls.Add(this.uxRelatedArticleURL);
			this.uxRelatedArticlesTab.Controls.Add(this.label8);
			this.uxRelatedArticlesTab.Controls.Add(this.uxRelatedArticleID);
			this.uxRelatedArticlesTab.Controls.Add(this.label7);
			this.uxRelatedArticlesTab.Controls.Add(this.label6);
			this.uxRelatedArticlesTab.Location = new System.Drawing.Point(4, 25);
			this.uxRelatedArticlesTab.Name = "uxRelatedArticlesTab";
			this.uxRelatedArticlesTab.Padding = new System.Windows.Forms.Padding(3);
			this.uxRelatedArticlesTab.Size = new System.Drawing.Size(327, 897);
			this.uxRelatedArticlesTab.TabIndex = 2;
			this.uxRelatedArticlesTab.Text = "Related Articles";
			this.uxRelatedArticlesTab.UseVisualStyleBackColor = true;
			// 
			// button3
			// 
			this.button3.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button3.Location = new System.Drawing.Point(19, 203);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(194, 23);
			this.button3.TabIndex = 19;
			this.button3.Text = "Insert into Article";
			this.button3.UseVisualStyleBackColor = true;
			this.button3.Click += new System.EventHandler(this.button3_Click);
			// 
			// uxArticlesInsertIntoArticleById
			// 
			this.uxArticlesInsertIntoArticleById.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxArticlesInsertIntoArticleById.Location = new System.Drawing.Point(19, 97);
			this.uxArticlesInsertIntoArticleById.Name = "uxArticlesInsertIntoArticleById";
			this.uxArticlesInsertIntoArticleById.Size = new System.Drawing.Size(194, 23);
			this.uxArticlesInsertIntoArticleById.TabIndex = 18;
			this.uxArticlesInsertIntoArticleById.Text = "Insert into Article";
			this.uxArticlesInsertIntoArticleById.UseVisualStyleBackColor = true;
			this.uxArticlesInsertIntoArticleById.Click += new System.EventHandler(this.uxArticlesInsertIntoArticleById_Click);
			// 
			// uxArticlesViewFile
			// 
			this.uxArticlesViewFile.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxArticlesViewFile.Location = new System.Drawing.Point(19, 820);
			this.uxArticlesViewFile.Name = "uxArticlesViewFile";
			this.uxArticlesViewFile.Size = new System.Drawing.Size(194, 23);
			this.uxArticlesViewFile.TabIndex = 17;
			this.uxArticlesViewFile.Text = "View File";
			this.uxArticlesViewFile.UseVisualStyleBackColor = true;
			// 
			// uxSearchResults
			// 
			this.uxSearchResults.FormattingEnabled = true;
			this.uxSearchResults.ItemHeight = 16;
			this.uxSearchResults.Location = new System.Drawing.Point(19, 417);
			this.uxSearchResults.Name = "uxSearchResults";
			this.uxSearchResults.Size = new System.Drawing.Size(292, 388);
			this.uxSearchResults.TabIndex = 16;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(16, 398);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(57, 16);
			this.label11.TabIndex = 15;
			this.label11.Text = "Results:";
			// 
			// uxSearchByPublication
			// 
			this.uxSearchByPublication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxSearchByPublication.FormattingEnabled = true;
			this.uxSearchByPublication.Location = new System.Drawing.Point(19, 318);
			this.uxSearchByPublication.Name = "uxSearchByPublication";
			this.uxSearchByPublication.Size = new System.Drawing.Size(292, 24);
			this.uxSearchByPublication.TabIndex = 14;
			// 
			// uxSearchByIssue
			// 
			this.uxSearchByIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxSearchByIssue.FormattingEnabled = true;
			this.uxSearchByIssue.Location = new System.Drawing.Point(19, 348);
			this.uxSearchByIssue.Name = "uxSearchByIssue";
			this.uxSearchByIssue.Size = new System.Drawing.Size(292, 24);
			this.uxSearchByIssue.TabIndex = 13;
			// 
			// uxSearchByAuthor
			// 
			this.uxSearchByAuthor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.uxSearchByAuthor.FormattingEnabled = true;
			this.uxSearchByAuthor.Location = new System.Drawing.Point(19, 274);
			this.uxSearchByAuthor.Name = "uxSearchByAuthor";
			this.uxSearchByAuthor.Size = new System.Drawing.Size(292, 24);
			this.uxSearchByAuthor.TabIndex = 12;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(16, 299);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(196, 16);
			this.label10.TabIndex = 10;
			this.label10.Text = "Search by Publication / Issue:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(16, 255);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(163, 16);
			this.label9.TabIndex = 8;
			this.label9.Text = "Search by Author Name:";
			// 
			// uxRelatedArticleURL
			// 
			this.uxRelatedArticleURL.Location = new System.Drawing.Point(19, 175);
			this.uxRelatedArticleURL.Name = "uxRelatedArticleURL";
			this.uxRelatedArticleURL.Size = new System.Drawing.Size(292, 22);
			this.uxRelatedArticleURL.TabIndex = 7;
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(16, 156);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(38, 16);
			this.label8.TabIndex = 6;
			this.label8.Text = "URL:";
			// 
			// uxRelatedArticleID
			// 
			this.uxRelatedArticleID.Location = new System.Drawing.Point(19, 69);
			this.uxRelatedArticleID.Name = "uxRelatedArticleID";
			this.uxRelatedArticleID.Size = new System.Drawing.Size(292, 22);
			this.uxRelatedArticleID.TabIndex = 5;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(16, 50);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(64, 16);
			this.label7.TabIndex = 4;
			this.label7.Text = "Article #:";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.ForeColor = System.Drawing.Color.Gray;
			this.label6.Location = new System.Drawing.Point(15, 16);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(222, 22);
			this.label6.TabIndex = 3;
			this.label6.Text = "Supporting Documents";
			// 
			// TreeBrowser
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl1);
			this.Name = "TreeBrowser";
			this.Size = new System.Drawing.Size(335, 926);
			((System.ComponentModel.ISupportInitialize)(this.uxPreview)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.uxImages.ResumeLayout(false);
			this.uxImages.PerformLayout();
			this.uxSupportingDocuments.ResumeLayout(false);
			this.uxSupportingDocuments.PerformLayout();
			this.uxRelatedArticlesTab.ResumeLayout(false);
			this.uxRelatedArticlesTab.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.TreeView uxSupportingDocumentsList;
        private System.Windows.Forms.PictureBox uxPreview;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage uxImages;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TreeView uxBrowseImages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage uxSupportingDocuments;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button uxDocsDownload;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TabPage uxRelatedArticlesTab;
        private System.Windows.Forms.Button uxArticlesInsertIntoArticleById;
        private System.Windows.Forms.Button uxArticlesViewFile;
        private System.Windows.Forms.ListBox uxSearchResults;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox uxSearchByPublication;
        private System.Windows.Forms.ComboBox uxSearchByIssue;
        private System.Windows.Forms.ComboBox uxSearchByAuthor;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox uxRelatedArticleURL;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox uxRelatedArticleID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button button3;
	}
}
