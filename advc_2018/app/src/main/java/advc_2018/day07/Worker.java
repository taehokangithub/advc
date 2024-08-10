package advc_2018.day07;

import advc_utils.Graphs.*;
import java.util.HashSet;
import java.util.PriorityQueue;

public class Worker 
{
    private Job m_job;
    private PriorityQueue<INode> m_pq;
    private HashSet<String> m_visited;
    private int m_baseSeconds;
    private int m_id; 
    private static int s_id_seed = 0;

    public Worker(PriorityQueue<INode> pq, HashSet<String> visited, int baseSeconds)
    {
        m_pq = pq;
        m_baseSeconds = baseSeconds;
        m_visited = visited;
        m_job = null;
        s_id_seed ++;
        m_id = s_id_seed;
    }

    public void run()
    {
        if (m_job != null)
        {
            m_job.run();
            if (m_job.isDone())
            {
                m_visited.add(m_job.getJobName());

                for (var edge : m_job.getNode().getEdges())
                {
                    if (JobGraph.canVisit(edge.target, m_visited))
                    {
                        m_pq.add(edge.target);
                    }
                }

                m_job = null;
            }
        }

        if (m_job == null)
        {
            var node = m_pq.poll();

            if (node != null)
            {
                String jobName = node.getName();
                final int jobTime = m_baseSeconds + 1 + (jobName.charAt(0) - 'A');
                m_job = new Job(node, jobTime);
            }
        }
    }

    public boolean hasJob()
    {
        return m_job != null;
    }
}
