using Informa.Library.Mail;
using Informa.Library.User.Profile;
using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.User.ResetPassword.Web
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class WebUserResetPasswordEmailFactory : IWebUserResetPasswordEmailFactory
	{
		protected readonly IFindUserProfileByUsername FindUserProfile;
		protected readonly IWebUserResetPasswordUrlFactory UrlFactory;
		protected readonly IEmailFactory EmailFactory;

		public WebUserResetPasswordEmailFactory(
			IFindUserProfileByUsername findUserProfile,
			IWebUserResetPasswordUrlFactory urlFactory,
			IEmailFactory emailFactory)
		{
			FindUserProfile = findUserProfile;
			UrlFactory = urlFactory;
			EmailFactory = emailFactory;
		}

		public IEmail Create(IUserResetPassword userResetPassword)
		{
			var email = EmailFactory.Create();
			// Get HTML from file (~/email/PasswordReset.html) [Configuration]
			var html = "<html><head></head><body><ul><li>Date: #Date#</li><li>First Name: #First_Name#</li><li>Last Name: #Last_Name#</li><li>Reset URL: <a href=\"#Reset_URL#\">Reset Link</a></li></ul>#Email_Content#</body></html>";
			var content = "<p>Sitecore Email Content</p>"; // Get copy from Sitecore
			var subject = "Reset Password";
			var url = UrlFactory.Create(userResetPassword);
			var from = "noreply@informa.com";
			var userProfile = FindUserProfile.Find(userResetPassword.Username);

			if (userProfile != null)
			{
				html = html.Replace("#First_Name#", userProfile.FirstName).Replace("#Last_Name#", userProfile.LastName);
			}

			html = html
				.Replace("#Date#", DateTime.Now.ToString("dddd, d MMMM yyyy"))
				.Replace("#Reset_URL#", url)
				.Replace("#Email_Content#", content);

			email.To = userResetPassword.Username;
			email.From = from;
			email.Body = html;
			email.IsBodyHtml = true;
			email.Subject = subject;

			return email;
		}
	}
}
