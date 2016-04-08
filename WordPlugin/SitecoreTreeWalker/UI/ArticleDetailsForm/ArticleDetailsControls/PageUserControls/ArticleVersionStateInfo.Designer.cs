namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	partial class ArticleVersionStateInfo
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
			this.label1 = new System.Windows.Forms.Label();
			this.uxVersionUpdated = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.uxVersionUpdateDate = new System.Windows.Forms.Label();
			this.uxHeadline = new System.Windows.Forms.Label();
			this.uxRefresh = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(26, 26);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(138, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Last version updated by : ";
			// 
			// uxVersionUpdated
			// 
			this.uxVersionUpdated.AutoSize = true;
			this.uxVersionUpdated.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.uxVersionUpdated.Location = new System.Drawing.Point(158, 26);
			this.uxVersionUpdated.Name = "uxVersionUpdated";
			this.uxVersionUpdated.Size = new System.Drawing.Size(27, 13);
			this.uxVersionUpdated.TabIndex = 1;
			this.uxVersionUpdated.Text = "xxxx";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.label3.Location = new System.Drawing.Point(26, 49);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(26, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "On:";
			// 
			// uxVersionUpdateDate
			// 
			this.uxVersionUpdateDate.AutoSize = true;
			this.uxVersionUpdateDate.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.uxVersionUpdateDate.Location = new System.Drawing.Point(50, 49);
			this.uxVersionUpdateDate.Name = "uxVersionUpdateDate";
			this.uxVersionUpdateDate.Size = new System.Drawing.Size(45, 13);
			this.uxVersionUpdateDate.TabIndex = 3;
			this.uxVersionUpdateDate.Text = "xx/xx/xx";
			this.uxVersionUpdateDate.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// uxHeadline
			// 
			this.uxHeadline.AutoSize = true;
			this.uxHeadline.Location = new System.Drawing.Point(29, 10);
			this.uxHeadline.Name = "uxHeadline";
			this.uxHeadline.Size = new System.Drawing.Size(0, 13);
			this.uxHeadline.TabIndex = 4;
			// 
			// uxRefresh
			// 
			this.uxRefresh.Location = new System.Drawing.Point(89, 68);
			this.uxRefresh.Name = "uxRefresh";
			this.uxRefresh.Size = new System.Drawing.Size(75, 23);
			this.uxRefresh.TabIndex = 5;
			this.uxRefresh.Text = "Refresh";
			this.uxRefresh.UseVisualStyleBackColor = true;
			this.uxRefresh.Click += new System.EventHandler(this.uxRefresh_Click);
			// 
			// ArticleVersionStateInfo
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(262, 100);
			this.Controls.Add(this.uxRefresh);
			this.Controls.Add(this.uxHeadline);
			this.Controls.Add(this.uxVersionUpdateDate);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.uxVersionUpdated);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "ArticleVersionStateInfo";
			this.ShowIcon = false;
			this.Text = "Article Version";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label uxVersionUpdated;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label uxVersionUpdateDate;
        public System.Windows.Forms.Label uxHeadline;
        private System.Windows.Forms.Button uxRefresh;
	}
}