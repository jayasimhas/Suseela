using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Properties;
using InformaSitecoreWord.SitecoreServer;
using InformaSitecoreWord.SitecoreTree;
using InformaSitecoreWord.UI.Controllers;
using InformaSitecoreWord.User;
using Word = Microsoft.Office.Interop.Word;
using InformaSitecoreWord.Util;
using ArticleStruct = InformaSitecoreWord.SitecoreTree.ArticleStruct;
using StaffStruct = InformaSitecoreWord.SitecoreServer.StaffStruct;
using TaxonomyStruct = InformaSitecoreWord.SitecoreServer.TaxonomyStruct;

namespace InformaSitecoreWord.UI
{
	public partial class ArticleDetails : Form
	{
		protected void InitializeDropdowms()
		{
			try
			{
				LoadCategories();

				LoadIssues();

				List<ItemStruct> publications = _scTree.GetPublications().ToList();
				publications.Insert(0, new ItemStruct() { ID = Guid.Empty, Name = "Select Publication" });

				uxPublication.DataSource = publications;
				uxPublication.DisplayMember = "Name";
				uxPublication.ValueMember = "ID";

			}
			catch (Exception ex)
			{
				//TODO: document what kind of exception do you expect to happen here
				//TODO: logging and error handling
				Globals.SitecoreAddin.LogException("Error in article details while initializing dropdowns", ex);
				throw ex;
			}
		}

		protected void LoadCategories()
		{
			try
			{
				Guid pubID = GetPublicationGuid();
				if (pubID == Guid.Empty)
				{
					uxArticleCategory.DataSource = new List<string>() {"Select a publication first"};
					uxArticleCategory.Enabled = false;
				}
				else
				{
					List<ItemStruct> categories = _scTree.GetArticleCategories(GetPublicationGuid()).ToList();
					if (categories.Count > 0)
					{
						categories.Insert(0, new ItemStruct() {Name = "Select Category", ID = Guid.Empty});
						uxArticleCategory.DataSource = categories;
						uxArticleCategory.DisplayMember = "Name";
						uxArticleCategory.ValueMember = "ID";
						uxArticleCategory.Enabled = true && (IsCheckedOutByMe || !HasArticleNumber());
					}
					else
					{
						uxArticleCategory.DataSource = new List<string>() {"No categories available"};
						uxArticleCategory.Enabled = false;
					}

				}
			}
			catch (Exception ex)
			{
				//TODO: document what kind of exception is expected to be caught here
				//it looks like this exception should happen in the normal flow of actions
				//not an exception, it seems to be an expectation
				Globals.SitecoreAddin.LogException("Error in article details while loading categories", ex);
				uxArticleCategory.DataSource = new List<string>() { "Select a publication first" };
				uxArticleCategory.Enabled = false;
			}
		}

		protected void LoadAuthors()
		{
			var matchingAuthors = new List<SitecoreServer.StaffStruct>();
			uxSelectAuthor.DataSource = null;
			if (GetPublicationGuid() == Guid.Empty)
			{
				matchingAuthors = _authors;
			}
				
			else
			{

				foreach (SitecoreServer.StaffStruct author in _authors)
				{
					if (author.Publications.Contains(GetPublicationGuid()))
					{
						matchingAuthors.Add(author);
					}
				}
			}
			

			if(matchingAuthors.Count == 0)
			{
				uxSelectAuthor.Enabled = false;
				uxAddAuthor.Enabled = false;
				matchingAuthors.Add(new SitecoreServer.StaffStruct { ID = Guid.Empty, Name = "There are no authors for selected publication" });
			}
			else
			{
				uxSelectAuthor.Enabled = true;
				uxAddAuthor.Enabled = true;
			}

			uxSelectAuthor.DataSource = matchingAuthors;
			uxSelectAuthor.DisplayMember = "Name";
			uxSelectAuthor.ValueMember = "ID";
		}

		protected void LoadEditors()
		{
			var matchingEditors = new List<SitecoreServer.StaffStruct>();
			uxSelectEditor.DataSource = null;
			if (GetPublicationGuid() == Guid.Empty)
			{
				matchingEditors = _editors;
			}
				
			else
			{

				foreach (SitecoreServer.StaffStruct editor in _editors)
				{
					if (editor.Publications.Contains(GetPublicationGuid()))
					{
						matchingEditors.Add(editor);
					}
				}
			}

			if (matchingEditors.Count == 0)
			{
				uxSelectEditor.Enabled = false;
				uxAddEditor.Enabled = false;
				matchingEditors.Add(new SitecoreServer.StaffStruct { ID = Guid.Empty, Name = "There are no editors for selected publication" });
			}
			else
			{
				uxSelectEditor.Enabled = true;
				uxAddEditor.Enabled = true;
			}

			uxSelectEditor.DataSource = matchingEditors;
			uxSelectEditor.DisplayMember = "Name";
			uxSelectEditor.ValueMember = "ID";
		}

