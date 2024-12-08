package advc_utils.Grid;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

import static org.junit.jupiter.api.Assertions.*;

public class Grid<T> implements IGrid<T>
{
    private List<List<T>> m_grid = new ArrayList<List<T>>();
    private Map<T, Character> m_tileToChar = new HashMap<T, Character>();
    private Point m_size;

    public Grid()
    {
    }

    public Grid(IGrid<T> other)
    {
        m_size = new Point(other.getSize());
        
        for (int y = 0; y < m_size.getY(); y ++)
        {
            var lineList = new ArrayList<T>();
            m_grid.add(lineList);

            for (int x = 0; x < m_size.getX(); x ++)
            {
                lineList.add(other.getTile(new Point(x, y)));
            }
        }

        m_tileToChar = other.getTileToCharMap();
    }

    @Override
    public Point getSize() { return m_size; }

    @Override
    public Map<T, Character> getTileToCharMap()
    {
        return m_tileToChar;
    }

    @Override
    public boolean isValid(IPoint p)
    {
        assertTrue(p.getNumAxis() == 2); // ensure it's a 2D point
        return isValidFast(p.getX(), p.getY());
    }

    @Override
    public boolean isValidFast(long x, long y)
    {
        return x >= 0 && y >= 0 && x < m_size.getX() && y < m_size.getY();
    }
    
    @Override
    public T getTile(IPoint p) 
    {
        assertTrue(p.getNumAxis() == 2); // ensure it's a 2D point
        if (!isValid(p))
        {
            assertTrue(false);
            return null;
        }
        return getTileFast(p.getX(), p.getY());
    }

    @Override
    public T getTileFast(long x, long y)
    {
        return m_grid.get((int)y).get((int)x);
    }

    @Override
    public void setTile(IPoint p, T tile)
    {
        assertTrue(p.getNumAxis() == 2); // ensure it's a 2D point
        if (!isValid(p))
        {
            throw new IllegalArgumentException();
        }        
        setTileFast(p.getX(), p.getY(), tile);
    }

    @Override
    public void setTileFast(long x, long y, T tile)
    {
        m_grid.get((int)y).set((int)x, tile);
    }

    @Override
    public void initialiseGrid(List<String> lines, IGrid.CharToTileCB<T> cb)
    {
        m_grid.clear();

        for (var line : lines)
        {
            var lineList = new ArrayList<T>();
            m_grid.add(lineList);

            for (char c : line.toCharArray())
            {
                final T t = cb.getTileFromChar(c);
                lineList.add(t);

                // todo: here add the tile-to-char map
                m_tileToChar.put(t, c);
            }
        }

        m_size = new Point(m_grid.get(0).size(), m_grid.size());
    }

    @Override
    public void initialiseGrid(IPoint size, T defaultValue)
    {
        m_size = new Point(size);
        m_grid.clear();

        for (int y = 0; y < size.getY(); y ++)
        {
            var lineList = new ArrayList<T>();
            m_grid.add(lineList);

            for (int x = 0; x < size.getX(); x ++)
            {
                lineList.add(defaultValue);
            }
        }
    }

    @Override
    public String getGridString()
    {
        return getGridString(c -> m_tileToChar.get(c));
    }

    @Override
    public String getGridString(TileToCharCB<T> cb)
    {
        StringBuilder sb = new StringBuilder();
        forEach((p, t) ->
        {
            sb.append("" + cb.getCharFromTile(t));
            if (p.getX() == m_size.x - 1)
            {
                sb.append("\n");
            }
        });
        return sb.toString();
    }

    @Override
    public void forEach(IGrid.ForEachCB<T> cb)
    {
        for(int y = 0; y < m_size.y; y ++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                final Point p = new Point(x, y);
                final var t = m_grid.get(y).get(x);
                cb.forEachPoint(p, t);
            }
        }
    }

    @Override
    public void forEachRaw(IGrid.ForEachRawCB<T> cb)
    {
        for(int y = 0; y < m_size.y; y ++)
        {
            for (int x = 0; x < m_size.x; x++)
            {
                final var t = m_grid.get(y).get(x);
                cb.forEachPoint(x, y, t);
            }
        }
    }    
}
