using System;
using System.Web.Mvc;

namespace Informa.Web.Areas.Article.Controllers
{
    public class TableController : Controller
    {
        public ActionResult Mobile(Guid tableId)
        {
            return View();
        }
    }
}