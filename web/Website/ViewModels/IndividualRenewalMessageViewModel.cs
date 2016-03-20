using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Salesforce;
using Informa.Library.Salesforce.EBIWebServices;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Informa.Library.User.Authentication.Web;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.SingleInstance)]
    public class IndividualRenewalMessageViewModel : IIndividualRenewalMessageViewModel
    {
        ISalesforceServiceContext _service;
        ITextTranslator _textTranslator;
        IIndividualSubscriptionRenewalMessageContext _context;
        IAuthenticatedUserContext _userContext;
        EBI_SubscriptionAndPurchase _latestSalesForceRecord;
        ISiteRootContext _siteRootContext;
        public IndividualRenewalMessageViewModel(ISalesforceServiceContext service, ITextTranslator textTranslator, IIndividualSubscriptionRenewalMessageContext context, IAuthenticatedUserContext userContext, ISiteRootContext siteRootContext)
        {
            _service = service;
            _context = context;
            _textTranslator = textTranslator;
            _userContext = userContext;
            _siteRootContext = siteRootContext;
            try
            {
                if (_userContext.IsAuthenticated)
                {
                    //Get SubscruiptionsAndPurchases records for the specifed usern
                    EBI_QuerySubscriptionsAndPurchasesResponse response = _service.Execute(x => x.querySubscriptionsAndPurchases(_userContext.User.Username));

                    //Get the latest record
                    _latestSalesForceRecord = response.subscriptionsAndPurchases.OrderByDescending(o => o.expirationDate).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this.GetType());
            }
        }

        public string DismissText
        {
            get
            {
                return _textTranslator.Translate("Subscriptions.Renewals.Dismiss");
            }
        }

        public bool Display
        {
            get
            {
                if (_latestSalesForceRecord == null)
                    return false;

                if (_latestSalesForceRecord.productCode.ToLower() != _siteRootContext.Item.Product_Code.ToLower())
                    return false;

                if ((_latestSalesForceRecord.expirationDate - DateTime.Now)?.TotalDays > _siteRootContext.Item.Days_To_Expiration)
                    return false;

                //string[] subsTypes = _siteRootContext.Item.Subscription_Type.ToLower().Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                //if (subsTypes == null || subsTypes.Contains(_latestSalesForceRecord.subscriptionType.ToLower()) == false)
                //    return false;

                //if (_latestSalesForceRecord.productType != _siteRootContext.Item.Product_Type.ToLower())
                //    return false;

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
                if (_latestSalesForceRecord == null)
                    return string.Empty;

                if (_latestSalesForceRecord.subscriptionType.ToLower() == "free-trial")
                    return _context.Message_FreeTrial.Replace("#FIRST_NAME#", _userContext.User.Name).Replace("#SUB_EXPIRATION#", _latestSalesForceRecord.expirationDate.Value.ToShortDateString());
                else
                    return _context.Message_IndividualSubscriptiong.Replace("#FIRST_NAME#", _userContext.User.Name).Replace("#SUB_EXPIRATION#", _latestSalesForceRecord.expirationDate.Value.ToShortDateString());
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