using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Informa.Library.Subscription
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class IndividualSubscriptionRenewalMessageContext : IIndividualSubscriptionRenewalMessageContext
    {
        ITextTranslator _textTranslator;
        ISiteRootContext _siteRootContext;
        IAuthenticatedUserContext _userContext;
        public IndividualSubscriptionRenewalMessageContext(ITextTranslator textTranslator, ISiteRootContext siteRootContext, IAuthenticatedUserContext userContext)
        {
            _textTranslator = textTranslator;
            _siteRootContext = siteRootContext;
            _userContext = userContext;
        }

        public string ID
        {
            get
            {
                return "renewalMessage_" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }
        }

        public string Message_IndividualSubscriptiong
        {
            get
            {

                return _userContext.User.Name + " " + _siteRootContext.Item.Individual_Subscription_Expiration_Warning_Text;
            }
        }

        public string Message_FreeTrial
        {
            get
            {
                return _siteRootContext.Item.Free_Trial_Expiration_Warning_Text;
            }
        }

        public string RenewalLinkText
        {
            get
            {
                return _siteRootContext.Item.Subscribe_Link.Text;
            }
        }

        public string RenewalLinkURL
        {
            get
            {
                return _siteRootContext.Item.Subscribe_Link.Url;
            }
        }
    }
}
