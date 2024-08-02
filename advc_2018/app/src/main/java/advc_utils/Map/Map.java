package advc_utils.Map;

import java.util.HashMap;

import advc_utils.Points.*;

// TODO : TEST ERROR, 
//      ; CREATE A TEST FOR POINT FROM STRING (PARSING)

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

    // Loops for each points stored in the map, calling the given ForEachCB
    public void forEach(ForEachCB<T> cb)
    {
        for (var entry : m_HashMap.entrySet())
        {
            cb.forEachPoint(new Point(entry.getKey()), entry.getValue());
        }
    }
}
