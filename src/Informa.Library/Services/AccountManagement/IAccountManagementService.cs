using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;

namespace Informa.Library.Services.AccountManagement {
    public interface IAccountManagementService {
        bool IsRestricted(I___BasePage page);
    }
}
