using Informa.Library.User.Content;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Search;
using System.Collections.Generic;

namespace Informa.Library.User.ProductPreferences
{
    public interface IUpdateUserProductPreference
    {
        IContentResponse UpdateUserSavedSearch(string accessToken, ISavedSearchEntity entity);

        bool UpdateNewsletterUserOptIns(string accessToken, string username, string publicationCode, IEnumerable<INewsletterUserOptIn> optIns);
        bool UpdateOffersOptIns(string accessToken, string username, string publicationCode, OffersOptIn optIn);
    }
}