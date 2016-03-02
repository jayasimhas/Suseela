using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using InformaSitecoreWord.Properties;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.UI.Controllers;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
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
			List<TaxonomyStruct> industries = SitecoreClient.SearchTaxonomy("").ToList();
			HDirectoryStruct industryDirectory = SitecoreClient.GetHierarchyByGuid(new Guid(Constants.INDUSTRY_GUID));

			TabController.InitializeSitecoreValues(industries, industryDirectory);

			_isLive = articleDetails.IsPublished;
			label28.Refresh();
		}

		private void label28_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
