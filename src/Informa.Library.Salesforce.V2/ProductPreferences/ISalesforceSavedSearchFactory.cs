using Informa.Library.User.Search;
using System.Collections.Generic;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public interface ISalesforceSavedSearchFactory
    {
        AddProductPreferenceRequest Create(ISavedSearchEntity entity);

        IList<ISavedSearchEntity> Create(ProductPreferencesResult entity);
    }
}
