// Decompiled with JetBrains decompiler
// Type: Sitecore.PrintStudio.PublishingEngine.Rendering.XmlTextFrameRenderer
// Assembly: Sitecore.PrintStudio.PublishingEngine, Version=8.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 6F2E1FF9-5217-43C4-81F8-90280749F85C
// Assembly location: D:\Projects\Informa\lib\PXM\Sitecore.PrintStudio.PublishingEngine.dll

using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.PrintStudio.PublishingEngine.Helpers;
using Sitecore.PrintStudio.PublishingEngine.Scripting;
using Sitecore.PrintStudio.PublishingEngine.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Sitecore.PrintStudio.PublishingEngine;
using Sitecore.PrintStudio.PublishingEngine.Rendering;

namespace Informa.Library.PXM
{
	public class CustomXmlTextFrameRenderer : InDesignItemRendererBase
	{
		[Obsolete]
		public string Flow { get; set; }

		public string InDesignContent { get; set; }

		public string ContentFieldName { get; set; }

		private readonly Dictionary<string, string> fieldStyles = new Dictionary<string, string>
		{
			{"Title", "1.0 Story Title" },
			{"Sub Title", "1.0 StorySub-Title" }
		};

		protected override void BeginRender(PrintContext printContext)
		{
			if (string.IsNullOrEmpty(this.DataSource))
				this.DataSource = this.RenderingItem["item reference"];
			if (!string.IsNullOrEmpty(this.ContentFieldName))
				return;
			this.ContentFieldName = this.RenderingItem["item field"];
		}

		protected override void RenderContent(PrintContext printContext, XElement output)
		{
			if (ScriptHelper.ExecuteScriptReference(printContext, this.RenderingItem, this.GetDataItem(printContext), output))
				return;
			bool flag = !SitecoreHelper.HasScriptItemAssigned(this.RenderingItem) && !SitecoreHelper.HasMergeItemAssigned(this.RenderingItem);
			if (flag && string.IsNullOrEmpty(this.ContentFieldName))
			{
				Item obj = this.RenderingItem.Children.FirstOrDefault<Item>((Func<Item, bool>)(c => c.TemplateName.Equals("p_text", StringComparison.InvariantCultureIgnoreCase)));
				if (obj != null && obj.Fields[printContext.Settings.StaticTextFieldName] != null)
				{
					this.DataSource = obj.ID.ToString();
					this.ContentFieldName = printContext.Settings.StaticTextFieldName;
					this.RenderDeep = false;
				}
			}
			Item dataItem = this.GetDataItem(printContext);
			XElement xelement = RenderItemHelper.CreateXElement("TextFrame", this.RenderingItem, printContext.Settings.IsClient, dataItem);
			this.SetAttributes(xelement);
			IEnumerable<XElement> xelements = (IEnumerable<XElement>)null;
			if (flag)
				xelements = this.GetContent(printContext, xelement);
			output.Add((object)xelement);
			xelement.Add((object)xelements);
			this.RenderChildren(printContext, xelement);
		}

