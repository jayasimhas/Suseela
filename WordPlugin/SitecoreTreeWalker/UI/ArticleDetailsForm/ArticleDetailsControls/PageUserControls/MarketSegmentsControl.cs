using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using PluginModels;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using SitecoreTreeWalker.UI.Controllers;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class MarketSegmentsControl : ArticleDetailsPageUserControl
	{
		public TaxonomyTabController TabController;
		protected bool _isLive;
		public MarketSegmentsControl()
		{
			InitializeComponent();
			TabController = new TaxonomyTabController(uxMarketSegmentsKeywords, uxMarketSegmentsViewTree,
													  uxMarketSegmentsViewSearch, uxMarketSegmentsResults, uxMarketSegmentsResultsTree,
													  uxMarketSegmentsSelected,
													  uxArrowUp, uxArrowDown);
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			List<TaxonomyStruct> marketSegments =
				SitecoreClient.SearchTaxonomy( "").ToList();
			HDirectoryStruct marketSegmentsDirectory =
				SitecoreClient.GetHierarchyByGuid(new Guid(Constants.MARKET_SEGMENT_GUID));
			TabController.InitializeSitecoreValues(marketSegments, marketSegmentsDirectory);

			_isLive = articleDetails.IsPublished;
			label25.Refresh();
		}
		
		public override void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			TabController.LinkToMenuItem(menuItem);
		}

		public override MenuSelectorItem GetMenuItem()
		{
			return TabController.MenuItem;
		}

		private void label25_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(SitecoreTreeWalker.Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
