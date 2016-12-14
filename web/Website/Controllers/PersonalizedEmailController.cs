using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Net;
using System.Text;
using Informa.Library.Site;
using Informa.Library.Mail;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Library.Services.Global;
using Sitecore.Configuration;
using System.Collections.Generic;
using Informa.Library.Utilities.Extensions;
using System;

namespace Informa.Web.Controllers
{
    [Route]
    public class PersonalizedEmailController : ApiController
    {
        private readonly EmailUtil _emailUtil;
        
        public PersonalizedEmailController(EmailUtil emailUtil)
        {
           
            _emailUtil = emailUtil;
        }

        [HttpGet]
        public HttpResponseMessage Get(string userId)
        {

            var isMailSent = false;

            var response = new HttpResponseMessage();
            response.StatusCode = string.IsNullOrWhiteSpace(userId) ? HttpStatusCode.BadRequest : HttpStatusCode.OK;
            if (!string.IsNullOrWhiteSpace(userId))
            {
             
                var emailBody = GetEmailBody(userId);
                var personalizedEmail = new Email
                {
                    To = userId,
                    Subject = "Personalized Test",
                    From = "pmbi-notifications@informa.com",
                    Body = emailBody,
                    IsBodyHtml = true
                };

                EmailSender EmailSender = new EmailSender();

                var isEmailSent = EmailSender.Send(personalizedEmail);
                 isMailSent = isEmailSent;
                if (!isEmailSent)
                {
                   // _logger.Warn($"Email sender failed");
                   // result = false;
                }


            }

            return response;
        }




        public string GetEmailBody(string senderEmail)
        {
            string emailHtml = string.Empty;
            try
            {
                   
 
                 IHtmlEmailTemplateFactory HtmlEmailTemplateFactory = new HtmlEmailTemplateFactory();
                 var htmlEmailTemplate = HtmlEmailTemplateFactory.Create("Emailtemplatepersonalization");


                if (htmlEmailTemplate == null)
                {
                    return null;
                }

                emailHtml = htmlEmailTemplate.Html;
                var replacements = new Dictionary<string, string>
                {
                    ["#FirstName#"] = senderEmail,
                    ["#Personalized_Content#"] = senderEmail,
                    ["#email#"] = senderEmail,
                };

                emailHtml = emailHtml.ReplacePatternCaseInsensitive(replacements);
            }
            catch (Exception ex)
            {
                
            }
            return emailHtml;
        }


        public string GetValue(string value, string defaultValue = null)
        {
            return value ?? defaultValue ?? string.Empty;
        }

    }
}
