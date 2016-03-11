using Informa.Library.Mail;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Registration.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebSendRegisterUserEmail : IWebSendRegisterUserEmail
	{
		protected readonly IWebRegisterUserEmailFactory EmailFactory;
		protected readonly IEmailSender EmailSender;

		public WebSendRegisterUserEmail(
			IWebRegisterUserEmailFactory emailFactory,
			IEmailSender emailSender)
		{
			EmailFactory = emailFactory;
			EmailSender = emailSender;
		}

		public void Process(INewUser newUser)
		{
			var email = EmailFactory.Create(newUser);

			if (email != null)
			{
				EmailSender.Send(email);
			}
		}
	}
}
