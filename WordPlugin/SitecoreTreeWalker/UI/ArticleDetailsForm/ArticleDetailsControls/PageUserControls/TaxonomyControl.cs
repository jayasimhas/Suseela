using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using InformaSitecoreWord.Properties;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.UI.Controllers;
using PluginModels;
using InformaSitecoreWord.Util;

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


            //Uncomment Start, use this code only when environment global/Taxonomy require at content root level as well
            /*List<TaxonomyStruct> taxonomyItems = SitecoreClient.SearchTaxonomy("").ToList();
            HDirectoryStruct taxonomyDirectory = SitecoreClient.GetHierarchyByGuid(Guid.Empty);//modified,21Sep16
            */
            //Uncomment End

            //Old Code not needed now
            //HDirectoryStruct taxonomyDirectory = SitecoreClient.GetHierarchyByGuid(new Guid(Constants.TAXONOMY_GUID));//commented,21Sep16

            //Uncomment Start, use this code only when environment global/Taxonomy require at content root level as well
            /*if (taxonomyItems != null && taxonomyDirectory != default(HDirectoryStruct) )
            {
                TabController.InitializeSitecoreValues(taxonomyItems, taxonomyDirectory);
            }*/
            //Uncomment End

            _isLive = articleDetails.IsPublished;
			label4.Refresh();
		}

        //added,21Sep16
        public void PopulateTaxonomyItems()
        {
            List<TaxonomyStruct> taxonomyItems = SitecoreClient.SearchTaxonomy("", PluginSingletonVerticalRoot.Instance.CurrentVertical.TaxonomyItem.ID).ToList();
            HDirectoryStruct taxonomyDirectory = SitecoreClient.GetHierarchyByGuid(PluginSingletonVerticalRoot.Instance.CurrentVertical.TaxonomyItem.ID);

            TabController.InitializeSitecoreValues(taxonomyItems, taxonomyDirectory);
            
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
