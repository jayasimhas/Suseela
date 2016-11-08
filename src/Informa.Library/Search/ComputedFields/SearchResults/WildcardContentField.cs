using HtmlAgilityPack;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Library.Utilities.TokenMatcher;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Autofac;

namespace Informa.Library.Search.ComputedFields.SearchResults {
    
    public class WildcardContentField : BaseGlassComputedField<I___BasePage> {
        public override object GetFieldValue(I___BasePage indexItem) {

            if (indexItem == null)
                return string.Empty;

            IDCDTokenMatchers dcd;
            using (var scope = AutofacConfig.ServiceLocator.BeginLifetimeScope()) {
                dcd = scope.Resolve<IDCDTokenMatchers>();
            }

            StringBuilder sb = new StringBuilder();
            if (indexItem.Body != null) {
                string b = (dcd == null)
                    ? indexItem.Body
                    : dcd.ProcessDCDTokens(indexItem.Body);
                var doc = new HtmlDocument();
                doc.LoadHtml(b);
                sb.AppendFormat("{0} ", doc.DocumentNode.InnerText);
            }

            if (indexItem.Title != null)
                sb.AppendFormat("{0} ", HttpUtility.HtmlDecode(indexItem.Title));

            if (indexItem.Sub_Title != null)
                sb.AppendFormat("{0} ", HttpUtility.HtmlDecode(indexItem.Sub_Title));

            return sb.ToString().Replace("\n", "").Replace("\r", "").Trim();
        }
    }
}
