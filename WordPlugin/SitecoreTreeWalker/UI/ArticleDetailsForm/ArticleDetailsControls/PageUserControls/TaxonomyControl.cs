using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using InformaSitecoreWord.Properties;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.UI.Controllers;
using PluginModels;
using PluginModels;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	public partial class TaxonomyControl : ArticleDetailsPageUserControl
	{
		public TaxonomyTabController TabController;
		protected bool _isLive;
		public TaxonomyControl()
		{
			InitializeComponent();
			TabController = new TaxonomyTabController(uxGeographyKeywords, uxGeographyViewTree, uxGeographyViewSearch,
													  uxGeographyResults, uxGeographyResultsTree, uxGeographySelected,
													  uxArrowUp, uxArrowDown);
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			List<TaxonomyStruct> taxonomyItems = SitecoreClient.SearchTaxonomy("").ToList();
			HDirectoryStruct taxonomyDirectory = SitecoreClient.GetHierarchyByGuid(new Guid(Constants.TAXONOMY_GUID));

            TabController.InitializeSitecoreValues(taxonomyItems, taxonomyDirectory);

			_isLive = articleDetails.IsPublished;
			label4.Refresh();
		}

		public override void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			TabController.LinkToMenuItem(menuItem);
		}

		public override MenuSelectorItem GetMenuItem()
		{
			return TabController.MenuItem;
		}

		private void label4_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
