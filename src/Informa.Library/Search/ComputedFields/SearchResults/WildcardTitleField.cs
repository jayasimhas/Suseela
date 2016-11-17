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
using Informa.Library.Logging;

namespace Informa.Library.Search.ComputedFields.SearchResults {
    
    public class WildcardTitleField : BaseGlassComputedField<I___BasePage> {
        public override object GetFieldValue(I___BasePage indexItem) {

            if (indexItem == null)
                return string.Empty;

            return (indexItem.Title != null)
                ? HttpUtility.HtmlDecode(indexItem.Title).Replace("\n", "").Replace("\r", "").Trim()
                : string.Empty;
        }
    }
}
