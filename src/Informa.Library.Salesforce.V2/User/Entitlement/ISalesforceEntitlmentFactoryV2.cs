using Informa.Library.Salesforce.User.Entitlement;

namespace Informa.Library.Salesforce.V2.User.Entitlement
{
    public interface ISalesforceEntitlmentFactoryV2
    {
        SalesforceEntitlement Create(UserEntitlement entitlement);
    }
}