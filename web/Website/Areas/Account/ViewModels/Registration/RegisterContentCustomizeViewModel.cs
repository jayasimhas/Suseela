using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Informa.Web.Areas.Account.ViewModels.Registration
{
    public class RegisterContentCustomizeViewModel:GlassViewModel<IRegistration_ContentCustomizePage>
    {
        public string Title => GlassModel?.Title;
        public string SubTitle => GlassModel?.Sub_Title;
        public IHtmlString Body => new MvcHtmlString(GlassModel?.Body);
        public IHtmlString HelpLink => new MvcHtmlString(GlassModel?.Help_Link);
        public string NotFollowedErrorText => GlassModel?.Not_Followed_Error;
    }
}