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

    @Override
    public Point getSize() { return m_size; }

    @Override
    public boolean isValid(IPoint p)
    {
        assertTrue(p.getNumAxis() == 2); // ensure it's a 2D point
        return p.getX() >= 0 && p.getY() >= 0 && p.getX() < m_size.getX() && p.getY() < m_size.getY();
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
        return m_grid.get(p.getY()).get(p.getX());
    }

    @Override
    public void setTile(IPoint p, T tile)
    {
        assertTrue(p.getNumAxis() == 2); // ensure it's a 2D point
        assertTrue(isValid(p));
     
        m_grid.get(p.getY()).set(p.getX(), tile);
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
    public String getGridString()
    {
        StringBuilder sb = new StringBuilder();
        forEach((p, t) ->
        {
            sb.append(m_tileToChar.get(t));
            if (p.getXlong() == m_size.x - 1)
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
}
