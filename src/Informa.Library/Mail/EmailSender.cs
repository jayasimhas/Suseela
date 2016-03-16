using System;
using System.Linq;
using System.Net;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net.Mail;
using Sitecore.Configuration;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailSender : IEmailSender
	{
		public bool Send(IEmail email)
        {
            var recipients = GetRecipients(email);
            using (var sitecoreEmail = new MailMessage(email.From, recipients.First(), email.Subject, email.Body))
		    {
                foreach (var recipient in recipients.Skip(1))
                {
                    sitecoreEmail.To.Add(recipient);
                }

                sitecoreEmail.IsBodyHtml = email.IsBodyHtml;

		        try
		        {
		            using (var client = CreateSmtpClient())
		            {
		                client.Send(sitecoreEmail);
		            }
		        }
		        catch
		        {
		            return false;
		        }

		        return true;
		    }
		}

		public bool SendWorkflowNotification(IEmail email, string replyEmail)
		{
		    var recipients = GetRecipients(email);
            using (var sitecoreEmail = new MailMessage(email.From, recipients.First(), email.Subject, email.Body))
            {
                foreach (var recipient in recipients.Skip(1))
                {
                    sitecoreEmail.To.Add(recipient);
                }

                sitecoreEmail.IsBodyHtml = email.IsBodyHtml;
                sitecoreEmail.ReplyToList.Add(replyEmail);

                try
                {
                    using (var client = CreateSmtpClient())
                    {
                        client.Send(sitecoreEmail);
                    }
                }
                catch
                {
                    return false;
                }

                return true;
            }
		}

	    private string[] GetRecipients(IEmail email)
	    {
	        return email.To.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries)
                .ToArray();
	    }

	    // This is taken from Sitecore.MainUtil; MainUtil.SendMail has a resource leak
        private static SmtpClient CreateSmtpClient()
        {
            string mailServer = Settings.MailServer;
            SmtpClient smtpClient;
            if (string.IsNullOrEmpty(mailServer))
            {
                smtpClient = new SmtpClient();
            }
            else
            {
                int mailServerPort = Settings.MailServerPort;
                smtpClient = mailServerPort <= 0 ? new SmtpClient(mailServer) : new SmtpClient(mailServer, mailServerPort);
            }
            string mailServerUserName = Settings.MailServerUserName;
            if (mailServerUserName.Length > 0)
            {
                string mailServerPassword = Settings.MailServerPassword;
                NetworkCredential networkCredential = new NetworkCredential(mailServerUserName, mailServerPassword);
                smtpClient.Credentials = (ICredentialsByHost)networkCredential;
            }
            return smtpClient;
        }
    }
}
