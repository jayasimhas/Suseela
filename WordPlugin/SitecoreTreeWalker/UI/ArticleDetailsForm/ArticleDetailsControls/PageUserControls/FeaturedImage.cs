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
using Informa.Web.Areas.Account.Models;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using SitecoreTreeWalker.Util.Document;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
    public partial class FeaturedImage : ArticleDetailsPageUserControl
    {
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

	    public Glass.Mapper.Sc.Fields.Image GetFeaturedImage()
	    {
			if(imageSelected == null) { return null;}
		    var image = new Glass.Mapper.Sc.Fields.Image
		    {
			    MediaId = SitecoreGetter.GetItemGuidByPath(imageSelected),
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
                alttextLbl.Text = mediaItem.Title;
			}
            catch (WebException)
            {
                Globals.SitecoreAddin.AlertConnectionFailure();
            }
        }

	    public void ShowHideElements(bool state)
	    {
			groupBox2.Visible = state;
			pictureBox1.Visible = state;
			filenameLblHeader.Visible = state;
			filenameLbl.Visible = state;
			alttextLblHeader.Visible = state;
			alttextLbl.Visible = state;
			captionTxtBox.Visible = state;
			sourceTxtBox.Visible = state;
			label5.Visible = state;
			label6.Visible = state;
			clearBtn.Visible = state;			
		}

        void uxInsertImage_Click(object sender, System.EventArgs e)
        {
            var button = sender as Control;
            if (button == null) return;
            var path = uxFeaturedImageTreeView.SelectedNode.Tag as SitecorePath;
            if (path == null) return;
        }

        static void InsertImage(string header, string title, SitecorePath path, string caption, string source)
        {
            var app = Globals.SitecoreAddin.Application;
            int numParagraph = 2;
            if (!string.IsNullOrEmpty(header)) numParagraph = numParagraph + 2;
            if (!string.IsNullOrEmpty(title)) numParagraph = numParagraph + 2;
            if (!string.IsNullOrEmpty(caption)) numParagraph = numParagraph + 2;
            if (!string.IsNullOrEmpty(source)) numParagraph = numParagraph + 2;
            for (int i = 0; i < numParagraph + 1; i++)
            {
                app.Selection.TypeParagraph();
            }
            Range selection = app.Selection.Previous(WdUnits.wdParagraph, numParagraph);

            if (!string.IsNullOrWhiteSpace(header))
            {
                selection.Text = header;
                selection.set_Style(DocumentAndParagraphStyles.ExhibitNumberStyle);
                selection = selection.Next(WdUnits.wdParagraph);
            }

            if (!string.IsNullOrEmpty(title))
            {
                selection.Text = title;
                selection.set_Style(DocumentAndParagraphStyles.ExhibitTitleStyle);
                selection = selection.Next(WdUnits.wdParagraph);
            }

            selection.Text = "Image: ";
            selection.set_Style(DocumentAndParagraphStyles.ImagePreviewStyle);
            selection.Collapse(WdCollapseDirection.wdCollapseEnd);
            selection.Text = path.DisplayName;
            try
            {
                app.ActiveDocument.Hyperlinks.Add(selection, SitecoreGetter.MediaPreviewUrl(path.Path), null, path.Path);
            }
            catch (WebException)
            {
                Globals.SitecoreAddin.AlertConnectionFailure();
            }
            finally
            {
                selection = selection.Next(WdUnits.wdParagraph);
            }

            if (!string.IsNullOrEmpty(caption))
            {
                selection.Text = caption;
                selection.set_Style(DocumentAndParagraphStyles.ExhibitCaptionStyle52);
                selection = selection.Next(WdUnits.wdParagraph);
            }

            if (!string.IsNullOrWhiteSpace(source))
            {
                selection.Text = source;
                selection.set_Style(DocumentAndParagraphStyles.SourceStyle);
                selection.Next(WdUnits.wdParagraph).Select();
            }
        }

        /// <summary>
        /// Initializes the contents of the "Browse Images" and "Supporting Documents" views.
        /// </summary>
        public void InitializeItems()
        {
            var values = SitecoreGetter.GetGraphicsRootNode();
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

		public void UpdateFields(WordPluginModel.ArticleStruct articleDetails)
		{			
			sourceTxtBox.Text = articleDetails.FeaturedImageSource;
			captionTxtBox.Text = articleDetails.FeaturedImageCaption;


			SitecoreItemGetter.SitecoreMediaItem mediaItem = _siteCoreItemGetter.DownloadSiteCoreMediaItem(articleDetails.FeaturedImage.ToString());
			if ((mediaItem == null) || !(mediaItem.Extension.ToLower().Contains("gif") ||mediaItem.Extension.ToLower().Contains("jpg") ||mediaItem.Extension.ToLower().Contains("png")))
			{
				pictureBox1.Visible = false;
				filenameLblHeader.Visible = false;
				filenameLbl.Visible = false;
				alttextLblHeader.Visible = false;
				alttextLbl.Visible = false;
				return;
			}

			//imageSelected = mediaItem.Url;
			pictureBox1.ImageLocation = mediaItem.FileName;
			filenameLbl.Text = mediaItem.Title;
			alttextLbl.Text = mediaItem.Title;

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
			ReloadItems();
			ShowHideElements(false);
			MenuItem.SetIndicatorIcon(Properties.Resources.redx);
		}
	}
}














