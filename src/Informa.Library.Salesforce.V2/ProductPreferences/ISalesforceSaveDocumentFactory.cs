using Informa.Library.User.Document;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public  interface ISalesforceSaveDocumentFactory
    {
        AddProductPreferenceRequest Create(string verticalName, string publicationCode, string Username, string documentName, string documentDescription, string documentId);

        IList<ISavedDocument> Create(ProductPreferencesResult entity);
    }

}
