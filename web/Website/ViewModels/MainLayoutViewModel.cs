using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;

namespace Informa.Web.ViewModels
{
	public class MainLayoutViewModel : GlassViewModel<IGlassBase>
	{
		public MainLayoutViewModel(
			IMaintenanceViewModel maintenanceViewModel,
			ISideNavigationMenuViewModel sideNavigationMenuViewModel,
			IFooterViewModel footerViewModel)
		{
			MaintenanceMessage = maintenanceViewModel;
			SideNavigationMenu = sideNavigationMenuViewModel;
			Footer = footerViewModel;
		}

		public IMaintenanceViewModel MaintenanceMessage;

		public ISideNavigationMenuViewModel SideNavigationMenu;

		public IFooterViewModel Footer;

		public string Title => "Site Title";
	}
}