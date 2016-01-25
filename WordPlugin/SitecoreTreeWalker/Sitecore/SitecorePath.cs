using System.Collections.Generic;
using System.Windows.Forms;

namespace SitecoreTreeWalker.Sitecore
{
    public class SitecorePath
    {
        public SitecorePath(string displayName, string path)
        {
            Retreieved = false;
            NestedNodesAdded = false;
            Path = path;
            DisplayName = displayName;
            Descendents = new List<SitecorePath>();
        }

        public string Path { get; set; }
        public string DisplayName { get; set; }

        public IList<SitecorePath> Descendents { get; set; }

        private bool Retreieved { get; set; }
        public bool NestedNodesAdded { get; set; }

        public void GetDecendents()
        {
            if (Retreieved) return;

            Descendents.Clear();

            var children = SitecoreGetter.GetChildrenDirectories(Path);
            foreach (var child in children)
            {
                var path = string.Format("{0}/{1}", Path, child.Name);
                var sitecorePath = new SitecorePath(child.Name, path);
                foreach (var nestedDir in child.Children)
                {
                    var nestedPath = string.Format("{0}/{1}", path, nestedDir);
                    sitecorePath.Descendents.Add(new SitecorePath(nestedDir, nestedPath));
                }

                Descendents.Add(sitecorePath);
            }

            Retreieved = true;
        }

        public TreeNode GetNode()
        {
            var node = new TreeNode(DisplayName) {Tag = this};
        	//node.ContextMenu = new SitecoreMenuItemContext(node.Text, Path);
			//no context menu
            return node;
        }
    }
}
