using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
	public interface IEntitlementProductFactory
	{
		IEntitledProduct Create(IEntitled_Product item);

        IEntitledProduct Create(IArticle item);
        //IEntitledProduct Create(IGeneral_Content_Page gPage);

    }
}