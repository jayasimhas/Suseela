using System.Collections.Generic;
using System.Linq;
using Glass.Mapper.Sc.Fields;
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
		private readonly string _oldDealsUrl;
		
		public RelatedDealsService(
			IDCDReader reader,
			ISiteSettings siteSettings)
		{
			_reader = reader;
			_oldDealsUrl = siteSettings.OldDealsUrl;
		}

		public IEnumerable<Link> GetRelatedDeals(IArticle article)
		{
			var dealIds = article?.Referenced_Deals?.Split(',') ?? new string[0];

			return dealIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetDealByRecordNumber(id)).Select(c => new Link
			{
				Text = c.Title,
				Url = string.Format(_oldDealsUrl, c.RecordNumber)
			});
		}
	}
}