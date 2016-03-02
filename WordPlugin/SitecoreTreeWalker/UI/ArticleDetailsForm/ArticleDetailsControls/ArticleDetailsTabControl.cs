using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.SitecoreTree;
using InformaSitecoreWord.UI.Controllers;
using InformaSitecoreWord.Util.Document;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class ArticleDetailsTabControl : UserControl
	{
		private TaxonomyTabController _industryTabController;
		private TaxonomyTabController _subjectsTabController;
		private TaxonomyTabController _geographyTabController;
		private TaxonomyTabController _marketSegmentsTabController;
		private TaxonomyTabController _therapeuticCategoriesTabController;

		private ArticleDetails2 _parent;

		public ArticleDetailsTabControl()
		{
			InitializeComponent();
			InitializeTaxonomyControllers();
		}

		/// <summary>
		/// Builds the Article Details based on the the data entered in the UI
		/// 
		/// Retrieve the relevant information from the UI
		/// </summary>
		/// <returns></returns>
		public SitecoreServer.ArticleStruct GetArticleDetails()
		{
			var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.WordApp.ActiveDocument);
			var articleDetails = new SitecoreServer.ArticleStruct
			    {
					ArticleNumber = articleInformationControl1.GetArticleNumber(),
					WebPublicationDate = articleInformationControl1.GetWebPublishDate(),
					ArticleCategory = articleInformationControl1.GetSelectedArticleCategoryGuid(),
					Title = metadataParser.Title,
					LongSummary = metadataParser.LongSummary,
					ShortSummary = metadataParser.ShortSummary,
					Subtitle = metadataParser.Subtitle,
					Deck = metadataParser.Deck,
			        Issue = articleInformationControl1.GetSelectedIssue(),
			        Publication = articleInformationControl1.GetSelectedPublicationGuid(),
			        Authors = articleInformationControl1.GetSelectedAuthors().ToArray(),
			        Editors = articleInformationControl1.GetSelectedEditors().ToArray(),
					Industries = _industryTabController.GetSelected(),
					Subjects = _subjectsTabController.GetSelected(),
					Geography = _geographyTabController.GetSelected(),
					MarketSegments = _marketSegmentsTabController.GetSelected(),
					TherapeuticCategories = _therapeuticCategoriesTabController.GetSelected(),
					NotesToEditorial = summaryControl1.GetNotesToEditors(),
					NotesToProduction = summaryControl1.GetNotesToProduction()
				};

			return articleDetails;
		}

		/// <summary>
		/// Set all the HasChanged statuses to false
		/// </summary>
		public void ResetChangedStatus()
		{
			_industryTabController.HasChanged = false;
			_subjectsTabController.HasChanged = false;
			_geographyTabController.HasChanged = false;
			_marketSegmentsTabController.HasChanged = false;
			_therapeuticCategoriesTabController.HasChanged = false;

			_parent.Repaint();
		}

		private void InitializeTaxonomyControllers()
		{
			InitializeIndustriesTab();
			InitializeSubjectsTab();
			InitializeGeographyTab();
			InitializeTherapeuticCategoriesTab();
			InitializeMarketSegmentsTab();

			uxIndustriesTabPage.Tag = _industryTabController;
			uxMarketSegmentsTabPage.Tag = _marketSegmentsTabController;
			uxTherapeuticCategoriesTabPage.Tag = _therapeuticCategoriesTabController;
			uxGeographyTabPage.Tag = _geographyTabController;
			uxSubjectsTabPage.Tag = _subjectsTabController;
		}

		private void InitializeIndustriesTab()
		{
			List<TaxonomyStruct> industries = SitecoreGetter.SearchTaxonomy(new Guid(Constants.INDUSTRY_GUID), "").ToList();
			HDirectoryStruct industryDirectory = SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.INDUSTRY_GUID));

			//_industryTabController = new TaxonomyTabController(uxIndustriesKeywords, uxIndustriesViewTree, uxIndustriesViewSearch,
			//    uxIndustriesResults, uxIndustriesResultsTree, uxIndustriesSelected, industries, industryDirectory,
			//    uxIndustriesIcon, tabControl1, uxIndustriesTabPage);
		}

		private void InitializeGeographyTab()
		{
			List<TaxonomyStruct> regions =
				SitecoreGetter.SearchTaxonomy(new Guid(Constants.REGION_GUID), "").ToList();
			HDirectoryStruct regionDirectory = SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.REGION_GUID));
			

			//_geographyTabController = new TaxonomyTabController(uxGeographyKeywords, uxGeographyViewTree, uxGeographyViewSearch,
			//    uxGeographyResults, uxGeographyResultsTree, uxGeographySelected, regions, regionDirectory,
			//    uxGeographyIcon, tabControl1, uxGeographyTabPage);
		}

		private void InitializeSubjectsTab()
		{
			List<TaxonomyStruct> subjects =
				SitecoreGetter.SearchTaxonomy(new Guid(Constants.SUBJECT_GUID), "").ToList();
			HDirectoryStruct subjectDirectory = SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.SUBJECT_GUID));

			//_subjectsTabController = new TaxonomyTabController(uxSubjectsKeywords, uxSubjectsViewTree, uxSubjectsViewSearch,
			//    uxSubjectsResults, uxSubjectsResultsTree, uxSubjectsSelected, subjects, subjectDirectory,
			//    uxSubjectsIcon, tabControl1, uxSubjectsTabPage);
		}

		private void InitializeTherapeuticCategoriesTab()
		{
			List<TaxonomyStruct> therapeuticCategories =
				SitecoreGetter.SearchTaxonomy(new Guid(Constants.THERAPEUTIC_CATEGORY_GUID), "").ToList();
			HDirectoryStruct therapeuticCategoriesDirectory = 
				SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.THERAPEUTIC_CATEGORY_GUID));

			//_therapeuticCategoriesTabController = new TaxonomyTabController(uxTherapeuticCategoriesKeywords, uxTherapeuticCategoriesViewTree, uxTherapeuticCategoriesViewSearch,
			//    uxTherapeuticCategoriesResults, uxTherapeuticCategoriesResultsTree, uxTherapeuticCategoriesSelected, therapeuticCategories, therapeuticCategoriesDirectory,
			//    uxTherapeuticCategoriesIcon, tabControl1, uxTherapeuticCategoriesTabPage);
		}

		private void InitializeMarketSegmentsTab()
		{
			List<TaxonomyStruct> marketSegments =
				SitecoreGetter.SearchTaxonomy(new Guid(Constants.MARKET_SEGMENT_GUID), "").ToList();
			HDirectoryStruct marketSegmentsDirectory = 
				SitecoreGetter.GetHierarchyByGuid(new Guid(Constants.MARKET_SEGMENT_GUID));

			//_marketSegmentsTabController = new TaxonomyTabController(uxMarketSegmentsKeywords, uxMarketSegmentsViewTree, uxMarketSegmentsViewSearch,
			//    uxMarketSegmentsResults, uxMarketSegmentsResultsTree, uxMarketSegmentsSelected, marketSegments, marketSegmentsDirectory,
			//    uxMarketSegmentsIcon, tabControl1, uxMarketSegmentsTabPage);
		}

		/// <summary>
		/// Sets the fields in the UI based on the inputted ArticleStruct
		/// </summary>
		/// <param name="articleDetails"></param>
		public void UpdateFields(SitecoreServer.ArticleStruct articleDetails)
		{
			articleInformationControl1.UpdateFields(articleDetails);
			//generalTagsControl.UpdateFields(whatever);
			_industryTabController.UpdateFields(articleDetails.Industries.ToList());
			_subjectsTabController.UpdateFields(articleDetails.Subjects.ToList());
			_geographyTabController.UpdateFields(articleDetails.Geography.ToList());
			_marketSegmentsTabController.UpdateFields(articleDetails.MarketSegments.ToList());
			_therapeuticCategoriesTabController.UpdateFields(articleDetails.TherapeuticCategories.ToList());
			summaryControl1.UpdateFields(articleDetails);
		}

		public bool CurrentPublicationIsDaily()
		{
			return articleInformationControl1.CurrentPublicationIsDaily();
		}

		public void UpdateArticleNumber(string articleNumber)
		{
			articleInformationControl1.UpdateArticleNumber(articleNumber);
		}

		public void SelectNextTab()
		{
			if (tabControl1.SelectedIndex <= tabControl1.TabCount)
			{
				tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
			}
		}

		public void SelectPreviousTab()
		{
			if (tabControl1.SelectedIndex > 0)
			{
				tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
			}
		}

		public void SetCheckoutStatus()
		{
			articleInformationControl1.SetCheckedOutStatus();
		}

		public string GetProperDate()
		{
			return articleInformationControl1.GetProperDate();
		}

		public string GetPublicationName()
		{
			return articleInformationControl1.GetSelectedPublicationName();
		}

		public void CheckOut()
		{
			articleInformationControl1.CheckOut(_parent.GetArticleNumber());
		}

		public void LinkToParent(ArticleDetails2 parent)
		{
			_parent = parent;
			articleInformationControl1.LinkToParent(parent);
		}

		/// <summary>
		/// Sets the enabled statuses and visibilities of the controls
		/// for an unlinked document
		/// </summary>
		public void PreLinkEnable()
		{
			articleInformationControl1.PreLinkEnable();

			uxRelatedDealsTabPage.Enabled = false;
			uxSupportingDocumentsTabPage.Enabled = false;
			uxCompaniesTabPage.Enabled = false;
			uxIndustriesTabPage.Enabled = false;
			uxSubjectsTabPage.Enabled = false;
			uxGeographyTabPage.Enabled = false;
			uxTherapeuticCategoriesTabPage.Enabled = false;
			uxMarketSegmentsTabPage.Enabled = false;
			uxGeneralTagsTabPage.Enabled = false;
			summaryControl1.PreLinkEnable();
		}

		/// <summary>
		/// Sets the enabled statuses and visibilities of the controls
		/// for a linked document
		/// </summary>
		public void PostLinkEnable()
		{
			articleInformationControl1.PostLinkEnable();

			uxRelatedDealsTabPage.Enabled = true;
			uxSupportingDocumentsTabPage.Enabled = true;
			uxCompaniesTabPage.Enabled = true;
			uxIndustriesTabPage.Enabled = true;
			uxSubjectsTabPage.Enabled = true;
			uxGeographyTabPage.Enabled = true;
			uxTherapeuticCategoriesTabPage.Enabled = true;
			uxMarketSegmentsTabPage.Enabled = true;
			uxGeneralTagsTabPage.Enabled = true;
			summaryControl1.PostLinkEnable();
		}

		private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			Brush textBrush;

			// Get the item from the collection.
			TabPage tabPage = tabControl1.TabPages[e.Index];

			// Get the real bounds for the tab rectangle.
			Rectangle tabBounds = tabControl1.GetTabRect(e.Index);

			//space to the right
			const int rightPadding = 5;
			tabBounds.X = tabBounds.X - rightPadding;
			tabBounds.Width = tabBounds.Width - rightPadding;

			if (e.State == DrawItemState.Selected)
			{
				// Draw a different background color, and don't paint a focus rectangle.
				textBrush = new SolidBrush(Color.Black);
				g.FillRectangle(Brushes.White, e.Bounds);
			}
			else
			{
				textBrush = new SolidBrush(e.ForeColor);
				e.DrawBackground();
				g.FillRectangle(Brushes.LightGray, e.Bounds);
			}

			// Use our own font. Because we CAN.
			var tabFont = new Font("Arial", 8f, FontStyle.Regular, GraphicsUnit.Point);

			// Draw string. Vertical alignment is center, align right.
			var stringFlags = 
				new StringFormat
					{
						Alignment = StringAlignment.Far, 
						LineAlignment = StringAlignment.Center
					};

			g.DrawString
				(tabPage.Text,
				 tabFont,
				 textBrush,
				 tabBounds,
				 new StringFormat(stringFlags));

			//draw asterisk if tab changed
			var asteriskFont = new Font("Arial", 20f, FontStyle.Regular, GraphicsUnit.Point);
			var asteriskFlags =
				new StringFormat
				{
					Alignment = StringAlignment.Near,
					LineAlignment = StringAlignment.Center
				};
			textBrush = new SolidBrush(Color.Green);

			const int distanceFromRightSide = 50;
			const int distanceFromTop = 10;

			if(tabPage.Tag != null)
			{
				var tabController = tabPage.Tag as ITabController;

				if(tabController != null)
				{

					tabBounds.X = tabBounds.X + distanceFromRightSide;
					tabBounds.Y = tabBounds.Y + distanceFromTop;

					if(tabController.HasChanged())
					{
						g.DrawString(
							"*",
							asteriskFont,
							textBrush,
							tabBounds,
							new StringFormat(asteriskFlags));
					}
				}
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabControl1.SelectedTab == uxSummaryTabPage)
			{
				summaryControl1.SetData(GetArticleDetails());
			}
			if (tabControl1.SelectedIndex == tabControl1.TabCount-1)
			{
				_parent.HideButtonNext();
			}
			else
			{
				_parent.ShowButtonNext();
			}
			if(tabControl1.SelectedIndex == 0)
			{
				_parent.HideButtonPrevious();
			}
			else
			{
				_parent.ShowButtonPrevious();
			}
		}
	}
}
