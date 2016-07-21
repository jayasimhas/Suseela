using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Library.Utilities.Settings;
using Informa.Model.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;


namespace Informa.Library.Article.Companies
{
	[AutowireService(LifetimeScope.PerScope)]
	public class RelatedDealsService : IRelatedDealsService
	{
		private readonly IDCDReader _reader;
        protected readonly ISiteRootContext SiteRootContext;

        public RelatedDealsService(ISiteRootContext siteRootContext,IDCDReader reader)
		{
			_reader = reader;
            SiteRootContext = siteRootContext;
        }

		public IEnumerable<Link> GetRelatedDeals(IArticle article)
		{
			var dealIds = article?.Referenced_Deals?.Split(',') ?? new string[0];

			return dealIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetDealByRecordNumber(id)).Select(c => new Link
			{
				Text = c.Title,
				Url = $"{SiteRootContext.Item?._Url}deals/{c.RecordNumber}"
            });
		}
	}
}