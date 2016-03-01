using Informa.Library.User.ResetPassword;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.ResetPassword
{
	public class ChangeViewModel : GlassViewModel<I___BasePage>
	{
		protected readonly IFindUserResetPassword FindUserResetPassword;

		public ChangeViewModel(
			IFindUserResetPassword findUserResetPassword)
		{
			FindUserResetPassword = findUserResetPassword;
		}

		protected IUserResetPassword UserResetPassword
		{
			get
			{
				return FindUserResetPassword.Find(Token);
			}
		}

		public string Title => "";
		public IHtmlString ResetBody => new MvcHtmlString("");
		public string NewPasswordLabelText => "";
		public string NewPasswordPlaceholderText => "";
		public string NewPasswordRepeatLabelText => "";
		public string NewPasswordRepeatPlaceholderText => "";
		public string SubmitButtonText => "";
		public IHtmlString RetryBody => new MvcHtmlString("");
		public bool IsValidToken => UserResetPassword.IsValid();
		public bool TokenFound => UserResetPassword != null;
		public string Token => HttpContext.Current.Request["rpToken"];
	}
}