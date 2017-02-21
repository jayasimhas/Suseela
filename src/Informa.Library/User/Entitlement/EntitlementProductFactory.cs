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
        protected readonly IEntitledProductItemAccesslCodesFactory ProductItemAccesslCodesFactory;

        public EntitlementProductFactory(
            IEntitledProductItemCodeFactory productCodeFactory,
            IEntitledProductItemChannelCodesFactory productItemChannelCodesFactory,
            IEntitledProductItemEntitlementLevelFactory productItemEntitlementLevelFactory,
            IEntitledProductItemAccesslCodesFactory productItemAccesslCodesFactory)
        {
            ProductCodeFactory = productCodeFactory;
            ProductItemChannelCodesFactory = productItemChannelCodesFactory;
            ProductItemEntitlementLevelFactory = productItemEntitlementLevelFactory;
            ProductItemAccesslCodesFactory = productItemAccesslCodesFactory;
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
            if(string.Equals(entitlementLevel.ToString(), EntitlementLevel.Channel.ToString(),System.StringComparison.OrdinalIgnoreCase))
            {
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
            if (string.Equals(entitlementLevel.ToString(), EntitlementLevel.Item.ToString(), System.StringComparison.OrdinalIgnoreCase))
            {
                 var itemlCodes = entitlementLevel == EntitlementLevel.Item ?
                ProductItemAccesslCodesFactory.Create(item) : new List<string>();

                return new EntitledProduct
                {
                    DocumentId = item.Article_Number,
                    IsFree = item.Free,
                    IsFreeWithRegistration = item.Free_With_Registration,
                    ProductCode = productCode,
                    PublishedOn = item.Actual_Publish_Date,
                    EntitlementLevel = entitlementLevel,
                    Channels = itemlCodes
                };
            }
            return new EntitledProduct();            
        }
    }
}
