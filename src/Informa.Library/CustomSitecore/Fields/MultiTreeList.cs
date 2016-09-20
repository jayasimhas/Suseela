using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Glass.Mapper.Sc.Configuration;
using Glass.Mapper.Sc.Configuration.Fluent;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.Sheer;
using Sitecore.Web.UI.WebControls;
using Informa.Library.Utilities.CMSHelpers;

namespace Informa.Library.CustomSitecore.Fields
{            
    public class FieldMap<T> : MultilistField where T : Field
    { 
        public FieldMap(Field innerField) : base(innerField)
        {

        }
    }


	// Exact copy of Sitecore's TreeList class except for a few
	// minor changes in the OnLoad function (and constructor)
	public class MultiTreeList : TreeList
	{
		protected new void Add()
		{
			if (!this.Disabled)
			{
				string viewStateString = base.GetViewStateString("ID");
				TreeviewEx treeviewEx = this.FindControl(viewStateString + "_all") as TreeviewEx;
				Assert.IsNotNull(treeviewEx, typeof(DataTreeview));
				Listbox listbox = this.FindControl(viewStateString + "_selected") as Listbox;
				Assert.IsNotNull(listbox, typeof(Listbox));
				Item selectionItem = treeviewEx.GetSelectionItem();
				if (selectionItem == null)
				{
					SheerResponse.Alert("Select an item in the Content Tree.", new string[0]);
				}
				else if (!this.HasExcludeTemplateForSelection(selectionItem))
				{
					if (this.IsDeniedMultipleSelection(selectionItem, listbox))
					{
						SheerResponse.Alert("You cannot select the same item twice.", new string[0]);
					}
					else if (this.HasIncludeTemplateForSelection(selectionItem))
					{
						SheerResponse.Eval("scForm.browser.getControl('" + viewStateString + "_selected').selectedIndex=-1");
						ListItem control = new ListItem();
						control.ID = GetUniqueID("L");
						Sitecore.Context.ClientPage.AddControl(listbox, control);
						control.Header = selectionItem.DisplayName;
						control.Value = control.ID + "|" + selectionItem.ID;
						SheerResponse.Refresh(listbox);
						SetModified();
					}
				}
			}
		}
		
		private bool HasExcludeTemplateForSelection(Item item)
		{
			return ((item == null)
				|| HasItemTemplate(item, this.ExcludeTemplatesForSelection)
				|| ExcludeBaseTemplatesForSelection.Split(',').Select(GetFormattedTemplateId).Distinct().Any(template => IsOfTemplate(item, template, true)));
		}

		private bool HasIncludeTemplateForSelection(Item item)
		{
			Assert.ArgumentNotNull(item, "item");
			return ((this.IncludeTemplatesForSelection.Length == 0 && IncludeBaseTemplatesForSelection.Length == 0)
				|| HasItemTemplate(item, this.IncludeTemplatesForSelection)
				|| IncludeBaseTemplatesForSelection.Split(',').Select(GetFormattedTemplateId).Distinct().Any(template => IsOfTemplate(item, template, true)));
		}

		private static bool HasItemTemplate(Item item, string templateList)
		{
			Assert.ArgumentNotNull(templateList, "templateList");
			if (item == null)
			{
				return false;
			}
			if (templateList.Length == 0)
			{
				return false;
			}
			string[] strArray = templateList.Split(new char[] { ',' });
			ArrayList list = new ArrayList(strArray.Length);
			for (int i = 0; i < strArray.Length; i++)
			{
				list.Add(strArray[i].Trim().ToLower());
			}
			return list.Contains(item.TemplateName.Trim().ToLower());
		}

		private bool IsDeniedMultipleSelection(Item item, Listbox listbox)
		{
			Assert.ArgumentNotNull(listbox, "listbox");
			if (item == null)
			{
				return true;
			}
			if (!this.AllowMultipleSelection)
			{
				foreach (ListItem item2 in listbox.Controls)
				{
					string[] strArray = item2.Value.Split(new char[] { '|' });
					if ((strArray.Length >= 2) && (strArray[1] == item.ID.ToString()))
					{
						return true;
					}
				}
			}
			return false;
		}

		// This method is the only method that differs from Sitecore's
		// TreeList class
		protected override void OnLoad(EventArgs args)
		{
			base.OnLoad(args);

			if (!Sitecore.Context.ClientPage.IsEvent)
			{
				// Custom Call
				this.SetProperties();
			}
		}
		
