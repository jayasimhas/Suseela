using System.Web.Mvc;
using System.Web.Http;


namespace Informa.Web.Areas.Download
{
    public class DownloadAreaRegistration : AreaRegistration
    {
        public override string AreaName => "Download";
        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "Download_api",
                "Download/api/{controller}/{action}/{id}",
                new { id = RouteParameter.Optional }
            );

            context.MapRoute(
                "Download_default",
                "Download/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}