using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebRegisterUserSession : IWebRegisterUserSession, IWebSetRegisterUserSession
	{
		public WebRegisterUserSession(
			)
		{

		}

		public INewUser NewUser
		{
			get
			{
				var sessionObject = HttpContext.Current.Session["WebRegisterUserSession"];

				return sessionObject is INewUser ? (INewUser)sessionObject : null;
			}
			set
			{
				HttpContext.Current.Session["WebRegisterUserSession"] = value;
			}
		}
	}
}
