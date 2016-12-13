using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.UserPreference;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Web.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Informa.Library.Utilities.Extensions;
using Informa.Web.ViewModels.Articles;
using Informa.Library.Search.Utilities;

namespace Informa.Web.ViewModels.Emails
{
    /// <summary>
    /// View model for PersonalizedEmail
    /// IPMP-906
    /// </summary>
    public class PersonalizedEmailViewModel
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserPreferenceContext UserPreferences;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;

        public PersonalizedEmailViewModel(
                        ISiteRootContext siteRootContext,
            IUserPreferenceContext userPreferences,
            IGlobalSitecoreService globalService,
            ITextTranslator textTranslator,
            IArticleSearch articleSearch,
            IArticleListItemModelFactory articleListableFactory)
        {
            SiteRootContext = siteRootContext;
            UserPreferences = userPreferences;
            GlobalService = globalService;
            TextTranslator = textTranslator;
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IList<ISection> Sections => GetSections();

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of my view page scetions</returns>
        private IList<ISection> GetSections()
        {
            var sections = new List<ISection>();

            if (UserPreferences.Preferences != null && UserPreferences.Preferences.PreferredChannels != null
               && UserPreferences.Preferences.PreferredChannels.Any())
            {

                var channels = UserPreferences.Preferences.PreferredChannels.OrderBy(channel => channel.ChannelOrder).ToList(); ;
                foreach (Channel channel in channels)
                {
                    CreateSections(channel, sections, UserPreferences.Preferences.IsChannelLevel, UserPreferences.Preferences.IsNewUser);
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
        private void CreateSections(Channel channel, List<ISection> sections, bool isChannelLevel, bool isNewUser)
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
        private void CreateSectionsFromChannels(Channel channel, List<ISection> sections, bool isNewUser, ref IList<Topic> topics)
        {
            var channelPageItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page>(channel.ChannelId);
            if (channelPageItem != null)
            {
                Section sec = new Section();
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

                SetArticlesForSection(sec);
                sec.SectionTitle = string.Format("{0} {1}", TextTranslator.Translate("Email.LatestFrom"), sec.ChannelName);
                sections.Add(sec);
            }
        }

        /// <summary>
        /// Sets the articles for section.
        /// </summary>
        /// <param name="sec">The sec.</param>
        private void SetArticlesForSection(Section sec)
        {
            var filter = ArticleSearch.CreateFilter();
            filter.Page = 1;
            filter.PageSize = SiteRootContext.Item.Max_Number_Of_Articles_Per_Section;
            filter.TaxonomyIds.AddRange(sec.TaxonomyIds.Select(taxonomy => new Guid(taxonomy)));
            var results = ArticleSearch.PersonalizedSearch(filter, null, null);
            if (results != null && results.Articles != null)
            {
                var articles = results.Articles.Where(a => a != null).Select(a => ArticleListableFactory.CreatePersonalizedArticle(a));
                var taxonomy = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page>(sec.ChannelId)?.Taxonomies.FirstOrDefault();
                var loadMore = new LoadMore
                {
                    LoadMoreLinkText = string.Format("{0} {1}", TextTranslator.Translate("Email.SeeMoreFrom"), sec.ChannelName),
                    LoadMoreLinkUrl = taxonomy != null ? SearchTaxonomyUtil.GetSearchUrl(taxonomy) : string.Empty
                };
                sec.Articles = articles;
                sec.LoadMore = loadMore;
            }
        }

        /// <summary>
        /// Creates the sections from topics.
        /// </summary>
        /// <param name="sections">The sections.</param>
        /// <param name="topics">The topics.</param>
        private void CreateSectionsFromTopics(List<ISection> sections, IList<Topic> topics)
        {
            Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicItem;
            string taxonomyId = string.Empty;
            foreach (Topic topic in topics)
            {
                topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId);
                if (topicItem != null)
                {
                    Section sec = new Section();
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
    }
}