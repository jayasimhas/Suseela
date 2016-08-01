using Informa.Library.User;
using Informa.Library.User.Registration;
using Informa.Library.User.Registration.Web;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Registration;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Autofac;
using Informa.Library.Company;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Offer;

namespace Informa.Web.Areas.Account.Controllers
{
    public class RegistrationApiController : ApiController
    {
        protected readonly IFindUserByEmail FindUser;
        protected readonly INewUserFactory NewUserFactory;
        protected readonly IWebRegisterUser RegisterUser;
        protected readonly IWebSetOptInsRegisterUser SetOptInsRegisterUser;
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly ISetPublicationsNewsletterUserOptIns SetNewsletterUserOptInsContext;
        protected readonly IUpdateOfferUserOptInContext OffersOptIn;

        public RegistrationApiController(
            IFindUserByEmail findUser,
            INewUserFactory newUserFactory,
            IWebRegisterUser registerUser,
            IWebSetOptInsRegisterUser setOptInsRegisterUser,
            IUserCompanyContext userCompanyContext,
            ISetPublicationsNewsletterUserOptIns setNewsletterUserOptInsContext,
            IUpdateOfferUserOptInContext offersOptIn)
        {
            FindUser = findUser;
            NewUserFactory = newUserFactory;
            RegisterUser = registerUser;
            SetOptInsRegisterUser = setOptInsRegisterUser;
            UserCompanyContext = userCompanyContext;
            SetNewsletterUserOptInsContext = setNewsletterUserOptInsContext;
            OffersOptIn = offersOptIn;
        }

        [HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult PreRegister(PreRegisterRequest request)
        {
            if (IsExistingUser(request.Username))
            {
                return CreateUserExistsResponse();
            }

            return Ok(new
            {
                success = true
            });
        }

        [HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult Register(RegisterRequest request)
        {
            if (IsExistingUser(request.Username))
            {
                return CreateUserExistsResponse();
            }

            var newUser = NewUserFactory.Create();

            newUser.FirstName = request.FirstName;
            newUser.LastName = request.LastName;
            newUser.Password = request.Password;
            newUser.Username = request.Username;

            if (request.AssociateMaster)
            {
                newUser.MasterId = request.MasterId;
                newUser.MasterPassword = request.MasterPassword;
            }

            var registerResult = RegisterUser.Register(newUser);
            var success = registerResult.Success;
            var reasons = new List<string>();

            if (!success)
            {
                reasons.AddRange(registerResult.Errors.Select(e => GetRegisterValidationReason(e)));
            }

            var registrationType = GetRegistrationType(UserCompanyContext);

            return Ok(new
            {
                success = success,
                reasons = reasons,
                registration_type = registrationType
            });
        }

        private string GetRegistrationType(IUserCompanyContext context)
        {
            if (context.Company == null)
            {
                return "Free User";
            }

            if (context.Company.Type == CompanyType.TransparentIP)
            {
                return "Transparent IP";
            }
            return "Corporate";
        }

        public string GetRegisterValidationReason(string error)
        {
            switch (error)
            {
                case "MasterIdInvalid":
                    return RegisterValidationReason.MasterIdInvalid;
                case "MasterIdExpired":
                    return RegisterValidationReason.MasterIdExpired;
                default:
                    return "Unknown";
            }
        }

        [HttpPost]
        [ArgumentsRequired]
        public IHttpActionResult SetOptIns(SetOptInsRequest request)
        {
            var newsletterUpdated = SetNewsletterUserOptInsContext.Set(request?.Newsletters?.Where(w => w.NewsletterChecked).Select(s => s.PublicationCode).ToList() ?? Enumerable.Empty<string>());
            var offersUpdated = OffersOptIn.Update(!request.Offers);

            return Ok(new
            {
                success = newsletterUpdated && offersUpdated
            });
        }

        public bool IsExistingUser(string username)
        {
            return FindUser.Find(username) != null;
        }

        public IHttpActionResult CreateUserExistsResponse()
        {
            return Ok(new
            {
                success = false,
                reasons = new List<string>
                {
                    { RegisterValidationReason.UsernameExists }
                }
            });
        }
    }
}
