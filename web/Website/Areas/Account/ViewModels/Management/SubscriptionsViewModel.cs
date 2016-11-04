using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.ViewModels.Account;
using Informa.Library.Publication;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.User;
using Informa.Library.User.Authentication.Web;
using System.Xml;
using System.Xml.Serialization;
using enterprise = Informa.Library.Salesforce.SFv2;
using System.ServiceModel;
using System.IO;
namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SubscriptionsViewModel : GlassViewModel<ISubscriptions_Page>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly ISignInViewModel SignInViewModel;

        protected readonly IFindSitePublicationByCode FindSitePublication;

        private readonly Dictionary<string, bool> RenewBtnSettings;
        private readonly Dictionary<string, bool> SubscriptionBtnSettings;
        private readonly IEnumerable<ISubscription> _subcriptions;
        protected readonly IUserSession UserSession;
        IWebAuthenticateUser WebAuthenticateUser;

        public SubscriptionsViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IUserSubscriptionsContext userSubscriptionsContext,
            ISignInViewModel signInViewModel,
            IFindSitePublicationByCode findSitePublication,
                        IUserSession userSession,
            IWebAuthenticateUser webAuthenticateUser)
        {
            TextTranslator = translator;
            UserContext = userContext;
            SignInViewModel = signInViewModel;
            FindSitePublication = findSitePublication;
            WebAuthenticateUser = webAuthenticateUser;
            UserSession = userSession;
            RenewBtnSettings = new Dictionary<string, bool>();
            SubscriptionBtnSettings = new Dictionary<string, bool>();
            //string s = GetArticleBody();
           // _subcriptions = userSubscriptionsContext.Subscriptions.Where(w => string.IsNullOrEmpty(w.Publication) == false && w.ExpirationDate >= DateTime.Now.AddMonths(-6)).OrderByDescending(o => o.ExpirationDate);

            //foreach (var sub in _subcriptions)
            //{
            //    //renew btns
            //    if (!RenewBtnSettings.ContainsKey(sub.ProductCode))
            //        RenewBtnSettings.Add(sub.ProductCode, WithinRenewRange(sub.ExpirationDate));
            //    else
            //        RenewBtnSettings[sub.ProductCode] &= WithinRenewRange(sub.ExpirationDate);
            //    //subscribe btns
            //    if (!SubscriptionBtnSettings.ContainsKey(sub.ProductCode))
            //        SubscriptionBtnSettings.Add(sub.ProductCode, IsValidSubscription(sub));
            //    else
            //        SubscriptionBtnSettings[sub.ProductCode] |= IsValidSubscription(sub);
            //}
        }
        public IEnumerable<SubscriptionViewModel> SubscriptionViewModels
        {
            get
            {
                //return _subcriptions.Select(s => new SubscriptionViewModel
                //{
                //    Expiration = "",
                //    Publication = FindSitePublication.Find(s.Publication)?.Name ?? s.Publication,
                //    Renewable = ShowRenewButton(s),
                //    Subscribable = ShowSubscribeButton(s.ProductCode),
                //    Type = s.ProductType,
                //    TaxonomyItems = GeTaxonomyItemsByProductCode(s.ProductCode),
                //    IsCurrentPublication = GlassModel.GetAncestors<ISite_Root>().FirstOrDefault().Publication_Code.Equals(s.ProductCode)
                //});
                //return new List<SubscriptionViewModel>() { new SubscriptionViewModel()
                //{
                //    Expiration = DateTime.Now,
                //    IsCurrentPublication = true,
                //    Publication = "Hello",
                //    Renewable = true,
                //    Subscribable = true,
                //    TaxonomyItems = null,
                //    Type = "How are you"

                //} };
                //string s = GetArticleBody();
                //return null;
                return GetArticleBody();
            }
        }
        public List<SubscriptionViewModel> GetArticleBody()
        {
            var sfUser = WebAuthenticateUser.AuthenticatedUser;
            //bool b = GlassModel.Free;

            string userId = sfUser?.UserId;
            string url = sfUser?.SalesForceURL;
            string sessionid = sfUser?.SalesForceSessionId;
            string email = sfUser?.Email;
            List<SubscriptionViewModel> output = new List<SubscriptionViewModel>();
            enterprise.QueryResult queryresult;
            if (sessionid != null)
            {

                queryresult = GetEntitlementfromSalesForce(url, sessionid, userId, email);


                if (queryresult.records != null)
                {

                    foreach (var record in queryresult.records)
                    {

                        output = ConvertObjectToXMLString(queryresult.records);
                        //List<string> TaxonomiesList = (List<string>)GlassModel.Taxonomies;

                    }
                    return output;
                }
                else return output;
            }
            else
            {
                return output;
            }
        }
        List<SubscriptionViewModel> ConvertObjectToXMLString(object classObject)
        {

            List<string> entitlementslist = new List<string>();
            List<SubscriptionViewModel> entitlementslistFinal = new List<SubscriptionViewModel>();
            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
            DateTime dtxyz = DateTime.Now;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                xmlSerializer.Serialize(memoryStream, classObject);
                memoryStream.Position = 0;
                xmlString = new StreamReader(memoryStream).ReadToEnd();

                //var xmlStr = File.ReadAllText(xmlString);
                XmlDocument xml = new XmlDocument();
                xml.LoadXml(xmlString);
                // XmlTextReader reader = new XmlTextReader(xmlString);
                //reader.WhitespaceHandling = WhitespaceHandling.None;
                
                string xpath = "//";

                string name;
                
                // var nodes = xml.SelectSingleNode("ArrayOfSObject/sObject/Account__c").InnerText;
                for (int i = 1; i < (((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes.Count; i++)
                {
                    if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_biofuels__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Bio Fuels");
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_frozen__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Froozen Food");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_canned_and_tomato_products__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Canned and tomato Products");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_cocoa__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Cocoa");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_coffee__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Coffee");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_coffee_premium__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Coffee");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_dairy__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Dairy");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_dfn__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Dfn");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_grains__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Garins");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_juices_and_beverages__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Juices and Beverages");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_meat__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Meat");
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_molasses_and_fi__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Molasses");
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_oils_and_oilseeds__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("oil and OilSeeds");
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_spices__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Spices");
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_sugar__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Spices");
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:ch_sugar_premium__c")
                    {
                        Boolean b = Convert.ToBoolean((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                        if (b)
                            entitlementslist.Add("Sugar");
                    }


                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:EndDate__c")
                    {
                        DateTime dt1;
                        DateTime dt2;
                        string strDate1;
                        string strDate2;
                        strDate1 = (((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower();
                        strDate2 = DateTime.Today.ToString("yyyy-MM-dd");
                        dtxyz = Convert.ToDateTime(strDate1);

                    }
                    


                }


            }

            foreach (string vm in entitlementslist)
            {
                if(vm != null && vm != string.Empty && vm != "500")
                entitlementslistFinal.Add(new SubscriptionViewModel()
                {
                    Publication = vm,
                    Expiration = dtxyz
                });
            }

            return entitlementslistFinal;
        }
        public enterprise.QueryResult GetEntitlementfromSalesForce(string url, string sessionid, string userId, string email)
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

                // query condition is added based on whether request is through transparent ip by checking email of pseudo user
                if (email == Sitecore.Configuration.Settings.GetSetting("SalesforcePseudoUser.Name"))
                {
                    query += " " + " where IP_Address__c = '" + "192.168.123.123" + "'";
                }
                else
                {
                    query += " " + "where User__c = '" + userId + "'";
                }

                queryClient.query(
                              header, //sessionheader
                              null, //queryoptions
                              null, //mruheader
                              null, //packageversionheader
                              query, //SOQL query
                              out queryresult
                           );

                //results


            }

            return queryresult;

        }

        public IEnumerable<ITaxonomy_Item> GeTaxonomyItemsByProductCode(string productCode)
        {
            IEnumerable<ITaxonomy_Item> taxnomyItems = new List<ITaxonomy_Item>();
            var verticalRootItem = GlassModel.GetAncestors<IVertical_Root>().FirstOrDefault();
            if (verticalRootItem != null)
            {
                var siteRoot =
                    verticalRootItem._ChildrenWithInferType.OfType<ISite_Root>()
                        .FirstOrDefault(eachChild => eachChild.Publication_Code.Equals(productCode));
                if (siteRoot != null)
                {
                    var pubItem = siteRoot._ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault()
                        ._ChildrenWithInferType.OfType<IAccount_Landing_Page>().FirstOrDefault()
                        ._ChildrenWithInferType.OfType<ISubscriptions_Page>().FirstOrDefault();

                    if (pubItem != null)
                        return pubItem.Taxonomies;
                }
            }
            return taxnomyItems;
        }

        public bool ShowRenewButton(ISubscription subscription)
        {
            //if all subscriptions of this type are within renew range and this subscription is not multi-user 
            return _subcriptions
                            .Where(a => a.ProductCode.Equals(subscription.ProductCode))
                            .All(b => WithinRenewRange(b.ExpirationDate))
                    && !IsMultiUser(subscription.SubscriptionType);
        }

        public bool ShowSubscribeButton(string productCode)
        {
            //if there aren't any valid subscriptions
            return (SubscriptionBtnSettings.ContainsKey(productCode)) && !RenewBtnSettings[productCode];
        }

       

        public bool IsMultiUser(string subscriptionType)
        {
            return subscriptionType.ToLower().Equals("multi-user");
        }

        public bool WithinRenewRange(DateTime dt)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
            return days < 119 && days >= 0;
        }

        public bool IsValidSubscription(ISubscription s)
        {
            // (08/31/2013) - (08/01/2013) = 31
            int days = Convert.ToInt16((s.ExpirationDate - DateTime.Now).TotalDays);
            return days > 0;
        }

        public string OffSiteRenewLink => GlassModel.Off_Site_Renew_Link?.Url ?? "#";
        public string SubscriptionHeaderSubTitle => GlassModel.Body;
	    public string SubjectType => TextTranslator.Translate("Subscriptions.SubjectType");
        public string Subscribe => TextTranslator.Translate("Subscriptions.Subscribe");
        public string Subscribed => TextTranslator.Translate("Subscriptions.Subscribed");
        public string Renew => TextTranslator.Translate("Subscriptions.Renew");
        public string OffSiteSubscriptionLink => GlassModel.Off_Site_Subscription_Link?.Url ?? "#";
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string Title => TextTranslator.Translate("Subscriptions.Title");
		public string PublicationText => TextTranslator.Translate("Subscriptions.Publication");
		public string SubscriptionTypeText => TextTranslator.Translate("Subscriptions.SubscriptionType");
		public string ExpirationDateText => TextTranslator.Translate("Subscriptions.ExpirationDate");
		public string ActionText => TextTranslator.Translate("Subscriptions.Action");
        public string BottomNotation => GlassModel.Bottom_Notation;
    }
}

