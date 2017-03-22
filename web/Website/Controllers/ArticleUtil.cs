using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Xml;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;
using Sitecore.Data.Locking;
using Sitecore.Web;
using Informa.Library.Article.Search;
using Informa.Library.Utilities;
using Informa.Models.Informa.Models.sitecore.templates.System.Workflow;
using PluginModels;
using Sitecore;
using Constants = Informa.Library.Utilities.References.Constants;
using IWorkflow = Sitecore.Workflows.IWorkflow;
using Informa.Library.Utilities.Extensions;
using Sitecore.Configuration;
using System.Net.Http;
using Newtonsoft.Json;
using Informa.Library.Rss;

namespace Informa.Web.Controllers
{
    [System.Web.Mvc.Route]
    public class ArticleController : ApiController
    {
        protected readonly IArticleSearch ArticleSearcher;
        protected readonly ISitecoreContext SitecoreContext;

        public ArticleController(IArticleSearch searcher, ISitecoreContext context)
        {
            ArticleSearcher = searcher;
            SitecoreContext = context;
        }

        /// <summary>
        /// redirects all article urls that have an article number but no trailing title
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <param name="prefix"></param>
        public void Get(int articleNumber, string prefix)
        {
            string numFormat = $"{prefix}{articleNumber:D6}";

            //find the new article page
            IArticleSearchFilter filter = ArticleSearcher.CreateFilter();
            filter.PageSize = 1;
            filter.Page = 1;
            filter.ArticleNumbers = numFormat.SingleToList();

            var results = ArticleSearcher.Search(filter);
            if (!results.Articles.Any())
                return;

            string newPath = ArticleSearch.GetArticleCustomPath(results.Articles.First());
            HttpContext.Current.Response.RedirectPermanent(newPath);
        }

        /// <summary>
        /// redirects all article urls starting with /article
        /// </summary>
        /// <param name="path"></param>
        public void Get(string year, string month, string day, string title)
        {
            IArticle a = SitecoreContext.GetCurrentItem<IArticle>();
            if (a == null)
                return;

            string newPath = ArticleSearch.GetArticleCustomPath(a);
            HttpContext.Current.Response.RedirectPermanent(newPath);
        }

    }

    public class ArticleUtil
    {

        static string WebDb = "web";
        private readonly ISitecoreService _sitecoreMasterService;
        protected readonly string _tempFolderFallover = System.IO.Path.GetTempPath();
        protected string _tempFileLocation;
        private readonly IArticleSearch _articleSearcher;
        protected readonly Func<string, ISitecoreService> SitecoreFactory;

        //protected readonly IWorkFlowUtil WorkflowUtil;
        /// <summary>
        /// Constructor
        /// </summary
        /// <param name="searcher"></param>
        /// <param name="sitecoreFactory"></param>
        /// <param name="siteRootContext"></param>
        public ArticleUtil(IArticleSearch searcher, Func<string, ISitecoreService> sitecoreFactory)
        {
            SitecoreFactory = sitecoreFactory;
            _sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
            _articleSearcher = searcher;
        }

        /// <summary>
        /// Returns the Article which has the corresonding Article Number. Return Type is Item
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <returns></returns>
        public Item GetArticleItemByNumber(string articleNumber, Guid publicationGuid = default(Guid))
        {

            ArticleItem articleItem = GetArticleByNumber(articleNumber, publicationGuid);
            if (articleItem != null)
            {
                var article = _sitecoreMasterService.GetItem<Item>(articleItem._Id);
                return article;
            }
            return null;
        }

        /// <summary>
        /// Returns the Article which has the corresonding Article Number. Return Type is IArticle
        /// </summary>
        /// <param name="articleNumber"></param>
        /// <returns></returns>
        public ArticleItem GetArticleByNumber(string articleNumber, Guid publicationGuid = default(Guid))
        {
            return GetArticleByNumber(articleNumber, Constants.MasterDb, publicationGuid);
        }

        public ArticleItem GetArticleByNumber(string articleNumber, string databaseName, Guid publicationGuid = default(Guid))
        {
            IArticleSearchFilter filter = _articleSearcher.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            var results = _articleSearcher.SearchCustomDatabase(filter, databaseName, publicationGuid);
            if (results.Articles.Any())
            {
                var foundArticle = results.Articles.FirstOrDefault();
                var service = SitecoreFactory(databaseName);
                if (foundArticle != null)
                    return service.GetItem<ArticleItem>(foundArticle._Id);
            }
            return null;
        }

