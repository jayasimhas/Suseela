using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.Util.Document;
using PluginModels;
using InformaSitecoreWord.Util;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class RelatedArticlesControl : ArticleDetailsPageUserControl
	{
		private InlineReferenceParser _inlineReferenceParser;
		protected bool _isLive;
		public RelatedArticlesControl()
		{
			InitializeComponent();
			_inlineReferenceParser = new InlineReferenceParser();
		}


		public override bool DoesNotChangeOnSaveMetadata()
		{
			return false;
		}

		public bool UpdatePreviewBox()
		{
			if (string.IsNullOrEmpty(uxArticleNumber.Text))
			{
				MessageBox.Show(@"Please enter an article number!", @"Informa");
				return false;
			}
			ArticlePreviewInfo info = SitecoreClient.DoesArticleExist(uxArticleNumber.Text)
										  ? SitecoreClient.GetArticlePreviewInfo(uxArticleNumber.Text)
										  : new ArticlePreviewInfo();
			bool retrieved = _uxArticlePreviewTable.UpdatePreview(info);
			_uxArticlePreviewTable.Tag = info;
			return retrieved;
		}

		public void AddToRelated()
		{
			var info = _uxArticlePreviewTable.Tag as ArticlePreviewInfo;
			if (info != null)
			{
				uxSelectedLayout.AddToRelated(info);
			}
			uxRetrieveArticle.Visible = true;
			uxAddToRelated.Visible = false;
			_uxArticlePreviewTable.Controls.Clear();
			_uxArticlePreviewTable.Tag = null;
		}

		private void uxRetrieveArticle_Click(object sender, EventArgs e)
		{
			if (UpdatePreviewBox())
			{
				uxRetrieveArticle.Visible = false;
				uxAddToRelated.Visible = true;
			}
		}

		private void uxAddToRelated_Click(object sender, EventArgs e)
		{
			AddToRelated();
		}

		private void uxArticleNumber_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
			{
				if (uxRetrieveArticle.Visible)
				{
					if (UpdatePreviewBox())
					{
						uxRetrieveArticle.Visible = false;
						uxAddToRelated.Visible = true;
					}
				}
				else if(uxAddToRelated.Visible)
				{
					AddToRelated();
				}
				e.SuppressKeyPress = true;
			}

		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			_inlineReferenceParser.ParseDocument(SitecoreAddin.ActiveDocument);
			uxSelectedLayout.MenuItem = MenuItem;
			uxSelectedLayout.UpdateMenuItem();
			if(string.IsNullOrEmpty(articleDetails.ArticleNumber))
			{
				return;
			}
			uxSelectedLayout.SitecoreReferencedArticles = articleDetails.ReferencedArticlesInfo.ToList();
			uxSelectedLayout.SitecoreRelatedArticles = articleDetails.RelatedArticlesInfo.ToList();
			uxSelectedLayout.LocalReferencedArticles = SitecoreClient.GetArticlePreviewInfo(_inlineReferenceParser.InlineReferenceGuids);
			uxSelectedLayout.PopulateTable();

			_isLive = articleDetails.IsPublished;
			uxArticleInformationHeaderLabel.Refresh();
		}

		public IEnumerable<Guid> GetInlineReferences()
		{
			return uxSelectedLayout.GetInlineReferences();
		}

		public IEnumerable<Guid> GetRelatedArticles()
		{
			return uxSelectedLayout.GetRelatedArticles();
		}

		public void ResetFields()
		{
			uxSelectedLayout.LocalRelatedArticles = new List<ArticlePreviewInfo>();
			uxSelectedLayout.SitecoreRelatedArticles = new List<ArticlePreviewInfo>();
			uxSelectedLayout.LocalReferencedArticles = new List<ArticlePreviewInfo>();
			uxSelectedLayout.SitecoreReferencedArticles = new List<ArticlePreviewInfo>();
			uxSelectedLayout.PopulateTable();
			uxArticleNumber.Clear();
			_uxArticlePreviewTable.Clear();
			MenuItem.SetIndicatorIcon(Properties.Resources.redx);
			MenuItem.SetIndicatorNumber("");
		}

		private void uxArticleNumber_TextChanged(object sender, EventArgs e)
		{
			if (uxAddToRelated.Visible)
			{
				_uxArticlePreviewTable.Clear();
				_uxArticlePreviewTable.Tag = null;
				uxRetrieveArticle.Visible = true;
				uxAddToRelated.Visible = false;
			}
		}

		private void uxViewArticle_Click(object sender, EventArgs e)
		{
			var article = _uxArticlePreviewTable.Tag as ArticlePreviewInfo;
			if (article != null)
				//Process.Start(PreviewLinkUpdater.GetPreviewURL(article.PreviewUrl).ToString());
				Process.Start(article.PreviewUrl);
		}

		/// <summary>
		/// Pushes all the Sitecore changes to the UI, updating it
		/// Should happen only on Article Save and Transfer
		/// </summary>
		public void PushSitecoreChanges()
		{
			uxSelectedLayout.SitecoreReferencedArticles = uxSelectedLayout.LocalReferencedArticles;
			uxSelectedLayout.SitecoreRelatedArticles =
				uxSelectedLayout.LocalRelatedArticles.Union(uxSelectedLayout.GetActiveSitecoreRelatedArticles()).ToList();
			uxSelectedLayout.LocalRelatedArticles = new List<ArticlePreviewInfo>();
			uxSelectedLayout.PopulateTable();
		}

		private void RelatedArticlesControl_Load(object sender, EventArgs e)
		{
			//_inlineReferenceParser.ParseDocument(Globals.SitecoreAddin.Application.ActiveDocument);
		}

		private void label1_Paint(object sender, PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}

	}
}
