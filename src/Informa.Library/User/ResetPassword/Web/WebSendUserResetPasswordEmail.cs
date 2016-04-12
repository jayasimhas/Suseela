using Informa.Library.Mail;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSendUserResetPasswordEmail : IWebSendUserResetPasswordEmail
	{
		protected readonly IWebUserResetPasswordEmailFactory EmailFactory;
		protected readonly IEmailSender EmailSender;

		public WebSendUserResetPasswordEmail(
			IWebUserResetPasswordEmailFactory emailFactory,
			IEmailSender emailSender)
		{
			EmailFactory = emailFactory;
			EmailSender = emailSender;
		}

		public void Process(IUserResetPassword userResetPassword)
		{
			var email = EmailFactory.Create(userResetPassword);

			if (email != null)
			{
				EmailSender.Send(email);
			}
		}
	}
}
