using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Library.Newsletter;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class SavedArticlesViewModel : GlassViewModel<I___BasePage>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly IManageSavedContent QuerySavedContent;
        public readonly ISignInViewModel SignInViewModel;

        public SavedArticlesViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IManageSavedContent querySavedContent,
            ISignInViewModel signInViewModel)
        {
            TextTranslator = translator;
            UserContext = userContext;
            QuerySavedContent = querySavedContent;
            SignInViewModel = signInViewModel;

            var result = QuerySavedContent.QueryItems(UserContext.User);
            SavedContent = (result.Success)
                ? result.SavedContentItems
                : Enumerable.Empty<ISavedContent>();
        }

        public IEnumerable<ISavedContent> SavedContent;

        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;
        public string GeneralErrorText => TextTranslator.Translate("SavedContent.GeneralError");
        public string NullUserText => TextTranslator.Translate("SavedContent.NullUserError");
        public string RequestFailedText => TextTranslator.Translate("SavedContent.RequestFailedError");
        public string TitleText => TextTranslator.Translate("SavedContent.TitleText");
        public string DateText => TextTranslator.Translate("SavedContent.DateText");
        public string RemoveText => TextTranslator.Translate("SavedContent.RemoveText");
        public string ItemRemovedMessage => TextTranslator.Translate("SavedContent.ItemRemovedMessage");
    }
}