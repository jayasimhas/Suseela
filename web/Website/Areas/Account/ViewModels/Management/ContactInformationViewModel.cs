using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using Glass.Mapper.Sc;
using Informa.Library.Company;
using Informa.Library.Globalization;
using Informa.Library.Salesforce.User.Profile;
using Informa.Library.User.Authentication;
using Informa.Library.User.Profile;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.Common;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Account;
using Informa.Web.ViewModels;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Sitecore.Data.Items;

namespace Informa.Web.Areas.Account.ViewModels.Management
{
    public class ContactInformationViewModel : GlassViewModel<IContact_Information_Page>
    {
        public readonly ITextTranslator TextTranslator;
        public readonly IAuthenticatedUserContext UserContext;
        public readonly IManageAccountInfo AccountInfo;
        public readonly ISignInViewModel SignInViewModel;
        public readonly ICompanyContext CompanyContext;
        public readonly ISalesforceFindUserProfile FindUserProfile;
        private readonly IItemReferences ItemReferences;
        public readonly ISitecoreContext SitecoreContext;

        public ContactInformationViewModel(
            ITextTranslator translator,
            IAuthenticatedUserContext userContext,
            IManageAccountInfo accountInfo,
            ISignInViewModel signInViewModel,
            ICompanyContext companyContext,
            ISalesforceFindUserProfile findUserProfile,
            IItemReferences itemReferences,
            ISitecoreContext sitecoreContext)
        {
            TextTranslator = translator;
            UserContext = userContext;
            AccountInfo = accountInfo;
            SignInViewModel = signInViewModel;
            CompanyContext = companyContext;
            FindUserProfile = findUserProfile;
            SfUserProfile = FindUserProfile.Find(UserContext.User.Username);
            ItemReferences = itemReferences;
            SitecoreContext = sitecoreContext;
        }
        
        public bool IsAuthenticated => UserContext.IsAuthenticated;
        public string Title => GlassModel?.Title;
        public string GeneralErrorText => TextTranslator.Translate("ContactInfo.GeneralError");
        public ISalesforceUserProfile SfUserProfile;

        #region Password

        public string UserNameTitleText => TextTranslator.Translate("ContactInfo.UserNameTitle");
        public string UpdateHelpText => GlassModel?.Update_Email_Help_Text ?? string.Empty;
        public string PasswordTitleText => TextTranslator.Translate("ContactInfo.PasswordTitle");
        public string CurrentPasswordLabelText => TextTranslator.Translate("ContactInfo.CurrentPasswordLabel");
        public string NewPasswordLabelText => TextTranslator.Translate("ContactInfo.NewPasswordLabel");
        public string NewPasswordConfirmLabelText => TextTranslator.Translate("ContactInfo.NewPasswordConfirmLabel");
        public string UpdatePasswordText => TextTranslator.Translate("ContactInfo.UpdatePassword");
        public string CurrentPasswordPlaceholderText => TextTranslator.Translate("ContactInfo.CurrentPasswordPlaceholder");
        public string NewPasswordPlaceholderText => TextTranslator.Translate("ContactInfo.NewPasswordPlaceholder");
        public string NewPasswordConfirmPlaceholderText => TextTranslator.Translate("ContactInfo.NewPasswordConfirmPlaceholder");
        public string PasswordUpdatedText => TextTranslator.Translate("ContactInfo.PasswordUpdated");

        #endregion Password

        #region Contact Info

