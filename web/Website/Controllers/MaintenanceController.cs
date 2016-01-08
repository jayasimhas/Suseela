using Informa.Library.Globalization;
using Informa.Library.SIte;
using Informa.Web.ViewModels;
using System;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
	public class MaintenanceController : Controller
	{
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly ITextTranslator TextTranslator;

		public MaintenanceController(
			ISiteRootContext siteRootContext,
			ITextTranslator textTranslator)
		{
			SiteRootContext = siteRootContext;
			TextTranslator = textTranslator;
		}

		public ViewResult MaintenanceMessage()
		{
			var siteRoot = SiteRootContext.Item;

			var model = new MaintenanceMessageViewModel
			{
				DismissText = TextTranslator.Translate("MaintenanceDismiss"),
				DisplayFrom = siteRoot == null ? default(DateTime) : siteRoot.System_Maintenance_Start_Date,
				DisplayTo = siteRoot == null ? default(DateTime) : siteRoot.System_Maintenance_End_Date,
				Message = siteRoot == null ? TextTranslator.Translate("MaintenanceMessage") : siteRoot.System_Maintenance_Text
			};

			return View(model);
		}
	}
}