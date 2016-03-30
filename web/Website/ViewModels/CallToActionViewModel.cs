﻿using Informa.Library.Globalization;
using Informa.Library.Site;
using Jabberwocky.Glass.Autofac.Attributes;
using System.Web;
using Glass.Mapper.Sc.Fields;
using Informa.Library.User.Authentication;
using Informa.Web.ViewModels.PopOuts;

namespace Informa.Web.ViewModels
{
    [AutowireService]
    public class CallToActionViewModel : ICallToActionViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IAuthenticatedUserContext AuthenticatedUserContext;

        public CallToActionViewModel(
            ITextTranslator textTranslator,
            ISignInViewModel signInViewModel,
            IRegisterPopOutViewModel registerPopOutViewModel,
            ISiteRootContext siteRootContext,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            TextTranslator = textTranslator;
            SignInViewModel = signInViewModel;
            RegisterPopOutViewModel = registerPopOutViewModel;
            SiteRootContext = siteRootContext;
            AuthenticatedUserContext = authenticatedUserContext;
        }

        #region Implementation of ICallToActionViewModel

        public ISignInViewModel SignInViewModel { get; }
        public IRegisterPopOutViewModel RegisterPopOutViewModel { get; }
        public string SigninTitle => TextTranslator.Translate("CallToAction.SignIn.Title");
        public string SigninSubtitle => TextTranslator.Translate("CallToAction.SignIn.SubTitle");
        public string RegisterTitle => TextTranslator.Translate("CallToAction.Register.Title");
        public string RegisterSubtitle => TextTranslator.Translate("CallToAction.Register.SubTitle");
        public string SubscribeTitle => TextTranslator.Translate("CallToAction.Subscribe.Title");
        public Link SubscribeLink => SiteRootContext.Item?.Subscribe_Link;
        public Link PurchaseLink => SiteRootContext.Item?.Purchase_Link;
        public bool IsAuthenticated => AuthenticatedUserContext.IsAuthenticated;

        #endregion
    }
}