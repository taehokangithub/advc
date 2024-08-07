package advc_utils.Map;

import java.util.HashMap;

import advc_utils.Points.*;

public class Map<T> implements IMap<T>
{
    private HashMap<String, T> m_HashMap = new HashMap<>();
    private IPoint m_max = new Point(Long.MIN_VALUE);
    private IPoint m_min = new Point(Long.MAX_VALUE);

    public boolean contains(IPoint p)
    {
        return m_HashMap.containsKey(p.toString());
    }

    public T getTile(IPoint p)
    {
        final String key = p.toString();
        if (m_HashMap.containsKey(key))
        {
            return m_HashMap.get(key);
        }
        return null; // It's weird for me but Java's enum is a reference type
    }

    public void setTile(IPoint p, T tile)
    {
        m_HashMap.put(p.toString(), tile);
        m_min.setMin(p);
        m_max.setMax(p);
    }

    // min/max coordinate getters
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
        return m_HashMap.size();
    }

    // Loops for each points stored in the map, calling the given ForEachCB
    public void forEachTile(ForEachTileCB<T> cb)
    {
        for (var entry : m_HashMap.entrySet())
        {
            cb.forEachTile(new Point(entry.getKey()), entry.getValue());
        }
    }

    public void forEachSpacePoint(ForEachSpacePointCB cb)
    {
        for (long y = m_min.getYlong(); y <= m_max.getYlong(); y ++)
        {
            for (long x = m_min.getXlong(); x <= m_max.getXlong(); x++)
            {
                cb.forEachSpacePoint(new Point(x, y));
            }
        }
    }
}
