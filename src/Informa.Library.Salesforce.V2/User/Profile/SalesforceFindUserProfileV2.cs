using Informa.Library.User.Profile;
using Informa.Library.User;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.SalesforceConfiguration;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceFindUserProfileV2 : ISalesforceFindUserProfileV2, IUserProfileFactory, IFindUserProfileByUsernameV2
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IHttpClientHelper HttpClientHelper;

        public SalesforceFindUserProfileV2(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }
        public IUserProfile Create(IUser user)
        {
            return Find(user?.AccessToken ?? string.Empty);
        }

        public ISalesforceUserProfile Find(string accessToken)
        {
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                return null;
            }

            var userInfoResponse = HttpClientHelper.GetDataResponse<SalesforceUserInfo>(new Uri(SalesforceConfigurationContext?.GetUserInfoEndPoints())
                , new AuthenticationHeaderValue("Authorization", "Bearer " + accessToken),
                new Dictionary<string, string>());

            return new SalesforceUserProfile
            {
                UserName = userInfoResponse?.UserName ?? string.Empty,
                FirstName = userInfoResponse?.FirstName ?? string.Empty,
                LastName = userInfoResponse?.LastName ?? string.Empty,
                Email = userInfoResponse?.Email,
                MiddleInitial = userInfoResponse?.MiddleName ?? string.Empty,
                NameSuffix = userInfoResponse?.NameSuffix ?? string.Empty,
                Salutation = userInfoResponse?.Salutation ?? string.Empty,
                BillCountry = userInfoResponse?.MailingCountry ?? string.Empty,
                BillAddress1 = userInfoResponse?.MailingStreet ?? string.Empty,
                BillAddress2 = userInfoResponse?.MailingStreet ?? string.Empty,
                BillCity = userInfoResponse?.MailingCity ?? string.Empty,
                BillPostalCode = userInfoResponse?.MailingPostalCode ?? string.Empty,
                BillState = userInfoResponse?.MailingState ?? string.Empty,
                ShipCountry = userInfoResponse?.MailingCountry ?? string.Empty,
                ShipAddress1 = userInfoResponse?.MailingStreet ?? string.Empty,
                ShipAddress2 = userInfoResponse?.MailingStreet ?? string.Empty,
                ShipCity = userInfoResponse?.MailingCity ?? string.Empty,
                ShipPostalCode = userInfoResponse?.MailingPostalCode ?? string.Empty,
                ShipState = userInfoResponse?.MailingState ?? string.Empty,
                CountryCode = userInfoResponse?.MailingCountry ?? string.Empty,
                Fax = string.Empty,
                PhoneExtension = string.Empty,
                Phone = string.Empty,
                PhoneType = string.Empty,
                ////Fax = userInfoResponse?.MailingCity ?? string.Empty,
                ////PhoneExtension = userInfoResponse?.MailingCity ?? string.Empty,
                ////Phone = userInfoResponse?.MailingCity ?? string.Empty,
                ////PhoneType = userInfoResponse?.MailingCity ?? string.Empty,
                Company = userInfoResponse?.CompanyName ?? string.Empty,
                JobFunction = userInfoResponse?.JobFunction ?? string.Empty,
                JobIndustry = userInfoResponse?.JobIndustry ?? string.Empty,
                JobTitle = userInfoResponse?.JobTitle ?? string.Empty
            };
        }

        IUserProfile IFindUserProfileByUsernameV2.Find(string accessToken)
        {
            return Find(accessToken);
        }
    }
}
