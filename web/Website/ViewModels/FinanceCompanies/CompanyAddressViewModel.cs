using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;


namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompanyAddressViewModel : GlassViewModel<ICompany_Detail_Page> 
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;

        public CompanyAddressViewModel(ISiteRootContext siteRootContext, ITextTranslator textTranslator)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
        }


        public string CompanyID => GlassModel.CompanyID;
        public string CompanyName => GlassModel.Companyname;
        public string CompanyAddress => GlassModel.Company_Address;
        public string CompanyLogo => GlassModel.Company_Logo?.Src;
        public string CompanyUrl => GlassModel.Company_Url?.Url;
        public string CompanyIntroductionText => GlassModel.Introduction_Text;

       
    }
}