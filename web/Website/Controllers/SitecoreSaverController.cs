using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using System.Xml;
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
using Sitecore.Workflows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Results;

namespace Informa.Web.Controllers
{
	[Route]
	public class SitecoreSaverController : ApiController
	{
		private ISitecoreService _sitecoreWebService;
		private readonly ISitecoreService _sitecoreMasterService;
		public const string MasterDb = "master";
		protected readonly string TempFolderFallover = System.IO.Path.GetTempPath();
		protected string TempFileLocation;

		public SitecoreSaverController(ISitecoreService sitecoreSevice, Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreWebService = sitecoreSevice;
			_sitecoreMasterService = sitecoreFactory(MasterDb);
			TempFileLocation = string.IsNullOrEmpty(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) ?
				TempFolderFallover : Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\temp.";
		}


		[HttpPost]
		public WordPluginModel.ArticleStruct CreateArticle([FromBody] WordPluginModel.CreateArticleRequest content)
		{
			using (new SecurityDisabler())
			{
				var publicationDate = DateTime.Parse(content.PublicationDate);
				var parent = GenerateDailyFolder(content.PublicationID, publicationDate);
				var rinsedName = Regex.Replace(content.Name, @"<(.|\n)*?>", string.Empty).Trim();
				var articleCreate = _sitecoreMasterService.Create<IArticle, IArticle_Date_Folder>(parent, rinsedName);
				var article = _sitecoreMasterService.GetItem<IArticle__Raw>(articleCreate._Id);
				article.Title = content.Name;
				article.Planned_Publish_Date = publicationDate;
				article.Created_Date = DateTime.Now;
				//_sitecoreMasterService.Save(article);
				article.Article_Number = SitecoreUtil.GetNextArticleNumber(articleCreate._Id.ToString(), content.PublicationID, publicationDate);
				_sitecoreMasterService.Save(article);
				var savedArticle = _sitecoreMasterService.GetItem<IArticle>(article._Id);
				var articleStruct = SitecoreUtil.GetArticleStruct(savedArticle);
				return articleStruct;
			}
		}
		/*

		//[HttpPost]
		public int GetWordVersionNumByGuid([FromBody] Guid articleGuid)
		{

			IArticle article = _sitecoreMasterService.GetItem<IArticle>(articleGuid);
			if (article == null)
			{
				return -1;
			}
			return GetWordVersionNumber(article);
		}

		//[HttpPost]
		public int GetWordVersionNumByNumber([FromBody] string articleNumber)
		{
			IArticle article = GetArticleByNumber(articleNumber);
			if (article == null)
			{
				return -1;
			}
			return GetWordVersionNumber(article);
		}


		//[HttpPost]
		public WordPluginModel.ArticlePreviewInfo GetArtPreInfo([FromBody] string articleNumber)
		{
			IArticle article = GetArticleByNumber(articleNumber);
			var preview = article != null
									? GetPreviewInfo(article)
									: new WordPluginModel.ArticlePreviewInfo();
			return preview;
		}

		//[HttpPost]
		public List<WordPluginModel.ArticlePreviewInfo> GetArticlePreviewInfo([FromBody] List<Guid> guids)
		{
			var previews = new List<WordPluginModel.ArticlePreviewInfo>();
			foreach (Guid guid in guids)
			{
				IArticle article = _sitecoreMasterService.GetItem<IArticle>(guid);
				if (article != null)
				{
					previews.Add(GetPreviewInfo(article));
				}
			}
			return previews;
		}

		public WordPluginModel.ArticlePreviewInfo GetPreviewInfo(IArticle article)
		{
			return new WordPluginModel.ArticlePreviewInfo
			{
				Title = article.Title,
				Publication = _sitecoreMasterService.GetItem<IGlassBase>(article.Publication)._Name,
				//Authors = article.Authors.Select(r => ((IStaff_Item)r).GetFullName()).ToList(), TODO
				Authors = article.Authors.Select(r => (((IStaff_Item)r).Last_Name + "," + ((IStaff_Item)r).First_Name)).ToList(),
				ArticleNumber = article.Article_Number,
				//Date = GetProperDate(), TODO
				PreviewUrl = "http://" + WebUtil.GetHostName() + "/?sc_itemid={" + article._Id + "}&sc_mode=preview&sc_lang=en",
				Guid = article._Id
			};
		}


		//[HttpPost]
		public string GetArticleGuidByNum([FromBody] string articleNumber)
		{
			IArticle article = GetArticleByNumber(articleNumber);
			return article == null ? Guid.Empty.ToString() : article._Id.ToString();
		}

		/// <summary>
		/// Get the Article URL by its article number.
		/// </summary>
		/// <param name="articlenumber">The unique article number</param>
		/// <returns>The article URL</returns>
		//[HttpPost]
		public string GetArticleUrl([FromBody] string articleNumber)
		{
			Item article = GetArticleItemByNumber(articleNumber);
			if (article == null) return null;
			var url = LinkManager.GetItemUrl(article).ToLower();
			return url;
		}

		//[HttpPost]
		public string GetArticleDynamicUrl([FromBody] string articlenumber)
		{
			var options = new LinkUrlOptions();
			string mediaUrl = LinkManager.GetDynamicUrl(GetArticleItemByNumber(articlenumber), options);
			return mediaUrl;
		}

		//[HttpPost]
		public string PreviewUrlArticle([FromBody] string articleNumber)
		{
			return PreviewArticleURL(articleNumber, WebUtil.GetHostName());
		}

		private string PreviewArticleURL(string articleNumber, string siteHost)
		{
			Guid guid = this.GetArticleByNumber(articleNumber)._Id;
			if (guid.Equals(Guid.Empty))
			{
				return null;
			}

			return "http://" + siteHost + "/?sc_itemid={" +
				guid + "}&sc_mode=preview&sc_lang=en";
		}

		public IArticle GetArticleByNumber(string articleNumber)
		{
			var articleFolder = _sitecoreMasterService.GetItem<IArticle_Folder>("{AF934D70-720B-47E8-B393-387A6F774CDF}");

			IArticle article = articleFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Year
				.SelectMany(y => y._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Month
				.SelectMany(z => z._ChildrenWithInferType.OfType<IArticle_Date_Folder>())) //Day
				.SelectMany(dayItem => dayItem._ChildrenWithInferType.OfType<IArticle>())
				.FirstOrDefault(a => a.Article_Number == articleNumber);

			return article;
		}

		public Item GetArticleItemByNumber(string articleNumber)
		{
			IArticle articleItem = GetArticleByNumber(articleNumber);
			var article = _sitecoreMasterService.GetItem<Item>(articleItem._Id);
			return article;
		}


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

		public int GetWordVersionNumber(IArticle article)
		{
			var wordDocURL = article.Word_Document.Url;
			wordDocURL = wordDocURL.Replace("-", " ");
			var wordDoc = _sitecoreMasterService.GetItem<Item>(wordDocURL);
			return wordDoc?.Version.Number ?? -1;
		}
	}

}