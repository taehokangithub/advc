package advc_utils.Graphs;

import java.util.Collection;

public interface IGraph 
{
    // Node
    void addNode(String name);
    void addNode(String name, long value, String strValue);
    INode getNode(String name);

    // Typical decalre-node-by-edge style in advc problems. 
    // You can call it without adding nodes explicitly
    void addEdge(String name1, String name2, int distance);
    void addEdge(String name1, String name2);

    void addDirectedEdge(String name1, String name2);
    void addDirectedEdge(String name1, String name2, int distance);

    // Manually set root node
    void setRoot(String name);
    INode getRoot();

    // Returns node collection view
    Collection<INode> getNodes();

    // Distance utils
    void setAllDistanceFromRoot();
    long getDistanceBetween(String name1, String name2);
}
