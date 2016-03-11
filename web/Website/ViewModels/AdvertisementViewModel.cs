﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Glass.Mapper.Sc;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Components;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Configuration;
using Jabberwocky.Glass.Autofac.Mvc.Models;

namespace Informa.Web.ViewModels
{
    public class AdvertisementViewModel : GlassViewModel<IAdvertisement>
    {
        protected readonly ITextTranslator TextTranslator;

        public AdvertisementViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }

        public string AdvertisementText => TextTranslator.Translate("Ads.Advertisement");
    }
}