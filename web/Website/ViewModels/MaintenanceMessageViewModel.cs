using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System;

namespace Informa.Web.ViewModels
{
	public class MaintenanceMessageViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly ISiteMaintenanceContext SiteMaintenanceContext;

		public MaintenanceMessageViewModel(
			ISiteMaintenanceContext siteMaintenanceContext)
		{
			SiteMaintenanceContext = siteMaintenanceContext;
		}

		public string Message => "Message";
		public string DismissText => "Dismiss Text";
		public DateTime DisplayFrom => DateTime.Now;
		public DateTime DisplayTo => DateTime.Now.AddDays(1);
	}
}