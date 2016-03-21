using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Web.ViewModels;

namespace Informa.Web.Areas.Account.ViewModels.Subscription
{
	public class SubscriptionModel
	{
		public string BodyText { get; set; }
        public bool IsAuthenticated { get; set; }
        public ISignInViewModel SignInViewModel { get; set; }
    }
}