        public ArticleItem GetArticlesByNumberWithinRTE(string articleNumber, string databaseName)
        {
            IArticleSearchFilter filter = _articleSearcher.CreateFilter();
            filter.ArticleNumbers = articleNumber.SingleToList();
            List<string> PubPrefixes = GetPublicationsPrefixes();
            string VerticalRoot = GetVerticalRootByPubPrefix(new string(articleNumber.Take(2).ToArray()));

            if (PubPrefixes.Any(n => articleNumber.StartsWith(n)))
            {
                string url;
                using (new Sitecore.SecurityModel.SecurityDisabler())
                {
                    //string searchPageId = new ItemReferences().VwbSearchPage.ToString().ToLower().Replace("{", "").Replace("}", "");
                    string hostName = Factory.GetSiteInfo("website")?.HostName ?? WebUtil.GetHostName();
                    url = string.Format("{0}://{1}/api/informasearch?pId={2}&q={3}&verticalroot={2}", HttpContext.Current.Request.Url.Scheme, hostName, VerticalRoot, articleNumber);
                }
                using (var client = new HttpClient())
                {
                    var response = client.GetStringAsync(url).Result;
                    if (!string.IsNullOrEmpty(response))
                    {
                        var resultarticles = JsonConvert.DeserializeObject<SearchResults>(response);
                        if (resultarticles.results.Any())
                        {
                            var foundArticle = resultarticles.results.FirstOrDefault();
                            var service = SitecoreFactory(databaseName);
                            if (foundArticle != null)
                                return service.GetItem<ArticleItem>(foundArticle.ItemId.ToString());
                        }                       
                    }                    
                }
            }            

            return null;
        }

        public List<string> GetPublicationsPrefixes()
        {
            List<string> pubPrefixes = new List<string>();
            //Pharma
            pubPrefixes.Add(Settings.GetSetting("Content.Pharma.ScripIntelligence.Prefix"));
            pubPrefixes.Add(Settings.GetSetting("Content.Pharma.InVivo.Prefix"));
            pubPrefixes.Add(Settings.GetSetting("Content.Pharma.PinkSheet.Prefix"));
            pubPrefixes.Add(Settings.GetSetting("Content.Pharma.MedtechInsight.Prefix"));
            pubPrefixes.Add(Settings.GetSetting("Content.Pharma.RoseSheet.Prefix"));
            //Agri
            pubPrefixes.Add(Settings.GetSetting("Content.Agri.Commodities.Prefix"));
            //Maritime
            pubPrefixes.Add(Settings.GetSetting("Content.Maritime.Lloydslist.Prefix"));

            return pubPrefixes;
        }

        public string GetVerticalRootByPubPrefix(string prefix)
        {
            Item[] allVerticalRoots = Factory.GetDatabase("master").SelectItems("/sitecore/content/*[@@templateid='{DE3615F6-1562-4CB4-80EA-7FA45F49B7B7}']");
            foreach (var VerticalRoot in allVerticalRoots)
            {
                Item[] allSiteRoots = Factory.GetDatabase("master").SelectItems("/sitecore/content/" + VerticalRoot.Name + "/*[@@templateid='{DD003F89-D57D-48CB-B428-FFB519AACA56}']");
                foreach (var siteRoot in allSiteRoots)
                {
                    if (siteRoot != null && !string.IsNullOrEmpty(siteRoot.Fields["Publication Prefix"].Value))
                    {
                        if (string.Equals(prefix, siteRoot.Fields["Publication Prefix"].Value, StringComparison.OrdinalIgnoreCase))
                        {
                            return siteRoot.ParentID.ToGuid().ToString();
                        }
                    }
                }
            }
            return string.Empty;
        }


        /// <summary>
        /// Returns the Version Number of Article
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public int GetWordVersionNumber(ArticleItem article)
        {
            if (article.Word_Document == null) return -1;
            var wordDocURL = article.Word_Document.Url;
            wordDocURL = wordDocURL.Replace("-", " ");
            var wordDoc = _sitecoreMasterService.GetItem<Item>(wordDocURL);
            return wordDoc?.Version.Number ?? -1;
        }

