package advc_utils;

import org.junit.jupiter.api.Test;

import advc_utils.Graphs.Graph;
import advc_utils.Graphs.IGraph;
import advc_utils.Graphs.INode;

import static org.junit.jupiter.api.Assertions.*;

public class GraphTest {

    @Test
    void graphNodeTest()
    {
        IGraph g = new Graph();

        final String name = "A-NODE-NAME";
        final long value = 15;
        final String sval = "sval";
        g.addNode(name, value, sval);
        g.addNode("B", value * 2, sval + "11");

        INode node = g.getNode(name);

        assertTrue(node != null, "node is null");
        assertTrue(node.getName() == name, "getName() " + node.getName() + " != " + name);
        assertTrue(node.getValue() == value, "getValue() " + node.getValue() + " != " + value);
        assertTrue(node.getStrValue() == sval, "getStrValue() " + node.getStrValue() + " != " + sval);
    }

    @Test
    void graphEdgeTest()
    {
        IGraph g = new Graph();
        
        g.addEdge("A", "B", 3);
        g.addEdge("A", "C");

        assertThrows(Exception.class, () -> g.addEdge("A", "C", 3));
        assertThrows(Exception.class, () -> g.addNode("A", 10, "val"));

        g.addNode("D");
        g.addEdge("A", "D", 5);
        g.addEdge("B", "E", 11);

        var cnt = g.getNodes().size();
        assertTrue(cnt == 5);
    }

    @Test
    void GraphDistanceTest()
    {
        IGraph graph = new Graph();

        graph.addEdge("A", "B", 12);
        graph.addEdge("A", "C", 3);
        graph.addEdge("A", "D", 9);
        graph.addEdge("C", "B", 13);
        graph.addEdge("C", "D", 5);
        graph.addEdge("D", "E", 2);
        graph.addEdge("E", "F", 6);
        graph.addEdge("B", "G", 1);
        graph.addEdge("F", "G", 3);
        graph.addEdge("E", "G", 1);
        /*
         *  Dijkstra all : node A : 0
            Dijkstra all : node C : 3
            Dijkstra all : node D : 8
            Dijkstra all : node E : 10
            Dijkstra all : node G : 11
            Dijkstra all : node B : 12
            Dijkstra all : node F : 14
         */

        var nodes = graph.getNodes();
        final int expectedSize = 7;
        assertTrue(nodes.size() == expectedSize, "nodes.size() " + nodes.size() + " != " + expectedSize); 

        graph.setAllDistanceFromRoot();

        checkGraphNodeDistance(graph, "F", 14);
        checkGraphNodeDistance(graph, "G", 11);
    }

    @Test 
    void GraphDistanceTest2()
    {
        IGraph graph = new Graph();
        
        graph.addEdge("Root", "A", 1);
        graph.addEdge("A", "B", 3);
        graph.addEdge("A", "C", 2);
        graph.addEdge("B", "D", 6);
        graph.addEdge("C", "D", 3);

        graph.addEdge("D", "E", 2);
        graph.addEdge("E", "H", 1);
        graph.addEdge("D", "F", 4);
        graph.addEdge("F", "G", 3);
        graph.addEdge("G", "H", 2);

        var nodes = graph.getNodes();

        final int expectedSize = 9;
        assertTrue(nodes.size() == expectedSize, "nodes.size() " + nodes.size() + " != " + expectedSize); 

        var root = graph.getRoot();
        assertTrue(root.getName() == "Root", "root name is " + root.getName());
        graph.setAllDistanceFromRoot();

        checkGraphNodeDistance(graph, "D", 6);
        checkGraphNodeDistance(graph, "G", 11);
    }

    @Test 
    void GraphDistanceTestBetween()
    {
        IGraph graph = new Graph();
        
        graph.addEdge("Root", "A", 1);
        graph.addEdge("A", "B", 3);
        graph.addEdge("A", "C", 2);
        graph.addEdge("B", "D", 6);
        graph.addEdge("C", "D", 3);

        graph.addEdge("D", "E", 2);
        graph.addEdge("E", "H", 1);
        graph.addEdge("D", "F", 4);
        graph.addEdge("F", "G", 3);
        graph.addEdge("G", "H", 2);

        graph.setRoot("Root");
        graph.setAllDistanceFromRoot();
        checkGraphNodeDistanceBetween(graph, "Root", "G", graph.getNode("G").getDistanceFromRoot());
        checkGraphNodeDistanceBetween(graph, "Root", "H", graph.getNode("H").getDistanceFromRoot());

        graph.setRoot("E");
        graph.setAllDistanceFromRoot();
        checkGraphNodeDistanceBetween(graph, "E", "Root", graph.getNode("Root").getDistanceFromRoot());
        checkGraphNodeDistanceBetween(graph, "E", "B", 8);
        checkGraphNodeDistanceBetween(graph, "D", "H", 3);

    }

    private void checkGraphNodeDistance(IGraph graph, String name, long expectedDistance)
    {
        var node = graph.getNode(name);
        assertTrue(node.getDistanceFromRoot() == expectedDistance, "Distance " + name + node.getDistanceFromRoot() + " != " + expectedDistance);
    }

    private void checkGraphNodeDistanceBetween(IGraph graph, String name1, String name2, long expectedDistance)
    {
        long distance = graph.getDistanceBetween(name1, name2);
        assertTrue(distance == expectedDistance, "Distance between " + name1 + " and " + name2 + " expected " + expectedDistance + ", result " + distance);
    }
}
