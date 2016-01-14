using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Sitecore.Data.Items;

namespace Informa.Web.Controllers
{
	public static class SitecoreUtil
	{
		static string WebDb = "web";

		//TODO: Business logic for Article Number Generation
		/// <summary>
		/// This method Generates the Article Number
		/// </summary>
		/// <param name="article"></param>
		/// <param name="publication"></param>
		/// <param name="articleDate"></param>
		/// <returns></returns>
		public static string GetNextArticleNumber(string article, Guid publication, DateTime articleDate)
		{
			string yymmdd = $"{articleDate:yyMMdd}";
			string prefix = GetPublicationPrefix(publication) + yymmdd;
			string number = article + prefix;
			return number;
		}

		/// <summary>
		/// This method gets the Publication Prefix which is used in Article Number Generation.
		/// </summary>
		/// <param name="publicationGuid"></param>
		/// <returns></returns>
		public static string GetPublicationPrefix(Guid publicationGuid)
		{
			var publicationPrefixDictionary =
				new Dictionary<Guid, string>
					{
						{new Guid("{110D559F-DEA5-42EA-9C1C-8A5DF7E70EF9}"), "SC"},
					};

			string value;
			return publicationPrefixDictionary.TryGetValue(publicationGuid, out value) ? value : null;
		}


