using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Authors {
    public interface IAuthorAlertOptions {
        ITopic_Alert_Options SetOptions(ITopic_Alert_Options options);
    }
}
