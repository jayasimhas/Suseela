using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using Informa.Library.Globalization;
using Informa.Library.Services.RobotsText;
using Informa.Library.Services.Sitemap;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using WebApi.OutputCache.Core.Cache;

namespace Informa.Web.ViewModels
{
    public class RobotsTextViewModel : GlassViewModel<IRobots_Text_Page>
    {
        protected readonly IRobotsTextService RobotService;
        
        public RobotsTextViewModel(IRobotsTextService robotService)
        {
            RobotService = robotService;
        }

        public string GetText()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("User-agent: *");
            sb.AppendLine(RobotService.GetDisallowedGeneralContentUrls());
            if (!string.IsNullOrEmpty(GlassModel.Text))
                sb.AppendLine(GlassModel.Text);

            return sb.ToString();
        }
    }
}

