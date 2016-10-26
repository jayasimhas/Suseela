using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Management;
using enterprise = Informa.Library.Salesforce.SFv2;
using System.ServiceModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using Informa.Library.User;
using Informa.Library.User.Authentication.Web;


namespace Informa.Web.Areas.Account.Controllers
{

    public class EntitlementApiController : ApiController
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageAccountInfo AccountInfo;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserProfileContext ProfileContext;
        //protected readonly IUserSalesForceContext UserSalesForceContext;
        protected readonly IUserSession UserSession;
        IWebAuthenticateUser WebAuthenticateUser;
        public EntitlementApiController(
            IAuthenticatedUserContext userContext,
            IManageAccountInfo accountInfo,
            ITextTranslator textTranslator,
            IUserProfileContext profileContext,
            //IUserSalesForceContext userSalesForceContext,
             IUserSession userSession,
            IWebAuthenticateUser webAuthenticateUser)
        {
            UserContext = userContext;
            AccountInfo = accountInfo;
            TextTranslator = textTranslator;
            ProfileContext = profileContext;
            WebAuthenticateUser = webAuthenticateUser;
            UserSession = userSession;
           // UserSalesForceContext = userSalesForceContext;
        }
        [HttpGet]
        [ValidateReasons]
        [ArgumentsRequired]
        public string GetUSerEntitlements()
        {
            string response = string.Empty;
            var sfUser = WebAuthenticateUser.AuthenticatedUser;
            //string url = UserSalesForceContext?.PreUserSalesforceDetails?.SalesForceURL;
            //string sessionid = UserSalesForceContext?.PreUserSalesforceDetails?.SalesForceSessionId;
            //string results = GetEntitlementfromSalesForce(url, sessionid);
            string userId = sfUser?.UserId;
            string url = sfUser?.SalesForceURL;
            string sessionid = sfUser?.SalesForceSessionId;
            string results = GetEntitlementfromSalesForce(url, sessionid, userId);
            response = "URL: " + url + "\n Session ID: " + sessionid + "\n SF Response: " + results;            
            return response;

        }


        public string GetEntitlementfromSalesForce(string url, string sessionid, string userId)
        {

            EndpointAddress EndpointAddr = new EndpointAddress(url);
            //instantiate session header object and set session id

            enterprise.SessionHeader header = new enterprise.SessionHeader();

            header.sessionId = sessionid;
            //create service client to call API endpoint

            enterprise.QueryResult queryresult = null;
            using (enterprise.SoapClient queryClient = new enterprise.SoapClient("Soap", EndpointAddr))
            {
                string query = "Select e.ch_sugar_premium__c, e.ch_sugar__c, e.ch_spices__c, e.ch_oils_and_oilseeds__c, e.ch_molasses_and_fi__c, e.ch_meat__c, e.ch_juices_and_beverages__c, e.ch_grains__c, e.ch_frozen__c, e.ch_dfn__c, e.ch_dairy__c, e.ch_coffee_premium__c, e.ch_coffee__c, e.ch_cocoa__c, e.ch_canned_and_tomato_products__c, e.ch_biofuels__c, e.User__c, e.UsageLimit__c, e.UsageLimitReached__c, e.UsageCount__c, e.Sugar_Premium_Code__c, e.Sugar_Code__c, e.Subscription__c, e.Subscription_Access_Type__c, e.StartDate__c, e.Spices_Code__c, e.SourceIP__c, e.Service__c, e.ServiceUsageLimit__c, e.ServiceStartDate__c, e.ServiceEndDate__c, e.Product_Type__c, e.Opportunity__c, e.OpportunityAccount__c, e.Oils_and_Oilseeds_Code__c, e.Number_of_Licences__c, e.Name, e.Molasses_and_FI_Code__c, e.Meter_Limit__c, e.Meat_Code__c, e.ManualUsageLimit__c, e.ManualStartDate__c, e.ManualEndDate__c, e.Juices_and_Beverages_Code__c, e.Inherit_Transparent_Entitlements__c, e.Id, e.Grains_Code__c, e.Frozen_Code__c, e.EntitlementType__c, e.EndDate__c, e.Deactivated_Date__c, e.Dairy_Code__c, e.DFN_Code__c, e.Contact__c, e.ContactName__c, e.ContactLastName__c, e.ContactFirstName__c, e.Coffee_Premium_Code__c, e.Coffee_Code__c, e.Cocoa_Code__c, e.Canned_and_Tomato_Products_Code__c, e.Biofuels_Code__c, e.BillingAddress__c, e.AuthType__c, e.Active__c, e.ActivatedDate__c, e.Account__c, e.AccountName__c, e.Access_Control__c From Entitlement__c e";
                query += " " + "where User__c = '" + userId + "'";


                queryClient.query(
                              header, //sessionheader
                              null, //queryoptions
                              null, //mruheader
                              null, //packageversionheader
                              query, //SOQL query
                              out queryresult
                           );

                //results
                if (queryresult.records != null)
                {
                    // return ConvertObjectToXMLString(queryresult.records);
                    return "Getting Records"; 
                    foreach (var record in queryresult.records)
                    {
                    //    // var Entitlemnet = (enterprise.Entitlement__c)record;

                        string xmlString = ConvertObjectToXMLString(queryresult.records);
                    //    // Save C# class object into Xml file
                        XElement xElement = XElement.Parse(xmlString);
                        var path = "d://userDetail.xml";
                        xElement.Save(path);
                        return "Yahoo";
                  
                    }
                }
                else return string.Empty;

            }



        }

        static string ConvertObjectToXMLString(object classObject)
        {
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();
            }
            return xmlString;
        }
    }
}






