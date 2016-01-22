//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text.RegularExpressions;
//using System.Web.Http;
//using Glass.Mapper.Sc;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
//using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
//using Informa.Web.Areas.Account.Models;
//using Jabberwocky.Glass.Models;
//using Sitecore.SecurityModel;
//using Sitecore.Web;

//namespace Informa.Web.Controllers
//{
//	[Route]
//	public class SitecoreSaverController : ApiController
//	{
//		private ISitecoreService _sitecoreWebService;
//		private readonly ISitecoreService _sitecoreMasterService;
//		public const string MasterDb = "master";
//		private readonly ArticleUtil _articleUtil;

//		public SitecoreSaverController(ISitecoreService sitecoreSevice, Func<string, ISitecoreService> sitecoreFactory, ArticleUtil articleUtil)
//		{
//			_sitecoreWebService = sitecoreSevice;
//			_sitecoreMasterService = sitecoreFactory(MasterDb);
//			_articleUtil = articleUtil;
//		}
		
//		[HttpPost]
//		public WordPluginModel.ArticleStruct CreateArticle([FromBody] WordPluginModel.CreateArticleRequest content)
//		{
//			using (new SecurityDisabler())
//			{
//				var publicationDate = DateTime.Parse(content.PublicationDate);
//				var parent = _articleUtil.GenerateDailyFolder(content.PublicationID, publicationDate);
//				var rinsedName = Regex.Replace(content.Name, @"<(.|\n)*?>", string.Empty).Trim();
//				var articleCreate = _sitecoreMasterService.Create<IArticle, IArticle_Date_Folder>(parent, rinsedName);
//				var article = _sitecoreMasterService.GetItem<IArticle__Raw>(articleCreate._Id);
//				article.Title = content.Name;
//				article.Planned_Publish_Date = publicationDate;
//				article.Created_Date = DateTime.Now;				
//				article.Article_Number = SitecoreUtil.GetNextArticleNumber(articleCreate._Id.ToString(), content.PublicationID, publicationDate);
//				_sitecoreMasterService.Save(article);
//				var savedArticle = _sitecoreMasterService.GetItem<IArticle>(article._Id);
//				var articleStruct = _articleUtil.GetArticleStruct(savedArticle);
//				return articleStruct;
//			}
//		}
		

//	}

//	[Route]
//	public class SaveArticleDetailsByGuidController : ApiController
//	{
//		private readonly SitecoreSaverUtil _sitecoreSaver;

//		public SaveArticleDetailsByGuidController(SitecoreSaverUtil sitecoreSaver)
//		{
//			_sitecoreSaver = sitecoreSaver;
//		}

//		[HttpPost]
//		public void Post([FromBody] WordPluginModel.SaveArticleDetailsByGuid content)
//		{
//			_sitecoreSaver.SaveArticleDetails(content.ArticleGuid, content.ArticleData, false, false);
//		}
//	}

//	[Route]
//	public class GetArticlePreviewInfoController : ApiController
//	{
//		private readonly ISitecoreService _sitecoreMasterService;
//		public const string MasterDb = "master";
		
//		protected string TempFileLocation;

//		public GetArticlePreviewInfoController(Func<string, ISitecoreService> sitecoreFactory)
//		{
//			_sitecoreMasterService = sitecoreFactory(MasterDb);
//		}

//		[HttpPost]
//		public List<WordPluginModel.ArticlePreviewInfo> Post([FromBody] List<Guid> guids)
//		{
//			var previews = new List<WordPluginModel.ArticlePreviewInfo>();
//			foreach (Guid guid in guids)
//			{
//				IArticle article = _sitecoreMasterService.GetItem<IArticle>(guid);
//				if (article != null)
//				{
//					previews.Add(GetPreviewInfo(article));
//				}
//			}
//			return previews;
//		}

