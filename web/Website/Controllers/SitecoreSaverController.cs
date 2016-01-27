using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.SecurityModel;
using Sitecore.Web;

namespace Informa.Web.Controllers
{
	[Route]
	public class CreateArticleController : ApiController
	{
		private ISitecoreService _sitecoreWebService;
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public CreateArticleController(ISitecoreService sitecoreSevice, Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreWebService = sitecoreSevice;
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public WordPluginModel.ArticleStruct Post([FromBody] WordPluginModel.CreateArticleRequest content)
		{
			using (new SecurityDisabler())
			{
				var publicationDate = DateTime.Parse(content.PublicationDate);
				var parent = _articleUtil.GenerateDailyFolder(content.PublicationID, publicationDate);
				var rinsedName = Regex.Replace(content.Name, @"<(.|\n)*?>", string.Empty).Trim();
				var articleCreate = _sitecoreMasterService.Create<IArticle, IArticle_Date_Folder>(parent, rinsedName);
				var article = _sitecoreMasterService.GetItem<IArticle__Raw>(articleCreate._Id);
				article.Title = content.Name;
				article.Planned_Publish_Date = publicationDate;
				article.Created_Date = DateTime.Now;
				article.Article_Number = SitecoreUtil.GetNextArticleNumber(articleCreate._Id.ToString(), content.PublicationID, publicationDate);
				_sitecoreMasterService.Save(article);
				var savedArticle = _sitecoreMasterService.GetItem<IArticle>(article._Id);
				var articleStruct = _articleUtil.GetArticleStruct(savedArticle);
				return articleStruct;
			}
		}
	}

	[Route]
	public class SaveArticleTextByGuidController : ApiController
	{
		private ISitecoreService _sitecoreWebService;
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;

		public SaveArticleTextByGuidController(ISitecoreService sitecoreSevice, Func<string, ISitecoreService> sitecoreFactory, SitecoreSaverUtil articleUtil)
		{
			_sitecoreWebService = sitecoreSevice;
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_sitecoreSaverUtil = articleUtil;
		}

		[HttpPost]
		public void Post([FromBody] WordPluginModel.SaveArticleTextByGuid content)
		{
			IArticle item = _sitecoreMasterService.GetItem<IArticle>(content.ArticleGuid);
			_sitecoreSaverUtil.SaveArticleDetailsAndText(item, content.WordText, content.ArticleData);
		}
	}

	[Route]
	public class SaveArticleTextController : ApiController
	{
		private ISitecoreService _sitecoreWebService;
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;
		private readonly ArticleUtil _articleUtil;

		public SaveArticleTextController(ISitecoreService sitecoreSevice, Func<string, ISitecoreService> sitecoreFactory, SitecoreSaverUtil sitecoreSaverUtil, ArticleUtil articleUtil)
		{
			_sitecoreWebService = sitecoreSevice;
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_sitecoreSaverUtil = sitecoreSaverUtil;
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public void Post([FromBody] WordPluginModel.SaveArticleText content)
		{
			IArticle article = _articleUtil.GetArticleByNumber(content.ArticleNumber);
			_sitecoreSaverUtil.SaveArticleDetailsAndText(article, content.WordText, content.ArticleData);
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
		public void Post([FromBody] WordPluginModel.SaveArticleDetailsByGuid content)
		{
			_sitecoreSaver.SaveArticleDetails(content.ArticleGuid, content.ArticleData, false, false);
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
		public WordPluginModel.CheckoutStatus Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.GetLockedStatus(article);
		}
	}


	[Route]
	public class GetLockedStatusByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public GetLockedStatusByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public WordPluginModel.CheckoutStatus Post([FromBody]Guid articleGuid)
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
		public bool Post([FromBody] string articleNumber)
		{
			IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
			return _articleUtil.DoesArticleHaveText(article);
		}
	}
	
	[Route]
	public class DoesArticleGuidHaveTextController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public DoesArticleGuidHaveTextController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public bool Post([FromBody]Guid articleGuid)
		{
			IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
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
		public const string MasterDb = "master";

		public DoesArticleGuidExistController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
		}

		[HttpPost]
		public bool Post([FromBody]Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return article != null;
		}
	}

	[Route]
	public class GetArticlePreviewInfoByGuidsController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;
		
		public GetArticlePreviewInfoByGuidsController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public List<WordPluginModel.ArticlePreviewInfo> Post([FromBody] List<Guid> guids)
		{
			var previews = new List<WordPluginModel.ArticlePreviewInfo>();
			foreach (Guid guid in guids)
			{
				IArticle article = _sitecoreMasterService.GetItem<IArticle>(guid);
				if (article != null)
				{
					previews.Add(_articleUtil.GetPreviewInfo(article));
				}
			}
			return previews;
		}
	}

	public class GetArticlePreviewInfoController : ApiController
	{	private readonly ArticleUtil _articleUtil;

