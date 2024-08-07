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

        m_map.forEachSpacePoint((IPoint p) ->
        {
            // TODO : the forEach can't add up simple integer. too bad. 
            // might need a different method that returns a collection of points or entries

            // TODO : for each space point, find the nearest node.
            //        check if this is a boundary point
            m_map.forEachTile((IPoint tilePoint, AreaNode node) ->
            {

            });
        });
        
        return 0;
    }
}
