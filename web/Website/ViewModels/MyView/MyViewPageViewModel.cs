using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.UserPreference;
using Informa.Web.Models;
using Jabberwocky.Glass.Autofac.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Web.ViewModels.MyView
{
    [AutowireService]
    public class MyViewPageViewModel : IMyViewPageViewModel
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserPreferenceContext UserPreferences;
        protected readonly IGlobalSitecoreService GlobalService;

        public MyViewPageViewModel(
                        ISiteRootContext siteRootContext,
            IUserPreferenceContext userPreferences,
            IGlobalSitecoreService globalService)
        {
            SiteRootContext = siteRootContext;
            UserPreferences = userPreferences;
            GlobalService = globalService;
        }

        public int InitialLaodSectionsCount => SiteRootContext.Item.Initial_Laod_Sections_Count;

        public int ItemsPerSection => SiteRootContext.Item.Items_Per_Section;

        public string MyViewSettingsPageUrl => SiteRootContext.Item.MyView_Settings_Page?._Url;

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
                    CreateSections(channel, sections, UserPreferences.Preferences.IsChannelLevel);
                }
            }

            return sections;
        }

        private void CreateSections(Channel channel, List<Section> sections,bool IsChannelLevel)
        {
            bool channelStatus = channel.IsFollowing;
            Section sec;
            IList<Topic> topics = new List<Topic>();
            Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic topicItem;
            Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page channelPageItem;
            string taxonomyId = string.Empty;
            if (channel.Topics != null && channel.Topics.Any())
            {
                channelStatus = channel.IsFollowing || channel.Topics.Any(topic => topic.IsFollowing);
                topics = channel.Topics.Where(topic => topic.IsFollowing).OrderBy(topic => topic.TopicOrder).ToList();
            }

            if (channelStatus && !IsChannelLevel)
            {
                foreach (Topic topic in topics)
                {
                    topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId);
                    if (topicItem != null)
                    {
                        sec = new Section();
                        sec.TaxonomyIds = new List<string>();
                        sec.ChannelName = topicItem?.Navigation_Text;
                        sec.ChannelId = topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                        if (!string.IsNullOrWhiteSpace(sec.ChannelId))
                            sec.TaxonomyIds.Add(sec.ChannelId);
                        sections.Add(sec);
                    }
                }
            }
            else
            {
                channelPageItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.IChannel_Page>(channel.ChannelId);
                if (channelPageItem != null)
                {
                    sec = new Section();
                    sec.TaxonomyIds = new List<string>();
                    sec.ChannelName = channelPageItem?.Display_Text;
                    sec.ChannelId    = channelPageItem.Taxonomies != null && channelPageItem.Taxonomies.Any() ? channelPageItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                    if (!string.IsNullOrWhiteSpace(sec.ChannelId))
                        sec.TaxonomyIds.Add(sec.ChannelId);
                    foreach (Topic topic in topics)
                    {
                        topicItem = GlobalService.GetItem<Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Topics.ITopic>(topic.TopicId);
                        taxonomyId = topicItem.Taxonomies != null && topicItem.Taxonomies.Any() ? topicItem?.Taxonomies.FirstOrDefault()._Id.ToString() : string.Empty;
                        if (!string.IsNullOrWhiteSpace(taxonomyId))
                            sec.TaxonomyIds.Add(taxonomyId);
                    }
                    sections.Add(sec);
                }
            }
        }
    }
}