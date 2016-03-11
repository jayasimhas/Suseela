using Informa.Library.Actions;
using System.Collections.Generic;

namespace Informa.Library.User.ResetPassword.Web
{
	public class WebGenerateUserResetPasswordActions : ActionsProcessor<IWebGenerateUserResetPasswordAction, IUserResetPassword>, IWebGenerateUserResetPasswordActions
	{
		public WebGenerateUserResetPasswordActions(
			IEnumerable<IWebGenerateUserResetPasswordAction> actions)
			: base(actions)
		{
			
		}
	}
}
