package advc_2018.day07;

import advc_utils.Graphs.*;
import advc_utils.Etc.SplitHelper;
import java.util.ArrayList;
import java.util.HashSet;
import java.util.List;
import java.util.PriorityQueue;

public class JobGraph 
{
    private Graph m_graph = new Graph();

    public JobGraph(List<String> lines)
    {
        for (var line : lines)
        {
            var parts = SplitHelper.CheckSplit(line, " must be finished before step ", 2);
            String namePart = parts[0];
            String targetPart = parts[1];
            final char nameChar = namePart.charAt(namePart.length() - 1);
            final char targetChar = targetPart.charAt(0);

            m_graph.addDirectedEdge(Character.toString(nameChar), Character.toString(targetChar));
        }
    }

    public Graph get()
    {
        return m_graph;
    }

    public int getTotalSeconds(int baseSeconds, int numWorkers)
    {
        int seconds = 0;
        var workers = new ArrayList<Worker>();
        var idleWorkers = new ArrayList<Worker>();
        var visited = new HashSet<String>();
        var pq = new PriorityQueue<INode>(new JobComparatorByName());

        for (INode node : m_graph.getNodes())
        {
            if (canVisit(node, visited))
            {
                pq.add(node);
            }
        }

        for (int i = 0; i < numWorkers; i ++)
        {
            workers.add(new Worker(pq, visited, baseSeconds));
        }

        boolean hasAnyJob = true;

        while (hasAnyJob)
        {
            hasAnyJob = false;

            idleWorkers.clear();

            for (var worker : workers)
            {
                worker.run();

                if (worker.hasJob())
                {
                    hasAnyJob = true;
                }
                else
                {
                    idleWorkers.add(worker);
                }
            }

            // late jobs - for example if the last worker finishes a job while other workers were idle - give the idle workers another chance
            if (hasAnyJob && !idleWorkers.isEmpty() && !pq.isEmpty())
            {
                for (var worker : idleWorkers)
                {
                    worker.run();
                }
            }

            seconds ++;
        }

        return seconds - 1;
    }

    public String getFullPath()
    {
        var sb = new StringBuilder();
        var pq = new PriorityQueue<INode>(new JobComparatorByName());
        var visited = new HashSet<String>();

        for (INode node : m_graph.getNodes())
        {
            if (canVisit(node, visited))
            {
                pq.add(node);
            }
        }

        while (!pq.isEmpty())
        {
            var node = pq.poll();
            sb.append(node.getName());
            visited.add(node.getName());

            for (var edge : node.getEdges())
            {
                if (canVisit(edge.target, visited))
                {
                    pq.add(edge.target);
                }
            }
            
        }
        if (sb.length() != m_graph.getNodes().size())
        {
            System.err.println(String.format("ERROR! path length %d, nodes %d", sb.length(), m_graph.getNodes().size()));
        }
        return sb.toString();
    }

    public static boolean canVisit(INode node, HashSet<String> visited)
    {
        if (!visited.contains(node.getName()))
        {
            boolean allFinished = true;

            for (var precondition : node.getEdges())
            {
                if (precondition.dir == Edge.Dir.EdgeTargetIsDestination)
                {
                    continue; // skip child, concerns only pre-condition(parents)
                }
                
                if (!visited.contains(precondition.target.getName()))
                {
                    allFinished = false;
                    break;
                }
            }
            return allFinished;
        }
        return false;
    }
}
