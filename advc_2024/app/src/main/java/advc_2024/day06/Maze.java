package advc_2024.day06;

import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;
import advc_utils.Grid.Grid;
import advc_utils.Points.*;
import advc_utils.Points.IPoint.EDir;

public class Maze 
{
    enum Tile { Road, Block, HumanUpper, Guarded }
    static final private IPoint s_dummyPoint = new Point(0, 0);
    private Grid<Tile> m_grid = new Grid<>();
    private IPoint m_startingPointUpper = s_dummyPoint;

    public Maze(List<String> lines)
    {
        m_grid.initialiseGrid(lines, this::getTileFromChar);

        for(int y = 0; y < m_grid.getSize().y; y ++)
        {
            for (int x = 0; x < m_grid.getSize().x; x++)
            {
                IPoint p = new Point(x, y);
                final var t = m_grid.getTile(p);
                if (t == Tile.HumanUpper)
                {
                    m_startingPointUpper = new Point(p);
                    break;
                }
            }
        }
    }

    public Tile getTileFromChar(char c)
    {
        switch (c) 
        {
            case '#' : return Tile.Block;
            case '.' : return Tile.Road;
            case '^' : return Tile.HumanUpper;
            default : 
                throw new IllegalArgumentException();
        }
    }

    public int getTotalGuardTiles()
    {
        Actor actor = new Actor(m_startingPointUpper, PointStatic.getDir4Point(EDir.UP));

        do
        {
            m_grid.setTile(actor.GetLocation(), Tile.Guarded);
            moveActorByRule(actor, m_grid);
        }
        while(m_grid.isValid(actor.GetLocation()));

        AtomicInteger sum = new AtomicInteger(0);

        m_grid.forEach((_, t) -> {
            if (t == Tile.Guarded)
            {
                sum.incrementAndGet();
            }
        });

        return sum.get();
    }

    public int findObstructionPositions()
    {
        AtomicInteger sum = new AtomicInteger(0);
        getTotalGuardTiles(); // mark "guarded"
        m_grid.setTile(m_startingPointUpper, Tile.Road); // open starting point for road.
        var copiedGrid = new Grid<Tile>(m_grid);

        m_grid.forEachRaw((x, y, tile) ->
        {
            if (tile == Tile.Guarded)
            {
                copiedGrid.setTileFast(x, y, Tile.Block);

                if (hasLoop(copiedGrid))
                {
                    sum.incrementAndGet();
                }

                copiedGrid.setTileFast(x, y, Tile.Guarded);
            }
        });

        return sum.get();
    }

    private boolean hasLoop(Grid<Tile> grid)
    {
        Actor actor = new Actor(m_startingPointUpper, PointStatic.getDir4Point(EDir.UP));

        var visitedGrid = new Grid<IPoint>();
        visitedGrid.initialiseGrid(m_grid.getSize(), s_dummyPoint);

        do
        {
            var loc = actor.GetLocation();
            var vel = actor.GetVelocity();
            
            if (visitedGrid.getTileFast(loc.getX(), loc.getY()).equals(vel))
            {
                return true;
            }

            visitedGrid.setTileFast(loc.getX(), loc.getY(), vel);
            moveActorByRule(actor, grid);
        }
        while(grid.isValid(actor.GetLocation()));

        return false;
    }

    private void moveActorByRule(Actor actor, Grid<Tile> grid)
    {
        // Emergency optimisation - normally shouldn't do that
        Point actorLoc = (Point) actor.GetLocation();

        for (int i = 0; i < 4; i ++)
        {
            IPoint direction = actor.GetVelocity();
            final long nextX = actorLoc.x + direction.getX();
            final long nextY = actorLoc.y + direction.getY();

            if (grid.isValidFast(nextX, nextY) && grid.getTileFast(nextX, nextY) == Tile.Block)
            {
                actor.SetVelocity(direction.getRotated(EDir.RIGHT));

                /* no idea why it doesn work (memory full) : need to investigate
                System.out.print("Velocity from " + direction.toString());
                direction.rotate(EDir.RIGHT);
                System.out.print(" to " + direction.toString());
                actor.SetVelocity(direction);
                System.out.println(" to  " + actor.GetVelocity().toString());
                */
            }
            else 
            {
                actorLoc.x = nextX;
                actorLoc.y = nextY;
                break;
            }
        }
    }
}