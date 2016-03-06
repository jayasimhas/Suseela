using Jabberwocky.Glass.Autofac.Attributes;
using Sitecore;
using System.Net.Mail;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailSender : IEmailSender
	{
		public bool Send(IEmail email)
		{
			var sitecoreEmail = new MailMessage(email.From, email.To, email.Subject, email.Body);

			sitecoreEmail.IsBodyHtml = email.IsBodyHtml;

			try
			{
				MainUtil.SendMail(sitecoreEmail);
			}
			catch
			{
				return false;
			}

			return true;
		}

		public bool SendWorkflowNotification(IEmail email, string replyEmail)
		{
			var sitecoreEmail = new MailMessage(email.From, email.To, email.Subject, email.Body) { IsBodyHtml = email.IsBodyHtml };		
			sitecoreEmail.ReplyToList.Add(replyEmail);
			try
			{
				MainUtil.SendMail(sitecoreEmail);
			}
			catch
			{
				return false;
			}

			return true;
		}
	}
}
