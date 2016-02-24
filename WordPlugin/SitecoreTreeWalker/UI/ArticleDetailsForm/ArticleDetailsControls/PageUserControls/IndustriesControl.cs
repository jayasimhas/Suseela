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
	public partial class IndustriesControl : ArticleDetailsPageUserControl
	{
		public TaxonomyTabController TabController;
		protected bool _isLive;
		public IndustriesControl()
		{
			InitializeComponent();
			TabController = new TaxonomyTabController(uxIndustriesKeywords, uxIndustriesViewTree, uxIndustriesViewSearch,
				uxIndustriesResults, uxIndustriesResultsTree, uxIndustriesSelected,
				uxArrowUp, uxArrowDown);
		}

		public override void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			TabController.LinkToMenuItem(menuItem);
		}

		public override MenuSelectorItem GetMenuItem()
		{
			return TabController.MenuItem;
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			List<TaxonomyStruct> industries = SitecoreGetter.SearchTaxonomy("").ToList();
			HDirectoryStruct industryDirectory = SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.INDUSTRY_GUID));

			TabController.InitializeSitecoreValues(industries, industryDirectory);

			_isLive = articleDetails.IsPublished;
			label28.Refresh();
		}

		private void label28_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(SitecoreTreeWalker.Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
