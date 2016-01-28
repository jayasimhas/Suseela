using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels
{
	public interface IArticleListItemModelFactory
	{
		IListable Create(IArticle article);
	}
}
