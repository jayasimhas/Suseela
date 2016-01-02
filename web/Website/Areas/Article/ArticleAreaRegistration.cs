using System.Web.Mvc;

namespace Informa.Web.Areas.Article
{
    public class ArticleAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Article";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Article",
                "Article/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional, namespaces = new[] {"Informa.Web.Areas.Article.*"} }
            );
        }
    }
}