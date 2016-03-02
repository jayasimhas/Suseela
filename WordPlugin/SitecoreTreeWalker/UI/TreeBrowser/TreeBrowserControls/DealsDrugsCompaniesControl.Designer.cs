namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	partial class DealsDrugsCompaniesControl
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
            this.label3 = new System.Windows.Forms.Label();
            this.uxDealNumberGo = new System.Windows.Forms.Button();
            this.uxDealNumber = new System.Windows.Forms.TextBox();
            this.uxCompanyNameGo = new System.Windows.Forms.Button();
            this.uxCompanyName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.uxInsertIntoArticle = new System.Windows.Forms.Button();
            this.uxViewDetails = new System.Windows.Forms.Button();
            this.uxPreviewPanel = new System.Windows.Forms.Panel();
			this.uxPreviewDeals = new DealPreviewControl();
            this.uxRetrieveInformation = new System.Windows.Forms.Button();
            this.HeaderPanel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
			this.companyTreeView1 = new CompanyTreeView();
            this.label2 = new System.Windows.Forms.Label();
            this.uxPreviewPanel.SuspendLayout();
            this.HeaderPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(1, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(138, 19);
            this.label3.TabIndex = 11;
            this.label3.Text = "Look-Up by Deal #:";
            // 
            // uxDealNumberGo
            // 
            this.uxDealNumberGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxDealNumberGo.Location = new System.Drawing.Point(287, 51);
            this.uxDealNumberGo.Name = "uxDealNumberGo";
            this.uxDealNumberGo.Size = new System.Drawing.Size(39, 26);
            this.uxDealNumberGo.TabIndex = 14;
            this.uxDealNumberGo.Text = "GO";
            this.uxDealNumberGo.UseVisualStyleBackColor = true;
            this.uxDealNumberGo.Click += new System.EventHandler(this.uxDealNumberGo_Click);
            // 
            // uxDealNumber
            // 
            this.uxDealNumber.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxDealNumber.Location = new System.Drawing.Point(1, 51);
            this.uxDealNumber.Name = "uxDealNumber";
            this.uxDealNumber.Size = new System.Drawing.Size(282, 26);
            this.uxDealNumber.TabIndex = 13;
            this.uxDealNumber.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxDealNumber_KeyDown);
            // 
            // uxCompanyNameGo
            // 
            this.uxCompanyNameGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCompanyNameGo.Location = new System.Drawing.Point(287, 130);
            this.uxCompanyNameGo.Name = "uxCompanyNameGo";
            this.uxCompanyNameGo.Size = new System.Drawing.Size(39, 26);
            this.uxCompanyNameGo.TabIndex = 24;
            this.uxCompanyNameGo.Text = "GO";
            this.uxCompanyNameGo.UseVisualStyleBackColor = true;
            this.uxCompanyNameGo.Click += new System.EventHandler(this.uxCompanyNameGo_Click);
            // 
            // uxCompanyName
            // 
            this.uxCompanyName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxCompanyName.Location = new System.Drawing.Point(1, 130);
            this.uxCompanyName.Name = "uxCompanyName";
            this.uxCompanyName.Size = new System.Drawing.Size(282, 26);
            this.uxCompanyName.TabIndex = 23;
            this.uxCompanyName.TextChanged += new System.EventHandler(this.uxCompanyName_TextChanged);
            this.uxCompanyName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.uxCompanyName_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(0, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(219, 19);
            this.label7.TabIndex = 22;
            this.label7.Text = "Start typing a company name...";
            // 
            // uxInsertIntoArticle
            // 
            this.uxInsertIntoArticle.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.uxInsertIntoArticle.Location = new System.Drawing.Point(81, 398);
            this.uxInsertIntoArticle.Name = "uxInsertIntoArticle";
            this.uxInsertIntoArticle.Size = new System.Drawing.Size(156, 26);
            this.uxInsertIntoArticle.TabIndex = 27;
            this.uxInsertIntoArticle.Text = "Insert Into Article";
            this.uxInsertIntoArticle.UseVisualStyleBackColor = true;
            this.uxInsertIntoArticle.Visible = false;
            this.uxInsertIntoArticle.Click += new System.EventHandler(this.uxInsertIntoArticle_Click);
            // 
            // uxViewDetails
            // 
            this.uxViewDetails.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.uxViewDetails.Location = new System.Drawing.Point(81, 369);
            this.uxViewDetails.Name = "uxViewDetails";
            this.uxViewDetails.Size = new System.Drawing.Size(156, 26);
            this.uxViewDetails.TabIndex = 26;
            this.uxViewDetails.Text = "View Details";
            this.uxViewDetails.UseVisualStyleBackColor = true;
            this.uxViewDetails.Click += new System.EventHandler(this.uxViewDetails_Click);
            // 
            // uxPreviewPanel
            // 
            this.uxPreviewPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPreviewPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uxPreviewPanel.Controls.Add(this.uxPreviewDeals);
            this.uxPreviewPanel.Location = new System.Drawing.Point(4, 187);
            this.uxPreviewPanel.Name = "uxPreviewPanel";
            this.uxPreviewPanel.Size = new System.Drawing.Size(322, 161);
            this.uxPreviewPanel.TabIndex = 29;
            // 
            // uxPreviewDeals
            // 
            this.uxPreviewDeals.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPreviewDeals.ColumnCount = 2;
            this.uxPreviewDeals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 40F));
            this.uxPreviewDeals.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 60F));
            this.uxPreviewDeals.Location = new System.Drawing.Point(0, 0);
            this.uxPreviewDeals.Name = "uxPreviewDeals";
            this.uxPreviewDeals.RowCount = 4;
            this.uxPreviewDeals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.uxPreviewDeals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.uxPreviewDeals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.uxPreviewDeals.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.uxPreviewDeals.Size = new System.Drawing.Size(320, 160);
            this.uxPreviewDeals.TabIndex = 28;
            // 
            // uxRetrieveInformation
            // 
            this.uxRetrieveInformation.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.uxRetrieveInformation.Location = new System.Drawing.Point(81, 398);
            this.uxRetrieveInformation.Name = "uxRetrieveInformation";
            this.uxRetrieveInformation.Size = new System.Drawing.Size(156, 26);
            this.uxRetrieveInformation.TabIndex = 30;
            this.uxRetrieveInformation.Text = "Retrieve Information";
            this.uxRetrieveInformation.UseVisualStyleBackColor = true;
            this.uxRetrieveInformation.Click += new System.EventHandler(this.uxRetrieveInformation_Click);
            // 
            // HeaderPanel1
            // 
            this.HeaderPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.HeaderPanel1.Controls.Add(this.label1);
            this.HeaderPanel1.Location = new System.Drawing.Point(0, 0);
            this.HeaderPanel1.Name = "HeaderPanel1";
            this.HeaderPanel1.Size = new System.Drawing.Size(333, 27);
            this.HeaderPanel1.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(238)))));
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(57)))), ((int)(((byte)(115)))));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(333, 27);
            this.label1.TabIndex = 9;
            this.label1.Text = "  Deals";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.label9);
            this.panel1.Location = new System.Drawing.Point(0, 79);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(333, 27);
            this.panel1.TabIndex = 32;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(231)))), ((int)(((byte)(238)))));
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Bold);
            this.label9.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(57)))), ((int)(((byte)(115)))));
            this.label9.Location = new System.Drawing.Point(0, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(333, 27);
            this.label9.TabIndex = 20;
            this.label9.Text = "  Companies";
            // 
            // companyTreeView1
            // 
            this.companyTreeView1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.companyTreeView1.Location = new System.Drawing.Point(4, 187);
            this.companyTreeView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.companyTreeView1.Name = "companyTreeView1";
            this.companyTreeView1.Size = new System.Drawing.Size(322, 161);
            this.companyTreeView1.TabIndex = 33;
			this.companyTreeView1.CompanyDoubleClicked += new CompanyTreeView.CompanyDoubleClick(this.companyTreeView1_CompanyDoubleClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(1, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 19);
            this.label2.TabIndex = 34;
            this.label2.Text = "Results";
            // 
            // DealsDrugsCompaniesControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.AutoScroll = true;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.companyTreeView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.HeaderPanel1);
            this.Controls.Add(this.uxRetrieveInformation);
            this.Controls.Add(this.uxInsertIntoArticle);
            this.Controls.Add(this.uxViewDetails);
            this.Controls.Add(this.uxCompanyNameGo);
            this.Controls.Add(this.uxCompanyName);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.uxDealNumberGo);
            this.Controls.Add(this.uxDealNumber);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.uxPreviewPanel);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "DealsDrugsCompaniesControl";
            this.Size = new System.Drawing.Size(333, 550);
            this.Load += new System.EventHandler(this.DealsDrugsCompaniesControl_Load);
            this.uxPreviewPanel.ResumeLayout(false);
            this.HeaderPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button uxDealNumberGo;
		private System.Windows.Forms.TextBox uxDealNumber;
		private System.Windows.Forms.Button uxCompanyNameGo;
		private System.Windows.Forms.TextBox uxCompanyName;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button uxInsertIntoArticle;
		private System.Windows.Forms.Button uxViewDetails;
		private DealPreviewControl uxPreviewDeals;
		private System.Windows.Forms.Panel uxPreviewPanel;
        private System.Windows.Forms.Button uxRetrieveInformation;
		private System.Windows.Forms.Panel HeaderPanel1;
        private System.Windows.Forms.Panel panel1;
        private CompanyTreeView companyTreeView1;
        private System.Windows.Forms.Label label2;
	}
}
