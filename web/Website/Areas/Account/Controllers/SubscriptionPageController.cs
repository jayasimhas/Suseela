using Glass.Mapper.Sc;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.Areas.Account.ViewModels.Subscription;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SubscriptionPageController : Controller
    {
		// GET: Account/SubscriptionPage
		private readonly ISitecoreService _sitecoreMasterService;
		private readonly IItemReferences _itemReferences;
		public SubscriptionPageController(Func<string, ISitecoreService> sitecoreFactory, IItemReferences itemReferences)
		{
			_sitecoreMasterService = sitecoreFactory(Constants.MasterDb);
			_itemReferences = itemReferences;
		}
		public ActionResult Index(string pub)
        {
			//TODO: Add logic to subscribe the user using salesforce and also check for logged in user functionality
			if (!string.IsNullOrEmpty(pub) && pub.ToLower().Equals("scrip"))
			{
				SubscriptionModel subscriptionModel = new SubscriptionModel();
				var item = _sitecoreMasterService.GetItem<I___BasePage>(_itemReferences.SubscriptionPage);
				subscriptionModel.BodyText = item.Body;
				return View("~/Areas/Account/Views/Management/Subscriptions.cshtml", subscriptionModel);
			}
			var emailPreferncesItem = _sitecoreMasterService.GetItem<Item>(_itemReferences.EmailPreferences);
            return Redirect("/accounts/email-preferences"); // TODO Remove harcoded path
        }
    }
}