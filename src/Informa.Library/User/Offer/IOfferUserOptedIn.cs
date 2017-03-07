using Informa.Library.User.Newsletter;

namespace Informa.Library.User.Offer
{
    public interface IOfferUserOptedIn
    {
        OffersOptIn OptedIn(string username);
    }
}
