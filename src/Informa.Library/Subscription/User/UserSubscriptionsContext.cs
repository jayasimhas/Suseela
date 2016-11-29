using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Informa.Library.Subscription.User
{
    [AutowireService(LifetimeScope.PerScope)]
    public class UserSubscriptionsContext : IUserSubscriptionsContext
    {
        private const string subscriptionsSessionKey = nameof(UserSubscriptionsContext);

        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IAuthenticatedUserSession UserSession;
        protected readonly IFindUserSubscriptions FindSubscriptions;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public UserSubscriptionsContext(
            IAuthenticatedUserContext userContext,
            IAuthenticatedUserSession userSession,
            IFindUserSubscriptions findSubscriptions,
            ISiteRootContext siterootContext,
            IGlobalSitecoreService globalService)
        {
            UserContext = userContext;
            UserSession = userSession;
            FindSubscriptions = findSubscriptions;
            SiterootContext = siterootContext;
            GlobalService = globalService;
        }

        public IEnumerable<ISubscription> Subscriptions
        {
            get
            {
                if (!UserContext.IsAuthenticated)
                {
                    return Enumerable.Empty<ISubscription>();
                }
                //Commenting it for the time being for local verification.
                var subscriptionSession = UserSession.Get<IEnumerable<ISubscription>>(subscriptionsSessionKey);

                if (subscriptionSession.HasValue)
                {
                    return subscriptionSession.Value;
                }

                var subscriptions = Subscriptions = FindSubscriptions.Find(UserContext.User?.Username);

                if (subscriptions != null && subscriptions.Any())
                {

                    if (subscriptions.Any(n => n.ProductCode == SiterootContext.Item.Publication_Code && n.ExpirationDate > DateTime.Now))
                    {
                        var selectedSubscription = subscriptions.Where(n => n.ProductCode == SiterootContext.Item.Publication_Code).FirstOrDefault();
                        List<ChannelSubscription> channelSubscription = new List<ChannelSubscription>();
                        var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                    _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();
                        var channelsPageItem = homeItem._ChildrenWithInferType.OfType<IChannels_Page>().FirstOrDefault();
                        var channelPages = channelsPageItem._ChildrenWithInferType.OfType<IChannel_Page>();
                        if (homeItem != null && channelsPageItem != null && channelPages != null)
                        {
                            foreach (var channel in channelPages)
                            {
                                channelSubscription.Add(new ChannelSubscription { ChannelId = channel.Channel_Code, ExpirationDate = subscriptions.Where(n => n.ProductCode == SiterootContext.Item.Publication_Code).Select(p => p.ExpirationDate).FirstOrDefault() });

                            }
                        }
                        selectedSubscription.SubscribedChannels = channelSubscription;
                    }
                }
                return subscriptions;
            }
            set
            {
                UserSession.Set(subscriptionsSessionKey, value);
            }
        }
    }
}
