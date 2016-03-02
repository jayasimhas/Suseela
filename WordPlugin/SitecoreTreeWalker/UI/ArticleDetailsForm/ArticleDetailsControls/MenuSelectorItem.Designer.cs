namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	partial class MenuSelectorItem
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
			this.IndicatorIcon = new System.Windows.Forms.Label();
			this.MenuTitle = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// IndicatorIcon
			// 
			this.IndicatorIcon.BackColor = System.Drawing.Color.Transparent;
			this.IndicatorIcon.Font = new System.Drawing.Font("Segoe UI", 12F);
			this.IndicatorIcon.ForeColor = System.Drawing.Color.White;
			this.IndicatorIcon.Location = new System.Drawing.Point(0, 0);
			this.IndicatorIcon.Margin = new System.Windows.Forms.Padding(0);
			this.IndicatorIcon.Name = "IndicatorIcon";
			this.IndicatorIcon.Size = new System.Drawing.Size(35, 35);
			this.IndicatorIcon.TabIndex = 0;
			this.IndicatorIcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.IndicatorIcon.Click += new System.EventHandler(this.IndicatorIcon_Click);
			this.IndicatorIcon.MouseEnter += new System.EventHandler(this.IndicatorIcon_MouseEnter);
			this.IndicatorIcon.MouseLeave += new System.EventHandler(this.IndicatorIcon_MouseLeave);
			// 
			// MenuTitle
			// 
			this.MenuTitle.BackColor = System.Drawing.Color.Transparent;
			this.MenuTitle.Location = new System.Drawing.Point(35, 0);
			this.MenuTitle.Name = "MenuTitle";
			this.MenuTitle.Size = new System.Drawing.Size(145, 35);
			this.MenuTitle.TabIndex = 1;
			this.MenuTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.MenuTitle.Click += new System.EventHandler(this.MenuTitle_Click);
			this.MenuTitle.MouseEnter += new System.EventHandler(this.MenuTitle_MouseEnter);
			this.MenuTitle.MouseLeave += new System.EventHandler(this.MenuTitle_MouseLeave);
			// 
			// MenuSelectorItem
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.Controls.Add(this.MenuTitle);
			this.Controls.Add(this.IndicatorIcon);
			this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.Margin = new System.Windows.Forms.Padding(0);
			this.Name = "MenuSelectorItem";
			this.Size = new System.Drawing.Size(180, 35);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label IndicatorIcon;
		private System.Windows.Forms.Label MenuTitle;
	}
}
