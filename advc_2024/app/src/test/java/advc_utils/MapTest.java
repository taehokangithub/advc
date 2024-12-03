package advc_utils;


import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import advc_utils.Map.*;
import advc_utils.Points.*;

import static org.junit.jupiter.api.Assertions.*;

public class MapTest {
    private enum Tile { WALL, ROAD, HUMAN, ERROR };

    private Map<Tile> m_map;
    private int m_cntLoop;

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
    void testSpacePointLoopCount()
    {
        m_cntLoop = 0;
        m_map.forEachSpacePoint((IPoint) ->
        {
            m_cntLoop ++;
        });

        IPoint maxPoint = m_map.getMax();
        IPoint minPoint = m_map.getMin();

        final long expected = (maxPoint.getX() - minPoint.getX() + 1) * (maxPoint.getY() - minPoint.getY() + 1);
        assertTrue(m_cntLoop == expected);
    }
    
    @Test
    void testTileLoopCount()
    {
        m_cntLoop = 0;
        m_map.forEachTile((IPoint p, Tile t) ->
        {
            m_cntLoop ++;
        });
        assertTrue(m_cntLoop == 10);
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

    @Test
    void testMinMax2()
    {
        Map<Integer> map = new Map<>();

        map.setTile(new Point(3,5,6,7), 0);
        map.setTile(new Point(1,2,9,8), 1);

        assertTrue(map.getMax().equals(new Point(3, 5, 9, 8)), map.getMax().toString());
        assertTrue(map.getMin().equals(new Point(1, 2, 6, 7)), map.getMin().toString());
    }
}
