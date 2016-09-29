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

namespace Informa.Web.ViewModels
{
    [AutowireService]
	public class AnalyticsViewModel : GlassViewModel<I___BasePage>, IAnalyticsViewModel {

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
            IDCDReader dcdReader) {

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
		public string GetArticleTaxonomy(Guid itemId) {
            return Article != null ? ArticleSearch.GetArticleTaxonomies(Article._Id, itemId) : string.Empty;
        }
        public string Article_Entitlement => GetArticleEntitlements();
        public bool IsFree => Article?.Free ?? false;
        public bool IsFreeWithRegistration => Article?.Free_With_Registration ?? false;
        public string GetArticleEntitlements() {
            if (IsFree) {
                return "Free View";
            }
            if (IsFreeWithRegistration || IsEntitledProductItemContext.IsEntitled(Article)) {
                return "Entitled Full View";
            }
            return "Unentitled Abstract View";
        }
        public string ArticleRegions => GetArticleTaxonomy(ItemReferences.RegionsTaxonomyFolder);
        public string ArticleSubject => GetArticleTaxonomy(ItemReferences.SubjectsTaxonomyFolder);
        public string ArticleTherapy => GetArticleTaxonomy(ItemReferences.TherapyAreasTaxonomyFolder);
        public string SiteEnvrionment => SiteSettings.GetSetting("Env.Value", string.Empty);
        public bool IsUserLoggedIn => AuthenticatedUserContext.IsAuthenticated;
        public string UserName => AuthenticatedUserContext.User?.Name ?? string.Empty;

        public string UserCompany => UserCompanyContext?.Company?.Name;
        public string CorporateName => UserCompanyContext?.Company?.Name;
        public string UserEmail => UserProfileContext.Profile?.Email ?? string.Empty;
        public string UserIndustry => UserProfileContext.Profile?.JobIndustry ?? string.Empty;
        public string CompanyId => UserCompanyContext?.Company?.Id;
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
            if (IsFreeWithRegistration) {
                return "Free Trial";
            }

			if (context.Company == null)
			{
				return "Free User";
			}

			if (context.Company.Type == CompanyType.TransparentIP)
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
			var ids = string.Join("|", UserEntitlementsContext.Entitlements.Select(i => $"'{i.OpportunityLineItemId}'"));
			return string.IsNullOrWhiteSpace(ids) ? string.Empty : ids;
		}
    }
}