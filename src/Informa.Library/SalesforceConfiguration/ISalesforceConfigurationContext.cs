using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
namespace Informa.Library.SalesforceConfiguration
{
    public interface ISalesforceConfigurationContext
    {
        bool IsNewSalesforceEnabled { get; }

        ISalesforce_Configuration SalesForceConfiguration { get; }
    }
}
