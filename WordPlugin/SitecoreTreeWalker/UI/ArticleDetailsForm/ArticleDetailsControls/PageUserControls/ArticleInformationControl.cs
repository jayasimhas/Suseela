using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.SitecoreServer;
using SitecoreTreeWalker.SitecoreTree;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.Interfaces;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.Util.Document;
using ArticleStruct = SitecoreTreeWalker.SitecoreTree.ArticleStruct;
using StaffStruct = SitecoreTreeWalker.SitecoreTree.StaffStruct;

namespace SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls.PageUserControls
{
	/// <summary>
	/// Primary control for the Article Information tab
	/// </summary>
	public partial class ArticleInformationControl : ArticleDetailsPageUserControl
	{
		private const string RemoteTimezoneId = "Eastern Standard Time";

		private ArticleDetail _parent;
		protected DocumentCustomProperties _documentCustomProperties;

		private string ArticleNumber;

		protected bool _isLive;

		public ArticleInformationControl()
		{
			InitializeComponent();
		}

        public bool _isCheckedOut;
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

        public bool _isCheckedOutByMe;
		public bool IsCheckedOutByMe
		{
			get { return _isCheckedOutByMe; }
			set
			{
				if (value)
				{
					IsCheckedOut = true;
				}
				_isCheckedOutByMe = value;
			}
		}

		/// <summary>
		/// transfers current material to sitecore
		/// versions it
		/// unlocks the item if current user is the one who has a lock on it
		/// 
		/// Be sure to catch potential WebException
		/// </summary>
		public bool CheckIn(bool save = true)
		{
			bool saved = false;
			try
			{
				//SitecoreAddin.WordApp.ActiveDocument.Saved = true;
				if (save)
				{
					 saved = _parent.SaveArticle();

					if (!saved)
					{
						return false;
					}
				}
				
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when saving article.", ex);
				
				if(ex is WebException)
				{
					throw;
				}
				else
				{
					MessageBox.Show(@"Error in article details when checking in article.", @"Elsevier");
				}
			}
			finally
			{
				Guid articleGuid = _parent.ArticleDetails.ArticleGuid;
				if(articleGuid != Guid.Empty)
				{
					SitecoreArticle.CheckInArticle(articleGuid);
				}
				else
				{
					SitecoreArticle.CheckInArticle(_parent.GetArticleNumber()); 
				}
				IsCheckedOutByMe = false;
				IsCheckedOut = false;
			}
			return saved;
		}

