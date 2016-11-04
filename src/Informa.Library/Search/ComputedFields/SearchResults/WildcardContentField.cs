using HtmlAgilityPack;
using Informa.Library.Services.Search.Fields.Base;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Informa.Library.Search.ComputedFields.SearchResults {
    
    public class WildcardContentField : BaseGlassComputedField<I___BasePage> {
        public override object GetFieldValue(I___BasePage indexItem) {

            if (indexItem == null)
                return string.Empty;

            StringBuilder sb = new StringBuilder();
            if (indexItem.Body != null) {
                var doc = new HtmlDocument();
                doc.LoadHtml(indexItem.Body);
                sb.AppendFormat("{0} ", doc.DocumentNode.InnerText);
            }

            if (indexItem.Title != null)
                sb.AppendFormat("{0} ", HttpUtility.HtmlDecode(indexItem.Title));

            if (indexItem.Sub_Title != null)
                sb.AppendFormat("{0} ", HttpUtility.HtmlDecode(indexItem.Sub_Title));

            return sb.ToString();
        }
    }
}
