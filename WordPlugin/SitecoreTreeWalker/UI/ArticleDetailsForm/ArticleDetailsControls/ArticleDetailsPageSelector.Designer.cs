using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	partial class ArticleDetailsPageSelector
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
			this.FlowLayoutMenu = new System.Windows.Forms.FlowLayoutPanel();
			this.ArticleInformationMenuItem = new MenuSelectorItem();
			this.RelatedArticlesMenuItem = new MenuSelectorItem();
			this.WorkflowMenuItem = new MenuSelectorItem();
			this.FeaturedImagesMenuItem = new MenuSelectorItem();
			this.TaxonomyMenuItem = new MenuSelectorItem();
			this.CurrentPagePanel = new System.Windows.Forms.Panel();
			this.pageArticleInformationControl = new ArticleInformationControl();
			this.pageTaxonomyControl = new TaxonomyControl();
			this.pageFeaturedImageControl = new FeaturedImage();
			this.pageWorkflowControl = new WorkflowControl();
			this.pageRelatedArticlesControl = new RelatedArticlesControl();
			this.FlowLayoutPanel = new System.Windows.Forms.Panel();
			this.FlowLayoutMenu.SuspendLayout();
			this.CurrentPagePanel.SuspendLayout();
			this.FlowLayoutPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// FlowLayoutMenu
			// 
			this.FlowLayoutMenu.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.FlowLayoutMenu.BackColor = System.Drawing.Color.White;
			this.FlowLayoutMenu.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FlowLayoutMenu.Controls.Add(this.ArticleInformationMenuItem);
			this.FlowLayoutMenu.Controls.Add(this.RelatedArticlesMenuItem);
			this.FlowLayoutMenu.Controls.Add(this.WorkflowMenuItem);
			this.FlowLayoutMenu.Controls.Add(this.FeaturedImagesMenuItem);
			this.FlowLayoutMenu.Controls.Add(this.TaxonomyMenuItem);
			this.FlowLayoutMenu.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
			this.FlowLayoutMenu.Location = new System.Drawing.Point(0, 0);
			this.FlowLayoutMenu.Name = "FlowLayoutMenu";
			this.FlowLayoutMenu.Size = new System.Drawing.Size(180, 268);
			this.FlowLayoutMenu.TabIndex = 0;
			// 
			// ArticleInformationMenuItem
			// 
			this.ArticleInformationMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.ArticleInformationMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ArticleInformationMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.ArticleInformationMenuItem.HasChanged = false;
			this.ArticleInformationMenuItem.Location = new System.Drawing.Point(0, 0);
			this.ArticleInformationMenuItem.Margin = new System.Windows.Forms.Padding(0);
			this.ArticleInformationMenuItem.Name = "ArticleInformationMenuItem";
			this.ArticleInformationMenuItem.Selected = false;
			this.ArticleInformationMenuItem.Size = new System.Drawing.Size(180, 35);
			this.ArticleInformationMenuItem.TabIndex = 0;
			this.ArticleInformationMenuItem.Click += new System.EventHandler(this.ArticleInformationMenuItem_Click);
			// 
			// RelatedArticlesMenuItem
			// 
			this.RelatedArticlesMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.RelatedArticlesMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.RelatedArticlesMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.RelatedArticlesMenuItem.HasChanged = false;
			this.RelatedArticlesMenuItem.Location = new System.Drawing.Point(0, 35);
			this.RelatedArticlesMenuItem.Margin = new System.Windows.Forms.Padding(0);
			this.RelatedArticlesMenuItem.Name = "RelatedArticlesMenuItem";
			this.RelatedArticlesMenuItem.Selected = false;
			this.RelatedArticlesMenuItem.Size = new System.Drawing.Size(180, 35);
			this.RelatedArticlesMenuItem.TabIndex = 1;
			this.RelatedArticlesMenuItem.Click += new System.EventHandler(this.RelatedArticlesMenuItem_Click);
			// 
			// WorkflowMenuItem
			// 
			this.WorkflowMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.WorkflowMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.WorkflowMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.WorkflowMenuItem.HasChanged = false;
			this.WorkflowMenuItem.Location = new System.Drawing.Point(0, 70);
			this.WorkflowMenuItem.Margin = new System.Windows.Forms.Padding(0);
			this.WorkflowMenuItem.Name = "WorkflowMenuItem";
			this.WorkflowMenuItem.Selected = false;
			this.WorkflowMenuItem.Size = new System.Drawing.Size(180, 35);
			this.WorkflowMenuItem.TabIndex = 5;
			this.WorkflowMenuItem.Click += new System.EventHandler(this.WorkflowMenuItem_Click);
			// 
			// FeaturedImagesMenuItem
			// 
			this.FeaturedImagesMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.FeaturedImagesMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.FeaturedImagesMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.FeaturedImagesMenuItem.HasChanged = false;
			this.FeaturedImagesMenuItem.Location = new System.Drawing.Point(0, 105);
			this.FeaturedImagesMenuItem.Margin = new System.Windows.Forms.Padding(0);
			this.FeaturedImagesMenuItem.Name = "FeaturedImagesMenuItem";
			this.FeaturedImagesMenuItem.Selected = false;
			this.FeaturedImagesMenuItem.Size = new System.Drawing.Size(180, 35);
			this.FeaturedImagesMenuItem.TabIndex = 6;
			this.FeaturedImagesMenuItem.Click += new System.EventHandler(this.FeaturedImageMenuItem_Click);
			// 
			// TaxonomyMenuItem
			// 
			this.TaxonomyMenuItem.BackColor = System.Drawing.Color.Transparent;
			this.TaxonomyMenuItem.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.TaxonomyMenuItem.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.TaxonomyMenuItem.HasChanged = false;
			this.TaxonomyMenuItem.Location = new System.Drawing.Point(0, 140);
			this.TaxonomyMenuItem.Margin = new System.Windows.Forms.Padding(0);
			this.TaxonomyMenuItem.Name = "TaxonomyMenuItem";
			this.TaxonomyMenuItem.Selected = false;
			this.TaxonomyMenuItem.Size = new System.Drawing.Size(180, 35);
			this.TaxonomyMenuItem.TabIndex = 7;
			this.TaxonomyMenuItem.Click += new System.EventHandler(this.TaxonomyMenuItem_Click);
			// 
			// CurrentPagePanel
			// 
			this.CurrentPagePanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