        public ArticlePreviewInfo GetPreviewInfo(ArticleItem article)
        {
            var item = _sitecoreMasterService.GetItem<Item>(article._Id);
            var publicationItem = ArticleExtension.GetAncestorItemBasedOnTemplateID(item);
            Guid publicationGuid = publicationItem.ID.Guid;

            return new ArticlePreviewInfo
            {
                Title = HttpUtility.HtmlDecode(article.Title),
                Publication = _sitecoreMasterService.GetItem<IGlassBase>(publicationGuid)._Name,
                Authors = article.Authors.Select(r => (((IStaff_Item)r).Last_Name + "," + ((IStaff_Item)r).First_Name)).ToList(),
                ArticleNumber = article.Article_Number,
                Date = article.Actual_Publish_Date,
                PreviewUrl = "http://" + WebUtil.GetHostName() + "/?sc_itemid={" + article._Id + "}&sc_mode=preview&sc_lang=en",
                Guid = article._Id
            };
        }

        public ArticlePreviewInfo GetPreviewInfo(IArticle article)
        {
            return GetPreviewInfo(_sitecoreMasterService.GetItem<ArticleItem>(article._Id));
        }


        public string PreviewArticleURL(string articleNumber, string siteHost)
        {
            Guid guid = GetArticleByNumber(articleNumber)._Id;
            if (guid.Equals(Guid.Empty))
            {
                return null;
            }

            return "http://" + siteHost + "/?sc_itemid={" + guid + "}&sc_mode=preview&sc_lang=en";
        }

        /// <summary>
        /// Locks with Article with a Default user
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool LockArticle(Item article)
        {
            if (article.Locking.IsLocked())
            {
                throw new ApplicationException("Trying to lock an already locked article!");
            }
            if (string.IsNullOrEmpty(Sitecore.Context.User.DisplayName))
            {
                return false;
            }
            bool loggedIn = Sitecore.Context.User.IsAuthenticated;
            if (!loggedIn)
            {
                { return false; }
            }

            using (new EditContext(article))
            {
                article.Locking.Lock();
            }

            return true;
        }

        /// <summary>
        /// Unlocks the Article
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public bool UnlockArticle(Item article)
        {
            if (article == null)
            {
                return false;
            }
            string userID = article.Locking.GetOwner();
            if (string.IsNullOrEmpty(userID)) return false;

            bool loggedIn = Sitecore.Context.User.IsAuthenticated;
            if (!loggedIn)
            {
                return false;
            }
            using (new EditContext(article))
            {
                article.Locking.Unlock();
                //there is already a new version created before saving an article
                //var item = article.Versions.AddVersion();
            }

            return true;
        }

        /// <summary>
        /// Returns the CheckedoutStatus of an Article
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        public CheckoutStatus GetLockedStatus(Item article)
        {
            if (article == null)
            {
                var nex = new NullReferenceException("Article item provided was null!");
                throw nex;
            }

            var checkoutStatus = new CheckoutStatus();

            ItemLocking itemLocking = article.Locking;
            checkoutStatus.Locked = itemLocking.IsLocked();
            checkoutStatus.User = itemLocking.GetOwner();

            return checkoutStatus;
        }

        public bool DoesArticleHaveText(ArticleItem article)
        {
            if (string.IsNullOrEmpty(article.Body))
            {
                return false;
            }

            var x = new XmlDocument();

            //using a try-catch here in case the body text isn't in XML format
            try
            {
                x.LoadXml(article.Body);
                return !string.IsNullOrEmpty(x.InnerText.Trim());
            }
            catch (Exception)
            {
                return !string.IsNullOrEmpty(article.Body.Trim());
            }
        }

        public DateTime GetArticleActualPublishedDate(Guid itemID)
        {
            var article = _sitecoreMasterService.GetItem<Item>(itemID);

            var date = DateUtil.IsoDateToDateTime(article.Fields["Actual Publish Date"].Value);
            var actualPublishDate = new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second, DateTimeKind.Local);

            return actualPublishDate;
        }


