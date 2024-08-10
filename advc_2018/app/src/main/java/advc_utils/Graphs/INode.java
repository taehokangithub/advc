package advc_utils.Graphs;

import java.util.Collection;

public interface INode extends Comparable<INode>{

    String getName();
    
    // distance from root
    void setDistanceFromRoot(long value);
    long getDistanceFromRoot();

    // numeric (long) value
    void setValue(long value);
    long getValue();

    // string value 
    void setStrValue(String value);
    String getStrValue();

    // Edges
    boolean addEdge(INode target);
    boolean addEdge(INode target, long distance);
    boolean addDirectedEdge(INode target, Edge.Dir dir);
    boolean addDirectedEdge(INode target, long distance, Edge.Dir dir);
    Collection<Edge> getEdges();
}
