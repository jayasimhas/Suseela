using Glass.Mapper.Sc;
using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Navigation;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Library.User.Newsletter;
using Informa.Library.Utilities.Extensions;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
    public class RegisterUserOptInsViewModel : GlassViewModel<IRegistration_Thank_You_Page>
    {
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly ITextTranslator TextTranslator;
        protected readonly IReturnUrlContext ReturnUrlContext;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IPublicationsNewsletterUserOptInContext PublicationNewsletterUserOptInContext;

        public RegisterUserOptInsViewModel(
            IUserCompanyContext userCompanyContext,
            ITextTranslator textTranslator,
            IReturnUrlContext returnUrlContext,
            IGlobalSitecoreService globalService,
            ISiteRootContext siteRootContext,
            IPublicationsNewsletterUserOptInContext publicationNewsletterUserOptInContext)
        {
            UserCompanyContext = userCompanyContext;
            TextTranslator = textTranslator;
            ReturnUrlContext = returnUrlContext;
            GlobalService = globalService;
            SiteRootContext = siteRootContext;
            PublicationNewsletterUserOptInContext = publicationNewsletterUserOptInContext;

            PublicationNewsletterOptIns = PublicationNewsletterUserOptInContext.OptIns.ToList();
            PublicationNewsletterOptIns.Where(w => w.Publication.Code == SiteRootContext.Item.Publication_Code).FirstOrDefault().OptIn = true;
        }

        public List<IPublicationNewsletterUserOptIn> PublicationNewsletterOptIns { get; set; }// => PublicationNewsletterUserOptInContext.OptIns.ToList();
        public string Title => GlassModel?.Title;
        public string SubTitle => UserCompanyContext.Company == null ? GlassModel?.Sub_Title : GlassModel?.Company_Sub_Title.ReplacePatternCaseInsensitive("#User_Company_Name#", UserCompanyContext.Company.Name);
        public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
        public string SubmitText => TextTranslator.Translate("Registration.OptIn.Submit");
        public string GeneralErrorText => TextTranslator.Translate("Registration.OptIn.GeneralError");
        public string OffersLabelText => TextTranslator.Translate("Registration.OptIn.OffersLabel");
        public string NewslettersLabelText => TextTranslator.Translate("Registration.OptIn.NewslettersLabel");
        public string NewslettersLabelTemplateText => TextTranslator.Translate("Registration.OptIns.NewslettersLabelTemplate");
        public string RegisterReturnUrl
        {
            get
            {
                var url = ReturnUrlContext.Url;

                return string.IsNullOrEmpty(url) ? "/" : url;
            }
        }
    }
}
