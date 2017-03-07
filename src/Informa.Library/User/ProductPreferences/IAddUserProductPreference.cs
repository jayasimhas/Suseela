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
        bool AddUserContentPreferences(string userName, string accessToken, string verticleCode
            , string publicationCode, string contentPreferences);

        ISavedDocumentWriteResult AddSavedDocument(string verticalName, string publicationCode,string userName, string documentName, string documentDescription, string documentId, string accessToken);

        bool AddNewsletterUserOptIns(string verticalName, string publicationCode, string username,string accessToken, IEnumerable<INewsletterUserOptIn> optIns);

        bool AddOffersOptIns(string verticalName, string publicationCode, string username, string accessToken, bool optIn);
    }
}