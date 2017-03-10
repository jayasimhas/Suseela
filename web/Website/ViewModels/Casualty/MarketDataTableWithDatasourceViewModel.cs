using Glass.Mapper.Sc.Fields;
using Informa.Library.Services.Global;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.View_Templates;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.ViewModels.Casualty
{
    public class MarketDataTableWithDatasourceViewModel : GlassViewModel<IMarket_Data_Component_Parameters>
    {
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        protected readonly IGlobalSitecoreService GlobalService;

        public MarketDataTableWithDatasourceViewModel(IAuthenticatedUserContext authenticatedUserContext, 
            IGlobalSitecoreService globalService,
            IRenderingContextService renderingParametersService)
        {
            AuthenticatedUserContext = authenticatedUserContext;
            GlobalService = globalService;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IMarket_Data_Table_Types>();
        }
        public IMarket_Data_Table_Types RenderingParameters { get; set; }
        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// Page Title
        /// </summary>        
        public string PageTitle => GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString()).Title;
        /// <summary>
        /// Table Title
        /// </summary>
        public string TableTitle => GlassModel?.Table_Title;
        /// <summary>
        /// Table SubTitle
        /// </summary>
        public string TableSubTitle => GlassModel?.Table_SubTitle;
        /// <summary>
        /// Dropdowns feed URL
        /// </summary>
        public string DropdownsFeedUrl => GlassModel?.Dropdowns_Feed_URL;
        /// <summary>
        /// Table data feed URL
        /// </summary>
        public string TableFeedUrl => GlassModel?.Table_Result_Feed_URL;
        /// <summary>
        /// Data Provider
        /// </summary>
        public string DataProvider => GlassModel?.DataProvider;
        /// <summary>
        /// Data Provider Logo
        /// </summary>
        public Image ProviderLogo => GlassModel?.ProviderLogo;
        public string jsonDropdownData => GetjsonDropdownData();
        private string GetjsonDropdownData()
        {
            if (GlassModel != null && !string.IsNullOrEmpty(GlassModel.Dropdowns_Feed_URL))
            {
                var client = new WebClient();
                return client.DownloadString(GlassModel.Dropdowns_Feed_URL);
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
