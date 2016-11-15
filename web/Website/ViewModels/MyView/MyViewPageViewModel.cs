using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Library.User.UserPreference;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels.MyView
{
    public class MyViewPageViewModel : GlassViewModel<IGeneral_Content_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserPreferenceContext UserPreferences;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ITextTranslator TextTranslator;
        public readonly ICallToActionViewModel CallToActionViewModel;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

        public MyViewPageViewModel(
                        ISiteRootContext siteRootContext,
            IUserPreferenceContext userPreferences,
            IGlobalSitecoreService globalService,
            ITextTranslator textTranslator,
            ICallToActionViewModel callToActionViewModel,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            SiteRootContext = siteRootContext;
            UserPreferences = userPreferences;
            GlobalService = globalService;
            TextTranslator = textTranslator;
            CallToActionViewModel = callToActionViewModel;
            AuthenticatedUserContext = authenticatedUserContext;
        }

        /// <summary>
        /// Gets a value indicating whether user is authenticated.
        /// </summary>
        /// <value>
        /// <c>true</c> if user is authenticated; otherwise, <c>false</c>.
        /// </value>
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;

        /// <summary>
        /// Gets the initial laod sections count.
        /// </summary>
        /// <value>
        /// The initial laod sections count.
        /// </value>
        public int InitialLaodSectionsCount => SiteRootContext.Item.Initial_Laod_Sections_Count;

        /// <summary>
        /// Gets the page heading.
        /// </summary>
        /// <value>
        /// The page heading.
        /// </value>
        public string PageHeading => GlassModel?.Title;

        /// <summary>
        /// Gets the page description.
        /// </summary>
        /// <value>
        /// The page description.
        /// </value>
        public string PageDescription => GlassModel?.Body;

        /// <summary>
        /// Gets the edit my view button lable text.
        /// </summary>
        /// <value>
        /// The edit my view button lable text.
        /// </value>
        public string EditMyViewButtonLableText => TextTranslator.Translate("MyView.EditMyViewButtonLableText");

        /// <summary>
        /// Gets the items per section.
        /// </summary>
        /// <value>
        /// The items per section.
        /// </value>
        public int ItemsPerSection => SiteRootContext.Item.Items_Per_Section;

        /// <summary>
        /// Gets my view settings page URL.
        /// </summary>
        /// <value>
        /// My view settings page URL.
        /// </value>
        public string MyViewSettingsPageUrl => SiteRootContext.Item.MyView_Settings_Page?._Url;

        /// <summary>
        /// Gets a value indicating whether user is following any item.
        /// </summary>
        /// <value>
        /// <c>true</c> if user is following any item; otherwise, <c>false</c>.
        /// </value>
        public bool IsFollowingAnyItem => UserPreferences.Preferences != null &&
            UserPreferences.Preferences.PreferredChannels != null &&
            UserPreferences.Preferences.PreferredChannels.Any() &&
            ((UserPreferences.Preferences.IsNewUser &&
            UserPreferences.Preferences.PreferredChannels.Any(ch => ch.IsFollowing)) ||
            (!UserPreferences.Preferences.IsNewUser &&
            UserPreferences.Preferences.PreferredChannels.Any(ch => ch.Topics != null
            && ch.Topics.Any() && ch.Topics.Any(tp => tp.IsFollowing))));


        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <value>
        /// The sections.
        /// </value>
        public IList<Section> Sections => GetSections();

        /// <summary>
        /// Gets the json data.
        /// </summary>
        /// <value>
        /// The json data.
        /// </value>
        public string JSONData => GetJSONData();

        /// <summary>
        /// Gets or sets the article identifier.
        /// </summary>
        /// <value>
        /// The article identifier.
        /// </value>
        public static string ArticleId { get; set; }

        /// <summary>
        /// Gets or sets the updated article identifier.
        /// </summary>
        /// <value>
        /// The updated article identifier.
        /// </value>
        public string UpdatedArticleId
        {
            get
            {
                return ArticleId;
            }
            set
            {

            }
        }

        /// <summary>
        /// Gets the json data.
        /// </summary>
        /// <returns>Json string of data</returns>
        private string GetJSONData()
        {
            var jsonObject = new
            {
                EditMyViewButtonLableText = EditMyViewButtonLableText,
                MyViewSettingsPageLink = MyViewSettingsPageUrl,
                DefaultSectionLoadCount = InitialLaodSectionsCount,
                PerSectionItemCount = ItemsPerSection,
                Sections = Sections
            };
            return JsonConvert.SerializeObject(jsonObject);
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        /// <returns>List of my view page scetions</returns>
        private IList<Section> GetSections()
        {
            var sections = new List<Section>();

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
                        taxonomyId = topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
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
    }
}