| System.Windows.Forms.AnchorStyles.Left)
| System.Windows.Forms.AnchorStyles.Right)));
			this.CurrentPagePanel.BackColor = System.Drawing.Color.White;
			this.CurrentPagePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.CurrentPagePanel.Controls.Add(this.pageArticleInformationControl);
			this.CurrentPagePanel.Controls.Add(this.pageTaxonomyControl);
			this.CurrentPagePanel.Controls.Add(this.pageFeaturedImageControl);
			this.CurrentPagePanel.Controls.Add(this.pageWorkflowControl);
			this.CurrentPagePanel.Controls.Add(this.pageRelatedArticlesControl);
			this.CurrentPagePanel.Location = new System.Drawing.Point(190, 3);
			this.CurrentPagePanel.Name = "CurrentPagePanel";
			this.CurrentPagePanel.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
			this.CurrentPagePanel.Size = new System.Drawing.Size(650, 549);
			this.CurrentPagePanel.TabIndex = 1;
			// 
			// pageArticleInformationControl
			// 
			this.pageArticleInformationControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageArticleInformationControl.BackColor = System.Drawing.Color.White;
			this.pageArticleInformationControl.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.pageArticleInformationControl.IsCheckedOut = false;
			this.pageArticleInformationControl.IsCheckedOutByMe = false;
			this.pageArticleInformationControl.Location = new System.Drawing.Point(11, 10);
			this.pageArticleInformationControl.Name = "pageArticleInformationControl";
			this.pageArticleInformationControl.Size = new System.Drawing.Size(627, 535);
			this.pageArticleInformationControl.TabIndex = 0;
			this.pageArticleInformationControl.Visible = false;
			// 
			// pageTaxonomyControl
			// 
			this.pageTaxonomyControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageTaxonomyControl.Location = new System.Drawing.Point(20, 10);
			this.pageTaxonomyControl.Name = "pageTaxonomyControl";
			this.pageTaxonomyControl.Size = new System.Drawing.Size(610, 449);
			this.pageTaxonomyControl.TabIndex = 7;
			this.pageTaxonomyControl.Visible = false;
			// 
			// pageSubjectsControl
			// 
			this.pageFeaturedImageControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageFeaturedImageControl.BackColor = System.Drawing.Color.White;
			this.pageFeaturedImageControl.Location = new System.Drawing.Point(20, 10);
			this.pageFeaturedImageControl.Name = "pageFeaturedImageControl";
			this.pageFeaturedImageControl.Size = new System.Drawing.Size(610, 449);
			this.pageFeaturedImageControl.TabIndex = 6;
			this.pageFeaturedImageControl.Visible = false;
			// 
			// pageWorkflowControl
			// 
			this.pageWorkflowControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageWorkflowControl.BackColor = System.Drawing.Color.White;
			this.pageWorkflowControl.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.pageWorkflowControl.Location = new System.Drawing.Point(20, 10);
			this.pageWorkflowControl.Margin = new System.Windows.Forms.Padding(0);
			this.pageWorkflowControl.Name = "pageWorkflowControl";
			this.pageWorkflowControl.Size = new System.Drawing.Size(610, 449);
			this.pageWorkflowControl.TabIndex = 5;
			this.pageWorkflowControl.Visible = false;
			// 
			// pageRelatedArticlesControl
			// 
			this.pageRelatedArticlesControl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pageRelatedArticlesControl.BackColor = System.Drawing.Color.White;
			this.pageRelatedArticlesControl.Font = new System.Drawing.Font("Segoe UI", 8.25F);
			this.pageRelatedArticlesControl.Location = new System.Drawing.Point(20, 10);
			this.pageRelatedArticlesControl.Name = "pageRelatedArticlesControl";
			this.pageRelatedArticlesControl.Size = new System.Drawing.Size(610, 449);
			this.pageRelatedArticlesControl.TabIndex = 1;
			this.pageRelatedArticlesControl.Visible = false;
			// 
			// FlowLayoutPanel
			// 
			this.FlowLayoutPanel.Controls.Add(this.FlowLayoutMenu);
			this.FlowLayoutPanel.Location = new System.Drawing.Point(3, 3);
			this.FlowLayoutPanel.Name = "FlowLayoutPanel";
			this.FlowLayoutPanel.Size = new System.Drawing.Size(180, 271);
			this.FlowLayoutPanel.TabIndex = 2;
			this.FlowLayoutPanel.Paint += new System.Windows.Forms.PaintEventHandler(this.FlowLayoutPanel_Paint);
			// 
			// ArticleDetailsPageSelector
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.Transparent;
			this.Controls.Add(this.FlowLayoutPanel);
			this.Controls.Add(this.CurrentPagePanel);
			this.Name = "ArticleDetailsPageSelector";
			this.Size = new System.Drawing.Size(850, 641);
			this.FlowLayoutMenu.ResumeLayout(false);
			this.CurrentPagePanel.ResumeLayout(false);
			this.FlowLayoutPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.FlowLayoutPanel FlowLayoutMenu;
		private System.Windows.Forms.Panel CurrentPagePanel;
		public MenuSelectorItem ArticleInformationMenuItem;
		public MenuSelectorItem RelatedArticlesMenuItem;
		//public MenuSelectorItem RelatedDealsMenuItem;
		//public MenuSelectorItem SupportingDocumentsMenuItem;
		//public MenuSelectorItem CompaniesMenuItem;
		public MenuSelectorItem WorkflowMenuItem;
		public MenuSelectorItem FeaturedImagesMenuItem;
        public MenuSelectorItem TaxonomyMenuItem;					
		//public MenuSelectorItem NotesMenuItem;
		public ArticleInformationControl pageArticleInformationControl;
		public RelatedArticlesControl pageRelatedArticlesControl;
		//public NotesControl PageNotesControl;						
		public TaxonomyControl pageTaxonomyControl;
		public FeaturedImage pageFeaturedImageControl;
		public WorkflowControl pageWorkflowControl;
        private System.Windows.Forms.Panel FlowLayoutPanel;
	}
}
