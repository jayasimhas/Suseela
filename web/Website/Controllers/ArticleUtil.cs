using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;

namespace Informa.Web.Controllers
{
	public class ArticleUtil
	{

		static string WebDb = "web";
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		protected readonly string _tempFolderFallover = System.IO.Path.GetTempPath();
		protected string _tempFileLocation;

		public ArticleUtil(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);			

		}

		public Item GetArticleItemByNumber(string articleNumber)
		{
			ISitecoreService sitecoreMasterService = new SitecoreContentContext();
			IArticle articleItem = GetArticleByNumber(articleNumber);
			var article = sitecoreMasterService.GetItem<Item>(articleItem._Id);
			return article;
		}

		public IArticle GetArticleByNumber(string articleNumber)
		{
			ISitecoreService sitecoreMasterService = new SitecoreContentContext();
			var articleFolder = sitecoreMasterService.GetItem<IArticle_Folder>("{AF934D70-720B-47E8-B393-387A6F774CDF}");

			IArticle article = articleFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Year
				.SelectMany(y => y._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Month
				.SelectMany(z => z._ChildrenWithInferType.OfType<IArticle_Date_Folder>())) //Day
				.SelectMany(dayItem => dayItem._ChildrenWithInferType.OfType<IArticle>())
				.FirstOrDefault(a => a.Article_Number == articleNumber);

			return article;
		}

		public int GetWordVersionNumber(IArticle article)
		{
			var wordDocURL = article.Word_Document.Url;
			wordDocURL = wordDocURL.Replace("-", " ");
			var wordDoc = _sitecoreMasterService.GetItem<Item>(wordDocURL);
			return wordDoc?.Version.Number ?? -1;
		}

		/// <summary>
		/// Generates the parent for a article
		/// </summary>	
		public IArticle_Date_Folder GenerateDailyFolder(Guid publicationGuid, DateTime date)
		{
			var publication = _sitecoreMasterService.GetItem<IGlassBase>(publicationGuid);
			string year = date.Year.ToString();
			string month = date.Month.ToString();
			string day = date.Day.ToString();
			IArticle_Folder articlesFolder;
			IArticle_Date_Folder yearFolder;
			IArticle_Date_Folder monthFolder;
			IArticle_Date_Folder dayFolder;

			// Articles Folder
			if (!publication._ChildrenWithInferType.OfType<IArticle_Folder>().Any())
			{
				var article = _sitecoreMasterService.Create<IArticle_Folder, IGlassBase>(publication, "Articles");
				_sitecoreMasterService.Save(article);
				articlesFolder = article;
			}
			else
			{
				articlesFolder = publication._ChildrenWithInferType.OfType<IArticle_Folder>().First();
			}

			// Year
			if (articlesFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().Any(x => x._Name == year))
			{
				yearFolder = articlesFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().First(x => x._Name == year);
			}
			else
			{
				var yearItem = _sitecoreMasterService.Create<IArticle_Date_Folder, IArticle_Folder>(articlesFolder, year);
				_sitecoreMasterService.Save(yearItem);
				yearFolder = yearItem;
			}

			// Month
			if (yearFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().Any(x => x._Name == month))
			{
				monthFolder = yearFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().First(x => x._Name == month);
			}
			else
			{
				var monthItem = _sitecoreMasterService.Create<IArticle_Date_Folder, IArticle_Date_Folder>(yearFolder, month);
				_sitecoreMasterService.Save(monthItem);
				monthFolder = monthItem;
			}

			// Day
			if (monthFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().Any(x => x._Name == day))
			{
				dayFolder = monthFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>().First(x => x._Name == day);
			}
			else
			{
				var dayItem = _sitecoreMasterService.Create<IArticle_Date_Folder, IArticle_Date_Folder>(monthFolder, day);
				_sitecoreMasterService.Save(dayItem);
				dayFolder = dayItem;
			}

			return dayFolder;
		}

		public WordPluginModel.ArticleStruct GetArticleStruct(IArticle articleItem)
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

			if (articleItem.Word_Document != null)
			{
				var wordDocURL = articleItem.Word_Document.Url;
				wordDocURL = wordDocURL.Replace("-", " ");
				var wordDoc = Sitecore.Context.Database.GetItem(wordDocURL);

				if (wordDoc != null)
				{
					articleStruct.WordDocVersionNumber = wordDoc.Version.Number;
					articleStruct.WordDocLastUpdateDate = wordDoc.Statistics.Updated.ToString();
					articleStruct.WordDocLastUpdatedBy = wordDoc.Statistics.UpdatedBy;
				}
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
		public WordPluginModel.ArticleStruct GetArticleStructTest(IArticle articleItem)
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

			return articleStruct;
		}

	}
}