		protected void LoadIssues()
		{
			try
			{
				Guid pubID = GetPublicationGuid();
				if (pubID == Guid.Empty)
				{
					uxArticleInformationIssue.DataSource = new List<string>() { "Select a publication first" };
					//centralize logic of issue enablization
					uxArticleInformationIssue.Enabled = false;
				}
				else
				{
					string publicationFrequency = _scTree.GetPublicationFrequency(pubID);

					if (publicationFrequency == "Daily")
					{
						uxArticleInformationIssue.DataSource = new List<string>() {"Daily publication chosen"};
						uxArticleInformationIssue.Enabled = false;
						_currentPublicationIsDaily = true;
					}
					else
					{
						_currentPublicationIsDaily = false;
						List<ItemStruct> issues = _scTree.GetIssues(pubID).ToList();
						if (issues.Count > 0)
						{
							uxArticleInformationIssue.Enabled = true && (IsCheckedOutByMe || !HasArticleNumber());
							issues.Insert(0, new ItemStruct() { Name = "Select Issue" , ID = Guid.Empty});
							uxArticleInformationIssue.DataSource = issues;
							uxArticleInformationIssue.DisplayMember = "Name";
							uxArticleInformationIssue.ValueMember = "ID";
						}
						else
						{
							uxArticleInformationIssue.DataSource = new List<string>() { "No issues available" };
							uxArticleInformationIssue.Enabled = false;
						}	
					}
				}
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details while loading issues", ex);
				uxArticleInformationIssue.DataSource = new List<string>() { "Select a publication first" };
				uxArticleInformationIssue.Enabled = false;
			}


		}

		protected void InitializeControllers()
		{
			List<SitecoreTree.TaxonomyStruct> industries = _scTree.SearchIndustries("").ToList();
			HDirectoryStruct industryDirectory = _scTree.GetHierarchyByGuid(new Guid(Constants.INDUSTRY_GUID));

			Guid subjectGuid = new Guid(Constants.SUBJECT_GUID);
			List<SitecoreTree.TaxonomyStruct> subjects = _scTree.SearchTaxonomy(subjectGuid, "").ToList();
			HDirectoryStruct subjectDirectory = _scTree.GetHierarchyByGuid(subjectGuid);

			Guid regionGuid = new Guid(Constants.REGION_GUID);
			List<SitecoreTree.TaxonomyStruct> regions = _scTree.SearchTaxonomy(regionGuid, "").ToList();
			HDirectoryStruct regionDirectory = _scTree.GetHierarchyByGuid(regionGuid);

			Guid therapeuticCategoryGuid = new Guid(Constants.THERAPEUTIC_CATEGORY_GUID);
			List<SitecoreTree.TaxonomyStruct> therapeuticCategories = _scTree.SearchTaxonomy(therapeuticCategoryGuid, "").ToList();
			HDirectoryStruct therapeuticCategoryDirectory = _scTree.GetHierarchyByGuid(therapeuticCategoryGuid);

			Guid marketSegmentGuid = new Guid(Constants.MARKET_SEGMENT_GUID);
			List<SitecoreTree.TaxonomyStruct> marketSegments = _scTree.SearchTaxonomy(marketSegmentGuid, "").ToList();
			HDirectoryStruct marketSegmentDirectory = _scTree.GetHierarchyByGuid(marketSegmentGuid);

			_industryTabController = new TaxonomyTabController(uxIndustriesKeywords, uxIndustriesViewTree, uxIndustriesViewSearch, 
				uxIndustriesResults,uxIndustriesResultsTree, uxIndustriesSelected, industries, industryDirectory,
				uxIndustriesIcon, tabControl1, tabControl1.TabPages[4]);

			_subjectTabController = new TaxonomyTabController(uxSubjectsKeywords, uxSubjectsViewTree, uxSubjectsViewSearch, uxSubjectsResults,
				uxSubjectsResultsTree, uxSubjectsSelected, subjects, subjectDirectory, uxSubjectsIcon, tabControl1, tabControl1.TabPages[5]);

			_geographyTabController = new TaxonomyTabController(uxGeographyKeywords, uxGeographyViewTree, uxGeographyViewSearch, uxGeographyResults,
				uxGeographyResultsTree, uxGeographySelected, regions, regionDirectory, uxGeographyIcon, tabControl1, tabControl1.TabPages[6]);

			_therapeuticCategoriesTabController = new TaxonomyTabController(uxTherapeuticCategoriesKeywords, uxTherapeuticCategoriesViewTree, uxTherapeuticCategoriesViewSearch,
				uxTherapeuticCategoriesResults, uxTherapeuticCategoriesResultsTree, uxTherapeuticCategoriesSelected, therapeuticCategories,
				therapeuticCategoryDirectory, uxTherapeuticCategoriesIcon, tabControl1, tabControl1.TabPages[7]);

			_marketSegmentsTabController = new TaxonomyTabController(uxMarketSegmentsKeywords, uxMarketSegmentsViewTree, uxMarketSegmentsViewSearch, uxMarketSegmentsResults,
				uxMarketSegmentsResultsTree, uxMarketSegmentsSelected, marketSegments, marketSegmentDirectory, uxMarketSegmentsIcon, tabControl1, tabControl1.TabPages[8]);

			tabControl1.TabPages[4].Tag = _industryTabController;
			tabControl1.TabPages[5].Tag = _subjectTabController;
			tabControl1.TabPages[6].Tag = _geographyTabController;
			tabControl1.TabPages[7].Tag = _therapeuticCategoriesTabController;
			tabControl1.TabPages[8].Tag = _marketSegmentsTabController;

			_taxonomyTabControllers.Add(_industryTabController);
			_taxonomyTabControllers.Add(_subjectTabController);
			_taxonomyTabControllers.Add(_geographyTabController);
			_taxonomyTabControllers.Add(_therapeuticCategoriesTabController);
			_taxonomyTabControllers.Add(_marketSegmentsTabController);

			loginControl1.ToReveal =uxArticlePanel;


			if(SitecoreUser.GetUser().LoggedIn)
			{
				loginControl1.HideLogin();
			}

			Image collapsedImage = Properties.Resources.right_icon;
			Image expandedImage = Properties.Resources.down_icon;

			_industriesExpandPanel = new ExpandFlowPanel(uxSummaryIndustriesExpand, uxSummaryIndustriesFlow, 
				expandedImage, collapsedImage);
			_subjectsExpandPanel = new ExpandFlowPanel(uxSummarySubjectsExpand, uxSummarySubjectsFlow, 
				expandedImage, collapsedImage);
			_geographyExpandPanel = new ExpandFlowPanel(uxSummaryGeographyExpand, uxSummaryGeographyFlow, 
				expandedImage, collapsedImage);
			_therapeuticCategoriesExpandPanel = new ExpandFlowPanel(uxSummaryTherapeuticCategoriesExpand, 
				uxSummaryTherapeuticCategoriesFlow, expandedImage, collapsedImage);
			_marketSegmentsExpandPanel = new ExpandFlowPanel(uxSummaryMarketSegmentsExpand, uxSummaryMarketSegmentsFlow, 
				expandedImage, collapsedImage);

		}

