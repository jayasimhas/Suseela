using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.User.Authentication
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class RequestUserAuthenticationContext : IUserAuthenticationContext
	{
		public bool IsAuthenticated => HttpContext.Current.Request.IsAuthenticated;
	}
}
