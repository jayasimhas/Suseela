using Informa.Library.Mail;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebUserResetPasswordEmailFactory : IWebUserResetPasswordEmailFactory
	{
		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IEmailFactory EmailFactory;

		public WebUserResetPasswordEmailFactory(
			IFindUserProfileByUsername findUserProfile,
			IEmailFactory emailFactory)
		{
			FindUserProfile = findUserProfile;
			EmailFactory = emailFactory;
		}

		public IEmail Create(IUserResetPassword userResetPassword)
		{
			var email = EmailFactory.Create();
			var userProfile = FindUserProfile.Find(userResetPassword.Username);

			// Get copy from Sitecore
			// Get template from HTML file (~/email/PasswordReset.html) [Configuration]
			// Create body from copy, HTML & user profile

			return email;
		}
	}
}
