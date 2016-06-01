using System.Collections.Generic;
using Glass.Mapper.Sc.Fields;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Article.Companies
{
	public interface IRelatedDealsService
	{
		IEnumerable<Link> GetRelatedDeals(IArticle article);
	}
}