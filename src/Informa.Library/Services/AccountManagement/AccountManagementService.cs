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

namespace Informa.Library.Services.AccountManagement {
    
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
            ISiteRootsContext siteRootsContext)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            ItemReferences = itemReferences;
            UserEntitlementsContext = userEntitlementsContext;
            SiteRootsContext = siteRootsContext;
        }

        public bool IsRestricted(I___BasePage page)
        {
            bool isGeneralContentPage = page is IGeneral_Content_Page;
            if (!isGeneralContentPage)
                return false;

            IGeneral_Content_Page gPage = (IGeneral_Content_Page)page;
            return (IsRestrictedByRegistration(gPage) || IsRestrictedByEntitlement(gPage))
                ? true
                : false;
        }

        public bool IsRestrictedByRegistration(IGeneral_Content_Page page) {
            bool isRestricted = page.Restrict_Access.Equals(ItemReferences.FreeWithRegistration);
            return (isRestricted && !AuthenticatedUserContext.IsAuthenticated);
        }

        public bool IsRestrictedByEntitlement(IGeneral_Content_Page page) {
            bool isRestricted = ((IGeneral_Content_Page)page).Restrict_Access.Equals(ItemReferences.FreeWithEntitlement);            
            return (isRestricted && !IsEntitled(page));
        }

        public bool IsEntitled(IGeneral_Content_Page page) {
            var siteRoot = SiteRootsContext.SiteRoots.FirstOrDefault(sr => page._Path.StartsWith(sr._Path));
            if (siteRoot == null)
                return false;

            return UserEntitlementsContext.Entitlements.Any(e => string.Equals(e.ProductCode, siteRoot.Publication_Code, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
