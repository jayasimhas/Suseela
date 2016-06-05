using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using PluginModels;
using Microsoft.Office.Interop.Word;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.Util.Document;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class FeaturedImage : ArticleDetailsPageUserControl
    {
        private bool isImageSelected { get; set; }
        public string imageSelected { get; set; }
        protected SitecoreItemGetter _siteCoreItemGetter;
        public FeaturedImage()
        {
            InitializeComponent();
            if (!DesignMode)
            {
                SetSitecoreItemGetter(new SitecoreItemGetter());
                ReloadItems();
            }

        }

        public ImageItem GetFeaturedImage()
        {
            if (imageSelected == null) { return null; }
            var image = new ImageItem
            {
                MediaId = SitecoreClient.GetItemGuidByPath(imageSelected),
                Alt = alttextLbl.Text
            };

            return image;
        }

        public string GetFeaturedImageCaption()
        {
            return captionTxtBox.Text;
        }
        public string GetFeaturedImageSource()
        {
            return sourceTxtBox.Text;
        }

        public void SetSitecoreItemGetter(SitecoreItemGetter siteCoreItemGetter)
        {
            _siteCoreItemGetter = siteCoreItemGetter;
        }

        private static void AddRootNode(TreeView tree, string path, string title)
        {
            var sitecorePath = new SitecorePath(title, path);
            try
            {
                sitecorePath.GetDecendents();
            }
            catch (WebException)
            {
                Globals.SitecoreAddin.AlertConnectionFailure();
            }

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
                var thread = new Thread(() =>
                {
                    try
                    {
                        updater.Update();
                    }
                    catch (WebException)
                    {

                        Globals.SitecoreAddin.AlertConnectionFailure();
                    }
                    catch (System.Web.Services.Protocols.SoapException)
                    {
                        Globals.SitecoreAddin.Alert("Window has gotten out of sync with Sitecore! Please refresh tab.");
                    }
                });
                thread.Start();
            }
        }

        private void uxBrowseImages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pictureBox1.Image = Properties.Resources.loading;
            ShowHideElements(true);

            TreeNode node = e.Node;
            var sitecorePath = node.Tag as SitecorePath;

            if (sitecorePath == null)
            {
                ShowHideElements(false);
                return;
            }
            try
            {
                SitecoreItemGetter.SitecoreMediaItem mediaItem = _siteCoreItemGetter.DownloadSiteCoreMediaItem(sitecorePath.Path);
                if ((mediaItem == null) ||
                    !(mediaItem.Extension.ToLower().Contains("gif") ||
                      mediaItem.Extension.ToLower().Contains("jpg") ||
                      mediaItem.Extension.ToLower().Contains("png")))
                {
                    ShowHideElements(false);
                    return;
                }

                imageSelected = sitecorePath.Path;
                pictureBox1.ImageLocation = mediaItem.FileName;
                filenameLbl.Text = mediaItem.Title;
                if (!string.IsNullOrEmpty(mediaItem.AltText))
                {
                    alttextLbl.Text = mediaItem.AltText;
                }

                UpdateIcon();
                isImageSelected = true;
            }
            catch (WebException)
            {
                Globals.SitecoreAddin.AlertConnectionFailure();
            }
        }

        public void ShowHideElements(bool state)
        {
            pictureBox1.Visible = state;
            filenameLblHeader.Visible = state;
            filenameLbl.Visible = state;
            alttextLblHeader.Visible = state;
            alttextLbl.Visible = state;
        }

        /// <summary>
        /// Initializes the contents of the "Browse Images".
        /// </summary>
        public void InitializeItems()
        {
            var values = SitecoreClient.GetGraphicsRootNode();
            AddRootNode(uxFeaturedImageTreeView, values[1], values[0]);
        }

        public void InitializeItems(ArticleStruct articleDetails)
        {
            if (articleDetails != null)
            {
                var featuredImage = articleDetails.FeaturedImageSource;
                var imageMediaItem = SitecoreClient.GetMediaLibraryItem(featuredImage);
            }

            var values = SitecoreClient.GetGraphicsRootNode();
            AddRootNode(uxFeaturedImageTreeView, values[1], values[0]);
        }

        public void ResetFields()
        {
            ClearItems();
            InitializeItems();
            sourceTxtBox.Text = string.Empty;
            captionTxtBox.Text = string.Empty;
            MenuItem.SetIndicatorIcon(Properties.Resources.redx);
        }

        public void UpdateFields(ArticleStruct articleDetails)
        {
            sourceTxtBox.Text = articleDetails.FeaturedImageSource;
            captionTxtBox.Text = articleDetails.FeaturedImageCaption;


            SitecoreItemGetter.SitecoreMediaItem mediaItem = _siteCoreItemGetter.DownloadSiteCoreMediaItem(articleDetails.FeaturedImage.ToString());
            if ((mediaItem == null) || !(mediaItem.Extension.ToLower().Contains("gif") || mediaItem.Extension.ToLower().Contains("jpg") || mediaItem.Extension.ToLower().Contains("png")))
            {
                pictureBox1.Visible = false;
                filenameLblHeader.Visible = false;
                filenameLbl.Visible = false;
                alttextLblHeader.Visible = false;
                alttextLbl.Visible = false;
                return;
            }

            imageSelected = articleDetails.FeaturedImage.ToString();
            pictureBox1.ImageLocation = mediaItem.FileName;
            filenameLbl.Text = mediaItem.Title;
            alttextLbl.Text = mediaItem.AltText;

        }
        /// <summary>
        /// Removes all items in the TreeBrowser
        /// </summary>
        public void ClearItems()
        {
            uxFeaturedImageTreeView.Nodes.Clear();
        }

        /// <summary>
        /// Removes the current items in the TreeBrowser and re-initializes them.
        /// </summary>
        public void ReloadItems()
        {
            ClearItems();
            InitializeItems();
        }

        private void clearBtn_Click(object sender, EventArgs e)
        {
            ResetFields();
            imageSelected = null;
            ShowHideElements(false);
            MenuItem.SetIndicatorIcon(Properties.Resources.redx);
            if (isImageSelected)
            {
                MenuItem.HasChanged = true;
                MenuItem.UpdateBackground();
                isImageSelected = false;
            }
            
        }

        private void UpdateIcon()
        {
            if (MenuItem != null)
            {
                MenuItem.SetIndicatorIcon(Properties.Resources.blankgreen);
                MenuItem.Refresh();
                String iconStr = "";
                MenuItem.SetIndicatorNumber(iconStr);
                MenuItem.HasChanged = true;
                MenuItem.UpdateBackground();
            }
        }
    }
}














