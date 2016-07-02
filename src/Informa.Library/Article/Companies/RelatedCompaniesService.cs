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
	public class RelatedCompaniesService : IRelatedCompaniesService
	{
		private readonly IDCDReader _reader;
		private readonly string _oldCompaniesUrl;
		
		public RelatedCompaniesService(
						IDCDReader reader,
						ISiteSettings siteSettings)
		{
			_reader = reader;
			_oldCompaniesUrl = siteSettings.OldCompaniesUrl;

		}

		public IEnumerable<Link> GetRelatedCompanyLinks(IArticle article)
		{
			var companyIds = article?.Referenced_Companies?.Split(',') ?? new string[0];

			return companyIds.Where(id => !string.IsNullOrEmpty(id)).Select(id => _reader.GetCompanyByRecordNumber(id)).Where(c => c != null).Select(c => new Link
			{
				Text = c.Title,
				Url = string.Format(_oldCompaniesUrl, c.RecordNumber)
			});
		}
	}
}