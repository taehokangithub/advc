package advc_2024.day14;

import java.util.ArrayList;
import java.util.List;

import advc_utils.Etc.Logger;
import advc_utils.Etc.SplitHelper;
import advc_utils.Etc.Logger.LoggerType;
import advc_utils.Grid.Grid;
import advc_utils.Points.Actor;
import advc_utils.Points.IPoint;
import advc_utils.Points.Point;
import advc_utils.Points.IPoint.EAxis;

public class Robots 
{
    private ArrayList<Actor> m_robots = new ArrayList<>();
    private Logger m_Logger = new Logger(LoggerType.DummyLogger);
    private IPoint m_areaSize;

    public Robots(List<String> lines, IPoint areaSize)
    {
        m_areaSize = areaSize;

        for (var line : lines)
        {
            var lineParts = SplitHelper.CheckSplit(line, " ", 2);
            var posParts = SplitHelper.CheckSplit(lineParts[0], "=", 2);
            var posLocParts = SplitHelper.CheckSplit(posParts[1], ",", 2);
            var velParts = SplitHelper.CheckSplit(lineParts[1], "=", 2);
            var velLocParts = SplitHelper.CheckSplit(velParts[1], ",", 2);

            final long posX = Long.parseLong(posLocParts[0]);
            final long posY = Long.parseLong(posLocParts[1]);
            final long velX = Long.parseLong(velLocParts[0]);
            final long velY = Long.parseLong(velLocParts[1]);

            Actor robot = new Actor(new Point(posX, posY), new Point(velX, velY));
            m_robots.add(robot);
        }
    }

    public long getSafetyFactor(int turns)
    {
        long[] count = new long[5];

        for (var robot : m_robots)
        {
            moveRobot(robot, turns);
            final int quadrant = getQuadrant(robot);
            count[quadrant] ++;

            m_Logger.log("Robot [%s] quadrant %d, count %d", robot.toString(), quadrant, count[quadrant]);
        }

        return count[1] * count[2] * count[3] * count[4];
    }

    public long findXMasTree()
    {
        long i = 0;

        do
        {
            i ++;
            for (var robot : m_robots)
            {
                moveRobot(robot, 1);
            }
        }
        while(!checkTree(15));

        return i;
    }

    private void moveRobot(Actor robot, int turns)
    {
        var location = robot.GetLocation();
        var velocity = robot.GetVelocity();

        long locX = location.getX();
        long locY = location.getY();

        locX = (locX + (velocity.getX() * turns)) % m_areaSize.getX();
        locY = (locY + (velocity.getY() * turns)) % m_areaSize.getY();
        
        while (locX < 0)
        {
            locX += m_areaSize.getX();
        }
        while (locY < 0 )
        {
            locY += m_areaSize.getY();
        }

        location.setAxisValue(EAxis.X, locX);
        location.setAxisValue(EAxis.Y, locY);
    }

    private int getQuadrant(Actor robot)
    {
        final long midX = m_areaSize.getX() / 2;
        final long midY = m_areaSize.getY() / 2;        
        final long x = robot.GetLocation().getX();
        final long y = robot.GetLocation().getY();

        if (x == midX || y == midY)
        {
            return 0;
        }

        if (y < midY)
        {
            if (x < midX)
            {
                return 1;
            }
            return 2;
        }
        if (x < midX)
        {
            return 3;
        }
        return 4;
    }

    private boolean checkTree(int maxLength)
    {
        final Character EMPTY = ' ';
        final Character FILLED = '|';

        Grid<Character> grid = new Grid<>();
        grid.initialiseGrid(m_areaSize, EMPTY);

        for (var robot : m_robots)
        {
            grid.setTile(robot.GetLocation(), FILLED);
        }

        for (var robot : m_robots)
        {
            boolean hasFound = true;
            final long x = robot.GetLocation().getX();
            final long y = robot.GetLocation().getY();

            for (int i = 0; i < maxLength; i ++)
            {
                if (!grid.isValidFast(x, y + i) || grid.getTileFast(x, y + i) == EMPTY)
                {
                    hasFound = false;
                    break;
                }
            }
            if (hasFound)
            {
                m_Logger.log("Found vertical line from %s", robot.GetLocation());
                m_Logger.log(grid.getGridString(c -> c));
                return true;
            }
        }

        return false;
    }
}
