using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI.WebControls;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Global;
using Sitecore.Data.Items;

namespace Informa.Library.Services.Global {
    public interface IGlobalSitecoreService {
        IInforma_Bar GetInformaBar();
        IEnumerable<ListItem> GetSalutations();
        IEnumerable<ListItem> GetNameSuffixes();
        IEnumerable<ListItem> GetJobFunctions();
        IEnumerable<ListItem> GetJobIndustries();
        IEnumerable<ListItem> GetPhoneTypes();
        IEnumerable<ListItem> GetCountries();
        T GetItem<T>(Guid g) where T : class;
        T GetItem<T>(string id) where T : class;
        Item GetItemByTemplateId(string templateId);
        string GetPageTitle(I___BasePage page);
        ISite_Root GetSiteRootAncestor(Guid g);
        string GetPublicationName(Guid g);
    }
}
