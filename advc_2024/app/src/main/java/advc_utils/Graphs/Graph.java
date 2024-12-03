package advc_utils.Graphs;

import java.util.Collection;
import java.util.HashMap;

public class Graph implements IGraph
{
    private HashMap<String, INode> m_nodeMap = new HashMap<>();
    private INode m_root;

    @Override
    public void addNode(String name)
    {
        addNode(name, 0, "");
    }

    @Override
    public void addNode(String name, long value, String strValue)
    {
        if (m_nodeMap.containsKey(name))
        {
            throw new IllegalArgumentException("Error! duplicated node name is not allowed");
        }

        var node = getOrCreateNode(name);
        node.setValue(value);
        node.setStrValue(strValue);
        m_nodeMap.put(name, node);
    }

    @Override
    public INode getNode(String name)
    {
        return m_nodeMap.get(name);
    }

    @Override
    public void addDirectedEdge(String name1, String name2, int distance)
    {
        var node1 = getOrCreateNode(name1);
        var node2 = getOrCreateNode(name2);

        if (!node1.addDirectedEdge(node2, distance, Edge.Dir.EdgeTargetIsDestination) 
            || !node2.addDirectedEdge(node1, distance, Edge.Dir.EdgeTargetIsSource))
        {
            throw new IllegalArgumentException("Error! possibly duplicated node between " + node1.getName() + " and " + node2.getName());
        }
        
        if (m_root == null)
        {
            m_root = node1;
        }
    }

    @Override
    public void addDirectedEdge(String name1, String name2)
    {
        this.addDirectedEdge(name1, name2, 1);
    }

    @Override
    public void addEdge(String name1, String name2, int distance)
    {
        var node1 = getOrCreateNode(name1);
        var node2 = getOrCreateNode(name2);

        if (!node1.addEdge(node2, distance) || !node2.addEdge(node1, distance))
        {
            throw new IllegalArgumentException("Error! possibly duplicated node between " + node1.getName() + " and " + node2.getName());
        }
        
        if (m_root == null)
        {
            m_root = node1;
        }
    }

    @Override
    public void addEdge(String name1, String name2)
    {
        this.addEdge(name1, name2, 1);
    }

    @Override
    public void setRoot(String name)
    {
        if (!m_nodeMap.containsKey(name))
        {
            throw new IllegalArgumentException("setRoot node name " + name + " doesn't exist");
        }

        m_root = getOrCreateNode(name);
    }

    @Override
    public INode getRoot()
    {
        return m_root;
    }

    @Override
    public Collection<INode> getNodes()
    {
        return m_nodeMap.values();
    }

    @Override
    public void setAllDistanceFromRoot()
    {
        GraphUtils.setAllDistanceFromRoot(this);
    }

    @Override
    public long getDistanceBetween(String name1, String name2)
    {
        return GraphUtils.getDistanceBetween(this, name1, name2);
    }

    private INode getOrCreateNode(String name)
    {
        var node = m_nodeMap.get(name);
        if (node == null)
        {
            node = new Node(name);
            m_nodeMap.put(name, node);
        }
        return node;
    }
}
