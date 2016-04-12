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
	public partial class SubjectsControl : ArticleDetailsPageUserControl
	{
		public TaxonomyTabController TabController;
		protected bool _isLive;
		public SubjectsControl()
		{
			InitializeComponent();
			TabController = new TaxonomyTabController(uxSubjectsKeywords, uxSubjectsViewTree, uxSubjectsViewSearch,
													  uxSubjectsResults, uxSubjectsResultsTree, uxSubjectsSelected,
													  uxArrowUp, uxArrowDown);
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			List<TaxonomyStruct> subjects =
				SitecoreClient.SearchTaxonomy("").ToList();
			HDirectoryStruct subjectDirectory = SitecoreClient.GetHierarchyByGuid(new Guid(Constants.SUBJECT_GUID));

			TabController.InitializeSitecoreValues(subjects, subjectDirectory);

			_isLive = articleDetails.IsPublished;
			label8.Refresh();
		}

		public override void LinkToMenuItem(MenuSelectorItem menuItem)
		{
			TabController.LinkToMenuItem(menuItem);
		}

		public override MenuSelectorItem GetMenuItem()
		{
			return TabController.MenuItem;
		}

		private void label8_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Live!", new Font("SegoeUI", 18), Brushes.Green, 510, 1);
			}
		}
	}
}
