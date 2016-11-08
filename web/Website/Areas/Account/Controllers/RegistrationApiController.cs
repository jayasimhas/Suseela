using System.Collections;
using Informa.Library.User;
using Informa.Library.User.Registration;
using Informa.Library.User.Registration.Web;
using Informa.Library.Utilities.WebApi.Filters;
using Informa.Web.Areas.Account.Models.User.Registration;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Glass.Mapper.Sc.IoC;
using Informa.Library.Company;
using Informa.Library.Subscription.User;
using Informa.Library.User.Authentication;
using Informa.Library.User.Newsletter;
using Informa.Library.User.Offer;
using Informa.Library.User.Orders;
using Informa.Library.User.Profile;
using System;
using log4net;

namespace Informa.Web.Areas.Account.Controllers
{
    public class RegistrationApiController : ApiController
    {
        protected readonly IFindUserByEmail FindUser;
        protected readonly INewUserFactory NewUserFactory;
        protected readonly IWebRegisterUser RegisterUser;
        protected readonly IWebSetOptInsRegisterUser SetOptInsRegisterUser;
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly IManageAccountInfo AccountInfo;
        protected readonly IUserOrder UserOrder;
        protected readonly ISetPublicationsNewsletterUserOptIns SetNewsletterUserOptInsContext;
        protected readonly IUpdateOfferUserOptInContext OffersOptIn;
        private readonly ILog Logger;

        public RegistrationApiController(
            IFindUserByEmail findUser,
            INewUserFactory newUserFactory,
            IWebRegisterUser registerUser,
            IWebSetOptInsRegisterUser setOptInsRegisterUser,
            IManageAccountInfo accountInfo,
            IUserOrder userOrder,
            IUserCompanyContext userCompanyContext,
            ISetPublicationsNewsletterUserOptIns setNewsletterUserOptInsContext,
            IUpdateOfferUserOptInContext offersOptIn,
            ILog logger)
        {
            FindUser = findUser;
            NewUserFactory = newUserFactory;
            RegisterUser = registerUser;
            SetOptInsRegisterUser = setOptInsRegisterUser;
            UserCompanyContext = userCompanyContext;
            AccountInfo = accountInfo;
            UserOrder = userOrder;
            SetNewsletterUserOptInsContext = setNewsletterUserOptInsContext;
            OffersOptIn = offersOptIn;
            Logger = logger;

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

        [HttpPost]
        [ValidateReasons]
        [ArgumentsRequired]
        public IHttpActionResult RegisterFreeTrial(RegisterFreeTrialRequest form)
        {
            if (IsExistingUser(form.Username))
                return CreateUserExistsResponse();

            var newUser = NewUserFactory.Create();

            newUser.FirstName = form.FirstName;
            newUser.LastName = form.LastName;
            newUser.Password = form.Password;
            newUser.Username = form.Username;

            var registerResult = RegisterUser.Register(newUser);
            if (!registerResult.Success)
                return Ok(new
                {
                    success = false,
                    reasons = registerResult.Errors.Select(GetRegisterValidationReason).ToList(),
                });

            var userContext = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IAuthenticatedUserContext)) as IAuthenticatedUserContext;
            if (!userContext.IsAuthenticated)
                return Ok(new
                {
                    success = false,
                    reasons = "LoginFailed"
                });

            var updateResult = AccountInfo.UpdateContactInfo(
                userContext.User, form.FirstName, form.LastName, string.Empty, string.Empty, string.Empty, form.Country, form.Address1, form.Address2,
                form.City, form.PostalCode, form.State, form.Country, form.Address1, form.Address2, form.City, form.PostalCode, form.State,
                string.Empty, string.Empty, string.Empty, form.Phone, string.Empty, form.Company, string.Empty, string.Empty, form.JobTitle);
            if (!updateResult.Success)
                return Ok(new
                {
                    success = false,
                    reasons = "AccountUpdatedFailed"
                });

            var userSubsContext = System.Web.Mvc.DependencyResolver.Current.GetService(typeof(IUserSubscriptionsContext)) as IUserSubscriptionsContext;
            var orderResult = UserOrder.CreateUserOrder(userContext.User, userSubsContext.Subscriptions);
            if (!orderResult.Success)
                return Ok(new
                {
                    success = false,
                    reasons = "CreateOrderFailed"
                });

            return Ok(new
            {
                success = true,
                reasons = string.Empty,
                registration_type = "Free Trial"
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
            try
            {
                var newsletterUpdated = SetNewsletterUserOptInsContext.Set(request?.Newsletters?.Where(w => w.NewsletterChecked).Select(s => s.PublicationCode).ToList() ?? Enumerable.Empty<string>());
                Logger.Error("Var newsletterUpdated = " + newsletterUpdated.ToString());
                var offersUpdated = OffersOptIn.Update(!request.Offers);
                Logger.Error("Var offersUpdated = " + offersUpdated.ToString());
                return Ok(new
                {
                    success = newsletterUpdated && offersUpdated
                });
            }
            catch (Exception ex)
            {
                Logger.Error("Error in SetOptIns Call", ex);
            }

            return Ok(new
            {
                success = false
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
