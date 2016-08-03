using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Informa.Library.Utilities.WebApi.Filters
{
    public class CSPFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var response = filterContext.HttpContext.Response;
            response.AddHeader("Content-Security-Policy", "script-src 'self'");
            response.AddHeader("X-WebKit-CSP", "script-src 'self'");
            response.AddHeader("X-Content-Security-Policy", "script-src 'self'");
            base.OnActionExecuting(filterContext);
        }
    }
}
