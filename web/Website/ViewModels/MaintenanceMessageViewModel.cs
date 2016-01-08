using System;

namespace Informa.Web.ViewModels
{
	public class MaintenanceMessageViewModel : IMaintenanceMessageViewModel
	{
		public string Message { get; set; }
		public string DismissText { get; set; }
		public DateTime DisplayFrom { get; set; }
		public DateTime DisplayTo { get; set; }
	}
}