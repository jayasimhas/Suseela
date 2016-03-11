using Sitecore.Data;
using Sitecore.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Models.DCD
{
    public class DCDConstants
    {
        public const string CompanyTokenFormat = "[C#{0}:{1}]";

        public const string DealTokenRegex = @"\[W#(.*?)\]";
        public const string CompanyTokenRegex = @"\[C#(.*?)\]";

        public static readonly string EmailNoReplySenderAddress = Sitecore.Configuration.Settings.GetSetting("Mail.MailServerFromAddress");
        public static readonly string BusinessAcronym = "IBI";

        public static readonly ID DCDConfigurationItemID = new ID("{52461CA2-7C9F-483C-B5F2-433B9351FA6F}");
    }
}
