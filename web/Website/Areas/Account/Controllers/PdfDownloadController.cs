#region using
using System.IO;
using System.Linq;
using System.Web.Mvc;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using System.Text;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using Informa.Library.User.UserPreference;
using Informa.Library.Site;
using Informa.Library.Article.Search;
using Informa.Library.Services.Global;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Web.ViewModels.PDF;
using Informa.Web.ViewModels.Articles;
using Informa.Web.Helpers;
using iTextSharp.tool.xml;
using Informa.Library.PDF;
using Informa.Web.ViewModels;
using Informa.Library.Search.Utilities;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Globalization;
using Informa.Library.Subscription;
#endregion

namespace Informa.Web.Areas.Account.Controllers
{
    public class PdfDownloadController : Controller
    {
        GlobalElements globalElement = new GlobalElements();
        string completeHtml = string.Empty;
        protected readonly IUserPreferenceContext UserPreferences;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly IArticleSearch Searcher;
        protected readonly ITextTranslator TextTranslator;
        protected readonly SideNavigationMenuViewModel UserSubcriptions;
        public PdfDownloadController(IUserPreferenceContext userPreferences,
                                        ISiteRootContext siterootContext,
                                        IGlobalSitecoreService globalService, 
                                        IArticleSearch articleSearch, 
                                        IArticleListItemModelFactory articleListableFactory,
                                        IArticleSearch searcher,
                                        ITextTranslator textTranslator,
                                        SideNavigationMenuViewModel userSubscriptionsContext)
                                    {
                                        UserPreferences = userPreferences;
                                        SiterootContext = siterootContext;
                                        GlobalService = globalService;
                                        ArticleSearch = articleSearch;
                                        ArticleListableFactory = articleListableFactory;
                                        Searcher = searcher;
                                        TextTranslator = textTranslator;
                                        UserSubcriptions = userSubscriptionsContext;
                                    }

        /// <summary>
        /// Controller method for downloading pdf
        /// </summary>
        /// <param name="pdfPageUrl">Page URL</param>
        /// <param name="userEmail">User email who is downloading PDF</param>
        /// <param name="PdfTitle">PDF Title</param>
        /// <param name="DataToolLinkDesc">Data tool description</param>
        /// <param name="DataToolLinkText">Data Tool Link Text</param>
        /// <returns></returns>
        public ActionResult GenerateAndDownloadPdf(string pdfPageUrl, string userEmail, string PdfTitle, string DataToolLinkDesc, string DataToolLinkText)
        {
            GeneratePdfDocument(pdfPageUrl, userEmail, PdfTitle, DataToolLinkDesc, DataToolLinkText, PdfType.Manual, null);
            return new EmptyResult();
        }

