using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class ArticleAdViewModel : GlassViewModel<IArticle>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;

        public ArticleAdViewModel(
						ISiteRootContext siteRootContext,
            ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
        }

        public string MediumAdID => (!string.IsNullOrEmpty(GlassModel?.Article_Medium_Slot_ID)) 
            ? GlassModel.Article_Medium_Slot_ID 
            : SiteRootContext.Item?.Global_Article_Medium_Slot_ID ?? string.Empty;
        public string FilmstripAdID => (!string.IsNullOrEmpty(GlassModel?.Article_Filmstrip_Slot_ID))
            ? GlassModel.Article_Filmstrip_Slot_ID
            : SiteRootContext.Item?.Global_Article_Filmstrip_Slot_ID ?? string.Empty;
        public string AdZone => SiteRootContext.Item?.Global_Article_Ad_Zone ?? string.Empty;
        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
    }
}