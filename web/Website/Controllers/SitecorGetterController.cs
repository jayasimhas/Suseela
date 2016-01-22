using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Article_Sizes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Style_Mapping;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Text_Nodes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Web;

namespace Informa.Web.Controllers
{
	[Route]
	public class TaxonomyController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public TaxonomyController(ISitecoreService service)
		{
			_sitecoreService = service;

		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.TaxonomyStruct>> Get()
		{
			List<WordPluginModel.TaxonomyStruct> result = new List<WordPluginModel.TaxonomyStruct>();

			/*
            using (var client = new HttpClient())
            {
                 HttpResponseMessage response = await client.GetAsync($"http://informa.miked.velir.com/api/Taxonomy/{Guid.NewGuid()}");

                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<List<TaxonomyStruct>>();
                }          
            }
			*/

			var baseFolder = _sitecoreService.GetItem<IFolder>(new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}"));
			result = baseFolder?._ChildrenWithInferType.OfType<ITaxonomy_Item>()
				.Select(eachChild => new WordPluginModel.TaxonomyStruct() { Name = eachChild.Item_Name, ID = eachChild._Id }).ToList();
			return Json(result);
		}

		// GET api/<controller>/5
		public JsonResult<List<WordPluginModel.TaxonomyStruct>> Get(Guid id)
		{
			List<WordPluginModel.TaxonomyStruct> result = new List<WordPluginModel.TaxonomyStruct>();

			var baseFolder = _sitecoreService.GetItem<IGlassBase>(id);
			result = baseFolder?._ChildrenWithInferType.OfType<ITaxonomy_Item>()
				.Select(eachChild => new WordPluginModel.TaxonomyStruct() { Name = eachChild.Item_Name, ID = eachChild._Id }).ToList();
			return Json(result);
		}

		// POST api/<controller>
		public void Post([FromBody]Guid value)
		{
		}

		// PUT api/<controller>/5
		public void Put(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		public void Delete(int id)
		{
		}
	}


	public class SearchTaxonomyController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;

		public SearchTaxonomyController(ISitecoreService service)
		{
			_sitecoreService = service;
		}

		// GET api/<controller>
		public JsonResult<WordPluginModel.MediaItemStruct> Get()
		{
			return Json(new WordPluginModel.MediaItemStruct());
		}

		public JsonResult<List<WordPluginModel.TaxonomyStruct>> Get(string searchTerm)
		{
			var taxonomyItem = _sitecoreService.GetItem<IFolder>(new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}"));
			if (taxonomyItem == null)
			{
				return null;
			}
			List<ITaxonomy_Item> children;
			if (!string.IsNullOrEmpty(searchTerm))
			{
				children = taxonomyItem?._ChildrenWithInferType.OfType<ITaxonomy_Item>().Where(i => TaxonomyMatch(i.Item_Name, searchTerm)
					).ToList();
			}
			else
			{
				children = taxonomyItem?._ChildrenWithInferType.OfType<ITaxonomy_Item>().ToList();
			}
			var matches = children.Select(child => new WordPluginModel.TaxonomyStruct { ID = child._Id, Name = child.Item_Name, Section = child._Parent._Name }).ToList();

			return Json(matches);

		}

		public bool TaxonomyMatch(string itemName, string term)
		{
			string lcName = itemName.ToLower();
			string lcTerm = term.ToLower();
			return lcName.Contains(lcTerm);
		}
	}

