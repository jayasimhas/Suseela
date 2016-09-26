using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.DataTools
{
    public class AMChartViewModel: GlassViewModel<IAMcharts_Dashboard>
    {
        protected readonly ISiteRootContext SiteRootContext;
        protected readonly IGlobalSitecoreService GlobalService;
        public AMChartViewModel(ISiteRootContext siteRootContext, IGlobalSitecoreService globalService)
        {
            SiteRootContext = siteRootContext;
            GlobalService = globalService;
        }

        #region AMcharts parameters
        public string ChartType => GlassModel?.ChartType;
        public string CategoryField => GlassModel?.CategoryField;
        public string ValueField => GlassModel?.ValueField;
        public string Height => GlassModel?.Height;
        public string Width => GlassModel?.Width;
        public string JsonPath => GlassModel?.JsonPath;

        #endregion

    }
}