using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Informa.Web.Areas.Account.Models;
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


namespace SitecoreTreeWalker.UI
{
    public partial class ESRibbon
    {
        SitecoreUser _user = SitecoreUser.GetUser();
        public WordPluginModel.ArticleStruct ArticleDetails = new WordPluginModel.ArticleStruct();
        private DocumentCustomProperties _documentCustomProperties;
        private Microsoft.Office.Tools.CustomTaskPane myCustomTaskPane;

        private void ESRibbon_Load(object sender, RibbonUIEventArgs e)
        {
			LogoutBtn.Visible = false;
            LoginBtn.Visible = true;
        }

        private void uxShowTree_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (_user.IsLoggedIn)
                {
                    Globals.SitecoreAddin.Log("User is logged in, showing the tree...");
                    ShowTree();
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
                                ShowTree();
                            }
                        };
                    login.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when showing the tree browser!", ex);
                MessageBox.Show
                    (@"An error has occurred while attempting to display the Sitecore browser tab. Please restart Word and try again." +
                     Environment.NewLine + Environment.NewLine +
                     @"If the problem persists, contact your system administrator.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void uxArticleDetails_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                ArticleDetail.Open();
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("ESRibbon.uxArticleDetails_Click: Error loading the article information window!", ex);
                MessageBox.Show
                    (@"An error has occurred while attempting to display the article information window. Please restart Word and try again." +
                     Environment.NewLine + Environment.NewLine +
                     @"If the problem persists, contact your system administrator.", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void PreviewArticleBtn_Click(object sender, RibbonControlEventArgs e)
        {
            CheckLoginAndPerformAction(GetPreview);
        }

        private void PreviewMobileArticleBtn_Click(object sender, RibbonControlEventArgs e)
        {
            CheckLoginAndPerformAction(GetMobilePreview);
        }
        private void SaveToSitecoreBtn_Click(object sender, RibbonControlEventArgs e)
        {
            CheckLoginAndPerformAction(TodoMethod);
        }

        private void ArticlePreviewBtn_Click(object sender, RibbonControlEventArgs e)
        {
            CheckLoginAndPerformAction(TodoMethod);
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

        private void GetPreview()
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            Process.Start(GetPreviewUrl(false));
        }


        private void GetMobilePreview()
        {
            if (GetArticleNumber() == null)
            {
                MessageBox.Show(@"There is no article linked!", @"Informa", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            Process.Start(GetPreviewUrl(true));
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
        public void SetArticleDetails(WordPluginModel.ArticleStruct articleStruct)
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
            string guid = SitecoreArticle.GetArticleGuidByArticleNumber(GetArticleNumber());
            string domain = ApplicationConfig.GetPropertyValue("DomainName");
            string mobileUrlParam = isMobile ? "&mobile=1" : String.Empty;
            string redirect = Uri.EscapeDataString(domain + @"?sc_itemid={" + guid + @"}&sc_mode=preview&sc_lang=en" + mobileUrlParam);
            return domain + @"Util/LoginRedirectToPreview.aspx?redirect=" + redirect;

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
    }
}
