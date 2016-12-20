using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
    public class ProcessUserRequestController : Controller
    {
        // GET: ProcessUserRequest
        public ActionResult Index(string code, string state)
        {

            return Redirect(state);
        }
    }
}