		protected Guid GetPublicationGuid()
		{
			return _documentCustomProperties.PublicationID;
		}

		protected SitecoreServer.ArticleStruct GetArticleDetails()
		{
			var article = new SitecoreServer.ArticleStruct();
			article.ArticleNumber = GetArticleNumber();

			article.Title = GetDocumentTitle();
			article.Subtitle = uxSubtitle.Text;
			article.Summary = uxArticleInformationSummary.Text;

			article.WebPublicationDate = uxWebPublishDate.Value;
			article.PrintPublicationDate = DateTime.Parse(uxPrintPubDate.Text);

			article.Industries = _industryTabController.SelectedList.Select(t => new TaxonomyStruct(){ID = t.ID, Name = t.Name}).ToArray();
			article.Geography = _geographyTabController.SelectedList.Select(t => new TaxonomyStruct() { ID = t.ID, Name = t.Name }).ToArray();
			article.Subjects = _subjectTabController.SelectedList.Select(t => new TaxonomyStruct() { ID = t.ID, Name = t.Name }).ToArray();
			article.MarketSegments = _marketSegmentsTabController.SelectedList.Select(t => new TaxonomyStruct() { ID = t.ID, Name = t.Name }).ToArray();
			article.TherapeuticCategories = _therapeuticCategoriesTabController.SelectedList.Select(t => new TaxonomyStruct() { ID = t.ID, Name = t.Name }).ToArray();

			article.NotesToEditorial = uxEditorNotes.Text;
			article.NotesToProduction = uxProductionNotes.Text;

			article.Authors = uxSelectedAuthors.Selected.Select(t => new SitecoreServer.StaffStruct() { ID = t.ID, Name = t.Name }).ToArray();
			article.Editors = uxSelectedEditors.Selected.Select(t => new SitecoreServer.StaffStruct() { ID = t.ID, Name = t.Name }).ToArray();
			
			if (uxArticleCategory.SelectedItem is ItemStruct)
			{
			article.ArticleCategory = ((ItemStruct)uxArticleCategory.SelectedItem).ID;
			}


			article.Publication = ((ItemStruct)uxPublication.SelectedItem).ID;

			article.RelatedInlineArticles = _relatedInlineArticles.ToArray();

			return article;

		}

		protected void GetArticeDetailsFromSitecore()
		{
			string articleNumber = GetArticleNumber();
			try
			{
				ArticleStruct articleDetails = _scTree.GetArticleDetails(articleNumber);

				//TODO: Set the geography, industries, and subjects data

				//uxPrintPubDate.Text = articleDetails.PrintPublicationDate.ToString();
				//uxWebPublishDate.Value = articleDetails.WebPublicationDate;

				_industryTabController.SetSelected(articleDetails.Industries.ToList());
				_geographyTabController.SetSelected(articleDetails.Geography.ToList());
				_subjectTabController.SetSelected(articleDetails.Subjects.ToList());
				_therapeuticCategoriesTabController.SetSelected(articleDetails.TherapeuticCategories.ToList());
				_marketSegmentsTabController.SetSelected(articleDetails.MarketSegments.ToList());

				uxPublication.SelectedValue = articleDetails.Publication;
				LoadCategories();
				LoadIssues();

				if (!uxArticleCategory.ValueMember.IsNullOrEmpty())
				{
					uxArticleCategory.SelectedValue = articleDetails.ArticleCategory;
				}

				if (!uxArticleInformationIssue.ValueMember.IsNullOrEmpty())
				{
					uxArticleInformationIssue.SelectedValue = articleDetails.Issue;
				}

				uxTitle.Text = articleDetails.Title;

				uxProductionNotes.Text = articleDetails.NotesToProduction;
				uxEditorNotes.Text = articleDetails.NotesToEditorial;

				uxSelectedAuthors.Populate(articleDetails.Authors.Select(r => 
					new SitecoreServer.StaffStruct()
						{
							ID = r.ID,
							Name = r.Name,
							Publications = r.Publications
						}).ToList());
				uxSelectedEditors.Populate(articleDetails.Editors.Select(r =>
					new SitecoreServer.StaffStruct()
					{
						ID = r.ID,
						Name = r.Name,
						Publications = r.Publications
					}).ToList());
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details while getting article details from sitecore", ex);
				throw new ApplicationException("Could not get details for article [" + articleNumber + "] from sitecore", ex);
			}
		}

