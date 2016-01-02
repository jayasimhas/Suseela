using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SitecoreTreeWalker.UI
{
	public class DialogFactory
	{
		public static DialogResult PromptAutoLock()
		{
			const string message = "This article is currently unlocked. Would you like to lock it and edit it?\n\n" 
								+"If you select 'Yes' it will be locked for you and will remain locked until you unlock it or move it through the Workflow.\n\n"
								+"If you select no, the article will open in read only mode.";
			return MessageBox.Show(message, @"Elsevier", MessageBoxButtons.YesNo);
		}
	}
}
