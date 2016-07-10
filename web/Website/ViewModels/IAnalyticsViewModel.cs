using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels {
    public interface IAnalyticsViewModel {
		string PublicationName { get; }
        string PageTitleAnalytics { get; }
        string PageType { get; }
        string ArticlePublishDate { get; }
        IArticle Article { get; }
        string ArticleContentType { get; }
        string ArcticleNumber { get; }
        bool ArticleEmbargoed { get; }
        string ArticleMediaType { get; }
        string ArticleAuthors { get; }
        string UserEntitlementStatus { get; }
        string GetArticleTaxonomy(Guid itemId);
        string Article_Entitlement { get; }
        bool IsFree { get; }
        string GetArticleEntitlements();
        string ArticleRegions { get; }
        string ArticleSubject { get; }
        string ArticleTherapy { get; }
        string SiteEnvrionment { get; }
        bool IsUserLoggedIn { get; }
        string UserName { get; }
        string UserCompany { get; }
        string CorporateName { get; }
        string UserEmail { get; }
        string UserIndustry { get; }
        string CompanyId { get; }
        string SubscribedProducts { get; }
        string CustomTags { get; }
        string ContactId { get; }
        string AccountId { get; }
        string UserEntitlements { get; }
        string UserIp { get; }
        string DateCreated { get; }
        string PageDescription { get; }
        string PageTitleOverride { get; }
        string MetaKeyWords { get; }
		string ArticleEntitlements { get; }
		string ContentEntitlementType { get; }
		string EntitlementType { get; }
	    string OpportunityIds { get; }
	    string OpportunityLineItemIds { get; }
    }
}
