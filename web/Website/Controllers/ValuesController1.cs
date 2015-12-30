using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models;
using Jabberwocky.Glass.Models;
using Newtonsoft.Json;
using Sitecore.Data;
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
			//var fitlerPublicationIds = FilterPublicationItem.GetAll().Select(fp => fp.InnerItem.ID.Guid).ToList();
			//var regularPublications = publicationRootChildren.Where(pub => !fitlerPublicationIds.Contains(pub.Value));
			return Json(publicationRootChildren.Select(c => new WordPluginModel.ItemStruct() { Name = c.Key, ID = c.Value }).ToList());		
		}
		public Dictionary<string, Guid> GetChildrenWithIDs(string pathOrId)
		{			
			Item item = _sitecoreService.GetItem<Item>(pathOrId);
			return item?.Children.ToArray().ToDictionary(child => child.DisplayName, child => child.ID.Guid);
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
			//db.GetItem(path);
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
				Url = "https://" + WebUtil.GetHostName() +
					  MediaManager.GetMediaUrl(item)
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
			var children = new List<WordPluginModel.HDirectoryStruct>();

			foreach (Item child in item.Children.ToArray())
			{
				children.Add(GetHierarchy(child.Paths.Path));
			}
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





}