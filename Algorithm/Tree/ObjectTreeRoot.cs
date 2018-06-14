namespace TreeExample
{
    using System.Collections.Generic;

    public class ObjectTreeRoot
    {
        public List<ObjectTreeNode> Nodes { get; set; }

        public ObjectTreeRoot()
        {
            Nodes = new List<ObjectTreeNode>();
        }
    }
}
