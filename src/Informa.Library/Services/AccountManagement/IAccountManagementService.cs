using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.AccountManagement {
    public interface IAccountManagementService {
        bool IsUserRestricted(I___BasePage page);
        bool IsPageRestrictionSet(I___BasePage page);
        bool IsUserRestrictedByRegistration(Guid page);
        bool IsUserRestrictedByEntitlement(Guid page, string path, string entitlementCode = null);
        bool IsUserEntitled(string path, string entitlementCode = null);
    }
}
