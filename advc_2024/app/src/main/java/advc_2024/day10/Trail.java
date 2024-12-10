package advc_2024.day10;

import advc_utils.Etc.Logger;
import advc_utils.Etc.Logger.LoggerType;
import advc_utils.Grid.Grid;
import advc_utils.Points.IPoint;
import advc_utils.Points.PointStatic;

import java.util.ArrayDeque;
import java.util.HashSet;
import java.util.List;
import java.util.Queue;
import java.util.concurrent.atomic.AtomicInteger;

public class Trail 
{
    private static final int VAL_TRAINHEAD = 0;
    private static final int VAL_HIGHEST = 9;

    private Grid<Integer> m_grid = new Grid<>();
    private Logger m_logger = new Logger(LoggerType.DummyLogger);

    public enum Version { ver1, ver2 };

    public Trail(List<String> lines)
    {
        m_grid.initialiseGrid(lines, c -> (c - '0'));
    }

    public int getTotalTrailheadsScores(Version ver)
    {
        AtomicInteger sum = new AtomicInteger(0);

        m_grid.forEach((p, _) ->
        {
            if (m_grid.getTile(p) == VAL_TRAINHEAD)
            {
                sum.addAndGet(getTrailheadScore(p, ver == Version.ver1));
            }
        });

        return sum.get();
    }

    private int getTrailheadScore(IPoint trailhead, boolean removeDuplicatedPath)
    {
        int sum = 0;
        Queue<IPoint> q = new ArrayDeque<IPoint>();
        HashSet<String> visited = new HashSet<>();

        q.add(trailhead);

        while (!q.isEmpty())
        {
            final IPoint curLoc = q.poll();
            final int curVal = m_grid.getTile(curLoc);

            for (var dir : PointStatic.Dir4Points)
            {
                final IPoint newLoc = curLoc.getAdded(dir);
                if (!m_grid.isValid(newLoc))
                {
                    continue;
                }

                final int newVal = m_grid.getTile(newLoc);

                if (newVal == curVal + 1)
                {
                    if (removeDuplicatedPath)
                    {
                        if (visited.contains(newLoc.toString()))
                        {
                            continue;
                        }
                        visited.add(newLoc.toString());
                    }

                    if (newVal == VAL_HIGHEST)
                    {
                        sum ++;  // found!
                        m_logger.log("Found path from %s to %s, total %d so far", trailhead.toString(), newLoc.toString(), sum);

                    }
                    else
                    {
                        m_logger.log("Moving from %s(%d) to %s(%d)", curLoc.toString(), curVal, newLoc.toString(), newVal);
                        q.add(newLoc);
                    }
                }

            }
        }
        
        return sum;
    }
}
