using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserIpAddressViewModel : IUserIpAddressViewModel
	{
		public string IpAddressLabelText => "Spoof Email";
		public string IpAddressSubmitText => "OK";
	}
}