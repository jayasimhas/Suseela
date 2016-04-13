using Informa.Library.User;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledProductEntitlementAccessLevelContext
    {
        EntitledAccessLevel GetAccessLevel(IEntitledProductItem entitlementProduct);
    }
}