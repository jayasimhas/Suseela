using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Companies
{
	public interface IRelatedCompaniesService
	{
		IEnumerable<Link> GetRelatedCompanyLinks(IArticle article);
	}
}
