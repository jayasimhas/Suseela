using System.Collections.Generic;

namespace SitecoreTreeWalker.UI.Controllers
{
    public class SummaryController
    {
        public ExpandFlowPanel IndustriesExpand;
        public SummaryController(ExpandFlowPanel industriesExpand)
        {
            IndustriesExpand = industriesExpand;
        }

        /// <summary>
        /// Set summary data for industries
        /// </summary>
        /// <param name="data">List of selected industries</param>
        public void SetIndustriesData(List<string> data)
        {
            IndustriesExpand.SetData(data);
        }
    }
}