	[Route]
	public class GraphicsNodeController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GraphicsNodeController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string[]> Get()
		{
			List<string> result = new List<string>();
			var baseFolder = _sitecoreService.GetItem<IGlassBase>("{3D6658D8-A0BF-4E75-B3E2-D050FABCF4E1}");
			if (baseFolder != null)
			{
				result.Add(baseFolder._Name);
				result.Add(baseFolder._Path);
			}
			return Json(result.ToArray());
		}
	}

	[Route]
	public class GetParagraphStylesController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetParagraphStylesController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.WordStyleStruct>> Get()
		{
			var baseFolder = _sitecoreService.GetItem<IGlassBase>(new Guid("{0C51AAA3-70D9-408C-9130-852F32BF4BEE}"));
			var styleItems = baseFolder?._ChildrenWithInferType.OfType<IMS_Paragraph_Style>()
				.Select(eachChild => new WordPluginModel.WordStyleStruct() { CssClass = eachChild.CSS_Class, CssElement = eachChild.CSS_Element, WordStyle = eachChild.Title }).ToList();
			return Json(styleItems);
		}
	}

	[Route]
	public class GetCharacterStylesController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetCharacterStylesController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.WordStyleStruct>> Get()
		{
			var baseFolder = _sitecoreService.GetItem<IGlassBase>("{18EE6A90-E8AE-4DEA-B475-274567126FC0}");
			var styleItems = baseFolder?._ChildrenWithInferType.OfType<IMS_Character_Style>()
				.Select(eachChild => new WordPluginModel.WordStyleStruct() { CssClass = eachChild.CSS_Class, CssElement = eachChild.CSS_Element, WordStyle = eachChild.Title }).ToList();
			return Json(styleItems);
		}
	}

	[Route]
	public class GetAuthorsController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetAuthorsController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.StaffStruct>> Get()
		{
			var staffFolder = _sitecoreService.GetItem<IFolder>(new Guid("{37E1CA4F-1B6F-46E2-85D1-123879EDA20E}"));
			var members = staffFolder?._ChildrenWithInferType.OfType<IStaff_Item>().Where(c => !c.Inactive)
				.Select(eachChild => new WordPluginModel.StaffStruct() { Name = eachChild.Last_Name + ", " + eachChild.First_Name, ID = eachChild._Id }).ToList();
			return Json(members);
		}
	}

	[Route]
	public class GetPublicationsController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetPublicationsController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ItemStruct>> Get()
		{
			var contentFolder = _sitecoreService.GetItem<IMain_Section>(new Guid("{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}"));
			var members = contentFolder?._ChildrenWithInferType.OfType<IHome_Page>().Select(eachChild => new WordPluginModel.ItemStruct()
			{ Name = eachChild._Name, ID = eachChild._Id }).ToList();
			return Json(members);
		}
	}

	[Route]
	public class IsAvailableController : ApiController
	{
		public IsAvailableController() { }
		// GET api/<controller>
		public JsonResult<bool> Get() { return Json(true); }
	}

	[Route]
	public class GetArticleSizeForPublicationController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetArticleSizeForPublicationController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ArticleSize>> Get(Guid publicationID)
		{
			var articleSizesRootNode = Sitecore.Context.Database.GetItem("{F5C84045-7B2B-4725-8D9B-3680BEA0AFCE}");
			if (articleSizesRootNode == null)
			{
				//_logger.Error("Root article size node not found!");
				return null;
			}

			var publicationArticleSizeNodeList = articleSizesRootNode.Children.Where
				(x =>
					(((IArticle_Sizes_Folder)x).Publication != null && ((IArticle_Sizes_Folder)x).Publication == publicationID
					)).ToList();

			if (!publicationArticleSizeNodeList.Any())
			{
				//_logger.Warn("No matching article sizes for this publication: [" + publicationID.ToString() + "]");
				return Json(new List<WordPluginModel.ArticleSize>());
			}

			List<WordPluginModel.ArticleSize> sizes = null;
			try
			{
				var publicationArticleSizeNode = publicationArticleSizeNodeList.Single();

				sizes = publicationArticleSizeNode.Children.ToList().Select
					(x => new WordPluginModel.ArticleSize()
					{
						MaximumWordCount = Int32.Parse(((IArticle_Size)x).Maximum_Word_Count),
						MinimumWordCount = Int32.Parse(((IArticle_Size)x).Minimum_Word_Count),
						Name = x.Name,
						ID = x.ID.ToGuid()
					}).ToList();
			}
			catch (Exception ex)
			{
				//_logger.Error("Error getting the article sizes for this publication: [" + publicationID.ToString() + "]", ex);
			}

			return Json(sizes);
		}
	}

	[Route]
	public class GetMediaTypesController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetMediaTypesController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ItemStruct>> Get()
		{
			var publicationRootChildren = GetChildrenWithIDs("{C00E39E4-3566-4307-93AE-8471769D6B36}");
			return Json(publicationRootChildren.Select(c => new WordPluginModel.ItemStruct() { Name = c.Key, ID = c.Value }).ToList());
		}
		public Dictionary<string, Guid> GetChildrenWithIDs(string pathOrId)
		{
			Item item = _sitecoreService.GetItem<Item>(pathOrId);
			return item?.Children.ToArray().ToDictionary(child => child.DisplayName, child => child.ID.Guid);
		}
	}

	//TODO: This might have bugs, and would need to fix it.
	[Route]
	public class GetWidthHeightOfMediaItemController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetWidthHeightOfMediaItemController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<int[]> Get(string path)
		{
			ImageMedia mediaImage = _sitecoreService.GetItem<ImageMedia>(path);
			var image = mediaImage.GetImage();
			if (image == null)
			{
				return null;
			}
			var dimens = new int[2];
			if (!Int32.TryParse(image.Width.ToString(), out dimens[0]) || !Int32.TryParse(image.Height.ToString(), out dimens[1]))
			{
				return null;
			}
			return Json(dimens);
		}
		public Dictionary<string, Guid> GetChildrenWithIDs(string pathOrId)
		{
			Item item = _sitecoreService.GetItem<Item>(pathOrId);
			return item?.Children.ToArray().ToDictionary(child => child.DisplayName, child => child.ID.Guid);
		}
	}

	[Route]
	//TODO - need to fix this
	public class WordPluginController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public WordPluginController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>

		[HttpGet]
		[ActionName("GetMaxLengthShortSummary")]
		public JsonResult<int> GetMaxLengthShortSummary()
		{
			string maxLengthShortSummary = "1000";
			int length;
			Int32.TryParse(maxLengthShortSummary, out length);
			return Json(length);
		}
		[HttpGet]
		[ActionName("GetMaxLengthLongSummary")]
		public JsonResult<int> GetMaxLengthLongSummary()
		{
			string maxLengthLongSummary = "1500";
			int length;
			Int32.TryParse(maxLengthLongSummary, out length);
			return Json(length);
		}
	}

	[Route]
	public class SupportingDocumentsNodeController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public SupportingDocumentsNodeController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string[]> Get()
		{
			List<string> result = new List<string>();
			var siteConfigItem = _sitecoreService.GetItem<ISite_Config>("{BE2B8891-635F-49C1-8BA9-4D2F6C7C5ACE}");
			if (siteConfigItem == null) return Json(result.ToArray());
			var supportingDocumentFieldValue = siteConfigItem.Supporting_Documents_Folder;
			if (supportingDocumentFieldValue == new Guid()) return Json(result.ToArray());
			var supportingDocumentFolder = _sitecoreService.GetItem<IGlassBase>(supportingDocumentFieldValue);
			if (supportingDocumentFolder == null) return Json(result.ToArray());
			result.Add(supportingDocumentFolder._Name);
			result.Add(supportingDocumentFolder._Path);
			return Json(result.ToArray());
		}
	}

	[Route]
	public class GetChildrenDirectoriesController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetChildrenDirectoriesController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<WordPluginModel.DirectoryStruct[]> Get()
		{
			var item = _sitecoreService.GetItem<Item>("{11111111-1111-1111-1111-111111111111}");
			var children = new List<WordPluginModel.DirectoryStruct>();

			foreach (var child in item.Children.ToArray())
			{
				var dir = new WordPluginModel.DirectoryStruct
				{
					Name = child.Name,
					ChildrenList = new List<string>()
				};
				var nestedChildren = child.Children.ToArray();
				foreach (var name in nestedChildren)
				{
					dir.ChildrenList.Add(name.Name);
				}

				if (nestedChildren.Length == 0)
				{
					var media = MediaManager.GetMedia(child);
					string ext = media.Extension.ToLower();

					if (!(ext.Contains("gif") ||
						  ext.Contains("jpg") ||
						  ext.Contains("png"))) continue;
				}

				if (nestedChildren.Length == 0)
				{
					var media = MediaManager.GetMedia(child);
					string ext = media.Extension.ToLower();

					if (!(ext.Contains("doc") ||
						  ext.Contains("docx") ||
						  ext.Contains("xls") ||
						  ext.Contains("xlsx") ||
						  ext.Contains("ppt") ||
						  ext.Contains("pptx") ||
						  ext.Contains("pdf"))) continue;
				}
				dir.Children = dir.ChildrenList.ToArray();
				children.Add(dir);
			}

			return Json(children.ToArray());
		}

		public JsonResult<WordPluginModel.DirectoryStruct[]> Get(string path)
		{
			bool picsOnly = path.ToLower().Contains("media library");
			bool docsOnly = path.ToLower().Contains("supporting documents");
			if (docsOnly) picsOnly = false;
			var item = _sitecoreService.GetItem<Item>(path);
			var children = new List<WordPluginModel.DirectoryStruct>();

			foreach (var child in item.Children.ToArray())
			{
				var dir = new WordPluginModel.DirectoryStruct
				{
					Name = child.Name,
					ChildrenList = new List<string>()
				};
				var nestedChildren = child.Children.ToArray();
				foreach (var name in nestedChildren)
				{
					dir.ChildrenList.Add(name.Name);
				}

				if (nestedChildren.Length == 0 && picsOnly)
				{
					var media = MediaManager.GetMedia(child);
					string ext = media.Extension.ToLower();

					if (!(ext.Contains("gif") ||
						  ext.Contains("jpg") ||
						  ext.Contains("png"))) continue;
				}

				if (nestedChildren.Length == 0 && docsOnly)
				{
					var media = MediaManager.GetMedia(child);
					string ext = media.Extension.ToLower();

					if (!(ext.Contains("doc") ||
						  ext.Contains("docx") ||
						  ext.Contains("xls") ||
						  ext.Contains("xlsx") ||
						  ext.Contains("ppt") ||
						  ext.Contains("pptx") ||
						  ext.Contains("pdf"))) continue;
				}
				dir.Children = dir.ChildrenList.ToArray();
				children.Add(dir);
			}

			return Json(children.ToArray());
		}

	}
	public class GetMediaLibraryItemController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetMediaLibraryItemController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<WordPluginModel.MediaItemStruct> Get()
		{
			return Json(new WordPluginModel.MediaItemStruct());
		}

		public JsonResult<WordPluginModel.MediaItemStruct> Get(string path)
		{
			Item item = _sitecoreService.GetItem<Item>(path);
			if (item == null) return Json(new WordPluginModel.MediaItemStruct());

			var media = MediaManager.GetMedia(item);

			var stream = media.GetStream();
			if (stream == null)
			{
				return Json(new WordPluginModel.MediaItemStruct());
			}
			var data = new byte[stream.Stream.Length];
			stream.Stream.Read(data, 0, (int)stream.Stream.Length);

			var mediaItem = new WordPluginModel.MediaItemStruct
			{
				Data = data,
				Extension = media.Extension,
				Name = item.DisplayName,
				Path = path,
				Uploader = item.Statistics.CreatedBy,
				UploadDate = item.Statistics.Created,
				Url = "https://" + WebUtil.GetHostName() +
										  MediaManager.GetMediaUrl(item)
			};
			return Json(mediaItem);
		}
	}

	[Route]
	public class GetMediaStatisticsController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetMediaStatisticsController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<WordPluginModel.MediaItemStruct> Get()
		{
			return Json(new WordPluginModel.MediaItemStruct());
		}

		public JsonResult<WordPluginModel.MediaItemStruct> Get(string path)
		{
			Item item = _sitecoreService.GetItem<Item>(path);
			if (item == null) return Json(new WordPluginModel.MediaItemStruct());

			var media = MediaManager.GetMedia(item);
			var mediaItem = new WordPluginModel.MediaItemStruct
			{
				Extension = media.Extension,
				Name = item.DisplayName,
				Path = path,
				Uploader = item.Statistics.CreatedBy,
				UploadDate = item.Statistics.Created,
				Url = "https://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(item)
			};
			return Json(mediaItem);
		}
	}
	public class MediaPreviewUrlController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public MediaPreviewUrlController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string> Get()
		{
			return Json(string.Empty);
		}

		public JsonResult<string> Get(string path)
		{
			Item media = _sitecoreService.GetItem<Item>(path);
			string url = "https://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(media);
			return Json(url);
		}

	}

	public class GetDynamicUrlController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetDynamicUrlController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string> Get()
		{
			return Json(string.Empty);
		}

		public JsonResult<string> Get(string path)
		{
			var options = new LinkUrlOptions();
			var item = _sitecoreService.GetItem<Item>(path);
			if (item == null)
			{
				return null;
			}
			string mediaUrl = LinkManager.GetDynamicUrl(item, options);
			return Json(mediaUrl);
		}

	}

	public class GetDocumentPasswordController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetDocumentPasswordController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string> Get()
		{
			IText_Node pass = _sitecoreService.GetItem<IText_Node>("{990801B9-36A1-499C-91D4-D1562D4B93F4}");
			return Json(pass.Text);			
		}
	}

	[Route]
	public class GetHierarchyByGuidController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetHierarchyByGuidController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<string> Get()
		{
			return Json(string.Empty);
		}

		public JsonResult<WordPluginModel.HDirectoryStruct> Get(string guid)
		{
			var item = _sitecoreService.GetItem<Item>(guid);
			var children = item.Children.ToArray().Select(child => GetHierarchy(child.Paths.Path)).ToList();

			var childNode = new WordPluginModel.HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
			childNode.Children = childNode.ChildrenList.ToArray();
			return Json(childNode);
		}

		public WordPluginModel.HDirectoryStruct GetHierarchy(string path)
		{
			var item = _sitecoreService.GetItem<Item>(path);
			var children = new List<WordPluginModel.HDirectoryStruct>();
			List<Item> searchField = item.Children.ToList();

			foreach (Item child in searchField)
			{
				children.Add(GetHierarchy(child.Paths.Path));
			}
			var childNode = new WordPluginModel.HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
			childNode.Children = childNode.ChildrenList.ToArray();
			return childNode;
		}

	}

	[Route]
	public class GetArticleDetailsBGController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		private readonly ArticleUtil _articleUtil;
		public GetArticleDetailsBGController(ISitecoreService service, ArticleUtil articleUtil)
		{
			_sitecoreService = service;
			_articleUtil = articleUtil;
		}
		// GET api/<controller>
		public JsonResult<WordPluginModel.ArticleStruct> Get()
		{
			return Json(new WordPluginModel.ArticleStruct());
		}

		public JsonResult<WordPluginModel.ArticleStruct> Get(string guid)
		{
			IArticle article = _sitecoreService.GetItem<IArticle>(guid);
			return Json(article == null ? new WordPluginModel.ArticleStruct() : _articleUtil.GetArticleStruct(article));
		}
	}

	[Route]
	//TODO - wrtie a service that will check for duplicate article names
	public class DoesArticleNameAlreadyExistInIssueController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public DoesArticleNameAlreadyExistInIssueController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<bool> Get()
		{
			return Json(false);
		}
	}


	[Route]
	public class GetArticleUrlController : ApiController
	{
		private readonly ArticleUtil _articleUtil;
		public GetArticleUrlController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}
		// GET api/<controller>
		/// <summary>
		/// Get the Article URL by its article number.
		/// </summary>
		/// <returns>The article URL</returns>
		public JsonResult<string> Get(string articleNumber)
		{
			Item article = _articleUtil.GetArticleItemByNumber(articleNumber);
			if (article == null) return null;
			var url = LinkManager.GetItemUrl(article).ToLower();
			return Json(url);
		}		
	}
}