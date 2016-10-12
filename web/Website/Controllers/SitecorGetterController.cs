﻿using System;
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
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Glass.Models;
using PluginModels;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using Informa.Library.Utilities.CMSHelpers;
using Informa.Library.Utilities;

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

        //modified,21Sep16
        public JsonResult<List<TaxonomyStruct>> Get()
        {
            List<TaxonomyStruct> result = new List<TaxonomyStruct>();
            var baseFolder = _sitecoreService.GetItem<IFolder>(new Guid(ItemIdResolver.GetItemIdByKey("TaxonomyRoot")));
            //Old code commented
            /*
            List<TaxonomyStruct> result = new List<TaxonomyStruct>(); 
            var baseFolder = _sitecoreService.GetItem<IFolder>(new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}"));
            result = baseFolder?._ChildrenWithInferType.OfType<ITaxonomy_Item>()
                .Select(eachChild => new TaxonomyStruct() { Name = eachChild.Item_Name, ID = eachChild._Id }).ToList();*/

            List<TaxonomyStruct> taxonomyGlobal = new List<TaxonomyStruct>();
            //Uncomment Start, use this code only when environment global/Taxonomy require at content root level as well
            /*
            Guid environmentGlobalGuid = (Guid)_sitecoreService.GetItem<IMain_Section>(
                new Guid(Constants.ContentRootNode))?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>()?.FirstOrDefault()?._Id;

            var taxonomyItems = _sitecoreService.GetItem<Item>(environmentGlobalGuid)?.Axes.GetDescendants()
                .Where(a => ArticleExtension.IsID(a.Template, ITaxonomy_ItemConstants.TemplateId.ToString()));

            taxonomyGlobal = taxonomyItems.Select(eachChild => new TaxonomyStruct() { Name = eachChild.Name, ID = eachChild.ID.Guid }).ToList();
            */
            //UnComment End

            return Json(taxonomyGlobal);
        }

        // GET api/<controller>/5
        public JsonResult<List<TaxonomyStruct>> Get(Guid id)
        {
            List<TaxonomyStruct> result = new List<TaxonomyStruct>();

            var baseFolder = _sitecoreService.GetItem<IGlassBase>(id);
            result = baseFolder?._ChildrenWithInferType.OfType<ITaxonomy_Item>()
                .Select(eachChild => new TaxonomyStruct() { Name = eachChild.Item_Name, ID = eachChild._Id }).ToList();
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

    [Route]
    public class WorkflowController : ApiController
    {
        private readonly IArticleSearch _search;
        private readonly IArticleSearchFilter _articleSearchFilter;
        private readonly ArticleUtil _articleUtil;
        public WorkflowController(ArticleUtil articleUtil)
        {
            _articleUtil = articleUtil;
        }
        // GET api/<controller>



        [Route]
        public JsonResult<ArticleWorkflowState> Get(Guid articleGuid)
        {
            var workFlowState = _articleUtil.GetWorkFlowState(articleGuid);
            return Json(workFlowState);
        }

        public JsonResult<ArticleWorkflowState> Get(string articleNumber)
        {
            var article = _articleUtil.GetArticleByNumber(articleNumber);
            if (article != null)
            {
                var workflowState = _articleUtil.GetWorkFlowState(article._Id);
                return Json(workflowState);
            }
            return Json(new ArticleWorkflowState());
        }
    }

    [Route]
    public class GetUserInfoController : ApiController
    {
        // GET api/<controller>
        public JsonResult<List<string>> Get(string username)
        {
            List<string> lockUserInformation = new List<string>();
            try
            {
                Sitecore.Security.Accounts.User editor = Sitecore.Security.Accounts.User.FromName(username, false);
                if (editor != null && !string.IsNullOrEmpty(editor.Profile.FullName))
                {
                    lockUserInformation.Add(editor.Profile.FullName);
                    lockUserInformation.Add(editor.Profile.Email);
                }
            }
            catch (Exception ex)
            {
                // ignored
            }
            return Json(lockUserInformation);
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
        public JsonResult<MediaItemStruct> Get()
        {
            return Json(new MediaItemStruct());
        }

        public JsonResult<List<TaxonomyStruct>> Get(string searchTerm)
        {
            var taxonomyItem = _sitecoreService.GetItem<Item>(new Guid(ItemIdResolver.GetItemIdByKey("TaxonomyRoot")));

            var matches = PopulateTaxonomyStruct(searchTerm, taxonomyItem);
            /*if (taxonomyItem == null)
            {
                return null;
            }
            List<Item> children;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                children = taxonomyItem.Axes.GetDescendants().Where(i => TaxonomyMatch(i.Name, searchTerm)).ToList();
            }
            else
            {
                children = taxonomyItem.Axes.GetDescendants().ToList();
            }
            var matches = children.Select(child => new TaxonomyStruct { ID = child.ID.Guid, Name = child.DisplayName }).ToList();
            */
            return Json(matches);

        }

        public bool TaxonomyMatch(string itemName, string term)
        {
            string lcName = itemName.ToLower();
            string lcTerm = term.ToLower();
            return lcName.Contains(lcTerm);
        }

        //modified,21Sep16
        public JsonResult<List<TaxonomyStruct>> Get(string searchTerm, Guid verticalTaxonomyGuid)
        {
            List<TaxonomyStruct> matches = null;
            if (verticalTaxonomyGuid != default(Guid))
            {
                Guid mainSectionGuid = new Guid(SitecoreSettingResolver.Instance.ContentRootGuid);
                var globalTaxonomyFolderGuid = (Guid)_sitecoreService.GetItem<IMain_Section>(mainSectionGuid)
                        ?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault()
                        ?._ChildrenWithInferType.OfType<ITaxonomy_Folder>().FirstOrDefault()?._Id;

                matches = PopulateTaxonomyStruct(searchTerm, _sitecoreService.GetItem<Item>(globalTaxonomyFolderGuid));
                matches.AddRange(PopulateTaxonomyStruct(searchTerm, _sitecoreService.GetItem<Item>(verticalTaxonomyGuid)));
            }
            return Json(matches);
        }

        private List<TaxonomyStruct> PopulateTaxonomyStruct(string searchTerm, Item taxonomyItem)
        {
            if (taxonomyItem == null)
            {
                return null;
            }
            List<Item> children;
            if (!string.IsNullOrEmpty(searchTerm))
            {
                children = taxonomyItem.Axes.GetDescendants().Where(i => TaxonomyMatch(i.Name, searchTerm)).ToList();
            }
            else
            {
                children = taxonomyItem.Axes.GetDescendants().ToList();
            }
            return children.Select(child => new TaxonomyStruct { ID = child.ID.Guid, Name = child.DisplayName }).ToList();
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
            var baseFolder = _sitecoreService.GetItem<IGlassBase>(ItemIdResolver.GetItemIdByKey("MediaLibraryFolder"));
            if (baseFolder != null)
            {
                result.Add(baseFolder._Name);
                result.Add(baseFolder._Path);
            }
            return Json(result.ToArray());
        }
    }

    [Route]
    public class GetContactEmailController : ApiController
    {
        private ISitecoreService _sitecoreService;
        public GetContactEmailController(Func<string, ISitecoreService> sitecoreFactory)
        {
            _sitecoreService = sitecoreFactory(Constants.MasterDb);
        }
        // GET api/<controller>
        public JsonResult<string> Get()
        {
            var siteConfigItem = _sitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode);
            if (siteConfigItem == null) return Json(string.Empty);
            var supportingEmailFieldValue = siteConfigItem.Contact_Email;
            if (string.IsNullOrEmpty(supportingEmailFieldValue)) return Json(string.Empty);
            return Json(supportingEmailFieldValue);
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
        public JsonResult<List<WordStyleStruct>> Get()
        {
            var baseFolder = _sitecoreService.GetItem<IGlassBase>(new Guid(ItemIdResolver.GetItemIdByKey("ParagraphStylesFolder")));
            var styleItems = baseFolder?._ChildrenWithInferType.OfType<IMS_Paragraph_Style>()
                .Select(eachChild => new WordStyleStruct() { CssClass = eachChild.CSS_Class, CssElement = eachChild.CSS_Element, WordStyle = eachChild.Title }).ToList();
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
        public JsonResult<List<WordStyleStruct>> Get()
        {
            var baseFolder = _sitecoreService.GetItem<IGlassBase>(ItemIdResolver.GetItemIdByKey("CharacterStylesFolder"));
            var styleItems = baseFolder?._ChildrenWithInferType.OfType<IMS_Character_Style>()
                .Select(eachChild => new WordStyleStruct() { CssClass = eachChild.CSS_Class, CssElement = eachChild.CSS_Element, WordStyle = eachChild.Title }).ToList();
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
        public JsonResult<List<StaffStruct>> Get(Guid veticalGuid = default(Guid))
        {
           IStaff_Root staffRootFolder = null;

            if (veticalGuid != default(Guid))//WP Vertical Specific Change
            {
                staffRootFolder = _sitecoreService.GetItem<IVertical_Root>(veticalGuid)
                    ._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault()?
                    ._ChildrenWithInferType.OfType<IStaff_Root>().FirstOrDefault();
            }
            else
            {
                //Default, if some external system need this other than WP, so thats why name is hard coded
                staffRootFolder = _sitecoreService.GetItem<IStaff_Root>(new Guid(SitecoreSettingResolver.Instance.ItemSetting["StaffParentFolder.pharma"]));
            }

            var members = staffRootFolder?._ChildrenWithInferType.OfType<IStaff_Item>().Where(c => !c.Inactive).OrderBy(o => o.Last_Name)
                .Select(eachChild => new StaffStruct() { Name = eachChild.Last_Name + ", " + eachChild.First_Name, ID = eachChild._Id }).ToList();
            return Json(members);
        }
    }

    [Route]
    public class GetVerticalsController : ApiController
    {
        private ISitecoreService _sitecoreService;
        public GetVerticalsController(Func<string, ISitecoreService> sitecoreFactory)
        {
            _sitecoreService = sitecoreFactory(Constants.MasterDb);
            
        }
        public JsonResult<List<VerticalStruct>> Get()
        {
            Guid mainSectionGuid = new Guid(SitecoreSettingResolver.Instance.ContentRootGuid);
            var contentFolder = _sitecoreService.GetItem<IMain_Section>(mainSectionGuid);

            var verticalsPublications = contentFolder?._ChildrenWithInferType.OfType<IVertical_Root>().Select(c =>
                         new VerticalStruct
                         {
                             ID = c._Id,
                             Name = String.IsNullOrEmpty(c.Vertical_Name) ? c._Name : c.Vertical_Name,
                             TaxonomyItem = (ItemStruct) c._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault()?
                             ._ChildrenWithInferType.OfType<ITaxonomy_Folder>().Select(
                                 s => new ItemStruct { ID=(Guid)s._Id, Name=s._Name}).FirstOrDefault(),
                             Publications = c._ChildrenWithInferType.OfType<ISite_Root>().Select(e => new ItemStruct
                             {
                                 ID = e._Id,
                                 Name = string.IsNullOrEmpty(e.Publication_Name) ? e._Name : e.Publication_Name,
                             }).ToList()
                         }).ToList();
            return Json(verticalsPublications);
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
        public JsonResult<List<ItemStruct>> Get()
        {
            var contentFolder = _sitecoreService.GetItem<IMain_Section>(new Guid(Constants.ContentRootNode));
            var members = contentFolder?._ChildrenWithInferType.OfType<ISite_Root>().Select(eachChild => new ItemStruct()
            { Name = string.IsNullOrEmpty(eachChild.Publication_Name) ? eachChild._Name : eachChild.Publication_Name, ID = eachChild._Id }).ToList();
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
    public class GetMediaTypesController : ApiController
    {
        private readonly ISitecoreService _sitecoreService;
        public GetMediaTypesController(ISitecoreService service)
        {
            _sitecoreService = service;
        }
        // GET api/<controller>
        public JsonResult<List<ItemStruct>> Get()
        {
            var publicationRootChildren = GetChildrenWithIDs(ItemIdResolver.GetItemIdByKey("MediaTypeIconsFolder"));
            return Json(publicationRootChildren.Select(c => new ItemStruct() { Name = c.Key, ID = c.Value }).ToList());
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
        public JsonResult<List<ItemStruct>> Get()
        {
            var publicationRootChildren = GetChildrenWithIDs(ItemIdResolver.GetItemIdByKey("ContentTypesFolder"));
            return Json(publicationRootChildren.Select(c => new ItemStruct() { Name = c.Key, ID = c.Value }).ToList());
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
            var length = _sitecoreService.GetItem<IText_Node>(ItemIdResolver.GetItemIdByKey("MaxLengthShortSummary"));
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
            var length = _sitecoreService.GetItem<IText_Node>(ItemIdResolver.GetItemIdByKey("MaxLengthLongSummary"));
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
        public JsonResult<DirectoryStruct[]> Get()
        {
            var item = _sitecoreService.GetItem<Item>(ItemIdResolver.GetItemIdByKey("SitecoreRoot"));
            var children = new List<DirectoryStruct>();

            foreach (var child in item.Children.ToArray())
            {
                var dir = new DirectoryStruct
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

                    if (!(ext.Contains("mp3") ||
                          ext.Contains("doc") ||
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

        public JsonResult<DirectoryStruct[]> Get(string path)
        {
            bool picsOnly = path.ToLower().Contains("media library");
            bool docsOnly = path.ToLower().Contains("supporting documents");
            if (docsOnly) picsOnly = false;
            var item = _sitecoreService.GetItem<Item>(path);
            var children = new List<DirectoryStruct>();

            foreach (var child in item.Children.ToArray())
            {
                var dir = new DirectoryStruct
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

                    if (!(ext.Contains("mp3") ||
                          ext.Contains("doc") ||
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
        public JsonResult<MediaItemStruct> Get()
        {
            return Json(new MediaItemStruct());
        }

        public JsonResult<MediaItemStruct> Get(string path)
        {
            Item item = _sitecoreService.GetItem<Item>(path);
            if (item == null) return Json(new MediaItemStruct());

            var media = MediaManager.GetMedia(item);

            var stream = media.GetStream();
            if (stream == null)
            {
                return Json(new MediaItemStruct());
            }
            var data = new byte[stream.Stream.Length];
            stream.Stream.Read(data, 0, (int)stream.Stream.Length);

            var mediaItem = new MediaItemStruct
            {
                Data = data,
                Extension = media.Extension,
                Name = item.DisplayName,
                Path = path,
                Uploader = item.Statistics.UpdatedBy,
                UploadDate = item.Statistics.Created,
                Url = "http://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(item)
            };

            IImage imageItem = _sitecoreService.GetItem<IImage>(path);
            if (imageItem != null)
            {
                mediaItem.altText = imageItem.Alt;
            }

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
        public JsonResult<MediaItemStruct> Get()
        {
            return Json(new MediaItemStruct());
        }

        public JsonResult<MediaItemStruct> Get(string path)
        {
            Item item = _sitecoreService.GetItem<Item>(path);
            if (item == null) return Json(new MediaItemStruct());

            var media = MediaManager.GetMedia(item);
            var mediaItem = new MediaItemStruct
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
        Func<string, ISitecoreService> _serviceFactory { get; }
        public MediaPreviewUrlController(Func<string, ISitecoreService> serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }
        // GET api/<controller>
        public JsonResult<string> Get()
        {
            return Json(string.Empty);
        }

        public JsonResult<string> Get(string path)
        {
            //Read from MasterDb
            using (var serviceScope = _serviceFactory(Constants.MasterDb))
            {
                Item media = serviceScope.GetItem<Item>(path);
                string url = "http://" + WebUtil.GetHostName() + MediaManager.GetMediaUrl(media);
                return Json(url);
            }
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
            IText_Node pass = _sitecoreService.GetItem<IText_Node>(ItemIdResolver.GetItemIdByKey("WordPlugInPassword"));
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
        //Commented,22Sep,Not useful
        // GET api/<controller>
        //public JsonResult<string> Get()
        //{
        //    return Json(string.Empty);
        //}

        //moified,21Sep16
        public JsonResult<HDirectoryStruct> Get(string guid = null)
        {
            Item item = null;
            Item taxonomyFolderItem = null;

            HDirectoryStruct globalNode = null;
            HDirectoryStruct childNode = null;
            List<HDirectoryStruct> children = null;

            HDirectoryStruct childNodeRoot = null;
            List<HDirectoryStruct> childNodeRootChildren = null;
            try
            {
                Guid environmentGlobalGuid = (Guid)_sitecoreService.GetItem<IMain_Section>(
                    new Guid(SitecoreSettingResolver.Instance.ContentRootGuid))
                ?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>()?.FirstOrDefault()?._Id;

                if (environmentGlobalGuid != default(Guid))
                {
                    var taxonomyFolderGuid = (Guid)_sitecoreService.GetItem<Item>(environmentGlobalGuid)?.Axes.GetDescendants()
                        .Where(a => ArticleExtension.IsID(a.Template, ITaxonomy_FolderConstants.TemplateId.ToString())).FirstOrDefault().ID.Guid;

                    if (taxonomyFolderGuid != default(Guid))
                    {
                        taxonomyFolderItem = _sitecoreService.GetItem<Item>(taxonomyFolderGuid);
                        childNodeRootChildren = taxonomyFolderItem.Children.Select(child => GetHierarchy(child.Paths.Path)).ToList();
                        childNodeRoot = new HDirectoryStruct() { ChildrenList = childNodeRootChildren, Name = taxonomyFolderItem.DisplayName, ID = taxonomyFolderItem.ID.ToGuid() };
                    }
                }
            }catch(Exception exp)
            {
                //skip to proceed vertical
            }

            //Specific Vertical
            if (new Guid(guid) != default(Guid))
            {
                item = _sitecoreService.GetItem<Item>(guid);
                children = item.Children.Select(child => GetHierarchy(child.Paths.Path)).ToList();
                childNode = new HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };childNode = new HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
            }

            if (childNodeRoot != null && childNodeRoot.ChildrenList != null)
            {
                if (childNode.ChildrenList != null)
                {
                    childNodeRoot.ChildrenList.AddRange(childNode.ChildrenList);
                }
                childNode = childNodeRoot;
                childNode.Children = childNodeRoot.ChildrenList.ToArray();
            }
            else if(childNode != null && childNode.ChildrenList != null)
             {
                childNode.Children = childNode.ChildrenList.ToArray();
            }

            //childNode.Children = childNode.ChildrenList.ToArray();//Comment this line if the above code (environment global/Taxonomy) is uncommented 
            return Json(childNode);
        }

        public HDirectoryStruct GetHierarchy(string path)
        {
            var item = _sitecoreService.GetItem<Item>(path);
            
            var children = item.Children.Select(child => GetHierarchy(child.Paths.Path)).ToList();
            var childNode = new HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
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
        public JsonResult<ArticleStruct> Get()
        {
            return Json(new ArticleStruct());
        }

        public JsonResult<ArticleStruct> Get(string articleNumber)
        {
            ArticleItem article = _articleUtil.GetArticleByNumber(articleNumber);
            return Json(article == null ? new ArticleStruct() : _articleUtil.GetArticleStruct(article));
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

        public JsonResult<ArticleStruct> Get()
        {
            return Json(new ArticleStruct());
        }

        public JsonResult<ArticleStruct> Get(string articleGuid)
        {
            ArticleItem article = _sitecoreService.GetItem<ArticleItem>(articleGuid);
            return Json(article == null ? new ArticleStruct() : _articleUtil.GetArticleStruct(article));
        }
    }

    [Route]

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

    public class GetServerTimezoneController : ApiController
    {
        public GetServerTimezoneController()
        {
        }

        public JsonResult<TimeZoneInfo> Get()
        {
            return Json(TimeZoneInfo.Local);
        }
    }

}