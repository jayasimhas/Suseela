﻿using Informa.Library.User.UserPreference;

namespace Informa.Library.Salesforce.V2.ProductPreferences
{
    public interface ISalesforceContentPreferencesFactory
    {
        AddProductPreferenceRequest Create(string userName, string verticalPreferenceLocale,
            string publicationCode, string contentPreferences);
        IUserPreferences Create(ProductPreferencesResult entity);
    }
}
