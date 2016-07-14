using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Library.Services.AccountManagement {
    public interface IAccountManagementService {
        bool IsRestricted(I___BasePage page);
        bool IsRestrictedByRegistration(IGeneral_Content_Page page);
        bool IsRestrictedByEntitlement(IGeneral_Content_Page page);
        bool IsEntitled(IGeneral_Content_Page page);
    }
}
