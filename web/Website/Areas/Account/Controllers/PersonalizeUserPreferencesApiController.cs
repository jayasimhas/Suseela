using Informa.Library.Globalization;
using Informa.Library.User.UserPreference;
using Informa.Web.Areas.Account.Models.User.Personalization;
using System.Web.Http;

namespace Informa.Web.Areas.Account.Controllers
{
    public class PersonalizeUserPreferencesApiController : ApiController
    {
        protected readonly IUserPreferenceContext UserPreferenceContext;
        protected readonly ITextTranslator TextTranslator;

        public PersonalizeUserPreferencesApiController(
            IUserPreferenceContext userPreferenceContext,
            ITextTranslator textTranslator)
        {
            UserPreferenceContext = userPreferenceContext;
            TextTranslator = textTranslator;
        }

        [HttpPost]
        public IHttpActionResult Update(PersonalizeUserPreferencesRequest request)
        {
            if (string.IsNullOrEmpty(request.UserPreferences))
            {
                return Ok(new
                {
                    success = false,
                    reasons = new string[] { NoPreferencesSelectedKey }
                });
            }
            var response = UserPreferenceContext.Set(request.UserPreferences);

            return Ok(new
            {
                success = response,
                reasons = new string[] { ErrorwhileSavingPreferencesKey }
            });
        }

        protected string NoPreferencesSelectedKey => TextTranslator.Translate("MyViewSettings.NoPreferencesSelected");
        protected string ErrorwhileSavingPreferencesKey => TextTranslator.Translate("MyViewSettings.ErrorWhileSavingPreferences");
    }
}