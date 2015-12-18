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
			// 
			// uxCaption
			// 
			this.uxCaption.Location = new System.Drawing.Point(12, 137);
			this.uxCaption.Name = "uxCaption";
			this.uxCaption.Size = new System.Drawing.Size(289, 22);
			this.uxCaption.TabIndex = 3;
			// 
			// uxSource
			// 
			this.uxSource.Location = new System.Drawing.Point(12, 191);
			this.uxSource.Multiline = true;
			this.uxSource.Name = "uxSource";
			this.uxSource.Size = new System.Drawing.Size(289, 44);
			this.uxSource.TabIndex = 4;
			// 
			// uxCancel
			// 
			this.uxCancel.Location = new System.Drawing.Point(199, 241);
			this.uxCancel.Name = "uxCancel";
			this.uxCancel.Size = new System.Drawing.Size(75, 23);
			this.uxCancel.TabIndex = 6;
			this.uxCancel.Text = "Cancel";
			this.uxCancel.UseVisualStyleBackColor = true;
			this.uxCancel.Click += new System.EventHandler(this.uxCancel_Click);
			// 
			// uxInsertImage
			// 
			this.uxInsertImage.Location = new System.Drawing.Point(63, 241);
			this.uxInsertImage.Name = "uxInsertImage";
			this.uxInsertImage.Size = new System.Drawing.Size(87, 23);
			this.uxInsertImage.TabIndex = 5;
			this.uxInsertImage.Text = "Insert Image";
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
			// 
			// GraphicsMetadataForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.White;
			this.ClientSize = new System.Drawing.Size(311, 281);
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
			this.Text = "Graphics and Images Metadata";
			this.Load += new System.EventHandler(this.GraphicsMetadataForm_Load);
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
	}
}