using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Informa.Library.Services.AccountManagement;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Library.Services.Global;
using Glass.Mapper.Sc;

namespace Informa.Web.ViewModels {
    public class ExecutiveSummaryViewModel : GlassViewModel<I___BasePage> {

        private readonly ITextTranslator TextTranslator;
        private readonly IAccountManagementService AccountService;
        private readonly IItemReferences ItemReferences;
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISitecoreContext SitecoreContext;

        public ExecutiveSummaryViewModel(
            ITextTranslator textTranslator,
            IAccountManagementService accountService,
            IItemReferences itemReferences, IGlobalSitecoreService globalService, ISitecoreContext sitecoreContext)
        {
            TextTranslator = textTranslator;
            AccountService = accountService;
            ItemReferences = itemReferences;
            GlobalService = globalService;
            SitecoreContext = sitecoreContext;
        }

        public string ExecutiveSummary => TextTranslator.Translate("SharedContent.ExecutiveSummary");

        public string summary => Sitecore.Context.Item["Summary"];

        public bool IsArticlePage => GlassModel is IArticle;

		public bool ShowSummary => AccountService.IsUserRestricted(GlassModel) || (AccountService.IsPageRestrictionSet(GlassModel));

       

    }
}