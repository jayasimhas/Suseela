using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Word = Microsoft.Office.Interop.Word;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Word;

namespace SitecoreTreeWalker
{
    public partial class SitecoreAddin
    {
        private void SitecoreAddIn_Startup(object sender, System.EventArgs e)
        {
        }

        private void SitecoreAddIn_Shutdown(object sender, System.EventArgs e)
        {
        }

        #region VSTO generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.Startup += new System.EventHandler(SitecoreAddIn_Startup);
            this.Shutdown += new System.EventHandler(SitecoreAddIn_Shutdown);
        }
        
        #endregion
    }
}
