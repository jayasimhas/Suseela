using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using SitecoreTreeWalker.UI.Controllers;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
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
				SitecoreGetter.SearchTaxonomy(new Guid(Constants.THERAPEUTIC_CATEGORY_GUID), "").ToList();
			HDirectoryStruct therapeuticCategoriesDirectory =
				SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.THERAPEUTIC_CATEGORY_GUID));

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
				e.Graphics.DrawImage(SitecoreTreeWalker.Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
