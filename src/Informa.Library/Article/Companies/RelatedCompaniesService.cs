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
	[AutowireService(LifetimeScope.PerScope)]
	public class RelatedCompaniesService : IRelatedCompaniesService
	{
		private readonly IDCDReader _reader;
		private readonly string _oldCompaniesUrl;
	    private readonly ICacheProvider _cacheProvider;

		public RelatedCompaniesService(
            IDCDReader reader, 
            ISiteSettings siteSettings,
            ICacheProvider cacheProvider)
		{
			_reader = reader;
			_oldCompaniesUrl = siteSettings.OldCompaniesUrl;
		    _cacheProvider = cacheProvider;

		}
        
        public IEnumerable<Link> GetRelatedCompanyLinks(IArticle article)
	    {
            string cacheKey = $"{nameof(RelatedCompaniesService)}-RelatedCompanyLinks-{article._Id}";
            return (Context.PageMode.IsNormal)
                ? _cacheProvider.GetFromCache(cacheKey, () => BuildRelatedCompanyLinks(article))
                : BuildRelatedCompanyLinks(article);
        }

        private IEnumerable<Link> BuildRelatedCompanyLinks(IArticle article) { 

            var companyIds = article?.Referenced_Companies?.Split(',') ?? new string[0];

			return companyIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetCompanyByRecordNumber(id)).Select(c => new Link
			{
				Text = c.Title,
				Url = string.Format(_oldCompaniesUrl, c.RecordNumber)
			});
		}
	}
}