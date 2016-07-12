﻿using System.Collections.Generic;
using Glass.Mapper.Sc;
using Glass.Mapper.Sc.Fields;
using Informa.Library.Article.Companies;
using Informa.Library.Globalization;
using Informa.Library.Utilities.References;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Autofac.Attributes;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels.CompaniesAndDeals
{
    public class StratrgicTransactionViewModel : GlassViewModel<IArticle>
    {
        [AutowireService(true)]
        public interface IDependencies
        {
            ISitecoreService SitecoreService { get; set; }
        }

        public StratrgicTransactionViewModel(IDependencies dependencies)
        {
            var strategicTransactionComponent =
                dependencies.SitecoreService.GetItem<IStrategic_Transactions>(Constants.StrategicTransactionsComponent);
            if (strategicTransactionComponent == null) return;
            Logo = strategicTransactionComponent.Logo.Src;
            Body = strategicTransactionComponent.Body;
            SubscribeButtonText = strategicTransactionComponent.Subscribe_Button_Text;
        }

        public string Logo { get; set; }
        public string Body { get; set; }
        public string SubscribeButtonText { get; set; }
        public string SubscribeButtonURL { get; set; }
    }
}
