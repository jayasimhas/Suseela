using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Utilities.Settings;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Core.Caching;
using Sitecore;

namespace Informa.Library.Article.Companies
{
	public interface IRelatedDealsService
	{
		IEnumerable<Link> GetRelatedDeals(IArticle article);
	}

	[AutowireService(LifetimeScope.PerScope)]
	public class RelatedDealsService : IRelatedDealsService
	{
		private readonly IDCDReader _reader;
		private readonly string _oldDealsUrl;
	    private readonly ICacheProvider _cacheProvider;

		public RelatedDealsService(
            IDCDReader reader, 
            ISiteSettings siteSettings,
            ICacheProvider cacheProvider)
		{
			_reader = reader;
			_oldDealsUrl = siteSettings.OldDealsUrl;
		    _cacheProvider = cacheProvider;

		}

	    public IEnumerable<Link> GetRelatedDeals(IArticle article)
	    {
            string cacheKey = $"{nameof(RelatedDealsService)}-RelatedDeals-{article._Id}";
            return (Context.PageMode.IsNormal)
                ? _cacheProvider.GetFromCache(cacheKey, () => BuildRelatedDeals(article))
                : BuildRelatedDeals(article);
        }

        private IEnumerable<Link> BuildRelatedDeals(IArticle article) {

            var dealIds = article?.Referenced_Deals?.Split(',') ?? new string[0];

			return dealIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetDealByRecordNumber(id)).Select(c => new Link
			{
				Text = c.Title,
				Url = string.Format(_oldDealsUrl, c.RecordNumber)
			});
		}
	}
}