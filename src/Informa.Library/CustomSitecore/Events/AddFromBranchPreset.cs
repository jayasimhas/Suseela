using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.SecurityModel;

namespace Informa.Library.CustomSitecore.Events {
    
    public class AddFromBranchPreset {
        public virtual void OnItemAdded(object sender, EventArgs args) {
            Assert.ArgumentNotNull(args, "args");

            Item newItem = Event.ExtractParameter(args, 0) as Item;
            if (newItem?.Branch == null || newItem.Branch.InnerItem.Children.Count != 1)
                return;

            Item branchItem = newItem.Branch.InnerItem;

            //build dictionary of items of the branch items: guid/item 
            string branchItemPath = branchItem.Children.First().Paths.FullPath;
            var allBranchItems = branchItem.Axes
                .GetDescendants()
                .Concat(new Item[] { branchItem })
                .ToDictionary(a => a.ID.ToString());
            
            //build dictionary of items in new tree: path/item
            string newRootPath = newItem.Paths.FullPath;
            var allNewItems = newItem.Axes
                .GetDescendants()
                .Concat(new Item[] { newItem })
                .ToDictionary(a => a.Paths.FullPath.Replace(newRootPath, string.Empty));
            
            //loop through all new items 
            foreach (Item i in allNewItems.Values)
            {
                //pull the shared and final layout fields
                string layout = i[FieldIDs.LayoutField];
                string finalLayout = i[FieldIDs.FinalLayoutField];

                //if there's no layout skip it
                if (string.IsNullOrEmpty(layout)
                && string.IsNullOrEmpty(finalLayout))
                    continue;

                //replace original datasources with new ones
                layout = UpdateMatches(layout, branchItemPath, allBranchItems, allNewItems);
                finalLayout = UpdateMatches(finalLayout, branchItemPath, allBranchItems, allNewItems);

                //update new item layout values
                using (new SecurityDisabler())
                {
                    using (new EditContext(i)) {
                        i[FieldIDs.LayoutField] = layout;
                        i[FieldIDs.FinalLayoutField] = finalLayout;
                    }
                }
            }
        }

        protected string UpdateMatches(string layout, string branchRootPath, Dictionary<string, Item> branchItems, Dictionary<string, Item> newItems)
        {
            //ds="{0413669C-3852-4459-862F-8A3BE5FADDA3}"
            Regex r = new Regex(@"(ds=[\""])({[a-zA-Z0-9]{8}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{4}-[a-zA-Z0-9]{12}})", RegexOptions.IgnoreCase);
            foreach (Match m in r.Matches(layout)) {
                if (!m.Success)
                    continue;

                //if ds is found in the old branch set, replace it with corresponding relative  new item
                string originalDS = m.Groups[2].Value;
                if (!branchItems.ContainsKey(originalDS))
                    continue;

                //if any match exists in the dictionary get the relative path and find the new item
                Item originalItem = branchItems[originalDS];
                string relativePath = originalItem.Paths.FullPath.Replace(branchRootPath, string.Empty);

                if (!newItems.ContainsKey(relativePath))
                    continue;

                //replace the id with the new item
                Item relativeItem = newItems[relativePath];
                layout = layout.Replace(originalDS, relativeItem.ID.ToString());
            }

            return layout;
        }
    }
}
