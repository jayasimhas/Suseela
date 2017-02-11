using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
	public interface IIsEntitledProducItemContext
	{
		bool IsEntitled(IEntitled_Product item);
        bool IsEntitled(IArticle item);
    }
}