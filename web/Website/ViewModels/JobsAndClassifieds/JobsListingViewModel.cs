using HtmlAgilityPack;
using Informa.Library.Globalization;
using Informa.Library.JobsAndClassifieds;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using Jabberwocky.Glass.Autofac.Mvc.Services;
using mshtml;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Informa.Web.ViewModels.JobsAndClassifieds
{
    public class JobsListingViewModel : GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        public JobsListingViewModel(IGlobalSitecoreService globalService,
            ISiteRootContext siterootContext,
            ITextTranslator textTranslator,
            IRenderingContextService renderingParametersService)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
            JobsListingSettings = renderingParametersService.GetCurrentRenderingParameters<IJobs_Listing_No_of_Jobs_Per_Page>();
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
        public IJobs_Listing_No_of_Jobs_Per_Page JobsListingSettings { get; set; }
        /// <summary>
        /// Method to get All published Job Tiles
        /// </summary>
        /// <returns>List of Jobs</returns>
        public List<IJobTile> GetJobTiles()
        {
            List<IJobTile> jobTiles = new List<IJobTile>();
            string JobShortdesc = string.Empty;
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
                            HTMLDocument htmldoc = new HTMLDocument();
                            IHTMLDocument2 htmldoc2 = (IHTMLDocument2)htmldoc;
                            htmldoc2.write(new object[] { job.Body });
                            JobShortdesc = htmldoc2.body?.outerText;
                            JobShortdesc = Regex.Replace(JobShortdesc, "\r\n", string.Empty);
                            if (JobShortdesc != null && JobShortdesc.Length > 150)
                            {
                                jobTiles.Add(new JobTile { JobTitle = job.Title, JobLogo = job.JobLogo, JobShortDescription = new string(JobShortdesc.Take(150).ToArray()) + "...", JobPublishedDate = job.PublishedDate, JobDetailUrl = job._AbsoluteUrl });
                            }
                            else if (JobShortdesc != null && JobShortdesc.Length <= 150)
                            {
                                jobTiles.Add(new JobTile { JobTitle = job.Title, JobLogo = job.JobLogo, JobShortDescription = JobShortdesc, JobPublishedDate = job.PublishedDate, JobDetailUrl = job._AbsoluteUrl });
                            }
                            htmldoc2.close();
                            htmldoc.close();
                        }
                    }
                }
            }
            return jobTiles.OrderByDescending(n => n.JobPublishedDate).ToList();
        }
    }
}
