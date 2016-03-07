using Informa.Library.Mail;
using Informa.Models.DCD;
using log4net;
using System;
using System.Text;

namespace Informa.Library.DCD.XMLImporting
{
    public static class DCDImportLogger
    {
        static ILog _logger;
        static DCDConfigurationItem dcdConfig;
        static DCDImportLogger()
        {
            try
            {
                _logger = LogManager.GetLogger("DCDImportAppender");// Sitecore.Diagnostics.LoggerFactory.GetLogger("DCDImportAppender");
                dcdConfig = Sitecore.Data.Database.GetDatabase("web").GetItem(DCDConstants.DCDConfigurationItemID);
            }
            catch (Exception ex) { }
        }

        public static void Debug(string message)
        {
            _logger.Debug(message);
        }

        public static void Info(string message)
        {
            _logger.Info(message);
        }

        public static void Warn(string message)
        {
            _logger.Warn(message);
        }

        public static void Error(string message)
        {
            _logger.Error(message);
        }

        public static void Error(string message, Exception ex)
        {
            _logger.Error(message, ex);
        }

        public static void ThrowCriticalImportError(string error, string processingFileName, ImportLog importLog, StringBuilder importLogNotes)
        {
            ThrowCriticalImportError(error, null, processingFileName, importLog, importLogNotes);
        }

        public static void ThrowCriticalImportError(string error, Exception e, string processingFileName, ImportLog importLog, StringBuilder importLogNotes)
        {
            try
            {
                //Log the error in the database and the log
                importLogNotes.AppendLine(error);
                importLog.ImportEnd = DateTime.Now;
                importLog.Result = ImportResult.Failure.ToString();
                importLog.Notes = importLogNotes.ToString();

                using (DCDContext dc = new DCDContext())
                    dc.UpdateImportLogEntry(importLog);
            }
            catch (Exception ex)
            {
                DCDImportLogger.Error("Error writing log message to database", ex);
            }

            if (e != null)
            {
                DCDImportLogger.Error(error, e);
            }
            else
            {
                DCDImportLogger.Error(error);
            }

            string subject = "Error processing Deals, Companies, Drugs import file. ";
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("There was an error processing the import file: " + processingFileName);
            sb.AppendLine();
            sb.AppendLine("ERROR:");
            sb.AppendLine();
            sb.AppendLine(error);

            if (e != null)
            {
                sb.AppendLine(e.Message);
                if (e.InnerException != null)
                {
                    sb.AppendLine(e.InnerException.Message);
                }
            }

            if (!string.IsNullOrEmpty(importLogNotes.ToString()))
            {
                sb.AppendLine(importLogNotes.ToString());
            }

            SendNotification(subject, sb.ToString());
        }

        public static bool SendNotification(string subject, string body)
        {
            IEmailSender mailer = new EmailSender();
            var message = new Informa.Library.Mail.Email();
            //var from = new System.Net.Mail.MailAddress(DCDConstants.EmailNoReplySenderAddress);

            message.IsBodyHtml = false;

            foreach (var email in dcdConfig.GetEmailDistributionList())
            {
                if (string.IsNullOrEmpty(email)) continue;

                message.To = message.To + email + ",";
            }
            message.To = message.To.TrimEnd(',');

            message.Body = body;
            message.Subject = subject;
            message.From = DCDConstants.EmailNoReplySenderAddress;

            return mailer.Send(message);
        }
    }
}
