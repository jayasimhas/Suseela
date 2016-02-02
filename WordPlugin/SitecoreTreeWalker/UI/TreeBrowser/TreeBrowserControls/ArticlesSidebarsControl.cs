using System;
using System.Diagnostics;
using System.Windows.Forms;
using Informa.Web.Areas.Account.Models;
using Microsoft.Office.Interop.Word;
using SitecoreTreeWalker.Util;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;

namespace SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls
{
	public partial class ArticlesSidebarsControl : UserControl
	{
		public ArticlesSidebarsControl()
		{
			InitializeComponent();
		}

		private void InsertRelatedArticle(WordPluginModel.ArticlePreviewInfo article)
		{
			if(string.IsNullOrEmpty(uxRelatedArticleNumber.Text))
			{
				MessageBox.Show(@"Please enter an article number");
				return;
			}
			string relatedArticleNumber = uxRelatedArticleNumber.Text;
			Object address = article.PreviewUrl;

			var app = Globals.SitecoreAddin.Application;

			app.Selection.Text = "([A#" + relatedArticleNumber.Trim() + "])";
			Range hyperlinkRange = app.ActiveDocument.Range(app.Selection.Start + 1, app.Selection.End - 1);
			//Word.Hyperlink hyperlink = 
				app.ActiveDocument.Hyperlinks.Add(hyperlinkRange, ref address, null, InternalLinkTooltip);
			app.Selection.MoveRight(WdUnits.wdCharacter, 2);
		}

		public static bool IsArticleOrSidebarHyperlink(Hyperlink hyperlink)
		{
			return hyperlink != null &&
			       (hyperlink.ScreenTip != null && hyperlink.ScreenTip.Equals(InternalLinkTooltip));
		}

		public const string InternalLinkTooltip = "sitecore";

		private void uxRelatedArticleNumberGo_Click(object sender, EventArgs e)
		{
			RelatedArticleGo();
		}

		private void uxSidebarArticleNumberGo_Click(object sender, EventArgs e)
		{
			SidebarGo();
		}

		private void RelatedArticleGo()
		{
			if (uxInsertIntoArticle.Visible)
			{
				InsertRelatedArticle(uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo);
				SetToRetriveArticleMode();
				uxRelatedArticleNumber.Clear();
			}
			else
			{
				PreviewArticle(uxRelatedArticleNumber.Text);
				TryToSetToInsertArticleMode();
			}
		}

		private void SidebarGo()
		{
			if(uxInsertIntoArticle.Visible)
			{
				InsertSidebarArticle(uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo);
				SetToRetriveArticleMode();
				uxSidebarArticleNumber.Clear();
			}
			else
			{
				PreviewArticle(uxSidebarArticleNumber.Text);
				TryToSetToInsertArticleMode();
			}
		}

		private void InsertSidebarArticle(WordPluginModel.ArticlePreviewInfo article)
		{
			if (string.IsNullOrEmpty(uxSidebarArticleNumber.Text))
			{
				MessageBox.Show(@"Please enter an article number", @"Elsevier");
				return;
			}

			string sidebarArticleNumber = article.ArticleNumber;
			string address = article.PreviewUrl;

			if (string.IsNullOrEmpty(address))
			{
				MessageBox.Show(@"Article number does not exist!", @"Elsevier");
				return;
			}

			var app = Globals.SitecoreAddin.Application;
			app.Selection.TypeParagraph();
			app.Selection.TypeParagraph();
			app.Selection.TypeParagraph();
			Range selection = app.Selection.Previous(WdUnits.wdParagraph).Previous(WdUnits.wdParagraph);
			selection.Text = "[Sidebar#" + sidebarArticleNumber.Trim() + "]";
			selection = app.ActiveDocument.Range(selection.Start, selection.End);
			app.ActiveDocument.Hyperlinks.Add(selection, address, null, InternalLinkTooltip);
			selection.set_Style("9.0 Sidebar");
			selection.Select();
			app.Selection.Next(WdUnits.wdParagraph).Select();
		}

		private void PreviewArticle(string articleNumber)
		{
			if (string.IsNullOrEmpty(articleNumber))
			{
				MessageBox.Show(@"Please enter an article number!", @"Elsevier");
				return;
			}
			WordPluginModel.ArticlePreviewInfo info = SitecoreArticle.DoesArticleExist(articleNumber)
										? SitecoreArticle.GetArticlePreviewInfo(articleNumber)
										: new WordPluginModel.ArticlePreviewInfo();
			uxArticlePreviewTable.UpdatePreview(info);
			uxArticlePreviewTable.Tag = info;
		}

		private void SetToRetriveArticleMode()
		{
			uxArticlePreviewTable.Clear();
			uxArticlePreviewTable.Tag = null;
			uxRetrieveArticle.Visible = true;
			uxInsertIntoArticle.Visible = false;
			uxPreviewArticle.Enabled = false;
		}

		private void TryToSetToInsertArticleMode()
		{
			var info = uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo;
			if(info != null && !string.IsNullOrEmpty(info.ArticleNumber))
			{
				uxRetrieveArticle.Visible = false;
				uxInsertIntoArticle.Visible = true;
				uxPreviewArticle.Enabled = true;
			}
		}

		private void uxRetrieveArticle_Click(object sender, EventArgs e)
		{
			string articleNumber = "";
			if (!string.IsNullOrEmpty(uxSidebarArticleNumber.Text))
			{
				articleNumber = uxSidebarArticleNumber.Text;
				
			}

			if (!string.IsNullOrEmpty(uxRelatedArticleNumber.Text))
			{
				articleNumber = uxRelatedArticleNumber.Text;
				
			}

			PreviewArticle(articleNumber);

			TryToSetToInsertArticleMode();
		}

		private void uxInsertIntoArticle_Click(object sender, EventArgs e)
		{
			if (!string.IsNullOrEmpty(uxSidebarArticleNumber.Text))
			{
				InsertSidebarArticle(uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo);
				uxSidebarArticleNumber.Clear();
			}

			if (!string.IsNullOrEmpty(uxRelatedArticleNumber.Text))
			{
				InsertRelatedArticle(uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo);
				uxRelatedArticleNumber.Clear();
			}
			SetToRetriveArticleMode();
		}

		private void uxPreviewArticle_Click(object sender, EventArgs e)
		{
			var article = uxArticlePreviewTable.Tag as WordPluginModel.ArticlePreviewInfo;
			if (article != null)
				Process.Start(PreviewLinkUpdater.GetPreviewURL(article.PreviewUrl).ToString());
		}

		private void uxRelatedArticleNumber_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				RelatedArticleGo();
				e.SuppressKeyPress = true;
			}
			else
			{
				SetToRetriveArticleMode();
				uxSidebarArticleNumber.Clear();
			}
			
		}

		private void uxSidebarArticleNumber_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
			{
				SidebarGo();
				e.SuppressKeyPress = true;
			}
			else
			{
				SetToRetriveArticleMode();
				uxRelatedArticleNumber.Clear();
			}
			
		}
	}
}
