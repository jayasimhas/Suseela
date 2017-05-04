using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Publication;
using Informa.Library.Services.Article;
using Informa.Library.User.Document;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.Areas.Account.Models.User.Management;
using Sitecore.Data;
using System.Linq;
using System.Web;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SavedDocumentApiController : ApiController
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISitePublicationNameContext NewsletterTypeContext;
        protected readonly ISaveDocumentContext SaveDocumentContext;
        protected readonly IRemoveDocumentContext RemoveDocumentContext;
        protected readonly IArticleService ArticleService;
        protected readonly ISavedDocumentsContext SavedDocumentsContext;

        protected string BadIDKey => TextTranslator.Translate("SavedDocument.BadID");

        public SavedDocumentApiController(
            ISitecoreContext sitecoreContext,
            ITextTranslator textTranslator,
            ISitePublicationNameContext newsletterTypeContext,
            ISaveDocumentContext saveDocumentContext,
            IRemoveDocumentContext removeDocumentContext,
            IArticleService articleService,
            ISavedDocumentsContext savedDocumentsContext
            )
        {
            SitecoreContext = sitecoreContext;
            TextTranslator = textTranslator;
            NewsletterTypeContext = newsletterTypeContext;
            SaveDocumentContext = saveDocumentContext;
            RemoveDocumentContext = removeDocumentContext;
            ArticleService = articleService;
            SavedDocumentsContext = savedDocumentsContext;
        }

        [HttpGet]
        public IHttpActionResult IsSaved(string DocumentID)
        {
            // var rawDocuementId = documentId.ToString("D").ToUpper();

            if (string.IsNullOrEmpty(DocumentID))
                return null;

            bool result = SavedDocumentsContext.SavedDocuments.Any(sd => sd.DocumentId.ToUpper().Equals(DocumentID));
            return Ok(new
            {
                success = result,
                message = result
            });
        }


        [HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult RemoveItem(SavedDocumentRemoveRequest request)
        {
            var result = RemoveDocumentContext.Remove(request.DocumentID, request.SalesforceID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult SaveItem(SavedDocumentSaveRequest request)
        {
            Sitecore.Data.ID itemID = ID.Null;
            if (!Sitecore.Data.ID.TryParse(request.DocumentID, out itemID))
            {
                return Ok(new
                {
                    success = false,
                    message = BadIDKey,
                    salesforceid = string.Empty
                });
            }

            var page = SitecoreContext.GetItem<I___BasePage>(itemID.Guid);
            var article = SitecoreContext.GetItem<IArticle>(itemID.Guid);
            var publicationName = ArticleService.GetArticlePublicationName(article);
            var result = SaveDocumentContext.Save(page.Title, publicationName, request.DocumentID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message,
                salesforceid = result.SalesforceId
            });
        }
    }
}