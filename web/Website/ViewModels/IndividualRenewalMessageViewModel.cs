using Informa.Library.Globalization;
using Informa.Library.Salesforce;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Linq;
using Informa.Library.User.Profile;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class IndividualRenewalMessageViewModel : IIndividualRenewalMessageViewModel
    {
        private const string PRODUCT_CODE = "scrip";
        private const string PRODUCT_TYPE = "publication";
        private readonly string[] SUBSCRIPTIONTYPE = new string[] { "individual", "free-trial", "individual internal" };

        protected readonly ISalesforceServiceContext _service;
        protected readonly ITextTranslator _textTranslator;
        protected readonly IIndividualSubscriptionRenewalMessageContext _context;
        protected readonly IAuthenticatedUserContext _userContext;
        protected readonly ISiteRootContext _siteRootContext;
        protected readonly IManageSubscriptions _manageSubscriptions;

        public IndividualRenewalMessageViewModel(
            ISalesforceServiceContext service,
            ITextTranslator textTranslator,
            IIndividualSubscriptionRenewalMessageContext context,
            IAuthenticatedUserContext userContext,
            ISiteRootContext siteRootContext,
            IManageSubscriptions manageSubscriptions)
        {
            _service = service;
            _context = context;
            _textTranslator = textTranslator;
            _userContext = userContext;
            _siteRootContext = siteRootContext;
            _manageSubscriptions = manageSubscriptions;
        }

        public string DismissText
        {
            get
            {
                return _textTranslator.Translate("Subscriptions.Renewals.Dismiss");
            }
        }

        private ISubscription GetLatestRecord()
        {
            ISubscriptionsReadResult results = _manageSubscriptions.QueryItems(_userContext.User);
            return results?.Subscriptions?.OrderByDescending(o => o.ExpirationDate).FirstOrDefault() ?? null;
        }

        public bool Display
        {
            get
            {
                if (_userContext == null || !_userContext.IsAuthenticated)
                    return false;

                var latestRecord = GetLatestRecord();

                if (latestRecord == null
                    || latestRecord.ProductCode.ToLower() != PRODUCT_CODE
                    || (latestRecord.ExpirationDate - DateTime.Now).TotalDays > _siteRootContext.Item.Days_To_Expiration
                    || SUBSCRIPTIONTYPE.Contains(latestRecord.SubscriptionType.ToLower()) == false
                    || latestRecord.ProductType.ToLower() != PRODUCT_TYPE)
                    return false;

                return true;
            }
        }

        public string Id
        {
            get
            {
                return _context.ID;
            }
        }

        public string Message
        {
            get
            {
                var latestRecord = GetLatestRecord();
                if (latestRecord == null)
                    return _context.Message_FreeTrial
                        .Replace("#FIRST_NAME#", _userContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", string.Empty);
                else if (latestRecord?.SubscriptionType.ToLower() == "free-trial")
                    return _context.Message_FreeTrial
                        .Replace("#FIRST_NAME#", _userContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", latestRecord.ExpirationDate.ToString("dd MMM yyyy"));
                else
                    return _context.Message_IndividualSubscriptiong
                        .Replace("#FIRST_NAME#", _userContext.User.Name)
                        .Replace("#SUB_EXPIRATION#", latestRecord.ExpirationDate.ToString("dd MMM yyyy"));
            }
        }

        public string RenewURL
        {
            get
            {
                return _context.RenewalLinkURL;
            }
        }

        public string RenewURLText
        {
            get
            {
                return _context.RenewalLinkText;
            }
        }
    }
}