using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Library.User.Authentication;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class GoToPortViewModel : GlassViewModel<IGo_To_Port_Component>
    {
        protected readonly ISiteRootContext SiterootContext;
        private readonly IAuthenticatedUserContext AuthenticatedUserContext;
        public GoToPortViewModel(ISiteRootContext siterootContext,
            IAuthenticatedUserContext authenticatedUserContext)
        {
            SiterootContext = siterootContext;
            AuthenticatedUserContext = authenticatedUserContext;
        }

        public bool IsUserAuthenticated => AuthenticatedUserContext.IsAuthenticated;
        /// <summary>
        /// HomePage URL
        /// </summary>
        public string homePageUrl => SiterootContext?.Item._Url;
        /// <summary>
        /// Logo
        /// </summary>
        public Image Logo => GlassModel?.Logo;
        /// <summary>
        /// Title when results available for the selected area
        /// </summary>
        public string TitleWithResult => GlassModel?.Title_With_Result;
        /// <summary>
        /// Title on initial page load
        /// </summary>
        public string TitleWithoutResult => GlassModel?.Title_Without_Result;
        /// <summary>
        /// Description
        /// </summary>
        public string Description => GlassModel?.Description;
    }
}