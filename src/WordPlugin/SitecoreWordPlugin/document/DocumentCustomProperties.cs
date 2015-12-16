using System;
using System.Diagnostics;
using Microsoft.Office.Core;
using System.Reflection;
using SitecoreTreeWalker.Util;
using Word = Microsoft.Office.Interop.Word;

namespace SitecoreTreeWalker.document
{
	public class DocumentCustomProperties
	{
		public readonly Word.Document _wordDoc;         
 
		public DocumentCustomProperties(Word.Document doc)
		{
			_wordDoc = doc;

            UpdatePluginVersionNumber();
		}

		public int WordSitecoreVersionNumber
		{
			get
			{
				if (string.IsNullOrWhiteSpace(GetPropertyValue(Constants.WordVersionNumber)))
				{
					return 0;
				}

				return Int32.Parse(GetPropertyValue(Constants.WordVersionNumber));
			}
			set
			{
			    SetCustomDocumentProperty(Constants.WordVersionNumber, value.ToString());
			}
		}

        public void UpdatePluginVersionNumber()
        {
            string versionNumber = "Development Version";
            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                versionNumber = System.Deployment.Application.ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString();
            }
            SetCustomDocumentProperty(Constants.PluginVersionNumber, versionNumber);
        }

		public string DocumentPassword
		{
			get { return GetPropertyValue(Constants.ArticleID); }
            set { SetCustomDocumentProperty(Constants.ArticleID, value); }
		}

		public string ArticleNumber
		{
			get { return GetPropertyValue(Constants.ArticleNumber); }
			set
			{
			    SetCustomDocumentProperty(Constants.ArticleNumber, value);
			}
		}

		public bool SetCustomDocumentProperty(string name, string value)
		{
			if ((value!= null) && (value.Trim().Length > 0))
			{
				object objMissing = Missing.Value;

				var props = (DocumentProperties)_wordDoc.CustomDocumentProperties;
				if (PropertyExists(name))
				{
					string currentValue = GetPropertyValue(name);
					/*if (currentValue != value)
					{
						//dirty the word doc
						_wordDoc.Saved = false;
					}*/
					DeleteExistingProperty(name);
				}
				props.Add(name, false, MsoDocProperties.msoPropertyTypeString, value, objMissing);
			}
			else
			{
				SetCustomDocumentPropertyToEmpty(name);
			}
			
			return true;
		}

		public bool SetCustomDocumentPropertyToEmpty(string name)
		{
			object objMissing = Missing.Value;
			var props = (DocumentProperties)_wordDoc.CustomDocumentProperties;
			if (PropertyExists(name))
			{
				DeleteExistingProperty(name);
				props.Add(name, false, MsoDocProperties.msoPropertyTypeString, "", objMissing);
				return true;
			}
			return false;
		}

		public bool PropertyExists(string name)
		{
			var props = (DocumentProperties)_wordDoc.CustomDocumentProperties;
			foreach (DocumentProperty prop in props)
			{
				try
				{
					if (prop.Name.Equals(name))
						return true;
				}
				catch (Exception ex)
				{
					Globals.SitecoreAddin.LogException("DocumentCustomProperties.PropertyExists: Property [" + name + "] doesn't exist!", ex);
				}
			}
			return false;
		}

		public string GetPropertyValue(string name)
		{
			var props = (DocumentProperties)_wordDoc.CustomDocumentProperties;
			foreach (DocumentProperty prop in props)
			{
				try
				{
					if (prop.Name.Equals(name))
						return prop.Value;
				}
				catch (Exception ex)
				{
					Globals.SitecoreAddin.LogException("DocumentCustomProperties.PropertyExists: Property [" + name + "] doesn't exist!", ex);
				}
			}
			return null;
		}

		protected void DeleteExistingProperty(string name)
		{
			var props = (DocumentProperties)_wordDoc.CustomDocumentProperties;
			foreach (DocumentProperty prop in props)
			{
				try
				{
					if (prop.Name.Equals(name))
						prop.Delete();
				}
				catch (Exception ex)
				{
					Globals.SitecoreAddin.LogException("DocumentCustomProperties.PropertyExists: Property [" + name + "] doesn't exist!", ex);
				}
			}
			return;
		}
	}
}
