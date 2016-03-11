using Informa.Library.User;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledProductContext
    {
        EntitledAccessLevel GetAccessLevel(IEntitledProductItem entitlementProduct);
    }
}