		protected string GetArticleNumber()
		{
			return _documentCustomProperties.ArticleNumber;
		}

		private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
		{
			Graphics g = e.Graphics;
			Brush textBrush;

			// Get the item from the collection.
			TabPage tabPage = tabControl1.TabPages[e.Index];

			// Get the real bounds for the tab rectangle.
			Rectangle tabBounds = tabControl1.GetTabRect(e.Index);

			//create some space at left
			tabBounds.X = tabBounds.X + 50;
			//tabBounds.Width = tabBounds.Width - 10;

			if (e.State == DrawItemState.Selected)
			{
				// Draw a different background color, and don't paint a focus rectangle.
				//textBrush = new SolidBrush(Color.Red);
				textBrush = new System.Drawing.SolidBrush(Color.Black);
				g.FillRectangle(Brushes.White, e.Bounds);
			}
			else
			{
				textBrush = new System.Drawing.SolidBrush(e.ForeColor);
				e.DrawBackground();
				g.FillRectangle(Brushes.LightGray, e.Bounds);
			}

			// Use our own font. Because we CAN.
			var tabFont = new Font("Microsoft Sans Serif", 10f, FontStyle.Regular, GraphicsUnit.Point);

			// Draw string. Center the text.
			var stringFlags = new StringFormat();
			stringFlags.Alignment = StringAlignment.Near;
			stringFlags.LineAlignment = StringAlignment.Center;

			g.DrawString
				(tabPage.Text,
				 tabFont,
				 textBrush,
				 tabBounds,
				 new StringFormat(stringFlags));

			var tab = tabPage.Tag as ITabController;
			if (tab != null)
			{
				//draw asterisk to indicate change
				string asterisk = tab.HasChanged() ? "*" : "";
				var asteriskFont = new Font("Arial", 20f);
				Brush asteriskBrush = new SolidBrush(Color.Green);
				Rectangle asteriskBounds = new Rectangle(tabBounds.X - 15, tabBounds.Y + 15, 25, 25);
				var asteriskFormat = new StringFormat { Alignment = StringAlignment.Near, LineAlignment = StringAlignment.Center };
				g.DrawString(asterisk, asteriskFont, asteriskBrush, asteriskBounds, asteriskFormat);
			}
		}

		protected Guid GetIssueGuid()
		{
			Guid id;
			try
			{
				id = new Guid(uxArticleInformationIssue.SelectedValue.ToString());
			}
			catch
			{
				id = Guid.Empty;
			}
			return id;
		}

		

		protected string GetWebPublishDate()
		{
			return uxWebPublishDate.Value.ToString();


		}

		protected string GetDocumentTitle()
		{
			return uxTitle.Text;
		}

		public ArticleDetails()
		{
			try
			{
				Globals.SitecoreAddin.Log("Initializing the article details window...");
				InitializeComponent();
				Opacity = 0;
			
				_scTree = new SCTree();
				_scServer = new SCServer();

				// TEMPORARY code to deal with QA login
				var credCache = new CredentialCache();
				var netCred =
					new NetworkCredential("velir", "ebi3000");
				credCache.Add(new Uri(_scTree.Url), "Basic", netCred);
				credCache.Add(new Uri(_scServer.Url), "Basic", netCred);

				_scTree.PreAuthenticate = true;
				_scTree.Credentials = credCache;

				_scServer.PreAuthenticate = true;
				_scServer.Credentials = credCache;

				// end temporary

				_user = SitecoreUser.GetUser();

				_authors = _scTree.GetAuthors().Select(t => new StaffStruct() 
				                                            	{ ID = t.ID, Name = t.Name, Publications = t.Publications}).ToArray().ToList();
				_editors = _scTree.GetEditors().Select(t => new StaffStruct() 
				                                            	{ ID = t.ID, Name = t.Name, Publications = t.Publications }).ToArray().ToList();

				_sitecoreArticle = new SitecoreArticle();
				_wordUtils = new WordUtils();
				_wordApp = SitecoreAddin.WordApp;
				_documentCustomProperties = new DocumentCustomProperties(_wordApp, _wordApp.ActiveDocument);

			
				try
				{
					InitializeDropdowms();
					InitializeControllers();
					Opacity = 1;

					SetCheckedOutStatus();

					_connectedAtStart = true;
				
				}
				catch
					(WebException e)
				{
					AlertConnectionFailed();
					Globals.SitecoreAddin.LogException("Error getting metadata or checked out status!", e);
					throw new ApplicationException("Error getting metadata or checked out status!", e);
				}

				var deleteIcon = new ImageList();
				deleteIcon.Images.Add(Resources.delete_icon);
				uxSelectedAuthors.SmallImageList = deleteIcon;
				uxSelectedEditors.SmallImageList = deleteIcon;

				string articleNumber = GetArticleNumber();
				if (!articleNumber.IsNullOrEmpty())
				{
					uxArticleNumber.Text = articleNumber;
					GetArticeDetailsFromSitecore();
				}
				uxLockStatus.Refresh();
				Globals.SitecoreAddin.Log("Article details window initialized.");
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error while loading article details!", ex);
			}
		}

