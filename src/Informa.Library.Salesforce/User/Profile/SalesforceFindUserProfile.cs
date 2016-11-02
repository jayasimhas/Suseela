using Informa.Library.User.Profile;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User;
using System;
using System.ServiceModel;
using enterprise = Informa.Library.Salesforce.SFv2;
using System.Text;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceFindUserProfile : ISalesforceFindUserProfile, IUserProfileFactory, IFindUserProfileByUsername
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceFindUserProfile(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

        public IUserProfile Create(IUser user)
        {
            return Find(user?.Username ?? string.Empty);
        }

        //implemented for new SF api
        public ISalesforceUserProfile Find(string userId, string url, string sessionId)
        {
            if (string.IsNullOrWhiteSpace(userId) || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(sessionId))
            {
                return null;
            }

            //  var profileResponse = Service.Execute(s => s.queryProfileContactInformation(username));
            var profileResponse = this.getProfileInfo(userId, url, sessionId);

            if (profileResponse != null && profileResponse.records.Length > 0 && profileResponse.records[0] != null)
            {
                var userPreference = profileResponse.records[0] as enterprise.User_Preference__c;

                var user = userPreference?.User__r;
                // var contact = user?.Contact;

                if (user != null)//  && contact != null)
                {
                   var salesforceUserProfile= new SalesforceUserProfile
                    {
                        Name = user.Name ?? string.Empty,
                        Company = userPreference.Company__c ?? string.Empty,
                        JobTitle = userPreference.Job_or_function__c ?? string.Empty,
                        Phone = userPreference?.Telephone__c ?? string.Empty,

                        BillAddress1 = userPreference.Primary_Address__c ?? string.Empty,
                        BillCity = userPreference.City_town__c ?? string.Empty,
                        BillPostalCode = userPreference?.Postcode__c ?? string.Empty,

                        /*For SFDC confirmed contacts */
                        SFConfirmedName = user.Contact?.Name ?? string.Empty,
                        SFConfirmedCompany = user.Contact?.Account?.Name ??string.Empty, 
                       SFConfirmedJobTitle =  user.Contact?.Title ?? string.Empty,  
                       SFConfirmedPhone = user.Contact?.MobilePhone ?? string.Empty,
                       SFConfirmedBillAddress1 = user.Contact?.MailingAddress?.street ?? string.Empty,
                       SFConfirmedBillCity = user.Contact?.MailingAddress?.city ?? string.Empty,
                       SFConfirmedBillPostalCode = user.Contact?.MailingAddress?.postalCode ?? string.Empty,
                   };

                    return salesforceUserProfile;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }


        }

        private enterprise.QueryResult getProfileInfo(string userId, string url, string sessionId)
        {
            EndpointAddress EndpointAddr = new EndpointAddress(url);

            enterprise.SessionHeader header = new enterprise.SessionHeader();
            header.sessionId = sessionId;

            //create service client to call API endpoint
            enterprise.QueryResult queryresult = null;
            using (enterprise.SoapClient queryClient = new enterprise.SoapClient("Soap", EndpointAddr))
            {
                // Query to pull all the data

                StringBuilder query = new StringBuilder(" Select u.User__c,   u.User_LoginID__c,u.Email__c, u.Telephone__c,user__r.Name,");
                query.Append("  u.T_s_C_s__c, u.TMT__c, u.Secondary_Address__c, u.Salutation__c, u.Primary_practice_area__c,");
                query.Append("  u.Primary_Address__c, u.Postcode__c,  u.Other_practice_area__c, ");
                query.Append("  u.Other_Job__c,    u.Job_or_function__c, u.Id,  ");
                query.Append("  u.Finance__c,  u.Country__c, u.Company__c, u.City_town__c, u.Agri__c, ");
                query.Append("  u.Agra_email_news_updates_and_offers__c, ");
                query.Append(" user__r.contact.accountid, user__r.contact.account.name ");
                query.Append(" user__r.contact.name, ");
                query.Append(" user__r.contact.email, ");
                query.Append(" user__r.contact.fax, ");
                query.Append(" user__r.contact.mobilephone, ");
                query.Append(" user__r.contact.title, ");
                query.Append(" user__r.contact.MailingAddress ");
                query.Append(" From User_Preference__c u where User__c = '0053E000000TyIl'");

                queryClient.query(
                             header, //sessionheader
                             null, //queryoptions
                             null, //mruheader
                             null, //packageversionheader
                             query.ToString(), //SOQL query
                             out queryresult
                          );

                return queryresult;

            }
        }

        public ISalesforceUserProfile Find(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                return null;
            }

            var profileResponse = Service.Execute(s => s.queryProfileContactInformation(username));

            if (!profileResponse.IsSuccess())
            {
                return null;
            }

            var profile = profileResponse.profile;

            return new SalesforceUserProfile
            {
                FirstName = profile?.name?.firstName ?? string.Empty,
                LastName = profile?.name?.lastName ?? string.Empty,
                Email = username,
                MiddleInitial = profile?.name?.middleInitial ?? string.Empty,
                NameSuffix = profile?.name?.nameSuffix ?? string.Empty,
                Salutation = profile?.name?.salutation ?? string.Empty,
                BillCountry = profile?.billingAddress?.country ?? string.Empty,
                BillAddress1 = profile?.billingAddress?.addressLine1 ?? string.Empty,
                BillAddress2 = profile?.billingAddress?.addressLine2 ?? string.Empty,
                BillCity = profile?.billingAddress?.city ?? string.Empty,
                BillPostalCode = profile?.billingAddress?.postalCode ?? string.Empty,
                BillState = profile?.billingAddress?.stateProvince ?? string.Empty,
                ShipCountry = profile?.shippingAddress?.country ?? string.Empty,
                ShipAddress1 = profile?.shippingAddress?.addressLine1 ?? string.Empty,
                ShipAddress2 = profile?.shippingAddress?.addressLine2 ?? string.Empty,
                ShipCity = profile?.shippingAddress?.city ?? string.Empty,
                ShipPostalCode = profile?.shippingAddress?.postalCode ?? string.Empty,
                ShipState = profile?.shippingAddress?.stateProvince ?? string.Empty,
                CountryCode = profile?.phoneFax?.countryCode ?? string.Empty,
                Fax = profile?.phoneFax?.fax ?? string.Empty,
                PhoneExtension = profile?.phoneFax?.phoneExtension ?? string.Empty,
                Phone = profile?.phoneFax?.phoneNumber ?? string.Empty,
                PhoneType = profile?.phoneFax?.phoneType ?? string.Empty,
                Company = profile?.companyJob?.company ?? string.Empty,
                JobFunction = profile?.companyJob?.function ?? string.Empty,
                JobIndustry = profile?.companyJob?.industry ?? string.Empty,
                JobTitle = profile?.companyJob?.title ?? string.Empty
            };
        }

        IUserProfile IFindUserProfileByUsername.Find(string username)
        {
            return Find(username);
        }

        IUserProfile IFindUserProfileByUsername.Find(string userId, string url, string sessionId)
        {
            return Find(userId, url, sessionId);
        }
    }
}

