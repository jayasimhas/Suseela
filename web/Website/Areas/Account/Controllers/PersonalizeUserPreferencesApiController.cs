using Informa.Library.Globalization;
using Informa.Library.User.UserPreference;
using Informa.Web.Areas.Account.Models.User.Personalization;
using Informa.Web.ViewModels.MyView;
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
                    reason = NoPreferencesSelectedKey
                });
            }
            var response = UserPreferenceContext.Set(request.UserPreferences);

            return Ok(new
            {
                success = response,
                reason = response ? SuccessMessage : ErrorMessage
            });
        }
        [HttpPost]
        public void UpdateArticleId(PersonalizeUserPreferencesRequest articleData)
        {
            if (!string.IsNullOrEmpty(articleData.UserPreferences))
            {
                MyViewPageViewModel.ArticleId = articleData.UserPreferences;
            }
        }
        protected string NoPreferencesSelectedKey => TextTranslator.Translate("MyViewSettings.NoPreferencesSelected");

        protected string ErrorMessage => TextTranslator.Translate("MyViewSettings.ErrorWhileSavingPreferences");

        protected string SuccessMessage => TextTranslator.Translate("MyViewSettings.SuccessMessage");
    }
}