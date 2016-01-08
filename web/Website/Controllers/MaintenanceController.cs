using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Models;
using System;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
	public class MaintenanceController : Controller
	{
		protected readonly ISitecoreService SitecoreService;
		protected readonly ISitecoreContext SitecoreContext;
		protected readonly ITextTranslator TextTranslator;

		public MaintenanceController(
			ISitecoreService sitecoreService,
			ISitecoreContext sitecoreContext,
			ITextTranslator textTranslator)
		{
			SitecoreService = sitecoreService;
			SitecoreContext = sitecoreContext;
			TextTranslator = textTranslator;
		}

		public ViewResult MaintenanceMessage()
		{
			var home = SitecoreContext.GetHomeItem<IGlassBase>();
			ISite_Root siteRoot = null;

			var model = new MaintenanceMessageViewModel
			{
				DismissText = TextTranslator.Translate("Dismiss"),
				DisplayFrom = siteRoot == null ? DateTime.Now : siteRoot.System_Maintenance_Start_Date,
				DisplayTo = siteRoot == null ? DateTime.Now.AddDays(1) : siteRoot.System_Maintenance_End_Date,
				Message = siteRoot == null ? string.Empty : siteRoot.System_Maintenance_Text
			};

			return View(model);
		}
	}
}