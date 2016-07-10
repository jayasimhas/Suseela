using System;
using System.Linq;
using Autofac;
using Autofac.Features.OwnedInstances;
using Glass.Mapper.Sc;
using Informa.Library.Article.Search;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Util;
using Informa.Library.Utilities.Extensions;

namespace Informa.Library.Article.Service
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class ArticleSearchService : IArticleSearchService
    {
        public static IArticleSearchService Instance => AutofacConfig.ServiceLocator.Resolve<Owned<IArticleSearchService>>().Value;

        private readonly IDependencies _;

        public ArticleSearchService(IDependencies _)
        {
            if (_ == null) throw new ArgumentNullException(nameof(_));
            this._ = _;
        }

        public ArticleItem GetArticleByNumber(string number)
        {
            using (var serviceScope = _.ServiceFactory(Constants.MasterDb))
            {
                using (var searchScope = _.SearchFactory())
                {
                    var service = serviceScope.Value;
                    var search = searchScope.Value;

                    IArticleSearchFilter filter = search.CreateFilter();
                    filter.ArticleNumbers = number.SingleToList();
                    var results = search.SearchCustomDatabase(filter, Constants.MasterDb);
                    if (results.Articles.Any())
                    {
                        var foundArticle = results.Articles.FirstOrDefault();
                        if (foundArticle != null)
                        {
                            return service.GetItem<ArticleItem>(foundArticle._Id);
                        }
                    }
                    return null;
                }
            }
        }

        [AutowireService(true)]
        public interface IDependencies
        {
            Func<string, Owned<ISitecoreService>> ServiceFactory { get; }
            Func<Owned<IArticleSearch>> SearchFactory { get; }
        }
    }
}
