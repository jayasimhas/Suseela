using Informa.Library.Net;
using Informa.Library.User;
using Informa.Library.User.SiteDebugging;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Web.ViewModels.SiteDebugging
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class UserIpAddressViewModel : IUserIpAddressViewModel
	{
		protected readonly IIpAddressFactory IpAddressFactory;
		protected readonly IUserIpAddressContext UserIpAddressContext;
		protected readonly ISiteDebuggingUserIpAddressContext DebugUserIpAddressContext;

		public UserIpAddressViewModel(
			IIpAddressFactory ipAddressFactory,
			IUserIpAddressContext userIpAddressContext,
			ISiteDebuggingUserIpAddressContext debugUserIpAddressContext)
		{
			IpAddressFactory = ipAddressFactory;
			UserIpAddressContext = userIpAddressContext;
			DebugUserIpAddressContext = debugUserIpAddressContext;
		}

		public string IpAddressLabelText => "Spoof IP";
		public string IpAddressInputName => "debugIpAddress";
		public string ClearIpAddressInputName => "debugClearIpAddress";
		public string IpAddressSubmitText => IsDebugging ? "Clear" : "OK";
		public bool IsDebugging
		{
			get
			{
				if (IsCleared)
				{
					DebugUserIpAddressContext.StopDebugging();

					return false;
				}

				if (DebugUserIpAddressContext.IsDebugging)
				{
					return true;
				}

				var ipAddress = IpAddressFactory.Create(HttpContext.Current.Request[IpAddressInputName]);

				if (ipAddress == null)
				{
					return false;
				}

				DebugUserIpAddressContext.StartDebugging(ipAddress);

				return true;
			}
		}
		public bool IsCleared => !string.IsNullOrWhiteSpace(HttpContext.Current.Request[ClearIpAddressInputName]);
		public string IpAddress
		{
			get
			{
				if (IsDebugging)
				{
					return UserIpAddressContext.IpAddress.ToString();
				}

				return string.Empty;
			}
		}
	}
}