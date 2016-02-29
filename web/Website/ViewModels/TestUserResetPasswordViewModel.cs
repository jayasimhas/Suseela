using Informa.Library.User.ResetPassword.Web;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Models;
using System.Web;

namespace Informa.Web.ViewModels
{
	public class TestUserResetPasswordViewModel : GlassViewModel<IGlassBase>
	{
		protected readonly IWebGenerateUserResetPassword GenerateUserResetPassword;

		public TestUserResetPasswordViewModel(
			IWebGenerateUserResetPassword generateUserResetPassword)
		{
			GenerateUserResetPassword = generateUserResetPassword;
		}

		public string Email => HttpContext.Current.Request["urpEmail"];

		public IWebGenerateUserResetPasswordResult Result
		{
			get
			{
				var email = Email;

				if (string.IsNullOrWhiteSpace(email))
				{
					return null;
				}

				return GenerateUserResetPassword.Generate(email);
			}
		}
	}
}