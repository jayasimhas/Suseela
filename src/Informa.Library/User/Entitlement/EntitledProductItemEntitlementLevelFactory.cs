using Informa.Library.Publication;
using Informa.Library.Site;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System.Linq;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class EntitledProductItemEntitlementLevelFactory : IEntitledProductItemEntitlementLevelFactory
    {
        protected readonly ISiteRootsContext SiteRootsContext;
        protected readonly ISitePublicationCodeFactory PublicationCodeFactory;
        protected readonly IItemReferences ItemReferences;

        public EntitledProductItemEntitlementLevelFactory(
            ISiteRootsContext siteRootsContext,
            ISitePublicationCodeFactory publicationCodeFactory,
            IItemReferences itemReferences)
        {
            SiteRootsContext = siteRootsContext;
            PublicationCodeFactory = publicationCodeFactory;
            ItemReferences = itemReferences;
        }

        public EntitlementLevel Create(IArticle item)
        {
            var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => item._Path.StartsWith(sr._Path));
            if (siteRoot != null && siteRoot.Entitlement_Type != null)
            {
                if (siteRoot.Entitlement_Type._Id.Equals(ItemReferences.ChannelLevelEntitlementType))
                {
                    return EntitlementLevel.Channel;
                }
            }
            return EntitlementLevel.Site;
        }
    }
}
