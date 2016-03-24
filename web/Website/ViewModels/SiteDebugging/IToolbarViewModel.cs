namespace Informa.Web.ViewModels.SiteDebugging
{
	public interface IToolbarViewModel
	{
		IUserIpAddressViewModel UserIpAddressViewModel { get; }
		IEntitlementsCheckEnabledViewModel EntitlementsCheckEnabledViewModel { get; }
		IUserEntitlementsViewModel UserEntitlementsViewModel { get; }
		bool Enabled { get; }
		string ToggleButtonText { get; }
	}
}