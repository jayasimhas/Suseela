using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using InformaSitecoreWord.Config;
using InformaSitecoreWord.document;
using InformaSitecoreWord.Sitecore;
using InformaSitecoreWord.UI;
using InformaSitecoreWord.UI.ArticleDetailsForm;
using InformaSitecoreWord.UI.TreeBrowser;
using InformaSitecoreWord.User;
using PluginModels;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Word;
using InformaSitecoreWord.Util;
using Word = Microsoft.Office.Interop.Word;
using StaffStruct = PluginModels.StaffStruct;

namespace InformaSitecoreWord
{
    public partial class SitecoreAddin
    {
        private TreeBrowser _browser;
        private Microsoft.Office.Tools.CustomTaskPane _myCustomTaskPane;
        protected StreamWriter _lameLogger;
        public string SupportEmailAddress { get; set; }

        /// <summary>
        /// Using this handler to access active document on startup
        /// </summary>
        private Word.ApplicationEvents4_DocumentChangeEventHandler _startup;
        private Word.ApplicationEvents4_DocumentBeforeCloseEventHandler _beforeCloseHandler;

        private static Word.Application _wordApp;

        public DialogResult AlertConnectionFailure()
        {
            return MessageBox.Show(@"Sitecore server could not be contacted!", @"Informa");
        }

        public DialogResult Alert(string alert)
        {
            return MessageBox.Show(alert, @"Informa");
        }

        public static Document ActiveDocument { get; private set; }

        public static void TagActiveDocument()
        {
            ActiveDocument = _wordApp.ActiveDocument;
        }

        /// <summary>
        /// Checks the Word version number to see if Word is 2010 or above
        /// </summary>
        /// <returns>Is this Word 2010 or newer?</returns>
        /// <remarks>If this method can't get the version number, it will default to false.</remarks>
        public bool Is2010OrAbove()
        {
            double version;
            if (!Double.TryParse(Globals.SitecoreAddin.Application.Version.ToString(), out version))
            {
                return false;
            }
            //version 12.0 is 2007; 14.0 is 2010
            Log("\t\t\tVersion:" + version);
            return version >= 14.0;
        }

        public static string GetPreviewUrlPrefix()
        {
            return ApplicationConfig.GetPropertyValue("DomainName") + @"Util/LoginRedirectToPreview.aspx?redirect=";
        }

        public void CloseSitecoreTreeBrowser(Word.Document doc)
        {
            var thesePanes = this.CustomTaskPanes.Where(c => c.Window == doc.ActiveWindow).AsParallel().ToList();
            thesePanes.ForEach(c => c.Visible = false);
        }

        private void ThisAddIn_Startup(object sender, System.EventArgs e)
        {
            try
            {
                InitializeLogger();
            }
            catch
            { // we want to silently fail here. Logging won't work, but at least it won't bring down the app.
            }

            try
            {
                SupportEmailAddress = SitecoreClient.GetContactEmail();
            }
            catch (Exception ex)
            {
                SupportEmailAddress = "";
                this.LogException("Unable to retrieve email for support!", ex);
            }

            try
            {
                InitializePlugin();
                _startup = new Word.ApplicationEvents4_DocumentChangeEventHandler(Application_DocumentChange);
                _beforeCloseHandler = new ApplicationEvents4_DocumentBeforeCloseEventHandler(Application_BeforeDocumentClose);

                Application.DocumentChange += _startup;
                Application.DocumentOpen += OpenArticleInformationWindowIfNeeded;
                Application.DocumentBeforeClose += _beforeCloseHandler;
                Application.DocumentChange += Application_DocumentChange_UpdateRibbon;
            }
            catch (Exception ex)
            {
                Log("Exception occurred during startup: " + ex.Message);
                Log("stack trace: " + ex.StackTrace);
            }

        }