//		public WordPluginModel.ArticlePreviewInfo GetPreviewInfo(IArticle article)
//		{
//			return new WordPluginModel.ArticlePreviewInfo
//			{
//				Title = article.Title,
//				Publication = _sitecoreMasterService.GetItem<IGlassBase>(article.Publication)._Name,
//				//Authors = article.Authors.Select(r => ((IStaff_Item)r).GetFullName()).ToList(), TODO
//				Authors = article.Authors.Select(r => (((IStaff_Item)r).Last_Name + "," + ((IStaff_Item)r).First_Name)).ToList(),
//				ArticleNumber = article.Article_Number,
//				//Date = GetProperDate(), TODO
//				PreviewUrl = "http://" + WebUtil.GetHostName() + "/?sc_itemid={" + article._Id + "}&sc_mode=preview&sc_lang=en",
//				Guid = article._Id
//			};
//		}
//	}


//	/*

//	//[HttpPost]
//	public int GetWordVersionNumByGuid([FromBody] Guid articleGuid)
//	{

//		IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
//		if (article == null)
//		{
//			return -1;
//		}
//		return GetWordVersionNumber(article);
//	}

//	//[HttpPost]
//	public int GetWordVersionNumByNumber([FromBody] string articleNumber)
//	{
//		IArticle article = GetArticleByNumber(articleNumber);
//		if (article == null)
//		{
//			return -1;
//		}
//		return GetWordVersionNumber(article);
//	}


//	//[HttpPost]
//	public WordPluginModel.ArticlePreviewInfo GetArtPreInfo([FromBody] string articleNumber)
//	{
//		IArticle article = GetArticleByNumber(articleNumber);
//		var preview = article != null
//								? GetPreviewInfo(article)
//								: new WordPluginModel.ArticlePreviewInfo();
//		return preview;
//	}




//	//[HttpPost]
//	public string GetArticleGuidByNum([FromBody] string articleNumber)
//	{
//		IArticle article = GetArticleByNumber(articleNumber);
//		return article == null ? Guid.Empty.ToString() : article._Id.ToString();
//	}

//	/// <summary>
//	/// Get the Article URL by its article number.
//	/// </summary>
//	/// <param name="articlenumber">The unique article number</param>
//	/// <returns>The article URL</returns>
//	//[HttpPost]
//	public string GetArticleUrl([FromBody] string articleNumber)
//	{
//		Item article = GetArticleItemByNumber(articleNumber);
//		if (article == null) return null;
//		var url = LinkManager.GetItemUrl(article).ToLower();
//		return url;
//	}

//	//[HttpPost]
//	public string GetArticleDynamicUrl([FromBody] string articlenumber)
//	{
//		var options = new LinkUrlOptions();
//		string mediaUrl = LinkManager.GetDynamicUrl(GetArticleItemByNumber(articlenumber), options);
//		return mediaUrl;
//	}

//	//[HttpPost]
//	public string PreviewUrlArticle([FromBody] string articleNumber)
//	{
//		return PreviewArticleURL(articleNumber, WebUtil.GetHostName());
//	}

//	private string PreviewArticleURL(string articleNumber, string siteHost)
//	{
//		Guid guid = this.GetArticleByNumber(articleNumber)._Id;
//		if (guid.Equals(Guid.Empty))
//		{
//			return null;
//		}

//		return "http://" + siteHost + "/?sc_itemid={" +
//			guid + "}&sc_mode=preview&sc_lang=en";
//	}

//	public IArticle GetArticleByNumber(string articleNumber)
//	{
//		var articleFolder = _sitecoreMasterService.GetItem<IArticle_Folder>("{AF934D70-720B-47E8-B393-387A6F774CDF}");

//		IArticle article = articleFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Year
//			.SelectMany(y => y._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Month
//			.SelectMany(z => z._ChildrenWithInferType.OfType<IArticle_Date_Folder>())) //Day
//			.SelectMany(dayItem => dayItem._ChildrenWithInferType.OfType<IArticle>())
//			.FirstOrDefault(a => a.Article_Number == articleNumber);

