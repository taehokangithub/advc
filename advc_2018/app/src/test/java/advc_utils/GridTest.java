package advc_utils;

import java.util.*;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import advc_utils.Grid.*;
import advc_utils.Points.Point;

import static org.junit.jupiter.api.Assertions.*;

public class GridTest {
    private enum Tile { WALL, ROAD, HUMAN, ERROR };

    private final String m_text = 
        "##########\n" +
        "#.....#..#\n" +
        "#...#.#..#\n" +
        "#..O#.#..#\n" +
        "##########\n";

    private IGrid<Tile> m_grid;
    private List<String> m_lines;

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
        m_grid = new Grid<Tile>();

        m_lines = Arrays.asList(m_text.split("\n"));
        m_grid.initialiseGrid(m_lines, this::getTileFromChar);
    }

    @Test
    void testMapMinBoundaryValid()
    {
        assertFalse(m_grid.isValid(new Point(-1, -2)));
        assertTrue(m_grid.isValid(new Point(0, 0)));
        assertTrue(m_grid.isValid(new Point(1, 0)));
        assertTrue(m_grid.isValid(new Point(0, 1)));
    }

    @Test
    void testMapMaxBoundaryValid()
    {
        assertFalse(m_grid.isValid(new Point(m_lines.getFirst().length(), m_lines.size())));
        assertFalse(m_grid.isValid(new Point(m_lines.getFirst().length() - 1, m_lines.size())));
        assertFalse(m_grid.isValid(new Point(m_lines.getFirst().length(), m_lines.size() - 1)));
        assertTrue(m_grid.isValid(new Point(m_lines.getFirst().length() - 1, m_lines.size() - 1)));
    }

    @Test
    void testGetTile()
    {
        assertTrue(m_grid.getTile(new Point(3, 3)) == Tile.HUMAN);
        assertTrue(m_grid.getTile(new Point(3, 4)) == Tile.WALL);
        assertTrue(m_grid.getTile(new Point(3, 2)) == Tile.ROAD);
    }

    @Test
    void testSetTile()
    {
        final Point p1 = new Point(1, 2);
        final Point p2 = new Point(2, 0);
        final Point p3 = new Point(0, 3);
        m_grid.setTile(p1, Tile.HUMAN);
        m_grid.setTile(p2, Tile.HUMAN);
        m_grid.setTile(p3, Tile.HUMAN);

        assertTrue(m_grid.getTile(p1) == Tile.HUMAN);
        assertTrue(m_grid.getTile(p2) == Tile.HUMAN);
        assertTrue(m_grid.getTile(p3) == Tile.HUMAN);
    }

    @Test
    void testGridString()
    {
        var ret_text = m_grid.getGridString();
        var ret_lines = Arrays.asList(ret_text.split("\n"));
        assertTrue(ret_lines.size() == m_lines.size());
        assertTrue(ret_lines.size() == 5, "ret lines " + ret_lines.size() + "  input lines " + m_lines.size());
    }
}
