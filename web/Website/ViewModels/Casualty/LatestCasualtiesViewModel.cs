﻿using Glass.Mapper.Sc.Fields;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.Casualty
{
    public class LatestCasualtiesViewModel : GlassViewModel<ILatest_Casualties_Component>
    {
        protected readonly ISiteRootContext SiterootContext;
        public LatestCasualtiesViewModel(ISiteRootContext siterootContext)
        {
            SiterootContext = siterootContext;
        }
        public string homePageUrl => SiterootContext?.Item._Url;
        public Image Logo => GlassModel?.Logo;
        public string Title => GlassModel?.Title;
        public string AdditionalInformation => GlassModel?.Additional_Information;
        public string FeedUrl => GlassModel?.External_Feed_Url;
    }
}