		/// <summary>
		/// Get from sitecore checked out status, then set control accordingly
		/// </summary>
		/// <remarks>This method is a beast. We should refactor it if possible.</remarks>
		protected void SetCheckedOutStatus()
		{
			string articleNum = GetArticleNumber();
			if (!articleNum.IsNullOrEmpty())
			{
				CheckoutStatus checkedOut = _scServer.GetLockedStatus(this.GetArticleNumber());
				IsCheckedOut = checkedOut.Locked;
				if (IsCheckedOut)
				{
					if (_user.Username == checkedOut.User.TextAfterString(@"\"))
					{ //Locked, checked out by me
						//TODO: what colors do they want for checked out by me?
						uxLockStatus.BackColor = Color.FromArgb(255, 217, 234, 211);
						//uxLockStatus.ForeColor = Color.Green;

						uxLockUser.Text = checkedOut.User;
						IsCheckedOutByMe = true;

						uxCheckInOut.Enabled = true;
						uxCheckInOut.Text = "Check in article";

						uxSaveChanges.Enabled = true;
						uxSaveAndTransfer.Enabled = true;
					}
					else
					{ //locked, checked out by other
						uxLockStatus.BackColor = Color.FromArgb(255, 244, 204, 204);
						//uxLockStatus.ForeColor = Color.FromArgb(255, 224, 102, 102);

						uxLockUser.Text = checkedOut.User;

						uxCheckInOut.Enabled = false;
						uxCheckInOut.Text = "Check out article";

						IsCheckedOutByMe = false;

						uxSaveChanges.Enabled = false;
						uxSaveAndTransfer.Enabled = false;
					}
					uxStatus.Text = "Locked";
				}
				else
				{ //Unlocked, not checked out
					uxLockStatus.BackColor = GroupBox.DefaultBackColor;
					//uxLockStatus.ForeColor = GroupBox.DefaultForeColor;

					uxLockUser.Text = @"N\A";

					uxCheckInOut.Enabled = true;
					uxCheckInOut.Text = "Check out article";
					uxStatus.Text = "Unlocked";

					IsCheckedOutByMe = false;
					IsCheckedOut = false;

					uxSaveChanges.Enabled = false;
					uxSaveAndTransfer.Enabled = false;
				}
				uxRefreshStatus.Enabled = true;
				uxLinkArticle.Visible = false;
				uxArticleNumberToLink.Visible = false;
			}
			else
			{ //locked, not created yet
				uxLockStatus.BackColor = GroupBox.DefaultBackColor;
				uxLockStatus.ForeColor = GroupBox.DefaultForeColor;

				uxLockUser.Text = @"N\A";

				IsCheckedOutByMe = false;
				IsCheckedOut = false;

				uxCheckInOut.Enabled = false;
				uxCheckInOut.Text = "Check out article";
				uxStatus.Text = "Unlocked";
				uxRefreshStatus.Enabled = false;
				uxLinkArticle.Visible = true;
				uxArticleNumberToLink.Visible = true;

				uxSaveChanges.Enabled = true;
				uxSaveAndTransfer.Enabled = true;
			}
			bool articleNotCreated = GetArticleNumber() == null;
			uxRelatedArticles.Enabled = IsCheckedOutByMe;
			uxSidebarsParents.Enabled = IsCheckedOutByMe;
			uxCompanies.Enabled = IsCheckedOutByMe;
			uxIndustries.Enabled = IsCheckedOutByMe;
			uxSubjects.Enabled = IsCheckedOutByMe;
			uxGeography.Enabled = IsCheckedOutByMe;
			uxTherapeuticCategories.Enabled = IsCheckedOutByMe;
			uxMarketSegments.Enabled = IsCheckedOutByMe;
			uxGeneralTags.Enabled = IsCheckedOutByMe;
			uxSummary.Enabled = IsCheckedOutByMe;
			uxSubtitle.Enabled = IsCheckedOutByMe;
			uxArticleInformationSummary.Enabled = IsCheckedOutByMe;
			//uxSelectAuthor.Enabled = IsCheckedOutByMe;
			uxAddAuthor.Enabled = IsCheckedOutByMe || articleNotCreated;
			//uxSelectedAuthors.Enabled = IsCheckedOutByMe;
			//uxSelectEditor.Enabled = IsCheckedOutByMe;
			//uxSelectedEditors.Enabled = IsCheckedOutByMe;
			uxSelectedEditors.DisableEdit = !IsCheckedOutByMe;
			uxSelectedAuthors.DisableEdit = !IsCheckedOutByMe;
			uxAddEditor.Enabled = IsCheckedOutByMe || articleNotCreated;

			//When Checked Out By You / Article Not Created Yet: 
			//should be able to view, add, remove authors and editors.

			//When checked out by another user or checked in, 
			//should be able to view the list of assigned authors/editors, 
			//and the list of available authors/editors, but not add or remove them.
			uxArticleType.Enabled = IsCheckedOutByMe;
			uxArticleSize.Enabled = IsCheckedOutByMe;
			uxArticleCategory.Enabled = IsCheckedOutByMe;

			uxTitle.Enabled = !IsCheckedOut || IsCheckedOutByMe;
			uxArticleInformationIssue.Enabled = !IsCheckedOut || IsCheckedOutByMe;
			uxPublication.Enabled = !IsCheckedOut || IsCheckedOutByMe;
			uxArticleCategory.Enabled = !IsCheckedOut || IsCheckedOutByMe;
		}

		private bool _isCheckedOut;
		public bool IsCheckedOut
		{
			get { return _isCheckedOut; }
			set
			{
				if (value == false)
				{ // if we're not checked out, we can't be checked out by me.
					IsCheckedOutByMe = false;
				}
				_isCheckedOut = value;
			}
		}

		private bool _isCheckedOutByMe;
		public bool IsCheckedOutByMe
		{
			get { return _isCheckedOutByMe; }
			set
			{
				if (value == true)
				{
					IsCheckedOut = true;
				}
				_isCheckedOutByMe = value;
			}
		}

		protected List<string> _publicationTypes;
		protected List<SitecoreServer.StaffStruct> _authors;
		protected List<SitecoreServer.StaffStruct> _editors;
		protected Word.Application _wordApp;
		protected DocumentCustomProperties _documentCustomProperties;
		protected SCTree _scTree;
		protected SitecoreArticle _sitecoreArticle;
		protected SCServer _scServer;
		protected WordUtils _wordUtils;
		protected readonly SitecoreUser _user;

		private TaxonomyTabController _subjectTabController;
		private TaxonomyTabController _geographyTabController;
		private TaxonomyTabController _industryTabController;
		private TaxonomyTabController _therapeuticCategoriesTabController;
		private TaxonomyTabController _marketSegmentsTabController;
		private List<TaxonomyTabController> _taxonomyTabControllers = new List<TaxonomyTabController>();
		private ExpandFlowPanel _industriesExpandPanel;
		private ExpandFlowPanel _subjectsExpandPanel;
		private ExpandFlowPanel _geographyExpandPanel;
		private ExpandFlowPanel _therapeuticCategoriesExpandPanel;
		private ExpandFlowPanel _marketSegmentsExpandPanel;
		private List<Guid> _relatedInlineArticles = new List<Guid>();

		private bool _currentPublicationIsDaily;
		private readonly bool _connectedAtStart;

		private void uxPublication_SelectedIndexChanged(object sender, EventArgs e)
		{
			Guid pubGuid = ((ItemStruct) uxPublication.SelectedItem).ID;
			if (pubGuid != Guid.Empty)
			{
				_documentCustomProperties.PublicationID = pubGuid;
			}

			try
			{
				LoadIssues();
				LoadAuthors();
				LoadEditors();
				if (IsCheckedOutByMe)
				{
					LoadCategories();
				}
			}

			catch (WebException ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when selection publication", ex);
				AlertConnectionFailed();
			}
		}

		/// <summary>
		/// If the article does not have an article number, get the next article number and save
		/// a stub of the article to sitecore
		/// </summary>
		/// <returns>True if a new article number was retrieved</returns>
		private bool PopulateArticleNumber()
		{
			if(HasArticleNumber())
			{
				return false;
			}
			string title = GetDocumentTitle();
			string webPublishDate = GetWebPublishDate();
			Guid issueGuid = GetIssueGuid();
			Guid pubGuid = GetPublicationGuid();
			if(string.IsNullOrEmpty(title))
			{
				MessageBox.Show(@"Please enter an article title.", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return false;
			}
			
			if(string.IsNullOrEmpty(webPublishDate))
			{
				MessageBox.Show(@"Please enter the web publish date.");
				return false;
			}

			if (pubGuid.Equals(new Guid()))
			{
				MessageBox.Show(@"Please select a publication.");
				return false;
			}

			if(issueGuid == Guid.Empty && !_currentPublicationIsDaily)
			{
				MessageBox.Show(@"Please select an issue.");
				return false;
			}

			this.SuspendLayout();
			string articleNumber =
				_sitecoreArticle.SaveStubToSitecore(
				title,
				webPublishDate,
				pubGuid,
				issueGuid);

			uxArticleNumber.Text = articleNumber;
			_documentCustomProperties.ArticleNumber = articleNumber;
			this.ResumeLayout();

			return true;
		}

		private bool HasArticleNumber()
		{
			return (!string.IsNullOrEmpty(_documentCustomProperties.ArticleNumber));
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			//if summary tab is selected
			if (tabControl1.SelectedIndex == 10)
			{
				uxSummaryMainFlow.SuspendLayout();
				_industriesExpandPanel.SetData(_industryTabController.SelectedList.Select(t => t.Name).ToList());
				_subjectsExpandPanel.SetData(_subjectTabController.SelectedList.Select(t => t.Name).ToList());
				_geographyExpandPanel.SetData(_geographyTabController.SelectedList.Select(t => t.Name).ToList());
				_therapeuticCategoriesExpandPanel.SetData(
					_therapeuticCategoriesTabController.SelectedList.Select(t => t.Name).ToList());
				_marketSegmentsExpandPanel.SetData(
					_marketSegmentsTabController.SelectedList.Select(t => t.Name).ToList());
				uxSummaryMainFlow.ResumeLayout();
			}
		}

		private void uxSaveAndTransfer_Click(object sender, EventArgs e)
		{
			SaveChangesAndTransfer();
		}

		protected void SaveChangesAndTransfer()
		{
			//TODO: logging
			try
			{
				this.Cursor = Cursors.WaitCursor;
				PopulateArticleNumber();
				if (HasArticleNumber())
				{
					XElement x = _wordUtils.GetWordDocTextWithStyles(_wordApp);
					string text = ParseForInlineReferences(x.ToString());
					_scServer.SaveArticleText(GetArticleNumber(), text, GetArticleDetails());
					_taxonomyTabControllers.ForEach(t => t.HasChanged = false);
					tabControl1.Refresh();
					MessageBox.Show("Article details saved and transferred!", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Information);
					SetCheckedOutStatus();
				}

				
			}
			catch(WebException ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when transferring article details", ex);
				AlertConnectionFailed();
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		private void uxSaveChanges_Click(object sender, EventArgs e)
		{
			SaveMetadata();
		}

		protected void SaveMetadata()
		{
			//TODO: error handling and logging
			try
			{
				this.Cursor = Cursors.WaitCursor;
				PopulateArticleNumber();
				if (HasArticleNumber())
				{
					//XElement x = _wordUtils.GetWordDocTextWithStyles(_wordApp);
					//ParseForInlineReferences(x.ToString());
					_scServer.SaveArticleDetails(GetArticleNumber(), GetArticleDetails());
					_taxonomyTabControllers.ForEach(t => t.HasChanged = false);
					tabControl1.Refresh();
					MessageBox.Show("Article details saved!", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Information);
					SetCheckedOutStatus();
				}

			}
			catch (WebException ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when saving metadata", ex);
				AlertConnectionFailed();
			}
			finally
			{
				this.Cursor = Cursors.Default;
			}
		}

		/// <summary>
		/// Returns a string with all the hyperlinks transformed to Sitecore links and
		/// populate the _inlineRelatedArticles
		/// </summary>
		/// <param name="doc">The document in string form</param>
		/// <returns>The document in string form with the html hyperlinks added</returns>
		private string ParseForInlineReferences(String doc)
		{
			string regex = @"\[A#.*?\]";
			string parsedDoc = doc;
			var match = Regex.Match(doc, regex);

			while(match.Success)
			{
				string articleNumber = match.ToString();
				articleNumber = Regex.Replace(articleNumber, @"\[.*?>", "");
				articleNumber = Regex.Replace(articleNumber, @"<.*?\]", "");
				
				Guid articleGuid = new Guid(_scTree.GetArticleGuidByNumber(articleNumber));
				if (articleGuid == Guid.Empty)
				{
					MessageBox.Show(@"Tried to link to article with non-existent article number: " + articleNumber);
					continue;
				}
				if (!_relatedInlineArticles.Contains(articleGuid))
				{
					_relatedInlineArticles.Add(articleGuid);
				}
				parsedDoc = parsedDoc.Remove(match.Index, match.Length);
				string dynUrl = _scTree.GetArticleDynamicUrl(articleNumber);
				string link = "<a href=\"" + dynUrl + "\">" + articleNumber + "</a>";
				parsedDoc = parsedDoc.Insert(match.Index, link);
				match = Regex.Match(parsedDoc, regex);
			}
			return parsedDoc;
		}


		private void AlertConnectionFailed()
		{
			//TODO: Logging
			if (_connectedAtStart)
			{
				MessageBox.Show(@"The Sitecore server could not be contacted. Please check your internet connection. If the problem persists, contact ed.schwehm@velir.com.", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			else
			{
				MessageBox.Show(@"Connection with Sitecore server lost. Please check your internet connection. If the problem persists, contact ed.schwehm@velir.com.", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void uxLogout_Click(object sender, EventArgs e)
		{
			loginControl1.Logout();
			Globals.SitecoreAddin.CloseSitecoreTreeBrowser(Globals.SitecoreAddin.Application.ActiveDocument);
		}

		private void uxArticleInformationIcon_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 0;
		}

		private void uxRelatedArticlesIcon_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 1;
		}

		private void uxSidebarsParentsIcon_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 2;
		}

		private void uxCompaniesIcon_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 3;
		}

		private void uxGeneralTagsIcon_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedIndex = 9;
		}

		private void uxAddAuthor_Click(object sender, EventArgs e)
		{
			var selectedAuthor = (SitecoreServer.StaffStruct)uxSelectAuthor.SelectedItem;
			uxSelectedAuthors.Add(selectedAuthor);
		}

		private void uxAddEditor_Click(object sender, EventArgs e)
		{
			var selectedEditor = (SitecoreServer.StaffStruct)uxSelectEditor.SelectedItem;
			uxSelectedEditors.Add(selectedEditor);
		}

		private void button38_Click(object sender, EventArgs e)
		{
			if (IsCheckedOutByMe)
			{ // checkin
				CheckIn();
			}
			else if (!IsCheckedOut)
			{ //checkout
				CheckOut();
			}
			else
			{ //Should never get here
				throw new ApplicationException("Cannot checkin an artile you haven't checked out or checkout a locked article.");
			}

			SetCheckedOutStatus();
		}

		protected void CheckOut()
		{
			try
			{
				if (_scTree.DoesArticleHaveText(GetArticleNumber()))
				{
					DialogResult dialogResult = MessageBox.Show
						("This article already has some content uploaded. If you choose to check out the article now and later upload, you will overwrite that content. Are you sure you wish to checkout this article?",
						 "Elsevier",
						 MessageBoxButtons.YesNo,
						 MessageBoxIcon.Question);
					if (dialogResult != DialogResult.Yes)
					{
						return;
					}
				}

				GetArticeDetailsFromSitecore();
				bool result = _scServer.CheckOutArticle(GetArticleNumber(), _user.Username);
				if (!result)
				{
					throw new WebException("Error checking in article");
				}
				IsCheckedOutByMe = true;
				IsCheckedOut = true;
			}
			catch (Exception ex)
			{
				AlertConnectionFailed();
				Globals.SitecoreAddin.LogException("Error in article details when checking out article", ex);
			}
		}

		protected void CheckIn()
		{
			try
			{
				SaveChangesAndTransfer();
				bool result = _scServer.CheckInArticle(GetArticleNumber());
				if (!result)
				{
					throw new WebException("Error checking in article");
				}
				IsCheckedOutByMe = false;
				IsCheckedOut = false;
			}
			catch (Exception ex)
			{
				AlertConnectionFailed();
				Globals.SitecoreAddin.LogException("Error in article details when checking in article", ex);
			}
		}

		private void uxLockStatus_Paint(object sender, PaintEventArgs e)
		{
			e.Graphics.DrawRectangle
				(GetLockBorderColor(),
				 e.ClipRectangle.Left,
				 e.ClipRectangle.Top,
				 e.ClipRectangle.Width - 1,
				 e.ClipRectangle.Height - 1);
			base.OnPaint(e);
		}

		protected Pen GetLockBorderColor()
		{
			if (IsCheckedOutByMe)
			{
				return Pens.Green;
			}
			if (IsCheckedOut)
			{
				return Pens.Red;
			}
			return Pens.Black;
		}

		private void uxNext_Click(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex <= tabControl1.TabCount)
			{
				tabControl1.SelectedIndex = tabControl1.SelectedIndex + 1;
			}
		}

		private void uxPrevious_Click(object sender, EventArgs e)
		{
			if (tabControl1.SelectedIndex > 0)
			{
				tabControl1.SelectedIndex = tabControl1.SelectedIndex - 1;
			}
		}

		private void uxRefreshStatus_Click(object sender, EventArgs e)
		{
			SetCheckedOutStatus();
		}

		private void uxLinkArticle_Click(object sender, EventArgs e)
		{
			string articleNumber = uxArticleNumberToLink.Text;
			if (articleNumber.IsNullOrEmpty())
			{
				MessageBox.Show
					("Please enter an article number to link to.", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			try
			{
				bool exists = _scTree.DoesArticleExist(articleNumber);
				if (!exists)
				{
					MessageBox.Show
						("Article number entered does not exist.", "Elsevier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}

				bool hasContent = _scTree.DoesArticleHaveText(articleNumber);
				if (hasContent)
				{
					var result = MessageBox.Show
						("The article number you are linking to has some content already saved. If you link this document to this article and save your content, you will overwrite the existing content." +
						 Environment.NewLine + Environment.NewLine + "Are you sure you want to link this document to this article?",
						 "Elsevier",
						 MessageBoxButtons.YesNo,
						 MessageBoxIcon.Question);
					if (result == DialogResult.No)
					{
						return;
					}
				}

				uxArticleNumber.Text = articleNumber;
				_documentCustomProperties.ArticleNumber = articleNumber;
				this.GetArticeDetailsFromSitecore();
				SetCheckedOutStatus();

			}
			catch (Exception ex)
			{
				//TODO: error handling and logging
				Globals.SitecoreAddin.LogException("Error in article details when linking article", ex);
			}
		}

		private void uxArticlePanel_VisibleChanged(object sender, EventArgs e)
		{
			SetCheckedOutStatus();
		}
	}
}
