using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.document
{
	public class DocumentProtection
	{
		public static bool Protect(DocumentCustomProperties props)
		{
			Document wordDoc = props._wordDoc;
			var dirtyState = wordDoc.Saved;
			try
			{
				var pass = SitecoreArticle.GetDocumentPassword();
				if (wordDoc.ProtectionType == WdProtectionType.wdNoProtection)
				{
					wordDoc.Protect(WdProtectionType.wdAllowOnlyReading, true, pass, false, true);
					props.DocumentPassword = pass;
				}
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Problem protecting the word document!", ex);
				return false;
			}
			finally
			{
				wordDoc.Saved = dirtyState;
			}
			return true;
		}

		public static bool Unprotect(DocumentCustomProperties props)
		{
			Document wordDoc = props._wordDoc;
			var dirtyState = wordDoc.Saved;
			try
			{
				string pass = props.DocumentPassword;
				if (wordDoc.ProtectionType > WdProtectionType.wdNoProtection)
				{ wordDoc.Unprotect(pass); }
			}
			catch (Exception ex)
			{
				Globals.SitecoreAddin.LogException("Problem unprotecting the word document!", ex);
				return false;
			}
			finally
			{
				wordDoc.Saved = dirtyState;
			}
			return true;
		}
	}
}
