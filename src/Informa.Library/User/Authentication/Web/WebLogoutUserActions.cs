using Informa.Library.Actions;
using System.Collections.Generic;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLogoutUserActions : ActionsProcessor<IWebLogoutUserAction, IUser>, IWebLogoutUserActions
	{
		public WebLogoutUserActions(
			IEnumerable<IWebLogoutUserAction> actions)
			: base(actions)
		{
			
		}
	}
}
