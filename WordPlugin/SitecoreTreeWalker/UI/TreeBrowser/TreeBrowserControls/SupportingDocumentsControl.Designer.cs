namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class SupportingDocumentsControl
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
			this.uxBrowseDocuments = new System.Windows.Forms.TreeView();
			this.panel1 = new System.Windows.Forms.Panel();
			this.uxUploader = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.uxUploadDate = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.uxDocumentType = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.uxDocumentTitle = new System.Windows.Forms.Label();
			this.uxViewDocument = new System.Windows.Forms.Button();
			this.uxInsert = new System.Windows.Forms.Button();
			this.uxRefresh = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// uxBrowseDocuments
			// 
			this.uxBrowseDocuments.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxBrowseDocuments.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxBrowseDocuments.Location = new System.Drawing.Point(2, 29);
			this.uxBrowseDocuments.Name = "uxBrowseDocuments";
			this.uxBrowseDocuments.Size = new System.Drawing.Size(193, 126);
			this.uxBrowseDocuments.TabIndex = 16;
			this.uxBrowseDocuments.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.uxBrowseDocuments_BeforeExpand);
			this.uxBrowseDocuments.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.uxBrowseDocuments_AfterSelect);
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.panel1.Controls.Add(this.uxUploader);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.uxUploadDate);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.uxDocumentType);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.uxDocumentTitle);
			this.panel1.Location = new System.Drawing.Point(4, 161);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(193, 140);
			this.panel1.TabIndex = 17;
			// 
			// uxUploader
			// 
			this.uxUploader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxUploader.AutoSize = true;
			this.uxUploader.Location = new System.Drawing.Point(34, 97);
			this.uxUploader.Name = "uxUploader";
			this.uxUploader.Size = new System.Drawing.Size(0, 13);
			this.uxUploader.TabIndex = 6;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(3, 97);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(25, 13);
			this.label3.TabIndex = 5;
			this.label3.Text = "By: ";
			this.label3.Visible = false;
			// 
			// uxUploadDate
			// 
			this.uxUploadDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxUploadDate.AutoSize = true;
			this.uxUploadDate.Location = new System.Drawing.Point(92, 74);
			this.uxUploadDate.Name = "uxUploadDate";
			this.uxUploadDate.Size = new System.Drawing.Size(0, 13);
			this.uxUploadDate.TabIndex = 4;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(3, 74);
			this.label4.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(83, 13);
			this.label4.TabIndex = 3;
			this.label4.Text = "Uploaded On: ";
			this.label4.Visible = false;
			// 
			// uxDocumentType
			// 
			this.uxDocumentType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxDocumentType.AutoSize = true;
			this.uxDocumentType.Location = new System.Drawing.Point(45, 51);
			this.uxDocumentType.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.uxDocumentType.Name = "uxDocumentType";
			this.uxDocumentType.Size = new System.Drawing.Size(0, 13);
			this.uxDocumentType.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(3, 51);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 10);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Type: ";
			this.label2.Visible = false;
			// 
			// uxDocumentTitle
			// 
			this.uxDocumentTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.uxDocumentTitle.AutoEllipsis = true;
			this.uxDocumentTitle.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.uxDocumentTitle.Location = new System.Drawing.Point(3, 14);
			this.uxDocumentTitle.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
			this.uxDocumentTitle.Name = "uxDocumentTitle";
			this.uxDocumentTitle.Size = new System.Drawing.Size(187, 32);
			this.uxDocumentTitle.TabIndex = 0;
			// 
			// uxViewDocument
			// 
			this.uxViewDocument.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxViewDocument.Location = new System.Drawing.Point(53, 305);
			this.uxViewDocument.Name = "uxViewDocument";
			this.uxViewDocument.Size = new System.Drawing.Size(106, 23);
			this.uxViewDocument.TabIndex = 18;
			this.uxViewDocument.Text = "View";
			this.uxViewDocument.UseVisualStyleBackColor = true;
			this.uxViewDocument.Click += new System.EventHandler(this.uxViewDocument_Click);
			// 
			// uxInsert
			// 
			this.uxInsert.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxInsert.Location = new System.Drawing.Point(53, 334);
			this.uxInsert.Name = "uxInsert";
			this.uxInsert.Size = new System.Drawing.Size(106, 23);
			this.uxInsert.TabIndex = 19;
			this.uxInsert.Text = "Insert";
			this.uxInsert.UseVisualStyleBackColor = true;
			this.uxInsert.Click += new System.EventHandler(this.uxInsert_Click);
			// 
			// uxRefresh
			// 
			this.uxRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.uxRefresh.Location = new System.Drawing.Point(54, 363);
			this.uxRefresh.Name = "uxRefresh";
			this.uxRefresh.Size = new System.Drawing.Size(105, 23);
			this.uxRefresh.TabIndex = 20;
			this.uxRefresh.Text = "Refresh";
			this.uxRefresh.UseVisualStyleBackColor = true;
			this.uxRefresh.Click += new System.EventHandler(this.uxRefresh_Click);
			// 
			// SupportingDocumentsControl
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoScroll = true;
			this.BackColor = System.Drawing.Color.White;
			this.Controls.Add(this.uxRefresh);
			this.Controls.Add(this.uxInsert);
			this.Controls.Add(this.uxViewDocument);
			this.Controls.Add(this.panel1);
			this.Controls.Add(this.uxBrowseDocuments);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Name = "SupportingDocumentsControl";
			this.Size = new System.Drawing.Size(200, 550);
			this.Load += new System.EventHandler(this.SupportingDocumentsControl_Load);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TreeView uxBrowseDocuments;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label uxDocumentType;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label uxDocumentTitle;
		private System.Windows.Forms.Label uxUploadDate;
		private System.Windows.Forms.Label uxUploader;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button uxViewDocument;
		private System.Windows.Forms.Button uxInsert;
		private System.Windows.Forms.Button uxRefresh;
	}
}
