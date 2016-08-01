using Informa.Library.Utilities.WebApi.Filters;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new CSPFilterAttribute());
        }
    }
}
