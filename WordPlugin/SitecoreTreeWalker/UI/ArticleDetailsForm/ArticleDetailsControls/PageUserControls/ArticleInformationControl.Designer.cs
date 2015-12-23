﻿namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbWebCategory = new System.Windows.Forms.ComboBox();
            this.lblWebCategory = new System.Windows.Forms.Label();
            this.uxArticleCategory = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.uxArticleInformationIssue = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.uxWebPublishTime = new System.Windows.Forms.DateTimePicker();
            this.uxEmbargoed = new System.Windows.Forms.CheckBox();
            this.uxWebPublishDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.uxPublication = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.uxNominate = new System.Windows.Forms.CheckBox();
            this.uxTopStory = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.uxSelectAuthor = new System.Windows.Forms.ComboBox();
            this.uxAddAuthor = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.uxSelectedAuthors = new SitecoreTreeWalker.UI.EasyRemoveListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
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
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cmbWebCategory);
            this.panel2.Controls.Add(this.lblWebCategory);
            this.panel2.Controls.Add(this.uxArticleCategory);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.uxArticleInformationIssue);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Location = new System.Drawing.Point(2, 544);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(605, 154);
            this.panel2.TabIndex = 15;
            this.panel2.Paint += new System.Windows.Forms.PaintEventHandler(this.panel2_Paint);
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
            this.uxArticleInformationIssue.Location = new System.Drawing.Point(494, 121);
            this.uxArticleInformationIssue.Name = "uxArticleInformationIssue";
            this.uxArticleInformationIssue.Size = new System.Drawing.Size(106, 21);
            this.uxArticleInformationIssue.TabIndex = 35;
            this.uxArticleInformationIssue.SelectedIndexChanged += new System.EventHandler(this.uxArticleInformationIssue_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label8.Location = new System.Drawing.Point(491, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(36, 13);
            this.label8.TabIndex = 34;
            this.label8.Text = "Issue:";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(81, 49);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(504, 22);
            this.textBox2.TabIndex = 81;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label4.Location = new System.Drawing.Point(6, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 13);
            this.label4.TabIndex = 44;
            this.label4.Text = "Label :";
            // 
            // uxWebPublishTime
            // 
            this.uxWebPublishTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.uxWebPublishTime.Location = new System.Drawing.Point(9, 109);
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
            this.uxEmbargoed.Location = new System.Drawing.Point(243, 111);
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
            this.uxWebPublishDate.Location = new System.Drawing.Point(121, 109);
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
            this.label11.Location = new System.Drawing.Point(6, 84);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(255, 13);
            this.label11.TabIndex = 40;
            this.label11.Text = "Scheduled Article Publish Date and Time (local):";
            // 
            // uxPublication
            // 
            this.uxPublication.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.uxPublication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.uxPublication.FormattingEnabled = true;
            this.uxPublication.Location = new System.Drawing.Point(81, 15);
            this.uxPublication.Name = "uxPublication";
            this.uxPublication.Size = new System.Drawing.Size(504, 21);
            this.uxPublication.TabIndex = 33;
            this.uxPublication.SelectedIndexChanged += new System.EventHandler(this.uxPublication_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label7.Location = new System.Drawing.Point(6, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Publication:";
            // 
            // uxNominate
            // 
            this.uxNominate.AutoSize = true;
            this.uxNominate.Location = new System.Drawing.Point(447, 39);
            this.uxNominate.Name = "uxNominate";
            this.uxNominate.Size = new System.Drawing.Size(153, 17);
            this.uxNominate.TabIndex = 52;
            this.uxNominate.Text = "Nominate for Homepage";
            this.uxNominate.UseVisualStyleBackColor = true;
            // 
            // uxTopStory
            // 
            this.uxTopStory.AutoSize = true;
            this.uxTopStory.Location = new System.Drawing.Point(447, 18);
            this.uxTopStory.Name = "uxTopStory";
            this.uxTopStory.Size = new System.Drawing.Size(149, 17);
            this.uxTopStory.TabIndex = 51;
            this.uxTopStory.Text = "Top Story on Newsletter";
            this.uxTopStory.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label12.Location = new System.Drawing.Point(6, 18);
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
            this.uxSelectAuthor.Location = new System.Drawing.Point(9, 39);
            this.uxSelectAuthor.Name = "uxSelectAuthor";
            this.uxSelectAuthor.Size = new System.Drawing.Size(302, 21);
            this.uxSelectAuthor.TabIndex = 19;
            // 
            // uxAddAuthor
            // 
            this.uxAddAuthor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxAddAuthor.Location = new System.Drawing.Point(320, 39);
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
            this.label15.Location = new System.Drawing.Point(6, 66);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(106, 13);
            this.label15.TabIndex = 44;
            this.label15.Text = "Selected Author(s):";
            // 
            // uxSelectedAuthors
            // 
            this.uxSelectedAuthors.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.uxSelectedAuthors.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.uxSelectedAuthors.Location = new System.Drawing.Point(9, 87);
            this.uxSelectedAuthors.Name = "uxSelectedAuthors";
            this.uxSelectedAuthors.Size = new System.Drawing.Size(373, 74);
            this.uxSelectedAuthors.TabIndex = 46;
            this.uxSelectedAuthors.UseCompatibleStateImageBehavior = false;
            this.uxSelectedAuthors.View = System.Windows.Forms.View.Details;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.uxSelectAuthor);
            this.groupBox1.Controls.Add(this.uxAddAuthor);
            this.groupBox1.Controls.Add(this.uxNominate);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.uxSelectedAuthors);
            this.groupBox1.Controls.Add(this.uxTopStory);
            this.groupBox1.Location = new System.Drawing.Point(5, 229);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(602, 171);
            this.groupBox1.TabIndex = 44;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Authors";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
            this.label2.Location = new System.Drawing.Point(6, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(130, 13);
            this.label2.TabIndex = 47;
            this.label2.Text = "Media Type (Optional) :";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(142, 142);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(443, 21);
            this.comboBox1.TabIndex = 78;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(6, 21);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(579, 90);
            this.textBox1.TabIndex = 80;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.uxEmbargoed);
            this.groupBox2.Controls.Add(this.uxWebPublishTime);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.comboBox1);
            this.groupBox2.Controls.Add(this.textBox2);
            this.groupBox2.Controls.Add(this.uxWebPublishDate);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.uxPublication);
            this.groupBox2.Location = new System.Drawing.Point(7, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(598, 178);
            this.groupBox2.TabIndex = 81;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Article Information";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.textBox1);
            this.groupBox3.Location = new System.Drawing.Point(7, 409);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(598, 117);
            this.groupBox3.TabIndex = 82;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Notes";
            // 
            // ArticleInformationControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ArticleInformationControl";
            this.Size = new System.Drawing.Size(610, 616);
            this.Load += new System.EventHandler(this.ArticleInformationControl_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

		}

		#endregion

        private System.Windows.Forms.Label label1;
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
		private System.Windows.Forms.DateTimePicker uxWebPublishTime;
		private System.Windows.Forms.CheckBox uxEmbargoed;
        private System.Windows.Forms.ComboBox cmbWebCategory;
        private System.Windows.Forms.Label lblWebCategory;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
	}
}
