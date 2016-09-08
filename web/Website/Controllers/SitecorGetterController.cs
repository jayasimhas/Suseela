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
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.System.Media.Unversioned;
using Jabberwocky.Glass.Models;
using PluginModels;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Resources.Media;
using Sitecore.Web;
using Sitecore.Data;

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

        public JsonResult<List<TaxonomyStruct>> Get()
        {
            List<TaxonomyStruct> result = new List<TaxonomyStruct>();
            var baseFolder = _sitecoreService.GetItem<IFolder>(new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}"));
            result = baseFolder?._ChildrenWithInferType.OfType<ITaxonomy_Item>()
                .Select(eachChild => new TaxonomyStruct() { Name = eachChild.Item_Name, ID = eachChild._Id }).ToList();
            return Json(result);
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
            var taxoGuid = new Guid("{E8A37C2D-FFE3-42D4-B38E-164584743832}");
            var taxonomyItem = _sitecoreService.GetItem<Item>(taxoGuid);
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
            var matches = children.Select(child => new TaxonomyStruct { ID = child.ID.Guid, Name = child.DisplayName, Section = child.ParentID.Guid.Equals(taxoGuid) ? null : child.ParentID.Guid.ToString() }).ToList();

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
            var baseFolder = _sitecoreService.GetItem<IGlassBase>(new Guid("{0C51AAA3-70D9-408C-9130-852F32BF4BEE}"));
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
            var baseFolder = _sitecoreService.GetItem<IGlassBase>("{18EE6A90-E8AE-4DEA-B475-274567126FC0}");
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
        public JsonResult<List<StaffStruct>> Get()
        {
            var staffFolder = _sitecoreService.GetItem<IFolder>(new Guid("{5C4D8806-C74E-465E-AB61-FC50F168BCBC}"));
            var members = staffFolder?._ChildrenWithInferType.OfType<I___Person>().Where(c => !c.Inactive).OrderBy(o => o.Last_Name)
                .Select(eachChild => new StaffStruct() { Name = eachChild.Last_Name + ", " + eachChild.First_Name, ID = eachChild._Id }).ToList();
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
            var publicationRootChildren = GetChildrenWithIDs("{C00E39E4-3566-4307-93AE-8471769D6B36}");
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
            var publicationRootChildren = GetChildrenWithIDs("{CAAD10A6-51CF-41F4-B6CE-773C8CA94CB9}");
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
        public JsonResult<DirectoryStruct[]> Get()
        {
            var item = _sitecoreService.GetItem<Item>("{11111111-1111-1111-1111-111111111111}");
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

        public JsonResult<HDirectoryStruct> Get(string guid)
        {
            var item = _sitecoreService.GetItem<Item>(guid);
            var children = item.Children.Select(child => GetHierarchy(child.Paths.Path)).ToList();

            var childNode = new HDirectoryStruct() { ChildrenList = children, Name = item.DisplayName, ID = item.ID.ToGuid() };
            childNode.Children = childNode.ChildrenList.ToArray();
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
			public GetServerTimezoneController() { }
			public JsonResult<TimeZoneInfo> Get()
			{
				return Json(TimeZoneInfo.Local);
			}
		}

    public class GetArticleActualPublishedDateController : ApiController
    {
        ArticleUtil _articleUtil;
        public GetArticleActualPublishedDateController(ArticleUtil articleUtil)
        {
            _articleUtil = articleUtil;
        }

        public JsonResult<DateTime> Get(Guid itemID)
        {
            return Json(_articleUtil.GetArticleActualPublishedDate(itemID));
        }
    }

    public class GetArticleWorkflowHistoryController : ApiController
    {
        public JsonResult<List<Tuple<DateTime, string, bool>>> Get(Guid itemID)
        {
            return Json(new Informa.Library.Utilities.SitecoreUtils.WorkflowUtil().GetWorkflowHistory(new ID(itemID)));
        }
    }
}
