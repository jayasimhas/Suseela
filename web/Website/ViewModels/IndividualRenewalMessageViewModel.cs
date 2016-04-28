using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Subscription;
using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Linq;
using Informa.Library.User.Profile;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
	public class IndividualRenewalMessageViewModel : IIndividualRenewalMessageViewModel
	{
		private const string PRODUCT_CODE = "scrip";
		private const string PRODUCT_TYPE = "publication";
		private readonly string[] SUBSCRIPTIONTYPE = new string[] { "individual", "free-trial", "individual internal" };

		protected readonly IIndividualSubscriptionRenewalMessageContext _context;
		protected readonly ISiteRootContext _siteRootContext;
		protected readonly IManageSubscriptions _manageSubscriptions;

		public IndividualRenewalMessageViewModel(
				ITextTranslator textTranslator,
				IIndividualSubscriptionRenewalMessageContext context,
				IAuthenticatedUserContext userContext,
				ISiteRootContext siteRootContext,
				IManageSubscriptions manageSubscriptions)
		{
			_context = context;
			_siteRootContext = siteRootContext;
			_manageSubscriptions = manageSubscriptions;

			ISubscription record = userContext.IsAuthenticated ? GetLatestRecord(userContext.User) : null;

			DismissText = textTranslator.Translate("Subscriptions.Renewals.Dismiss");
			Display = DisplayMessage(record);
			Message = Display ? GetMessage(record, userContext.User.Name) : string.Empty;
			Id = context.ID;
			RenewURL = context.RenewalLinkURL;
			RenewURLText = context.RenewalLinkText;
		}

		private ISubscription GetLatestRecord(IAuthenticatedUser user)
		{
			ISubscriptionsReadResult results = _manageSubscriptions.QueryItems(user);
			return results?.Subscriptions?.OrderByDescending(o => o.ExpirationDate).FirstOrDefault() ?? null;
		}

		private bool DisplayMessage(ISubscription subscription)
		{
			if (subscription == null
						|| subscription.ProductCode.ToLower() != PRODUCT_CODE
						|| (subscription.ExpirationDate - DateTime.Now).TotalDays > _siteRootContext.Item.Days_To_Expiration
						|| SUBSCRIPTIONTYPE.Contains(subscription.SubscriptionType.ToLower()) == false
						|| subscription.ProductType.ToLower() != PRODUCT_TYPE)
				return false;

			return true;
		}

		private string GetMessage(ISubscription subscription, string userName)
		{
			if (subscription == null)
			{
				return _context.Message_FreeTrial
						.Replace("#FIRST_NAME#", userName)
						.Replace("#SUB_EXPIRATION#", string.Empty);
			}

			if (subscription?.SubscriptionType.ToLower() == "free-trial")
			{
				return _context.Message_FreeTrial
						.Replace("#FIRST_NAME#", userName)
						.Replace("#SUB_EXPIRATION#", subscription.ExpirationDate.ToShortDateString());
			}

			return _context.Message_IndividualSubscriptiong
					.Replace("#FIRST_NAME#", userName)
					.Replace("#SUB_EXPIRATION#", subscription.ExpirationDate.ToShortDateString());
		}

		public string DismissText { get; set; }
		public bool Display { get; set; }
		public string Id { get; set; }
		public string Message { get; set; }
		public string RenewURL { get; set; }
		public string RenewURLText { get; set; }
	}
}