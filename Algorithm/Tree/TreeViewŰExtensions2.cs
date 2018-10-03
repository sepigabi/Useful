namespace Binarit.TOM2
{
    using Binarit.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class TreeViewExtensions
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(InterventionWorkflowLogic));

        public static IEnumerable<ObjectTreeNode> ToObjectTreeElements(this IEnumerable<ObjectTreeItem> objectTreeItems)
        {          
            var checkedObjectTreeItems = CheckObjectTreeItems(objectTreeItems);
            HashSet<ObjectTreeItem> itemsSet = new HashSet<ObjectTreeItem>(checkedObjectTreeItems);
            ObjectTreeRoot objectTreeRoot = new ObjectTreeRoot();

            foreach (ObjectTreeItem objectTreeItem in checkedObjectTreeItems)
            {
                if (String.IsNullOrEmpty(objectTreeItem.ParentId))
                {
                    objectTreeRoot.Nodes.Add(new ObjectTreeNode() { Id = objectTreeItem.Id, DisplayName = objectTreeItem.DisplayName, Link = objectTreeItem.Link, ImageUrl = objectTreeItem.ImageUrl, CssClass = objectTreeItem.CssClass });
                    itemsSet.Remove(objectTreeItem);
                }
            }

            while (itemsSet.Count > 0)
            {
                foreach (ObjectTreeItem item in itemsSet.ToList())
                {
                    if (IsExistNode(objectTreeRoot, item.ParentId, out ObjectTreeNode node))
                    {
                        node.Children.Add(new ObjectTreeNode() { Id = item.Id, DisplayName = item.DisplayName, Link = item.Link, ImageUrl = item.ImageUrl, CssClass = item.CssClass });
                        itemsSet.Remove(item);
                    }
                }
            }

            return objectTreeRoot.Nodes;
        }

        #region private methods

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

        /// <summary>
        /// Végtelen ciklusba kerülhetnénk, ha valamelyik elem ParentId-ja olyan elemre mutat, amelyik hiányzik, vagy ha az erdőben olyan fa van, amelyben kör van.
        /// </summary>
        /// <param name="objectTreeItems"></param>
        /// <returns></returns>
        private static IEnumerable<ObjectTreeItem> CheckObjectTreeItems(IEnumerable<ObjectTreeItem> forestElements)
        {
            return forestElements.FilterCyclicTrees().FilterElementsWithmissingParent();
        }

        private static IEnumerable<ObjectTreeItem> FilterCyclicTrees(this IEnumerable<ObjectTreeItem> forestElements)
        {
            var rootElements = forestElements.Where(e => e.ParentId == null);
            var finalListWithGoodElements = new List<ObjectTreeItem>();
            foreach (var rootElement in rootElements)
            {
                List<ObjectTreeItem> treeElements = new List<ObjectTreeItem>();
                treeElements.Add(rootElement);
                AddChildrenToTreeElements(treeElements, forestElements, rootElement);
                finalListWithGoodElements.AddRange(treeElements);
            }
            if(finalListWithGoodElements.Count < forestElements.Count())
            {
                var cyclicElements = forestElements.Where(e => !finalListWithGoodElements.Any(ge => ge.Id == e.Id));
                var cyclicElementIds = String.Join(",", cyclicElements.Select(e => e.Id));
                logger.Error($"Circular reference found in TechnicalObjectRelations. Affected TechnicalObjectIds: {cyclicElementIds}");
            }
            return finalListWithGoodElements;
        }

        private static void AddChildrenToTreeElements(List<ObjectTreeItem> treeElements, IEnumerable<ObjectTreeItem> forestElements, ObjectTreeItem parent)
        {
            var children = GetChildren(forestElements, parent);
            var goodChildren = children.Where(c => !AreAlreadyInGraph(treeElements, GetChildren(forestElements, c)));
            treeElements.AddRange(goodChildren);
            foreach (var goodChild in goodChildren)
            {
                AddChildrenToTreeElements(treeElements, forestElements, goodChild);
            }
        }

        private static bool AreAlreadyInGraph(List<ObjectTreeItem> treeElements, IEnumerable<ObjectTreeItem> elementsToTest)
        {
            foreach (var elementToTest in elementsToTest)
            {
                if (treeElements.Any(g => g.Id == elementToTest.Id))
                {
                    //TODO ide sosem jutunk ilyen adatstruktúra esetén...
                    logger.Error($"Circular reference found in TechnicalObjectRelations for TechnicalObjectId: {elementToTest.Id}");
                    return true;
                }
            }
            return false;
        }

        private static IEnumerable<ObjectTreeItem> GetChildren(IEnumerable<ObjectTreeItem> forestElements, ObjectTreeItem parent)
        {
            return forestElements.Where(i => i.ParentId == parent.Id);
        }

        private static IEnumerable<ObjectTreeItem> FilterElementsWithmissingParent(this IEnumerable<ObjectTreeItem> forestElements)
        {
            var finalListWithGoodElements = new List<ObjectTreeItem>();

            var goodChildElements = forestElements.Where(oti => !String.IsNullOrEmpty(oti.ParentId))
                .Where(oti => forestElements.Any(oti2 => oti2.Id == oti.ParentId));
            finalListWithGoodElements.AddRange(goodChildElements);
            var rootElements = forestElements.Where(oti => String.IsNullOrEmpty(oti.ParentId));
            finalListWithGoodElements.AddRange(rootElements);
            return finalListWithGoodElements;
        }

        #endregion
    }
}
