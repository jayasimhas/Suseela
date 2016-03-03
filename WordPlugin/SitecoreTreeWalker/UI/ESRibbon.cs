using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI.ArticleDetailsForm;
using InformaSitecoreWord.UI.TreeBrowser.TreeBrowserControls;
using InformaSitecoreWord.User;
using PluginModels;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Tools.Ribbon;
using InformaSitecoreWord.Util;
using InformaSitecoreWord.WebserviceHelper;
using InformaSitecoreWord.Util.Document;


namespace InformaSitecoreWord.UI
{
	public partial class ESRibbon
	{
		SitecoreUser _user = SitecoreUser.GetUser();
		public ArticleStruct ArticleDetails = new ArticleStruct();
		private DocumentCustomProperties _documentCustomProperties;
		private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;
		private WordUtils _wordUtils;
		private SitecoreClient _sitecoreArticle;

		private void ESRibbon_Load(object sender, RibbonUIEventArgs e)
		{
			LogoutBtn.Visible = false;
			LoginBtn.Visible = true;
		}

		private void ShowTree()
		{
			var app = Globals.SitecoreAddin.Application;
			var doc = app.ActiveDocument;
			Globals.SitecoreAddin.ShowTree(doc);
		}

		private void OpenPluginBtn_Click(object sender, RibbonControlEventArgs e)
		{
			CheckLoginAndPerformAction(OpenArticleInformation);
		}

		private void ArticlesBtn_Click(object sender, RibbonControlEventArgs e)
		{
			ArticlesSidebarsControl myUserControl = new ArticlesSidebarsControl();
			CheckLoginAndPerformAction(myUserControl, "Reference Articles");
		}

		private void IntelligenceProductsBtn_Click(object sender, RibbonControlEventArgs e)
		{
			DealsDrugsCompaniesControl myUserControl = new DealsDrugsCompaniesControl();
			CheckLoginAndPerformAction(myUserControl, "Deals and Companies");
		}

		private void Multimedia_Click(object sender, RibbonControlEventArgs e)
		{
			IFrameControl myUserControl = new IFrameControl();
			CheckLoginAndPerformAction(myUserControl, "Multimedia");
		}

		private void ImagesBtn_Click(object sender, RibbonControlEventArgs e)
		{
			GraphicsControl myUserControl = new GraphicsControl();
			CheckLoginAndPerformAction(myUserControl, "Images");
		}

		private void SupportingDocsBtn_Click(object sender, RibbonControlEventArgs e)
		{
			SupportingDocumentsControl myUserControl = new SupportingDocumentsControl();
			CheckLoginAndPerformAction(myUserControl, "Supporting Documents");
		}



		/// <summary>
		/// This is a method which takes in a Function which would be required to be called once the use logs in.
		/// It checks if the user is logged in or not. If not, then it open a Login Dailog box. If logged in then, runs the function.
		/// </summary>
		/// <param name="myAction">A function which needs to be executed</param>
		private void CheckLoginAndPerformAction(Action myAction)
		{
			if (_user.IsLoggedIn)
			{
				Globals.SitecoreAddin.Log("User is logged in, opening the Plugin...");
				myAction();
			}
			else
			{
				Globals.SitecoreAddin.Log("User is not logged in, prompting for password...");
				var login = new LoginWindow();
				login.loginControl1.uxLoginButton.Click +=
					delegate
					{
						if (_user.IsLoggedIn)
						{
							Globals.SitecoreAddin.Log("User has logged in, closing the login screen and showing the tree...");
							login.Close();
							login.Dispose();
							LoginLogoutButtonChange();
							myAction();
						}
					};
				login.ShowDialog();
			}
		}

		/// <summary>
		/// This is a method which takes in a Function which would be required to be called once the use logs in.
		/// It checks if the user is logged in or not. If not, then it open a Login Dailog box. If logged in then, runs the function.
		/// </summary>
		/// <param name="taskControl">the control which needs to be opened</param>
		/// <param name="title">Title of the Task Pane</param>
		private void CheckLoginAndPerformAction(UserControl taskControl, string title)
		{
			if (_user.IsLoggedIn)
			{
				Globals.SitecoreAddin.Log("User is logged in, opening the Plugin...");
				OpenTaskPane(taskControl, title);
			}
			else
			{
				Globals.SitecoreAddin.Log("User is not logged in, prompting for password...");
				var login = new LoginWindow();
				login.loginControl1.uxLoginButton.Click +=
					delegate
					{
						if (_user.IsLoggedIn)
						{
							Globals.SitecoreAddin.Log("User has logged in, closing the login screen and showing the tree...");
							login.Close();
							LoginLogoutButtonChange();
							OpenTaskPane(taskControl, title);
						}
					};
				login.ShowDialog();
			}
		}

