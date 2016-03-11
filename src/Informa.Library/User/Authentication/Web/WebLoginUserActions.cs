using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLoginUserActions : IWebLoginUserActions
	{
		protected readonly IEnumerable<IWebLoginUserAction> Actions;

		public WebLoginUserActions(
			IEnumerable<IWebLoginUserAction> actions)
		{
			Actions = actions;
		}

		public IEnumerator<IWebLoginUserAction> GetEnumerator()
		{
			return Actions == null ? Enumerable.Empty<IWebLoginUserAction>().GetEnumerator() : Actions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
