using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Management;

namespace Informa.Web.Areas.Account.Controllers
{
    public class ContactInfoApiController : ApiController
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageAccountInfo AccountInfo;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ITextTranslator TextTranslator;

        public ContactInfoApiController(
            IAuthenticatedUserContext userContext,
            IManageAccountInfo accountInfo,
            ISitecoreContext sitecoreContext,
            ITextTranslator textTranslator)
        {
            UserContext = userContext;
            AccountInfo = accountInfo;
            SitecoreContext = sitecoreContext;
            TextTranslator = textTranslator;
        }

        [HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult UpdateContactInfo(ContactInformationUpdateRequest form)
        {
            if (string.IsNullOrEmpty(UserContext.User?.Username))
            {
                return Ok(new
                {
                    success = false,
                    message = NullUserKey
                });
            }

            var result = AccountInfo.UpdateContactInfo(
                UserContext.User, form.FirstName, form.LastName, form.MiddleInitial, form.NameSuffix, form.Salutation,
                form.BillCountry, form.BillAddress1, form.BillAddress2, form.BillCity, form.BillPostalCode,
                form.BillState,
                form.ShipCountry, form.ShipAddress1, form.ShipAddress2, form.ShipCity, form.ShipPostalCode,
                form.ShipState,
                form.Fax, form.CountryCode, form.PhoneExtension, form.Phone, form.PhoneType, form.Company,
                form.JobFunction, form.JobIndustry, form.JobTitle);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        [HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult UpdatePassword(PasswordUpdateRequest request)
        {
            if (string.IsNullOrEmpty(UserContext.User?.Username))
            {
                return Ok(new
                {
                    success = false,
                    message = NullUserKey
                });
            }

            var result = AccountInfo.UpdatePassword(UserContext.User, request.CurrentPassword, request.NewPassword,
                false);

            return Ok(new
            {
                success = result.Success,
                message = result.Message
            });
        }

        protected string NullUserKey => TextTranslator.Translate("ContactInfo.NullUser");
    }
}