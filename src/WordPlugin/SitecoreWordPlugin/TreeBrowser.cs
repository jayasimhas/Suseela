using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using SitecoreTreeWalker.Sitecore;
using System.Threading;

namespace SitecoreTreeWalker
{
    public partial class TreeBrowser : UserControl
    {
        private void AddRootNode(TreeView tree, string path, string title)
        {
            var sitecorePath = new SitecorePath(title, path);
            sitecorePath.GetDecendents();
            var node = sitecorePath.GetNode();
            UpdateNode.AddTreeNode(sitecorePath, node.Nodes);
            tree.Nodes.Add(node);
        }

        private void NodeExpanded(object sender, TreeViewCancelEventArgs e)
        {
            var node = e.Node;
            foreach (TreeNode innerNode in node.Nodes)
            {
                var sitecorePath = innerNode.Tag as SitecorePath;
                var updater = new UpdateNode(sitecorePath, innerNode, this);
                var thread = new Thread((updater.Update));
                thread.Start();
            }
        }

        private const string ContentTree = "/sitecore/content";
        private const string MediaLibrary = "/sitecore/media library";

        private void SitecoreTreeView_DoubleClick(object sender, EventArgs e)
        {
            //var node = SitecoreTreeView.SelectedNode;
            //var path = node.Tag as SitecorePath;
            //int index = path.Path.Contains("media library") ? 1 : 0;
            //node.ContextMenu.MenuItems[index].PerformClick();
        }

        private void SitecoreTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            uxPreview.Visible = true;
            uxPreview.Image = SitecoreTreeWalker.Properties.Resources.loading;
            TreeNode node = e.Node;
            var sitecorePath = node.Tag as SitecorePath;

            if (sitecorePath == null)
            {
                uxPreview.Visible = false;
                return;
            }
            SitecoreItemGetter.SitecoreMediaItem mediaItem = 
                _siteCoreItemGetter.DownloadSiteCoreMediaItem(sitecorePath.Path);

            if ((mediaItem == null) ||
                !(mediaItem.Extension.ToLower().Contains("gif") ||
                          mediaItem.Extension.ToLower().Contains("jpg") ||
                          mediaItem.Extension.ToLower().Contains("png")))
            {
                uxPreview.Visible = false;
                return;
            }

            uxPreview.ImageLocation = mediaItem.FileName;
        }

        public TreeBrowser(Word.Application application, SitecoreItemGetter sitecoreItemGetter)
        {
            Application = application;
            InitializeComponent();
            ControlContext.Initalize(application);
            AddRootNode(uxSupportingDocumentsList, ContentTree, "Content");
            AddRootNode(uxBrowseImages, MediaLibrary, "Media Library");
            _siteCoreItemGetter = sitecoreItemGetter;
        }

        public TreeBrowser(Word.Application application)
            : this(application, new SitecoreItemGetter())
        { }

        protected SitecoreItemGetter _siteCoreItemGetter;
        protected Word.Application Application { get; set; }
    }
}
