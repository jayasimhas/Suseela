using System;

namespace SitecoreTreeWalker.UI.TreeBrowser
{
	partial class GraphicsMetadataForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GraphicsMetadataForm));
			this.label5 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.uxHeader = new System.Windows.Forms.TextBox();
			this.uxCaption = new System.Windows.Forms.TextBox();
			this.uxSource = new System.Windows.Forms.TextBox();
			this.uxCancel = new System.Windows.Forms.Button();
			this.uxInsertImage = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.uxTitle = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.uxSourceLabel = new System.Windows.Forms.Label();
			this.uxCaptionLabel = new System.Windows.Forms.Label();
			this.uxTitleLabel = new System.Windows.Forms.Label();
			this.uxHeaderLabel = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.uxFloatBox = new System.Windows.Forms.ComboBox();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(9, 10);
			this.label5.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(50, 13);
			this.label5.TabIndex = 11;
			this.label5.Text = "Header: ";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(9, 118);
			this.label2.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(54, 13);
			this.label2.TabIndex = 13;
			this.label2.Text = "Caption: ";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(12, 172);
			this.label3.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(45, 13);
			this.label3.TabIndex = 14;
			this.label3.Text = "Source:";
			// 
			// uxHeader
			// 
			this.uxHeader.Location = new System.Drawing.Point(12, 29);
			this.uxHeader.Name = "uxHeader";
			this.uxHeader.Size = new System.Drawing.Size(289, 22);
			this.uxHeader.TabIndex = 0;
			this.uxHeader.TextChanged += new System.EventHandler(this.header_TextChanged);
			// 
			// uxCaption
			// 
			this.uxCaption.Location = new System.Drawing.Point(12, 137);
			this.uxCaption.Name = "uxCaption";
			this.uxCaption.Size = new System.Drawing.Size(289, 22);
			this.uxCaption.TabIndex = 3;
			this.uxCaption.TextChanged += new System.EventHandler(this.caption_TextChanged);
			// 
			// uxSource
			// 
			this.uxSource.Location = new System.Drawing.Point(12, 191);
			this.uxSource.Multiline = true;
			this.uxSource.Name = "uxSource";
			this.uxSource.Size = new System.Drawing.Size(289, 44);
			this.uxSource.TabIndex = 4;
			this.uxSource.TextChanged += new System.EventHandler(this.source_TextChanged);
			// 
			// uxCancel
			// 
			this.uxCancel.Location = new System.Drawing.Point(68, 614);
			this.uxCancel.Name = "uxCancel";
			this.uxCancel.Size = new System.Drawing.Size(164, 23);
			this.uxCancel.TabIndex = 6;
			this.uxCancel.Text = "Cancel";
			this.uxCancel.UseVisualStyleBackColor = true;
			this.uxCancel.Click += new System.EventHandler(this.uxCancel_Click);
			// 
			// uxInsertImage
			// 
			this.uxInsertImage.Location = new System.Drawing.Point(68, 574);
			this.uxInsertImage.Name = "uxInsertImage";
			this.uxInsertImage.Size = new System.Drawing.Size(164, 23);
			this.uxInsertImage.TabIndex = 5;
			this.uxInsertImage.Text = "Insert";
			this.uxInsertImage.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(9, 64);
			this.label4.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(34, 13);
			this.label4.TabIndex = 12;
			this.label4.Text = "Title: ";
			// 
			// uxTitle
			// 
			this.uxTitle.Location = new System.Drawing.Point(12, 83);
			this.uxTitle.Name = "uxTitle";
			this.uxTitle.Size = new System.Drawing.Size(289, 22);
			this.uxTitle.TabIndex = 2;
			this.uxTitle.TextChanged += new System.EventHandler(this.title_TextChanged);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.pictureBox1);
			this.groupBox1.Controls.Add(this.uxSourceLabel);
			this.groupBox1.Controls.Add(this.uxCaptionLabel);
			this.groupBox1.Controls.Add(this.uxTitleLabel);
			this.groupBox1.Controls.Add(this.uxHeaderLabel);
			this.groupBox1.Location = new System.Drawing.Point(12, 314);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(286, 254);
			this.groupBox1.TabIndex = 15;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Preview";
			// 
			// pictureBox1
			// 
			this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox1.Location = new System.Drawing.Point(13, 59);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(256, 139);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 4;
			this.pictureBox1.TabStop = false;
			// 
			// uxSourceLabel
			// 
			this.uxSourceLabel.AutoSize = true;
			this.uxSourceLabel.Location = new System.Drawing.Point(10, 225);
			this.uxSourceLabel.Name = "uxSourceLabel";
			this.uxSourceLabel.Size = new System.Drawing.Size(42, 13);
			this.uxSourceLabel.TabIndex = 3;
			this.uxSourceLabel.Text = "Source";
			// 
			// uxCaptionLabel
			// 
			this.uxCaptionLabel.AutoSize = true;
			this.uxCaptionLabel.Location = new System.Drawing.Point(10, 201);
			this.uxCaptionLabel.Name = "uxCaptionLabel";
			this.uxCaptionLabel.Size = new System.Drawing.Size(48, 13);
			this.uxCaptionLabel.TabIndex = 2;
			this.uxCaptionLabel.Text = "Caption";
			// 
			// uxTitleLabel
			// 
			this.uxTitleLabel.AutoSize = true;
			this.uxTitleLabel.Location = new System.Drawing.Point(10, 43);
			this.uxTitleLabel.Name = "uxTitleLabel";
			this.uxTitleLabel.Size = new System.Drawing.Size(28, 13);
			this.uxTitleLabel.TabIndex = 1;
			this.uxTitleLabel.Text = "Title";
			// 
			// uxHeaderLabel
			// 
			this.uxHeaderLabel.AutoSize = true;
			this.uxHeaderLabel.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Bold);
			this.uxHeaderLabel.Location = new System.Drawing.Point(10, 18);
			this.uxHeaderLabel.Name = "uxHeaderLabel";
			this.uxHeaderLabel.Size = new System.Drawing.Size(44, 13);
			this.uxHeaderLabel.TabIndex = 0;
			this.uxHeaderLabel.Text = "Header";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 257);
			this.label1.Margin = new System.Windows.Forms.Padding(3, 10, 3, 3);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(39, 13);
			this.label1.TabIndex = 16;
			this.label1.Text = "Float: ";
			// 
			// usFloatBox
			// 
			this.uxFloatBox.FormattingEnabled = true;
			this.uxFloatBox.Items.AddRange(new object[] {
            "None",
            "Left",
            "Right"});
			this.uxFloatBox.Location = new System.Drawing.Point(15, 276);
			this.uxFloatBox.Name = "uxFloatBox";
			this.uxFloatBox.Size = new System.Drawing.Size(283, 21);
			this.uxFloatBox.TabIndex = 17;
			// 
			// GraphicsMetadataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(311, 649);
			this.Controls.Add(this.uxFloatBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.uxTitle);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.uxInsertImage);
			this.Controls.Add(this.uxCancel);
			this.Controls.Add(this.uxSource);
			this.Controls.Add(this.uxCaption);
			this.Controls.Add(this.uxHeader);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label5);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "GraphicsMetadataForm";
			this.Text = "Images Metadata";
			this.Load += new System.EventHandler(this.GraphicsMetadataForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		public System.Windows.Forms.TextBox uxHeader;
		public System.Windows.Forms.TextBox uxCaption;
		public System.Windows.Forms.TextBox uxSource;
		private System.Windows.Forms.Button uxCancel;
		public System.Windows.Forms.Button uxInsertImage;
		private System.Windows.Forms.Label label4;
		public System.Windows.Forms.TextBox uxTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label uxSourceLabel;
        private System.Windows.Forms.Label uxCaptionLabel;
        private System.Windows.Forms.Label uxTitleLabel;
        private System.Windows.Forms.Label uxHeaderLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
		public System.Windows.Forms.ComboBox uxFloatBox;
	}
}