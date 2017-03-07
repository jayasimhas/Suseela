using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.Utilities.References;
using Informa.Library.User.Entitlement;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.MarketData;

namespace Informa.Library.Services.AccountManagement
{

    [AutowireService(LifetimeScope.SingleInstance)]
    public class AccountManagementService : IAccountManagementService
    {

        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        private readonly IItemReferences ItemReferences;
        private readonly IUserEntitlementsContext UserEntitlementsContext;
        private readonly ISiteRootsContext SiteRootsContext;


        public AccountManagementService(
            IAuthenticatedUserContext authenticatedUserContext,
            IItemReferences itemReferences,
            IUserEntitlementsContext userEntitlementsContext,
            ISiteRootsContext siteRootsContext
            )
        {
            AuthenticatedUserContext = authenticatedUserContext;
            ItemReferences = itemReferences;
            UserEntitlementsContext = userEntitlementsContext;
            SiteRootsContext = siteRootsContext;

        }

        public bool IsUserRestricted(I___BasePage page)
        {
            bool isCompanyLandingPage = page is ICompany_Landing_Page;
            if (isCompanyLandingPage)
            {
                ICompany_Landing_Page CompanyLandingPage = (ICompany_Landing_Page)page;
                return (IsUserRestrictedByRegistration(CompanyLandingPage.Restrict_Access) || IsUserRestrictedByEntitlement(CompanyLandingPage.Restrict_Access, CompanyLandingPage._Path)) ? true : false;
            }
            bool isCompanyDetailPage = page is ICompany_Detail_Page;
            if (isCompanyDetailPage)
            {
                ICompany_Detail_Page CompanyDetailPage = (ICompany_Detail_Page)page;
                return (IsUserRestrictedByRegistration(CompanyDetailPage.Restrict_Access) || IsUserRestrictedByEntitlement(CompanyDetailPage.Restrict_Access, CompanyDetailPage._Path)) ? true : false;
            }
            bool isCompanyGraphDetailPage = page is ICompany_Graph_Detail_Page;
            if (isCompanyGraphDetailPage)
            {
                ICompany_Graph_Detail_Page CompanyGraphDetailPage = (ICompany_Graph_Detail_Page)page;
                return (IsUserRestrictedByRegistration(CompanyGraphDetailPage.Restrict_Access) || IsUserRestrictedByEntitlement(CompanyGraphDetailPage.Restrict_Access, CompanyGraphDetailPage._Path)) ? true : false;
            }
            bool isCompanyPeerGroupPage = page is ICompany_Peer_Group_Detail_Page;
            if (isCompanyPeerGroupPage)
            {
                ICompany_Peer_Group_Detail_Page CompanyPeerGroupPage = (ICompany_Peer_Group_Detail_Page)page;
                return (IsUserRestrictedByRegistration(CompanyPeerGroupPage.Restrict_Access) || IsUserRestrictedByEntitlement(CompanyPeerGroupPage.Restrict_Access, CompanyPeerGroupPage._Path)) ? true : false;
            }
            bool isMarketDataDetailPage = page is IMarketData_Detail_Page;
            if (isMarketDataDetailPage)
            {
                IMarketData_Detail_Page MarketDataDetailPage = (IMarketData_Detail_Page)page;
                return (IsUserRestrictedByRegistration(MarketDataDetailPage.Restrict_Access) || IsUserRestrictedByEntitlement(MarketDataDetailPage.Restrict_Access, MarketDataDetailPage._Path)) ? true : false;
            }
            bool isGeneralContentPage = page is IGeneral_Content_Page;
            if (!isGeneralContentPage)
                return false;

            IGeneral_Content_Page gPage = (IGeneral_Content_Page)page;
            if (gPage != null && gPage.Enable_Entitlement && !string.IsNullOrEmpty(gPage.Entitlement_Code))
            {
                return (IsUserRestrictedByRegistration(gPage.Restrict_Access) || IsUserRestrictedByEntitlement(gPage.Restrict_Access, gPage._Path, gPage.Entitlement_Code)) ? true : false;
            }
            else
            {
                return (IsUserRestrictedByRegistration(gPage.Restrict_Access) || IsUserRestrictedByEntitlement(gPage.Restrict_Access, gPage._Path)) ? true : false;
            }

        }


