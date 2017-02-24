using Informa.Library.Publication;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.UserPreference;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.User.Entitlement
{
    [AutowireService]
    public class EntitledProductItemAccesslCodesFactory : IEntitledProductItemAccesslCodesFactory
    {
        protected readonly ISiteRootsContext SiteRootsContext;
        protected readonly ISitePublicationCodeFactory PublicationCodeFactory;
        protected readonly IGlobalSitecoreService GlobalService;
        private string itemCodeFormat = "{0}.{1}";

        public EntitledProductItemAccesslCodesFactory(
            ISiteRootsContext siteRootsContext,
            ISitePublicationCodeFactory publicationCodeFactory,
            IGlobalSitecoreService globalService)
        {
            SiteRootsContext = siteRootsContext;
            PublicationCodeFactory = publicationCodeFactory;
            GlobalService = globalService;
        }
        public IList<string> Create(IArticle item)
        {
            var itemCodes = new List<string>();
            var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => item._Path.StartsWith(sr._Path));
            var chargeableTax = siteRoot?.Taxonomy_Listings;
            var productCode = siteRoot?.Publication_Code ?? string.Empty;

            if (item.Content_Type != null && item.Content_Type.Enable_Entitlement && !string.IsNullOrEmpty(item.Content_Type.Entitlement_Code))
            {
                var itemCode = string.Empty;
                itemCode = item.Content_Type.Enable_Entitlement && chargeableTax.Any(n => n.Entitlement_Code == item.Content_Type.Entitlement_Code) ? string.Format(itemCodeFormat, productCode, item.Content_Type.Entitlement_Code) : string.Empty;
                if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    itemCodes.Add(itemCode);
                }
            }
            if (item.Media_Type != null && item.Media_Type.Enable_Entitlement && !string.IsNullOrEmpty(item.Media_Type.Entitlement_Code))
            {
                var itemCode = string.Empty;
                itemCode = item.Content_Type.Enable_Entitlement && chargeableTax.Any(n => n.Entitlement_Code == item.Content_Type.Entitlement_Code) ? string.Format(itemCodeFormat, productCode, item.Content_Type.Entitlement_Code) : string.Empty;
                if (!string.IsNullOrWhiteSpace(itemCode))
                {
                    itemCodes.Add(itemCode);
                }
            }
            if (item.Taxonomies != null & item.Taxonomies.Any())
            {
                foreach (var taxonomy in item.Taxonomies)
                {
                    var itemCode = string.Empty;
                    itemCode = taxonomy.Enable_Entitlement && chargeableTax.Any(n => n.Entitlement_Code == taxonomy.Entitlement_Code) ? string.Format(itemCodeFormat, productCode, taxonomy.Entitlement_Code) : string.Empty;
                    if (!string.IsNullOrWhiteSpace(itemCode))
                    {
                        itemCodes.Add(itemCode);
                    }
                }
            }
            return itemCodes;
        }
    }
}
