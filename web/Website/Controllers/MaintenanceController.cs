using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Models;
using Sitecore.Globalization;
using System;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
	public class MaintenanceController : Controller
	{
		protected readonly ISitecoreService SitecoreService;
		protected readonly ISitecoreContext SitecoreContext;

		public MaintenanceController(ISitecoreService sitecoreService, ISitecoreContext sitecoreContext)
		{
			SitecoreService = sitecoreService;
			SitecoreContext = sitecoreContext;
		}

		public ViewResult MaintenanceMessage()
		{
			var home = SitecoreContext.GetHomeItem<IGlassBase>();
			ISite_Root siteRoot = null;

			var model = new MaintenanceMessageViewModel
			{
				DismissText = Translate.Text("Dismiss"),
				DisplayFrom = siteRoot == null ? DateTime.Now : siteRoot.System_Maintenance_Start_Date,
				DisplayTo = siteRoot == null ? DateTime.Now.AddDays(1) : siteRoot.System_Maintenance_End_Date,
				Message = siteRoot == null ? "Dismiss" : siteRoot.System_Maintenance_Text
			};

			return View(model);
		}
	}
}