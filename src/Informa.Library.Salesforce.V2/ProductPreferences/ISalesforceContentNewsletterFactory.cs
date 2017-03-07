using Informa.Library.User.Newsletter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public interface ISalesforceContentNewsletterFactory
    {
        AddProductPreferenceRequest Create(string username, string verticalName, string publicationCode, IEnumerable<INewsletterUserOptIn> optIns);
        IList<INewsletterUserOptIn> Create(ProductPreferencesResult entity);
        UpdateProductPreferenceRequest CreateUpdateRequest(INewsletterUserOptIn entity);

        AddProductPreferenceRequest Create(string username, string verticalName, string publicationCode, bool optIn);
        UpdateProductPreferenceRequest CreateUpdateRequest(OffersOptIn Optin);
        IOffersOptIn CreateOfferOptinGetRequest(ProductPreferencesResult entity);
    }
}