		public void OpenTaskPane(UserControl taskControl, string title)
		{
			if (Globals.SitecoreAddin.CustomTaskPanes.Count >= 1)
			{
				var paneCount = Globals.SitecoreAddin.CustomTaskPanes.Count();
				for (var i = 0; i < paneCount; i++)
				{
					Globals.SitecoreAddin.CustomTaskPanes.RemoveAt(i);
				}
			}
			myCustomTaskPane = Globals.SitecoreAddin.CustomTaskPanes.Add(taskControl, title);
			myCustomTaskPane.DockPosition = MsoCTPDockPosition.msoCTPDockPositionRight;
			myCustomTaskPane.Width = 350;
			myCustomTaskPane.Visible = true;
		}

		/// <summary>
		/// The Method would open up and initialze the Article Information Plugin window. You can use this to set Taxonomy items, Article MetaData etc.
		/// </summary>
		private void OpenArticleInformation()
		{
			try
			{
				ArticleDetail.Open();
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("ESRibbon.OpenArticleInformation: Error loading the article information window!", ex);
				MessageBox.Show
					(@"An error has occurred while attempting to display the article information window. Please restart Word and try again." +
					 Environment.NewLine + Environment.NewLine +
					 @"If the problem persists, contact your system administrator.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void GetPreview()
		{
			if (GetArticleNumber() == null)
			{
				MessageBox.Show(@"There is no article linked!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			Process.Start(GetPreviewUrl(false));
		}


		/// <summary>
		/// 
		/// </summary>
		/// <returns>Null if no article number has been set to the document; 
		/// otherwise, the article number set to the document</returns>
		public string GetArticleNumber()
		{
			SitecoreAddin.TagActiveDocument();
			_documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
			ArticleDetails.ArticleNumber = _documentCustomProperties.ArticleNumber;
			return ArticleDetails.ArticleNumber;
		}

		/// <summary>
		/// Sets the member ArticleStruct ArticleDetails to the inputted
		/// ArticleStruct articleStruct
		/// </summary>
		/// <param name="articleStruct"></param>
		public void SetArticleDetails(ArticleStruct articleStruct)
		{
			ArticleDetails = articleStruct;
		}

		public void SetArticleNumber(string articleNumber)
		{
			Globals.SitecoreAddin.Log("Setting article number to #" + articleNumber);
			ArticleDetails.ArticleNumber = articleNumber;
			_documentCustomProperties.ArticleNumber = articleNumber;
		}

		private string GetPreviewUrl(bool isMobile)
		{
			string guid = SitecoreClient.GetArticleGuidByArticleNumber(GetArticleNumber());
			string domain = Constants.EDITOR_ENVIRONMENT_SERVERURL;
			string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
			string redirect = (domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
			return redirect;
		}

		public void TodoMethod()
		{
			var app = Globals.SitecoreAddin.Application;
			var doc = app.ActiveDocument;
			Globals.SitecoreAddin.ShowTree(doc);
		}


		private void LoginButton_Click(object sender, RibbonControlEventArgs e)
		{
			if (_user.IsLoggedIn)
			{
				LoginLogoutButtonChange();
			}
			else
			{
				Globals.SitecoreAddin.Log("User is not logged in, prompting for password...");
				var login = new LoginWindow();
				login.loginControl1.uxLoginButton.Click +=
					delegate
					{
						if (_user.IsLoggedIn)
						{
							Globals.SitecoreAddin.Log(
								"User has logged in, closing the login screen and showing the tree...");
							login.Close();
						}
						LoginLogoutButtonChange();
					};
				login.ShowDialog();
			}
		}


		private void LogoutBtn_Click(object sender, RibbonControlEventArgs e)
		{
			if (!_user.IsLoggedIn) return;
			Globals.SitecoreAddin.Log("User is logged in, opening the Plugin...");
			try
			{
				var loginControl1 = new LoginControl();
				loginControl1.Logout();
				Globals.SitecoreAddin.CloseSitecoreTreeBrowser(Globals.SitecoreAddin.Application.ActiveDocument);
				LoginLogoutButtonChange();
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error while logging out!", ex);
				throw;
			}
		}

		public void LoginLogoutButtonChange()
		{
			LoginBtn.Visible = !LoginBtn.Visible;
			LogoutBtn.Visible = !LogoutBtn.Visible;
		}

		private void ArticlePreviewMenu_Click(object sender, RibbonControlEventArgs e)
		{
			CheckLoginAndPerformAction(GetPreview);
		}

		private void SaveToSitecoreBtn_Click(object sender, RibbonControlEventArgs e)
		{


			//var documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
			//if (!string.IsNullOrEmpty(documentCustomProperties.ArticleNumber))
			//{
			if (GetArticleNumber() != null)
			{
				CheckLoginAndPerformAction(SaveArticleData);
			}
			else
			{
				CheckLoginAndPerformAction(OpenArticleInformation);
			}
		}

		public void SaveArticleData()
		{
			Globals.SitecoreAddin.Log("Save and transferring");
			SuspendLayout();

			SitecoreAddin.ActiveDocument.Saved = false;
			_wordUtils = new WordUtils();
			try
			{
				var metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument, _wordUtils.CharacterStyleTransformer);
				if (PreSavePrompts(metadataParser)) return;
				SaveArticleToSitecoreUpdateUi(metadataParser);
			}
			catch (WebException wex)
			{
				AlertConnectionFailure();
				Globals.SitecoreAddin.LogException("Web connection error when saving and transferring article!", wex);
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error when saving and transferring article!", ex);
				MessageBox.Show(@"Error when saving and transferring article! Error recorded in logs.", @"Informa");
			}
			finally
			{
				ResumeLayout();
			}
			Document activeDocument = SitecoreAddin.ActiveDocument;
			var path = activeDocument.Path;
			if (!activeDocument.ReadOnly && !string.IsNullOrWhiteSpace(path))
			{
				WordUtils.Save(activeDocument);
			}
		}

		private void SaveArticleToSitecoreUpdateUi(ArticleDocumentMetadataParser metadataParser, string body = null)
		{
			_documentCustomProperties.ArticleNumber = GetArticleNumber();
			InvalidStylesHighlighter highlighter = InvalidStylesHighlighter.GetParser();
			bool hasInvalidStyles = highlighter.HighlightAllInvalidStyles(Globals.SitecoreAddin.Application.ActiveDocument);
			if (hasInvalidStyles && !AskContinueInvalidStyle())
			{
				return;
			}

			var saved = SaveArticle(metadataParser, body);

			if (!saved)
			{
				return;
			}
			
			MessageBox.Show(@"Article successfully saved to Sitecore!", @"Informa");			
		}

		/// <summary>
		/// 
		/// 
		/// Be sure to catch WebException indicating server could not be contacted
		/// Saves a locked document (and unlocks the local document)
		/// </summary>
		public bool SaveArticle(ArticleDocumentMetadataParser metadataParser = null, string body = null)
		{
			var documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
			var articleNumber = GetArticleNumber();

			if (!string.IsNullOrEmpty(documentCustomProperties.ArticleNumber) && !string.IsNullOrEmpty(articleNumber))
			{
				if (articleNumber != documentCustomProperties.ArticleNumber)
				{
					string alertMessage = "Article numbers do not match, are you sure you would like to continue? " +
										  "This can happen accidentally if you switching between documents quickly during saving.";
					DialogResult result = MessageBox.Show(alertMessage, "Article numbers do not match",
														  MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

					if (result == DialogResult.No)
					{
						return false;
					}
				}
			}

			try
			{
				if (HasArticleNumber())
				{
					var copy = ArticleDetails.ArticleGuid;
					ArticleDetails = GetArticleDetails(articleNumber,metadataParser);
					ArticleDetails.ArticleGuid = copy;
					_sitecoreArticle = new SitecoreClient();
					//TODO - Add workflow stuff here
					List<string> errors = _sitecoreArticle.SaveArticle(SitecoreAddin.ActiveDocument, ArticleDetails, 
						new Guid(), new List<StaffStruct>(), GetArticleNumber(), body);
					if (errors != null && errors.Any())
					{
						foreach (string error in errors)
						{
							if (!String.IsNullOrEmpty(error))
							{
								MessageBox.Show(error, error.Contains("not secure") ? @" Non-Secure Multimedia Content" : @"Informa");
							}
						}
						return false;
					}					
				}
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error when saving article!", ex);
				throw;
			}

			return true;
		}

		public ArticleStruct GetArticleDetails(string articleNumber, ArticleDocumentMetadataParser metadataParser = null)
		{
			var sitecoreArticleDetails = SitecoreClient.ForceReadArticleDetails(articleNumber);
			if (metadataParser == null)
			{
				metadataParser = new ArticleDocumentMetadataParser(SitecoreAddin.ActiveDocument,_wordUtils.CharacterStyleTransformer);
			}
			string longSummary = metadataParser.LongSummary;
			var articleDetails = new ArticleStruct
			{
				ArticleNumber = sitecoreArticleDetails.ArticleNumber,
				WebPublicationDate = sitecoreArticleDetails.WebPublicationDate,
				Title = metadataParser.Title.Trim(),
				Summary = longSummary,
				Subtitle = metadataParser.Subtitle,
				Publication = sitecoreArticleDetails.Publication,
				Authors = sitecoreArticleDetails.Authors.ToList(),
				Label = sitecoreArticleDetails.Label,
				MediaType = sitecoreArticleDetails.MediaType,
				NotesToEditorial = sitecoreArticleDetails.NotesToEditorial,
				Taxonomoy = sitecoreArticleDetails.Taxonomoy,
				RelatedInlineArticles = sitecoreArticleDetails.RelatedInlineArticles,
				RelatedArticles = sitecoreArticleDetails.RelatedArticles,
				ArticleSpecificNotifications = sitecoreArticleDetails.GlobalNotifications,
				Embargoed = sitecoreArticleDetails.Embargoed,
				FeaturedImageCaption = sitecoreArticleDetails.FeaturedImageCaption,
				FeaturedImageSource = sitecoreArticleDetails.FeaturedImageSource
			};

			//RelatedInlineArticles = sitecoreArticleDetails.RelatedInlineArticles.ToList(),
			//	RelatedArticles = sitecoreArticleDetails.RelatedArticles.ToList(),
			//	ArticleSpecificNotifications = sitecoreArticleDetails.GlobalNotifications.ToList(),

			if (sitecoreArticleDetails.FeaturedImage != new Guid())
			{
				articleDetails.FeaturedImage = sitecoreArticleDetails.FeaturedImage;
			}

			return articleDetails;
		}

		/// <summary>
		/// Should be equivalent to "IsDocumentLinkedToSitecoreArticleItem"
		/// </summary>
		/// <returns></returns>
		private bool HasArticleNumber()
		{
			return (!string.IsNullOrEmpty(_documentCustomProperties.ArticleNumber));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="metadataParser"></param>
		/// <returns>True if user wishes to break</returns>
		private bool PreSavePrompts(ArticleDocumentMetadataParser metadataParser)
		{
			if (string.IsNullOrEmpty(metadataParser.Title))
			{
				MessageBox.Show(@"Please enter an article title.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Warning);
				return true;
			}

			if (NotifyTitleTooLong(metadataParser.Title.Trim().Length))
			{
				return true;
			}

			if (metadataParser.TitleCount > 1)
			{
				if (!AskContinueMultipleTitles())
				{
					return true;
				}
			}
			int maxLengthLongSummary = SitecoreClient.GetMaxLengthLongSummary();
			if (metadataParser.LongSummary.Length > maxLengthLongSummary)
			{
				if (!AskExceededCharacterLimit("Summary", maxLengthLongSummary))
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Presents a prompt if title is too long
		/// </summary>
		/// <param name="titleLength"></param>
		/// <returns>True if titleLength is too long; else false</returns>
		protected bool NotifyTitleTooLong(int titleLength)
		{
			int maxLength = ApplicationConfig.GetTitleMaxCharacters();
			if (titleLength > maxLength)
			{
				MessageBox.Show($"Title is too long! Title character limit is {maxLength}. Current title is {titleLength} characters long!", @"Informa");
				return true;
			}
			return false;
		}

		/// <summary>
		/// Prompts the user to continue even though there are multiple sections styled as "title".
		/// </summary>
		/// <returns>True if the user wishes to continue</returns>
		protected bool AskContinueMultipleTitles()
		{
			DialogResult dialogResult = MessageBox.Show
							(@"There are multiple paragraphs that are styled as Story Title. " +
								@"Do you wish to proceed?",
								@"Informa",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question);
			return (dialogResult == DialogResult.Yes);
		}

		/// <summary>
		/// Prompts the user that they have exceeded a character limit for a field
		/// </summary>
		/// <param name="field"></param>
		/// <param name="characterLimit"></param>
		/// <returns>True if the user wishes to continure</returns>
		protected bool AskExceededCharacterLimit(string field, int characterLimit)
		{
			string message =
				String.Format("You have exceed the character limit of {0} characters for the {1}. " +
							  "The {1} will be truncated. Do you wish to continue?",
							  characterLimit, field);
			DialogResult dialogResult = MessageBox.Show
							(message,
								@"Informa",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question);
			return (dialogResult == DialogResult.Yes);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>True if user wishes to continue</returns>
		protected bool AskContinueInvalidStyle()
		{
			DialogResult dialogResult = MessageBox.Show
				(@"Unmapped styles have been identified in your document. " +
				 @"If you wish to proceed with saving, this text will be " +
				 @"transferred with the default story text style. Do you " +
				 @"wish to proceed?",
				 @"Informa",
				 MessageBoxButtons.YesNo,
				 MessageBoxIcon.Question);
			return (dialogResult == DialogResult.Yes);
		}

		public DialogResult AlertConnectionFailure()
		{
			return MessageBox.Show
				(@"Sitecore server could not be contacted! Please try again in a few minutes." + Environment.NewLine +
				 Environment.NewLine + @"If the problem persists, contact your system administrator.",
				 @"Informa",
				 MessageBoxButtons.OK,
				 MessageBoxIcon.Error);
		}
	}
}
