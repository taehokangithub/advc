package advc_utils.Graphs;

import java.util.PriorityQueue;
import java.util.HashSet;

final class GraphUtils {
    
    static void setAllDistanceFromRoot(IGraph graph)
    {
        var pq = new PriorityQueue<INode>();

        var nodes = graph.getNodes();
        var root = graph.getRoot();

        root.setDistanceFromRoot(0);
        for (var node : nodes)
        {
            if (node != root)
            {
                node.setDistanceFromRoot(Long.MAX_VALUE);
            }
            pq.add(new Node(node));
        }

        HashSet<String> visited = new HashSet<>();

        while (!pq.isEmpty())
        {
            var node = pq.poll();
            if (visited.contains(node.getName()))
            {
                continue;
            }
            visited.add(node.getName());

            var edges = node.getEdges();
            for (var edge : edges)
            {
                final var target = edge.target;
                final long potentialDistance = node.getDistanceFromRoot() + edge.distance;

                if (target.getDistanceFromRoot() > potentialDistance)
                {
                    // Update original node in the graph
                    var originalNode = graph.getNode(target.getName());
                    originalNode.setDistanceFromRoot(potentialDistance);

                    var newNode = new Node(originalNode);
                    pq.add(newNode);
                }
            }
        }
    }

    // ---------- wrapper class to calculate distance without modifying objects ---------------
    private static class LocalNode implements Comparable<LocalNode>
    {
        public long distanceFromStart;
        public INode node;

        public LocalNode(INode other, long distance)
        {
            node = new Node(other);
            distanceFromStart = distance;
        }

        @Override
        public int compareTo(LocalNode other) 
        {
            return Long.compare(this.distanceFromStart, other.distanceFromStart);
        }
    }

    static long getDistanceBetween(IGraph graph, String name1, String name2)
    {
        var pq = new PriorityQueue<LocalNode>();

        INode startNode = graph.getNode(name1);
        for (var edge : startNode.getEdges())
        {
            var localNode = new LocalNode(edge.target, edge.distance);
            pq.add(localNode);
        }

        HashSet<String> visited = new HashSet<>();
        visited.add(name1);

        while(!pq.isEmpty())
        {
            var localNode = pq.poll();

            if (visited.contains(localNode.node.getName()))
            {
                continue;
            }
            visited.add(localNode.node.getName());
            
            if (localNode.node.getName() == name2)
            {
                return localNode.distanceFromStart;
            }

            for (var edge : localNode.node.getEdges())
            {
                if (!visited.contains(edge.target.getName()))
                {
                    pq.add(new LocalNode(edge.target, localNode.distanceFromStart + edge.distance));
                }
            }
        }

        throw new IllegalArgumentException("There is no path between " + name1 + " and " + name2);
    }
}
