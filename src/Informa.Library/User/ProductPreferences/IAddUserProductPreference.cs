using Informa.Library.User.Content;
using Informa.Library.User.Document;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Search;
using System.Collections.Generic;

namespace Informa.Library.User.ProductPreferences
{
    public interface IAddUserProductPreference
    {
        IContentResponse AddUserSavedSearch(string accessToken, ISavedSearchEntity entity);
        bool AddUserContentPreferences(string userName, string accessToken, string verticalPreferenceLocale
            , string publicationCode, string contentPreferences);

        ISavedDocumentWriteResult AddSavedDocument(string verticalPreferenceLocale, string publicationCode,string userName, string documentName, string documentDescription, string documentId, string accessToken);

        bool AddNewsletterUserOptIns(string verticalPreferenceLocale, string publicationCode, string username,string accessToken, IEnumerable<INewsletterUserOptIn> optIns);

        bool AddOffersOptIns(string verticalPreferenceLocale, string publicationCode, string username, string accessToken, bool optIn);
    }
}