		public static WordPluginModel.ArticleStruct GetArticleStruct(IArticle articleItem)
		{
			var articleStruct = new WordPluginModel.ArticleStruct
			{
				ArticleGuid = articleItem._Id,
				Title = articleItem.Title,
				WebPublicationDate = articleItem.Planned_Publish_Date,
				ArticleNumber = articleItem.Article_Number
			};

			var authors = articleItem.Authors.Select(r => ((IStaff_Item)r)).ToList();
			articleStruct.Authors =
				authors.Select(
					r => new WordPluginModel.StaffStruct()
					{
						ID = r._Id,
						Name = r.Last_Name + ", " + r.First_Name,
					}).ToList();

			articleStruct.Taxonomoy = articleItem.Taxonomies.Select(r => new WordPluginModel.TaxonomyStruct() { Name = r._Name, ID = r._Id }).ToList();
			
			var wordDocURL = articleItem.Word_Document.Url;
			wordDocURL = wordDocURL.Replace("-", " ");
			var wordDoc = Sitecore.Context.Database.GetItem(wordDocURL);

			if (wordDoc != null)
			{
				articleStruct.WordDocVersionNumber = wordDoc.Version.Number;
				articleStruct.WordDocLastUpdateDate = wordDoc.Statistics.Updated.ToString();
				articleStruct.WordDocLastUpdatedBy = wordDoc.Statistics.UpdatedBy;
			}

			try
			{
				ISitecoreService service = new SitecoreContext(WebDb);
				var webItem = service.GetItem<Item>(articleItem._Id);
				articleStruct.IsPublished = webItem != null;
			}
			catch
			{
				articleStruct.IsPublished = false;
			}

			articleStruct.Embargoed = articleItem.Embargoed;


			return articleStruct;
		}
		public static WordPluginModel.ArticleStruct GetArticleStructTest(IArticle articleItem)
		{
			var articleStruct = new WordPluginModel.ArticleStruct
			{
				ArticleGuid = articleItem._Id,
				Title = articleItem.Title,
				WebPublicationDate = articleItem.Planned_Publish_Date,
				ArticleNumber = articleItem.Article_Number
			};
			/*
			HomePageItem home = ItemReference.Home.InnerItem;
			
			articleStruct.IsFeaturedArticle = IsNominatedForHomepage();
			articleStruct.HasBeenNominated = HasBeenNominated.Checked;
			articleStruct.IsTopStory = this.TopStory.Checked;			

			articleStruct.ArticleCategory = this.PrintCategory.Item != null ? this.PrintCategory.Item.ID.ToGuid() : Guid.Empty;
			articleStruct.WebCategory = WebCategory.Item != null ? WebCategory.Item.ID.ToGuid() : Guid.Empty;

			articleStruct.Geography = this.Geography.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { Name = r.DisplayName, ID = r.ID.ToGuid() }).ToList();
			articleStruct.Industries = this.Industries.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { Name = r.DisplayName, ID = r.ID.ToGuid() }).ToList();
			articleStruct.Subjects = this.Subjects.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { Name = r.DisplayName, ID = r.ID.ToGuid() }).ToList();
			articleStruct.TherapeuticCategories = this.TherapeuticCategories.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { Name = r.DisplayName, ID = r.ID.ToGuid() }).ToList();
			articleStruct.MarketSegments = this.MarketSegments.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { Name = r.DisplayName, ID = r.ID.ToGuid() }).ToList();
			articleStruct.GeneralTags = this.Tags.ListItems.Select(r => new SitecoreUtil.TaxonomyStruct() { ID = r.ID.ToGuid() }).ToList();

			articleStruct.NotesToEditorial = this.NotestoEditorial.Raw;
			articleStruct.NotesToProduction = this.NotestoProduction.Raw;



			List<StaffMemberItem> editors = this.Editors.ListItems.Select(r => ((StaffMemberItem)r)).ToList();
			articleStruct.Editors =
				editors.Select(
					r => new SitecoreUtil.StaffStruct()
					{
						ID = r.ID.ToGuid(),
						Name = r.LastName.Text + ", " + r.FirstName.Text,
						Publications = r.Publications.ListItems.Select(p => p.ID.ToGuid()).ToArray()
					}).ToList();


			//TODO: confirm: volume/issue only relevant for weekly/monthly pubs)
			if (BaseTemplateReference.IsValidTemplate(InnerItem.Parent, IssueItem.TemplateId))
			{
				articleStruct.Issue = this.InnerItem.Parent.ID.ToGuid();
				articleStruct.Volume =
					this.InnerItem.Axes.GetAncestors().Where(a => BaseTemplateReference.IsValidTemplate(a, VolumeItem.TemplateId)).Single().ID.ToGuid();
			}

			//TODO: add dates back in (check with Joel/Paul/Charlie/Adam to see where dates go on items)
			//article.PrintPublicationDate = DateTime.Parse(this.PrintPublicationDate.Text);
			//article.WebPublicationDate = DateTime.Parse(this.WebPublicationDate.Text);

			Item articleParent = this.InnerItem.Parent;

			if (IsPublished())
			{
				articleStruct.WebPublicationDate = this.InnerItem.Publishing.PublishDate;
			}
			else if (BaseTemplateReference.IsValidTemplate(articleParent, IssueItem.TemplateId))
			{
				articleStruct.WebPublicationDate = this.ArticleDate.DateTime; //non-daily's webpubdate is ArticleDate field
				articleStruct.PrintPublicationDate = ((IssueItem)articleParent).IssueDate.DateTime;
				//printpubdate is the article's issue's date
			}
			else if (BaseTemplateReference.IsValidTemplate(articleParent, DayFolderItem.TemplateId))
			{
				articleStruct.WebPublicationDate = ((DayFolderItem)articleParent).Date;
				articleStruct.WebPublicationDate = articleStruct.WebPublicationDate.Add(this.ArticleDate.DateTime.TimeOfDay);
				//daily's webpubdate is determined by its ancestors

				articleStruct.PrintPublicationDate = DateTime.MinValue;
				//TODO: better way to signify daily article does not have a printpubdate?
			}

			articleStruct.ReferencedArticlesInfo =
				this.RelatedInlineArticles.ListItems.Select(a => ((ArticleItem)a).GetPreviewInfo()).ToList();

			articleStruct.RelatedArticlesInfo =
				this.RelatedArticles.ListItems.Select(a => ((ArticleItem)a).GetPreviewInfo()).ToList();

			articleStruct.Publication =
				this.InnerItem.Axes.GetAncestors().Where(a => BaseTemplateReference.IsValidTemplate(a, PublicationItem.TemplateId)).Single().ID.ToGuid();

			// In order to read the available commands for a given workflow state, we need to be in a secured environment.
			try
			{
				articleStruct.WorkflowState = _workflowController.GetWorkflowState(this.ID.ToGuid());
			}
			catch (Exception ex)
			{
				_logger.Error("Could not fetch workflow state for article: [" + this.ID.ToString() + "]", ex);
			}

			var wordDocURL = this.WordDocument.Url;
			wordDocURL = wordDocURL.Replace("-", " ");
			var wordDoc = Sitecore.Context.Database.GetItem(wordDocURL);

			if (wordDoc != null)
			{
				articleStruct.WordDocVersionNumber = wordDoc.Version.Number;
				articleStruct.WordDocLastUpdateDate = wordDoc.Statistics.Updated.ToString();
				articleStruct.WordDocLastUpdatedBy = wordDoc.Statistics.UpdatedBy;
			}

			articleStruct.ArticleSize = this.ArticleSize.Item != null ? this.ArticleSize.Item.ID.ToGuid() : Guid.Empty;

			try
			{
				var webItem = SitecoreDatabases.Live.GetItem(this.ID);
				articleStruct.IsPublished = webItem != null;
			}
			catch
			{
				articleStruct.IsPublished = false;
			}

			articleStruct.Embargoed = this.Embargoed.Checked;
			*/

			return articleStruct ;
		}


	}
}