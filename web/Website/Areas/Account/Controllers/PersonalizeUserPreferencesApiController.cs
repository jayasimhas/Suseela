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

            return Ok(new
            {
                success = response,
                reason = response ? SuccessMessage : ErrorMessage
            });
        }

        /// <summary>
        /// Updates the article identifier.
        /// </summary>
        /// <param name="articleData">The article data.</param>
        [HttpPost]
        public void UpdateArticleId(PersonalizeUserPreferencesRequest articleData)
        {
            if (!string.IsNullOrEmpty(articleData.UserPreferences))
            {
                MyViewPageViewModel.ArticleId = articleData.UserPreferences;
            }
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