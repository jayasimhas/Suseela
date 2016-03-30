using System;
using System.Linq;
using System.Net;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Net.Mail;
using log4net;
using Sitecore.Configuration;

namespace Informa.Library.Mail
{
	[AutowireService(LifetimeScope.SingleInstance)]
	public class EmailSender : IEmailSender
	{
	    private ILog _logger;
	    public EmailSender()
	    {
            _logger = LogManager.GetLogger("LogFileAppender");
        }
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
		        catch(Exception e)
		        {
		            _logger.Error("SmtpClient not created:", e);

                    _logger.Warn($"Mail server: {Settings.MailServer}");
                    _logger.Warn($"Mail server port: {Settings.MailServerPort}");
                    _logger.Warn($"Mail server port: {Settings.MailServerPassword}");
                    _logger.Warn($"Mail server port: {Settings.MailServerUserName}");

                    var emailClient = CreateSmtpClient();

                    _logger.Warn($"Client {emailClient.EnableSsl}");
                    var port = Settings.MailServerPort <= 0 ? "NoPort:" : "hasPort";
                    _logger.Warn($"Client, {emailClient.Port},{port}");

                    bool enableSsl;
                    emailClient.EnableSsl =
                        bool.TryParse(Sitecore.Configuration.Settings.GetSetting("Mail.MailServerEnableSsl"), out enableSsl) &&
                        enableSsl;

                    _logger.Warn($"EnableSSL: {enableSsl}");


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
                catch (Exception e)
                {   
                    _logger.Error("SmtpClient not created:", e);
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
            bool enableSsl;
            smtpClient.EnableSsl =
                bool.TryParse(Sitecore.Configuration.Settings.GetSetting("Mail.MailServerEnableSsl"), out enableSsl) &&
                enableSsl;

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
