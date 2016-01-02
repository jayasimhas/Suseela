using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using SitecoreTreeWalker.SitecoreTree;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;
using SitecoreTreeWalker.Sitecore;
using System.Threading;

namespace SitecoreTreeWalker
{
	public partial class TreeBrowser : UserControl
	{
		private const string ConnectionLostMessage = "Connection with Sitecore server lost. Please check your internet connection. " +
											   "If the problem persists, contact ed.schwehm@velir.com. To reload Sitecore items in browser, " +
											   "please toggle Sitecore Tree off and on.";

		private void AddRootNode(TreeView tree, string path, string title)
		{
			var sitecorePath = new SitecorePath(title, path);
			try
			{
				sitecorePath.GetDecendents();
			}
			catch (WebException)
			{

				MessageBox.Show(ConnectionLostMessage);
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
				var thread = new Thread(() => {
					try
					{
						updater.Update();
					}
					catch(WebException)
					{
						
						MessageBox.Show(ConnectionLostMessage);
					}
				});
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
			catch(WebException)
			{
				MessageBox.Show(
					@"Connection with Sitecore server lost. Please check your internet connection. If the problem persists, contact ed.schwehm@velir.com");
			}
		}

		public TreeBrowser(Word.Application application, SitecoreItemGetter sitecoreItemGetter)
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
			_scTree = new SCTree();
			Globals.SitecoreAddin.Log("Tree Browser initialized.");
		}

		/// <summary>
		/// Initializes the contents of the "Browse Images" and "Supporting Documents" views.
		/// </summary>
		public void InitializeItems()
		{
			AddRootNode(uxSupportingDocumentsList, ContentTree, "Content");
			AddRootNode(uxBrowseImages, MediaLibrary, "Media Library");
		}

		/// <summary>
		/// Removes all items in the TreeBrowser
		/// </summary>
		public void ClearItems()
		{
			uxSupportingDocumentsList.Nodes.Clear();
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

		public TreeBrowser(Word.Application application)
			: this(application, new SitecoreItemGetter())
		{ }

		protected SitecoreItemGetter _siteCoreItemGetter;
		protected Word.Application Application { get; set; }

		private void uxArticlesInsertIntoArticleById_Click(object sender, EventArgs e)
		{
			if(uxRelatedArticleID.Text == "")
			{
				MessageBox.Show(@"Please enter an article number");
				return;
			}

			Object address = _scTree.GetArticleUrl(uxRelatedArticleID.Text);

			if(address.ToString() == "")
			{
				MessageBox.Show(@"Article number does not exist!");
				return;
			}

			//string tooltip = _scTree.GetArticleDynamicUrl(uxRelatedArticleID.Text);

			var app = Globals.SitecoreAddin.Application;
			app.Selection.TypeText("[A#");
			Word.Range hyperlinkRange = app.ActiveDocument.Range(app.Selection.Start, app.Selection.End);
			Word.Hyperlink hyperlink = app.ActiveDocument.Hyperlinks.Add(hyperlinkRange, ref address, null, null, uxRelatedArticleID.Text);
			Word.Range range = hyperlink.Range;

			app.Selection.Start = range.End;
			app.Selection.End = range.End;
			app.Selection.Text = "]";

			range.Start = range.End + 1;
			range.End = range.Start;
			
			//trying to give document focus; have not found solution
			app.Selection.Select();
			range.Select();
		}

		private readonly SCTree _scTree;

		private void button3_Click(object sender, EventArgs e)
		{
			Object address = uxRelatedArticleURL.Text;
			if (address.ToString() == "")
			{
				MessageBox.Show(@"Please enter a URL.");
				return;
			}
			var app = Globals.SitecoreAddin.Application;
			Object selection = app.Selection.Range; //selected text or cursor position if nothing selected
			app.ActiveDocument.Hyperlinks.Add(selection, ref address);
		}
	}
}
