using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;

namespace Informa.Library.Services.AccountManagement {

    [AutowireService(LifetimeScope.SingleInstance)]
    public class AccountManagementService : IAccountManagementService
    {

        private readonly IAuthenticatedUserContext AuthenticatedUserContext;

        public AccountManagementService(IAuthenticatedUserContext authenticatedUserContext)
        {
            AuthenticatedUserContext = authenticatedUserContext;
        }

        public bool IsRestricted(I___BasePage page)
        {
            bool isRestrictedByRegistration = page is IGeneral_Content_Page
                    ? ((IGeneral_Content_Page)page).Restrict_To_Registered_Users
                    : false;

            return (isRestrictedByRegistration && !AuthenticatedUserContext.IsAuthenticated);
        }
    }
}
