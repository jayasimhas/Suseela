using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SavedArticlesViewModel : GlassViewModel<I___BasePage>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly IManageSavedDocuments QuerySavedDocuments;
        public readonly ISignInViewModel SignInViewModel;
        public readonly ISitecoreService SitecoreService;

        public SavedArticlesViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IManageSavedDocuments querySavedDocuments,
            ISignInViewModel signInViewModel,
            ISitecoreService sitecoreService)
        {
            TextTranslator = translator;
            UserContext = userContext;
            QuerySavedDocuments = querySavedDocuments;
            SignInViewModel = signInViewModel;
            SitecoreService = sitecoreService;

            var result = QuerySavedDocuments.QueryItems(UserContext.User);
            SavedDocuments = (result.Success)
                ? result.SavedDocuments
                : Enumerable.Empty<ISavedDocument>();
        }

        private IArticle GetArticle(string ID)
        {
            Guid result;
            if (!Guid.TryParse(ID, out result))
                return null;

            return SitecoreService.GetItem<IArticle>(result);
        }

        public string GetUrl(string ID)
        {
            var article = GetArticle(ID);
            if (article == null)
                return string.Empty;

            return article._Url;
        }

        public string GetActualPublishDate(string ID, string dateFormat)
        {
            var article = GetArticle(ID);
            if (article == null)
                return DateTime.Now.ToString(dateFormat);

            return article.Actual_Publish_Date.ToString(dateFormat);
        }

        public IEnumerable<ISavedDocument> SavedDocuments;

        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;
        public string GeneralErrorText => TextTranslator.Translate("SavedDocuments.GeneralError");
        public string NullUserText => TextTranslator.Translate("SavedDocuments.NullUserError");
        public string RequestFailedText => TextTranslator.Translate("SavedDocuments.RequestFailedError");
        public string TitleText => TextTranslator.Translate("SavedDocuments.TitleText");
        public string DateText => TextTranslator.Translate("SavedDocuments.DateText");
        public string RemoveText => TextTranslator.Translate("SavedDocuments.RemoveText");
        public string ItemRemovedMessage => TextTranslator.Translate("SavedDocuments.ItemRemovedMessage");
        public string BadIDText => TextTranslator.Translate("SavedDocuments.BadIDText"); 
    }
}