using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using System;
using System.Linq;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Jabberwocky.Autofac.Attributes;
using Informa.Library.User.Profile;

namespace Informa.Web.ViewModels
{
	[AutowireService(LifetimeScope.PerScope)]
	public class IndividualRenewalMessageViewModel : IIndividualRenewalMessageViewModel
	{
		private const string PRODUCT_TYPE = "publication";
		private readonly string[] SUBSCRIPTIONTYPE = new string[] { "individual", "free-trial", "individual internal" };

		protected readonly IIndividualSubscriptionRenewalMessageContext Context;
		protected readonly ISiteRootContext SiteRootContext;
		protected readonly IUserSubscriptionsContext UserSubscriptionsContext;
		protected readonly IUserProfileContext ProfileContext;

		public IndividualRenewalMessageViewModel(
				 ITextTranslator textTranslator,
				 IIndividualSubscriptionRenewalMessageContext context,
				 IAuthenticatedUserContext userContext,
				 ISiteRootContext siteRootContext,
				 IUserSubscriptionsContext userSubscriptionsContext,
				 IUserProfileContext profileContext)
		{
			Context = context;
			SiteRootContext = siteRootContext;
			UserSubscriptionsContext = userSubscriptionsContext;
			ProfileContext = profileContext;

			ISubscription record = userContext.IsAuthenticated ? GetLatestExpiringRecord() : null;

			DismissText = textTranslator.Translate("Subscriptions.Renewals.Dismiss");
			Display = DisplayMessage(record);

			Message = Display ? GetMessage(record, profileContext?.Profile?.FirstName ?? string.Empty) : string.Empty;
			Id = context.ID;
			RenewURL = context.RenewalLinkURL;
			RenewURLText = context.RenewalLinkText;
		}

		private ISubscription GetLatestExpiringRecord()
		{
			return UserSubscriptionsContext.Subscriptions?.Where(subscription => subscription.ProductCode.ToLower() == SiteRootContext.Item.Publication_Code.ToLower()
						&& (subscription.ExpirationDate - DateTime.Now).TotalDays <= SiteRootContext.Item.Days_To_Expiration
						&& SUBSCRIPTIONTYPE.Contains(subscription.SubscriptionType.ToLower())
						&& subscription.ProductType.ToLower() == PRODUCT_TYPE).OrderByDescending(o => o.ExpirationDate).FirstOrDefault() ?? null;
		}

		private bool DisplayMessage(ISubscription subscription)
		{
			if (subscription == null)
				return false;

			return true;
		}

		private string GetMessage(ISubscription subscription, string userName)
		{
			if (subscription == null)
			{
				return Context.Message_FreeTrial
						.Replace("#FIRST_NAME#", userName)
						.Replace("#SUB_EXPIRATION#", string.Empty);
			}

			if (subscription?.SubscriptionType.ToLower() == "free-trial")
			{
				return Context.Message_FreeTrial
						.Replace("#FIRST_NAME#", userName)
						.Replace("#SUB_EXPIRATION#", subscription.ExpirationDate.ToString("dd MMM yyyy"));
			}

			return Context.Message_IndividualSubscriptiong
					.Replace("#FIRST_NAME#", userName)
					.Replace("#SUB_EXPIRATION#", subscription.ExpirationDate.ToString("dd MMM yyyy"));
		}

		public string DismissText { get; set; }
		public bool Display { get; set; }
		public string Id { get; set; }
		public string Message { get; set; }
		public string RenewURL { get; set; }
		public string RenewURLText { get; set; }
	}
}