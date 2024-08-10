package advc_2018.day07;

import advc_utils.Graphs.INode;

public class Job 
{
    private int m_remainingSeconds;
    private INode m_node;

    public Job(INode node, int seconds)
    {
        m_node = node;
        m_remainingSeconds = seconds;
    }

    public void run()
    {
        m_remainingSeconds --;

        if (m_remainingSeconds < 0)
        {
            throw new IllegalStateException("Remaining seconds " + m_remainingSeconds);
        }
    }

    public boolean isDone()
    {
        return m_remainingSeconds == 0;
    }

    public String getJobName()
    {
        return m_node.getName();
    }

    public INode getNode()
    {
        return m_node;
    }
}
