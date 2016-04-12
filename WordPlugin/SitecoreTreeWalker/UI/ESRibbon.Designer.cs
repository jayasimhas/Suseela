namespace InformaSitecoreWord.UI
{
    partial class ESRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public ESRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ESRibbon));
			this.tab1 = this.Factory.CreateRibbonTab();
			this.InsightTab = this.Factory.CreateRibbonTab();
			this.PluginGrp = this.Factory.CreateRibbonGroup();
			this.OpenPluginBtn = this.Factory.CreateRibbonButton();
			this.SaveToSitecoreBtn = this.Factory.CreateRibbonButton();
			this.ArticlePreviewMenu = this.Factory.CreateRibbonButton();
			this.ReferencesGrp = this.Factory.CreateRibbonGroup();
			this.ArticlesBtn = this.Factory.CreateRibbonButton();
			this.IntelligenceProductsBtn = this.Factory.CreateRibbonButton();
			this.Multimedia = this.Factory.CreateRibbonButton();
			this.ImagesBtn = this.Factory.CreateRibbonButton();
			this.SupportingDocsBtn = this.Factory.CreateRibbonButton();
			this.LogoutGrp = this.Factory.CreateRibbonGroup();
			this.LogoutBtn = this.Factory.CreateRibbonButton();
			this.LoginBtn = this.Factory.CreateRibbonButton();
			this.tab1.SuspendLayout();
			this.InsightTab.SuspendLayout();
			this.PluginGrp.SuspendLayout();
			this.ReferencesGrp.SuspendLayout();
			this.LogoutGrp.SuspendLayout();
			this.SuspendLayout();
			// 
			// tab1
			// 
			this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
			this.tab1.Label = "Add-Ins";
			this.tab1.Name = "tab1";
			// 
			// InsightTab
			// 
			this.InsightTab.Groups.Add(this.PluginGrp);
			this.InsightTab.Groups.Add(this.ReferencesGrp);
			this.InsightTab.Groups.Add(this.LogoutGrp);
			this.InsightTab.Label = "Insight Platform";
			this.InsightTab.Name = "InsightTab";
			// 
			// PluginGrp
			// 
			this.PluginGrp.Items.Add(this.OpenPluginBtn);
			this.PluginGrp.Items.Add(this.SaveToSitecoreBtn);
			this.PluginGrp.Items.Add(this.ArticlePreviewMenu);
			this.PluginGrp.Name = "PluginGrp";
			// 
			// OpenPluginBtn
			// 
			this.OpenPluginBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.OpenPluginBtn.Description = "Open Plugin";
			this.OpenPluginBtn.Image = ((System.Drawing.Image)(resources.GetObject("OpenPluginBtn.Image")));
			this.OpenPluginBtn.Label = "Open Plug-In";
			this.OpenPluginBtn.Name = "OpenPluginBtn";
			this.OpenPluginBtn.OfficeImageId = "PropertySheet";
			this.OpenPluginBtn.ShowImage = true;
			this.OpenPluginBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.OpenPluginBtn_Click);
			// 
			// SaveToSitecoreBtn
			// 
			this.SaveToSitecoreBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.SaveToSitecoreBtn.Image = ((System.Drawing.Image)(resources.GetObject("SaveToSitecoreBtn.Image")));
			this.SaveToSitecoreBtn.Label = "Save to Sitecore";
			this.SaveToSitecoreBtn.Name = "SaveToSitecoreBtn";
			this.SaveToSitecoreBtn.OfficeImageId = "PropertySheet";
			this.SaveToSitecoreBtn.ShowImage = true;
			this.SaveToSitecoreBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SaveToSitecoreBtn_Click);
			// 
			// ArticlePreviewMenu
			// 
			this.ArticlePreviewMenu.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.ArticlePreviewMenu.Image = ((System.Drawing.Image)(resources.GetObject("ArticlePreviewMenu.Image")));
			this.ArticlePreviewMenu.Label = "Article Preview";
			this.ArticlePreviewMenu.Name = "ArticlePreviewMenu";
			this.ArticlePreviewMenu.OfficeImageId = "PropertySheet";
			this.ArticlePreviewMenu.ShowImage = true;
			this.ArticlePreviewMenu.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ArticlePreviewMenu_Click);
			// 
			// ReferencesGrp
			// 
			this.ReferencesGrp.Items.Add(this.ArticlesBtn);
			this.ReferencesGrp.Items.Add(this.IntelligenceProductsBtn);
			this.ReferencesGrp.Items.Add(this.Multimedia);
			this.ReferencesGrp.Items.Add(this.ImagesBtn);
			this.ReferencesGrp.Items.Add(this.SupportingDocsBtn);
			this.ReferencesGrp.Label = "References";
			this.ReferencesGrp.Name = "ReferencesGrp";
			// 
			// ArticlesBtn
			// 
			this.ArticlesBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.ArticlesBtn.Image = ((System.Drawing.Image)(resources.GetObject("ArticlesBtn.Image")));
			this.ArticlesBtn.Label = "Articles";
			this.ArticlesBtn.Name = "ArticlesBtn";
			this.ArticlesBtn.OfficeImageId = "PropertySheet";
			this.ArticlesBtn.ShowImage = true;
			this.ArticlesBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ArticlesBtn_Click);
			// 
			// IntelligenceProductsBtn
			// 
			this.IntelligenceProductsBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.IntelligenceProductsBtn.Image = ((System.Drawing.Image)(resources.GetObject("IntelligenceProductsBtn.Image")));
			this.IntelligenceProductsBtn.Label = "Intelligence Products";
			this.IntelligenceProductsBtn.Name = "IntelligenceProductsBtn";
			this.IntelligenceProductsBtn.OfficeImageId = "PropertySheet";
			this.IntelligenceProductsBtn.ShowImage = true;
			this.IntelligenceProductsBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.IntelligenceProductsBtn_Click);
			// 
			// Multimedia
			// 
			this.Multimedia.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.Multimedia.Image = ((System.Drawing.Image)(resources.GetObject("Multimedia.Image")));
			this.Multimedia.Label = "Multimedia";
			this.Multimedia.Name = "Multimedia";
			this.Multimedia.OfficeImageId = "PropertySheet";
			this.Multimedia.ShowImage = true;
			this.Multimedia.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.Multimedia_Click);
			// 
			// ImagesBtn
			// 
			this.ImagesBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.ImagesBtn.Image = ((System.Drawing.Image)(resources.GetObject("ImagesBtn.Image")));
			this.ImagesBtn.Label = "Images";
			this.ImagesBtn.Name = "ImagesBtn";
			this.ImagesBtn.OfficeImageId = "PropertySheet";
			this.ImagesBtn.ShowImage = true;
			this.ImagesBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.ImagesBtn_Click);
			// 
			// SupportingDocsBtn
			// 
			this.SupportingDocsBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.SupportingDocsBtn.Image = ((System.Drawing.Image)(resources.GetObject("SupportingDocsBtn.Image")));
			this.SupportingDocsBtn.Label = "Supporting Docs";
			this.SupportingDocsBtn.Name = "SupportingDocsBtn";
			this.SupportingDocsBtn.OfficeImageId = "PropertySheet";
			this.SupportingDocsBtn.ShowImage = true;
			this.SupportingDocsBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.SupportingDocsBtn_Click);
			// 
			// LogoutGrp
			// 
			this.LogoutGrp.Items.Add(this.LogoutBtn);
			this.LogoutGrp.Items.Add(this.LoginBtn);
			this.LogoutGrp.Name = "LogoutGrp";
			// 
			// LogoutBtn
			// 
			this.LogoutBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.LogoutBtn.Image = global::InformaSitecoreWord.Properties.Resources.sidearrow;
			this.LogoutBtn.Label = "Logout";
			this.LogoutBtn.Name = "LogoutBtn";
			this.LogoutBtn.ShowImage = true;
			this.LogoutBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.LogoutBtn_Click);
			// 
			// LoginBtn
			// 
			this.LoginBtn.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
			this.LoginBtn.Image = ((System.Drawing.Image)(resources.GetObject("LoginBtn.Image")));
			this.LoginBtn.Label = "Login";
			this.LoginBtn.Name = "LoginBtn";
			this.LoginBtn.ShowImage = true;
			this.LoginBtn.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.LoginButton_Click);
			// 
			// ESRibbon
			// 
			this.Name = "ESRibbon";
			this.RibbonType = "Microsoft.Word.Document";
			this.Tabs.Add(this.tab1);
			this.Tabs.Add(this.InsightTab);
			this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.ESRibbon_Load);
			this.tab1.ResumeLayout(false);
			this.tab1.PerformLayout();
			this.InsightTab.ResumeLayout(false);
			this.InsightTab.PerformLayout();
			this.PluginGrp.ResumeLayout(false);
			this.PluginGrp.PerformLayout();
			this.ReferencesGrp.ResumeLayout(false);
			this.ReferencesGrp.PerformLayout();
			this.LogoutGrp.ResumeLayout(false);
			this.LogoutGrp.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        private Microsoft.Office.Tools.Ribbon.RibbonTab InsightTab;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup PluginGrp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LogoutBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton OpenPluginBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SaveToSitecoreBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup ReferencesGrp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ArticlesBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton IntelligenceProductsBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton Multimedia;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton ImagesBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton SupportingDocsBtn;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup LogoutGrp;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton LoginBtn;
		internal Microsoft.Office.Tools.Ribbon.RibbonButton ArticlePreviewMenu;
	}

	partial class ThisRibbonCollection
    {
        //internal ESRibbon ESRibbon
        //{
        //    get { return this.GetRibbon<ESRibbon>(); }
        //}
    }
}
