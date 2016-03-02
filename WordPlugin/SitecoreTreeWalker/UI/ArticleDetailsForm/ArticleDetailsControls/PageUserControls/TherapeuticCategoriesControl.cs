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
	public partial class TherapeuticCategoriesControl : ArticleDetailsPageUserControl
	{
		public TaxonomyTabController TabController;
		protected bool _isLive;
		public TherapeuticCategoriesControl()
		{
			InitializeComponent();
			TabController = new TaxonomyTabController(uxTherapeuticCategoriesKeywords,
													  uxTherapeuticCategoriesViewTree, uxTherapeuticCategoriesViewSearch,
													  uxTherapeuticCategoriesResults, uxTherapeuticCategoriesResultsTree,
													  uxTherapeuticCategoriesSelected,
													  uxArrowUp, uxArrowDown);
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			List<TaxonomyStruct> therapeuticCategories =
				SitecoreClient.SearchTaxonomy("").ToList();
			HDirectoryStruct therapeuticCategoriesDirectory =
				SitecoreClient.GetHierarchyByGuid(new Guid(Constants.THERAPEUTIC_CATEGORY_GUID));

			TabController.InitializeSitecoreValues(therapeuticCategories, therapeuticCategoriesDirectory);

			_isLive = articleDetails.IsPublished;
			label28.Refresh();
		}

		public override void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			TabController.LinkToMenuItem(menuItem);
		}

		public override MenuSelectorItem GetMenuItem()
		{
			return TabController.MenuItem;
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
