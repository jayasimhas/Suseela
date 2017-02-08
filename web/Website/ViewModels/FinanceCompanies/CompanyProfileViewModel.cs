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
    public class CompanyProfileViewModel: GlassViewModel<ICompany_Detail_Page> 
    {
        protected readonly ITextTranslator TextTranslator;
        protected readonly ISitecoreContext Context;
        public CompanyProfileViewModel(ISitecoreContext siteRootContext, ITextTranslator textTranslator)
        {
            Context = siteRootContext;
            TextTranslator = textTranslator;
        }
        string id = HttpContext.Current.Request.QueryString["Id"];        
        public string ProfileTitle => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Profile_Title : GlassModel.Profile_Title;
        public string ProfileText => !string.IsNullOrEmpty(id) ? Context.GetItem<ICompany_Detail_Page>(id)?.Profile_Text : GlassModel.Profile_Text;
      
    }
}