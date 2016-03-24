using InformaSitecoreWord.Sitecore;
using System;
//using System.Windows;
using System.Windows.Forms;

namespace InformaSitecoreWord.Util
{
    public static class NLMFeedUtils
    {
        public static void PromptAndReExportNLMFeed(string articleNumber, bool isArticlePublished)
        {
            try
            {
                if (isArticlePublished)//Check if article is already published
                {
                    //If yes, ask author whether to reExport NLM feed which should have already been exported
                    if (MessageBox.Show("Would you like to re-export the NLM feed?", "Export NLM?", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                    {
                        //Call Web API to do the reExporting via the article Number
                        string result = SitecoreClient.ReExportArticleNlm(articleNumber);

                        if (string.IsNullOrEmpty(result))
                        {
                            //export success
                        }
                        else
                        {
                            Globals.SitecoreAddin.LogException("Something went wrong in the server side while reExporting NLM feed" + result);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Globals.SitecoreAddin.LogException("Error when requesting an article NLM Reexport", ex);
            }
        }
    }
}
