using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.SecurityModel;
using Sitecore.Workflows;

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
				var article = _sitecoreMasterService.Create<IArticle, IArticle_Date_Folder>(parent, rinsedName);
				article.Title = content.Name;
				article.Planned_Publish_Date = publicationDate;
				article.Created_Date = DateTime.Now;
				_sitecoreMasterService.Save(article);
				article.Article_Number = SitecoreUtil.GetNextArticleNumber(article._Id.ToString(), content.PublicationID, publicationDate);
				_sitecoreMasterService.Save(article);
				return SitecoreUtil.GetArticleStruct(article);
			}
		}

		public int SendDocumentToSitecore(string articleNumber, byte[] data, string extension, string username)
		{
			//IArticle article = _articleUtil.GetArticleByNumber(articleNumber);
			
			var articleFolder = _sitecoreMasterService.GetItem<IArticle_Folder>(new Guid());

			IArticle article = articleFolder._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Year
				.SelectMany(y => y._ChildrenWithInferType.OfType<IArticle_Date_Folder>() //Month
				.SelectMany(z => z._ChildrenWithInferType.OfType<IArticle_Date_Folder>())) //Day
				.SelectMany(dayItem => dayItem._ChildrenWithInferType.OfType<IArticle>())
				.FirstOrDefault(a => a.Article_Number == articleNumber);


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

		public int SendDocumentToSitecore(Guid articleGuid, byte[] data, string extension, string username)
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

	}
}