package advc_utils.Map;

import java.util.Collection;
import java.util.HashMap;

import advc_utils.Points.*;

public class Map<T> implements IMap<T>
{
    private HashMap<IPoint, T> m_hasmMap = new HashMap<>();
    private IPoint m_max = new Point(Long.MIN_VALUE);
    private IPoint m_min = new Point(Long.MAX_VALUE);

    public boolean contains(IPoint p)
    {
        return m_hasmMap.containsKey(p);
    }

    public T getTile(IPoint p)
    {
        if (m_hasmMap.containsKey(p))
        {
            return m_hasmMap.get(p);
        }
        return null; // It's weird for me but Java's enum is a reference type
    }

    public void setTile(IPoint p, T tile)
    {
        m_hasmMap.put(p, tile);
        m_min.setMin(p);
        m_max.setMax(p);
    }

    public Collection<IPoint> getTilePoints()
    {
        return m_hasmMap.keySet();
    }

    public IPoint getMin()
    {
        return m_min;
    }

    public IPoint getMax()
    {
        return m_max;
    }

    public int getCount()
    {
        return m_hasmMap.size();
    }

    public boolean isBoundary(IPoint p)
    {
        return (p.getX() == m_min.getX() ||
                p.getX() == m_max.getX() ||
                p.getY() == m_min.getY() ||
                p.getY() == m_max.getY());
    }

    // Loops for each points stored in the map, calling the given ForEachCB
    public void forEachTile(ForEachTileCB<T> cb)
    {
        for (var entry : m_hasmMap.entrySet())
        {
            cb.forEachTile(new Point(entry.getKey()), entry.getValue());
        }
    }

    public void forEachSpacePoint(ForEachSpacePointCB cb)
    {
        for (long y = m_min.getY(); y <= m_max.getY(); y ++)
        {
            for (long x = m_min.getX(); x <= m_max.getX(); x++)
            {
                cb.forEachSpacePoint(new Point(x, y));
            }
        }
    }
}
