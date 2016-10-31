using System.Web.Mvc;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.User;
using Informa.Library.User.Authentication.Web;
using System.ServiceModel;
using enterprise = Informa.Library.Salesforce.SFv2;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using System.Xml.Linq;

namespace Informa.Web.Areas.Account.Controllers
{
    public class TransparentIpEntitlementController : Controller
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageAccountInfo AccountInfo;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserProfileContext ProfileContext;
        protected readonly IUserSession UserSession;
        IWebAuthenticateUser WebAuthenticateUser;

        public TransparentIpEntitlementController(
            IAuthenticatedUserContext userContext,
            IManageAccountInfo accountInfo,
            ITextTranslator textTranslator,
            IUserProfileContext profileContext,
            IUserSession userSession,
            IWebAuthenticateUser webAuthenticateUser)
        {
            UserContext = userContext;
            AccountInfo = accountInfo;
            TextTranslator = textTranslator;
            ProfileContext = profileContext;
            WebAuthenticateUser = webAuthenticateUser;
            UserSession = userSession;
        }

        // GET: Account/TransparentIpEntitlement
        public ActionResult Index()
        {
            string result = getEntitlements();

            ViewBag.Result = result;

            return View();
        }



        private string getEntitlements()
        {
            var sfUser = WebAuthenticateUser.AuthenticatedUser;

            string userId = sfUser?.UserId;
            string url = sfUser?.SalesForceURL;
            string sessionid = sfUser?.SalesForceSessionId;

            string result = string.Empty;

            // check if the pseudo user is auto-logged in through transparent ip
            if (string.IsNullOrEmpty(url) || string.IsNullOrEmpty(sessionid))
            {
                result = "Not auto-logged-in through transparent ip";
                return result;
            }
            else
            {

                try
                {
                    result = this.getEntitlementfromSalesForce(url, sessionid, userId);
                }
                catch (System.Exception ex)
                {
                    string exception = ex.ToString();
                    result = "Error pulling entitlement... Please try again \n" + exception;
                }
            }

            return result;

        }

