using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Entitlement;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System.Collections.Generic;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class EntitlementProductFactory : IEntitlementProductFactory
    {
        protected readonly IEntitledProductItemCodeFactory ProductCodeFactory;
        protected readonly IEntitledProductItemChannelCodesFactory ProductItemChannelCodesFactory;
        protected readonly IEntitledProductItemEntitlementLevelFactory ProductItemEntitlementLevelFactory;

        public EntitlementProductFactory(
            IEntitledProductItemCodeFactory productCodeFactory,
            IEntitledProductItemChannelCodesFactory productItemChannelCodesFactory,
            IEntitledProductItemEntitlementLevelFactory productItemEntitlementLevelFactory)
        {
            ProductCodeFactory = productCodeFactory;
            ProductItemChannelCodesFactory = productItemChannelCodesFactory;
            ProductItemEntitlementLevelFactory = productItemEntitlementLevelFactory;
        }

        public IEntitledProduct Create(IEntitled_Product item)
        {
            var productCode = ProductCodeFactory.Create(item);
            return new EntitledProduct
            {
                DocumentId = item.Article_Number,
                IsFree = item.Free,
                IsFreeWithRegistration = item.Free_With_Registration,
                ProductCode = productCode,
                PublishedOn = item.Actual_Publish_Date
            };
        }

        public IEntitledProduct Create(IArticle item)
        {
            var productCode = ProductCodeFactory.Create(item);
            var entitlementLevel = ProductItemEntitlementLevelFactory.Create(item);
            var channelCodes = entitlementLevel == EntitlementLevel.Channel ?
                ProductItemChannelCodesFactory.Create(item) : new List<string>();

            return new EntitledProduct
            {
                DocumentId = item.Article_Number,
                IsFree = item.Free,
                IsFreeWithRegistration = item.Free_With_Registration,
                ProductCode = productCode,
                PublishedOn = item.Actual_Publish_Date,
                EntitlementLevel = entitlementLevel,
                Channels = channelCodes
            };
        }
    }
}
