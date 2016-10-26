using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Article.Companies
{
    [AutowireService(LifetimeScope.PerScope)]
    public class RelatedCompaniesService : IRelatedCompaniesService
    {
        private readonly IDCDReader _reader;
        protected readonly ISiteRootContext SiteRootContext;

        public RelatedCompaniesService(ISiteRootContext siteRootContext, IDCDReader reader)
        {
            _reader = reader;
            SiteRootContext = siteRootContext;
        }

        public IEnumerable<Link> GetRelatedCompanyLinks(IArticle article)
        {
            var companyIds = article?.Referenced_Companies?.Split(',') ?? new string[0];

            return companyIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetCompanyByRecordNumber(id)).Where(c => c != null).Select(c => new Link
            {
                Text = c.Title,
                Url = $"{SiteRootContext.Item?._Url}companies/{c.RecordNumber}"
            });
        }
    }
}