using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm
{
	partial class temp
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
			this.relatedArticlesControl1 = new RelatedArticlesControl();
			this.SuspendLayout();
			// 
			// relatedArticlesControl1
			// 
			this.relatedArticlesControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
						| System.Windows.Forms.AnchorStyles.Left)
						| System.Windows.Forms.AnchorStyles.Right)));
			this.relatedArticlesControl1.BackColor = System.Drawing.Color.White;
			this.relatedArticlesControl1.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.relatedArticlesControl1.Location = new System.Drawing.Point(12, 12);
			this.relatedArticlesControl1.Name = "relatedArticlesControl1";
			this.relatedArticlesControl1.Size = new System.Drawing.Size(610, 650);
			this.relatedArticlesControl1.TabIndex = 0;
			// 
			// temp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(641, 684);
			this.Controls.Add(this.relatedArticlesControl1);
			this.Name = "temp";
			this.Text = "temp";
			this.ResumeLayout(false);

		}

		#endregion

		private RelatedArticlesControl relatedArticlesControl1;
	}
}