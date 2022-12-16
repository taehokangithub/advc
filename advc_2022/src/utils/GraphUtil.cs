using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advc.Utils
{
    public class GraphNode
    {
        public List<GraphNode> Path { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public int Cost { get; set; } = 1;
        public long Value { get; set; } = default!;
        public string Data { get; set; } = string.Empty;
    }


    // --------------------------------------------------------
    // NamedGraph : nodes can be added/found by their unique names
    // --------------------------------------------------------
    public class NamedGraph : Loggable
    {
        private Dictionary<string, GraphNode> m_nodes = new();

        public IReadOnlyDictionary<string, GraphNode> Nodes => m_nodes;
        
        public (GraphNode, GraphNode) AddPath(string fromName, string toName)
        {
            var nodeFrom = GetOrCreateNodeByName(fromName);
            var nodeTo = GetOrCreateNodeByName(toName);

            // Allow only new relationship
            Debug.Assert(!nodeFrom.Path.Any(n => n.Name == toName));

            nodeFrom.Path.Add(nodeTo);

            LogDetail($"{nodeFrom.Name} adding a path {nodeTo.Name}, now {nodeFrom.Path.Count} path [{string.Join(",",nodeFrom.Path.Select(c => c.Name))}]");

            return (nodeFrom, nodeTo);
        }

        public GraphNode GetNodeByName(string name)
        {
            return m_nodes[name];
        }

        public bool TryGetNodeByName(string name, out GraphNode node)
        {
            if (m_nodes.ContainsKey(name))
            {
                node = GetNodeByName(name);
                return true;
            }
            
            node = default!;
            return false;
        }

        protected GraphNode GetOrCreateNodeByName(string name)
        {
            if (!TryGetNodeByName(name, out var node))
            {
                node = new GraphNode();
                node.Name = name;
                m_nodes[name] = node;
            }

            return node;
        }
    }

}