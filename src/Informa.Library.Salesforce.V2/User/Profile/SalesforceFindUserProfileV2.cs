﻿using Informa.Library.User.Profile;
using Informa.Library.User;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.SalesforceConfiguration;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System;
using Sitecore.Configuration;
using Newtonsoft.Json;

namespace Informa.Library.Salesforce.V2.User.Profile
{
    public class SalesforceFindUserProfileV2 : ISalesforceFindUserProfileV2, IUserProfileFactoryV2, IFindUserProfileByUsernameV2
    {
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;
        protected readonly IHttpClientHelper HttpClientHelper;
        protected readonly ISalesforceInfoLogger InfoLogger;

        public SalesforceFindUserProfileV2(
            IHttpClientHelper httpClientHelper,
            ISalesforceConfigurationContext salesforceConfigurationContext,
            ISalesforceInfoLogger infoLogger)
        {
            HttpClientHelper = httpClientHelper;
            SalesforceConfigurationContext = salesforceConfigurationContext;
            InfoLogger = infoLogger;
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

            InfoLogger.Log(SalesforceConfigurationContext?.GetUserInfoEndPoints(), this.GetType().Name);
            InfoLogger.Log(JsonConvert.SerializeObject(userInfoResponse), this.GetType().Name);

            return new SalesforceUserProfile
            {
                UserName = userInfoResponse?.UserName ?? string.Empty,
                FirstName = userInfoResponse?.FirstName ?? string.Empty,
                LastName = userInfoResponse?.LastName ?? string.Empty,
                Email = userInfoResponse?.Email,
                MiddleInitial = userInfoResponse?.CustomAttributes?.MiddleName ?? string.Empty,
                Salutation = userInfoResponse?.CustomAttributes?.Salutation ?? string.Empty,
                ShipCountry = userInfoResponse?.CustomAttributes?.MailingCountry ?? string.Empty,
                ShipAddress1 = userInfoResponse?.CustomAttributes?.MailingStreet ?? string.Empty,
                ShipCity = userInfoResponse?.CustomAttributes?.MailingCity ?? string.Empty,
                ShipPostalCode = userInfoResponse?.CustomAttributes?.MailingPostalCode ?? string.Empty,
                ShipState = userInfoResponse?.CustomAttributes?.MailingState ?? string.Empty,
                Phone = userInfoResponse?.CustomAttributes?.Phone ?? string.Empty,
                Mobile = userInfoResponse?.CustomAttributes?.Mobile ?? string.Empty,
                Company = userInfoResponse?.CustomAttributes?.CompanyName ?? string.Empty,
                JobFunction = userInfoResponse?.CustomAttributes?.JobFunction ?? string.Empty,
                JobIndustry = userInfoResponse?.CustomAttributes?.JobIndustry ?? string.Empty,
                JobTitle = userInfoResponse?.CustomAttributes?.JobTitle ?? string.Empty
            };
        }

        IUserProfile IFindUserProfileByUsernameV2.Find(string accessToken)
        {
            return Find(accessToken);
        }
    }
}