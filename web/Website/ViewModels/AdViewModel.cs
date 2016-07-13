using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

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

        public IArticle ArticleModel => GlassModel as IArticle;
        public IAdvertisement AdModel => GlassModel as IAdvertisement;

        public string MediumAdId => ArticleModel?.Article_Medium_Slot_ID.NullIfNoContent()
                                    ?? AdModel?.Slot_ID.NullIfNoContent()
                                    ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                    ?? string.Empty;

        public string FilmstripAdId => ArticleModel?.Article_Filmstrip_Slot_ID.NullIfNoContent()
                                       ?? AdModel?.Slot_ID.NullIfNoContent()
                                       ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                       ?? string.Empty;

        public string LeaderboardAdId => ArticleModel?.Leaderboard_Slot_ID.NullIfNoContent()
                                         ?? AdModel?.Slot_ID.NullIfNoContent()
                                         ?? SiteRootContext.Item?.Global_Leaderboard_Slot_ID.NullIfNoContent()
                                         ?? string.Empty;

        public string SlotId => AdModel?.Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Leaderboard_Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Article_Medium_Slot_ID.NullIfNoContent()
                                ?? SiteRootContext.Item?.Global_Article_Filmstrip_Slot_ID.NullIfNoContent()
                                ?? string.Empty;
            
        
        public string AdZone => SiteRootContext.Item?.Global_Article_Ad_Zone ?? string.Empty;
        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
        
        public string AuthorLeaderboardAdZone => SiteRootContext.Item?.Author_Leaderboard_Author_Ad_Zone;
        public string AuthorLeaderboardSlotID => SiteRootContext.Item?.Author_Leaderboard_Author_Slot_ID;
        public string AuthorRectangularAdZone => SiteRootContext.Item?.Author_Rectangular_Ad_Zone;
        public string AuthorRectangularSlotID => SiteRootContext.Item?.Author_Rectangular_Slot_ID;

        public bool IsValidAd(string adId) => !string.IsNullOrEmpty(AdZone) && !string.IsNullOrEmpty(adId);
    }
}