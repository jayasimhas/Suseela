using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.UserPreference;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Attributes;
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
        
        public MyViewPageViewModel(
                        ISiteRootContext siteRootContext,
            IUserPreferenceContext userPreferences,
            IGlobalSitecoreService globalService,
            ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            UserPreferences = userPreferences;
            GlobalService = globalService;
            TextTranslator = textTranslator;
        }

        public int InitialLaodSectionsCount => SiteRootContext.Item.Initial_Laod_Sections_Count;

        public string PageHeading => GlassModel?.Title;

        public string PageDescription => GlassModel?.Body;

        public string EditMyViewButtonLableText => TextTranslator.Translate("MyView.EditMyViewButtonLableText");

        public int ItemsPerSection => SiteRootContext.Item.Items_Per_Section;

        public string MyViewSettingsPageUrl => SiteRootContext.Item.MyView_Settings_Page?._Url;
        public static string ArticleId { get; set; }
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
        public IList<Section> Sections => GetSections();

        public string JSONData => GetJSONData();

        private string GetJSONData()
        {
            var jsonObject = new
            {
                MyViewSettingsPageLink = MyViewSettingsPageUrl,
                DefaultSectionLoadCount = InitialLaodSectionsCount,
                PerSectionItemCount = ItemsPerSection,
                Sections = Sections
            };
            return JsonConvert.SerializeObject(jsonObject);
        }

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
                taxonomyId = channelPageItem.Taxonomies != null && channelPageItem.Taxonomies.Any() ? channelPageItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                if (!string.IsNullOrWhiteSpace(taxonomyId))
                    sec.TaxonomyIds.Add(taxonomyId);
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