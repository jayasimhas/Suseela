using Informa.Library.Actions;
using System.Collections.Generic;

namespace Informa.Library.User.Authentication.Web
{
	public class WebLoginUserActions : ActionsProcessor<IWebLoginUserAction, IAuthenticatedUser>, IWebLoginUserActions
	{
		public WebLoginUserActions(
			IEnumerable<IWebLoginUserAction> actions)
			: base(actions)
		{

		}
	}
}