        public delegate void SelectedWordDocumentChangedDelegate();
        public event SelectedWordDocumentChangedDelegate SelectedWordDocumentChanged;
        private void Application_DocumentChange_UpdateRibbon()
        {
            if (SelectedWordDocumentChanged != null)
                SelectedWordDocumentChanged.Invoke();
        }

        void Application_DocumentChange()
        {
            Application.DocumentChange -= _startup;
            OpenArticleInformationWindowIfNeeded(Application.ActiveDocument);
            Application.ActiveDocument.Saved = true;
        }

        void Application_BeforeDocumentClose(Word.Document doc, ref bool cancel)
        {
            var documentCustomProps = new DocumentCustomProperties(doc);
            Application.DocumentChange -= Application_DocumentChange_UpdateRibbon;

            if (documentCustomProps.ArticleNumber.IsNullOrEmpty())
            { // If this isn't an elsevier article, default to the normal process
                return;
            }

            if (!doc.Saved)
            {
                var dialog = new SaveDialog();
                dialog.ShowDialog();

                switch (dialog.SelectedChoice)
                {
                    case SaveDialog.SaveChoice.SaveToSitecoreAndUnlock:
                        // at this point, there can be no metadata changes, only body text changes.
                        var _sitecoreClient = new SitecoreClient();
                        var errors = _sitecoreClient.SaveArticle(doc, SitecoreClient.ForceReadArticleDetails(documentCustomProps.ArticleNumber),
                            Guid.Empty, new List<StaffStruct>(), documentCustomProps.ArticleNumber);

                        if (errors.Count > 0)
                        {
                            MessageBox.Show
                                (@"There was an error saving the document to Sitecore. Please try again." + Environment.NewLine +
                                 Environment.NewLine + @"If the problem persists, contact your system administrator.",
                                 @"Informa",
                                 MessageBoxButtons.OK,
                                 MessageBoxIcon.Error);
                            cancel = true;
                        }
                        else
                        {
                            doc.Saved = true;
                            SitecoreClient.CheckInArticle(documentCustomProps.ArticleNumber);
                        }
                        break;
                    case SaveDialog.SaveChoice.SaveLocal:
                        doc.Save();
                        break;
                    case SaveDialog.SaveChoice.Cancel:
                        cancel = true;
                        break;
                    case SaveDialog.SaveChoice.DontSave:
                        doc.Saved = true;
                        break;
                }
            }
        }

        void OpenArticleInformationWindowIfNeeded(Word.Document doc)
        {
            var props = new DocumentCustomProperties(doc);
            if (props.PluginName != Constants.InformaPluginName) return;
            string articleNumber = props.ArticleNumber;
            if (!string.IsNullOrEmpty(articleNumber))
            {
                Log("Document opened with article number: " + articleNumber);
                if (!SitecoreUser.GetUser().IsLoggedIn)
                {
                    ArticleDetail.Open(true);
                }
                else
                {
                    CheckoutStatus checkedOut = SitecoreClient.GetLockedStatus(articleNumber);
                    if (checkedOut.User == SitecoreUser.GetUser().Username)
                    {
                        DocumentProtection.Unprotect(props);
                        return;
                    }
                    if (!checkedOut.Locked)
                    {
                        if (DialogFactory.PromptAutoLock() == DialogResult.Yes &&
                            SitecoreClient.CheckOutArticle(articleNumber, SitecoreUser.GetUser().Username))
                        {
                            DocumentProtection.Unprotect(props);
                        }
                    }
                    else
                    {
                        ArticleDetail.Open();
                    }
                }
            }
        }

        private void InitializePlugin()
        {
            Log("Starting add in...");

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                Log("Version: " + System.Windows.Forms.Application.ProductVersion);

            }

            _browser = new TreeBrowser(Application);
            _myCustomTaskPane = this.CustomTaskPanes.Add(_browser, "Sitecore References");
            _myCustomTaskPane.Visible = false;
            _myCustomTaskPane.Width = 335;
            _wordApp = Application;
            _wordApp.DocumentOpen += this.RemoveOrphanedTaskPanes;
            Log("Add in started.");
        }

