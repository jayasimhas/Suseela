using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.Util;

namespace InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls
{
	public partial class SupportingDocumentsControl : UserControl
	{
		protected SitecoreItemGetter _siteCoreItemGetter;
		public readonly List<string> ValidDocumentTypes =
			new List<string> {"doc", "docx", "ppt", "pptx", "xls", "xlsx", "pdf"};
		public SupportingDocumentsControl()
		{
			InitializeComponent();
		}

		protected void PreviewInfo(bool show)
		{
			foreach (Control control in panel1.Controls)
			{
				control.Visible = show;
			}
		}

		protected void ShowDocInfo(SitecoreItemGetter.SitecoreMediaItem doc)
		{
			uxDocumentTitle.Text = doc.Title;
			uxDocumentType.Text = doc.Extension;
			uxUploader.Text = doc.Uploader;
			uxUploadDate.Text = string.Format("{0:g}", doc.UploadDate);
		}

		private void uxBrowseDocuments_AfterSelect(object sender, TreeViewEventArgs e)
		{
			TreeNode node = e.Node;
			var sitecorePath = node.Tag as SitecorePath;
			PreviewInfo(false);
			if (sitecorePath == null)
			{
				return;
			}

			try
			{
				SitecoreItemGetter.SitecoreMediaItem mediaItem =
					_siteCoreItemGetter.DownloadSiteCoreMediaItem(sitecorePath.Path);
				if ((mediaItem == null) || !IsValidDocumentType(mediaItem.Extension.ToLower()))
				{
					return;
				}
				PreviewInfo(true);
				ShowDocInfo(mediaItem);
			}
			catch (WebException)
			{
				Globals.SitecoreAddin.AlertConnectionFailure();
			}
		}

		protected bool IsValidDocumentType(string type)
		{
			return ValidDocumentTypes.Contains(type);
		}
		public void SetSitecoreItemGetter(SitecoreItemGetter siteCoreItemGetter)
		{
			_siteCoreItemGetter = siteCoreItemGetter;
		}

		private void uxBrowseDocuments_BeforeExpand(object sender, TreeViewCancelEventArgs e)
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
				});
				thread.Start();
			}
		}

		private void SupportingDocumentsControl_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				SetSitecoreItemGetter(new SitecoreItemGetter());
				var values = SitecoreClient.GetSupportingDocumentsRootNode();
				AddRootNode(uxBrowseDocuments, values[1], values[0]);
			}
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

		private void uxViewDocument_Click(object sender, EventArgs e)
		{
			var path = uxBrowseDocuments.SelectedNode.Tag as SitecorePath;
			if (path == null) return;
			try
			{
				Process.Start(SitecoreClient.MediaPreviewUrl(path.Path));
			}
			catch (WebException)
			{
				Globals.SitecoreAddin.AlertConnectionFailure();
			}
		}

		private void uxInsert_Click(object sender, EventArgs e)
		{
			var path = uxBrowseDocuments.SelectedNode.Tag as SitecorePath;
			if (path == null) return;
			SitecoreItemGetter.SitecoreMediaItem mediaItem =_siteCoreItemGetter.DownloadSiteCoreMediaItem(path.Path);
			if ((mediaItem == null) || !IsValidDocumentType(mediaItem.Extension.ToLower()))
			{
				MessageBox.Show(@"Please insert valid media item.", @"Informa");
				return;
			}
			InsertDocument(path);
		}



		static void InsertDocument(SitecorePath path)
		{
			var media = SitecoreClient.GetMediaStatistics(path.Path);
			var app = Globals.SitecoreAddin.Application;
			var selection = app.Selection.Range;
			selection.Text = path.DisplayName + "." + media.Extension;
			app.ActiveDocument.Hyperlinks.Add(selection, media.Url, null,
											  SupportingDocumentsReferenceBuilder.DocumentHyperlinkTooltip + ":" + path.Path);
			var toSelect = app.ActiveDocument.Range(selection.End, selection.End);
			toSelect.Select();
		}

		private void uxRefresh_Click(object sender, EventArgs e)
		{
			var values = SitecoreClient.GetSupportingDocumentsRootNode();
			uxBrowseDocuments.Nodes.Clear();
			AddRootNode(uxBrowseDocuments, values[1], values[0]);
			PreviewInfo(false);
		}
	}
}
