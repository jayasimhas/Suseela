﻿using Informa.Library.Article.Search;
using Informa.Library.Globalization;
using Informa.Library.Search.Utilities;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.UserPreference;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Models;
using Informa.Web.ViewModels.Articles;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Log = Sitecore.Diagnostics.Log;
using System.Diagnostics;

namespace Informa.Web.ViewModels.Emails
{
    /// <summary>
    /// View model for PersonalizedEmail
    /// IPMP-906
    /// </summary>
    public class PersonalizedEmailViewModel
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;
        protected readonly PreferencesUtil PreferencesUtil;

        public PersonalizedEmailViewModel(
                        ISiteRootContext siteRootContext,
            IGlobalSitecoreService globalService,
            ITextTranslator textTranslator,
            IArticleSearch articleSearch,
            IArticleListItemModelFactory articleListableFactory,
            PreferencesUtil preferencesUtil)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
            TextTranslator = textTranslator;
            ArticleSearch = articleSearch;
            ArticleListableFactory = articleListableFactory;
            PreferencesUtil = preferencesUtil;
        }

        /// <summary>
        /// The personalized email request
        /// </summary>
        private PersonalizedEmailRequest _personalizedEmailRequest;

        private DateTime? _searchStartDate;

        private DateTime? _searchEndDate;

        /// <summary>
        /// Gets or sets the personalized email request.
        /// </summary>
        /// <value>
        /// The personalized email request.
        /// </value>
        public PersonalizedEmailRequest PersonalizedEmailRequest
        {
            get
            {
                return _personalizedEmailRequest;
            }
            set
            {
                _personalizedEmailRequest = value;
            }
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
            try
            {
                SetArticleSearchDateRange();
                IList<Channel> channels = new List<Channel>();

                if (_personalizedEmailRequest != null && !string.IsNullOrWhiteSpace(_personalizedEmailRequest.Value6__c))
                {
                    var userPreferences = JsonConvert.DeserializeObject<UserPreferences>(_personalizedEmailRequest.Value6__c.Replace("[CDATA[", "").Replace("]]", ""));

                    if (userPreferences != null && userPreferences.PreferredChannels != null
                       && userPreferences.PreferredChannels.Any())
                    {
                        channels = userPreferences.PreferredChannels.OrderBy(channel => channel.ChannelOrder).ToList();
                        Stopwatch sw = Stopwatch.StartNew();
                        foreach (Channel channel in channels)
                        {
                            CreateSections(channel, sections, userPreferences.IsChannelLevel, userPreferences.IsNewUser);
                        }
                        StringExtensions.WriteSitecoreLogs("Time taken to Create Sections for Personalized Email Body", sw, "CreateSections");
                    }

                }
                else
                {
                    Stopwatch sw = Stopwatch.StartNew();
                    channels = PreferencesUtil.GetAllChannels(false);
                    StringExtensions.WriteSitecoreLogs("Time taken to Get All Channels", sw, "GetAllChannels");
                    sw = Stopwatch.StartNew();
                    foreach (Channel channel in channels)
                    {
                        CreateSections(channel, sections, true, false);
                    }
                    StringExtensions.WriteSitecoreLogs("Time taken to Create Sections of all channels", sw, "GetAllChannels");
                }

                return sections;
            }
            catch (Exception e)
            {
                Log.Error("Could Not Generate Personalized Email Sections", e, this);
                return sections;
            }

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
            var channelPageItem = GlobalService.GetItem<IChannel_Page>(channel.ChannelId ?? PreferencesUtil.GetPreferenceId(channel.ChannelCode));
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
                        topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId ?? PreferencesUtil.GetPreferenceId(topic.TopicCode));
                        if (topicItem != null)
                        {
                            taxonomyId = topicItem != null && topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                            if (!string.IsNullOrWhiteSpace(taxonomyId))
                                sec.TaxonomyIds.Add(taxonomyId);
                        }
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
            filter.PageSize = !string.IsNullOrWhiteSpace(_personalizedEmailRequest.ArticleLimit) ?
               Convert.ToInt32(_personalizedEmailRequest.ArticleLimit) : SiteRootContext.Item.Max_Number_Of_Articles_Per_Section;
            filter.TaxonomyIds.AddRange(sec.TaxonomyIds.Select(taxonomy => new Guid(taxonomy)));
            var results = ArticleSearch.PersonalizedSearch(filter, _searchStartDate, _searchEndDate);
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
                topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId ?? PreferencesUtil.GetPreferenceId(topic.TopicCode));
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

        private void SetArticleSearchDateRange()
        {
            var searchInterval = !string.IsNullOrWhiteSpace(_personalizedEmailRequest.Time)
               ? _personalizedEmailRequest.Time : "24";
            _personalizedEmailRequest.DateFilter = !string.IsNullOrWhiteSpace(_personalizedEmailRequest.DateFilter)
                ? _personalizedEmailRequest.DateFilter : "hour";
            int timeQuery = -1 * Convert.ToInt32(searchInterval);
            switch (_personalizedEmailRequest.DateFilter.ToLower())
            {
                case "hour":
                    _searchStartDate = DateTime.Now.AddHours(timeQuery);
                    break;
                case "day":
                    _searchStartDate = DateTime.Now.AddDays(timeQuery);
                    break;
                case "year":
                    _searchStartDate = DateTime.Now.AddYears(timeQuery);
                    break;
                case "month":
                    _searchStartDate = DateTime.Now.AddMonths(timeQuery);
                    break;
                case "week":
                    _searchStartDate = DateTime.Now.AddDays(timeQuery * 7);
                    break;
                default:
                    _searchStartDate = DateTime.Now.AddHours(timeQuery);
                    break;
            }

            if (_searchStartDate != null && _searchStartDate > DateTime.MinValue)
            {
                _searchEndDate = DateTime.Now;
            }
        }
    }
}