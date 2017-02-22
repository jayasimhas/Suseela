using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Sitecore.Data.Items;

namespace Informa.Web.ViewModels
{
    public class AdViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;

        public AdViewModel(
            ISiteRootContext siteRootContext,
            ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;

            
        }
        public void Init()
        {
            var articleModel = GlassModel as IArticle;
            AdComponentModel = GlassModel as IAdvertisement;
            if (articleModel != null)
            {
                RectangularAdZone = LeaderboardAdZone = FilmstripAdZone = 
                    SiteRootContext.Item?.Global_Article_Ad_Zone;
                FilmstripSlotId = articleModel.Article_Filmstrip_Slot_ID.NullIfNoContent() ??
                                  SiteRootContext.Item?.Global_Article_Filmstrip_Slot_ID;

                //ISW-338 Serving ads based on section taxonomy

                if (articleModel.Taxonomies != null)
                {
                    int taxonomyItemCount = 0;
                    foreach (ITaxonomy_Item item in articleModel.Taxonomies)
                    {
                        if (item != null && taxonomyItemCount < 3)
                        {
                            LeaderboardSlotId = string.IsNullOrEmpty(LeaderboardSlotId) && !string.IsNullOrEmpty(item.Leaderboard_Slot_ID) ? item.Leaderboard_Slot_ID : LeaderboardSlotId;
                            RectangularSlotId = string.IsNullOrEmpty(RectangularSlotId) && !string.IsNullOrEmpty(item.Rectangular_Slot_ID) ? item.Rectangular_Slot_ID : RectangularSlotId;
                            if (!string.IsNullOrEmpty(item.Leaderboard_Slot_ID) && !string.IsNullOrEmpty(item.Rectangular_Slot_ID))
                                break;
                            taxonomyItemCount++;                          
                        }
                    }
                }
                if (string.IsNullOrEmpty(LeaderboardSlotId))
                    LeaderboardSlotId = articleModel.Leaderboard_Slot_ID.NullIfNoContent() ??
                                        SiteRootContext.Item?.Global_Leaderboard_Slot_ID;
                if (string.IsNullOrEmpty(RectangularSlotId))
                    RectangularSlotId = articleModel.Article_Medium_Slot_ID.NullIfNoContent() ??
                                      SiteRootContext.Item?.Company_Rectangular_Slot_ID;
            } else if (AdComponentModel != null)
            {
                RectangularAdZone = LeaderboardAdZone = AdComponentModel.Zone;
                RectangularSlotId = LeaderboardSlotId = AdComponentModel.Slot_ID;
                IsEditable = true;
            }
            else if (GlassModel is ICompany_Page)
            {
                RectangularAdZone = SiteRootContext.Item?.Company_Rectangular_Ad_Zone;
                RectangularSlotId = SiteRootContext.Item?.Company_Rectangular_Slot_ID;
                LeaderboardAdZone = SiteRootContext.Item?.Company_Leaderboard_Ad_Zone;
                LeaderboardSlotId = SiteRootContext.Item?.Company_Leaderboard_Slot_ID;
            }
            else if (GlassModel is IDeal_Page)
            {
                RectangularAdZone = SiteRootContext.Item?.Deal_Rectangular_Ad_Zone;
                RectangularSlotId = SiteRootContext.Item?.Deal_Rectangular_Slot_ID;
            }
            else if (GlassModel is IAuthor_Page)
            {
                RectangularAdZone = SiteRootContext.Item?.Author_Rectangular_Ad_Zone;
                RectangularSlotId = SiteRootContext.Item?.Author_Rectangular_Slot_ID;
                LeaderboardAdZone = SiteRootContext.Item?.Author_Leaderboard_Ad_Zone;
                LeaderboardSlotId = SiteRootContext.Item?.Author_Leaderboard_Slot_ID;
            }
            else //Default to Global Article and Global Leaderboard
            {
                RectangularAdZone = SiteRootContext.Item?.Global_Article_Ad_Zone;
                RectangularSlotId = SiteRootContext.Item?.Global_Article_Medium_Slot_ID;
                LeaderboardAdZone = SiteRootContext.Item?.Global_Leaderboard_Ad_Zone;
                LeaderboardSlotId = SiteRootContext.Item?.Global_Leaderboard_Slot_ID;
            }
        }

        public IArticle ArticleModel => GlassModel as IArticle;
        

        public string MediumAdId => ArticleModel?.Article_Medium_Slot_ID.NullIfNoContent()
                                    ?? AdComponentModel?.Slot_ID.NullIfNoContent()
                                    ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                    ?? string.Empty;

        public string FilmstripAdId => ArticleModel?.Article_Filmstrip_Slot_ID.NullIfNoContent()
                                       ?? AdComponentModel?.Slot_ID.NullIfNoContent()
                                       ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                       ?? string.Empty;

        public string LeaderboardAdId => ArticleModel?.Leaderboard_Slot_ID.NullIfNoContent()
                                         ?? AdComponentModel?.Slot_ID.NullIfNoContent()
                                         ?? SiteRootContext.Item?.Global_Leaderboard_Slot_ID.NullIfNoContent()
                                         ?? string.Empty;

        public string SlotId => AdComponentModel?.Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Leaderboard_Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Article_Filmstrip_Slot_ID.NullIfNoContent()
                                ?? string.Empty;
            
        
        public string AdZone => SiteRootContext.Item?.Global_Article_Ad_Zone ?? string.Empty;
        public bool IsValidAd(string adId) => !string.IsNullOrEmpty(AdZone) && !string.IsNullOrEmpty(adId);

        public string RectangularAdZone { get; private set; }
        public string RectangularSlotId { get; private set; }
        public string LeaderboardAdZone { get; private set; }
        public string LeaderboardSlotId { get; private set; }
        public string FilmstripAdZone { get; private set; }
        public string FilmstripSlotId { get; private set; }

        public bool IsEditable { get; private set; }
        public IAdvertisement AdComponentModel { get; private set; }

        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
        
        public bool IsValidRectangularAd => !string.IsNullOrEmpty(RectangularAdZone) && !string.IsNullOrEmpty(RectangularSlotId);
        public bool IsValidLeaderboardAd => !string.IsNullOrEmpty(LeaderboardAdZone) && !string.IsNullOrEmpty(LeaderboardSlotId);
        public bool IsValidFilmstripAd => !string.IsNullOrEmpty(FilmstripAdZone) && !string.IsNullOrEmpty(FilmstripSlotId);
        
    }
}