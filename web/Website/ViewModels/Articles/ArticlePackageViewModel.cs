using System;
using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Jabberwocky.Glass.Models;
using log4net;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels.Articles
{
    public class ArticlePackageViewModel : GlassViewModel<IGlassBase>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IPackage_Settings PackageSettings;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISitecoreContext SitecoreContext;

        public ArticlePackageViewModel(IGlassBase datasource,
            IRenderingContextService renderingParametersService, ISiteRootContext siteRootContext,ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            PackageSettings = renderingParametersService.GetCurrentRenderingParameters<IPackage_Settings>();
            _logger = logger;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
        }

        public IEnumerable<IArticle_Package> SelectedPackages => GetSelectedPackages();
        public bool HideImage => (PackageSettings != null) ? PackageSettings.Hide_Image : false; 
        
        private IEnumerable<IArticle_Package> GetSelectedPackages()
        {
            if(PackageSettings != null && PackageSettings.Select_Package.Any())
            {
                var selectedPackages = PackageSettings.Select_Package.OfType<IArticle_Package>()?.Where(p => p.IsInActive.Equals(false));
                return selectedPackages;
            }
            return Enumerable.Empty<IArticle_Package>();
        }

        public IEnumerable<IArticle> GetPackageArticles(IArticle_Package Package)
        {
            if (Package != null && Package.Package_Articles.Any())
            {
                var packageArticles = Package.Package_Articles.OfType<IArticle>()?.Where(p => p != null);
                return packageArticles;
            }
            return Enumerable.Empty<IArticle>();
        }
       
       
    }
}