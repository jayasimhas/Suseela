using System;
using System.Collections.Generic;
using System.Linq;
using Informa.Library.Globalization;
using Informa.Library.Subscription;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.ViewModels.Account;
using Informa.Library.Publication;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
	public class SubscriptionsViewModel : GlassViewModel<ISubscriptions_Page>
	{
		public readonly ITextTranslator TextTranslator;
		public readonly IAuthenticatedUserContext UserContext;
		public readonly ISignInViewModel SignInViewModel;

		protected readonly IFindSitePublicationByCode FindSitePublication;

		private readonly Dictionary<string, bool> RenewBtnSettings;
		private readonly Dictionary<string, bool> SubscriptionBtnSettings;
		private readonly IEnumerable<ISubscription> _subcriptions; 

		public SubscriptionsViewModel(
			ITextTranslator translator,
			IAuthenticatedUserContext userContext,
			IUserSubscriptionsContext userSubscriptionsContext,
			ISignInViewModel signInViewModel,
			IFindSitePublicationByCode findSitePublication)
		{
			TextTranslator = translator;
			UserContext = userContext;
			SignInViewModel = signInViewModel;
			FindSitePublication = findSitePublication;

			RenewBtnSettings = new Dictionary<string, bool>();
			SubscriptionBtnSettings = new Dictionary<string, bool>();
			_subcriptions = userSubscriptionsContext.Subscriptions;

			foreach (var sub in _subcriptions)
			{
				//renew btns
				if (!RenewBtnSettings.ContainsKey(sub.ProductCode))
					RenewBtnSettings.Add(sub.ProductCode, WithinRenewRange(sub.ExpirationDate));
				else
					RenewBtnSettings[sub.ProductCode] &= WithinRenewRange(sub.ExpirationDate);

				//subscribe btns
				if (!SubscriptionBtnSettings.ContainsKey(sub.ProductCode))
					SubscriptionBtnSettings.Add(sub.ProductCode, IsValidSubscription(sub));
				else
					SubscriptionBtnSettings[sub.ProductCode] |= IsValidSubscription(sub);
			}
		}

		public IEnumerable<SubscriptionViewModel> SubscriptionViewModels
		{
			get
			{
				return _subcriptions.Select(s => new SubscriptionViewModel
				{
					Expiration = s.ExpirationDate,
					Publication = FindSitePublication.Find(s.Publication)?.Name ?? s.Publication,
					Renewable = ShowRenewButton(s),
					Subscribable = ShowSubscribeButton(s.ProductCode),
					Type = s.ProductType,
                    TaxonomyItems = GeTaxonomyItemsByProductCode(s.ProductCode),
                    IsCurrentPublication = GlassModel.GetAncestors<ISite_Root>().FirstOrDefault().Publication_Code.Equals(s.ProductCode)
                });
			}
		}

	    public IEnumerable<ITaxonomy_Item> GeTaxonomyItemsByProductCode(string productCode)
	    {
	        IEnumerable<ITaxonomy_Item> taxnomyItems = new List<ITaxonomy_Item>();
	        var verticalRootItem = GlassModel.GetAncestors<IVertical_Root>().FirstOrDefault();
	        if (verticalRootItem != null)
	        {
	            var siteRoot =
	                verticalRootItem._ChildrenWithInferType.OfType<ISite_Root>()
	                    .FirstOrDefault(eachChild => eachChild.Publication_Code.Equals(productCode));
	            if (siteRoot != null)
	            {
	                var pubItem = siteRoot._ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault()
	                    ._ChildrenWithInferType.OfType<IAccount_Landing_Page>().FirstOrDefault()
	                    ._ChildrenWithInferType.OfType<ISubscriptions_Page>().FirstOrDefault();

	                if (pubItem != null)
	                    return pubItem.Taxonomies;
	            }
            }
	        return taxnomyItems;
	    }

	    public bool ShowRenewButton(ISubscription subscription)
		{
			//if all subscriptions of this type are within renew range and this subscription is not multi-user 
			return _subcriptions
							.Where(a => a.ProductCode.Equals(subscription.ProductCode))
							.All(b => WithinRenewRange(b.ExpirationDate))
					&& !IsMultiUser(subscription.SubscriptionType);
		}

		public bool ShowSubscribeButton(string productCode)
		{
			//if there aren't any valid subscriptions
			return (SubscriptionBtnSettings.ContainsKey(productCode)) && !RenewBtnSettings[productCode];
		}

		public bool IsMultiUser(string subscriptionType)
		{
			return subscriptionType.ToLower().Equals("multi-user");
		}

		public bool WithinRenewRange(DateTime dt)
		{
			// (08/31/2013) - (08/01/2013) = 31
			int days = Convert.ToInt16((dt - DateTime.Now).TotalDays);
			return days < 119 && days >= 0;
		}

		public bool IsValidSubscription(ISubscription s)
		{
			// (08/31/2013) - (08/01/2013) = 31
			int days = Convert.ToInt16((s.ExpirationDate - DateTime.Now).TotalDays);
			return days > 0;
		}

		public string OffSiteRenewLink => GlassModel.Off_Site_Renew_Link?.Url ?? "#";
        public string SubscriptionHeaderSubTitle => GlassModel.Body;
	    public string SubjectType => TextTranslator.Translate("Subscriptions.SubjectType");
        public string Subscribe => TextTranslator.Translate("Subscriptions.Subscribe");
        public string Subscribed => TextTranslator.Translate("Subscriptions.Subscribed");
        public string Renew => TextTranslator.Translate("Subscriptions.Renew");
        public string OffSiteSubscriptionLink => GlassModel.Off_Site_Subscription_Link?.Url ?? "#";
		public bool IsAuthenticated => UserContext.IsAuthenticated;
		public string Title => TextTranslator.Translate("Subscriptions.Title");
		public string PublicationText => TextTranslator.Translate("Subscriptions.Publication");
		public string SubscriptionTypeText => TextTranslator.Translate("Subscriptions.SubscriptionType");
		public string ExpirationDateText => TextTranslator.Translate("Subscriptions.ExpirationDate");
		public string ActionText => TextTranslator.Translate("Subscriptions.Action");
	}
}