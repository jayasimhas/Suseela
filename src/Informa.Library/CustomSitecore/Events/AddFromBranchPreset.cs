using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Informa.Library.Utilities;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.StringExtensions;

namespace Informa.Library.CustomSitecore.Events {
    /// <summary>
    /// Augments the functionality of Branch Templates by making any rendering data sources set in the layout on the branch
    /// that point to other children of the branch be repointed to the newly created branch item
    /// instead of the source branch item. This allows for templating including data source items using branches.
    /// </summary>
    /// <remarks>
    /// See the following for inspiration: https://github.com/kamsar/BranchPresets
    /// </remarks>
    public class AddFromBranchPreset {
        public virtual void OnItemAdded(object sender, EventArgs args) {
            Assert.ArgumentNotNull(args, "args");

            Item targetItem = Event.ExtractParameter(args, 0) as Item;
            if (targetItem?.Branch == null || targetItem.Branch.InnerItem.Children.Count != 1)
                return;

            // find all rendering data sources on the branch root item that point to an item under the branch template,
            // and repoint them to the equivalent subitem under the branch instance
            RewriteBranchRenderingDataSources(targetItem, targetItem.Branch);
        }

        protected virtual void RewriteBranchRenderingDataSources(Item item, BranchItem branchTemplateItem) {
            string branchBasePath = branchTemplateItem.InnerItem.Paths.FullPath;

            LayoutHelper.ApplyActionToAllRenderings(item, rendering => {
                if (string.IsNullOrWhiteSpace(rendering.Datasource))
                    return RenderingActionResult.None;

                // note: queries and multiple item datasources are not supported
                var renderingTargetItem = item.Database.GetItem(rendering.Datasource);

                if (renderingTargetItem == null)
                    Log.Warn("Error while expanding branch template rendering datasources: data source {0} was not resolvable.".FormatWith(rendering.Datasource), this);

                // if there was no valid target item OR the target item is not a child of the branch template we skip out
                if (renderingTargetItem == null || !renderingTargetItem.Paths.FullPath.StartsWith(branchBasePath, StringComparison.OrdinalIgnoreCase))
                    return RenderingActionResult.None;

                var relativeRenderingPath = renderingTargetItem.Paths.FullPath.Substring(branchBasePath.Length).TrimStart('/');
                relativeRenderingPath = relativeRenderingPath.Substring(relativeRenderingPath.IndexOf('/')); // we need to skip the "/$name" at the root of the branch children

                var newTargetPath = item.Paths.FullPath + relativeRenderingPath;

                var newTargetItem = item.Database.GetItem(newTargetPath);

                // if the target item was a valid under branch item, but the same relative path does not exist under the branch instance
                // we set the datasource to something invalid to avoid any potential unintentional edits of a shared data source item
                if (newTargetItem == null) {
                    rendering.Datasource = "INVALID_BRANCH_SUBITEM_ID";
                    return RenderingActionResult.None;
                }

                rendering.Datasource = newTargetItem.ID.ToString();
                return RenderingActionResult.None;
            });
        }
    }
}
