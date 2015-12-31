using System;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.Util.Document;

namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
	public partial class GraphicsControl : UserControl
	{
		protected SitecoreItemGetter _siteCoreItemGetter;
		public GraphicsControl()
		{
			InitializeComponent();
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
						Globals.SitecoreAddin.Alert("Window has gotten out of sync with Sitecore! " +
													"Please refresh tab.");
					}
				});
				thread.Start();
			}
		}

		private void uxBrowseImages_AfterSelect(object sender, TreeViewEventArgs e)
		{
			uxPreview.Visible = true;
			uxPreview.Image = Properties.Resources.loading;
			TreeNode node = e.Node;
			var sitecorePath = node.Tag as SitecorePath;

			if (sitecorePath == null)
			{
				uxPreview.Visible = false;
				return;
			}
			try
			{
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
			catch (WebException)
			{
				Globals.SitecoreAddin.AlertConnectionFailure();
			}
		}

		/// <summary>
		/// Initializes the contents of the "Browse Images" and "Supporting Documents" views.
		/// </summary>
		public void InitializeItems()
		{
			var values = SitecoreGetter.GetGraphicsRootNode();
			AddRootNode(uxBrowseImages, values[1], values[0]);
		}

		/// <summary>
		/// Removes all items in the TreeBrowser
		/// </summary>
		public void ClearItems()
		{
			uxBrowseImages.Nodes.Clear();
		}

		/// <summary>
		/// Removes the current items in the TreeBrowser and re-initializes them.
		/// </summary>
		public void ReloadItems()
		{
			ClearItems();
			InitializeItems();
		}

		private void GraphicsControl_Load(object sender, System.EventArgs e)
		{
			if (!DesignMode)
			{
				SetSitecoreItemGetter(new SitecoreItemGetter());
				ReloadItems();
			}
		}

		private void uxInsertIntoArticle_Click(object sender, System.EventArgs e)
		{
			var sitecorePath = uxBrowseImages.SelectedNode.Tag as SitecorePath;
			if (sitecorePath != null)
			{
				try
				{
					SitecoreItemGetter.SitecoreMediaItem mediaItem = _siteCoreItemGetter.DownloadSiteCoreMediaItem(sitecorePath.Path);
					if ((mediaItem == null) ||
						!(mediaItem.Extension.ToLower().Contains("gif") ||
						  mediaItem.Extension.ToLower().Contains("jpg") ||
						  mediaItem.Extension.ToLower().Contains("png")))
					{
						return;
					}
					var form = new GraphicsMetadataForm(mediaItem.FileName);
					form.uxInsertImage.Click += new System.EventHandler(uxInsertImage_Click);
					form.ShowDialog();
				}
				catch (WebException)
				{
					Globals.SitecoreAddin.AlertConnectionFailure();
				}
			}
		}

		void uxInsertImage_Click(object sender, System.EventArgs e)
		{
			var button = sender as Control;
			if (button == null) return;
			var form = button.Parent as GraphicsMetadataForm;
			if (form == null) return;
			var path = uxBrowseImages.SelectedNode.Tag as SitecorePath;
			if(path == null) return;
			InsertImage(form.uxHeader.Text, form.uxTitle.Text, path, form.uxCaption.Text, form.uxSource.Text);
			form.Close();
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

		private void uxRefresh_Click(object sender, System.EventArgs e)
		{
			ReloadItems();
		}
	}
}
