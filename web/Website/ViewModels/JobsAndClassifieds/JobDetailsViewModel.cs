using Glass.Mapper.Sc.Fields;
using Informa.Library.Globalization;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.JobsAndClassifieds
{
    public class JobDetailsViewModel:GlassViewModel<IJob_Detail_Page>
    {
        protected readonly ITextTranslator TextTranslator;
        public JobDetailsViewModel(ITextTranslator textTranslator)
        {
            TextTranslator = textTranslator;
        }
        public string JobTitle => GlassModel?.Title;
        public string JobSubTitle => GlassModel?.Sub_Title;
        public string JobDescription => GlassModel?.Body;
        public Image JobLogo => GlassModel?.JobLogo;
        public DateTime PublishedDate => GlassModel?.PublishedDate??Convert.ToDateTime("01/01/1990");
        public string PublishedText => TextTranslator.Translate("JobsAndClassifieds.PublishedText");

    }
}