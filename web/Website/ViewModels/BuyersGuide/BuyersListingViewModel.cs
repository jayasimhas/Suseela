using Informa.Library.Globalization;
using Informa.Library.Services.Global;
using Informa.Library.Site;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Base_Templates;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Pages;
using Informa.Models.Informa.Models.sitecore.templates.User_Defined.Objects;
using Jabberwocky.Glass.Autofac.Mvc.Models;
using System.Collections.Generic;
using mshtml;
using Informa.Library.AgrowBuyers;
using System.Linq;
using System.Text.RegularExpressions;
using Jabberwocky.Glass.Autofac.Mvc.Services;

namespace Informa.Web.ViewModels.BuyersGuide
{
    public class BuyersListingViewModel: GlassViewModel<I___BasePage>
    {
        protected readonly IGlobalSitecoreService GlobalService;
        protected readonly ISiteRootContext SiterootContext;
        protected readonly ITextTranslator TextTranslator;
        
        public BuyersListingViewModel(IGlobalSitecoreService globalService,
            ISiteRootContext siterootContext,
            ITextTranslator textTranslator,
            IRenderingContextService renderingParametersService)
        {
            GlobalService = globalService;
            SiterootContext = siterootContext;
            TextTranslator = textTranslator;
            RenderingParameters = renderingParametersService.GetCurrentRenderingParameters<IAgrow_Buyers_Listing_Display_StylesOptions>();
        }

        public IAgrow_Buyers_Listing_Display_StylesOptions RenderingParameters { get; set; }

        /// <summary>
        /// Job Tiles For Listing
        /// </summary>
        public List<AgrowTile> AgrowTiles => GetAgrowTiles();
        /// <summary>
        /// Listing page Title
        /// </summary>
        public string Title => GlassModel?.Title;
        /// <summary>
        /// Listing Page SubTitle
        /// </summary>
        public string SubTitle => GlassModel?.Sub_Title;

        /// <summary>
        /// Listing Page body
        /// </summary>
        public string Body => GlassModel?.Body;

        /// <summary>
        /// Get agrow buyer's details
        /// </summary>
        /// <returns>AgrowTile</returns>
        public List<AgrowTile> GetAgrowTiles()
        {
            List<AgrowTile> agrowTiles = new List<AgrowTile>();
            string JobShortdesc = string.Empty;
            var currentItem = GlobalService.GetItem<IGeneral_Content_Page>(Sitecore.Context.Item.ID.ToString());

            if (currentItem != null)
            {
                var jobsRootItem = currentItem._ChildrenWithInferType.OfType<IPage_Assets>().FirstOrDefault();

                if (jobsRootItem != null)
                {
                    var agrowItems = jobsRootItem._ChildrenWithInferType.OfType<IBuyers_Detail_Page>();
                    if (agrowItems != null && agrowItems.Count() > 0)
                    {

                        foreach (var buyersList in agrowItems)
                        {
                            HTMLDocument htmldoc = new HTMLDocument();
                            IHTMLDocument2 htmldoc2 = (IHTMLDocument2)htmldoc;
                            htmldoc2.write(new object[] { buyersList.Body });
                            JobShortdesc = htmldoc2.body?.outerText;
                            if (JobShortdesc != null)
                            {
                                JobShortdesc = Regex.Replace(JobShortdesc, "\r\n", string.Empty);
                            }

                            if (JobShortdesc != null && JobShortdesc.Length > 150)
                            {
                                agrowTiles.Add(new AgrowTile { AgrowTitle = buyersList.Title, AgrowBanner = buyersList.Banner,
                                    AgrowLogo = buyersList.AgrowLogo, AgrowShortDescription = new string(JobShortdesc.Take(150).ToArray()) + "...",
                                    AgrowDetailUrl = !string.IsNullOrEmpty(buyersList?.PageURL?.Url) ? buyersList?.PageURL?.Url : buyersList?._AbsoluteUrl,
                                    AgrowDetailText = !string.IsNullOrEmpty(buyersList?.PageURL?.Url) ? buyersList?.PageURL?.Text : buyersList?.Title
                                });
                            }
                            else if (JobShortdesc != null && JobShortdesc.Length <= 150)
                            {
                                agrowTiles.Add(new AgrowTile { AgrowTitle = buyersList.Title, AgrowBanner = buyersList.Banner,AgrowLogo = buyersList.AgrowLogo,
                                    AgrowShortDescription = JobShortdesc,AgrowDetailUrl = !string.IsNullOrEmpty(buyersList?.PageURL?.Url) ? buyersList?.PageURL?.Url : buyersList?._AbsoluteUrl,
                                    AgrowDetailText = !string.IsNullOrEmpty(buyersList?.PageURL?.Url) ? buyersList?.PageURL?.Text : buyersList?.Title
                                });
                            }
                            htmldoc2.close();
                            htmldoc.close();
                        }
                    }
                }
            }
            return agrowTiles;
        }
    }
}