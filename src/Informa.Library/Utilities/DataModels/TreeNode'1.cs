using System.Collections.Generic;

namespace Informa.Library.Utilities.DataModels
{
    public class TreeNode<T>
    {
        public T Value { get; }
        public string Note { get; set; }
        public List<TreeNode<T>> Children { get; }
        public TreeNode<T> Parent { get; set; }

        public TreeNode(T value, string note = null)
        {
            Value = value;
            Note = note;
            Children = new List<TreeNode<T>>();
        }

        public override bool Equals(object obj)
        {
            var node = obj as TreeNode<T>;
            return node != null && Value.Equals(node.Value);
        }

        public override int GetHashCode() => Value.GetHashCode();
    }
}
