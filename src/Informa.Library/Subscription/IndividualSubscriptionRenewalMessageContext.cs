using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System;

namespace Informa.Library.Subscription
{
    [AutowireService(LifetimeScope.PerScope)]
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
                return "renewalMessage_" + DateTime.Now.ToString("yyyyMMdd") + "_" + _userContext?.User?.Username;
            }
        }

        public string Message_IndividualSubscriptiong
        {
            get
            {

                return _siteRootContext.Item.Individual_Subscription_Expiration_Warning_Text;
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
                return _textTranslator.Translate("Subscriptions.Renewals.RenewalMessageSubscriptionLinkText");
            }
        }

		public string RenewalLinkURL => _siteRootContext.Item?.Subscribe_Link?.Url;

	}
}
