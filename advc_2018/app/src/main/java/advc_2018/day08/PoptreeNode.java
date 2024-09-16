package advc_2018.day08;

import java.util.ArrayList;
import java.util.LinkedList;
import java.util.List;

public class PoptreeNode 
{
    private List<PoptreeNode> m_children = new ArrayList<>();
    private List<Integer> m_metadata = new ArrayList<>();

    public void parse(LinkedList<Integer> params)
    {
        final int numChilren = params.pop();
        final int numMetadata = params.pop();

        for (int i = 0; i < numChilren; i ++)
        {
            var child = new PoptreeNode();
            m_children.add(child);
            
            child.parse(params);
        }

        for (int i = 0; i < numMetadata; i ++)
        {
            m_metadata.add(params.pop());
        }
    }

    public int getSumMetadata()
    {
        int sum = 0;
        for (var child : m_children)
        {
            sum += child.getSumMetadata();
        }

        for (var medadata : m_metadata)
        {
            sum += medadata;
        }
        return sum;
    }

    public int getMetaValue()
    {
        if (m_children.isEmpty())
        {
            return getSumMetadata();
        }

        int sum = 0;

        for (var index : m_metadata)
        {
            if (index >= 1 && index <= m_children.size())
            {
                sum += m_children.get(index - 1).getMetaValue();
            }
        }
        return sum;
    }
}