		// Modified from base TreeList
		private void SetProperties()
		{
			string id = StringUtil.GetString(new string[] { this.Source });
			if (Sitecore.Data.ID.IsID(id))
			{
				this.DataSource = this.Source;
			}
			else if ((this.Source != null) && !id.Trim().StartsWith("/", StringComparison.OrdinalIgnoreCase))
			{
				this.ExcludeTemplatesForSelection = StringUtil.ExtractParameter("ExcludeTemplatesForSelection", this.Source).Trim();
				this.IncludeTemplatesForSelection = StringUtil.ExtractParameter("IncludeTemplatesForSelection", this.Source).Trim();
				this.ExcludeBaseTemplatesForSelection = StringUtil.ExtractParameter("ExcludeBaseTemplatesForSelection", this.Source).Trim();
				this.IncludeBaseTemplatesForSelection = StringUtil.ExtractParameter("IncludeBaseTemplatesForSelection", this.Source).Trim();
				this.IncludeTemplatesForDisplay = StringUtil.ExtractParameter("IncludeTemplatesForDisplay", this.Source).Trim();
				this.ExcludeTemplatesForDisplay = StringUtil.ExtractParameter("ExcludeTemplatesForDisplay", this.Source).Trim();
				this.ExcludeItemsForDisplay = StringUtil.ExtractParameter("ExcludeItemsForDisplay", this.Source).Trim();
				this.IncludeItemsForDisplay = StringUtil.ExtractParameter("IncludeItemsForDisplay", this.Source).Trim();
				string strA = StringUtil.ExtractParameter("AllowMultipleSelection", this.Source).Trim().ToLower();
				this.AllowMultipleSelection = string.Compare(strA, "yes", StringComparison.OrdinalIgnoreCase) == 0;
				this.DataSource = StringUtil.ExtractParameter("DataSource", this.Source).Trim().ToLower();

				int dataSourceIndex = 2;
				string nthDS =
					StringUtil.ExtractParameter(String.Format("DataSource{0}", dataSourceIndex), Source).Trim().ToLower();
				while (!String.IsNullOrEmpty(nthDS))
				{
					DataSources.Add(dataSourceIndex, nthDS);
					dataSourceIndex++;
					nthDS = StringUtil.ExtractParameter(String.Format("DataSource{0}", dataSourceIndex), Source).Trim().ToLower();
				}

				this.DatabaseName = StringUtil.ExtractParameter("databasename", this.Source).Trim().ToLower();
				this.AncestorTemplateDatasource = StringUtil.ExtractParameter("AncestorTemplate", this.Source).Trim().ToLower();
			}
			else
			{
				this.DataSource = this.Source;
			}
		}

		public Dictionary<int, string> DataSources { get; private set; }

		private string _ancestorTemplateDatasource = string.Empty;
		public string AncestorTemplateDatasource
		{
			get
			{
				return _ancestorTemplateDatasource;
			}
			set
			{
				_ancestorTemplateDatasource = value;
			}
		}

		[Category("Data"), Description("Comma separated list of template names. If this value is set, items based on these template will not be included in the menu.")]
		public string ExcludeBaseTemplatesForSelection
		{
			get
			{
				return base.GetViewStateString("ExcludeBaseTemplatesForSelection");
			}
			set
			{
				Assert.ArgumentNotNull(value, "value");
				base.SetViewStateString("ExcludeBaseTemplatesForSelection", value);
			}
		}
		
		[Category("Data"), Description("Comma separated list of template names. If this value is set, only items based on these template can be included in the menu.")]
		public string IncludeBaseTemplatesForSelection
		{
			get
			{
				return base.GetViewStateString("IncludeBaseTemplatesForSelection");
			}
			set
			{
				Assert.ArgumentNotNull(value, "value");
				base.SetViewStateString("IncludeBaseTemplatesForSelection", value);
			}
		}
		
		#region Private Helpers

		private static string GetFormattedTemplateId(string templateId)
		{
			if (templateId == null)
				return null;

			Guid guid;
			return Guid.TryParse(templateId, out guid)
				? guid.ToString("B").ToUpperInvariant()
				: templateId;
		}

		private static bool IsOfTemplate(Item item, string templateId, bool deep)
		{
			return IsOfTemplate(item, templateId, deep ? -1 : 0);
		}

		private static bool IsOfTemplate(Item item, string templateId, int depth)
		{
			if (item == null || string.IsNullOrEmpty(item.TemplateID.ToString()))
				return false;
			if (depth < -1)
				throw new ArgumentOutOfRangeException("depth", (object) depth, "Depth but be -1, 0, or greater.");
			if (IsOfTemplate(item, templateId) ||
			    IsOfTemplate(item, ItemIdResolver.GetItemIdByKey("TemplateFolder")) && item.ID.ToString() == templateId)
				return true;
			if (depth == 0 || IsOfTemplate(item, ItemIdResolver.GetItemIdByKey("TemplateFolder")))
				return false;
			foreach (TemplateItem templateItem in item.Template.BaseTemplates)
			{
				if (templateItem.ID.ToString() == templateId ||
				    IsTemplateOfTemplate(templateItem, templateId, depth > 0 ? depth - 1 : depth))
					return true;
			}
			return false;
		}

		private static bool IsTemplateOfTemplate(TemplateItem item, string templateId, int depth)
		{
			if (item == null || string.IsNullOrEmpty(item.ID.ToString()))
				return false;
			if (depth < -1)
				throw new ArgumentOutOfRangeException("depth", (object) depth, "Depth but be -1, 0, or greater.");
			if (item.ID.ToString() == templateId)
				return true;
			if (item.ID.ToString() == ItemIdResolver.GetItemIdByKey("TemplateFolder") || depth == 0)
				return false;
			foreach (TemplateItem templateItem in item.BaseTemplates)
			{
				if (templateItem.ID.ToString() == templateId ||
				    IsTemplateOfTemplate(templateItem, templateId, depth > 0 ? depth - 1 : depth))
					return true;
			}
			return false;
		}

		private static bool IsOfTemplate(Item item, string templateId)
		{
			if (item == null || string.IsNullOrEmpty(item.TemplateID.ToString()))
				return false;
			return item.TemplateID.ToString() == templateId;
		}

		#endregion

	}
}