		protected virtual IEnumerable<XElement> GetContent(PrintContext printContext, XElement textFrameNode)
		{
			XAttribute xattribute = textFrameNode.Attribute((XName)"ParagraphStyle");
			string str = xattribute == null || string.IsNullOrEmpty(xattribute.Value) ? "NormalParagraphStyle" : xattribute.Value;
			if (!string.IsNullOrEmpty(this.InDesignContent))
				return this.FormatText(str, this.InDesignContent);
			if (string.IsNullOrEmpty(this.ContentFieldName))
				return (IEnumerable<XElement>)null;
			try
			{
				Item dataItem = this.GetDataItem(printContext);
				if (dataItem != null)
				{
					Field field = dataItem.Fields[this.ContentFieldName];
					if (field == null)
						return (IEnumerable<XElement>)null;
					switch (field.Type)
					{
						case "Rich Text":
							ParseContext context = new ParseContext(printContext.Database, printContext.Settings)
							{
								DefaultParagraphStyle = str,
								ParseDefinitions = RichTextParser.GetParseDefinitionCollection(this.RenderingItem)
							};
							string xml = RichTextParser.ConvertToXml(field.Value, context, printContext.Language);
							XElement element = new XElement((XName)"temp");
							element.AddFragment(xml);
							return element.Elements();
						case "Single-Line Text":
							str = fieldStyles.ContainsKey(ContentFieldName) ? fieldStyles[ContentFieldName] : str;
							string singleLineContent = SitecoreHelper.FetchFieldValue(dataItem, field.Name, printContext.Database, str);
							return this.FormatText(str, singleLineContent);
						default:
							string content = SitecoreHelper.FetchFieldValue(dataItem, field.Name, printContext.Database, str);
							return this.FormatText(str, content);
					}
				}
			}
			catch (Exception ex)
			{
				Logger.Error("Rendering TextFrame: " + (object)this.RenderingItem.ID, ex);
			}
			return (IEnumerable<XElement>)null;
		}

		protected IEnumerable<XElement> FormatText(string paragraphStyle, string content)
		{
			if (!content.Contains("<ParagraphStyle"))
			{
				if (!content.Contains("<CharacterStyle"))
					return (IEnumerable<XElement>)new List<XElement>()
		  {
			this.CreateParagraphStyle(paragraphStyle, content)
		  };
			}
			try
			{
				XElement xelement = XElement.Parse(string.Format("<innertext>{0}</innertext>", (object)content));
				if (xelement.HasElements)
					return xelement.Elements();
			}
			catch (Exception ex)
			{
				Logger.Error("PrintTextFrame", ex);
			}
			return (IEnumerable<XElement>)null;
		}

		protected virtual void SetAttributes(XElement textFrameNode)
		{
			textFrameNode.SetAttributeValue((XName)"SitecoreFieldname", (object)this.RenderingItem["item field"]);
			textFrameNode.SetAttributeValue((XName)"ItemReferenceID", (object)this.RenderingItem["item reference"]);
			textFrameNode.SetAttributeValue((XName)"RenderingID", (object)this.RenderingItem["xml renderer"]);
			Item targetItem = ((ReferenceField)this.RenderingItem.Fields["item reference"]).TargetItem;
			textFrameNode.SetAttributeValue((XName)"ItemReferenceDisplayName", targetItem != null ? (object)targetItem.DisplayName : (object)string.Empty);
			string str1 = this.RenderingItem["FlowName"];
			if (!str1.Contains("$"))
				return;
			try
			{
				string[] strArray = str1.Split(':');
				string str2 = strArray[0];
				string str3 = strArray[1];
				if (str3.ToLower().Contains("$itemid"))
				{
					XElement xelement = textFrameNode;
					XName name = (XName)"FlowName";
					string str4;
					if (targetItem == null)
						str4 = string.Empty;
					else
						str4 = str2 + "_" + str2 + "_" + targetItem.ID.ToString().Replace("{", string.Empty).Replace("}", string.Empty);
					xelement.SetAttributeValue(name, (object)str4);
				}
				if (!str3.ToLower().Contains("$field"))
					return;
				int num = str3.IndexOf("[", StringComparison.Ordinal);
				string index = str3.Substring(num + 1).Replace("]", string.Empty);
				string str5 = targetItem != null ? targetItem[index] : str1;
				textFrameNode.SetAttributeValue((XName)"FlowName", (object)(str2 + "_" + str5));
			}
			catch
			{
				textFrameNode.SetAttributeValue((XName)"FlowName", (object)str1);
			}
		}

		protected XElement CreateParagraphStyle(string paraStyle, string cdataContent)
		{
			XElement xelement = new XElement((XName)"ParagraphStyle");
			xelement.SetAttributeValue((XName)"Style", (object)paraStyle);
			xelement.Add((object)new XCData(cdataContent));
			return xelement;
		}
	}
}
