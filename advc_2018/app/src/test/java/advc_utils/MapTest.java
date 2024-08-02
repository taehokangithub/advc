package advc_utils;


import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import advc_utils.Map.*;
import advc_utils.Points.*;

import static org.junit.jupiter.api.Assertions.*;

public class MapTest {
    private enum Tile { WALL, ROAD, HUMAN, ERROR };

    private Map<Tile> m_map;

    public Tile getTileFromChar(char c)
    {
        switch (c) 
        {
            case '#' : return Tile.WALL;
            case '.' : return Tile.ROAD;
            case 'O' : return Tile.HUMAN;
            default : 
                assert false;
                return Tile.ERROR;
        }
    }

    @BeforeEach
    void initTest()
    {
        m_map = new Map<>();

        m_map.setTile(new Point(0, 1, 2, 0), Tile.WALL);
        m_map.setTile(new Point(0, 12, 0, 1), Tile.WALL);
        m_map.setTile(new Point(10, -1, 21, 3), Tile.ROAD);
        m_map.setTile(new Point(30, 0, 22, 5), Tile.ROAD);
        m_map.setTile(new Point(-190, 15, -2, -1), Tile.ROAD);
        m_map.setTile(new Point(0, -21, -32, -5), Tile.HUMAN);
        m_map.setTile(new Point(20, -1, 25, -10), Tile.HUMAN);
        m_map.setTile(new Point(100, 11, -12, -21), Tile.HUMAN);
        m_map.setTile(new Point(50, 21, 122, 7), Tile.WALL);
        m_map.setTile(new Point(-70, -8, -2, 1), Tile.WALL);

        //max : 100, 21, 122, 7
        //min : -190, -21, -32, -21
    }

    @Test
    void testContains()
    {
        assertTrue(m_map.contains(new Point(-190, 15, -2, -1)));
        assertTrue(m_map.contains(new Point(100, 11, -12, -21)));
        assertFalse(m_map.contains(new Point(99, 5, -1, -21)));
    }

    @Test
    void testGetter()
    {
        assertTrue(m_map.getTile(new Point(20, -1, 25, -10)) == Tile.HUMAN);
        assertTrue(m_map.getTile(new Point(50, 21, 122, 7)) == Tile.WALL);
        assertTrue(m_map.getTile(new Point(30, 0, 22, 5)) == Tile.ROAD);
    }

    @Test
    void testMinMax()
    {
        assertTrue(m_map.getMax().equals(new Point(100, 21, 122, 7)));
        assertTrue(m_map.getMin().equals(new Point(-190, -21, -32, -21)));
    }
}