        public string NameTitleText => TextTranslator.Translate("ContactInfo.NameTitle");
        public string SalutationLabelText => TextTranslator.Translate("ContactInfo.SalutationLabel");
        public string FirstNameLabelText => TextTranslator.Translate("ContactInfo.FirstNameLabel");
        public string FirstNamePlaceholderText => TextTranslator.Translate("ContactInfo.FirstNamePlaceholder");
        public string MiddleInitialLabelText => TextTranslator.Translate("ContactInfo.MiddleInitialLabel");
        public string MiddleInitialPlaceholderText => TextTranslator.Translate("ContactInfo.MiddleInitialPlaceholder");
        public string LastNameLabelText => TextTranslator.Translate("ContactInfo.LastNameLabel");
        public string LastNamePlaceholderText => TextTranslator.Translate("ContactInfo.LastNamePlaceholder");
        public string NameSuffixLabelText => TextTranslator.Translate("ContactInfo.NameSuffixLabel");
        public string CompanyTitleText => TextTranslator.Translate("ContactInfo.CompanyTitle");
        public string CompanyLabelText => TextTranslator.Translate("ContactInfo.CompanyLabel");
        public string CompanyPlaceholderText => TextTranslator.Translate("ContactInfo.CompanyPlaceholder");
        public string AssociatedCompanyLabelText => TextTranslator.Translate("ContactInfo.AssociatedCompanyLabel");
        public string AssociatedCompany => CompanyContext?.Company?.Name ?? string.Empty;
        public string JobTitleLabelText => TextTranslator.Translate("ContactInfo.JobTitleLabel");
        public string JobTitlePlaceholderText => TextTranslator.Translate("ContactInfo.JobTitlePlaceholder");
        public string JobFunctionLabelText => TextTranslator.Translate("ContactInfo.JobFunctionLabel");
        public string JobIndustryLabelText => TextTranslator.Translate("ContactInfo.JobIndustryLabel");
        public string PhoneTitleText => TextTranslator.Translate("ContactInfo.PhoneTitle");
        public string PhoneTypeLabelText => TextTranslator.Translate("ContactInfo.PhoneTypeLabel");
        public string CountryCodeLabelText => TextTranslator.Translate("ContactInfo.CountryCodeLabel");
        public string CountryCodePlaceholderText => TextTranslator.Translate("ContactInfo.CountryCodePlaceholder");
        public string PhoneLabelText => TextTranslator.Translate("ContactInfo.PhoneLabel");
        public string PhonePlaceholderText => TextTranslator.Translate("ContactInfo.PhonePlaceholder");
        public string PhoneExtensionLabelText => TextTranslator.Translate("ContactInfo.PhoneExtensionLabel");
        public string PhoneExtensionPlaceholderText => TextTranslator.Translate("ContactInfo.PhoneExtensionPlaceholder");
        public string FaxLabelText => TextTranslator.Translate("ContactInfo.FaxLabel");
        public string FaxPlaceholderText => TextTranslator.Translate("ContactInfo.FaxPlaceholder");
        public string BillingTitleText => TextTranslator.Translate("ContactInfo.BillingTitle");
        public string BillCountryLabelText => TextTranslator.Translate("ContactInfo.BillCountryLabel");
        public string BillAddress1LabelText => TextTranslator.Translate("ContactInfo.BillAddress1Label");
        public string BillAddress1PlaceholderText => TextTranslator.Translate("ContactInfo.BillAddress1Placeholder");
        public string BillAddress2LabelText => TextTranslator.Translate("ContactInfo.BillAddress2Label");
        public string BillAddress2PlaceholderText => TextTranslator.Translate("ContactInfo.BillAddress2Placeholder");
        public string BillCityLabelText => TextTranslator.Translate("ContactInfo.BillCityLabel");
        public string BillCityPlaceholderText => TextTranslator.Translate("ContactInfo.BillCityPlaceholder");
        public string BillPostalCodeLabelText => TextTranslator.Translate("ContactInfo.BillPostalCodeLabel");
        public string BillPostalCodePlaceholderText => TextTranslator.Translate("ContactInfo.BillPostalCodePlaceholder");
        public string BillStateLabelText => TextTranslator.Translate("ContactInfo.BillStateLabel");
        public string BillStatePlaceholderText => TextTranslator.Translate("ContactInfo.BillStatePlaceholder");
        public string SameAsBillingLabel => TextTranslator.Translate("ContactInfo.SameAsBillingLabel");
        public string ShippingTitleText => TextTranslator.Translate("ContactInfo.ShippingTitle");
        public string ShipCountryLabelText => TextTranslator.Translate("ContactInfo.ShipCountryLabel");
        public string ShipAddress1LabelText => TextTranslator.Translate("ContactInfo.ShipAddress1Label");
        public string ShipAddress1PlaceholderText => TextTranslator.Translate("ContactInfo.ShipAddress1Placeholder");
        public string ShipAddress2LabelText => TextTranslator.Translate("ContactInfo.ShipAddress2Label");
        public string ShipAddress2PlaceholderText => TextTranslator.Translate("ContactInfo.ShipAddress2Placeholder");
        public string ShipCityLabelText => TextTranslator.Translate("ContactInfo.ShipCityLabel");
        public string ShipCityPlaceholderText => TextTranslator.Translate("ContactInfo.ShipCityPlaceholder");
        public string ShipPostalCodeLabelText => TextTranslator.Translate("ContactInfo.ShipPostalCodeLabel");
        public string ShipPostalCodePlaceholderText => TextTranslator.Translate("ContactInfo.ShipPostalCodePlaceholder");
        public string ShipStateLabelText => TextTranslator.Translate("ContactInfo.ShipStateLabel");
        public string ShipStatePlaceholderText => TextTranslator.Translate("ContactInfo.ShipStatePlaceholder");
        public string UpdateContactInfoText => TextTranslator.Translate("ContactInfo.UpdateContactInfo");
        public string AccountUpdatedText => TextTranslator.Translate("ContactInfo.AccountUpdated");

        #endregion Contact Info

        #region Drop Down Lists 

        public IEnumerable<ListItem> Salutations => GetListing(ItemReferences.AccountSalutations);
        public IEnumerable<ListItem> Suffixes => GetListing(ItemReferences.AccountNameSuffixes);
        public IEnumerable<ListItem> JobFunctions => GetListing(ItemReferences.AccountJobFunctions);
        public IEnumerable<ListItem> JobIndustries => GetListing(ItemReferences.AccountJobIndustries);
        public IEnumerable<ListItem> PhoneTypes => GetListing(ItemReferences.AccountPhoneTypes);
        public IEnumerable<ListItem> Countries => GetListing(ItemReferences.AccountCountries);

        private IEnumerable<ListItem> GetListing(Guid g)
        {
            var item = SitecoreContext.GetItem<Item>(g);
            if (item == null)
                return Enumerable.Empty<ListItem>();

            return item.GetChildren().Select(a => SitecoreContext.GetItem<ITaxonomy_Item>(a.ID.Guid)).Select(b => new ListItem(b.Item_Name, b.Item_Name));
        }

        #endregion Drop Down Lists
    }
}