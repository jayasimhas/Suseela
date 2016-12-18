using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
namespace Informa.Library.SalesforceVersion
{
    public interface ISalesforceVersionContext
    {
        bool IsNewSalesforceEnabled { get; }

        ISalesforce_Configuration SalesForceConfiguration { get; }
    }
}
