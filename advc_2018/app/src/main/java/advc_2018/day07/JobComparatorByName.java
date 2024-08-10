package advc_2018.day07;

import advc_utils.Graphs.INode;
import java.util.Comparator;

public class JobComparatorByName implements Comparator<INode>
{
    @Override
    public int compare(INode a, INode b)
    {
        return a.getName().charAt(0) - b.getName().charAt(0);
    }
}
