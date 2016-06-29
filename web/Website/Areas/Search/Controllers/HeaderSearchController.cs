using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Informa.Library.CustomSitecore.Mvc;
using Jabberwocky.Glass.Autofac.Mvc.Attributes;
using SolrNet.Utils;
using Velir.Search.Core.Reference;

namespace Informa.Web.Areas.Search.Controllers
{
    public class HeaderSearchModel
    {
        public string SearchTerm { get; set; }
    }

    public class HeaderSearchController : GlassController
    {
        [HttpGetOrInvalidHttpPost]
        public ActionResult Search()
        {
            return View("~/Views/Search/HeaderSearch.cshtml");
        }

        [ValidHttpPost]
        [SitecoreValidateAntiForgeryToken]
        public ActionResult Search(HeaderSearchModel viewModel)
        {
           viewModel.SearchTerm = string.IsNullOrEmpty(viewModel.SearchTerm) ? string.Empty : viewModel.SearchTerm;

            //TODO put in non hardcoded values
            return Redirect(string.Format("{0}#?{1}={2}", "/search", SiteSettings.QueryString.QueryKey, HttpUtility.UrlEncode(viewModel.SearchTerm)));
        }
    }
}