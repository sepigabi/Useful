public class log4netConfigMerger : XmlMergerBase
    {
        public override string[] GetIdentifierAttributePrecedence()
        {
            return new string[] { "id", "name", "type" };
        }

        public override bool IsMultiOccureNode( XElement element )
        {
            switch (element.Name.ToString())
            {
                case "renderer":
                    return true;
                case "appender-ref":
                    return true;
                default:
                    return false;
            }
        }
    }
    
    
    
public abstract class XmlMergerBase
    {
        /// <summary>
        /// Returns, in precedence order, which attribute can identify a particular xml node as unique
        /// </summary>
        public abstract string[] GetIdentifierAttributePrecedence();

        /// <summary>
        /// Specifies that an xml node that does not have an identifier attribute can have multiple nodes at the same level (as a sibling) or only one
        /// </summary>
        public abstract bool IsMultiOccureNode( XElement element );

        /// <summary>
        /// Overrides / completes baseElement attributes based on inheritedElement attributes (except identifier attribute).
        /// Goes through the nodes of the inheritedElement. If there is a node of the same type in the baseElement and their identifier attribute is the same, we recursively
        /// call the merge on it, if there is none, we add it to the baseElement.
        /// </summary>
        public void Merge( XElement baseElement, XElement inheritedElement )
        {
            OverrideAttributeValues( baseElement, inheritedElement );

            var inheritedNodesByName = inheritedElement.Elements().GroupBy( e => e.Name );
            foreach (var inheritedNodeGroup in inheritedNodesByName)
            {
                var baseNodeGroup = baseElement.Elements().Where( e => e.Name == inheritedNodeGroup.Key );
                foreach (var inheritedNode in inheritedNodeGroup)
                {
                    string identifierAttribute = GetIdentifierAttribute( inheritedNode );
                    if (identifierAttribute != null)
                    {
                        var identifier = inheritedNode.Attribute( identifierAttribute ).Value;
                        var existingNodeInBase = baseNodeGroup.Where( n => n.Attribute( identifierAttribute )?.Value == identifier ).FirstOrDefault();
                        if (existingNodeInBase != null)
                        {
                            Merge( existingNodeInBase, inheritedNode );
                            continue;
                        }
                    }
                    else if (!IsMultiOccureNode( inheritedNode ))
                    {
                        var existingNodeInBase = baseNodeGroup.FirstOrDefault();
                        if (existingNodeInBase != null)
                        {
                            Merge( existingNodeInBase, inheritedNode );
                            continue;
                        }
                    }
                    baseElement.Add( inheritedNode );
                }
            }
        }

        /// <summary>
        /// If a given xml node has an identifier attribute, it returns its name.
        /// </summary>
        private string GetIdentifierAttribute( XElement element )
        {
            string[] identifierAttributePrecedence = GetIdentifierAttributePrecedence();
            for (int i = 0; i < identifierAttributePrecedence.Length; i++)
            {
                if (element.Attribute( identifierAttributePrecedence[ i ] ) != null)
                {
                    return identifierAttributePrecedence[ i ];
                }
            }
            return null;
        }

        /// <summary>
        /// Goes through the attributes of the inheritedElement. If the baseElement has the same attribute, its value will be overwritten, if not, it will be added.
        /// The identifier attribute is an exception because they are not overwritten (but added if not in baseElement).
        /// </summary>
        private void OverrideAttributeValues( XElement baseElement, XElement inheritedElement )
        {
            string identifierAttribute = GetIdentifierAttribute( inheritedElement );
            foreach (var inheritedAttribute in inheritedElement.Attributes())
            {
                var existingAttributeInBase = baseElement.Attribute( inheritedAttribute.Name );
                if (existingAttributeInBase == null)
                {
                    baseElement.Add( inheritedAttribute );
                }
                else if (inheritedAttribute.Name != identifierAttribute)
                {
                    existingAttributeInBase.Value = inheritedAttribute.Value;
                }
            }
        }
    }