        /// <summary>
        /// Generates the parent for a article
        /// </summary>	
        public IArticle_Date_Folder GenerateDailyFolder(Guid publicationGuid, DateTime date)
        {
            var publication = _sitecoreMasterService.GetItem<IGlassBase>(publicationGuid);
            string year = date.Year.ToString();
            var month = date.Month < 10 ? "0" + date.Month : date.Month.ToString();
            string day = date.Day < 10 ? "0" + date.Day : date.Day.ToString();

            IHome_Page homeFolder;
            IArticle_Folder articlesFolder;
            IArticle_Date_Folder yearFolder;
            IArticle_Date_Folder monthFolder;
            IArticle_Date_Folder dayFolder;

            // Home Folder
            if (!publication._ChildrenWithInferType.OfType<IHome_Page>().Any())
            {
                var home = _sitecoreMasterService.Create<IHome_Page, IGlassBase>(publication, "Home");
                _sitecoreMasterService.Save(home);
                homeFolder = home;
            }
            else
            {
                homeFolder = publication._ChildrenWithInferType.OfType<IHome_Page>().First();
            }

            // Articles Folder
            if (!homeFolder._ChildrenWithInferType.OfType<IArticle_Folder>().Any())
            {
                var article = _sitecoreMasterService.Create<IArticle_Folder, IGlassBase>(homeFolder, "Articles");
                _sitecoreMasterService.Save(article);
                articlesFolder = article;
            }
            else
            {
                articlesFolder = homeFolder._ChildrenWithInferType.OfType<IArticle_Folder>().First();
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

        public ArticleStruct GetArticleStruct(ArticleItem articleItem)
        {
            var article = _sitecoreMasterService.GetItem<ArticleItem>(articleItem._Id);
            var item = _sitecoreMasterService.GetItem<Item>(article._Id);
            var publicationItem = ArticleExtension.GetAncestorItemBasedOnTemplateID(item);
            Guid publicationGuid = publicationItem.ID.Guid;

            var articleStruct = new ArticleStruct
            {
                ArticleGuid = articleItem._Id,
                Title = articleItem.Title,
                ArticleNumber = articleItem.Article_Number,
                Publication = publicationGuid
            };

            if (articleItem.Content_Type != null)
            {
                articleStruct.Label = articleItem.Content_Type._Id;
            }

            if (articleItem.Media_Type != null)
            {
                articleStruct.MediaType = articleItem.Media_Type._Id;
            }
            articleStruct.WebPublicationDate = articleItem.Planned_Publish_Date;
            articleStruct.PrintPublicationDate = articleItem.Actual_Publish_Date;
            articleStruct.Embargoed = articleItem.Embargoed;
            var authors = articleItem.Authors.Select(r => ((IStaff_Item)r)).ToList();
            articleStruct.Authors = authors.Select(r => new StaffStruct { ID = r._Id, Name = r.Last_Name + ", " + r.First_Name, }).ToList();
            articleStruct.NotesToEditorial = articleItem.Editorial_Notes;

            articleStruct.RelatedArticlesInfo = articleItem.Related_Articles.Select(a => GetPreviewInfo(_sitecoreMasterService.GetItem<IArticle>(a._Id))).ToList();

            articleStruct.ArticleWorkflowState = GetWorkFlowState(articleItem._Id);


            articleStruct.FeaturedImageSource = articleItem.Featured_Image_Source;
            articleStruct.FeaturedImageCaption = articleItem.Featured_Image_Caption;
            if (articleItem.Featured_Image_16_9 != null)
            { articleStruct.FeaturedImage = articleItem.Featured_Image_16_9.MediaId; }

            articleStruct.Taxonomoy = articleItem.Taxonomies.Select(r => new TaxonomyStruct() { Name = r._Name, ID = r._Id, Section = r._Parent._Name }).ToList();

            articleStruct.ReferencedArticlesInfo = articleItem.Referenced_Articles.Select(a => GetPreviewInfo((IArticle)a)).ToList();

            if (articleItem.Word_Document != null)
            {
                var wordDocURL = articleItem.Word_Document.Url;
                wordDocURL = wordDocURL.Replace("-", " ");
                var wordDoc = _sitecoreMasterService.GetItem<Item>(wordDocURL);

                if (wordDoc != null)
                {
                    articleStruct.WordDocVersionNumber = wordDoc.Version.Number;
                    articleStruct.WordDocLastUpdateDate = wordDoc.Statistics.Updated.ToString();
                    articleStruct.WordDocLastUpdatedBy = wordDoc.Statistics.UpdatedBy;
                }
            }

            try
            {
                ISitecoreService service = new SitecoreContentContext();
                var webItem = service.GetItem<Item>(articleItem._Id);
                articleStruct.IsPublished = webItem != null;
            }
            catch (Exception ex)
            {
                articleStruct.IsPublished = false;
            }

            return articleStruct;
        }

        public ArticleWorkflowState GetWorkFlowState(Guid articleId)
        {
            var item = _sitecoreMasterService.GetItem<Item>(articleId);
            if (item?.State?.GetWorkflowState() == null) return null;
            var currentState = item.State.GetWorkflowState();
            Sitecore.Workflows.IWorkflow workflow = item.State.GetWorkflow();
            var workFlowState = new ArticleWorkflowState();
            workFlowState.IsFinal = currentState.FinalState;
            workFlowState.DisplayName = currentState.DisplayName;
            var commands = new List<ArticleWorkflowCommand>();
            foreach (Sitecore.Workflows.WorkflowCommand command in workflow.GetCommands(item))
            {
                var wfCommand = new ArticleWorkflowCommand();
                wfCommand.DisplayName = command.DisplayName;
                wfCommand.StringID = command.CommandID;
                ICommand commandItem = _sitecoreMasterService.GetItem<ICommand>(new Guid(command.CommandID));
                IState nextStateItem = _sitecoreMasterService.GetItem<IState>(commandItem.Next_State);

                if (nextStateItem != null)
                {
                    wfCommand.SendsToFinal = nextStateItem.Final;
                    wfCommand.GlobalNotifyList = new List<StaffStruct>();
                    foreach (var staff in nextStateItem.Staffs)
                    {
                        var staffItem = _sitecoreMasterService.GetItem<IStaff_Item>(staff._Id);
                        if (staffItem.Inactive)
                        {
                            continue;
                        }
                        var staffMember = new StaffStruct
                        {
                            ID = staffItem._Id,
                            Name = staffItem.Last_Name + " , " + staffItem.First_Name
                        };
                        //   staffMember.Publications = staff  //TODO :Check if this field if we need this field.
                        wfCommand.GlobalNotifyList.Add(staffMember);
                    }
                    commands.Add(wfCommand);
                }
            }
            workFlowState.Commands = commands;

            return workFlowState;
        }

        public void SetWorkflowState(Item i, string commandID)
        {
            i[FieldIDs.WorkflowState] = commandID;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <param name="commandID">Optional: if included, the command will be executed</param>
        /// <returns>The workflow state of the item (after workflow command has executed, if given one)</returns>
        public ArticleWorkflowState ExecuteCommandAndGetWorkflowState(Item i, string commandID)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                IWorkflow workflow = i.State.GetWorkflow();

                if (workflow == null)
                {
                    //uh oh
                    return new ArticleWorkflowState();
                }

                if (commandID != null)
                {
                    //var oldWorkflow = workflow.WorkflowID;
                    // This line will cause the workflow field to be set to null... sometimes
                    workflow.Execute(commandID, i, "comments", false);
                    //var info = new WorkflowInfo(oldWorkflow, i.Fields[Sitecore.FieldIDs.WorkflowState].Value);
                    //i.Database.DataManager.SetWorkflowInfo(i, info);
                    //i.Fields[Sitecore.FieldIDs.Workflow].Value = oldWorkflow;

                }
                var state = new ArticleWorkflowState();

                var currentState = workflow.GetState(i);
                state.DisplayName = currentState.DisplayName;
                state.IsFinal = currentState.FinalState;

                var commands = new List<ArticleWorkflowCommand>();
                foreach (Sitecore.Workflows.WorkflowCommand command in workflow.GetCommands(i))
                {
                    var wfCommand = new PluginModels.ArticleWorkflowCommand();
                    wfCommand.DisplayName = command.DisplayName;
                    wfCommand.StringID = command.CommandID;

                    ICommand commandItem = _sitecoreMasterService.GetItem<ICommand>(new Guid(command.CommandID));

                    IState stateItem = _sitecoreMasterService.GetItem<IState>(commandItem.Next_State);

                    if (stateItem == null)
                    {
                        //_logger.Warn("WorkflowController.ExecuteCommandAndGetWorkflowState: Next state for command [" + command.CommandID + "] is null!");
                    }
                    else
                    {
                        wfCommand.SendsToFinal = stateItem.Final;
                        wfCommand.GlobalNotifyList = new List<StaffStruct>();

                        foreach (var x in stateItem.Staffs)
                        {
                            var staffItem = _sitecoreMasterService.GetItem<IStaff_Item>(x._Id);
                            if (staffItem.Inactive) { continue; }

                            var staffMember = new StaffStruct();
                            staffMember.ID = staffItem._Id;
                            //staffMember.Name = staffItem.GetFullName();
                            //staffMember.Publications = staffItem.Publications.ListItems.Select(p => p.ID.ToGuid()).ToArray();
                            wfCommand.GlobalNotifyList.Add(staffMember);
                        }

                        commands.Add(wfCommand);
                    }
                }

                state.Commands = commands;

                return state;
            }
        }
    }
}