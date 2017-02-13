using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.User.Entitlement
{
    public interface IEntitledProductItemEntitlementLevelFactory
    {
        EntitlementLevel Create(IArticle item);
    }
}
