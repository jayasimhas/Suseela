using Informa.Library.Salesforce.User.Entitlement;

namespace Informa.Library.Salesforce.V2.User.Entitlement
{
    public class SalesforceEntitlmentFactoryV2 : ISalesforceEntitlmentFactoryV2
    {
        public SalesforceEntitlement Create(UserEntitlement entitlement)
        {
            return new SalesforceEntitlement
            {
                ProductCode = string.IsNullOrWhiteSpace(entitlement.Code) ? string.Empty : entitlement.Code,
                ProductType = entitlement.ProductType,
                Type = entitlement.Type,
                StartDate = entitlement.StartDate,
                SalesEndDate = entitlement.EndDate,
                AccessEndDate = entitlement.AccessEndDate,
                ProductName = entitlement.ProductName,
                Description = entitlement.Description,
                Name = entitlement.Name,
                IsActive = entitlement.Active
            };
        }
    }
}
