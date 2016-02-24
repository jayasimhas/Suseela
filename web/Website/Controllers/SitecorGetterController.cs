using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.System;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Article_Sizes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Style_Mapping;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global.Text_Nodes;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Informa.Library.Utilities.References;
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
		public TaxonomyController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}

		public JsonResult<List<WordPluginModel.TaxonomyStruct>> Get()
		{
			List<WordPluginModel.TaxonomyStruct> result = new List<WordPluginModel.TaxonomyStruct>();
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
		private ISitecoreService _sitecoreService;
		public SearchTaxonomyController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
		private ISitecoreService _sitecoreService;
		public GraphicsNodeController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
		private ISitecoreService _sitecoreService;
		public GetParagraphStylesController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
		private ISitecoreService _sitecoreService;
		public GetCharacterStylesController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
	public class GetItemGuidByPathController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetItemGuidByPathController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}
		// GET api/<controller>
		public JsonResult<Guid> Get(string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				var item = _sitecoreService.GetItem<Item>(path);
				return Json(item?.ID.Guid ?? new Guid());
			}
			return Json(new Guid());
		}
	}

	[Route]
	public class GetAuthorsController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetAuthorsController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.StaffStruct>> Get()
		{
			var staffFolder = _sitecoreService.GetItem<IFolder>(new Guid("{5C4D8806-C74E-465E-AB61-FC50F168BCBC}"));
			var members = staffFolder?._ChildrenWithInferType.OfType<I___Person>().Where(c => !c.Inactive)
				.Select(eachChild => new WordPluginModel.StaffStruct() { Name = eachChild.Last_Name + ", " + eachChild.First_Name, ID = eachChild._Id }).ToList();
			return Json(members);
		}
	}

	[Route]
	public class GetPublicationsController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetPublicationsController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ItemStruct>> Get()
		{
			var contentFolder = _sitecoreService.GetItem<IMain_Section>(new Guid("{0DE95AE4-41AB-4D01-9EB0-67441B7C2450}"));
			var members = contentFolder?._ChildrenWithInferType.OfType<ISite_Root>().Select(eachChild => new WordPluginModel.ItemStruct()
			{ Name = eachChild.Publication_Name, ID = eachChild._Id }).ToList();
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
		private ISitecoreService _sitecoreService;
		public GetArticleSizeForPublicationController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ArticleSize>> Get(Guid publicationID)
		{
			var articleSizesRootNode = _sitecoreService.GetItem<Item>("{F5C84045-7B2B-4725-8D9B-3680BEA0AFCE}");
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

	[Route]
	public class GetContentTypesController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetContentTypesController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<List<WordPluginModel.ItemStruct>> Get()
		{
			var publicationRootChildren = GetChildrenWithIDs("{CAAD10A6-51CF-41F4-B6CE-773C8CA94CB9}");
			return Json(publicationRootChildren.Select(c => new WordPluginModel.ItemStruct() { Name = c.Key, ID = c.Value }).ToList());
		}
		public Dictionary<string, Guid> GetChildrenWithIDs(string pathOrId)
		{
			Item item = _sitecoreService.GetItem<Item>(pathOrId);
			return item?.Children.ToArray().ToDictionary(child => child.DisplayName, child => child.ID.Guid);
		}
	}

	public class GetNumberController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		private readonly IArticleSearch _search;
		public GetNumberController(ISitecoreService service, IArticleSearch search)
		{
			_sitecoreService = service;
			_search = search;
		}
		// GET api/<controller>
		public JsonResult<long> Get(string publicationGuid)
		{
			return Json(_search.GetNextArticleNumber(new Guid(publicationGuid)));
		}
	}

	//TODO: This might have bugs, and would need to fix it.
	[Route]
	public class GetWidthHeightOfMediaItemController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetWidthHeightOfMediaItemController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
	public class GetMaxLengthShortSummaryController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetMaxLengthShortSummaryController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		// GET api/<controller>
		public JsonResult<int> Get()
		{
			var length = _sitecoreService.GetItem<IText_Node>("{5FF122A8-A7C6-4DAB-B135-9DD6E276EE08}");
			return Json(!string.IsNullOrEmpty(length?.Text) ? Int32.Parse(length.Text) : 1000);
		}
	}

	[Route]
	public class GetMaxLengthLongSummaryController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		public GetMaxLengthLongSummaryController(ISitecoreService service)
		{
			_sitecoreService = service;
		}
		public JsonResult<int> Get()
		{
			var length = _sitecoreService.GetItem<IText_Node>("{6759EBC6-101A-4187-9B3A-BE466C1C5DA0}");
			return Json(!string.IsNullOrEmpty(length?.Text) ? Int32.Parse(length.Text) : 1500);
		}
	}
	
	[Route]
	public class SupportingDocumentsNodeController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public SupportingDocumentsNodeController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}

		public JsonResult<string[]> Get()
		{
			List<string> result = new List<string>();
			var siteConfigItem = _sitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode);
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
		private ISitecoreService _sitecoreService;
		public GetChildrenDirectoriesController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
		private ISitecoreService _sitecoreService;
		public GetMediaLibraryItemController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
				Url = "http://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(item)
			};
			return Json(mediaItem);
		}
	}

	[Route]
	public class GetMediaStatisticsController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetMediaStatisticsController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
				Url = "http://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(item)
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
			string url = "http://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(media);
			return Json(url);
		}

	}

	public class GetDynamicUrlController : ApiController
	{
		private ISitecoreService _sitecoreService;
		public GetDynamicUrlController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
		}
		// GET api/<controller>
		public JsonResult<string> Get()
		{
			return Json(string.Empty);
		}

		public JsonResult<string> Get(string path)
		{
			if (path == null)
			{
				return null;
			}
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
		private ISitecoreService _sitecoreService;
		public GetDocumentPasswordController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
		private ISitecoreService _sitecoreService;
		public GetHierarchyByGuidController(Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
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
			List<Item> searchField = item.Children.ToList();

			var children = searchField.Select(child => GetHierarchy(child.Paths.Path)).ToList();
			var childNode = new WordPluginModel.HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
			childNode.Children = childNode.ChildrenList.ToArray();
			return childNode;
		}

	}

	[Route]
	public class GetArticleDetailsController : ApiController
	{
		private readonly ArticleUtil _articleUtil;
		public GetArticleDetailsController(ArticleUtil articleUtil)
		{
			_articleUtil = articleUtil;
		}
		// GET api/<controller>
		public JsonResult<WordPluginModel.ArticleStruct> Get()
		{
			return Json(new WordPluginModel.ArticleStruct());
		}

		public JsonResult<WordPluginModel.ArticleStruct> Get(string articleNumber)
		{
            ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
			return Json(article == null ? new WordPluginModel.ArticleStruct() : _articleUtil.GetArticleStruct(article));
		}
	}

	[Route]
	public class GetArticleDetailsBgController : ApiController
	{
		private readonly ISitecoreService _sitecoreService;
		private readonly ArticleUtil _articleUtil;
		public GetArticleDetailsBgController(ArticleUtil articleUtil, Func<string, ISitecoreService> sitecoreFactory)
		{
			_sitecoreService = sitecoreFactory(Constants.MasterDb);
			_articleUtil = articleUtil;
		}

		public JsonResult<WordPluginModel.ArticleStruct> Get()
		{
			return Json(new WordPluginModel.ArticleStruct());
		}

		public JsonResult<WordPluginModel.ArticleStruct> Get(string articleGuid)
		{
            ArticleItem article = _sitecoreService.GetItem<ArticleItem>(articleGuid);
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
}