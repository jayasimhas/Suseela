using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages.Company;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using log4net;
using Informa.Library.Services.Global;
using Sitecore.Data.Items;
using System.Net;
using Newtonsoft.Json;
using Glass.Mapper.Sc;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;

namespace Informa.Web.ViewModels.FinanceCompanies
{
    public class MergersAndAquisitionsViewModel : GlassViewModel<IGeneral_Content_Page>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly ITextTranslator TextTranslator;
        private readonly ILog _logger;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISitecoreContext SitecoreContext;

        public MergersAndAquisitionsViewModel(ISiteRootContext siteRootContext,
        ITextTranslator textTranslator, ILog logger, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext)
        {
            SiteRootContext = siteRootContext;
            TextTranslator = textTranslator;
            _logger = logger;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
        }


    }
}