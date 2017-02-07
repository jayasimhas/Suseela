using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Informa.Web.Helpers
{
    public static class MvcHelpers
    {
        public static string GetRazorViewAsString(object model, string filePath)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(HttpContext.Current);
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new ParseController());
            var razor = new RazorView(controllerContext, filePath, null, false, null);
            razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), new TempDataDictionary(), st), st);
            return st.ToString();
        }

        /// <summary>
        /// Get Razor View As String Overloaded method
        /// </summary>
        /// <param name="model"></param>
        /// <param name="filePath"></param>
        /// <param name="tempData"></param>
        /// <returns></returns>
        public static string GetRazorViewAsString(object model, string filePath, TempDataDictionary tempData)
        {
            var st = new StringWriter();
            var context = new HttpContextWrapper(HttpContext.Current);
            var routeData = new RouteData();
            var controllerContext = new ControllerContext(new RequestContext(context, routeData), new ParseController());
            var razor = new RazorView(controllerContext, filePath, null, false, null);
            razor.Render(new ViewContext(controllerContext, razor, new ViewDataDictionary(model), tempData, st), st);
            return st.ToString();
        }
    }
    public class ParseController : Controller
    {
    }
}