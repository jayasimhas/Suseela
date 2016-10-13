using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using InformaSitecoreWord.Util.Document;
using PluginModels;
using InformaSitecoreWord.Util;

namespace InformaSitecoreWord.UI.ArticleDetailsForm.ArticleDetailsControls
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
			pageArticleInformationControl.Visible = true;
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

			ArticleInformationMenuItem.SetMenuTitle("Article Information");
			RelatedArticlesMenuItem.SetMenuTitle("Related Articles");
			WorkflowMenuItem.SetMenuTitle("Workflow");
			FeaturedImagesMenuItem.SetMenuTitle("Featured Image");
			TaxonomyMenuItem.SetMenuTitle("Taxonomy");
		}

		public void InitializePages()
		{
			Pages.Add(pageArticleInformationControl);
			Pages.Add(pageRelatedArticlesControl);
			Pages.Add(pageWorkflowControl);
			Pages.Add(pageFeaturedImageControl);
			Pages.Add(pageTaxonomyControl);

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
			string ExecutiveSummary = metadataParser.ExecutiveSummary;

			var articleDetails = new ArticleStruct();
			try
			{
				articleDetails.ArticleNumber = pageArticleInformationControl.GetArticleNumber();
				articleDetails.WebPublicationDate = pageArticleInformationControl.GetWebPublishDate();
				articleDetails.Title = metadataParser.Title.Trim();
				articleDetails.Summary = ExecutiveSummary;
				articleDetails.Subtitle = metadataParser.Subtitle;
				articleDetails.Publication = pageArticleInformationControl.GetSelectedPublicationGuid();
				articleDetails.Authors = pageArticleInformationControl.GetSelectedAuthors().ToList();
				articleDetails.Label = pageArticleInformationControl.GetLabelGuid();
				articleDetails.MediaType = pageArticleInformationControl.GetMediaTypeGuid();
				articleDetails.NotesToEditorial = pageArticleInformationControl.GetNotes();
				articleDetails.Taxonomoy = pageTaxonomyControl.TabController.GetSelected().ToList();
				articleDetails.RelatedInlineArticles = pageRelatedArticlesControl.GetInlineReferences().ToList();
				articleDetails.RelatedArticles = pageRelatedArticlesControl.GetRelatedArticles().ToList();
				articleDetails.ArticleSpecificNotifications = pageWorkflowControl.GetNotifyList().ToList();
				articleDetails.Embargoed = pageArticleInformationControl.GetEmbargoedState();
				articleDetails.FeaturedImageCaption = pageFeaturedImageControl.GetFeaturedImageCaption();
				articleDetails.FeaturedImageSource = pageFeaturedImageControl.GetFeaturedImageSource();
				articleDetails.NotificationText = pageWorkflowControl.GetNotificationText();
				articleDetails.CommandID = pageWorkflowControl.GetSelectedCommand();

				if (pageFeaturedImageControl.GetFeaturedImage() != null)
				{
					articleDetails.FeaturedImage = pageFeaturedImageControl.GetFeaturedImage().MediaId;
				}
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.Log("ArticleDetailsPageSelector:GetArticleDetails - Error :" + ex.Message);
			}
			return articleDetails;
		}

		public ArticleStruct GetArticleDetailsWithoutDocumentParsing()
		{
			var articleDetails = new ArticleStruct();
			try
			{
                articleDetails.ArticleNumber = pageArticleInformationControl.GetArticleNumber();
				articleDetails.WebPublicationDate = pageArticleInformationControl.GetWebPublishDate();
				articleDetails.Publication = pageArticleInformationControl.GetSelectedPublicationGuid();
				articleDetails.Authors = pageArticleInformationControl.GetSelectedAuthors().ToList();
				articleDetails.Label = pageArticleInformationControl.GetLabelGuid();
				articleDetails.MediaType = pageArticleInformationControl.GetMediaTypeGuid();
				articleDetails.NotesToEditorial = pageArticleInformationControl.GetNotes();
				articleDetails.Taxonomoy = pageTaxonomyControl.TabController.GetSelected().ToList();
				articleDetails.RelatedInlineArticles = pageRelatedArticlesControl.GetInlineReferences().ToList();
				articleDetails.RelatedArticles = pageRelatedArticlesControl.GetRelatedArticles().ToList();
				articleDetails.Embargoed = pageArticleInformationControl.GetEmbargoedState();
				articleDetails.FeaturedImageCaption = pageFeaturedImageControl.GetFeaturedImageCaption();
				articleDetails.FeaturedImageSource = pageFeaturedImageControl.GetFeaturedImageSource();
				articleDetails.NotificationText = pageWorkflowControl.GetNotificationText();
				articleDetails.CommandID = pageWorkflowControl.GetSelectedCommand();

				if (pageFeaturedImageControl.GetFeaturedImage() != null)
				{
					articleDetails.FeaturedImage = pageFeaturedImageControl.GetFeaturedImage().MediaId;
				}
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.Log("ArticleDetailsPageSelector:GetArticleDetailsWithoutDocumentParsing - Error :" + ex.Message);
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

		public void SetDate(DateTime time, bool isLocalTimezone)
		{
			pageArticleInformationControl.SetPublicationTime(time, isLocalTimezone);
		}

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
			if (_parent != null)
				UpdateFields(SitecoreClient.ForceReadArticleDetails(_parent.GetArticleNumber()));
		}

		public void UpdateFields(Guid articleGuid)
		{
			UpdateFields(SitecoreClient.ForceReadArticleDetails(articleGuid));
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
			pageWorkflowControl.UpdateFields(articleDetails.ArticleWorkflowState, articleDetails);
            pageTaxonomyControl.UpdateFields(articleDetails);

            pageArticleInformationControl.OnVerticalItemChanged -= UpdateTaxonomyControl;//added,21Sep16
            pageArticleInformationControl.OnVerticalItemChanged += UpdateTaxonomyControl;//added,21Sep16

            if (string.IsNullOrEmpty(articleDetails.ArticleNumber))
			{
				return;
			}
			//pageSubjectsControl.TabController.UpdateFields(articleDetails.Subjects.ToList());	
			pageTaxonomyControl.TabController.UpdateFields(articleDetails.Taxonomoy.ToList());
			pageWorkflowControl.UpdateFields(articleDetails.ArticleWorkflowState, articleDetails);
		}

        //added,21Sep16
        public void UpdateTaxonomyControl()
        {
            if (PluginSingletonVerticalRoot.Instance.CurrentVertical.Name == null)
            {
                MessageBox.Show("Vertical Information Not Available.",@"Informa",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }
            if (PluginSingletonVerticalRoot.Instance.CurrentVertical.TaxonomyItem.ID == default(Guid))
            {
                //MessageBox.Show("Taxonomy not exists fot vertical.",@"Informa",MessageBoxButtons.OK,MessageBoxIcon.Exclamation);
                return;
            }

            pageTaxonomyControl.PopulateTaxonomyItems();
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
                prompt = false;
				return pageArticleInformationControl.CheckOut(prompt);
			}
			string articleNumber = _parent.GetArticleNumber();
			if (!string.IsNullOrEmpty(articleNumber))
			{
				if (!SitecoreClient.DoesArticleExist(articleNumber))
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
