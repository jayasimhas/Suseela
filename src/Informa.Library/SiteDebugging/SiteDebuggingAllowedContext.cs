using Informa.Library.Net;
using System.Linq;
using Informa.Library.User.Authentication;

namespace Informa.Library.SiteDebugging
{
	public class SiteDebuggingAllowedContext : ISiteDebuggingAllowedContext
	{
		protected readonly IIpAddressContext IpAddressContext;
	    protected readonly IAuthenticatedUserContext UserContext;
		protected readonly ISiteDebuggingAllowedConfiguration Configuration;

		public SiteDebuggingAllowedContext(
			IIpAddressContext ipAddressContext,
            IAuthenticatedUserContext userContext,
            ISiteDebuggingAllowedConfiguration configuration)
		{
			IpAddressContext = ipAddressContext;
            UserContext = userContext;
            Configuration = configuration;
		}

		public bool IsAllowed => IsAllowedByIpAddress || IsAllowedByEmailAddress;

		public bool IsAllowedByIpAddress => 
			IpAddressContext.IpAddress != null &&
			Configuration.IpAddresses.Any(ia => ia.Equals(IpAddressContext.IpAddress));

		public bool IsAllowedByEmailAddress => UserContext.IsAuthenticated && Configuration.EmailAddresses.Any(ia => ia.Equals(UserContext.User?.Email ?? string.Empty));
	}
}
