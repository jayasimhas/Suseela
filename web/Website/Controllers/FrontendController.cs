using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Glass.Mapper.Sc.Web.Mvc;
using Jabberwocky.Glass.Models;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;

namespace Informa.Web.Controllers
{
    public class FrontendController : SitecoreController
    {
        // GET: Frontend
        public ActionResult Template(string page)
        {
            //Sitecore.Context.Page.GetPlaceholder("main_content")
            return View("Index");
        }
    }
}