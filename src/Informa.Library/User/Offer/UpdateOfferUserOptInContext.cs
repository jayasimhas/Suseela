using Informa.Library.User.Authentication;
using Jabberwocky.Glass.Autofac.Attributes;

namespace Informa.Library.User.Offer
{
    [AutowireService(LifetimeScope.Default)]
    public class UpdateOfferUserOptInContext : IUpdateOfferUserOptInContext
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IOfferUserOptedInContext OfferOptedInContext;
        protected readonly IUpdateOfferUserOptIn UpdateOfferOptIn;

        public UpdateOfferUserOptInContext(
    IAuthenticatedUserContext userContext,
    IOfferUserOptedInContext offerOptedInContext,
    IUpdateOfferUserOptIn updateOfferOptIn)
        {
            UserContext = userContext;
            OfferOptedInContext = offerOptedInContext;
            UpdateOfferOptIn = updateOfferOptIn;
        }

        public bool Update(bool optIn)
        {
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
