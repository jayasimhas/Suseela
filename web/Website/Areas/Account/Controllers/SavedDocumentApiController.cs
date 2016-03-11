using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.Site.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.Areas.Account.Models.User.Management;
using Sitecore.Data;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SavedDocumentApiController : ApiController
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageSavedDocuments ManageSavedDocument;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ITextTranslator TextTranslator;

        protected string BadIDKey => TextTranslator.Translate("SavedDocument.BadID");

        public SavedDocumentApiController(
            IAuthenticatedUserContext userContext,
            IManageSavedDocuments manageSavedDocument,
            ISitecoreContext sitecoreContext,
            ITextTranslator textTranslator)
        {
            UserContext = userContext;
            ManageSavedDocument = manageSavedDocument;
            SitecoreContext = sitecoreContext;
            TextTranslator = textTranslator;
        }

        [HttpPost]
        public IHttpActionResult RemoveItem(SavedDocumentRemoveRequest request)
        {
            var result = ManageSavedDocument.RemoveItem(UserContext.User, request.DocumentID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
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
            
            var result = ManageSavedDocument.SaveItem(UserContext.User, page.Title, NewsletterType.Scrip.ToString(), request.DocumentID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }
    }
}