        private string getEntitlementfromSalesForce(string url, string sessionid, string userId)
        {

            //  url = "https://informabi--IDPOC.cs82.my.salesforce.com/services/Soap/c/38.0/00D3E0000008rEm/0DF3E0000008OTI";


            EndpointAddress EndpointAddr = new EndpointAddress(url);
            //instantiate session header object and set session id

            enterprise.SessionHeader header = new enterprise.SessionHeader();
            header.sessionId = sessionid;

            //create service client to call API endpoint
            enterprise.QueryResult queryresult = null;
            using (enterprise.SoapClient queryClient = new enterprise.SoapClient("Soap", EndpointAddr))
            {
                // Query to pull all the data
                string query = "Select e.ch_sugar_premium__c, e.ch_sugar__c, e.ch_spices__c, e.ch_oils_and_oilseeds__c, e.ch_molasses_and_fi__c, e.ch_meat__c, e.ch_juices_and_beverages__c, e.ch_grains__c, e.ch_frozen__c, e.ch_dfn__c, e.ch_dairy__c, e.ch_coffee_premium__c, e.ch_coffee__c, e.ch_cocoa__c, e.ch_canned_and_tomato_products__c, e.ch_biofuels__c, e.User__c, e.UsageLimit__c, e.UsageLimitReached__c, e.UsageCount__c, e.Sugar_Premium_Code__c, e.Sugar_Code__c, e.Subscription__c, e.Subscription_Access_Type__c, e.Sub_Network__c, e.StartDate__c, e.Spices_Code__c, e.SourceIP__c, e.Service__c, e.ServiceUsageLimit__c, e.ServiceStartDate__c, e.ServiceEndDate__c, e.Range_To__c, e.Range_From__c, e.Product_Type__c, e.Opportunity__c, e.OpportunityAccount__c, e.Oils_and_Oilseeds_Code__c, e.Number_of_Licences__c, e.Name, e.Molasses_and_FI_Code__c, e.Meter_Limit__c, e.Meat_Code__c, e.ManualUsageLimit__c, e.ManualStartDate__c, e.ManualEndDate__c, e.Juices_and_Beverages_Code__c, e.Inherit_Transparent_Entitlements__c, e.Id, e.IP_Range__c, e.IP_Address__c, e.Grains_Code__c, e.Frozen_Code__c, e.EntitlementType__c, e.EndDate__c, e.Deactivated_Date__c, e.Dairy_Code__c, e.DFN_Code__c, e.Contact__c, e.ContactName__c, e.ContactLastName__c, e.ContactFirstName__c, e.Coffee_Premium_Code__c, e.Coffee_Code__c, e.Cocoa_Code__c, e.Canned_and_Tomato_Products_Code__c, e.Biofuels_Code__c, e.BillingAddress__c, e.AuthType__c, e.Active__c, e.ActivatedDate__c, e.Account__c, e.AccountName__c, e.Access_Control__c From Entitlement__c e";
                query += " " + " where IP_Address__c = '" + "192.168.123.123" + "'";

                // query matching normal flow
                //string query = "Select e.ch_sugar_premium__c, e.ch_sugar__c, e.ch_spices__c, e.ch_oils_and_oilseeds__c, e.ch_molasses_and_fi__c, e.ch_meat__c, e.ch_juices_and_beverages__c, e.ch_grains__c, e.ch_frozen__c, e.ch_dfn__c, e.ch_dairy__c, e.ch_coffee_premium__c, e.ch_coffee__c, e.ch_cocoa__c, e.ch_canned_and_tomato_products__c, e.ch_biofuels__c, e.User__c, e.UsageLimit__c, e.UsageLimitReached__c, e.UsageCount__c, e.Sugar_Premium_Code__c, e.Sugar_Code__c, e.Subscription__c, e.Subscription_Access_Type__c, e.StartDate__c, e.Spices_Code__c, e.SourceIP__c, e.Service__c, e.ServiceUsageLimit__c, e.ServiceStartDate__c, e.ServiceEndDate__c, e.Product_Type__c, e.Opportunity__c, e.OpportunityAccount__c, e.Oils_and_Oilseeds_Code__c, e.Number_of_Licences__c, e.Name, e.Molasses_and_FI_Code__c, e.Meter_Limit__c, e.Meat_Code__c, e.ManualUsageLimit__c, e.ManualStartDate__c, e.ManualEndDate__c, e.Juices_and_Beverages_Code__c, e.Inherit_Transparent_Entitlements__c, e.Id, e.Grains_Code__c, e.Frozen_Code__c, e.EntitlementType__c, e.EndDate__c, e.Deactivated_Date__c, e.Dairy_Code__c, e.DFN_Code__c, e.Contact__c, e.ContactName__c, e.ContactLastName__c, e.ContactFirstName__c, e.Coffee_Premium_Code__c, e.Coffee_Code__c, e.Cocoa_Code__c, e.Canned_and_Tomato_Products_Code__c, e.Biofuels_Code__c, e.BillingAddress__c, e.AuthType__c, e.Active__c, e.ActivatedDate__c, e.Account__c, e.AccountName__c, e.Access_Control__c From Entitlement__c e"; 
                //query += " " + " where IP_Address__c = '" + "192.168.123.123" + "'";

                // Query to pull only channel details
                //string query = "Select e.ch_sugar_premium__c, e.ch_sugar__c, e.ch_spices__c, e.ch_oils_and_oilseeds__c, e.ch_molasses_and_fi__c, e.ch_meat__c, e.ch_juices_and_beverages__c, e.ch_grains__c, e.ch_frozen__c, e.ch_dfn__c, e.ch_dairy__c, e.ch_coffee_premium__c, e.ch_coffee__c, e.ch_cocoa__c, e.ch_canned_and_tomato_products__c, e.ch_biofuels__c From Entitlement__c e";
                //query += " " + " where IP_Address__c = '" + "192.168.123.123" + "'";


                queryClient.query(
                              header, //sessionheader
                              null, //queryoptions
                              null, //mruheader
                              null, //packageversionheader
                              query, //SOQL query
                              out queryresult
                           );

                //results
                if (queryresult != null & queryresult.records != null && queryresult.records.Length > 0 && queryresult.records[0] != null)
                {
                    string result = SerializeObject(queryresult.records[0]);

                    if (!string.IsNullOrEmpty(result))
                    {
                        try
                        {
                            XDocument doc = XDocument.Parse(result);
                            result = doc.ToString();
                        }
                        catch
                        {
                            //do nothing, because unformatted string will be returned.
                        }
                    }

                    return result;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public string SerializeObject(Library.Salesforce.SFv2.sObject toSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(toSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, toSerialize);
                return textWriter.ToString();
            }
        }
    }
}