		public GetArticlePreviewInfoController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public WordPluginModel.ArticlePreviewInfo Post([FromBody] string articleNumber)
		{
			IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
			var preview = article != null ? _articleUtil.GetPreviewInfo(article) : new WordPluginModel.ArticlePreviewInfo();
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
		public int Post([FromBody] string articleNumber)
		{
			IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
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
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public GetWordVersionNumByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public int Post([FromBody] Guid articleGuid)
		{
			IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
			if (article == null)
			{
				return -1;
			}
			return _articleUtil.GetWordVersionNumber(article);
		}
	}

	public class CheckOutArticleController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public CheckOutArticleController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public bool Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.LockArticle(article);
		}
	}
	public class CheckOutArticleByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public CheckOutArticleByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public bool Post([FromBody] Guid articleGuid)
		{
			Item article = _sitecoreMasterService.GetItem<Item>(articleGuid);
			return _articleUtil.LockArticle(article);
		}
	}

	public class CheckInArticleController : ApiController
	{
		private readonly ArticleUtil _articleUtil;

		public CheckInArticleController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public bool Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			return _articleUtil.UnlockArticle(article);
		}
	}
	public class CheckInArticleByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		private readonly ArticleUtil _articleUtil;

		public CheckInArticleByGuidController(Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_articleUtil = articleUtil;
		}

		[HttpPost]
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
		public const string MasterDb = "master";
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;

		public SendDocumentToSitecoreByGuidController(Func<string, ISitecoreService> sitecoreFactory, SitecoreSaverUtil sitecoreSaverUtil)
		{
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			_sitecoreSaverUtil = sitecoreSaverUtil;
		}

		[HttpPost]
		public int Post([FromBody] WordPluginModel.SendDocumentToSitecoreByGuid content)
		{
			IArticle article = _sitecoreMasterService.GetItem<IArticle>(content.ArticlGuid);
			return _sitecoreSaverUtil.SendDocumentToSitecore(article, content.Data, content.Extension);
		}
	}

	[Route]
	public class SendDocumentToSitecoreController : ApiController
	{
		public const string MasterDb = "master";
		private readonly SitecoreSaverUtil _sitecoreSaverUtil;
		private readonly ArticleUtil _articleUtil;

		public SendDocumentToSitecoreController(SitecoreSaverUtil sitecoreSaverUtil, ArticleUtil articleUtil)
		{
			_sitecoreSaverUtil = sitecoreSaverUtil;
			_articleUtil = articleUtil;
		}

		[HttpPost]
		public int Post([FromBody] WordPluginModel.SendDocumentToSitecore content)
		{
			IArticle article = _articleUtil.GetArticleByNumber(content.ArticleNumber);
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
		public string Post([FromBody] string articleNumber)
		{
			IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
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
		public string Post([FromBody] string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			if (article == null) return null;
			var url = LinkManager.GetItemUrl(article).ToLower();
			return url;
		}
	}

	/*

	public int SendDocumentToSitecoreByArticleNumber(string articleNumber, byte[] data, string extension, string username)
	{
		IArticle article = GetArticleByNumber(articleNumber);
		return SendDocumentToSitecore(article, data, extension, username);
	}

	private IArticle GetArticleFolders(IArticle_Date_Folder folder, string articleNumber)
	{
		var article = folder._ChildrenWithInferType.OfType<IArticle>().FirstOrDefault(x => x.Article_Number == articleNumber);

		if (article != null)
			return article;

		var articleFolders = folder._ChildrenWithInferType.OfType<IArticle_Date_Folder>();

		foreach (var nextFolder in articleFolders)
		{
			return GetArticleFolders(nextFolder, articleNumber);
		}

		return null;
		//	.FirstOrDefault(x =>

		//return folder._ChildrenWithInferType.OfType<IArticle>().FirstOrDefault(x => x.Article_Number == articleNumber) ?? 

	}

	public int SendDocumentToSitecoreByGuid(Guid articleGuid, byte[] data, string extension, string username)
	{
		IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
		return SendDocumentToSitecore(article, data, extension, username);
	}

	private int SendDocumentToSitecore(IArticle article, byte[] data, string extension, string username)
	{
		using (new SecurityDisabler())
		{
			MediaItem doc = WordDocToMediaLibrary.SaveWordDocIntoMediaLibrary(article,
				ConvertBytesToWordDoc(data, article.Article_Number, extension),
				article._Id.ToString(), extension, username);

			article.Word_Document.Url = doc.InnerItem.Paths.Path;
			//article.Word_Document.Type = @"internal";
			article.Word_Document.TargetId = new Guid(doc.ID.ToString());

			_sitecoreMasterService.Save(article);

			return doc.InnerItem.Version.Number;
		}
	}

	protected string ConvertBytesToWordDoc(byte[] data, string articleID, string extension)
	{
		var fileName = TempFileLocation + articleID + extension;

		if (IsFileUsedbyAnotherProcess(fileName))
		{
			fileName = TempFileLocation + articleID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + extension;
		}

		FileStream fs = null;
		MemoryStream ms = null;

		try
		{
			ms = new MemoryStream(data);
			fs = new FileStream(fileName, FileMode.Create);
			ms.WriteTo(fs);
		}
		catch (Exception ex)
		{
			var axe = new ApplicationException("Failed writing out the word document to path [" + fileName + "]!", ex);
			//_logger.Error("Failed writing out the word document to path [" + fileName + "]!", axe);
			throw axe;
		}
		finally
		{
			if (fs != null)
			{
				fs.Close();
				fs.Dispose();
			}
			if (ms != null)
			{
				ms.Close();
				ms.Dispose();
			}

		}

		return fileName;
	}

	protected bool IsFileUsedbyAnotherProcess(string filename)
	{
		var info = new FileInfo(filename);
		if (!info.Exists)
		{ return false; }

		FileStream fs = null;
		try
		{
			fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
		}
		catch (IOException)
		{
			return true;
		}
		finally
		{
			if (fs != null)
			{
				fs.Close();
				fs.Dispose();
			}
		}
		return false;

	}
	*/

}