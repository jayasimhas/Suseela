using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Web.ViewModels;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
	public class MaintenanceController : Controller
	{
		protected readonly ISiteMaintenanceContext SiteMaintenanceContext;
		protected readonly ITextTranslator TextTranslator;

		public MaintenanceController(
			ISiteMaintenanceContext siteMaintenanceContext,
			ITextTranslator textTranslator)
		{
			SiteMaintenanceContext = siteMaintenanceContext;
			TextTranslator = textTranslator;
		}

		public ViewResult MaintenanceMessage()
		{
			var siteMaintenanceInfo = SiteMaintenanceContext.Info;

			var model = new MaintenanceViewModel
			{
				DismissText = TextTranslator.Translate("MaintenanceDismiss"),
				DisplayFrom = siteMaintenanceInfo.From,
				DisplayTo = siteMaintenanceInfo.To,
				Message = siteMaintenanceInfo.Message
			};

			return View(model);
		}
	}
}