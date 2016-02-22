using System;
using System.Drawing;
using System.Windows.Forms;
using SitecoreTreeWalker.Sitecore;
using Application = Microsoft.Office.Interop.Word.Application;

namespace SitecoreTreeWalker.UI.TreeBrowser
{
    public partial class TreeBrowser : UserControl
    {
        public TreeBrowser(Application application, SitecoreItemGetter sitecoreItemGetter)
        {
            TreeBrowserWithTabSelect(application, sitecoreItemGetter, "");
        }

        public void TreeBrowserWithTabSelect(Application application, SitecoreItemGetter sitecoreItemGetter, string selectedTab)
        {
            Globals.SitecoreAddin.Log("Initializing the tree browser...");
            Application = application;
            InitializeComponent();
            ControlContext.Initalize(application);
            //InitializeItems();
            //This constructor is called upon Microsoft Word startup. Initializing the items 
            //will call the the Sitecore Tree. Instead, load the items only when the user tries
            //to open the Sitecore Tree.
            _siteCoreItemGetter = sitecoreItemGetter;
            Globals.SitecoreAddin.Log("Tree Browser initialized.");
        }

        public TreeBrowser(Application application)
            : this(application, new SitecoreItemGetter())
        { }

        protected SitecoreItemGetter _siteCoreItemGetter;
        protected Application Application { get; set; }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Brush textBrush;
            TabPage tabPage = SupportingDocumentsTabPage.TabPages[e.Index];
            Rectangle tabBounds = SupportingDocumentsTabPage.GetTabRect(e.Index);
            const int leftPadding = 3;
            tabBounds.Width = tabBounds.Width - leftPadding;
            tabBounds.X = tabBounds.X + leftPadding;
            if (e.State == DrawItemState.Selected)
            {
                textBrush = new SolidBrush(Color.Black);
                g.FillRectangle(Brushes.White, e.Bounds);
            }
            else
            {
                textBrush = new SolidBrush(e.ForeColor);
                e.DrawBackground();
                g.FillRectangle(Brushes.LightGray, e.Bounds);
            }

            var tabFont = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point);
            var stringFlags = new StringFormat();
            stringFlags.Alignment = StringAlignment.Near;
            stringFlags.LineAlignment = StringAlignment.Center;

            g.DrawString
                (tabPage.Text,
                 tabFont,
                 textBrush,
                 tabBounds,
                 new StringFormat(stringFlags));
        }

    }
}
