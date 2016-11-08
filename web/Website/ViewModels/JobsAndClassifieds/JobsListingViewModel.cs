using Informa.Library.Globalization;
using Informa.Library.JobsAndClassifieds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects.Jobs;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Informa.Web.ViewModels.JobsAndClassifieds
{
    public class JobsListingViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        public JobsListingViewModel(IGlobalSitecoreService globalService, 
            ISiteRootContext siterootContext, 
            ITextTranslator textTranslator)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
        }

        public List<IJobTile> JobTiles => GetJobTiles();
        public string Title => GlassModel?.Title;
        public string SubTitle => GlassModel?.Sub_Title;
        public string PublishedText => TextTranslator.Translate("JobsAndClassifieds.PublishedText");
        public string NextPageText => TextTranslator.Translate("JobsAndClassifieds.NextPageText");
        public string PrevPageText => TextTranslator.Translate("JobsAndClassifieds.PrevPageText");
        public string NoOfJobsPerPage => !string.IsNullOrEmpty(TextTranslator.Translate("JobsAndClassifieds.NoOfJobsPerPage"))? TextTranslator.Translate("JobsAndClassifieds.NoOfJobsPerPage"):"20";

        public List<IJobTile> GetJobTiles()
        {
            List<IJobTile> jobTiles = new List<IJobTile>();
            var homeItem = GlobalService.GetItem<IHome_Page>(SiterootContext.Item._Id.ToString()).
                _ChildrenWithInferType.OfType<IHome_Page>().FirstOrDefault();
            var jobsRootItem = homeItem._ChildrenWithInferType.OfType<IJobs_Root>().FirstOrDefault();
            if (jobsRootItem != null)
            {
                var jobsItem = jobsRootItem._ChildrenWithInferType.OfType<IJob_Detail_Page>();
                if (jobsItem != null && jobsItem.Count() > 0)
                {
                    foreach (var job in jobsItem)
                    {
                        jobTiles.Add(new JobTile { JobTitle = job.Title, JobLogo = job.JobLogo, JobShortDescription = new string(job.Body.Take(150).ToArray()), JobPublishedDate=job.PublishedDate, JobDetailUrl = job._AbsoluteUrl });
                    }
                }
            }
            return jobTiles.OrderByDescending(n=>n.JobPublishedDate).ToList();
        }
    }
}