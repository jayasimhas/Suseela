using System.Collections.Specialized;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Utilities.References;
using Informa.Library.Wrappers;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Library.User.EmailOptIns
{
    public class SavedSearchOptOutViewModel : GlassViewModel<IGeneral_Content_Page>
    {
        private readonly IDependencies _dependencies;

        [AutowireService(IsAggregateService = true)]
        public interface IDependencies
        {
            IHttpContextProvider HttpContextProvider { get; }
            IOptInManager OptInManager { get; }
            ITextTranslator TextTranslator { get; }
        }

        public SavedSearchOptOutViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        private string FailText => _dependencies.TextTranslator.Translate("Account.SavedSearch.OptOutFailed");

        private string _bodyText;
        public string BodyText => _bodyText ?? (_bodyText = GetBodyText());

        private string GetBodyText()
        {
            var query = _dependencies.HttpContextProvider.RequestUrl.Query;
            if(string.IsNullOrEmpty(query)) { return FailText; }

            NameValueCollection queryParsed = HttpUtility.ParseQueryString(query);
            var token = queryParsed[Constants.QueryString.EncryptedToken];
            if (string.IsNullOrEmpty(token)) { return FailText; }

            var optOutResponse = _dependencies.OptInManager.AnnonymousOptOut(token);
            return optOutResponse.BodyText;
        }
    }
}
