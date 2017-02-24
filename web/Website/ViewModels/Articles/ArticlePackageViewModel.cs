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
using Sitecore.Data.Items;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Folders;

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
        protected readonly IArticleListItemModelFactory ArticleListableFactory;

        public ArticlePackageViewModel(IGlassBase datasource,
            IRenderingContextService renderingParametersService, ISiteRootContext siteRootContext,ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext, IArticleListItemModelFactory articleListableFactory)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            PackageSettings = renderingParametersService.GetCurrentRenderingParameters<IPackage_Settings>();
            _logger = logger;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
            ArticleListableFactory = articleListableFactory;
        }

        public bool IsArticlePage => GlassModel is IArticle;
        public IEnumerable<IArticle_Package> SelectedPackages => GetSelectedPackages();
        public bool IsFullWidth => (PackageSettings != null) ? PackageSettings.IsFullWidth : false;
        Guid curItemID => GlassModel._Id;
        /// <summary>
        /// Get selected packages
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IArticle_Package> GetSelectedPackages()
        {
            if(PackageSettings != null && PackageSettings.Select_Package.Any())
            {
                var selectedPackages = PackageSettings.Select_Package.OfType<IArticle_Package>()?.Where(p => p.IsInActive.Equals(false));
                return selectedPackages;
            }
            return Enumerable.Empty<IArticle_Package>();
        }
        /// <summary>
        /// Get selected articles
        /// </summary>
        /// <param name="Package"></param>
        /// <returns></returns>
        public IEnumerable<IListableViewModel> GetPackageArticles(IArticle_Package Package)
        {
            try
            {
                IEnumerable<IListableViewModel> listablePackageArticles;
                if (Package != null && Package.Package_Articles.Any())
                {
                    var packageArticles = Package.Package_Articles.OfType<IArticle>()?.Where(p => p != null);
                    if (IsArticlePage && packageArticles != null && packageArticles.Any())
                    {
                        packageArticles = packageArticles.Where(i => !i._Id.ToString().Equals(curItemID.ToString(), StringComparison.InvariantCultureIgnoreCase));
                    }
                    if(packageArticles != null && packageArticles.Any())
                    {
                         listablePackageArticles = packageArticles.Select(a => ArticleListableFactory.Create(a));
                        return listablePackageArticles;
                    }
                    
                }
                return Enumerable.Empty<IListableViewModel>();
            }
            catch (Exception ex)
            {
                _logger.Error("Error Finding the package articles", ex);
                return Enumerable.Empty<IListableViewModel>();
            }
        }
        /// <summary>
        /// Get package referrers by link database
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IArticle_Package> GetPackageReferrers()
        {
           
            Item curItem = GlobalService.GetItem<Item>(curItemID);
            if (curItem == null) return null;
            IEnumerable<IArticle_Package> articlePackage;
            var links = Sitecore.Globals.LinkDatabase.GetReferrers(curItem);
            if (links == null)
                return null;
            var linkedItems = links.Select(i => i.GetSourceItem()).Where(i => i != null && !i.Name.Equals("__standard values", StringComparison.InvariantCultureIgnoreCase));
            if (linkedItems != null && linkedItems.Any())
                linkedItems = linkedItems.Where(i => i.TemplateID.ToString().Equals(Constants.PackageTemplateID, StringComparison.InvariantCultureIgnoreCase));
            if(linkedItems != null && linkedItems.Any())
            {
                articlePackage = linkedItems.OfType<IArticle_Package>()?.Where(p => p.IsInActive.Equals(false));
                return articlePackage;
            }
            return null;
        }
        /// <summary>
        /// Get package referrers by loop
        /// </summary>
        /// <returns></returns>
        public IEnumerable<IArticle_Package> GetPackageReferrersbyLoop()
        {
            try
            {
               Item curItem = GlobalService.GetItem<Item>(curItemID);
                if (curItem == null) return null;
                //IEnumerable<IArticle_Package> articlePackage;
                //Get the packages root item
                var verticalGlobal = GlobalService.GetVerticalRootAncestor(GlassModel._Id)?._ChildrenWithInferType.OfType<IEnvironment_Global_Root>().FirstOrDefault();
                var packageFolder = verticalGlobal._ChildrenWithInferType.OfType<IPackage_Folder>().FirstOrDefault();
                if (packageFolder == null) return null;
                var articlePackages = packageFolder._ChildrenWithInferType.OfType<IArticle_Package>()?.Where(p => p.IsInActive.Equals(false));
                var referedPackage = articlePackages.Where(p => p.Package_Articles.Any(i => i._Id.Equals(curItemID)));
                return referedPackage;
            }
            catch(Exception ex)
            {
                _logger.Error("Error Finding the package reference", ex);
                return null;
            }
        }


    }
}