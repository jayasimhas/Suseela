using System;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.Extensions;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Glass.Mapper.Sc;
using Informa.Library.Site;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;

namespace Informa.Web.ViewModels
{
    public class CompanyLatestNewsViewModel : GlassViewModel<ICompany_Detail_Page>
    {
        protected readonly IArticleSearch ArticleSearch;
        protected readonly ITextTranslator TextTranslator;
        public ISitecoreContext SitecoreContext;
        ISiteRootContext RootContext;

        public CompanyLatestNewsViewModel(
            IArticleSearch articleSearch, ITextTranslator textTranslator, ISitecoreContext sitecoreContext, ISiteRootContext rootContext)
        {
            ArticleSearch = articleSearch;
            TextTranslator = textTranslator;
            SitecoreContext = sitecoreContext;
            RootContext = rootContext;
            ItemsToDisplay = 4;

        }    

        public IEnumerable<IArticle> News { get; set; }

        IEnumerable<string> publicationNames { get; set; }
        IEnumerable<Guid> subjectId { get; set; }
    
        public int ItemsToDisplay { get; set; }

        public IEnumerable<IArticle> CompanyNews => GetLatestNewsForCompanies();

        public IEnumerable<ITaxonomy_Item> Taxonomies => GlassModel?.LongIds;
     
        public IArticle CompanyManualArticlePage => GlassModel?.Company_News_Article ;

        public string CompanyNewsSectionName => TextTranslator.Translate("Company.News.Section.Name");

        public string CompanyNewsSummary => TrimCompanyNewsSummary();

        public string CompanyName => GlassModel?.Companyname;

        /// <summary>
        /// To Trim Company News Summary
        /// </summary>
        /// <returns></returns>
        private string TrimCompanyNewsSummary()
        {          
            if (CompanyManualArticlePage.Summary.ToCharArray().Count() > 93)
            {
             return   CompanyManualArticlePage.Summary.Substring(0, 93) + "...";
            }
            else
            {
             return   CompanyManualArticlePage.Summary;                
            }
        }

        /// <summary>
        /// To find latest news for companies
        /// </summary>
        /// <returns>Article Collection</returns>
        private IEnumerable<IArticle> GetLatestNewsForCompanies()
        {
            publicationNames = new List<string>() { RootContext.Item.Publication_Name };
            subjectId = Taxonomies.Select(x => x._Id);        

            var filter = ArticleSearch.CreateFilter();
        
            filter.Page = 1;
            filter.PageSize = ItemsToDisplay;

            if (subjectId != null) filter.TaxonomyIds.AddRange(subjectId);
            if (publicationNames != null) filter.PublicationNames.AddRange(publicationNames);

            var results = ArticleSearch.ArticlesRelatedToCompany(filter);
            
            var articles = results.Articles.Where(a => a != null);
            return articles;            

        }


    }

}