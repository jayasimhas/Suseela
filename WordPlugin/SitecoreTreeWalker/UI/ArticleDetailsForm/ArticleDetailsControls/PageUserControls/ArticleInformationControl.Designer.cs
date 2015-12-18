namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleInformationControl
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.uxArticleNumberLabel = new System.Windows.Forms.TextBox();
            this.uxLinkToDocumentPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.uxLinkDocument = new System.Windows.Forms.Button();
            this.uxArticleNumberToLink = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.uxLockStatus = new System.Windows.Forms.Panel();
            this.label88 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.uxRefreshStatus = new System.Windows.Forms.Button();
            this.uxLockStatusLabel = new System.Windows.Forms.Label();
            this.uxLockUser = new System.Windows.Forms.Label();
            this.uxUnlockButton = new System.Windows.Forms.Button();
            this.uxLockButton = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.uxWebPublishTime = new System.Windows.Forms.DateTimePicker();
            this.uxEmbargoed = new System.Windows.Forms.CheckBox();
            this.uxWebPublishDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbWebCategory = new System.Windows.Forms.ComboBox();
            this.lblWebCategory = new System.Windows.Forms.Label();
            this.uxArticleCategory = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uxArticleInformationIssue = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.uxPublication = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.uxSelectAuthor = new System.Windows.Forms.ComboBox();
            this.uxAddAuthor = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.uxNominate = new System.Windows.Forms.CheckBox();
            this.uxTopStory = new System.Windows.Forms.CheckBox();
            this.uxWordCount = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.uxUnlinkDocument = new System.Windows.Forms.Button();
            this.uxVersionStatus = new System.Windows.Forms.Panel();
            this.uxVersionText = new System.Windows.Forms.Label();
            this.uxRefreshVersion = new System.Windows.Forms.Button();
            this.uxLastUpdatedBy = new System.Windows.Forms.Label();
            this.uxLastUpdateDate = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label21 = new System.Windows.Forms.Label();
            this.uxSelectedAuthors = new SitecoreTreeWalker.UI.EasyRemoveListView();
            this.PageNotesControl = new SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls.NotesControl();
            this.uxLinkToDocumentPanel.SuspendLayout();
            this.uxLockStatus.SuspendLayout();
            this.panel2.SuspendLayout();
            this.uxVersionStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.Gray;
            this.label1.Image = global::SitecoreTreeWalker.Properties.Resources.articleinfo_tabheader;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(610, 30);
            this.label1.TabIndex = 10;
            this.label1.Paint += new System.Windows.Forms.PaintEventHandler(this.label1_Paint);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label3.Location = new System.Drawing.Point(4, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 14);
            this.label3.TabIndex = 12;
            this.label3.Text = "Article Number:";
            // 
            // uxArticleNumberLabel
            // 
            this.uxArticleNumberLabel.BackColor = System.Drawing.Color.White;
            this.uxArticleNumberLabel.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.uxArticleNumberLabel.Location = new System.Drawing.Point(103, 37);
            this.uxArticleNumberLabel.Margin = new System.Windows.Forms.Padding(0);
            this.uxArticleNumberLabel.Name = "uxArticleNumberLabel";
            this.uxArticleNumberLabel.ReadOnly = true;
            this.uxArticleNumberLabel.Size = new System.Drawing.Size(119, 15);
            this.uxArticleNumberLabel.TabIndex = 13;
            this.uxArticleNumberLabel.Text = "Document Not Linked";
            // 
            // uxLinkToDocumentPanel
            // 
            this.uxLinkToDocumentPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxLinkToDocumentPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uxLinkToDocumentPanel.Controls.Add(this.label5);
            this.uxLinkToDocumentPanel.Controls.Add(this.uxLinkDocument);
            this.uxLinkToDocumentPanel.Controls.Add(this.uxArticleNumberToLink);
            this.uxLinkToDocumentPanel.Controls.Add(this.label6);
            this.uxLinkToDocumentPanel.Controls.Add(this.label4);
            this.uxLinkToDocumentPanel.Location = new System.Drawing.Point(0, 55);
            this.uxLinkToDocumentPanel.Name = "uxLinkToDocumentPanel";
            this.uxLinkToDocumentPanel.Size = new System.Drawing.Size(607, 102);
            this.uxLinkToDocumentPanel.TabIndex = 14;
            this.uxLinkToDocumentPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.uxLinkToDocumentPanel_Paint);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.Location = new System.Drawing.Point(3, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(601, 28);
            this.label5.TabIndex = 1;
            this.label5.Text = "If you would like to link this Word document to an existing article, enter the ar" +
    "ticle number below.";
            // 
            // uxLinkDocument
            // 
            this.uxLinkDocument.Location = new System.Drawing.Point(208, 66);
            this.uxLinkDocument.Name = "uxLinkDocument";
            this.uxLinkDocument.Size = new System.Drawing.Size(92, 21);
            this.uxLinkDocument.TabIndex = 17;
            this.uxLinkDocument.Text = "Link Document";
            this.uxLinkDocument.UseVisualStyleBackColor = true;
            this.uxLinkDocument.Click += new System.EventHandler(this.uxLinkDocument_Click);
            // 
            // uxArticleNumberToLink
            // 
            this.uxArticleNumberToLink.Location = new System.Drawing.Point(102, 67);
            this.uxArticleNumberToLink.Name = "uxArticleNumberToLink";
            this.uxArticleNumberToLink.Size = new System.Drawing.Size(100, 22);
            this.uxArticleNumberToLink.TabIndex = 16;
            this.uxArticleNumberToLink.TextChanged += new System.EventHandler(this.uxArticleNumberToLink_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Bold);
            this.label6.Location = new System.Drawing.Point(3, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "Article Number:";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Underline);
            this.label4.Location = new System.Drawing.Point(3, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 14);
            this.label4.TabIndex = 0;
            this.label4.Text = "Link Document to Article";
            // 
            // uxLockStatus
            // 
            this.uxLockStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uxLockStatus.Controls.Add(this.label88);
            this.uxLockStatus.Controls.Add(this.label22);
            this.uxLockStatus.Controls.Add(this.uxRefreshStatus);
            this.uxLockStatus.Controls.Add(this.uxLockStatusLabel);
            this.uxLockStatus.Controls.Add(this.uxLockUser);
            this.uxLockStatus.Controls.Add(this.uxUnlockButton);
            this.uxLockStatus.Controls.Add(this.uxLockButton);
            this.uxLockStatus.Location = new System.Drawing.Point(367, 55);
            this.uxLockStatus.Name = "uxLockStatus";
            this.uxLockStatus.Size = new System.Drawing.Size(240, 102);
            this.uxLockStatus.TabIndex = 75;
            this.uxLockStatus.Visible = false;
            // 
            // label88
            // 
            this.label88.AutoSize = true;
            this.label88.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label88.Location = new System.Drawing.Point(18, 47);
            this.label88.Margin = new System.Windows.Forms.Padding(0);
            this.label88.Name = "label88";
            this.label88.Size = new System.Drawing.Size(23, 13);
            this.label88.TabIndex = 43;
            this.label88.Text = "By:";
            this.label88.Click += new System.EventHandler(this.label88_Click);
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.label22.ForeColor = System.Drawing.Color.Gray;
            this.label22.Location = new System.Drawing.Point(17, 10);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(111, 19);
            this.label22.TabIndex = 47;
            this.label22.Text = "Working Status";
            // 
            // uxRefreshStatus
            // 
            this.uxRefreshStatus.Location = new System.Drawing.Point(21, 68);
            this.uxRefreshStatus.Name = "uxRefreshStatus";
            this.uxRefreshStatus.Size = new System.Drawing.Size(105, 21);
            this.uxRefreshStatus.TabIndex = 46;
            this.uxRefreshStatus.Text = "Refresh Status";
            this.uxRefreshStatus.UseVisualStyleBackColor = true;
            this.uxRefreshStatus.Click += new System.EventHandler(this.uxRefreshStatus_Click);
            // 
            // uxLockStatusLabel
            // 
            this.uxLockStatusLabel.AutoSize = true;
            this.uxLockStatusLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.uxLockStatusLabel.Location = new System.Drawing.Point(18, 32);
            this.uxLockStatusLabel.Margin = new System.Windows.Forms.Padding(0);
            this.uxLockStatusLabel.Name = "uxLockStatusLabel";
            this.uxLockStatusLabel.Size = new System.Drawing.Size(56, 13);
            this.uxLockStatusLabel.TabIndex = 42;
            this.uxLockStatusLabel.Text = "Unlocked";
            this.uxLockStatusLabel.Click += new System.EventHandler(this.uxLockStatusLabel_Click);
            // 
            // uxLockUser
            // 
            this.uxLockUser.AutoSize = true;
            this.uxLockUser.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxLockUser.Location = new System.Drawing.Point(40, 48);
            this.uxLockUser.Margin = new System.Windows.Forms.Padding(0);
            this.uxLockUser.Name = "uxLockUser";
            this.uxLockUser.Size = new System.Drawing.Size(58, 13);
            this.uxLockUser.TabIndex = 44;
            this.uxLockUser.Text = "Username";
            this.uxLockUser.Click += new System.EventHandler(this.uxLockUser_Click);
            // 
            // uxUnlockButton
            // 
            this.uxUnlockButton.Location = new System.Drawing.Point(132, 68);
            this.uxUnlockButton.Name = "uxUnlockButton";
            this.uxUnlockButton.Size = new System.Drawing.Size(75, 21);
            this.uxUnlockButton.TabIndex = 49;
            this.uxUnlockButton.Text = "Unlock";
            this.uxUnlockButton.UseVisualStyleBackColor = true;
            this.uxUnlockButton.Click += new System.EventHandler(this.uxUnlockButton_Click);
            // 
            // uxLockButton
            // 
            this.uxLockButton.Location = new System.Drawing.Point(132, 68);
            this.uxLockButton.Name = "uxLockButton";
            this.uxLockButton.Size = new System.Drawing.Size(75, 21);
            this.uxLockButton.TabIndex = 48;
            this.uxLockButton.Text = "Lock";
            this.uxLockButton.UseVisualStyleBackColor = true;
            this.uxLockButton.Click += new System.EventHandler(this.uxLockButton_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.uxWebPublishTime);
            this.panel2.Controls.Add(this.uxEmbargoed);
            this.panel2.Controls.Add(this.uxWebPublishDate);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.cmbWebCategory);
            this.panel2.Controls.Add(this.lblWebCategory);
            this.panel2.Controls.Add(this.uxArticleCategory);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.uxArticleInformationIssue);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.uxPublication);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Location = new System.Drawing.Point(0, 162);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(605, 154);
            this.panel2.TabIndex = 15;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
            // 
            // uxWebPublishTime
            // 
            this.uxWebPublishTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.uxWebPublishTime.Location = new System.Drawing.Point(420, 118);
            this.uxWebPublishTime.Name = "uxWebPublishTime";
            this.uxWebPublishTime.ShowUpDown = true;
            this.uxWebPublishTime.Size = new System.Drawing.Size(89, 22);
            this.uxWebPublishTime.TabIndex = 43;
            this.uxWebPublishTime.Value = new System.DateTime(1753, 1, 1, 0, 0, 0, 0);
            // 
            // uxEmbargoed
            // 
            this.uxEmbargoed.AutoSize = true;
            this.uxEmbargoed.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.uxEmbargoed.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uxEmbargoed.Location = new System.Drawing.Point(509, 121);
            this.uxEmbargoed.Name = "uxEmbargoed";
            this.uxEmbargoed.Size = new System.Drawing.Size(91, 17);
            this.uxEmbargoed.TabIndex = 42;
            this.uxEmbargoed.Text = "Embargoed?";
            this.uxEmbargoed.UseVisualStyleBackColor = true;
            // 
            // uxWebPublishDate
            // 
            this.uxWebPublishDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxWebPublishDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.uxWebPublishDate.Location = new System.Drawing.Point(317, 118);
            this.uxWebPublishDate.Name = "uxWebPublishDate";
            this.uxWebPublishDate.Size = new System.Drawing.Size(97, 22);
            this.uxWebPublishDate.TabIndex = 41;
            this.uxWebPublishDate.ValueChanged += new System.EventHandler(this.uxWebPublishDate_ValueChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label11.Location = new System.Drawing.Point(314, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(255, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Scheduled Article Publish Date and Time (local):";
            // 
            // cmbWebCategory
            // 
            this.cmbWebCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbWebCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWebCategory.Enabled = false;
            this.cmbWebCategory.FormattingEnabled = true;
            this.cmbWebCategory.Location = new System.Drawing.Point(317, 72);
            this.cmbWebCategory.Name = "cmbWebCategory";
            this.cmbWebCategory.Size = new System.Drawing.Size(283, 21);
            this.cmbWebCategory.TabIndex = 37;
            this.cmbWebCategory.Visible = false;
            this.cmbWebCategory.SelectedIndexChanged += new System.EventHandler(this.uxArticleCategory_SelectedIndexChanged);
            // 
            // lblWebCategory
            // 
            this.lblWebCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblWebCategory.AutoSize = true;
            this.lblWebCategory.Enabled = false;
            this.lblWebCategory.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.lblWebCategory.Location = new System.Drawing.Point(314, 56);
            this.lblWebCategory.Name = "lblWebCategory";
            this.lblWebCategory.Size = new System.Drawing.Size(120, 13);
            this.lblWebCategory.TabIndex = 36;
            this.lblWebCategory.Text = "Home Page Category:";
            this.lblWebCategory.Visible = false;
            // 
            // uxArticleCategory
            // 
            this.uxArticleCategory.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxArticleCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxArticleCategory.FormattingEnabled = true;
            this.uxArticleCategory.Location = new System.Drawing.Point(317, 25);
            this.uxArticleCategory.Name = "uxArticleCategory";
            this.uxArticleCategory.Size = new System.Drawing.Size(283, 21);
            this.uxArticleCategory.TabIndex = 37;
            this.uxArticleCategory.SelectedIndexChanged += new System.EventHandler(this.uxArticleCategory_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label9.Location = new System.Drawing.Point(314, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 13);
            this.label9.TabIndex = 36;
            this.label9.Text = "Article Category:";
            // 
            // uxArticleInformationIssue
            // 
            this.uxArticleInformationIssue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxArticleInformationIssue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxArticleInformationIssue.FormattingEnabled = true;
            this.uxArticleInformationIssue.Location = new System.Drawing.Point(6, 72);
            this.uxArticleInformationIssue.Name = "uxArticleInformationIssue";
            this.uxArticleInformationIssue.Size = new System.Drawing.Size(302, 21);
            this.uxArticleInformationIssue.TabIndex = 35;
            this.uxArticleInformationIssue.SelectedIndexChanged += new System.EventHandler(this.uxArticleInformationIssue_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(3, 57);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Issue:";
            // 
            // uxPublication
            // 
            this.uxPublication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPublication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxPublication.FormattingEnabled = true;
            this.uxPublication.Location = new System.Drawing.Point(6, 25);
            this.uxPublication.Name = "uxPublication";
            this.uxPublication.Size = new System.Drawing.Size(302, 21);
            this.uxPublication.TabIndex = 33;
            this.uxPublication.SelectedIndexChanged += new System.EventHandler(this.uxPublication_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Publication:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(4, 319);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(102, 13);
            this.label12.TabIndex = 16;
            this.label12.Text = "Choose Author(s):";
            // 
            // uxSelectAuthor
            // 
            this.uxSelectAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSelectAuthor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxSelectAuthor.FormattingEnabled = true;
            this.uxSelectAuthor.Location = new System.Drawing.Point(7, 335);
            this.uxSelectAuthor.Name = "uxSelectAuthor";
            this.uxSelectAuthor.Size = new System.Drawing.Size(302, 21);
            this.uxSelectAuthor.TabIndex = 19;
            // 
            // uxAddAuthor
            // 
            this.uxAddAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxAddAuthor.Location = new System.Drawing.Point(247, 362);
            this.uxAddAuthor.Name = "uxAddAuthor";
            this.uxAddAuthor.Size = new System.Drawing.Size(62, 21);
            this.uxAddAuthor.TabIndex = 21;
            this.uxAddAuthor.Text = "Add";
            this.uxAddAuthor.UseVisualStyleBackColor = true;
            this.uxAddAuthor.Click += new System.EventHandler(this.uxAddAuthor_Click);
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label15.Location = new System.Drawing.Point(315, 319);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "Selected Author(s):";
            // 
            // uxNominate
            // 
            this.uxNominate.AutoSize = true;
            this.uxNominate.Location = new System.Drawing.Point(5, 433);
            this.uxNominate.Name = "uxNominate";
            this.uxNominate.Size = new System.Drawing.Size(153, 17);
            this.uxNominate.TabIndex = 52;
            this.uxNominate.Text = "Nominate for Homepage";
            this.uxNominate.UseVisualStyleBackColor = true;
            // 
            // uxTopStory
            // 
            this.uxTopStory.AutoSize = true;
            this.uxTopStory.Location = new System.Drawing.Point(5, 410);
            this.uxTopStory.Name = "uxTopStory";
            this.uxTopStory.Size = new System.Drawing.Size(149, 17);
            this.uxTopStory.TabIndex = 51;
            this.uxTopStory.Text = "Top Story on Newsletter";
            this.uxTopStory.UseVisualStyleBackColor = true;
            // 
            // uxWordCount
            // 
            this.uxWordCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxWordCount.AutoSize = true;
            this.uxWordCount.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxWordCount.Location = new System.Drawing.Point(395, 411);
            this.uxWordCount.Name = "uxWordCount";
            this.uxWordCount.Size = new System.Drawing.Size(13, 13);
            this.uxWordCount.TabIndex = 55;
            this.uxWordCount.Text = "0";
            // 
            // label19
            // 
            this.label19.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label19.AutoSize = true;
            this.label19.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label19.Location = new System.Drawing.Point(315, 411);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(74, 13);
            this.label19.TabIndex = 53;
            this.label19.Text = "Word Count:";
            // 
            // uxUnlinkDocument
            // 
            this.uxUnlinkDocument.Location = new System.Drawing.Point(233, 33);
            this.uxUnlinkDocument.Name = "uxUnlinkDocument";
            this.uxUnlinkDocument.Size = new System.Drawing.Size(102, 21);
            this.uxUnlinkDocument.TabIndex = 57;
            this.uxUnlinkDocument.Text = "Unlink Document";
            this.uxUnlinkDocument.UseVisualStyleBackColor = true;
            this.uxUnlinkDocument.Visible = false;
            this.uxUnlinkDocument.Click += new System.EventHandler(this.uxUnlinkDocument_Click);
            // 
            // uxVersionStatus
            // 
            this.uxVersionStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.uxVersionStatus.Controls.Add(this.uxVersionText);
            this.uxVersionStatus.Controls.Add(this.uxRefreshVersion);
            this.uxVersionStatus.Controls.Add(this.uxLastUpdatedBy);
            this.uxVersionStatus.Controls.Add(this.uxLastUpdateDate);
            this.uxVersionStatus.Controls.Add(this.label20);
            this.uxVersionStatus.Controls.Add(this.label21);
            this.uxVersionStatus.Location = new System.Drawing.Point(0, 55);
            this.uxVersionStatus.Name = "uxVersionStatus";
            this.uxVersionStatus.Size = new System.Drawing.Size(335, 102);
            this.uxVersionStatus.TabIndex = 76;
            this.uxVersionStatus.Visible = false;
            // 
            // uxVersionText
            // 
            this.uxVersionText.AutoSize = true;
            this.uxVersionText.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.uxVersionText.ForeColor = System.Drawing.Color.Gray;
            this.uxVersionText.Location = new System.Drawing.Point(14, 10);
            this.uxVersionText.Name = "uxVersionText";
            this.uxVersionText.Size = new System.Drawing.Size(226, 19);
            this.uxVersionText.TabIndex = 48;
            this.uxVersionText.Text = "Document Content is Up To Date";
            // 
            // uxRefreshVersion
            // 
            this.uxRefreshVersion.Location = new System.Drawing.Point(14, 69);
            this.uxRefreshVersion.Name = "uxRefreshVersion";
            this.uxRefreshVersion.Size = new System.Drawing.Size(175, 21);
            this.uxRefreshVersion.TabIndex = 62;
            this.uxRefreshVersion.Text = "Refresh Version Information";
            this.uxRefreshVersion.UseVisualStyleBackColor = true;
            this.uxRefreshVersion.Click += new System.EventHandler(this.uxRefreshVersion_Click);
            // 
            // uxLastUpdatedBy
            // 
            this.uxLastUpdatedBy.AutoSize = true;
            this.uxLastUpdatedBy.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxLastUpdatedBy.Location = new System.Drawing.Point(43, 46);
            this.uxLastUpdatedBy.Name = "uxLastUpdatedBy";
            this.uxLastUpdatedBy.Size = new System.Drawing.Size(58, 13);
            this.uxLastUpdatedBy.TabIndex = 61;
            this.uxLastUpdatedBy.Text = "Username";
            // 
            // uxLastUpdateDate
            // 
            this.uxLastUpdateDate.AutoSize = true;
            this.uxLastUpdateDate.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.uxLastUpdateDate.Location = new System.Drawing.Point(117, 33);
            this.uxLastUpdateDate.Name = "uxLastUpdateDate";
            this.uxLastUpdateDate.Size = new System.Drawing.Size(51, 13);
            this.uxLastUpdateDate.TabIndex = 60;
            this.uxLastUpdateDate.Text = "1/1/1900";
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label20.Location = new System.Drawing.Point(14, 46);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(23, 13);
            this.label20.TabIndex = 59;
            this.label20.Text = "By:";
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label21.Location = new System.Drawing.Point(14, 32);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(97, 13);
            this.label21.TabIndex = 58;
            this.label21.Text = "Last Updated On:";
            // 
            // uxSelectedAuthors
            // 
            this.uxSelectedAuthors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSelectedAuthors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.uxSelectedAuthors.Location = new System.Drawing.Point(315, 334);
            this.uxSelectedAuthors.Name = "uxSelectedAuthors";
            this.uxSelectedAuthors.Size = new System.Drawing.Size(290, 74);
            this.uxSelectedAuthors.TabIndex = 46;
            this.uxSelectedAuthors.UseCompatibleStateImageBehavior = false;
            this.uxSelectedAuthors.View = System.Windows.Forms.View.Details;
            // 
            // PageNotesControl
            // 
            this.PageNotesControl.Location = new System.Drawing.Point(-3, 456);
            this.PageNotesControl.Name = "PageNotesControl";
            this.PageNotesControl.Size = new System.Drawing.Size(610, 140);
            this.PageNotesControl.TabIndex = 77;
            // 
            // ArticleInformationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.PageNotesControl);
            this.Controls.Add(this.uxLockStatus);
            this.Controls.Add(this.uxUnlinkDocument);
            this.Controls.Add(this.uxWordCount);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.uxNominate);
            this.Controls.Add(this.uxTopStory);
            this.Controls.Add(this.uxSelectedAuthors);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.uxAddAuthor);
            this.Controls.Add(this.uxSelectAuthor);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.uxLinkToDocumentPanel);
            this.Controls.Add(this.uxArticleNumberLabel);
            this.Controls.Add(this.uxVersionStatus);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ArticleInformationControl";
            this.Size = new System.Drawing.Size(610, 595);
            this.Load += new System.EventHandler(this.ArticleInformationControl_Load);
            this.uxLinkToDocumentPanel.ResumeLayout(false);
            this.uxLinkToDocumentPanel.PerformLayout();
            this.uxLockStatus.ResumeLayout(false);
            this.uxLockStatus.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.uxVersionStatus.ResumeLayout(false);
            this.uxVersionStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox uxArticleNumberLabel;
		private System.Windows.Forms.Panel uxLinkToDocumentPanel;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button uxLinkDocument;
		private System.Windows.Forms.TextBox uxArticleNumberToLink;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.ComboBox uxPublication;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.ComboBox uxArticleInformationIssue;
		private System.Windows.Forms.ComboBox uxArticleCategory;
		private System.Windows.Forms.DateTimePicker uxWebPublishDate;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox uxSelectAuthor;
		private System.Windows.Forms.Button uxAddAuthor;
		public EasyRemoveListView uxSelectedAuthors;
		private System.Windows.Forms.Label label15;
		public System.Windows.Forms.CheckBox uxNominate;
		public System.Windows.Forms.CheckBox uxTopStory;
		private System.Windows.Forms.Label uxWordCount;
		private System.Windows.Forms.Label label19;
		private System.Windows.Forms.Button uxUnlinkDocument;
		private System.Windows.Forms.Panel uxLockStatus;
		private System.Windows.Forms.Button uxRefreshStatus;
		private System.Windows.Forms.Label uxLockStatusLabel;
		private System.Windows.Forms.Label uxLockUser;
		private System.Windows.Forms.Label label88;
		private System.Windows.Forms.Panel uxVersionStatus;
		private System.Windows.Forms.Label uxLastUpdatedBy;
		private System.Windows.Forms.Label uxLastUpdateDate;
		private System.Windows.Forms.Label label20;
		private System.Windows.Forms.Label label21;
		private System.Windows.Forms.Label label22;
		private System.Windows.Forms.Button uxRefreshVersion;
		private System.Windows.Forms.Label uxVersionText;
		private System.Windows.Forms.Button uxLockButton;
		private System.Windows.Forms.Button uxUnlockButton;
		private System.Windows.Forms.DateTimePicker uxWebPublishTime;
		private System.Windows.Forms.CheckBox uxEmbargoed;
        private System.Windows.Forms.ComboBox cmbWebCategory;
        private System.Windows.Forms.Label lblWebCategory;
        public NotesControl PageNotesControl;
	}
}
