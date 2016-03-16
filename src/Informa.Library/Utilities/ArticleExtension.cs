﻿using System.Collections.Generic;
using System.Linq;
using Informa.Library.Utilities.References;
using Sitecore.Data.Items;

namespace Informa.Library.Utilities
{
	public static class ArticleExtension
	{

		public static Item GetAncestorItemBasedOnTemplateID(Item item)
		{
			IEnumerable<Item> pages = item.Axes.GetAncestors().Where(a => IsID(a.Template, Constants.Site_Root_Template_ID));
			return pages.FirstOrDefault();
		}

		public static bool IsID(TemplateItem template, string targetTemplateId)
		{
			bool ret = false;
			if (template == null)
				return ret;
			if (template.ID.ToString() == targetTemplateId)
			{
				ret = true;
			}
			else {

				IEnumerable<TemplateItem> baseTemps = template.BaseTemplates.Where(a => !a.Name.ToLower().Equals("standard template"));
				ret = baseTemps.Any(t => IsID(t, targetTemplateId));
			}
			return ret;
		}
	}
}