		/// <summary>
		/// Checks out article associated with _parent.ArticleDetails.ArticleGuid
		/// </summary>
		/// <returns></returns>
		public bool CheckOut(bool prompt = false)
		{
			Guid articleGuid = _parent.ArticleDetails.ArticleGuid;
			if (articleGuid == Guid.Empty)
			{
				MessageBox.Show
					(@"No article associated with file.", @"Elsevier",
					 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			try
			{
				bool exists = SitecoreArticle.DoesArticleExist(articleGuid);
				if (!exists)
				{
					MessageBox.Show
						(@"Article no longer exists on Sitecore", @"Elsevier",
						 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				ArticleNumber = _parent.ArticleDetails.ArticleNumber;
                //uxArticleNumberLabel.Text = ArticleNumber;
                

				CheckoutStatus checkedOut = SitecoreArticle.GetLockedStatus(articleGuid);

				if (SitecoreArticle.DoesArticleHaveText(articleGuid) && prompt)
				{
					DialogResult dialogResult = MessageBox.Show
						(@"This article already has some content uploaded. "
							+ @"If you choose to check out the article now and later upload, "
							+ @"you will overwrite that content. "
							+ @"Are you sure you wish to checkout this article?",
							@"Elsevier",
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question);
					if (dialogResult != DialogResult.Yes)
					{
						return false;
					}
				}
				if (!checkedOut.Locked)
				{
					if (_parent.CloseOnSuccessfulLock)
					{
						if (DialogFactory.PromptAutoLock() == DialogResult.Yes)
						{
							SitecoreArticle.CheckOutArticle(articleGuid, SitecoreUser.GetUser().Username); 
						}
					} 
					else
					{
						SitecoreArticle.CheckOutArticle(articleGuid, SitecoreUser.GetUser().Username);
					}
				}
				SetCheckedOutStatus();
				if (_parent.CloseOnSuccessfulLock && IsCheckedOutByMe)
				{
					return true;
				}
				//establish link, regardless of lock status
				_parent.SetArticleDetails(SitecoreGetter.ForceReadArticleDetails(articleGuid));
				_parent.UpdateFields();
				return true;
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when checking out article: " + articleGuid, ex);
				throw;
			}
		}

		/// <summary>
		/// Ascertain valid article number
		/// Link to article
		/// Indicate item is locked
		/// </summary>
		/// <param name="articleNumber"></param>
		/// <param name="prompt">Flag to prompt user if problem occurs</param>
		public bool CheckOut(string articleNumber, bool prompt = true)
		{
			if (articleNumber.IsNullOrEmpty())
			{
				MessageBox.Show
					(@"Please enter an article number to link to.", @"Elsevier", 
					 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			try
			{
				bool exists = SitecoreArticle.DoesArticleExist(articleNumber);
				if (!exists && prompt)
				{
					MessageBox.Show
						(@"Article number entered does not exist.", @"Elsevier",
						 MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return false;
				}
				ArticleNumber = articleNumber;
                //uxArticleNumberLabel.Text = articleNumber;

				CheckoutStatus checkedOut = SitecoreArticle.GetLockedStatus(articleNumber);

				if (!checkedOut.Locked)
				{ //if unlocked, then lock it by current user
					if (SitecoreArticle.DoesArticleHaveText(articleNumber) && prompt)
					{
						DialogResult dialogResult = MessageBox.Show
							(@"This article already has some content uploaded. "
							 + @"If you choose to check out the article now and later upload, "
							 + @"you will overwrite that content. "
							 + @"Are you sure you wish to checkout this article?",
							 @"Elsevier",
							 MessageBoxButtons.YesNo,
							 MessageBoxIcon.Question);
						if (dialogResult != DialogResult.Yes)
						{
							return false;
						}
					}
					SitecoreArticle.CheckOutArticle(articleNumber, SitecoreUser.GetUser().Username);
				}
				SetCheckedOutStatus();
				if (_parent.CloseOnSuccessfulLock && IsCheckedOutByMe) return true;
				//establish link, regardless of lock status
				_parent.SetArticleDetails(SitecoreGetter.ForceReadArticleDetails(articleNumber));
				_parent.UpdateFields();
				return true;
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Error in article details when checking out article: [" + articleNumber + "]", ex);
				throw;
			}
		}

		/// <summary>
		/// Get from sitecore checked out status, then set control accordingly
		/// 
		/// Unprotects document if locked by current user
		/// </summary>
		public void SetCheckedOutStatus()
		{
			if(_parent == null || _parent.ArticleDetails == null)
			{
				return;
			}
			_documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
			string articleNum = _parent.GetArticleNumber();
			if (!articleNum.IsNullOrEmpty())
			{ //document is linked to an article
				SetArticleNumber(articleNum);
				CheckoutStatus checkedOut;
				if (_parent.ArticleDetails.ArticleGuid != Guid.Empty)
				{
					checkedOut = SitecoreArticle.GetLockedStatus(_parent.ArticleDetails.ArticleGuid); 
				}
				else
				{
					checkedOut = SitecoreArticle.GetLockedStatus(articleNum);
				}
				IsCheckedOut = checkedOut.Locked;
				if (IsCheckedOut)
				{
					if (SitecoreUser.GetUser().Username == checkedOut.User)
					{ //locked by me
						
						IndicateCheckedOutByMe(checkedOut);
					}
					else
					{ //locked by other
						IndicateCheckedOutByOther(checkedOut);
					}
                    //uxLockStatusLabel.Text = @"Locked";
				}
				else
				{ //unlocked
					IndicateUnlocked();
				}
                //uxRefreshStatus.Enabled = true;
			}
			else
			{ //document is not linked to an article
				DocumentProtection.Unprotect(_documentCustomProperties);
				IsCheckedOutByMe = false;
				IsCheckedOut = false;

				_parent.PreLinkEnable();
			}
		}

		private void IndicateUnlocked()
		{
			_parent.PreLinkEnable();
			IndicatedUnfavoredLink();

            //uxLockStatus.BackColor = DefaultBackColor;
            //uxLockUser.Text = @"N\A";
            //uxLockStatusLabel.Text = @"Unlocked";

			IsCheckedOutByMe = false;
			IsCheckedOut = false;

            //uxUnlockButton.Visible = false;
            //uxLockButton.Visible = true;
            //uxLockButton.Enabled = true;
			DocumentProtection.Protect(_documentCustomProperties);
		}

		public void IndicatedUnfavoredLink()
		{
            //uxLinkToDocumentPanel.Visible = false;
            //uxLockStatus.Visible = true;
            //uxVersionStatus.Visible = true;
            uxPublication.Enabled = false;
            //_parent.EnablePreview();
            //_parent.HideCreationButtons();

			uxSelectAuthor.Enabled = false;
			//uxSelectedAuthors.Enabled = false;
			//uxSelectedAuthors.DisableEdit = true;
			uxAddAuthor.Enabled = false;
		}

		/// <summary>
		/// Enables/disables some controls since it's so similar to a PreLinkEnable state
		/// </summary>
		/// <param name="checkedOut"></param>
        public void IndicateCheckedOutByOther(CheckoutStatus checkedOut)
		{
			//uxLockStatus.BackColor = Color.FromArgb(255, 244, 204, 204);

            //uxLockUser.Text = FormatUserName(checkedOut.User);

			IsCheckedOutByMe = false;

			_parent.PreLinkEnable();

			IndicatedUnfavoredLink();
            //uxUnlockButton.Visible = true;
            //uxUnlockButton.Enabled = false;
            //uxLockButton.Visible = false;
			DocumentProtection.Protect(_documentCustomProperties);
		}

		public void IndicateCheckedOutByMe(CheckoutStatus checkedOut)
		{
			DocumentProtection.Unprotect(_documentCustomProperties);
			IsCheckedOutByMe = true;
			if (_parent.CloseOnSuccessfulLock && CheckWordDocVersion(_parent.ArticleDetails, false))
			{
				_parent.Close();
				return;
			}
			_parent.CloseOnSuccessfulLock = false;
            //uxLockStatus.BackColor = Color.FromArgb(255, 217, 234, 211);

            //uxLockUser.Text = FormatUserName(checkedOut.User);
			_parent.PostLinkEnable();
            //uxUnlockButton.Visible = true;
            //uxLockButton.Visible = false;
            //uxUnlockButton.Enabled = true;
		}

		public void InitializePublications()
		{
			List<ItemStruct> publications = SitecoreGetter.GetPublications();
			publications.Insert(0, new ItemStruct { ID = Guid.Empty, Name = "Select Publication" });
			uxPublication.DataSource = publications;
			uxPublication.DisplayMember = "Name";
			uxPublication.ValueMember = "ID";
		}

		public void InitializeMediaType()
		{
			List<ItemStruct> mediaTypes = SitecoreGetter.GetMediaTypes();
			mediaTypes.Insert(0, new ItemStruct { ID = Guid.Empty, Name = "Select Media Type" });
			uxMediaTypes.DataSource = mediaTypes;
			uxMediaTypes.DisplayMember = "Name";
			uxMediaTypes.ValueMember = "ID";
		}

		public void LinkToParent(ArticleDetail parent)
		{
			_parent = parent;
		}

		/// <summary>
		/// Returns Guid of selected publication, or an empty Guid of none selected
		/// </summary>
		/// <returns></returns>
		public Guid GetSelectedPublicationGuid()
		{
			try
			{
				if (uxPublication.SelectedValue.GetType() == typeof(ItemStruct))
				{
					return ((ItemStruct) uxPublication.SelectedValue).ID;
				}
				return (Guid) uxPublication.SelectedValue;
			}
			catch
			{
				return Guid.Empty;
			}
		}


		public string GetDisplayName(ArticleSize articleSize, int wordCount)
		{
			string displayName = articleSize.Name;
			if (articleSize.MinimumWordCount < 0 && articleSize.MaximumWordCount > 0)
			{
				displayName += " (<" + articleSize.MaximumWordCount + " words)";
				if (wordCount < articleSize.MaximumWordCount)
				{
					displayName += " (recommended)";
				}
			}
			else if (articleSize.MinimumWordCount > 0 && articleSize.MaximumWordCount > 0)
			{
				displayName += " (" + articleSize.MinimumWordCount + "-" + articleSize.MaximumWordCount + " words)";
				if (wordCount < articleSize.MaximumWordCount && wordCount > articleSize.MinimumWordCount)
				{
					displayName += " (recommended)";
				}
			}
			else if (articleSize.MinimumWordCount > 0 && articleSize.MaximumWordCount < 0)
			{
				displayName += " (>" + articleSize.MinimumWordCount + " words)";
				if (wordCount > articleSize.MinimumWordCount)
				{
					displayName += " (recommended)";
				}
			}

			return displayName;
		}

		public string GetSelectedPublicationName()
		{
			return ((ItemStruct)uxPublication.SelectedItem).Name;
		}

		public List<StaffStruct> GetSelectedAuthors()
		{
			return uxSelectedAuthors.Selected;
		}

		public List<StaffStruct> GetSelectedNotifyees()
		{
			//TODO: Implement the UI for this and then this code too!
			return new List<StaffStruct>();
		}

		/// <summary>
		/// Gets the web publish date (as a combination of date and time).
		/// </summary>
		/// <returns></returns>
		public DateTime GetWebPublishDate()
		{
			var localDate = uxWebPublishDate.Value.Date.Add(uxWebPublishTime.Value.TimeOfDay);
			
			TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(RemoteTimezoneId);

			Globals.SitecoreAddin.Log("GetWebPublishDate: Date before DateTime conversion: [" +
									  localDate.Date.ToString() + "].");
			Globals.SitecoreAddin.Log("GetWebPublishDate: Time before DateTime conversion:  [" +
									  localDate.TimeOfDay.ToString() + "].");

			DateTime convertedTime = TimeZoneInfo.ConvertTime(localDate, easternZone);

			Globals.SitecoreAddin.Log("GetWebPublishDate: Date after DateTime conversion: [" +
									  convertedTime.Date.ToString() + "].");
			Globals.SitecoreAddin.Log("GetWebPublishDate: Time after DateTime conversion:  [" +
									  convertedTime.TimeOfDay.ToString() + "].");

			return convertedTime;
		}

		/// <summary>
		/// Gets the web publish date.
		/// </summary>
		public string GetProperDate()
		{
			return GetWebPublishDate().ToString();
		}

		public string GetArticleNumber()
		{
			return ArticleNumber;
		}

		public bool GetEmbargoedState()
		{
			return uxEmbargoed.Checked;
		}

		public void SetArticleVersionInformation(ArticleStruct articleStruct, bool outOfDate)
		{
			if (outOfDate)
			{
				//uxVersionStatus.BackColor = Color.FromArgb(255, 244, 204, 204);
                _parent.articleStatusBar1.uxVersionStateButton.BackColor = Color.FromArgb(255, 244, 204, 204);                
				//uxVersionText.Text = @"More Recent Version Available";
				//uxVersionText.Font = new Font(uxVersionText.Font, FontStyle.Italic | FontStyle.Bold);
								
			}
			else
			{
				//uxVersionStatus.BackColor = Color.FromArgb(255, 217, 234, 211);
				//uxVersionText.Text = @"Document Content is Up To Date";
				//uxVersionText.Font = new Font(uxVersionText.Font, FontStyle.Bold);
			}

            //uxLastUpdateDate.Text = articleStruct.WordDocLastUpdateDate;
            //uxLastUpdatedBy.Text = FormatUserName(articleStruct.WordDocLastUpdatedBy);
		}

		public void UpdateFields(ArticleStruct articleDetails)
		{
			InitializePublications();
			InitializeMediaType();
			SetCheckedOutStatus();
			if (string.IsNullOrEmpty(articleDetails.ArticleNumber))
			{
				return;
			}
			uxPublication.SelectedValue = articleDetails.Publication;			
			if (articleDetails.Authors != null)
			{
				uxSelectedAuthors.PopulateRegular(articleDetails.Authors.Select(r =>
																		 new StaffStruct
																			{
																				ID = r.ID,
																				Name = r.Name,
																				Publications = r.Publications
																			}).ToList()); 
			}

            //uxArticleNumberLabel.Text = articleDetails.ArticleNumber;
			if (articleDetails.WebPublicationDate > DateTime.MinValue)
			{
				SetPublicationTime(articleDetails.WebPublicationDate, false);
			}
			
			ArticleNumber = articleDetails.ArticleNumber;

			uxEmbargoed.Checked = articleDetails.Embargoed;

			CheckWordDocVersion(articleDetails);

			_isLive = articleDetails.IsPublished;
			label1.Refresh();
		}

		public void SetPublicationTime(DateTime publicationDate, bool isLocal)
		{
			TimeZoneInfo.ClearCachedData();

			if (isLocal)
			{
				Globals.SitecoreAddin.Log("SetPublicationTime: Setting date field to [" + publicationDate.Date.ToString() + "].");
				uxWebPublishDate.Value = publicationDate.Date;
				Globals.SitecoreAddin.Log("SetPublicationTime: Setting time field to [" + publicationDate.TimeOfDay.ToString() + "].");
				uxWebPublishTime.Value = publicationDate;
			}
			else
			{
				const string easternZoneId = "Eastern Standard Time";

				TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById(easternZoneId);

				Globals.SitecoreAddin.Log("SetPublicationTime: Date before DateTime conversion: [" +
										  publicationDate.Date.ToString() + "].");
				Globals.SitecoreAddin.Log("SetPublicationTime: Time before DateTime conversion:  [" +
										  publicationDate.TimeOfDay.ToString() + "].");

				DateTime convertedTime = TimeZoneInfo.ConvertTime(publicationDate, easternZone, TimeZoneInfo.Local);

				Globals.SitecoreAddin.Log("SetPublicationTime: Date after DateTime conversion: [" +
										  convertedTime.Date.ToString() + "].");
				Globals.SitecoreAddin.Log("SetPublicationTime: Time after DateTime conversion:  [" +
										  convertedTime.TimeOfDay.ToString() + "].");

				uxWebPublishDate.Value = convertedTime.Date;
				uxWebPublishTime.Value = convertedTime;

			}
		}

		public void UpdateArticleNumber(string articleNumber)
		{
            //uxArticleNumberLabel.Text = articleNumber;
		}

		/// <summary>
		/// Updates the list of authors according to the selected publication
		/// If no publication is selected, all authors are available.
		/// </summary>
		public void UpdateAuthorsList()
		{
			var matchingAuthors = SitecoreGetter.LazyReadAuthors();
			uxSelectAuthor.DataSource = null;

			if (matchingAuthors.Count == 0 || (matchingAuthors.Count == 1 && matchingAuthors[0].ID == Guid.Empty))
			{

				if (matchingAuthors.Count == 0)
				{
					matchingAuthors.Add(new StaffStruct
											{
												ID = Guid.Empty,
												Name = "No authors found!"
											}); 
				}
			}

			uxSelectAuthor.DataSource = matchingAuthors;
			uxSelectAuthor.DisplayMember = "Name";
			uxSelectAuthor.ValueMember = "ID";
		}

		public void DisableAuthorControlsIfNoAuthors()
		{
			if (uxSelectAuthor.Items.Count == 0 || (uxSelectAuthor.Items.Count == 1 && ((StaffStruct)uxSelectAuthor.Items[0]).ID == Guid.Empty))
			{
				uxSelectAuthor.Enabled = false;
				uxAddAuthor.Enabled = false;
			}
		}

		/// <summary>
		/// Enables and disables the controls necessary for editing once the document
		/// has been linked to a Sitecore article
		/// </summary>
		public void PostLinkEnable()
		{
			uxPublication.Enabled = false;
            //uxLinkDocument.Enabled = false;
			uxSelectAuthor.Enabled = true;
			uxSelectedAuthors.Enabled = true;
			DisableAuthorControlsIfNoAuthors();
			uxSelectedAuthors.DisableEdit = false;
			uxAddAuthor.Enabled = true;

            //uxLinkToDocumentPanel.Visible = false;
            //uxLockStatus.Visible = true;
			//uxVersionStatus.Visible = true;

            //uxUnlinkDocument.Visible = true;
		}

		/// <summary>
		/// Disables and disables the controls necessary for editing once the document
		/// has not been linked to a Sitecore article
		/// </summary>
		public void PreLinkEnable()
		{
			uxPublication.Enabled = true;
            //uxLinkDocument.Enabled = true;
			uxNominate.Enabled = false;
			uxTopStory.Enabled = false;

			uxSelectAuthor.Enabled = true;
			uxAddAuthor.Enabled = true;
			DisableAuthorControlsIfNoAuthors();
			uxSelectedAuthors.Enabled = true;
			uxSelectedAuthors.DisableEdit = false;


            //uxLinkToDocumentPanel.Visible = true;
            //uxLockStatus.Visible = false;
			//uxVersionStatus.Visible = false;

            //uxUnlinkDocument.Visible = false;
		}

		public void ResetFields()
		{
			uxPublication.SelectedIndex = 0;
			uxSelectedAuthors.Reset();
			uxNominate.Checked = false;
			uxTopStory.Checked = false;
			SetPublicationTime(DateTime.Today, true);
			MenuItem.SetIndicatorIcon(Properties.Resources.redx);
		}

		public void IndicateChanged()
		{
			if(MenuItem != null)
			{
				MenuItem.HasChanged = true;
				MenuItem.UpdateBackground();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns>True if local document is up-to-date; else false</returns>
		public bool CheckWordDocVersion()
		{
			return CheckWordDocVersion(SitecoreGetter.ForceReadArticleDetails(GetArticleNumber()));
		}

        
		/// <summary>
		/// 
		/// </summary>
		/// <param name="articleStruct"></param>
		/// <param name="prompt"></param>
		/// <returns>True if local document is up-to-date; else false</returns>
		public bool CheckWordDocVersion(ArticleStruct articleStruct, bool prompt = true)
		{
			int localWordDocVersion = _documentCustomProperties.WordSitecoreVersionNumber;

			int sitecoreWordDocVersion = articleStruct.WordDocVersionNumber;
			SetArticleVersionInformation(articleStruct, (sitecoreWordDocVersion > localWordDocVersion));
			if ((sitecoreWordDocVersion > localWordDocVersion))
			{ //we're out of date!
				if (prompt)
				{
					string message =
								"A newer version of this article  was uploaded on {0} by {1}. " +
								"If you continue using this version, you may overwrite more recent " +
								"changes. To get the most recent version, navigate to Sitecore to download the new document.";

					MessageBox.Show
						(String.Format(message, articleStruct.WordDocLastUpdateDate, articleStruct.WordDocLastUpdatedBy),
						 @"Elsevier",
						 MessageBoxButtons.OK,
						 MessageBoxIcon.Exclamation); 
				}
				return false;
			}
			return true;
		}

		public void SetArticleNumber(string articleNumber)
		{
			_parent.SetArticleNumber(articleNumber);
			ArticleNumber = articleNumber;
            //uxArticleNumberLabel.Text = articleNumber;
		}


		public string FormatUserName(string userNameWithDomain)
		{
			if (userNameWithDomain.IsNullOrEmpty())
			{ return userNameWithDomain; }

			string formattedUserName = userNameWithDomain;

			if (formattedUserName.IndexOf("\\") > 0)
			{
				formattedUserName = formattedUserName.Substring(formattedUserName.IndexOf("\\")+1);
			}

			return formattedUserName;
		}

		private void panel2_Paint(object sender, PaintEventArgs e)
		{
			/*var borderColor = Color.FromArgb(222, 231, 238);
			ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor, ButtonBorderStyle.Solid);*/
		}

		private void uxRefreshStatus_Click(object sender, EventArgs e)
		{
			SetCheckedOutStatus();
		}

		private void uxRefreshVersion_Click(object sender, EventArgs e)
		{
			//CheckWordDocVersion();
		}

		private void uxLockButton_Click(object sender, EventArgs e)
		{
            /*
			if (!SitecoreArticle.DoesArticleExist(uxArticleNumberLabel.Text)) return;
			if(SitecoreArticle.CheckOutArticle(uxArticleNumberLabel.Text, SitecoreUser.GetUser().Username))
			{
				CheckWordDocVersion(_parent.ArticleDetails);
			}
			SetCheckedOutStatus();
             * */
		}

        /*
		private void uxUnlockButton_Click(object sender, EventArgs e)
		{
			if (!SitecoreArticle.DoesArticleExist(uxArticleNumberLabel.Text)) return;
			SitecoreArticle.CheckInArticle(uxArticleNumberLabel.Text);
			SetCheckedOutStatus();
		}
         * */

		private void uxAddAuthor_Click(object sender, EventArgs e)
		{
			var selectedAuthor = (StaffStruct)uxSelectAuthor.SelectedItem;
			uxSelectedAuthors.Add(selectedAuthor);
			IndicateChanged();
		}

        /*
		private void uxLinkDocument_Click(object sender, EventArgs e)
		{
			if (CheckOut(uxArticleNumberToLink.Text, true))
			{
				_parent.ResetChangedStatus();
			}
		}
         * */

		private void uxUnlinkDocument_Click(object sender, EventArgs e)
		{
			if (_isCheckedOutByMe)
			{
				CheckIn(false);
			}
            //uxArticleNumberLabel.Text = @"Document Not Linked";
			DocumentPropertyEditor.Clear(SitecoreAddin.ActiveDocument);
			_parent.PreLinkEnable();
			_parent.SetArticleNumber(null);
			_parent.UnlinkWordFileFromSitecoreItem();
			_parent.ResetFields();
			_parent.ResetChangedStatus(true); //hack-ish to reset all fields
		}

		private void uxPublication_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateAuthorsList();
			IndicateChanged();
		}

		private void uxArticleCategory_SelectedIndexChanged(object sender, EventArgs e)
		{
			IndicateChanged();
		}

		private void uxWebPublishDate_ValueChanged(object sender, EventArgs e)
		{
			IndicateChanged();
		}

		private void ArticleInformationControl_Load(object sender, EventArgs e)
		{
			if (!DesignMode)
			{
				var wordApp = SitecoreAddin.ActiveDocument.Application;
				if (wordApp == null) return;
				// "ComputeStatistics" dirties the document. We don't want that.
				bool currentSavedState = SitecoreAddin.ActiveDocument.Saved;

				//uxWordCount.Text = wordApp.ActiveDocument.ComputeStatistics(Microsoft.Office.Interop.Word.WdStatistic.wdStatisticWords).ToString();

				SitecoreAddin.ActiveDocument.Saved = currentSavedState;

				_documentCustomProperties = new DocumentCustomProperties(SitecoreAddin.ActiveDocument);
			}
		}

		private void uxLinkToDocumentPanel_Paint(object sender, PaintEventArgs e)
		{
			var borderColor = Color.FromArgb(222, 231, 238);
			ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor, ButtonBorderStyle.Solid);
		}

		private void label1_Paint(object sender, PaintEventArgs e)
		{
			if (_isLive)
			{
				e.Graphics.DrawImage(Properties.Resources.live, 570, 1, 28, 28);
				e.Graphics.DrawString("Published!", new Font("SegoeUI", 18), Brushes.Green, 450, 1);
			}
		}

        private void uxArticleNumberToLink_TextChanged(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        public ArticleInformationControl GetObject()
        {
            return this;
        }

        private void uxLockUser_Click(object sender, EventArgs e)
        {

        }

        private void label88_Click(object sender, EventArgs e)
        {

        }

        private void uxLockStatusLabel_Click(object sender, EventArgs e)
        {

        }
	}
}
