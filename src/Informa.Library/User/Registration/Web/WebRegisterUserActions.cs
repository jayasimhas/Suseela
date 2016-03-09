using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.Registration.Web
{
	public class WebRegisterUserActions : IWebRegisterUserActions
	{
		protected readonly IEnumerable<IWebRegisterUserAction> Actions;

		public WebRegisterUserActions(
			IEnumerable<IWebRegisterUserAction> actions)
		{
			Actions = actions;
		}

		public IEnumerator<IWebRegisterUserAction> GetEnumerator()
		{
			return Actions == null ? Enumerable.Empty<IWebRegisterUserAction>().GetEnumerator() : Actions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
