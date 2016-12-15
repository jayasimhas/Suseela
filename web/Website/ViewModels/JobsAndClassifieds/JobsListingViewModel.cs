using HtmlAgilityPack;
using Informa.Library.Globalization;
using Informa.Library.JobsAndClassifieds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
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
        /// <summary>
        /// Job Tiles For Listing
        /// </summary>
        public List<IJobTile> JobTiles => GetJobTiles();
        /// <summary>
        /// Listing page Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Listing Page SubTitle
        /// </summary>
        public string SubTitle => GlassModel?.Sub_Title;
        /// <summary>
        /// Published Text
        /// </summary>
        public string PublishedText => TextTranslator.Translate("JobsAndClassifieds.PublishedText");
        /// <summary>
        /// Next Page Link Text
        /// </summary>
        public string NextPageText => TextTranslator.Translate("JobsAndClassifieds.NextPageText");
        /// <summary>
        /// Prev Page Link Text
        /// </summary>
        public string PrevPageText => TextTranslator.Translate("JobsAndClassifieds.PrevPageText");
        /// <summary>
        /// No of Jobs to be displayed Per Page
        /// </summary>
        public string NoOfJobsPerPage => !string.IsNullOrEmpty(TextTranslator.Translate("JobsAndClassifieds.NoOfJobsPerPage")) ? TextTranslator.Translate("JobsAndClassifieds.NoOfJobsPerPage") : "20";
        /// <summary>
        /// Method to get All published Job Tiles
        /// </summary>
        /// <returns>List of Jobs</returns>
        public List<IJobTile> GetJobTiles()
        {
            List<IJobTile> jobTiles = new List<IJobTile>();
            var currentItem = GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString());

            if (currentItem != null)
            {
                var jobsRootItem = currentItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();
                if (jobsRootItem != null)
                {
                    var jobsItem = jobsRootItem._ChildrenWithInferType.OfType<IJob_Detail_Page>();
                    if (jobsItem != null && jobsItem.Count() > 0)
                    {
                        foreach (var job in jobsItem)
                        {
                            HtmlDocument doc = new HtmlDocument();
                            doc.LoadHtml(job.Body);
                            string JobShortdesc = doc.DocumentNode.InnerText;
                            jobTiles.Add(new JobTile { JobTitle = job.Title, JobLogo = job.JobLogo, JobShortDescription = new string(JobShortdesc.Take(150).ToArray()), JobPublishedDate = job.PublishedDate, JobDetailUrl = job._AbsoluteUrl });
                        }
                    }
                }
            }
            return jobTiles.OrderByDescending(n => n.JobPublishedDate).ToList();
        }
    }
}
