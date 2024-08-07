package advc_2018.day06;

import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

public class AreaNode 
{
    private int m_id;
    private IPoint m_point;
    private static int s_id_number = 0;

    public AreaNode(IPoint point)
    {
        m_point = new Point(point);
        m_id = s_id_number ++;
    }

    public int getId()
    {
        return m_id;
    }

    public IPoint getPoint()
    {
        return m_point;
    }
}
