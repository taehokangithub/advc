package advc_2018.day08;

import java.util.LinkedList;

public class Poptree 
{
    private PoptreeNode m_root;

    public Poptree(String str)
    {
        m_root = new PoptreeNode();
        var paramsStr =  str.split(" ");
        var params = new LinkedList<Integer>();
        for (var paramStr : paramsStr)
        {
            params.add(Integer.parseInt(paramStr));
        }
        m_root.parse(params);
    }

    public int getSumMetadata()
    {
        return m_root.getSumMetadata();
    }

    public int getMetaValue()
    {
        return m_root.getMetaValue();
    }
}
