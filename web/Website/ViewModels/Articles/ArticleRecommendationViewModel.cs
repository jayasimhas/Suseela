using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.Site;
using Informa.Library.Globalization;
using Jabberwocky.Glass.Models;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Library.User.Authentication;
using Informa.Library.User.UserPreference;
using Informa.Library.Services.Global;
using Informa.Web.Models;
using Newtonsoft.Json;
using Informa.Library.User.Entitlement;

namespace Informa.Web.ViewModels.Articles
{
    public class ArticleRecommendationViewModel : GlassViewModel<IArticle>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserPreferenceContext UserPreferencesContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IUserEntitlementsContext UserEntitlementsContext;


        public ArticleRecommendationViewModel(ISiteRootContext siteRootContext,
            ITextTranslator textTranslator, 
            IAuthenticatedUserContext authenticatedUserContext, 
            IUserPreferenceContext userPreferences, 
            IGlobalSitecoreService globalService,
            IUserEntitlementsContext userEntitlementsContext)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            AuthenticatedUserContext = authenticatedUserContext;
            UserPreferencesContext = userPreferences;
            GlobalService = globalService;
            UserEntitlementsContext = userEntitlementsContext;

        }
        public string PublicationName => SiteRootContext.Item.Publication_Name;
        public string PublicationCode => SiteRootContext.Item.Publication_Code;

        public string EditorPicksText => TextTranslator.Translate("Article.EditorsPicks");
        public string WhatToReadNextText => TextTranslator.Translate("Article.WhatToReadNext");
        public string SuggestedForYouText => TextTranslator.Translate("Article.Suggestedforyou");
        public bool IsGlobalToggleEnabled => SiteRootContext.Item.Enable_MyView_Toggle;
        public IEnumerable<IArticle> EditorsPicks
        {
            get
            {
                var editorsPickList = GlassModel?.Editors_Picks.Select(x => (IArticle)x);
                return editorsPickList;
            }
        }
        public string TaxonomyItems
        {
            get
            {
                var taxonomyIds = string.Join(",", GlassModel.Taxonomies.Select(i => $"'{i._Id.ToString()}'"));
                return taxonomyIds;
            }
        }
    
        public IEnumerable<IEntitlement> Entitlements => UserEntitlementsContext.Entitlements;

        public string EntitlementIDs => GetOpportunityIds();
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        public bool UserPreferencesExists => (UserPreferencesContext.Preferences != null && UserPreferencesContext.Preferences.PreferredChannels != null && UserPreferencesContext.Preferences.PreferredChannels.Any());

        public bool IsFollowingAnyItem => UserPreferencesContext.Preferences != null &&
            UserPreferencesContext.Preferences.PreferredChannels != null &&
            UserPreferencesContext.Preferences.PreferredChannels.Any() &&
            ((UserPreferencesContext.Preferences.IsNewUser &&
            UserPreferencesContext.Preferences.PreferredChannels.Any(ch => ch.IsFollowing)) ||
            (!UserPreferencesContext.Preferences.IsNewUser &&
            UserPreferencesContext.Preferences.PreferredChannels.Any(ch => ch.Topics != null
            && ch.Topics.Any() && ch.Topics.Any(tp => tp.IsFollowing))));

        
        public string PreferedTaxonomies => GetPreferedTaxonomyIds();
        /// <summary>
        /// Gets the json data.
        /// </summary>
        /// <returns>Json string of data</returns>
        private string GetPreferedTaxonomyIds()
        {
            IList<Section> Sections = GetSections();
            var taxnomyids = string.Join(",", Sections.Select(i => $"'{i.TaxonomyIds.ElementAt(0)}'"));
            return taxnomyids;
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of my view page scetions</returns>
        private IList<Section> GetSections()
        {
            var sections = new List<Section>();

            Section sec = new Section();
            sec.TaxonomyIds = new List<string>() { "{1D9DF21A-6D1C-4313-8BAE-DE880FDC9C3B}" };
            sec.ChannelName = "channel1";
            sec.ChannelId = "ch1";
            sections.Add(sec);
            Section sec2 = new Section();
            sec2.TaxonomyIds = new List<string>() { "{9CA4BFEE-8798-4D39-A2E8-5BEF5F3DEBAB}" };
            sec2.ChannelName = "channel2";
            sec2.ChannelId = "ch2";
            sections.Add(sec2);
            Section sec3 = new Section();
            sec3.TaxonomyIds = new List<string>() { "{8115E430-0419-4B6B-B6E6-5CCEA59705FB}" };
            sec3.ChannelName = "channel3";
            sec3.ChannelId = "ch3";
            sections.Add(sec3);
            Section sec4 = new Section();
            sec4.TaxonomyIds = new List<string>() { "{59163F1F-D047-46D2-9F51-BAA597DC0BB9}" };
            sec4.ChannelName = "channel4";
            sec4.ChannelId = "ch4";
            sections.Add(sec4);
            return sections;

            if (UserPreferencesContext.Preferences != null && UserPreferencesContext.Preferences.PreferredChannels != null
               && UserPreferencesContext.Preferences.PreferredChannels.Any())
            {

                var channels = UserPreferencesContext.Preferences.PreferredChannels.OrderBy(channel => channel.ChannelOrder).ToList(); ;
                foreach (Channel channel in channels)
                {
                    CreateSections(channel, sections, UserPreferencesContext.Preferences.IsChannelLevel, UserPreferencesContext.Preferences.IsNewUser);
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
        private void CreateSections(Channel channel, List<Section> sections, bool isChannelLevel, bool isNewUser)
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
        private void CreateSectionsFromChannels(Channel channel, List<Section> sections, bool isNewUser, ref IList<Topic> topics)
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
                sections.Add(sec);
            }
        }

        /// <summary>
        /// Creates the sections from topics.
        /// </summary>
        /// <param name="sections">The sections.</param>
        /// <param name="topics">The topics.</param>
        private void CreateSectionsFromTopics(List<Section> sections, IList<Topic> topics)
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

        private string GetOpportunityIds()
        {
            return "'1d9df21a-6d1c-4313-8bae-de880fdc9c3b','9ca4bfee-8798-4d39-a2e8-5bef5f3debab','8115e430-0419-4b6b-b6e6-5ccea59705fb','59163f1f-d047-46d2-9f51-baa597dc0bb9'";
            var UserEntitlements = UserEntitlementsContext.Entitlements;
            UserEntitlements = UserEntitlements.Where(i => i.ProductCode.ToLower() == PublicationCode.ToLower());
            var ids = string.Join(",", UserEntitlements.Select(i => $"'{i.OpportunityId}'"));
            return string.IsNullOrWhiteSpace(ids) ? string.Empty : ids;
        }

    }
}