        /// <summary>
        /// Controller method for downloading Personalize pdf
        /// </summary>
        /// <param name="pdfPageUrl"></param>
        /// <param name="userEmail"></param>
        /// <param name="PdfTitle"></param>
        /// <param name="PubStartDate"></param>
        /// <param name="PubEndDate"></param>
        /// <param name="ArticleSize"></param>
        /// <param name="DataToolLinkDesc"></param>
        /// <param name="DataToolLinkText"></param>
        /// <returns></returns>
        public ActionResult GenerateAndDownloadPersonalizePdf(string pdfPageUrl, string userEmail, string PdfTitle, DateTime? PubStartDate, DateTime? PubEndDate, int ArticleSize, string DataToolLinkDesc, string DataToolLinkText)
        {
            if (PubStartDate == default(DateTime) && PubEndDate == default(DateTime))
            {
                PubEndDate = DateTime.Now;
                PubStartDate = DateTime.Now.AddHours(-24);
            }
            else if (PubStartDate > PubEndDate)
            {
                PubStartDate = null;
                PubEndDate = null;
            }
            else if (PubStartDate == default(DateTime) || PubEndDate == default(DateTime))
            {
                PubEndDate = DateTime.Now;
                PubStartDate = DateTime.Now.AddHours(-24);
            }

            string strngHtml = string.Empty;
            List<PersonalizedPdfViewModel> pdfArticle = new List<PersonalizedPdfViewModel>();
            ArticleSearchRequest artSearch = new ArticleSearchRequest();
            artSearch.PageNo = 1;
            artSearch.PageSize = ArticleSize;
            artSearch.TaxonomyIds = new List<string>();

            IList<Web.Models.Section> getsection = GetSections();

            for (int i = 0; i < getsection.Count(); i++)
            {
                var texonomy = getsection[i].TaxonomyIds;
                foreach (var texn in texonomy)
                {
                    artSearch.TaxonomyIds.Add(texn);
                }
                var articles = GetArticles(artSearch, PubStartDate, PubEndDate);
                foreach (var selectArticle in articles.Articles.Where(a => a != null))
                {
                    pdfArticle.Add(new PersonalizedPdfViewModel
                    {
                        Body = selectArticle.Body,
                        PublishDate = selectArticle.Actual_Publish_Date,
                        Summary = selectArticle.Summary,
                        Texonomies = selectArticle.Taxonomies.Take(3).Select(x => new LinkableModel
                        {
                            LinkableText = x.Item_Name,
                            LinkableUrl = SearchTaxonomyUtil.GetSearchUrl(x)
                        }),
                        Title = selectArticle.Title,
                        ImageUrl = selectArticle.Featured_Image_16_9?.Src ?? string.Empty,
                        ImageAltText = selectArticle.Featured_Image_16_9?.Alt ?? string.Empty,
                        ImageCaption = selectArticle.Featured_Image_Caption,
                        ImageSource = selectArticle.Featured_Image_Source,
                        abslouteUrl = selectArticle._AbsoluteUrl,
                        ContentType = selectArticle.Content_Type.Item_Name,
                        Sub_Title = selectArticle.Sub_Title,
                        Author = selectArticle.Authors.Select(x => new PersonModel(x)),
                        RelatedArticles = GetRelatedArticles(selectArticle),
                        ExecutiveSummary = TextTranslator.Translate("SharedContent.ExecutiveSummary")
                    });
                }
                artSearch.TaxonomyIds.Clear();
            }

            foreach (var slctArticle in pdfArticle)
            {
                var perHtml = MvcHelpers.GetRazorViewAsString(slctArticle, "~/Views/Shared/Components/PDF/FullArticlePersonalizedPDF.cshtml");
                if (perHtml != null)
                    strngHtml += perHtml;
            }
            GeneratePdfDocument(pdfPageUrl, userEmail, PdfTitle, DataToolLinkDesc, DataToolLinkText, PdfType.Personalized, strngHtml);
            return new EmptyResult();
        }

