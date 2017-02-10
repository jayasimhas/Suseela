using Elsevier.Library.Interfaces.Factory;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.Extensions;
using Informa.Web.ViewModels.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.SponsoredContent
{
    public class SponsoredContent : ISponsoredContent
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly IArticleListItemModelFactory ArticleListableFactory;

        public IListableViewModel ListableItems { get; set; }

        public ArticleViewType ArticleViewType { get; set; }        
        public SponsoredContent(IArticleSearch articleSearch)
        {
            ArticleSearch = articleSearch;
        }
        public void GetSponsoreDetials()
        {
            Guid sponsoredTaxonomy = new Guid(Sitecore.Configuration.Settings.GetSetting("Sponsored.Content.Taxonomy"));
            var filter = ArticleSearch.CreateFilter();

            filter.Page = 1;
            filter.PageSize = 100;
            if (sponsoredTaxonomy != null) filter.TaxonomyIds.Add(sponsoredTaxonomy);

            var results = ArticleSearch.Search(filter);
            var articles =
                results.Articles.Where(a => a != null)
                    .Select(a => ArticleListableFactory.Create(a).Alter(l => l.DisplayImage = false));            
        }
    }
}