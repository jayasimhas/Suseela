using System.Collections.Specialized;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
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
            ISiteRootContext SiteRootContext { get; }
        }

        public SavedSearchOptOutViewModel(IDependencies dependencies)
        {
            _dependencies = dependencies;
        }

        public void Init()
        {
            PopulatePropertiesFromResponse(OptOut());
            CurrentPublicationName = _dependencies.SiteRootContext.Item?.Publication_Name;
        }
        
        public string HeaderText { get; private set; }
        public string BodyText { get; private set; }
        public string SavedSearchName { get; private set; }
        public bool IsOptOutSuccessful { get; private set; }
        public string CurrentPublicationName { get; private set; }

        private void PopulatePropertiesFromResponse(OptInResponseModel response)
        {
            IsOptOutSuccessful = response.IsSuccessful;
            if (IsOptOutSuccessful)
            {
                HeaderText = _dependencies.TextTranslator.Translate("Account.SavedSearches.OptOutHeader");
                SavedSearchName = response.BodyText;
                BodyText = GlassModel?.Body.Replace(SavedSearchNameToken, SavedSearchName);
            }
            else
            {
                HeaderText = _dependencies.TextTranslator.Translate("Account.SavedSearches.OptOutFailed");
                BodyText = string.Empty;
            }
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
