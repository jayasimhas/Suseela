using System.Collections.Generic;

namespace Informa.Library.Utilities.DataModels
{
    public class TreeNode<TKey, TData>
    {
        public TKey Key { get; }
        public TData Data { get; set; }
        public List<TreeNode<TKey, TData>> Children { get; }
        public TreeNode<TKey, TData> Parent { get; set; }

        public TreeNode(TKey key)
        {
            Key = key;
            Children = new List<TreeNode<TKey, TData>>();
        }

        public override bool Equals(object obj)
        {
            var node = obj as TreeNode<TKey, TData>;
            return node != null && Key.Equals(node.Key);
        }

        public override int GetHashCode() => Key.GetHashCode();
    }
}
