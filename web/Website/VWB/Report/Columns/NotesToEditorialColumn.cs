using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Elsevier.Library.CustomItems.Publication.General;
using Sitecore.Data.Items;
using Velir.Utilities;

namespace Elsevier.Web.VWB.Report.Columns
{
	public class NotesToEditorialColumn : IVwbColumn
	{
		public int Compare(ArticleItemWrapper x, ArticleItemWrapper y)
		{
			return x.NotesToEditorial.CompareTo(y.NotesToEditorial);
		}

		public string GetHeader()
		{
			return "Notes To Editorial";
		}

		public string Key()
		{
			return "nte";
		}

		public TableCell GetCell(ArticleItemWrapper articleItemWrapper)
		{
			ArticleItem articleItem = articleItemWrapper.InnerItem;
			var cell = new TableCell();
			var text = new TextBox
			           	{
			           		Text = articleItemWrapper.NotesToEditorial,
							TextMode = TextBoxMode.MultiLine,
							Rows = 5,
							CssClass = "block",
							ID = "ta_"+articleItem.ID
			           	};
			text.EnableViewState = false;
			var refresh = new Button
			              	{
			              		Text = "Save"
			              	};
			
			text.Attributes.Add("itemID", articleItem.ID.ToString());
			refresh.Attributes.Add("itemID", articleItem.ID.ToString());
			refresh.OnClientClick= "UpdateEditorialNote('" + articleItem.ID.ToString() + "'); return false;";
			cell.Controls.Add(text);
			cell.Controls.Add(refresh);
			return cell;
		}

        public Dictionary<string, string> GetDropDownValues(List<ArticleItemWrapper> results)
        {
            throw new NotImplementedException();
        }
    }
}