using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
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

        public ArticleAdViewModel(ISitecoreService sitecoreService)
        {
            SitecoreService = sitecoreService;
        }

        public string MediumAdID => SitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode).Global_Article_Medium_Slot_ID;
        public string FilmstripAdID => SitecoreService.GetItem<ISite_Config>(Constants.ScripRootNode).Global_Article_Filmstrip_Slot_ID;
    }
}