using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;
using log4net;

namespace Informa.Library.User.Offer
{
	[AutowireService(LifetimeScope.Default)]
	public class UpdateOfferUserOptInContext : IUpdateOfferUserOptInContext
	{
		protected readonly IAuthenticatedUserContext UserContext;
		protected readonly IOfferUserOptedInContext OfferOptedInContext;
		protected readonly IUpdateOfferUserOptIn UpdateOfferOptIn;
        protected readonly ILog Logger;

                public UpdateOfferUserOptInContext(
			IAuthenticatedUserContext userContext,
			IOfferUserOptedInContext offerOptedInContext,
			IUpdateOfferUserOptIn updateOfferOptIn,
            ILog logger)
		{
			UserContext = userContext;
			OfferOptedInContext = offerOptedInContext;
			UpdateOfferOptIn = updateOfferOptIn;
            Logger = logger;
		}

		public bool Update(bool optIn)
		{
            Logger.Error("UserContext.IsAuthenticated : " + UserContext.IsAuthenticated);
            if (!UserContext.IsAuthenticated)
			{
				return false;
			}

			var success = UpdateOfferOptIn.Update(UserContext.User?.Username, optIn);

			if (success)
			{
				OfferOptedInContext.Clear();
			}

			return success;
		}
	}
}
