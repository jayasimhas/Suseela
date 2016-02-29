using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PluginModels;
using Microsoft.Office.Core;
using Microsoft.Office.Tools.Ribbon;
using SitecoreTreeWalker.Config;
using SitecoreTreeWalker.document;
using SitecoreTreeWalker.Sitecore;
using SitecoreTreeWalker.UI.ArticleDetailsForm;
using SitecoreTreeWalker.UI.TreeBrowser.TreeBrowserControls;
using SitecoreTreeWalker.Util;
using SitecoreTreeWalker.User;
using SitecoreTreeWalker.WebserviceHelper;
using SitecoreTreeWalker.Util.Document;
using SitecoreTreeWalker.UI.ArticleDetailsForm.ArticleDetailsControls;
using System.Net;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.UI
{
    public partial class ESRibbon
    {
        SitecoreUser _user = SitecoreUser.GetUser();
        public ArticleStruct ArticleDetails = new ArticleStruct();
        private DocumentCustomProperties _documentCustomProperties;
        private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;

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
            string guid = ArticleDetails.ArticleGuid.ToString();// SitecoreArticle.GetArticleGuidByArticleNumber(GetArticleNumber());
            string domain = ApplicationConfig.GetPropertyValue("DomainName");
            if (domain.StartsWith("http") == false)
                domain = "http://" + domain;
            return domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en";
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

        private void SaveArticleBtn_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //This refills ActiveDocument data with the actually active word document
                SitecoreAddin.TagActiveDocument();

                //If the word document has not been linked to sitecore, then login and create the document
                if (SitecoreAddin.ActiveDocument == null || string.IsNullOrEmpty(new DocumentCustomProperties(SitecoreAddin.ActiveDocument).ArticleNumber))
                {
                    CheckLoginAndPerformAction(OpenArticleInformation);
                }
                else
                {//else save the document
                    ArticleDocumentManager articleMgr = new ArticleDocumentManager();

                    articleMgr.SaveDocument();

                    MessageBox.Show("Article successfully saved to Sitecore", Constants.MESSAGEBOX_TITLE);
                }
            }
            catch (WebException wex)
            {
                Globals.SitecoreAddin.LogException("Web connection error when saving metadata!", wex);
                MessageBox.Show("Could not connect to Sitecore");
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.Log("Error while saving document: " + ex.ToString());
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void ArticlePreviewMenu_Click(object sender, RibbonControlEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //This refills ActiveDocument data with the actually active word document
                SitecoreAddin.TagActiveDocument();

                Action previewAction = () =>
                {
                    new ArticleDocumentManager().PreviewArticle();
                };

                CheckLoginAndPerformAction(previewAction);
            }
            catch (WebException wex)
            {
                Globals.SitecoreAddin.LogException("Web connection error when saving metadata!", wex);
                MessageBox.Show("Could not connect to Sitecore");
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error Previewing article", ex);
                MessageBox.Show(@"Error when saving article. Error recorded in logs.", Constants.MESSAGEBOX_TITLE);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }

        private void SaveMetaDataBtn_Click(object sender, RibbonControlEventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //This refills ActiveDocument data with the actually active word document
                SitecoreAddin.TagActiveDocument();

                //If the word document has not been linked to sitecore, then login and create the document
                if (SitecoreAddin.ActiveDocument == null || string.IsNullOrEmpty(new DocumentCustomProperties(SitecoreAddin.ActiveDocument).ArticleNumber))
                {
                    CheckLoginAndPerformAction(OpenArticleInformation);
                }
                else
                {//else save the document
                    ArticleDocumentManager articleMgr = new ArticleDocumentManager();

                    articleMgr.SaveMetaData();
                }

                MessageBox.Show("Metadata saved", Constants.MESSAGEBOX_TITLE);
            }
            catch (WebException wex)
            {
                Globals.SitecoreAddin.LogException("Web connection error when saving metadata!", wex);
                MessageBox.Show("Could not connect to Sitecore");
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when saving meta data!", ex);
                MessageBox.Show(@"Error when saving metadata! Error recorded in logs.", Constants.MESSAGEBOX_TITLE);
            }
            finally
            {
                ResumeLayout();
                Cursor.Current = Cursors.Default;
            }
        }
    }
}
