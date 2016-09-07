using System;
using System.Collections.Generic;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.SecurityModel;
using Sitecore.Web;
using Informa.Library.Utilities.References;
using PluginModels;
using Informa.Library.CustomSitecore.Ribbon;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;

namespace Informa.Web.Controllers
{
	[Route]
	public class CreateArticleController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;
		private readonly IArticleSearch _articleSearch;

		public CreateArticleController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil, IArticleSearch search)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
			_articleSearch = search;
		}

		[HttpPost]
		[Authorize]
		public ArticleStruct Post([FromBody] CreateArticleRequest content)
		{
			try
			{
				var publicationDate = DateTime.Parse(content.PublicationDate);
				var parent = _articleUtil.GenerateDailyFolder(content.PublicationID, publicationDate);
				var rinsedName = ItemUtil.ProposeValidItemName(content.Name);
				var articleCreate = _sitecoreMasterService.Create<IArticle, IArticle_Date_Folder>(parent, rinsedName);

				//Hack to start the workflow
				var articleItem = _sitecoreMasterService.GetItem<Item>(articleCreate._Id);
				var savedArticle = _sitecoreMasterService.GetItem<ArticleItem>(articleCreate._Id);
				//var intialWorkflow = _sitecoreMasterService.Database.WorkflowProvider.GetWorkflow("{926E6200-EB76-4AD4-8614-691D002573AC}");
				var intialWorkflow = _sitecoreMasterService.Database.WorkflowProvider.GetWorkflow(savedArticle.Crawl<ISite_Root>().Workflow.ToString());
				intialWorkflow.Start(articleItem);

				var article = _sitecoreMasterService.GetItem<IArticle__Raw>(articleCreate._Id);
				article.Title = content.Name;
				article.Planned_Publish_Date = publicationDate;
				article.Created_Date = DateTime.Now;
				article.Article_Number = SitecoreUtil.GetNextArticleNumber(_articleSearch.GetNextArticleNumber(content.PublicationID), content.PublicationID);
				_sitecoreMasterService.Save(article);
				savedArticle = _sitecoreMasterService.GetItem<ArticleItem>(articleCreate._Id);
				var articleStruct = _articleUtil.GetArticleStruct(savedArticle);
				return articleStruct;
			}
			catch (Exception ex)
			{
				return new ArticleStruct { RemoteErrorMessage = ex.ToString() };
			}
		}


		[HttpGet]
		[Authorize]
		public long Get()
		{
			using (new SecurityDisabler())
			{
				return _articleSearch.GetNextArticleNumber(Guid.NewGuid());
			}
		}
	}

	[Route]
	public class SaveArticleTextByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;

		public SaveArticleTextByGuidController(Func<string, ISitecoreService> sitecoreFactory, SitecoreSaverUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_sitecoreSaverUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public void Post([FromBody] SaveArticleTextByGuid content)
		{
			ArticleItem item = _sitecoreMasterService.GetItem<ArticleItem>(content.ArticleGuid);
			_sitecoreSaverUtil.SaveArticleDetailsAndText(item, content.WordText, content.ArticleData);
		}
	}

	[Route]
	public class SaveArticleTextController : ApiController
	{
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;
		private readonly ArticleUtil _articleUtil;

		public SaveArticleTextController(SitecoreSaverUtil sitecoreSaverUtil, ArticleUtil articleUtil)
		{
			_sitecoreSaverUtil = sitecoreSaverUtil;
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public void Post([FromBody] SaveArticleText content)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(content.ArticleNumber);
			_sitecoreSaverUtil.SaveArticleDetailsAndText(article, content.WordText, content.ArticleData);
		}
	}

	[Route]
	public class SaveArticleDetailsController : ApiController
	{
		private readonly SitecoreSaverUtil _sitecoreSaver;

		public SaveArticleDetailsController(SitecoreSaverUtil sitecoreSaver)
		{
			_sitecoreSaver = sitecoreSaver;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public void Post([FromBody] SaveArticleDetails content)
		{
			_sitecoreSaver.SaveArticleDetails(content.ArticleNumber, content.ArticleData, false);
		}
	}

	[Route]
	public class SaveArticleDetailsByGuidController : ApiController
	{
		private readonly SitecoreSaverUtil _sitecoreSaver;

		public SaveArticleDetailsByGuidController(SitecoreSaverUtil sitecoreSaver)
		{
			_sitecoreSaver = sitecoreSaver;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public void Post([FromBody] SaveArticleDetailsByGuid content)
		{
			_sitecoreSaver.SaveArticleDetails(content.ArticleGuid, content.ArticleData, false, false);
		}
	}

	[Route]
	public class AuthenticateUserController : ApiController
	{
		private readonly SitecoreSaverUtil _sitecoreSaver;

		public AuthenticateUserController(SitecoreSaverUtil sitecoreSaver)
		{
			_sitecoreSaver = sitecoreSaver;
		}

		[HttpPost]
		public UserStatusStruct Post([FromBody] LoginModel content)
		{
			return SitecoreUtil.GetUserStatus(content.Username, content.Password);
		}
	}


	[Route]
	public class GetLockedStatusController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetLockedStatusController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public CheckoutStatus Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.GetLockedStatus(article);
		}
	}


	[Route]
	public class GetLockedStatusByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public GetLockedStatusByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public CheckoutStatus Post([FromBody] Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return _articleUtil.GetLockedStatus(article);
		}
	}

	[Route]
	public class DoesArticleHaveTextController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public DoesArticleHaveTextController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] string articleNumber)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
			return _articleUtil.DoesArticleHaveText(article);
		}
	}

	[Route]
	public class DoesArticleGuidHaveTextController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public DoesArticleGuidHaveTextController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] Guid articleGuid)
		{
			ArticleItem article = _sitecoreMasterService.GetItem<ArticleItem>(articleGuid);
			return _articleUtil.DoesArticleHaveText(article);
		}
	}

	[Route]
	public class DoesArticleExistController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public DoesArticleExistController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return article != null;
		}
	}

	[Route]
	public class DoesArticleGuidExistController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;

		public DoesArticleGuidExistController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return article != null;
		}
	}

	[Route]
	public class GetArticlePreviewInfoByGuidsController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public GetArticlePreviewInfoByGuidsController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public List<ArticlePreviewInfo> Post([FromBody] List<Guid> guids)
		{
			var previews = new List<ArticlePreviewInfo>();
			foreach (Guid guid in guids)
			{
				ArticleItem article = _sitecoreMasterService.GetItem<ArticleItem>(guid);
				if (article != null)
				{
					previews.Add(_articleUtil.GetPreviewInfo(article));
				}
			}
			return previews;
		}
	}

	public class GetArticlePreviewInfoController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetArticlePreviewInfoController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public ArticlePreviewInfo Post([FromBody] string articleNumber)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
			var preview = article != null ? _articleUtil.GetPreviewInfo(article) : new ArticlePreviewInfo();
			return preview;
		}
	}


	[Route]
	public class GetWordVersionNumByNumberController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetWordVersionNumByNumberController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public int Post([FromBody] string articleNumber)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
			if (article == null)
			{
				return -1;
			}
			return _articleUtil.GetWordVersionNumber(article);
		}
	}

	[Route]
	public class GetWordVersionNumByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public GetWordVersionNumByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public int Post([FromBody] Guid articleGuid)
		{
			ArticleItem article = _sitecoreMasterService.GetItem<ArticleItem>(articleGuid);
			if (article == null)
			{
				return -1;
			}
			return _articleUtil.GetWordVersionNumber(article);
		}
	}

	[Route]
	public class CheckOutArticleController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public CheckOutArticleController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.LockArticle(article);
		}
	}

	[Route]
	public class CheckOutArticleByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public CheckOutArticleByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return _articleUtil.LockArticle(article);
		}
	}

	[Route]
	public class CheckInArticleController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public CheckInArticleController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.UnlockArticle(article);
		}
	}

	[Route]
	public class CheckInArticleByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly ArticleUtil _articleUtil;

		public CheckInArticleByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public bool Post([FromBody] Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return _articleUtil.UnlockArticle(article);
		}
	}

	[Route]
	public class SendDocumentToSitecoreByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;

		public SendDocumentToSitecoreByGuidController(Func<string, ISitecoreService> sitecoreFactory,
			SitecoreSaverUtil sitecoreSaverUtil)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_sitecoreSaverUtil = sitecoreSaverUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public int Post([FromBody] SendDocumentToSitecoreByGuid content)
		{
			ArticleItem article = _sitecoreMasterService.GetItem<ArticleItem>(content.ArticlGuid);
			return _sitecoreSaverUtil.SendDocumentToSitecore(article, content.Data, content.Extension);
		}
	}

	[Route]
	public class SendDocumentToSitecoreController : ApiController
	{
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;
		private readonly ArticleUtil _articleUtil;

		public SendDocumentToSitecoreController(SitecoreSaverUtil sitecoreSaverUtil, ArticleUtil articleUtil)
		{
			_sitecoreSaverUtil = sitecoreSaverUtil;
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		[Authorize]
		public int Post([FromBody] SendDocumentToSitecore content)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(content.ArticleNumber);
			return _sitecoreSaverUtil.SendDocumentToSitecore(article, content.Data, content.Extension);
		}
	}

	[Route]
	public class GetArticleGuidByNumController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetArticleGuidByNumController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		public string Post([FromBody] string articleNumber)
		{
			ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
			return article?._Id.ToString() ?? Guid.Empty.ToString();
		}
	}

	[Route]
	public class PreviewUrlArticleController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public PreviewUrlArticleController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		public string Post([FromBody] string articleNumber)
		{
			return _articleUtil.PreviewArticleURL(articleNumber, WebUtil.GetHostName());
		}
	}

	[Route]
	public class GetArticleDynamicUrlController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetArticleDynamicUrlController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		public string Post([FromBody] string articleNumber)
		{
			var options = new LinkUrlOptions();
			string mediaUrl = LinkManager.GetDynamicUrl(_articleUtil.GetArticleItemByNumber(articleNumber), options);
			return mediaUrl;
		}
	}

	/// <summary>
	/// Get the Article URL by its article number.
	/// </summary>
	/// <param name="articlenumber">The unique article number</param>
	/// <returns>The article URL</returns>
	[Route]
	public class GetArticleUrlController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public GetArticleUrlController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		[Authorize]
		public string Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			if (article == null) return null;
			var url = LinkManager.GetItemUrl(article).ToLower();
			return url;
		}
	}

	[Route]
	public class ReExportNlmController : ApiController
	{
		[HttpPost]
		//[Authorize]
		public string Post([FromBody] string articleNumber)
		{
			return new ReExportArticleNlm().ReExport(articleNumber);
		}
	}
}