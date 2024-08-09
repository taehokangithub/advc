package advc_2018.day06;

import advc_utils.Map.IMap;
import advc_utils.Points.IPoint;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;

public class AreaMap 
{
    private IMap<AreaNode> m_map;

    public AreaMap(List<String> lines)
    {
        m_map = AreaMapParser.parse(lines);
    }

    public int getLargestInternalArea()
    {
        HashSet<Integer> boundaryNodes = new HashSet<>();
        HashMap<Integer, Integer> internalNodes = new HashMap<>();

        m_map.forEachSpacePoint((IPoint spacePoint) ->
        {
            int minDistance = Integer.MAX_VALUE;
            int minId = Integer.MAX_VALUE;

            for (var tilePoint : m_map.getTilePoints())
            {
                IPoint diff = spacePoint.getSubbed(tilePoint);
                final var manhattanDistance = (int)diff.getManhattanDistance();

                if (manhattanDistance < minDistance)
                {
                    final var node = m_map.getTile(tilePoint);
                    minId = node.getId();
                    minDistance = manhattanDistance;
                }
            }

            if (m_map.isBoundary(spacePoint))
            {
                boundaryNodes.add(minId);
                internalNodes.remove(minId);
            }
            else if (!boundaryNodes.contains(minId))
            {
                internalNodes.put(minId, internalNodes.getOrDefault(minId, 0) + 1);
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
        return largestAreaValue;
    }
}
