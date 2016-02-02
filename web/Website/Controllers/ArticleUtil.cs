using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Web;
using Informa.Library.Article.Search;
using Sitecore.ContentSearch;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Mvc.Controllers;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class ArticleController : ApiController
    {
        protected readonly IArticleSearch ArticleSearcher;
        protected readonly ISitecoreContext SitecoreContext;
        
        public ArticleController(IArticleSearch searcher, ISitecoreContext context)
        {
            ArticleSearcher = searcher;
            SitecoreContext = context;
        }

        public void Get(int year, int month, int day, string articleNumber)
        {
            //find the new article page
            IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
            filter.PageSize = 1;
            filter.Page = 1;
            filter.ArticleNumber = $"SC{articleNumber}";

            var results = ArticleSearcher.Search(filter);
            if (!results.Articles.Any())
                return;

            //redirect 
            IArticle a = results.Articles.First();
            HttpContext.Current.Response.Redirect(a._Url);
        }
    }

    public class ArticleUtil
	{

		static string WebDb = "web";
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		protected readonly string _tempFolderFallover = System.IO.Path.GetTempPath();
		protected string _tempFileLocation;

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="sitecoreFactory"></param>
		public ArticleUtil(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
		}

		/// <summary>
		/// Returns the Article which has the corresonding Article Number. Return Type is Item
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <returns></returns>
		public Item GetArticleItemByNumber(string articleNumber)
		{
			
			IArticle articleItem = GetArticleByNumber(articleNumber);
			var article = _sitecoreMasterService.GetItem<Item>(articleItem._Id);
			return article;
		}

		/// <summary>
		/// Returns the Article which has the corresonding Article Number. Return Type is IArticle
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <returns></returns>
		public IArticle GetArticleByNumber(string articleNumber)
		{
			var articleFolder = _sitecoreMasterService.GetItem<IArticle_Folder>("{9191621D-2FE9-47A9-B1DA-DA89F17796C6}");

			IArticle article = articleFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Year
				.SelectMany(y => y._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Month
				.SelectMany(z => z._ChildrenWithInferType.OfType<IArticle_Date_Folder>())) //Day
				.SelectMany(dayItem => dayItem._ChildrenWithInferType.OfType<IArticle>())
				.FirstOrDefault(a => a.Article_Number == articleNumber);
			if (article == null)
				return null;
			return _sitecoreMasterService.GetItem<ArticleItem>(article._Id);
			
		}

		/// <summary>
		/// Returns the Version Number of Article
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public int GetWordVersionNumber(IArticle article)
		{
			if (article.Word_Document == null) return -1;
			var wordDocURL = article.Word_Document.Url;
			wordDocURL = wordDocURL.Replace("-", " ");
			var wordDoc = _sitecoreMasterService.GetItem<Item>(wordDocURL);
			return wordDoc?.Version.Number ?? -1;
		}

		public WordPluginModel.ArticlePreviewInfo GetPreviewInfo(IArticle article)
		{
			return new WordPluginModel.ArticlePreviewInfo
			{
				Title = article.Title,
				Publication = _sitecoreMasterService.GetItem<IGlassBase>(article.Publication)._Name,
				//Authors = article.Authors.Select(r => ((IStaff_Item)r).GetFullName()).ToList(), TODO
				Authors = article.Authors.Select(r => (((IAuthor)r).Last_Name + "," + ((IAuthor)r).First_Name)).ToList(),
				ArticleNumber = article.Article_Number,
				//Date = GetProperDate(), TODO
				PreviewUrl = "http://" + WebUtil.GetHostName() + "/?sc_itemid={" + article._Id + "}&sc_mode=preview&sc_lang=en",
				Guid = article._Id
			};
		}

		public string PreviewArticleURL(string articleNumber, string siteHost)
		{
			Guid guid = GetArticleByNumber(articleNumber)._Id;
			if (guid.Equals(Guid.Empty))
			{
				return null;
			}

			return "http://" + siteHost + "/?sc_itemid={" + guid + "}&sc_mode=preview&sc_lang=en";
		}

		/// <summary>
		/// Locks with Article with a Default user
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public bool LockArticle(Item article)
		{
			if (article.Locking.IsLocked())
			{
				throw new ApplicationException("Trying to lock an already locked article!");
			}
			//TODO - Might need to work on the user and login
			/*
			if (userID.IsNullOrEmpty())
			{
				return false;
			}
			bool loggedIn = Sitecore.Security.Authentication.AuthenticationManager.Login(userID);
			if (!loggedIn)
			{
				{ return false; }
			}
			*/

			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				using (new EditContext(article))
				{
					article.Locking.Lock();
				}
			}
			Sitecore.Security.Authentication.AuthenticationManager.Logout();
			return true;
		}

		/// <summary>
		/// Unlocks the Article
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public bool UnlockArticle(Item article)
		{
			if (article == null)
			{
				return false;
			}
			string userID = article.Locking.GetOwner();
			if (string.IsNullOrEmpty(userID)) return false;
			//TODO: assuming domain is specified here.
			bool loggedIn = Sitecore.Security.Authentication.AuthenticationManager.Login(userID);
			if (!loggedIn)
			{
				return false;
			}

			//TODO: use real security
			using (new Sitecore.SecurityModel.SecurityDisabler())
			{
				using (new EditContext(article))
				{
					article.Locking.Unlock();
					//there is already a new version created before saving an article
					//var item = article.Versions.AddVersion();
				}
			}

			Sitecore.Security.Authentication.AuthenticationManager.Logout();

			return true;
		}

		/// <summary>
		/// Returns the CheckedoutStatus of an Article
		/// </summary>
		/// <param name="article"></param>
		/// <returns></returns>
		public WordPluginModel.CheckoutStatus GetLockedStatus(Item article)
		{
			if (article == null)
			{
				var nex = new NullReferenceException("Article item provided was null!");
				throw nex;
			}

			var checkoutStatus = new WordPluginModel.CheckoutStatus();

			ItemLocking itemLocking = article.Locking;
			checkoutStatus.Locked = itemLocking.IsLocked();
			checkoutStatus.User = itemLocking.GetOwner();

			return checkoutStatus;
		}

		public bool DoesArticleHaveText(IArticle article)
		{
			if (string.IsNullOrEmpty(article.Body))
			{
				return false;
			}

			var x = new XmlDocument();

			//using a try-catch here in case the body text isn't in XML format
			try
			{
				x.LoadXml(article.Body);
				return !string.IsNullOrEmpty(x.InnerText.Trim());
			}
			catch (Exception)
			{
				return !string.IsNullOrEmpty(article.Body.Trim());
			}
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
				PrintPublicationDate = articleItem.Actual_Publish_Date,
				ArticleNumber = articleItem.Article_Number,
				NotesToEditorial = articleItem.Editorial_Notes,
				Embargoed = articleItem.Embargoed
			};

			var authors = articleItem.Authors.Select(r => ((IAuthor)r)).ToList();
			articleStruct.Authors =
				authors.Select(
					r => new WordPluginModel.StaffStruct()
					{
						ID = r._Id,
						Name = r.Last_Name + ", " + r.First_Name,
					}).ToList();

			articleStruct.Taxonomoy = articleItem.Taxonomies.Select(r => new WordPluginModel.TaxonomyStruct() { Name = r._Name, ID = r._Id }).ToList();
			articleStruct.ReferencedArticlesInfo = articleItem.Referenced_Articles.Select(a => GetPreviewInfo(((IArticle)a))).ToList();
			articleStruct.RelatedArticlesInfo = articleItem.Related_Articles.Select(a => GetPreviewInfo(a)).ToList();

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
			//TODO - Get article Publication
			articleStruct.Publication = articleItem.Publication;
			//articleStruct.Publication  = new Guid("{3818C47E-4B75-4305-8F01-AB994150A1B0}");			
			/*
			/sitecore/content//*[@@id='{articleStruct.ArticleGuid}']/ancestor::*[@@templateid='{3B0461BF-9ABC-4AF1-B937-C8D225FC2313}']
			Sitecore.Data.Items.Item[] items =  database.SelectItems("fast:/sitecore/content/Product Catalog/Industrial/Products//*[@@templateid='yourTemplateId']");
			*/

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

			//TODO - Workflow - 
			// In order to read the available commands for a given workflow state, we need to be in a secured environment.
			//try
			//{
			//	articleStruct.WorkflowState = _workflowController.GetWorkflowState(this.ID.ToGuid());
			//}
			
			return articleStruct;
		}		
	}
}