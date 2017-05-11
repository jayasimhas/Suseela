using Informa.Library.Globalization;
using Informa.Library.User.Authentication;
using Informa.Library.User.UserPreference;
using Informa.Web.Areas.Account.Models.User.Personalization;
using Informa.Web.ViewModels.MyView;
using Jabberwocky.Core.Caching;
using System.Web.Http;

namespace Informa.Web.Areas.Account.Controllers
{
    public class PersonalizeUserPreferencesApiController : ApiController
    {
        protected readonly IUserPreferenceContext UserPreferenceContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly ICacheProvider CacheProvider;

        public PersonalizeUserPreferencesApiController(
            IUserPreferenceContext userPreferenceContext,
            ITextTranslator textTranslator, IAuthenticatedUserContext userContext, ICacheProvider cacheProvider)
        {
            UserPreferenceContext = userPreferenceContext;
            TextTranslator = textTranslator;
            CacheProvider = cacheProvider;
        }

        /// <summary>
        /// Updates the user preferences.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Update(PersonalizeUserPreferencesRequest request)
        {
            if (string.IsNullOrEmpty(request.UserPreferences))
            {
                return Ok(new
                {
                    success = false,
                    reason = NoPreferencesSelectedKey
                });
            }
            var response = UserPreferenceContext.Set(request.UserPreferences);

            if(response)
            {
                CacheProvider.EmptyCache();
            }

            return Ok(new
            {
                success = response,
                reason = response ? SuccessMessage : ErrorMessage
            });
        }
        /// <summary>
        /// Gets the no preferences selected key.
        /// </summary>
        /// <value>
        /// The no preferences selected key.
        /// </value>
        protected string NoPreferencesSelectedKey => TextTranslator.Translate("MyViewSettings.NoPreferencesSelected");

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        protected string ErrorMessage => TextTranslator.Translate("MyViewSettings.ErrorWhileSavingPreferences");

        /// <summary>
        /// Gets the success message.
        /// </summary>
        /// <value>
        /// The success message.
        /// </value>
        protected string SuccessMessage => TextTranslator.Translate("MyViewSettings.SuccessMessage");
    }
}