        public bool IsPageRestrictionSet(I___BasePage page)
        {
            bool isCompanyLandingPage = page is ICompany_Landing_Page;
            if (isCompanyLandingPage)
            {
                ICompany_Landing_Page CompanyLandingPage = (ICompany_Landing_Page)page;
                return ((CompanyLandingPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || CompanyLandingPage.Equals(ItemReferences.FreeWithEntitlement)) && CompanyLandingPage.Show_Summary_When_Entitled);
            }
            bool isCompanyDetailPage = page is ICompany_Detail_Page;
            if (isCompanyDetailPage)
            {
                ICompany_Detail_Page CompanyDetailPage = (ICompany_Detail_Page)page;
                return ((CompanyDetailPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || CompanyDetailPage.Equals(ItemReferences.FreeWithEntitlement)) && CompanyDetailPage.Show_Summary_When_Entitled);
            }
            bool isCompanyGraphDetailPage = page is ICompany_Graph_Detail_Page;
            if (isCompanyGraphDetailPage)
            {
                ICompany_Graph_Detail_Page CompanyGraphDetailPage = (ICompany_Graph_Detail_Page)page;
                return ((CompanyGraphDetailPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || CompanyGraphDetailPage.Equals(ItemReferences.FreeWithEntitlement)) && CompanyGraphDetailPage.Show_Summary_When_Entitled);
            }
            bool isCompanyPeerGroupPage = page is ICompany_Peer_Group_Detail_Page;
            if (isCompanyPeerGroupPage)
            {
                ICompany_Peer_Group_Detail_Page CompanyPeerGroupPage = (ICompany_Peer_Group_Detail_Page)page;
                return ((CompanyPeerGroupPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || CompanyPeerGroupPage.Equals(ItemReferences.FreeWithEntitlement)) && CompanyPeerGroupPage.Show_Summary_When_Entitled);
            }
            bool isMarketDataDetailPage = page is IMarketData_Detail_Page;
            if (isMarketDataDetailPage)
            {
                IMarketData_Detail_Page MarketDataDetailPage = (IMarketData_Detail_Page)page;
                return ((MarketDataDetailPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || MarketDataDetailPage.Equals(ItemReferences.FreeWithEntitlement)) && MarketDataDetailPage.Show_Summary_When_Entitled);
            }
            bool isGeneralContentPage = page is IGeneral_Content_Page;
            if (!isGeneralContentPage)
                return false;
            IGeneral_Content_Page gPage = (IGeneral_Content_Page)page;
            return ((gPage.Restrict_Access.Equals(ItemReferences.FreeWithRegistration) || gPage.Equals(ItemReferences.FreeWithEntitlement)) && gPage.Show_Summary_When_Entitled);

        }

        public bool IsUserRestrictedByRegistration(Guid Restrict_Access)
        {
            bool isRestricted = Restrict_Access.Equals(ItemReferences.FreeWithRegistration);
            return (isRestricted && !AuthenticatedUserContext.IsAuthenticated);
        }

        public bool IsUserRestrictedByEntitlement(Guid Restrict_Access, string path, string entitlementCode = null)
        {
            if (!string.IsNullOrEmpty(entitlementCode))
            {
                return (!IsUserEntitled(path, entitlementCode));
            }
            else
            {
                bool isRestricted = Restrict_Access.Equals(ItemReferences.FreeWithEntitlement);
                return (isRestricted && !IsUserEntitled(path));
            }
        }

        public bool IsUserEntitled(string _path, string entitlementCode = null)
        {
            var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => _path.StartsWith(sr._Path));
            if (siteRoot == null)
                return false;

            return !string.IsNullOrEmpty(entitlementCode) ?
                UserEntitlementsContext.Entitlements.Any(e => string.Equals(e.ProductCode, siteRoot.Publication_Code + "." + entitlementCode, StringComparison.CurrentCultureIgnoreCase)) :
                UserEntitlementsContext.Entitlements.Any(e => string.Equals(e.ProductCode, siteRoot.Publication_Code, StringComparison.CurrentCultureIgnoreCase)) || UserEntitlementsContext.Entitlements.Any();
        }

    }
}
