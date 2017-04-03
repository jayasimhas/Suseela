using System;
using System.Linq;
using Informa.Library.Article.Search;
using Informa.Library.Company;
using Informa.Library.Site;
using Informa.Library.Subscription.User;
using Informa.Library.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Informa.Library.User.Entitlement;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.References;
using Informa.Library.Utilities.Settings;
using Informa.Library.Utilities.WebUtils;
using Informa.Library.Wrappers;
using Informa.Model.DCD;
using Informa.Models.DCD;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Social.Infrastructure.Utils;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using System.Collections.Generic;
using Informa.Library.Services.Global;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using Jabberwocky.Glass.Autofac.Util;
using Autofac;
using System.Compat.Web;
using Informa.Library.Services.Captcha;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Web.ViewModels
{
    [AutowireService]
    public class AnalyticsViewModel : GlassViewModel<I___BasePage>, IAnalyticsViewModel
    {

        public readonly IItemReferences ItemReferences;
        protected readonly IIsEntitledProducItemContext IsEntitledProductItemContext;
        protected readonly IArticleSearch ArticleSearch;
        protected readonly ISiteSettings SiteSettings;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IUserProfileContext UserProfileContext;
        protected readonly IUserSubscriptionsContext UserSubscriptionsContext;
        protected readonly IWebAuthenticateUser WebAuthenticateUser;
        protected readonly IUserEntitlementsContext UserEntitlementsContext;
        protected readonly IUserIpAddressContext UserIpAddressContext;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly IHttpContextProvider HttpContextProvider;
        protected readonly IDCDReader DcdReader;
        protected readonly ITaxonomyService TaxonomyService;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly IRecaptchaService RecaptchaSettings;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public AnalyticsViewModel(
            IItemReferences itemReferences,
            IIsEntitledProducItemContext isEntitledProductItemContext,
            IArticleSearch articleSearch,
            ISiteSettings siteSettings,
            IAuthenticatedUserContext authenticatedUserContext,
            IUserCompanyContext userCompanyContext,
            IUserProfileContext userProfileContext,
            IUserSubscriptionsContext userSubscriptionsContext,
            IWebAuthenticateUser webAuthenticateUser,
            IUserEntitlementsContext userEntitlementsContext,
            IUserIpAddressContext userIpAddressContext,
            ISiteRootContext siteRootContext,
            IHttpContextProvider httpContextProvider,
            IDCDReader dcdReader,
            ITaxonomyService taxonomyService, 
            IGlobalSitecoreService globalService, 
            IRecaptchaService recaptchaSettings,
            ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            ItemReferences = itemReferences;
            IsEntitledProductItemContext = isEntitledProductItemContext;
            ArticleSearch = articleSearch;
            SiteSettings = siteSettings;
            AuthenticatedUserContext = authenticatedUserContext;
            UserCompanyContext = userCompanyContext;
            UserProfileContext = userProfileContext;
            UserSubscriptionsContext = userSubscriptionsContext;
            WebAuthenticateUser = webAuthenticateUser;
            UserEntitlementsContext = userEntitlementsContext;
            UserIpAddressContext = userIpAddressContext;
            SiteRootContext = siteRootContext;
            HttpContextProvider = httpContextProvider;
            DcdReader = dcdReader;
            EntitlementType = GetEntitlementType(UserCompanyContext);
            UserEntitlements = GetUserEntitlements();
            SubscribedProducts = GetSubscribedProducts();
            OpportunityLineItemIds = GetOpportunityLineItemIds();
            OpportunityIds = GetOpportunityIds();
            TaxonomyService = taxonomyService;
            GlobalService = globalService;
            RecaptchaSettings = recaptchaSettings;
            SalesforceConfigurationContext = salesforceConfigurationContext;
        }


        public IArticle Article => GlassModel as IArticle;

        public string PublicationName => SiteRootContext.Item.Publication_Name;
        public string PublicationCode => SiteRootContext.Item.Publication_Code;
        public string PageTitleAnalytics => GlassModel?.Title ?? string.Empty;
        public string PageType => Sitecore.Context.Item.TemplateName;
        public string AdDomain => SiteRootContext.Item.Ad_Domain;
        public string ArticlePublishDate => (Article != null && Article.Actual_Publish_Date > DateTime.MinValue)
                    ? Article.Actual_Publish_Date.ToString("MM/dd/yyyy")
                    : DateTime.MinValue.ToString("MM/dd/yyyy");
        public string ArticleContentType => Article?.Content_Type?.Item_Name.Trim();
        public string ArcticleNumber => Article?.Article_Number;
        public bool ArticleEmbargoed => Article?.Embargoed ?? false;
        public string ArticleMediaType => Article?.Media_Type?.Item_Name;
        public string ArticleAuthors => (Article?.Authors == null)
                ? string.Empty
                : $"[{string.Join(",", Article.Authors.Select(x => $"'{x._Name.Trim()}'"))}]";
        public string UserEntitlementStatus => IsEntitledProductItemContext.IsEntitled(Article) ? "entitled" : "unentitled";
        public string GetArticleTaxonomy(Guid itemId)
        {
            return Article != null ? ArticleSearch.GetArticleTaxonomies(Article._Id, itemId) : string.Empty;
        }
        public string Article_Entitlement => GetArticleEntitlements();
        public bool IsFree => Article?.Free ?? false;
        public bool IsFreeWithRegistration => Article?.Free_With_Registration ?? false;
        public string GetArticleEntitlements()
        {
            if (IsFree)
            {
                return "Free View";
            }
            if (IsFreeWithRegistration || IsEntitledProductItemContext.IsEntitled(Article))
            {
                return "Entitled Full View";
            }
            return "Unentitled Abstract View";
        }
        public string ArticleRegions => GetArticleTaxonomy(ItemReferences.RegionsTaxonomyFolder);
        public string ArticleSubject => GetArticleTaxonomy(ItemReferences.PharmaSubjectsTaxonomyFolder);
        public string ArticleTherapy => GetArticleTaxonomy(ItemReferences.PharmaTherapyAreasTaxonomyFolder);
        public string SiteEnvrionment => SiteSettings.GetSetting("Env.Value", string.Empty);
        public bool IsUserLoggedIn => AuthenticatedUserContext.IsAuthenticated;
        public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;
        //ISW-1102 (No longer able to Email Article to friend + ReCaptcha now missing)
        public string CaptchaSiteKey => RecaptchaSettings.SiteKey;
        public string UserCompany => SalesforceConfigurationContext.IsNewSalesforceEnabled ? UserProfileContext?.Profile?.Company : UserCompanyContext?.Company?.Name;
        public string CorporateName => SalesforceConfigurationContext.IsNewSalesforceEnabled ? UserProfileContext?.Profile?.Company : UserCompanyContext?.Company?.Name;
        public string UserEmail => UserProfileContext.Profile?.Email ?? string.Empty;
        public string UserIndustry => UserProfileContext.Profile?.JobIndustry ?? string.Empty;
        public string CompanyId => SalesforceConfigurationContext.IsNewSalesforceEnabled ? string.Empty : UserCompanyContext?.Company?.Id;
        public string SubscribedProducts { get; }
        public string CustomTags => GlassModel?.Custom_Meta_Tags ?? string.Empty;
        public string ContactId => WebAuthenticateUser.AuthenticatedUser?.ContactId ?? string.Empty;
        public string AccountId
        {
            get
            {
                var accountInfo = WebAuthenticateUser.AuthenticatedUser?.AccountId;

                if (accountInfo == null || !accountInfo.Any())
                    return string.Empty;

                string allAccountIds = string.Join(",", accountInfo.Select(a => $"'{a}'"));
                return $"[{allAccountIds}]";
            }
        }
        public string UserEntitlements { get; }
        public string OpportunityIds { get; }
        public string OpportunityLineItemIds { get; }
        public string UserIp => UserIpAddressContext.IpAddress.ToString();
        public string DateCreated => (Sitecore.Context.Item != null && Sitecore.Context.Item.Statistics.Created > DateTime.MinValue)
                    ? Sitecore.Context.Item.Statistics.Created.ToServerTimeZone().ToString("MM/dd/yyyy")
                    : DateTime.MinValue.ToString("MM/dd/yyyy");
        public string PageDescription => GlassModel?.Meta_Description ?? string.Empty;
        public string PageTitleOverride => GlassModel?.Meta_Title_Override ?? string.Empty;
        public string MetaKeyWords => GlassModel?.Meta_Keywords ?? string.Empty;
        //ISW 338 Serving ads based on section taxonomy
        public string TaxonomyName=> GetTaxonomyDetails();
        public string ArticleEntitlements
        {
            get
            {
                if (IsFree)
                {
                    return "Free View";
                }

                if (IsFreeWithRegistration)
                {
                    return "Free With Registration View";
                }

                if (IsEntitledProductItemContext.IsEntitled(Article))
                {
                    return "Entitled Full View";
                }

                return "Abstract View";
            }
        }
        public string ContentEntitlementType => GetContentEntitlement(UserCompanyContext);
        public string EntitlementType { get; }
        public string DealName
        {
            get
            {
                if (!(GlassModel is IDeal_Page)) return null;
                var recordNumber = UrlUtils.GetLastUrlSement(HttpContextProvider.Current);
                var deal = DcdReader.GetDealByRecordNumber(recordNumber);
                return deal?.Title;
            }
        }

        public string CompanyName
        {
            get
            {
                if (!(GlassModel is ICompany_Page)) return null;
                var recordNumber = UrlUtils.GetLastUrlSement(HttpContextProvider.Current);
                var company = DcdReader.GetCompanyByRecordNumber(recordNumber);
                return company?.Title;
            }
        }



        /***** Privates *****/
        private string GetContentEntitlement(IUserCompanyContext context)
        {
            if (!IsEntitledProductItemContext.IsEntitled(Article))
            {
                return "unentitled";
            }

            return $"entitled - {GetEntitlementType(context)}";
        }

        private string GetEntitlementType(IUserCompanyContext context)
        {
            if (IsFreeWithRegistration)
            {
                return "Free Trial";
            }

            if (!SalesforceConfigurationContext.IsNewSalesforceEnabled && context.Company == null)
            {
                return "Free User";
            }

            if (!SalesforceConfigurationContext.IsNewSalesforceEnabled && context.Company.Type == CompanyType.TransparentIP)
            {
                return "Transparent IP";
            }
            return "Corporate";
        }

	    private string GetUserEntitlements()
	    {            
            string allEntitlements = string.Join(",", UserEntitlementsContext.Entitlements.Select(a => $"'{a.ProductCode}'"));
			return !string.IsNullOrEmpty(allEntitlements) ? $"[{allEntitlements}]" : string.Empty;
		}


        private string GetSubscribedProducts()
        {
            var subscriptions = UserSubscriptionsContext.Subscriptions;
            string allSubscriptions = subscriptions == null ? string.Empty : string.Join(",", UserSubscriptionsContext.Subscriptions.Select(a => $"'{a.ProductCode}'"));
            return !string.IsNullOrEmpty(allSubscriptions) ? $"[{allSubscriptions}]" : string.Empty;
        }

        private string GetOpportunityIds()
        {
            var UserEntitlements = UserEntitlementsContext.Entitlements;
            UserEntitlements = UserEntitlements.Where(i => i.ProductCode.ToLower() == PublicationCode.ToLower());
            var ids = string.Join("|", UserEntitlements.Select(i => $"'{i.OpportunityId}'"));
            return string.IsNullOrWhiteSpace(ids) ? string.Empty : ids;
        }

	    private string GetOpportunityLineItemIds()
	    {
			var ids = string.Join("|", UserEntitlementsContext.Entitlements.Where(w => w.ProductCode.ToLower() == SiteRootContext.Item.Publication_Code.ToLower()).Select(i => $"'{i.OpportunityLineItemId}'"));
			return string.IsNullOrWhiteSpace(ids) ? string.Empty : ids;
		}
        //ISW 338 Serving ads based on section taxonomy
        private string GetTaxonomyDetails()
        {
            string taxonaomyParentName=string.Empty;
            if (Article != null)
            {
                if (Article.Taxonomies != null)
                {
                    int taxonomyItemCount = 0;
                    foreach (ITaxonomy_Item item in Article.Taxonomies)
                    {
                        if (item != null && taxonomyItemCount < 3)
                        {
                            if (string.IsNullOrEmpty(taxonaomyParentName))
                            {
                               // taxonaomyParentName += TaxonomyService.GetTaxonomyParentFromItem(item).ToLower().Replace(" ",string.Empty) + ":" + item.Item_Name.ToLower();
                                taxonaomyParentName += (TaxonomyService.GetTaxonomyParentFromItem(item).ToLower().Replace(" ", string.Empty) + ":" + item.Item_Name.ToLower().Trim()).Replace("\r\n", "").Replace("\r", "").Replace("\n", "");
                                taxonomyItemCount++;
                            }
                            else
                            {
                                taxonaomyParentName += "|" + (TaxonomyService.GetTaxonomyParentFromItem(item).ToLower().Replace(" ", string.Empty) + ":" + item.Item_Name.ToLower().Trim()).Replace("\r\n","").Replace("\r","").Replace("\n","");
                                taxonomyItemCount++;
                            }
                           
                        }
                    }
                }
            }
            return  taxonaomyParentName;
        }
    }
}