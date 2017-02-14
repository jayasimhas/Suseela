using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Library.User.Entitlement;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SubscriptionViewModel
    {
        public string Publication { get; set; }
        public string Type { get; set; }
        public DateTime Expiration { get; set; }
        public bool Renewable { get; set; }
        public bool Subscribable { get; set; }
        public IEnumerable<SubscriptionChannelViewModel> ChannelItems { get; set; }
        public bool IsCurrentPublication { get; set; }

        public EntitlementLevel Entitlement_Type { get; set; }
    }

    public class SubscriptionChannelViewModel
    {
        public string ChannelName { get; set; }
        public string ChannelCode { get; set; }
        public DateTime ChannelExpirationdate { get; set; }
        public bool Renewable { get; set; }
        public bool Subscribable { get; set; }
    }
}