        /// <summary>
        /// Generating PDF Body
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF Document</param>
        /// <param name="pdfPageUrl">Page URL</param>
        /// <param name="userEmail">User email who is downloading PDF</param>
        /// <param name="PdfTitle">PDF Title</param>
        /// <param name="DataToolLinkDesc">Data tool description</param>
        /// <param name="DataToolLinkText">Data Tool Link Text</param>
        private void GeneratePdfBody(PdfWriter writer, Document document, string userEmail, string DataToolLinkDesc, string DataToolLinkText, string pdfPageUrl, PdfType pdfType, string strngHtml = null)
        {
            string fullPageHtml = string.Empty;
            string html = string.Empty;
            HtmlDocument doc = new HtmlDocument();

            if (!string.IsNullOrEmpty(pdfPageUrl))
            {
                if (!pdfPageUrl.StartsWith("http"))
                {
                    pdfPageUrl = HttpContext.Request.Url.Scheme + "://" + pdfPageUrl;
                }
                HttpWebRequest webRequest = WebRequest.Create(pdfPageUrl) as HttpWebRequest;
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                webRequest.Method = "POST";
                System.Net.WebClient client = new WebClient();
                client.UseDefaultCredentials = true;
                client.Headers.Add("User-Agent: Other");
                byte[] data = client.DownloadData(pdfPageUrl);
                fullPageHtml = Encoding.UTF8.GetString(data);
                doc.LoadHtml(fullPageHtml);

                if (pdfType == PdfType.Personalized && !string.IsNullOrEmpty(strngHtml))
                {
                    var personalizedHtmlNode = doc.DocumentNode.SelectSingleNode("//div[@class='personalized-pdf-article-body']");
                    if (personalizedHtmlNode != null)
                    {
                        personalizedHtmlNode.InnerHtml = strngHtml;
                    }
                }
                html = doc.GetElementbyId("mainContentPdf").InnerHtml;

                var replacements = new Dictionary<string, string>
                {
                    ["<p"] = "<p style=\"color:#58595b; font-size:18px; line-height:30px;\"",
                    ["<li>"] = "<li style=\"color:#58595b; font-size:18px; line-height:30px;\">",
                    ["</p>"] = "</p>",
                    ["#UserName#"] = userEmail,
                    ["#HeaderDate#"] = DateTime.Now.ToString("dd MMMM yyyy"),
                    ["#FooterDate#"] = DateTime.Now.ToString("dd MMM yyyy")
                };
                html = html.ReplacePatternCaseInsensitive(replacements);
                string decodedHtml = HtmlEntity.DeEntitize(html);

                HtmlDocument ReqdDoc = new HtmlDocument();
                ReqdDoc.LoadHtml(decodedHtml);

                var tableNodes = ReqdDoc.DocumentNode.SelectNodes("//table[@id='tableFromArticle']")?.ToList();
                if (tableNodes != null && tableNodes.Any())
                {
                    foreach (var tableNode in tableNodes)
                    {
                        tableNode.SetAttributeValue("width", "688px");
                        tableNode.SetAttributeValue("style", "color:#58595b");
                    }
                }

                HtmlNode CommonFooterNode = ReqdDoc.DocumentNode.SelectSingleNode("//div[@class='pdf-footer']");
                if (CommonFooterNode != null)
                {
                    globalElement.CommonFooter = CommonFooterNode.InnerText;
                    CommonFooterNode.ParentNode.RemoveChild(CommonFooterNode);
                }
                var domain = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host;

                var images = ReqdDoc.DocumentNode.SelectNodes("//img/@src")?.ToList();
                if (images != null && images.Any())
                {
                    foreach (HtmlNode img in images)
                    {
                        if (!img.Attributes["src"].Value.StartsWith("http") && !img.Attributes["src"].Value.StartsWith("https") && !img.Attributes["src"].Value.StartsWith("www"))
                        {
                            img.SetAttributeValue("src", domain + img.Attributes["src"].Value);
                        }
                    }
                }
                var links = ReqdDoc.DocumentNode.SelectNodes("//a/@href")?.ToList();
                if (links != null && links.Any())
                {
                    foreach (var link in links)
                    {
                        if (!link.Attributes["href"].Value.StartsWith("http") && !link.Attributes["href"].Value.StartsWith("https") && !link.Attributes["href"].Value.StartsWith("www") && !link.Attributes["href"].Value.StartsWith("mailto"))
                        {
                            link.SetAttributeValue("href", domain + link.Attributes["href"].Value);
                            link.SetAttributeValue("target", "_blank");

                        }

                        if (link.Name != "img" && !link.ChildNodes.Select(n => n.Name).Contains("img"))
                        {
                            link.InnerHtml = link.InnerText;
                        }

                        if (!link.Attributes.Contains(@"style"))
                        {
                            link.SetAttributeValue("style", "color:#be1e2d; text-decoration:none");
                        }
                    }
                }

                var articleNodes = ReqdDoc.DocumentNode.SelectNodes("//div[@class='article-body-content']")?.ToList();
                if (articleNodes != null && articleNodes.Any())
                {
                    foreach (var articleNode in articleNodes)
                    {
                        var articleTitleNode = articleNode.SelectSingleNode(".//a[@class='article-title']");
                        var articleURL = articleTitleNode?.Attributes["href"].Value;

                        //AMcharts in Article
                        var amchartNodes = articleNode.SelectNodes(".//div[@class='amchart-section-pdf']")?.ToList();
                        if (amchartNodes != null && amchartNodes.Any())
                        {
                            foreach (var amchartNode in amchartNodes)
                            {
                                var newNodeHtml = "<b><p style=\"margin: 0 0 16px 0; color:#58595b; font-size:18px; line-height:30px;\">" + DataToolLinkDesc + "</p></b><br /><br />" + "<a style=\"color:#be1e2d; text-decoration:none; font-size:18px; line-height:30px;\" href=\"" + articleURL + "\">" + DataToolLinkText + "</a>";
                                HtmlNode newNode = HtmlNode.CreateNode("div");
                                newNode.InnerHtml = newNodeHtml;
                                amchartNode.ParentNode.ReplaceChild(newNode, amchartNode);
                            }
                        }

                        //Tableau in Article
                        var tableauNodes = articleNode.SelectNodes(".//div[@class='article-data-tool-placeholder']")?.ToList();
                        if (tableauNodes != null && tableauNodes.Any())
                        {
                            foreach (var tableauNode in tableauNodes)
                            {
                                var newNodeHtml = "<b><p style=\"margin: 0 0 16px 0; color:#58595b; font-size:18px; line-height:30px;\">" + DataToolLinkDesc + "</p></b><br /><br />" + "<a style=\"color:#be1e2d; text-decoration:none; font-size:18px; line-height:30px;\" href=\"" + articleURL + "\">" + DataToolLinkText + "</a>";
                                HtmlNode newNode = HtmlNode.CreateNode("div");
                                newNode.InnerHtml = newNodeHtml;
                                tableauNode.ParentNode.ReplaceChild(newNode, tableauNode);
                            }
                        }
                    }
                }

                var executiveSummaryNodes = ReqdDoc.DocumentNode.SelectNodes("//span[@class='article-executive-summary-body']")?.ToList();
                if (executiveSummaryNodes != null && executiveSummaryNodes.Any())
                {
                    foreach (var executiveSummaryNode in executiveSummaryNodes)
                    {
                        if (executiveSummaryNode.FirstChild.Name != "p")
                        {
                            var newNode = HtmlNode.CreateNode("<span class=\"article-executive-summary-body\" style=\"font-size:18px; color:#58595b;\">");
                            newNode.InnerHtml = "<p style=\"color:#58595b; font-size:18px; line-height:30px;\">" + executiveSummaryNode.InnerHtml + "</p>";
                            executiveSummaryNode.ParentNode.ReplaceChild(newNode, executiveSummaryNode);
                        }
                    }
                }
                ReqdDoc.OptionOutputAsXml = true;
                ReqdDoc.OptionCheckSyntax = true;
                ReqdDoc.OptionFixNestedTags = true;

                using (var srHtml = new StringReader(ReqdDoc.DocumentNode.InnerHtml))
                {
                    //Parse the HTML
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, document, srHtml);
                }
            }
        }

