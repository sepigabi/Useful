namespace TreeExample
{
    using System;
    using System.Collections.Generic;

    public class ObjectTreeNode
    {
        public String Id { get; set; }

        public String DisplayName { get; set; }

        public String Link { get; set; }

        public List<ObjectTreeNode> Children { get; set; }

        public ObjectTreeNode()
        {
            Children = new List<ObjectTreeNode>();
             
        }

        public ObjectTreeNode Find(Func<ObjectTreeNode, Boolean> myFunc)
        {
            if (myFunc(this))
            {
                return this;
            }

            foreach (ObjectTreeNode node in Children)
            {
                if (myFunc(node))
                {
                    return node;
                }
                else
                {
                    ObjectTreeNode test = node.Find(myFunc);
                    if (test != null)
                        return test;
                }
            }
            return null;
        }
    }
}
