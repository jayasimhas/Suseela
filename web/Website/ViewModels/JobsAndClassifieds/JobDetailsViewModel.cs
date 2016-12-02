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
        /// <summary>
        /// Job Title
        /// </summary>
        public string JobTitle => GlassModel?.Title;
        /// <summary>
        /// Job SubTitle
        /// </summary>
        public string JobSubTitle => GlassModel?.Sub_Title;
        /// <summary>
        /// Job Description
        /// </summary>
        public string JobDescription => GlassModel?.Body;
        /// <summary>
        /// Job Logo
        /// </summary>
        public Image JobLogo => GlassModel?.JobLogo;
        /// <summary>
        /// Published Date
        /// </summary>
        public DateTime PublishedDate => GlassModel?.PublishedDate??Convert.ToDateTime("01/01/1990");
        /// <summary>
        /// Published Text
        /// </summary>
        public string PublishedText => TextTranslator.Translate("JobsAndClassifieds.PublishedText");

    }
}