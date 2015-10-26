//using System.Web.Mvc;
//using Sitecore.Configuration;

//namespace Informa.Web.Areas.Admin
//{
//    public class AdminAreaRegistration : AreaRegistration 
//    {
//        public override string AreaName 
//        {
//            get 
//            {
//                return "Admin";
//            }
//        }

//        public override void RegisterArea(AreaRegistrationContext context)
//        {
//	        context.MapRoute(
//				"Admin_default",
//				"Admin/{controller}/{action}/{id}",
//				new {
//						action = "Index",
//						id = UrlParameter.Optional,
//						namespaces = new[] { "Informa.Web.Areas.Admin.*" }
//				}
//			);
//		}
//	}
//}