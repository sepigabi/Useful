namespace Binarit.TOM2
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TreeViewExtensions
    {
        public static ObjectTreeRoot ToObjectTreeRoot(this IEnumerable<ObjectTreeItem> objectTreeItems)
        {
            ObjectTreeRoot objectTreeRoot = new ObjectTreeRoot();

            HashSet<ObjectTreeItem> itemsSet = new HashSet<ObjectTreeItem>(objectTreeItems);
            foreach (ObjectTreeItem objectTreeItem in objectTreeItems)
            {
                if (String.IsNullOrEmpty(objectTreeItem.ParentId))
                {
                    objectTreeRoot.Nodes.Add(new ObjectTreeNode() { Id = objectTreeItem.Id, DisplayName = objectTreeItem.DisplayName, Link = objectTreeItem.Link });
                    itemsSet.Remove(objectTreeItem);
                }
            }

            while (itemsSet.Count > 0)
            {
                foreach (ObjectTreeItem item in itemsSet.ToList())
                {
                    if (IsExistNode(objectTreeRoot, item.ParentId, out ObjectTreeNode node))
                    {
                        node.Children.Add(new ObjectTreeNode() { Id = item.Id, DisplayName = item.DisplayName, Link = item.Link });
                        itemsSet.Remove(item);
                    }
                }
            }

            return objectTreeRoot;
        }

        private static Boolean IsExistNode(ObjectTreeRoot objectTreeRoot, String NodeId, out ObjectTreeNode foundedNode)
        {
            foundedNode = null;
            foreach (ObjectTreeNode node in objectTreeRoot.Nodes)
            {
                foundedNode = node.Find(n => n.Id == NodeId);
                if (foundedNode != null)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