//		return article;
//	}

//	public Item GetArticleItemByNumber(string articleNumber)
//	{
//		IArticle articleItem = GetArticleByNumber(articleNumber);
//		var article = _sitecoreMasterService.GetItem<Item>(articleItem._Id);
//		return article;
//	}


//	public int SendDocumentToSitecoreByArticleNumber(string articleNumber, byte[] data, string extension, string username)
//	{
//		IArticle article = GetArticleByNumber(articleNumber);
//		return SendDocumentToSitecore(article, data, extension, username);
//	}

//	private IArticle GetArticleFolders(IArticle_Date_Folder folder, string articleNumber)
//	{
//		var article = folder._ChildrenWithInferType.OfType<IArticle>().FirstOrDefault(x => x.Article_Number == articleNumber);

//		if (article != null)
//			return article;

//		var articleFolders = folder._ChildrenWithInferType.OfType<IArticle_Date_Folder>();

//		foreach (var nextFolder in articleFolders)
//		{
//			return GetArticleFolders(nextFolder, articleNumber);
//		}

//		return null;
//		//	.FirstOrDefault(x =>

//		//return folder._ChildrenWithInferType.OfType<IArticle>().FirstOrDefault(x => x.Article_Number == articleNumber) ?? 

//	}

//	public int SendDocumentToSitecoreByGuid(Guid articleGuid, byte[] data, string extension, string username)
//	{
//		IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
//		return SendDocumentToSitecore(article, data, extension, username);
//	}

//	private int SendDocumentToSitecore(IArticle article, byte[] data, string extension, string username)
//	{
//		using (new SecurityDisabler())
//		{
//			MediaItem doc = WordDocToMediaLibrary.SaveWordDocIntoMediaLibrary(article,
//				ConvertBytesToWordDoc(data, article.Article_Number, extension),
//				article._Id.ToString(), extension, username);

//			article.Word_Document.Url = doc.InnerItem.Paths.Path;
//			//article.Word_Document.Type = @"internal";
//			article.Word_Document.TargetId = new Guid(doc.ID.ToString());

//			_sitecoreMasterService.Save(article);

//			return doc.InnerItem.Version.Number;
//		}
//	}

//	protected string ConvertBytesToWordDoc(byte[] data, string articleID, string extension)
//	{
//		var fileName = TempFileLocation + articleID + extension;

//		if (IsFileUsedbyAnotherProcess(fileName))
//		{
//			fileName = TempFileLocation + articleID + "_" + DateTime.Now.ToString("yyyyMMdd-HHmmss") + extension;
//		}

//		FileStream fs = null;
//		MemoryStream ms = null;

//		try
//		{
//			ms = new MemoryStream(data);
//			fs = new FileStream(fileName, FileMode.Create);
//			ms.WriteTo(fs);
//		}
//		catch (Exception ex)
//		{
//			var axe = new ApplicationException("Failed writing out the word document to path [" + fileName + "]!", ex);
//			//_logger.Error("Failed writing out the word document to path [" + fileName + "]!", axe);
//			throw axe;
//		}
//		finally
//		{
//			if (fs != null)
//			{
//				fs.Close();
//				fs.Dispose();
//			}
//			if (ms != null)
//			{
//				ms.Close();
//				ms.Dispose();
//			}

//		}

//		return fileName;
//	}

//	protected bool IsFileUsedbyAnotherProcess(string filename)
//	{
//		var info = new FileInfo(filename);
//		if (!info.Exists)
//		{ return false; }

//		FileStream fs = null;
//		try
//		{
//			fs = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.None);
//		}
//		catch (IOException)
//		{
//			return true;
//		}
//		finally
//		{
//			if (fs != null)
//			{
//				fs.Close();
//				fs.Dispose();
//			}
//		}
//		return false;

//	}
//	*/

//	}