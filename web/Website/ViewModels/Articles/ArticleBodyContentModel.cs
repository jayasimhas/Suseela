using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Search.ComputedFields.SearchResults.Converter.MediaTypeIcon;
using Informa.Library.Services.Article;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Entitlement;
using Informa.Library.Utilities.Extensions;
using Informa.Models.FactoryInterface;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Library.Site;
using enterprise = Informa.Library.Salesforce.SFv2;
using System.ServiceModel;
using System.IO;
using System.Xml.Serialization;
using System.Xml.Linq;
using Informa.Library.User;
using Informa.Library.User.Authentication.Web;
using System.Xml;

namespace Informa.Web.ViewModels.Articles
{
	public class ArticleBodyContentModel : ArticleEntitledViewModel
	{
		public readonly ICallToActionViewModel CallToActionViewModel;
		protected readonly ITextTranslator TextTranslator;
		protected readonly IArticleService ArticleService;
        //JIRA IPMP-56
        protected readonly ISiteRootContext SiteRootContext;
        private readonly Lazy<string> _lazyBody;
        protected readonly IUserSession UserSession;
        IWebAuthenticateUser WebAuthenticateUser;
        public ArticleBodyContentModel(
						IArticle model,
						IIsEntitledProducItemContext entitledProductContext,
						ITextTranslator textTranslator,
						ICallToActionViewModel callToActionViewModel,
						IArticleService articleService,
                        IAuthenticatedUserContext authenticatedUserContext,
                        ISiteRootContext siteRootContext,
                        ISitecoreUserContext sitecoreUserContext,
                        IUserSession userSession,
            IWebAuthenticateUser webAuthenticateUser)
                        
						: base(entitledProductContext, authenticatedUserContext, sitecoreUserContext)
         {
			TextTranslator = textTranslator;
			CallToActionViewModel = callToActionViewModel;
			ArticleService = articleService;
            // JIRA IPMP-56
            SiteRootContext = siteRootContext;
            WebAuthenticateUser = webAuthenticateUser;
            UserSession = userSession;
            _lazyBody = new Lazy<string>(() => IsFree || (IsFreeWithRegistration && AuthenticatedUserContext.IsAuthenticated) || IsEntitled() ? ArticleService.GetArticleBody(model) : "");
		}
      

        public string Title => GlassModel.Title;
		public string Sub_Title => GlassModel.Sub_Title;
		public bool DisplayLegacyPublication => LegacyPublicationNames.Any();

		public IEnumerable<string> LegacyPublicationNames => ArticleService.GetLegacyPublicationNames(GlassModel, SiteRootContext.Item.Legacy_Brand_Active);// JIRA IPMP-56

        public string LegacyPublicationText => ArticleService.GetLegacyPublicationText(GlassModel, SiteRootContext.Item.Legacy_Brand_Active,GlassModel.Escenic_ID,GlassModel.Legacy_Article_Number);  // JIRA IPMP-56      

        private string _summary;
		public string Summary => _summary ?? (_summary = ArticleService.GetArticleSummary(GlassModel));

		private IEnumerable<IPersonModel> _authors;
		public IEnumerable<IPersonModel> Authors
				=> _authors ?? (_authors = GlassModel.Authors.Select(x => new PersonModel(x)));

		private DateTime? _date;
		public DateTime Date
		{
			get
			{
				if (!_date.HasValue)
				{
					_date = GlassModel.GetDate();
				}
				return _date.Value;
			}
		}
		public string Category => GlassModel.Article_Category;
        //public string Body => _lazyBody.Value;
        public string Body => GetArticleBody();
        public string ContentType => GlassModel.Content_Type?.Item_Name;
		public MediaTypeIconData MediaTypeIconData => ArticleService.GetMediaTypeIconData(GlassModel);
		public IFeaturedImage Image => new ArticleFeaturedImage(GlassModel);
		public string FeaturedImageSource => TextTranslator.Translate("Article.FeaturedImageSource");
        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");

        // JIRA IPMP-56
        public bool IsActiveLegacyBrand => SiteRootContext.Item.Legacy_Brand_Active;
        public List<string> LagacyBrandUrl => ArticleService.GetLegacyPublicationNames(GlassModel, SiteRootContext.Item.Legacy_Brand_Active).ToList<string>();


        public string GetArticleBody()
        {
            var sfUser = WebAuthenticateUser.AuthenticatedUser;

            string userId = sfUser?.UserId;
            string url = sfUser?.SalesForceURL;
            string sessionid = sfUser?.SalesForceSessionId;
            string email = sfUser?.Email;

            enterprise.QueryResult queryresult;
            queryresult=GetEntitlementfromSalesForce(url, sessionid, userId,email);

                if (queryresult.records != null)
                {

                    foreach (var record in queryresult.records)
                    {

                       
                        List<String> Userentitlement = ConvertObjectToXMLString(queryresult.records);
                        //List<string> TaxonomiesList = (List<string>)GlassModel.Taxonomies;
                        IEnumerable<object> TaxonomiesList = (IEnumerable<object>)GlassModel.Taxonomies;

                        //List<int> Taxonomlist = new List<int>();
                        List<string> Taxonomlist = new List<string>();
                        for (int i = 0; i < GlassModel.Taxonomies.ToList<object>().Count; i++)
                        {
                            Taxonomlist.Add((GlassModel.Taxonomies.ToArray())[i]._Id.ToString());
                        }


                        var results = Taxonomlist.Intersect(Userentitlement, StringComparer.OrdinalIgnoreCase);
                        if (results.Count() > 0)
                        {
                            return "You are  viewing the entitled  content" + _lazyBody.Value;
                        }
                        else
                            return "You are not entitle for viewing this content";
                    }
                    return string.Empty;
                }
                else return string.Empty;

            }

            //return string.Empty;
        //}

        public enterprise.QueryResult  GetEntitlementfromSalesForce(string url, string sessionid, string userId,string email)
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




        List<String> ConvertObjectToXMLString(object classObject)
        {

            List<string> entitlementslist = new List<string>();

            string xmlString = null;
            XmlSerializer xmlSerializer = new XmlSerializer(classObject.GetType());
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
                // var nodes = xml.SelectSingleNode("ArrayOfSObject/sObject/Account__c").InnerText;
                for (int i = 1; i < (((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes.Count; i++)
                {
                    if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Biofuels_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Cocoa_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }
                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Coffee_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Canned_and_Tomato_Products_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Coffee_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Coffee_Premium_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }


                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:DFN_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Dairy_Code__c>")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }


                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Frozen_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }


                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Grains_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "Inherit_Transparent_Entitlements__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Meat_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText);
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Meter_Limit__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Molasses_and_FI_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }


                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Oils_and_Oilseeds_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:Juices_and_Beverages_Code__c")
                    {
                        entitlementslist.Add((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower());
                    }

                    else if ((((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].Name == "q1:EndDate__c")
                    {
                        DateTime dt1;
                        DateTime dt2;
                        string strDate1;
                        string strDate2;
                        strDate1 = (((System.Xml.XmlElement)(xml.SelectSingleNode("ArrayOfSObject/sObject")))).ChildNodes[i].InnerText.ToLower();
                        strDate2 = DateTime.Today.ToString("yyyy-MM-dd");

                       

                        if (DateTime.TryParse(strDate1, out dt1) && DateTime.TryParse(strDate2, out dt2))
                        {
                            if (dt1.Date < dt2.Date)
                            {
                                entitlementslist.Clear();
                            }
                        }



                    }



                }


            }
            return entitlementslist;
        }





    }




}