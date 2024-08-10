package advc_2018.day06;

import advc_utils.Etc.SplitHelper;
import advc_utils.Map.IMap;
import advc_utils.Map.Map;
import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

public class AreaMap 
{
    private IMap<Node> m_map;
    private int m_largestAreaValue;
    private int m_safeAreas;

    public AreaMap(List<String> lines)
    {
        m_map = parse(lines);
    }

    public int getLargestInternalArea()
    {
        return m_largestAreaValue;
    }

    public int getSafeAreas()
    {
        return m_safeAreas;
    }

    public void analyse(int safeAreaDistance)
    {
        HashSet<Integer> boundaryNodes = new HashSet<>();
        HashMap<Integer, Integer> internalNodes = new HashMap<>();

        m_map.forEachSpacePoint((IPoint spacePoint) ->
        {
            int minDistance = Integer.MAX_VALUE;
            int minId = Integer.MAX_VALUE;
            long sumDistance = 0;

            for (var tilePoint : m_map.getTilePoints())
            {
                final var manhattanDistance = (int)spacePoint.getManhattanDistance(tilePoint);
                sumDistance += manhattanDistance;

                if (manhattanDistance < minDistance)
                {
                    final var node = m_map.getTile(tilePoint);
                    minId = node.getId();
                    minDistance = manhattanDistance;
                }
            }

            if (sumDistance < safeAreaDistance)
            {
                m_safeAreas ++;
            }            

            if (!boundaryNodes.contains(minId))
            {
                if (m_map.isBoundary(spacePoint))
                {
                    boundaryNodes.add(minId);
                    internalNodes.remove(minId);
                }
                else
                {
                    internalNodes.put(minId, internalNodes.getOrDefault(minId, 0) + 1);
                }
            }
        });
        
        int largestAreaValue = Integer.MIN_VALUE;

        for (var areaValue : internalNodes.values())
        {
            if (areaValue > largestAreaValue)
            {
                largestAreaValue = areaValue;
            }
        }
        m_largestAreaValue = largestAreaValue;
    }

    private static Map<Node> parse(List<String> lines)
    {
        Map<Node> map = new Map<>();

        for (var line : lines)
        {
            var parts = SplitHelper.CheckSplit(line, ", ", 2);

            final long x = Long.parseLong(parts[0]);
            final long y = Long.parseLong(parts[1]);
            IPoint point = new Point(x, y);

            map.setTile(point, new Node(point));
        }

        return map;
    }    
}
