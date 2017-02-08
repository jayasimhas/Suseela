using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Glass.Mapper.Sc;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class CompanyAddressViewModel : GlassViewModel<ICompany_Detail_Page>
    {
        
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISitecoreContext Context;
        public CompanyAddressViewModel(
            ITextTranslator textTranslator,
            ISitecoreContext context)
        {
            TextTranslator = textTranslator;
            Context = context;
        }
        string id = HttpContext.Current.Request.QueryString["Id"];       
        public string CompanyName => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Companyname : GlassModel?.Companyname;
        public string CompanyAddress => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Company_Address : GlassModel?.Company_Address;
        public string CompanyLogo => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Company_Logo?.Src : GlassModel?.Company_Logo?.Src;
        public string CompanyUrl => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Company_Url?.Url : GlassModel?.Company_Url?.Url;
        public string CompanyIntroductionText => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Introduction_Text : GlassModel?.Introduction_Text;        
    }
}