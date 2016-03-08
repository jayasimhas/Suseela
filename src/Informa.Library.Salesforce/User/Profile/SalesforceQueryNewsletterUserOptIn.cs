using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.User;
using Informa.Library.User.Profile;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.User.Authentication;

namespace Informa.Library.Salesforce.User.Profile
{
    public class SalesforceQueryNewsletterUserOptIn : IQueryNewsletterUserOptIn
    {
        protected readonly ISalesforceServiceContext Service;

        public SalesforceQueryNewsletterUserOptIn(
            ISalesforceServiceContext service)
        {
            Service = service;
        }

        public IQueryNewsletterUserOptInResult Query(IAuthenticatedUser user)
        {
            if (string.IsNullOrEmpty(user?.Email))
            {
                return ErrorResult;
            }
            
            var response = Service.Execute(s => s.queryEmailNewsletterOptins(user.Email));

            if (!response.IsSuccess())
            {
                return ErrorResult;
            }

            return new SalesforceQueryNewsletterUserOptInResult
            {
                Success = true,
                NewsletterOptIns = (response.emailNewsletterOptins != null && response.emailNewsletterOptins.Any()) 
                    ? response.emailNewsletterOptins.Select(a => new NewsletterOptIn()
                    {
                        Frequency = a.frequency,
                        Name = a.optinName,
                        ReceivesEmailAlert = (a.IsReceivingEmailAlertSpecified && a.IsReceivingEmailAlert.Value),
                        ReceivesNewsletterAlert = (a.IsReceivingEmailNewsletterSpecified && a.IsReceivingEmailNewsletter.Value)
                    })
                    : Enumerable.Empty<INewsletterOptIn>()
            };
        }

        public IQueryNewsletterUserOptInResult ErrorResult => new SalesforceQueryNewsletterUserOptInResult
        {
            Success = false,
            NewsletterOptIns = Enumerable.Empty<INewsletterOptIn>()
        };
    }
}