        /// <summary>
        /// Generate Personalize Pdf Body
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        /// <param name="pdfPageUrl"></param>
        /// <param name="userEmail"></param>
        /// <param name="DataToolLinkDesc"></param>
        /// <param name="DataToolLinkText"></param>
        /// <param name="fullPersonalizedHtml"></param>
        private void GeneratePdfDocument(string pdfPageUrl, string userEmail, string PdfTitle, string DataToolLinkDesc, string DataToolLinkText, PdfType pdfType, string strngHtml)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Document document = new Document(PageSize.A4);
                PdfWriter writer = PdfWriter.GetInstance(document, ms);

                globalElement.CommonHeader = PdfTitle;
                writer.PageEvent = globalElement;
                document.Open();

                GeneratePdfBody(writer, document, userEmail, DataToolLinkDesc, DataToolLinkText, pdfPageUrl, pdfType, strngHtml);
                writer.StrictImageSequence = true;

                //Add meta information to the document
                document.AddAuthor(userEmail);
                document.AddCreator("PDF Creation using iTextSharp");
                document.AddKeywords("PDF Creation using iTextSharp");
                document.AddSubject(PdfTitle);
                document.AddTitle(PdfTitle);

                document.Close();
                writer.Close();

                System.Web.HttpContext.Current.Response.ContentType = "application/pdf";
                System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "inline;filename=" + PdfTitle + ".pdf");
                System.Web.HttpContext.Current.Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
            }
        }
        /*---------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of my view page scetions</returns>
        private IList<Web.Models.Section> GetSections()
        {
            var sections = new List<Web.Models.Section>();

            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
               && UserPreferences.Preferences.PreferredChannels.Any())
            {

                var channels = UserPreferences.Preferences.PreferredChannels.OrderBy(channel => channel.ChannelOrder).ToList();
                foreach (Channel channel in channels)
                {
                    CreateSections(channel, sections, UserPreferences.Preferences.IsChannelLevel, UserPreferences.Preferences.IsNewUser);
                }
            }
            if(sections.Count == 0)
            {
                bool IsTopicSubscription = false;
                IEnumerable<ISubscription>  subscriptions = UserSubcriptions.GetValidSubscriptions();
                //IsTopicSubscription = subscriptions.Select(a => a.IsTopicSubscription);
                foreach (var sub in subscriptions)
                {
                    IsTopicSubscription = sub.IsTopicSubscription;
                    for (int i = 0; i < sub.SubscribedChannels.Count; i++)
                    {
                        CreateSectionsFromChannels(sub.SubscribedChannels[i], sections, IsTopicSubscription);
                    }
                }
            }
            return sections;
        }

        /// <summary>
        /// Creates the sections.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="sections">The sections.</param>
        /// <param name="isChannelLevel">if set to <c>true</c> [is channel level].</param>
        /// <param name="isNewUser">if set to <c>true</c> [is new user].</param>
        private void CreateSections(Channel channel, List<Web.Models.Section> sections, bool isChannelLevel, bool isNewUser)
        {
            bool channelStatus = channel.IsFollowing;
            IList<Topic> topics = new List<Topic>();

            if (channel.Topics != null && channel.Topics.Any())
            {
                channelStatus = (channel.IsFollowing && isNewUser) || channel.Topics.Any(topic => topic.IsFollowing);
                topics = channel.Topics.Where(topic => topic.IsFollowing).OrderBy(topic => topic.TopicOrder).ToList();
            }

            if (channelStatus && !isChannelLevel)
            {
                CreateSectionsFromTopics(sections, topics);
            }
            else if (channelStatus)
            {
                CreateSectionsFromChannels(channel, sections, isNewUser, ref topics);
            }
        }

        /// <summary>
        /// Creates the sections from channels.
        /// </summary>
        /// <param name="channel">The channel.</param>
        /// <param name="sections">The sections.</param>
        /// <param name="isNewUser">if set to <c>true</c> [is new user].</param>
        /// <param name="topics">The topics.</param>
        private void CreateSectionsFromChannels(Channel channel, List<Web.Models.Section> sections, bool isNewUser, ref IList<Topic> topics)
        {
            var channelPageItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page>(channel.ChannelId);
            if (channelPageItem != null)
            {
                Web.Models.Section sec = new Web.Models.Section();
                sec.TaxonomyIds = new List<string>();
                sec.ChannelName = channelPageItem?.Display_Text;
                sec.ChannelId = channelPageItem._Id.ToString();
                string taxonomyId = string.Empty;
                if (channel.IsFollowing && (topics == null || !topics.Any()))
                {
                    taxonomyId = channelPageItem.Taxonomies != null && channelPageItem.Taxonomies.Any() ? channelPageItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(taxonomyId))
                        sec.TaxonomyIds.Add(taxonomyId);
                }
                if (!topics.Any() && channel.Topics != null && channel.Topics.Any())
                    topics = channel.Topics;
                if (topics != null && topics.Any())
                {
                    Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicItem;
                    foreach (Topic topic in topics)
                    {
                        topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId);
                        taxonomyId = topicItem != null && topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                        if (!string.IsNullOrWhiteSpace(taxonomyId))
                            sec.TaxonomyIds.Add(taxonomyId);
                    }
                }
                else if (isNewUser)
                {
                    var pageAssetsItem = channelPageItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                    if (pageAssetsItem != null)
                    {
                        var topicItems = pageAssetsItem._ChildrenWithInferType.
                               OfType<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>();
                        foreach (Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topic in topicItems)
                        {
                            taxonomyId = topic.Taxonomies != null && topic.Taxonomies.Any() ? topic?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                            if (!string.IsNullOrWhiteSpace(taxonomyId))
                                sec.TaxonomyIds.Add(taxonomyId);
                        }
                    }
                }
                sections.Add(sec);
            }
        }

        /// <summary>
        /// Create sections from Channels for entitlement
        /// </summary>
        /// <param name="Channel"></param>
        /// <param name="sections"></param>
        /// <param name="IsTopicSubscription"></param>
        private void CreateSectionsFromChannels(ChannelSubscription Channel, List<Web.Models.Section> sections, bool IsTopicSubscription)
        {
            var channelPageItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page>(Channel._ChannelId);
            if (channelPageItem != null)
            {
                Web.Models.Section sec = new Web.Models.Section();
                sec.TaxonomyIds = new List<string>();
                sec.ChannelName = channelPageItem?.Display_Text;
                sec.ChannelId = channelPageItem._Id.ToString();
                string taxonomyId = string.Empty;
                if (!IsTopicSubscription)
                {
                    taxonomyId = channelPageItem.Taxonomies != null && channelPageItem.Taxonomies.Any() ? channelPageItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(taxonomyId))
                        sec.TaxonomyIds.Add(taxonomyId);
                }

                if (IsTopicSubscription)
                {
                    Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicItem;
                    topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(Channel._ChannelId);
                    taxonomyId = topicItem != null && topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;

                    if (!string.IsNullOrWhiteSpace(taxonomyId))
                        sec.TaxonomyIds.Add(taxonomyId);
                }
                sections.Add(sec);
            }
        }

        /// <summary>
        /// Creates the sections from topics.
        /// </summary>
        /// <param name="sections">The sections.</param>
        /// <param name="topics">The topics.</param>
        private void CreateSectionsFromTopics(List<Web.Models.Section> sections, IList<Topic> topics)
        {
            Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicItem;
            string taxonomyId = string.Empty;
            foreach (Topic topic in topics)
            {
                topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId);
                if (topicItem != null)
                {
                    Web.Models.Section sec = new Web.Models.Section();
                    sec.TaxonomyIds = new List<string>();
                    sec.ChannelName = topicItem?.Navigation_Text;
                    sec.ChannelId = topicItem._Id.ToString();
                    taxonomyId = topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(taxonomyId))
                        sec.TaxonomyIds.Add(taxonomyId);
                    sections.Add(sec);
                }
            }
        }

        /// <summary>
        /// Get article based on date.
        /// </summary>
        /// <param name="articleRequest"></param>
        /// <param name="PubStartDate"></param>
        /// <param name="PubEndDate"></param>
        /// <returns></returns>
        public IPersonalizedArticleSearchResults GetArticles(ArticleSearchRequest articleRequest, DateTime? PubStartDate, DateTime? PubEndDate)
        {
            //if (articleRequest == null || articleRequest.TaxonomyIds == null || articleRequest.TaxonomyIds.Count < 1)
            //    return new { Articles = "No articles found" };
            var filter = ArticleSearch.CreateFilter();
            filter.Page = articleRequest.PageNo;
            filter.PageSize = articleRequest.PageSize;
            filter.TaxonomyIds.AddRange(articleRequest.TaxonomyIds.Select(x => new Guid(x)));
            var results = ArticleSearch.PersonalizedSearch(filter, PubStartDate, PubEndDate);
            return new PersonalizedArticleSearchResults
            {
                Articles = results.Articles,
                TotalResults = results.TotalResults
            };
        }

        /// <summary>
        /// Get Related Articles
        /// </summary>
        /// <param name="article"></param>
        /// <returns></returns>
        private IEnumerable<IListable> GetRelatedArticles(IArticle article)
        {
            var relatedArticles = article.Related_Articles.Concat(article.Referenced_Articles).Take(10).ToList();

            if (relatedArticles.Count < 10)
            {
                var filter = Searcher.CreateFilter();
                filter.ReferencedArticle = article._Id;
                filter.PageSize = 10 - relatedArticles.Count;

                var results = Searcher.Search(filter);
                relatedArticles.AddRange(results.Articles);
            }
            return relatedArticles.Where(r => r != null).Select(x => ArticleListableFactory.Create(GlobalService.GetItem<IArticle>(x._Id))).Cast<IListable>().OrderByDescending(x => x.ListableDate);
        }
    }
    public class GlobalElements : PdfPageEventHelper
    {

        // This is the contentbyte object of the writer
        PdfContentByte cb;

        // this is the BaseFont we are going to use for the header / footer
        BaseFont bf = null;

        // This keeps track of the creation time
        DateTime PrintTime = DateTime.Now;
        public string CommonHeader { get; set; }
        public string CommonFooter { get; set; }

        /// <summary>
        /// Overriding PDF OnOpenDocument for reading PDF writer content on document open 
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF document</param>
        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            try
            {
                PrintTime = DateTime.Now;
                bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                cb = writer.DirectContent;
            }
            catch (DocumentException de)
            {

            }
            catch (IOException ioe)
            {

            }
        }
        /// <summary>
        /// Set Document margins on startPage method
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="document"></param>
        public override void OnStartPage(PdfWriter writer, Document document)
        {
            if (writer.PageNumber == 1)
            {
                document.SetMargins(40, 40, 30, 60);
            }
            else
            {
                document.SetMargins(40, 40, 60, 60);
            }
            document.NewPage();
        }
        /// <summary>
        /// Adding Common Header and Footer
        /// </summary>
        /// <param name="writer">PDF writer</param>
        /// <param name="document">PDF document</param>
        public override void OnEndPage(PdfWriter writer, Document document)
        {
            base.OnEndPage(writer, document);

            Font baseFontNormal = new Font(Font.FontFamily.HELVETICA, 12f, Font.NORMAL, BaseColor.BLACK);
            Font baseFontBig = new Font(Font.FontFamily.HELVETICA, 12f, Font.BOLD, BaseColor.BLACK);
            string PageNumber = writer.PageNumber.ToString();

            //Add paging to footer
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetTextMatrix(document.PageSize.GetRight(40), document.PageSize.GetBottom(20));
                cb.ShowText(PageNumber);
                cb.EndText();
            }

            //Adding common header from page number-2
            if (writer.PageNumber > 1)
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 10);
                cb.SetColorFill(BaseColor.DARK_GRAY);
                cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetTop(25));
                cb.ShowText(!string.IsNullOrEmpty(CommonHeader) ? CommonHeader : string.Empty);
                cb.EndText();
            }
            //Adding common footer
            cb.BeginText();
            cb.SetFontAndSize(bf, 10);
            cb.SetColorFill(BaseColor.DARK_GRAY);
            cb.SetTextMatrix(document.PageSize.GetLeft(40), document.PageSize.GetBottom(20));
            cb.ShowText(!string.IsNullOrEmpty(CommonFooter) ? CommonFooter : string.Empty);
            cb.EndText();


            //ColumnText ct = new ColumnText(cb);
            //ct.SetSimpleColumn(document.PageSize.GetLeft(40), document.PageSize.GetBottom(20), 70, 70, 20, Element.ALIGN_CENTER);
            //ct.Go();


            //Move the pointer and draw line to separate header section from rest of page
            if (writer.PageNumber == 1)
            {
                cb.MoveTo(40, document.PageSize.Height - 80);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 80);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.DARK_GRAY);
            }
            else
            {
                cb.MoveTo(40, document.PageSize.Height - 40);
                cb.LineTo(document.PageSize.Width - 40, document.PageSize.Height - 40);
                cb.Stroke();
                cb.SetColorStroke(BaseColor.DARK_GRAY);
            }

            //Move the pointer and draw line to separate footer section from rest of page
            cb.MoveTo(40, document.PageSize.GetBottom(40));
            cb.LineTo(document.PageSize.Width - 40, document.PageSize.GetBottom(40));
            cb.Stroke();
            cb.SetColorStroke(BaseColor.DARK_GRAY);
        }
    }
}
