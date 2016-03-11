using Informa.Library.Actions;
using System.Collections.Generic;

namespace Informa.Library.User.Registration.Web
{
	public class WebRegisterUserActions : ActionsProcessor<IWebRegisterUserAction, INewUser>, IWebRegisterUserActions
	{
		public WebRegisterUserActions(
			IEnumerable<IWebRegisterUserAction> actions)
			: base(actions)
		{
			
		}
	}
}
