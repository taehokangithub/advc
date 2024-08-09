package advc_2018.day06;

import advc_utils.Etc.SplitHelper;
import advc_utils.Map.*;
import advc_utils.Points.*;


import java.util.List;

class AreaMapParser 
{
    public static Map<AreaNode> parse(List<String> lines)
    {
        Map<AreaNode> map = new Map<>();

        for (var line : lines)
        {
            var parts = SplitHelper.CheckSplit(line, ", ", 2);

            final long x = Long.parseLong(parts[0]);
            final long y = Long.parseLong(parts[1]);
            IPoint point = new Point(x, y);

            map.setTile(point, new AreaNode(point));
        }

        return map;
    }
}