        private void InitializeLogger()
        {
            try
            {
                string fileName = ApplicationConfig.GetPropertyValue("LogFileDirectory");
                fileName = fileName.Replace("{path}", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));
                fileName = fileName.Replace("{date}", DateTime.Now.ToString("_yyyyMMdd"));


                var fi = new FileInfo(fileName);
                if (fi.Directory != null && !fi.Directory.Exists)
                { fi.Directory.Create(); }

                _lameLogger = new StreamWriter(fileName, true);
            }
            catch (Exception ex)
            {
                try
                {
                    Trace.WriteLine("Could not load config file to get log path, using defaults.");
                    Trace.WriteLine("Exepction detail: " + ex.Message);
                    Trace.WriteLine(ex.StackTrace);

                    string fileName = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "InformaWordPlugin\\";
                    fileName += "informa_word_addin_" + DateTime.Now.ToString("_yyyyMMdd") + ".log";

                    var fi = new FileInfo(fileName);
                    if (fi.Directory != null && !fi.Directory.Exists)
                    { fi.Directory.Create(); }

                    _lameLogger = new StreamWriter(fileName, true);
                }
                catch (Exception x)
                {
                    Trace.WriteLine("Could not load logger from defaults, punting.");
                    Trace.WriteLine("Exepction detail: " + x.Message);
                    Trace.WriteLine(x.StackTrace);
                }
            }

            Log("ApplicationConfig.ConfigPath = [" + ApplicationConfig.ConfigPath + "], ApplicationConfig.RegistryHive = [" + ApplicationConfig.RegistryHive + "].");
        }

        public void Log(string text)
        {
            try
            {
                if (_lameLogger == null)
                { return; }
                _lameLogger.WriteLine(DateTime.Now.ToString() + ": " + text);
                _lameLogger.Flush();
            }
            catch
            { // don't let logging bring down the world
            }

        }

        public void LogException(string text)
        {
            Log(text);
        }

        public void LogException(string text, Exception ex)
        {
            try
            {
                if (_lameLogger == null)
                { return; }
                _lameLogger.WriteLine(DateTime.Now.ToString() + ": " + text);
                _lameLogger.WriteLine(DateTime.Now.ToString() + ": " + ex.Message);
                _lameLogger.WriteLine(DateTime.Now.ToString() + ": " + ex.StackTrace);
                _lameLogger.Flush();
            }
            catch
            {  // don't let logging bring down the world
            }
        }

        private void ThisAddIn_Shutdown(object sender, System.EventArgs e)
        {

        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(ThisAddIn_Startup);
            this.Shutdown += new System.EventHandler(ThisAddIn_Shutdown);
        }

        public void ShowTree(Word.Document doc)
        {
            var thesePanes = this.CustomTaskPanes.Where(c => c.Window == doc.ActiveWindow).AsParallel().ToList();
            if (thesePanes.Count > 0)
            /*Temperory removing the functionality to hide the tabs on click */
            { //thesePanes.ForEach(c => c.Visible = !c.Visible); 
                thesePanes.ForEach(c => c.Visible = true);
            }

            else
            {
                var ctp = this.CustomTaskPanes.Add(new TreeBrowser(Application), "Sitecore References", doc.ActiveWindow);
                ctp.Visible = true;
                ctp.Width = 335;
            }

        }

        public void RemoveOrphanedTaskPanes(Word.Document doc)
        {
            for (int i = this.CustomTaskPanes.Count; i > 0; i--)
            {
                var ctp = this.CustomTaskPanes[i - 1];
                try
                {
                    if (ctp.Window == null)
                    {
                        this.CustomTaskPanes.Remove(ctp);
                    }
                }
                catch
                {
                    this.CustomTaskPanes.Remove(ctp);
                }
            }
        }

        #endregion
    }
}
