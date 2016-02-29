using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.User.ResetPassword.Web
{
	public class WebGenerateUserResetPasswordActions : IWebGenerateUserResetPasswordActions
	{
		protected readonly IEnumerable<IWebGenerateUserResetPasswordAction> Actions;

		public WebGenerateUserResetPasswordActions(
			IEnumerable<IWebGenerateUserResetPasswordAction> actions)
		{
			Actions = actions;
		}

		public IEnumerator<IWebGenerateUserResetPasswordAction> GetEnumerator()
		{
			return Actions == null ? Enumerable.Empty<IWebGenerateUserResetPasswordAction>().GetEnumerator() : Actions.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}
