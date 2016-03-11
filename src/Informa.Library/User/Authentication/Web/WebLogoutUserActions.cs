using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLogoutUserActions : IWebLogoutUserActions
	{
		protected readonly IEnumerable<IWebLogoutUserAction> Actions;

		public WebLogoutUserActions(
			IEnumerable<IWebLogoutUserAction> actions)
		{
			Actions = actions;
		}

		public IEnumerator<IWebLogoutUserAction> GetEnumerator()
		{
			return Actions == null ? Enumerable.Empty<IWebLogoutUserAction>().GetEnumerator() : Actions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
