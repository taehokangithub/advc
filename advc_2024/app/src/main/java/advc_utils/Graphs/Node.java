package advc_utils.Graphs;

import java.util.Collection;
import java.util.HashMap;

public class Node implements INode
{
    private String m_name;
    private long m_distanceFromRoot;
    private long m_value;
    private String m_strValue;
    private HashMap<String, Edge> m_edges = new HashMap<>();

    public Node(String name) 
    {
        m_name = name;
    }

    public Node(INode other)
    {
        m_name = other.getName();
        m_distanceFromRoot = other.getDistanceFromRoot();
        m_value = other.getValue();
        m_strValue = other.getStrValue();
        for (Edge edge : other.getEdges())
        {
            m_edges.put(edge.target.getName(), edge);
        }
    }

    @Override
    public int compareTo(INode other) {
        return Long.compare(this.getDistanceFromRoot(), other.getDistanceFromRoot());
    }    

    @Override
    public String getName()
    {
        return m_name;
    }

    @Override
    public void setDistanceFromRoot(long value) 
    {
        m_distanceFromRoot = value;
    }

    @Override
    public long getDistanceFromRoot() 
    {
        return m_distanceFromRoot;
    }

    @Override
    public void setValue(long value)
    {
        m_value = value;
    }

    @Override
    public long getValue()
    {
        return m_value;
    }

    @Override
    public void setStrValue(String value)
    {
        m_strValue = value;
    }

    @Override
    public String getStrValue()
    {
        return m_strValue;
    }

    @Override
    public boolean addEdge(INode target)
    {
        return this.addDirectedEdge(target, 1, Edge.Dir.NotDirected);
    }

    @Override
    public boolean addEdge(INode target, long distance)
    {
        return this.addDirectedEdge(target, distance, Edge.Dir.NotDirected);
    }

    @Override
    public boolean addDirectedEdge(INode target, Edge.Dir dir)
    {
        return this.addDirectedEdge(target, 1, dir);
    }

    @Override
    public boolean addDirectedEdge(INode target, long distance, Edge.Dir dir)
    {
        if (target == this)
        {
            System.err.println("Error! adding edge of the node itself" + m_name);
            return false;
        }
        
        if (m_edges.containsKey(target.getName()))
        {
            return false;
        }

        Edge edge = new Edge();
        edge.distance = distance;
        edge.target = target;
        edge.dir = dir;
        m_edges.put(target.getName(), edge);

        return true;
    }

    @Override
    public Collection<Edge> getEdges()
    {
        return m_edges.values();
    }


}
