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
using Informa.Library.User.Authentication.Web;

namespace Informa.Web.Areas.Account.Controllers
{
    public class ContactInfoApiController : ApiController
    {
        protected readonly IAuthenticatedUserContext UserContext;
        protected readonly IManageAccountInfo AccountInfo;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IUserProfileContext ProfileContext;
        protected readonly IWebAuthenticateUser WebAuthenticateUser;

        public ContactInfoApiController(
            IAuthenticatedUserContext userContext,
            IManageAccountInfo accountInfo,
            ITextTranslator textTranslator,
            IUserProfileContext profileContext, 
            IWebAuthenticateUser webAuthenticateUser)
        {
            UserContext = userContext;
            AccountInfo = accountInfo;
            TextTranslator = textTranslator;
            ProfileContext = profileContext;
            WebAuthenticateUser = webAuthenticateUser;
        }

        [HttpPost]
        //[ValidateReasons]
        //[ArgumentsRequired]
        public IHttpActionResult UpdateContactInfo(ContactInformationUpdateRequest form)
        {
            if (string.IsNullOrEmpty(WebAuthenticateUser?.AuthenticatedUser?.UserId))
            {
                return Ok(new
                {
                    success = false,
                    reasons = new string[] { NullUserKey }
                });
            }

            var result = AccountInfo.UpdateContactInfo(
                WebAuthenticateUser.AuthenticatedUser,
                form.Id,
                form.Company,
                form.JobTitle,
                form.Phone,
                form.BillAddress1,
                form.BillCity,
                form.BillPostalCode);


            //var result = AccountInfo.UpdateContactInfo(
            //    UserContext.User, form.FirstName, form.LastName, form.MiddleInitial, form.NameSuffix, form.Salutation,
            //    form.BillCountry, form.BillAddress1, form.BillAddress2, form.BillCity, form.BillPostalCode,
            //    form.BillState,
            //    form.ShipCountry, form.ShipAddress1, form.ShipAddress2, form.ShipCity, form.ShipPostalCode,
            //    form.ShipState,
            //    form.Fax, form.CountryCode, form.PhoneExtension, form.Phone, form.PhoneType, form.Company,
            //    form.JobFunction, form.JobIndustry, form.JobTitle);

            if (result.Success)
                ProfileContext.Clear();

            return Ok(new
            {
                success = result.Success,
                reasons = new string[] { GetContactResultKey(result) }
            });


            // commented for POC
            //if (string.IsNullOrEmpty(UserContext.User?.Username))
            // {
            //     return Ok(new
            //     {
            //         success = false,
            //         reasons = new string[] { NullUserKey }
            //     });
            // }

            // var result = AccountInfo.UpdateContactInfo(
            //     UserContext.User, form.FirstName, form.LastName, form.MiddleInitial, form.NameSuffix, form.Salutation,
            //     form.BillCountry, form.BillAddress1, form.BillAddress2, form.BillCity, form.BillPostalCode,
            //     form.BillState,
            //     form.ShipCountry, form.ShipAddress1, form.ShipAddress2, form.ShipCity, form.ShipPostalCode,
            //     form.ShipState,
            //     form.Fax, form.CountryCode, form.PhoneExtension, form.Phone, form.PhoneType, form.Company,
            //     form.JobFunction, form.JobIndustry, form.JobTitle);

            // if (result.Success)
            //     ProfileContext.Clear();

            // return Ok(new
            // {
            //     success = result.Success,
            //     reasons = new string[] { GetContactResultKey(result) }
            // });

        }

           protected string GetContactResultKey(IAccountInfoWriteResult result)
           {
               if (result.Success)
                   return "";
               else if (result.Message.Contains("required"))
                   return "Required";
               else
                   return "ContactUpdateFailed";
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
                       reasons = new string[] { NullUserKey }
                   });
               }

               var result = AccountInfo.UpdatePassword(UserContext.User, request.CurrentPassword, request.NewPassword, false);

               return Ok(new
               {
                   success = result.Success,
                   reasons = new string[] { GetPasswordResultKey(result) }
               });
           }

           protected string GetPasswordResultKey(IAccountInfoWriteResult result)
           {
               if (result.Success)
                   return "";
               else if (result.Message.Contains("invalid username and password"))
                   return "InvalidPasswordValues";
               else
                   return "PasswordUpdateFailed";
           }

           protected string NullUserKey => TextTranslator.Translate("ContactInfo.NullUser");
       }
   }
 