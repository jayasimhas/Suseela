﻿using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Company;
using Informa.Library.Company.User.Entitlement;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Library.Utilities.Extensions;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Attributes;
using Jabberwocky.Glass.Models;
using System.Linq;
using Informa.Library.SalesforceConfiguration;

namespace Informa.Web.ViewModels
{
    [AutowireService(LifetimeScope.PerScope)]
    public class CompanyRegisterMessageViewModel : ICompanyRegisterMessageViewModel
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IUserCompanyContext UserCompanyContext;
        protected readonly IAllowCompanyRegisterUserContext AllowCompanyRegisterUser;
        protected readonly ISitecoreContext SitecoreContext;
        protected readonly ICompanyEntitlementsContext CompanyEntitlements;
        protected readonly ISalesforceConfigurationContext SalesforceConfigurationContext;

        public CompanyRegisterMessageViewModel(
                     ITextTranslator textTranslator,
                     ISiteRootContext siteRootContext,
                     IUserCompanyContext userCompanyContext,
                     IAllowCompanyRegisterUserContext allowCompanyRegisterUser,
                     ISitecoreContext sitecoreContext,
                     ICompanyEntitlementsContext companyEnt,
                     ISalesforceConfigurationContext salesforceConfigurationContext)
        {
            TextTranslator = textTranslator;
            SiteRootContext = siteRootContext;
            UserCompanyContext = userCompanyContext;
            AllowCompanyRegisterUser = allowCompanyRegisterUser;
            SitecoreContext = sitecoreContext;
            CompanyEntitlements = companyEnt;
            SalesforceConfigurationContext = salesforceConfigurationContext;

        }

        public bool IsUserIPEntitledForPublication
        {
            get
            {
                if (CompanyEntitlements == null)
                    return false;

                return CompanyEntitlements.Entitlements.Any(a => a.ProductType.ToLower() == "publication" && a.ProductCode.ToLower() == SiteRootContext.Item.Publication_Code.ToLower());
            }
        }
        //public IList<IEntitlement> IPAddressEntitlements { get; set; }
        public string CompanyName => UserCompanyContext.Company?.Name ?? string.Empty;
        public string Message => (SiteRootContext.Item?.Recognized_IP_Announcment_Text ?? string.Empty).ReplacePatternCaseInsensitive("#Company_Name#", CompanyName);
        public string DismissText => TextTranslator.Translate("Maintenance.MaintenanceDismiss");
        public bool Display => !SalesforceConfigurationContext.IsNewSalesforceEnabled && AllowCompanyRegisterUser.IsAllowed && !(SitecoreContext.GetCurrentItem<IGlassBase>(inferType: true) is IRegistration_Page) && IsUserIPEntitledForPublication;
        public string RegisterLinkText => SiteRootContext.Item?.Register_Link?.Text;
        public string RegisterLinkUrl => (string.IsNullOrEmpty(SiteRootContext?.Item?.Register_Link?.Url))
                     ? string.Empty
                     : $"{SiteRootContext.Item.Register_Link.Url}?returnUrl={HttpContext.Current.Request.Url.AbsolutePath}";
    }
}

