using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Services.Sitemap;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class ArticleAdViewModel : GlassViewModel<IArticle>
    {
        protected readonly ISitecoreService SitecoreService;
        protected readonly ITextTranslator TextTranslator;

        public ArticleAdViewModel(
            ISitecoreService sitecoreService,
            ITextTranslator textTranslator)
        {
            SitecoreService = sitecoreService;
            TextTranslator = textTranslator;
        }

        public string MediumAdID => SitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode)?.Global_Article_Medium_Slot_ID ?? string.Empty;
        public string FilmstripAdID => SitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode)?.Global_Article_Filmstrip_Slot_ID ?? string.Empty;
        public string AdZone => SitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode)?.Global_Article_Ad_Zone ?? string.Empty;
        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
    }
}