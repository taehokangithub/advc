package advc_utils;

import org.junit.jupiter.api.Test;

import advc_utils.Graphs.INode;
import advc_utils.Graphs.Node;

import static org.junit.jupiter.api.Assertions.*;

public class NodeTest {

    @Test
    void nodeBasicTest()
    {
        INode node1 = new Node("A");
        INode node2 = new Node("B");

        boolean ret = node1.addEdge(node2, 10);
        assertTrue(ret);

        ret = node1.addEdge(node2);
        assertFalse(ret);

        final long value = 173829384;
        final String strValue = "alsidjf32";
        final long distance = 382481234;

        node2.setValue(value);
        node2.setStrValue(strValue);
        node2.setDistanceFromRoot(distance);

        assertTrue(value == node2.getValue());
        assertTrue(strValue == node2.getStrValue());
        assertTrue(distance == node2.getDistanceFromRoot());
    }

    @Test
    void nodeEdgeTest()
    {
        INode node1 = new Node("A");
        INode node2 = new Node("B");
        INode node3 = new Node("C");

        node1.addEdge(node2, 3);
        node1.addEdge(node3, 5);

        var edges = node1.getEdges();
        assertTrue(edges.size() == 2);

        long sum = 0;
        for (var edge : edges) 
        {
            sum += edge.distance;
        }
        assertTrue(sum == 8);
    }
}
