using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advc.Utils
{
    public class TreeNode
    {
        public TreeNode? Parent { get; set; } = null;
        public List<TreeNode> Children { get; set; } = new();
        public string Name { get; set; } = string.Empty;
        public int Depth { get; set; } = 0;
        public int Cost { get; set; } = 1;
        public long Value { get; set; } = default!;
        public string Data { get; set; } = string.Empty;
    }

    // --------------------------------------------------------
    // TreeBase : Generic tree
    // --------------------------------------------------------
    public class TreeBase
    {
        public bool AllowLogDetail { get; set; } = false;
        public TreeNode Root { get; private set; } = new();
        public delegate void OnNodeCallback(TreeNode node);
        private bool m_hasAnyRootSet = false; // to avoid all the null checks
        protected bool HasAnyRootSet => m_hasAnyRootSet;

        public void DFS(OnNodeCallback? preCallback, OnNodeCallback? postCallback)
        {
            Debug.Assert(Root.Depth == 0);
            LogDetail($"[DFS] Starting from root {Root.Name}");

            DFSInternal(Root, preCallback, postCallback);
        }
        
        public void TransformRoot(TreeNode root)
        {
            root.Depth = 0;
            TransformRootInternal(root, new());
            SetRoot(root);
        }
    #region protected methods
        protected void SetRoot(TreeNode node)
        {
            while (node.Parent != null)
            {
                node = node.Parent;
            }

            Root = node;
            m_hasAnyRootSet = true;
            LogDetail($"[TreeBase] Set Root {Root.Name}");
        }

        protected void LogDetail(string str)
        {
            if (AllowLogDetail)
            {
                Console.WriteLine(str);
            }
        }
    #endregion
    #region private methods
        private void TransformRootInternal(TreeNode node, HashSet<TreeNode> visited)
        {
            List<TreeNode> toTravese = new();

            var addIfNotVisited = (TreeNode? n) => 
            {
                if (n != null && !visited.Contains(n))
                {
                    toTravese.Add(n);
                }
            };

            addIfNotVisited(node.Parent);

            foreach (var child in node.Children)
            {
                addIfNotVisited(child);
            }

            visited.Add(node);
            node.Children = toTravese;

            foreach (var child in node.Children)
            {
                child.Depth = node.Depth + 1;
                TransformRootInternal(child, visited);
            }
        }

        private void DFSInternal(TreeNode node, OnNodeCallback? preCallback, OnNodeCallback? postCallback)
        {
            if (node.Parent != null)
            {
                // One DFS means all depth are set
                node.Depth = node.Parent.Depth + 1;
            }

            if (preCallback != null)
            {
                preCallback(node);
            }
            
            foreach (var child in node.Children)
            {
                DFSInternal(child, preCallback, postCallback);
            }

            if (postCallback != null)
            {
                postCallback(node);
            }
        }
    #endregion
    }

    // --------------------------------------------------------
    // NamedTree : nodes can be added/found by their unique names
    // --------------------------------------------------------
    public class NamedTree : TreeBase
    {
        private Dictionary<string, TreeNode> m_nodes = new();
        
        public void AddNode(string parent, string child)
        {
            var nodeParent = GetOrCreateNodeByName(parent);
            var nodeChild = GetOrCreateNodeByName(child);

            if (!nodeParent.Children.Any(n => n.Name == child))
            {
                nodeParent.Children.Add(nodeChild);
                nodeChild.Parent = nodeParent;

                LogDetail($"{nodeParent.Name} adding a child {nodeChild.Name}, now {nodeParent.Children.Count} children [{string.Join(",",nodeParent.Children.Select(c => c.Name))}]");                
            }
            
            if (!HasAnyRootSet || Root == nodeChild)
            {
                SetRoot(nodeParent);
            }
        }

        public TreeNode GetNodeByName(string name)
        {
            return m_nodes[name];
        }

    #region private methods

        protected TreeNode? TryGetNodeByName(string name)
        {
            if (m_nodes.ContainsKey(name))
            {
                return GetNodeByName(name);
            }
            //LogDetail($"couldn't find {name} from [{string.Join(",",m_nodes.Select(n => n.Key))}]");
            return null;
        }

        protected TreeNode GetOrCreateNodeByName(string name)
        {
            var node = TryGetNodeByName(name);

            if (node == null)
            {
                node = new TreeNode();
                node.Name = name;
                m_nodes[name] = node;
            }

            return node;
        }
    #endregion
    }

}