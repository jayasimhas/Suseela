using Informa.Web.ViewModels;
using System;
using System.Web.Mvc;

namespace Informa.Web.Controllers
{
	public class MaintenanceController : Controller
	{
		public MaintenanceController()
		{

		}

		public ViewResult MaintenanceMessage()
		{
			var model = new MaintenanceMessageViewModel
			{
				DismissText = "Dismiss",
				DisplayFrom = DateTime.Now,
				DisplayTo = DateTime.Now.AddDays(1),
				Message = "Maintenance message"
			};

			return View(model);
		}
	}
}