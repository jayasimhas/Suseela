using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.User.Document;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.Areas.Account.Models.User.Management;
using Sitecore.Data;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SavedDocumentApiController : ApiController
    {
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ITextTranslator TextTranslator;
		protected readonly ISiteNewsletterTypeContext NewsletterTypeContext;
		protected readonly ISaveDocumentContext SaveDocumentContext;
		protected readonly IRemoveDocumentContext RemoveDocumentContext;

		protected string BadIDKey => TextTranslator.Translate("SavedDocument.BadID");

        public SavedDocumentApiController(
            ISitecoreContext sitecoreContext,
            ITextTranslator textTranslator,
			ISiteNewsletterTypeContext newsletterTypeContext,
			ISaveDocumentContext saveDocumentContext,
			IRemoveDocumentContext removeDocumentContext)
        {
            SitecoreContext = sitecoreContext;
            TextTranslator = textTranslator;
			NewsletterTypeContext = newsletterTypeContext;
			SaveDocumentContext = saveDocumentContext;
			RemoveDocumentContext = removeDocumentContext;
        }

        [HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult RemoveItem(SavedDocumentRemoveRequest request)
        {
            var result = RemoveDocumentContext.Remove(request.DocumentID);

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
                    message = BadIDKey
                });
            }

            var page = SitecoreContext.GetItem<I___BasePage>(itemID.Guid);
            
            var result = SaveDocumentContext.Save(page.Title, NewsletterTypeContext.Type, request.DocumentID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }
    }
}