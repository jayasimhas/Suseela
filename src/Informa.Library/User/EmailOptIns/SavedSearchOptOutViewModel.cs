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
        private const string SavedSearchNameToken = "[SavedSearchName]";

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

        private bool _optOutHasRun;
        private string _headerText;

        public string HeaderText
        {
            get
            {
                if(!_optOutHasRun) { PopulateTextProperties(); }
                return _headerText;
            }
        }

        private string _bodyText;
        public string BodyText
        {
            get
            {
                if(!_optOutHasRun) { PopulateTextProperties(); }
                return _bodyText;
            }
        }

        private void PopulateTextProperties()
        {
            var response = OptOut();
            if (response.IsSuccessful)
            {
                _headerText = _dependencies.TextTranslator.Translate("Account.SavedSearches.OptOutHeader");
                _bodyText = GlassModel?.Body.Replace(SavedSearchNameToken, response.BodyText);
            }
            else
            {
                _headerText = _dependencies.TextTranslator.Translate("Account.SavedSearches.OptOutFailed");
                _bodyText = string.Empty;
            }
            _optOutHasRun = true;
        }

        private OptInResponseModel OptOut()
        {
            var query = _dependencies.HttpContextProvider.RequestUrl.Query;
            if(string.IsNullOrEmpty(query)) { return new OptInResponseModel {IsSuccessful = false}; }

            NameValueCollection queryParsed = HttpUtility.ParseQueryString(query);
            var token = queryParsed[Constants.QueryString.EncryptedToken];
            if (string.IsNullOrEmpty(token)) { return new OptInResponseModel { IsSuccessful = false }; }

            return _dependencies.OptInManager.AnnonymousOptOut(token);
        }
    }
}
