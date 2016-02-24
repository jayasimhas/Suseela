using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using PluginModels;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using SitecoreTreeWalker.Util.Document;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls
{
	public partial class ArticleDetailsPageSelector : UserControl
	{
		public List<MenuSelectorItem> MenuItems = new List<MenuSelectorItem>();
		public List<ArticleDetailsPageUserControl> Pages = new List<ArticleDetailsPageUserControl>();

		private ArticleDetail _parent;

		public ArticleDetailsPageSelector()
		{
			InitializeComponent();

			InitializeMenuItems();

			ArticleInformationMenuItem.Selected = true;
			//pageArticleInformationControl.Visible = true;
			ArticleInformationMenuItem.UpdateBackground();
		}

		#region Initialization and Linking

		private void InitializeMenuItems()
		{
			MenuItems.Add(ArticleInformationMenuItem);
			MenuItems.Add(RelatedArticlesMenuItem);
			MenuItems.Add(WorkflowMenuItem);
			MenuItems.Add(FeaturedImagesMenuItem);
			MenuItems.Add(TaxonomyMenuItem);
			//MenuItems.Add(NotesMenuItem);

			ArticleInformationMenuItem.SetMenuTitle("Article Information");
			//RelatedDealsMenuItem.SetMenuTitle("Related Deals");
			//RelatedDealsMenuItem.SetIndicatorIcon(Properties.Resources.blankred);
			//RelatedDealsMenuItem.SetIndicatorNumber("0");
			RelatedArticlesMenuItem.SetMenuTitle("Related Articles");
			//SupportingDocumentsMenuItem.SetMenuTitle("Supporting Documents");
			//SupportingDocumentsMenuItem.SetIndicatorNumber("0");
			//SupportingDocumentsMenuItem.SetIndicatorIcon(Properties.Resources.blankred);
			//CompaniesMenuItem.SetMenuTitle("Companies");
			//CompaniesMenuItem.SetIndicatorNumber("0");
			//CompaniesMenuItem.SetIndicatorIcon(Properties.Resources.blankred);
			WorkflowMenuItem.SetMenuTitle("Workflow");
			FeaturedImagesMenuItem.SetMenuTitle("Featured Image");
			TaxonomyMenuItem.SetMenuTitle("Taxonomy");
			//NotesMenuItem.SetMenuTitle("Notes");			       
		}

		public void InitializePages()
		{
			Pages.Add(pageArticleInformationControl);
			Pages.Add(pageRelatedArticlesControl);
			Pages.Add(pageWorkflowControl);
			Pages.Add(pageFeaturedImageControl);
			Pages.Add(pageTaxonomyControl);
			//Pages.Add(PageNotesControl);

			LinkMenuItems();

			//don't have redx for summary menuitem
			(Pages[Pages.Count - 1]).LinkToMenuItem(MenuItems[MenuItems.Count - 1]);

			foreach (ArticleDetailsPageUserControl p in Pages)
			{
				p.Visible = false;
			}
		}

		private void LinkMenuItems()
		{
			// manually set the main info page to redx 
			(Pages[0]).LinkToMenuItem(MenuItems[0]);
			MenuItems[0].SetIndicatorIcon(Properties.Resources.redx);

			for (int i = 1; i < MenuItems.Count - 1; i++)
			{
				(Pages[i]).LinkToMenuItem(MenuItems[i]);
				MenuItems[i].SetIndicatorIcon(Properties.Resources.blankred);
			}
		}

		#endregion

		#region Data Retrieval
		/// <summary>
		/// Builds the Article Details based on the the data entered in the UI
		/// 
		/// Retrieve the relevant information from the UI
		/// </summary>
		/// <returns></returns>
		public ArticleStruct GetArticleDetails(ArticleDocumentMetadataParser metadataParser = null)
		{
			if (metadataParser == null)
			{
				metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument,
																   _parent.GetWordUtils().CharacterStyleTransformer);
			}
			string longSummary = metadataParser.LongSummary;
			string shortSummary = metadataParser.ShortSummary;
			var articleDetails = new ArticleStruct
			{
				ArticleNumber = pageArticleInformationControl.GetArticleNumber(),
				WebPublicationDate = pageArticleInformationControl.GetWebPublishDate(),
				Title = metadataParser.Title.Trim(),
				Summary = longSummary,
				Subtitle = metadataParser.Subtitle,
				Publication = pageArticleInformationControl.GetSelectedPublicationGuid(),
				Authors = pageArticleInformationControl.GetSelectedAuthors().ToList(),
				Label = pageArticleInformationControl.GetLabelGuid(),
				MediaType = pageArticleInformationControl.GetMediaTypeGuid(),
				NotesToEditorial = pageArticleInformationControl.GetNotes(),
				Taxonomoy = pageTaxonomyControl.TabController.GetSelected().ToList(),
				//Subjects = pageSubjectsControl.TabController.GetSelected(),
				//NotesToEditorial = pageArticleInformationControl.PageNotesControl.GetNotesToEditors(),
				RelatedInlineArticles = pageRelatedArticlesControl.GetInlineReferences().ToList(),
				RelatedArticles = pageRelatedArticlesControl.GetRelatedArticles().ToList(),
				ArticleSpecificNotifications = pageArticleInformationControl.GetSelectedNotifyees().ToList(),
				Embargoed = pageArticleInformationControl.GetEmbargoedState(),
				FeaturedImageCaption = pageFeaturedImageControl.GetFeaturedImageCaption(),
				FeaturedImageSource = pageFeaturedImageControl.GetFeaturedImageSource()
			};

			if (pageFeaturedImageControl.GetFeaturedImage() != null)
			{
				articleDetails.FeaturedImage = pageFeaturedImageControl.GetFeaturedImage().MediaId;
			}

			return articleDetails;
		}

		public ArticleStruct GetArticleDetailsWithoutDocumentParsing()
		{
			var articleDetails = new ArticleStruct
			{
				ArticleNumber = pageArticleInformationControl.GetArticleNumber(),
				WebPublicationDate = pageArticleInformationControl.GetWebPublishDate(),
				Publication = pageArticleInformationControl.GetSelectedPublicationGuid(),
				Authors = pageArticleInformationControl.GetSelectedAuthors().ToList(),
				Label = pageArticleInformationControl.GetLabelGuid(),
				MediaType = pageArticleInformationControl.GetMediaTypeGuid(),
				NotesToEditorial = pageArticleInformationControl.GetNotes(),
				//Industries = pageIndustriesControl.TabController.GetSelected(),
				Taxonomoy = pageTaxonomyControl.TabController.GetSelected().ToList(),
				//Subjects = pageSubjectsControl.TabController.GetSelected(),			
				//TODO - Editorial Notes
				//NotesToEditorial = pageArticleInformationControl.PageNotesControl.GetNotesToEditors(),
				//although this technically requires document parsing, we want to retrieve it 
				//as though it didn't
				RelatedInlineArticles = pageRelatedArticlesControl.GetInlineReferences().ToList(),
				RelatedArticles = pageRelatedArticlesControl.GetRelatedArticles().ToList(),
				Embargoed = pageArticleInformationControl.GetEmbargoedState(),
				FeaturedImageCaption = pageFeaturedImageControl.GetFeaturedImageCaption(),
				FeaturedImageSource = pageFeaturedImageControl.GetFeaturedImageSource(),
			};

			if (pageFeaturedImageControl.GetFeaturedImage() != null)
			{
				articleDetails.FeaturedImage = pageFeaturedImageControl.GetFeaturedImage().MediaId;
			}

			return articleDetails;
		}

		public string GetProperDate()
		{
			return pageArticleInformationControl.GetProperDate();
		}

		public string GetPublicationName()
		{
			return pageArticleInformationControl.GetSelectedPublicationName();
		}
		public Guid GetPublicationGuid()
		{
			return pageArticleInformationControl.GetSelectedPublicationGuid();
		}

		public int GetTaxonomyCount()
		{
			return pageTaxonomyControl.TabController.GetSelected().ToList().Count;
		}

		public DateTime GetDate()
		{
			return pageArticleInformationControl.GetWebPublishDate();
		}

		public void SetDate(DateTime time)
		{
			pageArticleInformationControl.SetPublicationTime(time, true);
		}

		/*
		public bool TryingToNominateWithNoIndustries()
		{
		    return pageArticleInformationControl.uxNominate.Enabled
		           && pageArticleInformationControl.uxNominate.Checked;
		    //&& pageIndustriesControl.TabController.GetSelected().Count() == 0;
		}

		public bool TryingToNominateWithNoPrimaryIndustries()
		{
		    return pageArticleInformationControl.uxNominate.Enabled
		           && pageArticleInformationControl.uxNominate.Checked;
		    //&& !SitecoreGetter.HasPrimaryIndustries(pageIndustriesControl.TabController.GetSelected().Select(i => i.ID));
		}
         * */

		#endregion

		#region Minor UI Manipulation

		public void SwitchToPage(UserControl page)
		{
			Pages.ForEach(p => p.Visible = false);
			page.Visible = true;
			page.BringToFront();
			foreach (MenuSelectorItem m in MenuItems)
			{
				m.Selected = false;
			}
			MenuSelectorItem menuItem = ((ArticleDetailsPageUserControl)page).GetMenuItem();
			menuItem.Selected = true;

			foreach (MenuSelectorItem m in MenuItems)
			{
				m.UpdateBackground();
			}
		}

		public void SwitchToNextPage()
		{
			MenuSelectorItem selected = MenuItems.Where(m => m.Selected).Single();
			int index = MenuItems.IndexOf(selected);

			if (index < MenuItems.Count - 1)
			{
				selected.Selected = false;
				selected.UpdateBackground();

				selected = MenuItems[index + 1];
				selected.Selected = true;
				selected.UpdateBackground();

				Pages[index].Visible = false;
				Pages[index + 1].Visible = true;
			}
		}

		public void SwitchToPreviousPage()
		{
			MenuSelectorItem selected = MenuItems.Where(m => m.Selected).Single();
			int index = MenuItems.IndexOf(selected);
			if (index > 0)
			{
				selected.Selected = false;
				selected.UpdateBackground();

				selected = MenuItems[index - 1];
				selected.Selected = true;
				selected.UpdateBackground();

				Pages[index].Visible = false;
				Pages[index - 1].Visible = true;
			}
		}

		/// <summary>
		/// Sets the enabled statuses and visibilities of the controls
		/// for an unlinked document
		/// </summary>
		public void PreLinkEnable()
		{
			pageArticleInformationControl.PreLinkEnable();
			pageRelatedArticlesControl.Enabled = false;
			pageWorkflowControl.Enabled = false;
			pageFeaturedImageControl.Enabled = false;
			pageTaxonomyControl.Enabled = false;
			//pageArticleInformationControl.PageNotesControl.PreLinkEnable();
		}

		/// <summary>
		/// Sets the enabled statuses and visibilities of the controls
		/// for a linked document
		/// </summary>
		public void PostLinkEnable()
		{
			pageArticleInformationControl.PostLinkEnable();

			pageRelatedArticlesControl.Enabled = true;
			pageWorkflowControl.Enabled = true;
			pageFeaturedImageControl.Enabled = true;
			pageTaxonomyControl.Enabled = true;
			//pageArticleInformationControl.PageNotesControl.PostLinkEnable();
		}

		#endregion

		#region UI Updaters 

		public void UpdateFields()
		{
			UpdateFields(SitecoreGetter.ForceReadArticleDetails(_parent.GetArticleNumber()));
		}

		public void UpdateFields(Guid articleGuid)
		{
			UpdateFields(SitecoreGetter.ForceReadArticleDetails(articleGuid));
		}

		public void ResetFields()
		{
			pageArticleInformationControl.ResetFields();
			pageWorkflowControl.ResetNotificationList();
			//pageSubjectsControl.TabController.ResetFields();
			pageTaxonomyControl.TabController.ResetFields();
			pageFeaturedImageControl.ResetFields();
			//pageArticleInformationControl.PageNotesControl.ResetFields();
			pageRelatedArticlesControl.ResetFields();
		}

		/// <summary>
		/// Sets the fields in the UI based on the inputted ArticleStruct
		/// </summary>
		/// <param name="articleDetails"></param>
		public void UpdateFields(ArticleStruct articleDetails)
		{
			pageArticleInformationControl.UpdateFields(articleDetails);
			pageRelatedArticlesControl.UpdateFields(articleDetails);
			pageFeaturedImageControl.UpdateFields(articleDetails);
			//pageWorkflowControl.UpdateFields(articleDetails.WorkflowState);
			pageTaxonomyControl.UpdateFields(articleDetails);

			if (string.IsNullOrEmpty(articleDetails.ArticleNumber))
			{
				return;
			}
			//pageSubjectsControl.TabController.UpdateFields(articleDetails.Subjects.ToList());	
			pageTaxonomyControl.TabController.UpdateFields(articleDetails.Taxonomoy.ToList());
			pageWorkflowControl.UpdateFields(articleDetails.WorkflowState);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="prompt">Flag to prompt user if problem occurs</param>
		/// <returns></returns>
		public bool CheckOut(bool prompt = true)
		{
			if (_parent.ArticleDetails.ArticleGuid != Guid.Empty)
			{
				return pageArticleInformationControl.CheckOut(prompt);
			}
			string articleNumber = _parent.GetArticleNumber();
			if (!string.IsNullOrEmpty(articleNumber))
			{
				if (!SitecoreArticle.DoesArticleExist(articleNumber))
				{
					//_parent.SetArticleNumber(null);
					return false;
				}
				Globals.SitecoreAddin.Log("Article guid not found.");
				return pageArticleInformationControl.CheckOut(articleNumber, prompt);
			}
			Globals.SitecoreAddin.Log("Article #" + articleNumber + " not found.");
			return false;
		}

		public void SetCheckoutStatus()
		{
			pageArticleInformationControl.SetCheckedOutStatus();
		}

		public void LinkToParent(ArticleDetail parent)
		{
			_parent = parent;
			pageArticleInformationControl.LinkToParent(parent);
		}

		/// <summary>
		/// Set all the HasChanged statuses to false
		/// </summary>
		public void ResetChangedStatus(bool savingArticle = false)
		{
			foreach (ArticleDetailsPageUserControl p in Pages)
			{

				MenuSelectorItem menuItem = p.GetMenuItem();
				if (savingArticle || !p.DoesNotChangeOnSaveMetadata())
				{
					menuItem.HasChanged = false;
				}

				menuItem.UpdateBackground();
			}
			//pageGeneralTagsControl.PushSitecoreChanges();

		}

		#endregion

		#region MenuItemActionListeners

		private void ArticleInformationMenuItem_Click(object sender, EventArgs e)
		{
			SwitchToPage(pageArticleInformationControl);
		}

		private void RelatedArticlesMenuItem_Click(object sender, EventArgs e)
		{
			SwitchToPage(pageRelatedArticlesControl);
		}

		private void WorkflowMenuItem_Click(object sender, EventArgs e)
		{
			SwitchToPage(pageWorkflowControl);
		}

		private void FeaturedImageMenuItem_Click(object sender, EventArgs e)
		{
			SwitchToPage(pageFeaturedImageControl);
		}

		private void TaxonomyMenuItem_Click(object sender, EventArgs e)
		{
			SwitchToPage(pageTaxonomyControl);
		}

		#endregion

		private void FlowLayoutPanel_Paint(object sender, PaintEventArgs e)
		{
			var borderColor = Color.FromArgb(130, 135, 144);
			ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor, ButtonBorderStyle.Solid);
		}

	}
}
