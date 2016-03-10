using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Informa.Library.Newsletter;
using Informa.Library.Site.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Web.Areas.Account.Models.User.Management;

namespace Informa.Web.Areas.Account.Controllers
{
    public class SavedContentApiController : ApiController
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageSavedContent ManageSavedContent;

        public SavedContentApiController(
            IAuthenticatedUserContext userContext,
            IManageSavedContent manageSavedContent)
        {
            UserContext = userContext;
            ManageSavedContent = manageSavedContent;
        }

        [HttpPost]
        public IHttpActionResult RemoveItem(SavedContentRemoveRequest request)
        {
            var result = ManageSavedContent.RemoveItem(UserContext.User, request.DocumentID);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        public IHttpActionResult SaveItem(SavedContentSaveRequest request)
        {
            var result = ManageSavedContent.SaveItem(UserContext.User, request.DocumentName, request.DocumentDescription, request.DocumentID, DateTime.Now);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }
    }
}