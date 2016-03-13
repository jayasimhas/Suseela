using Informa.Library.Actions;
using System.Collections.Generic;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLogoutUserActions : ActionsProcessor<IWebLogoutUserAction, IAuthenticatedUser>, IWebLogoutUserActions
	{
		public WebLogoutUserActions(
			IEnumerable<IWebLogoutUserAction> actions)
			: base(actions)
		{
			
		}
	}
}
