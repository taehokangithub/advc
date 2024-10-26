package advc_2018.day10;

import advc_utils.Etc.SplitHelper;
import advc_utils.Map.IMap;
import advc_utils.Map.Map;
import advc_utils.Points.Actor;
import advc_utils.Points.IActor;
import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

import java.util.ArrayList;
import java.util.List;
import java.util.function.Function;
import java.util.concurrent.atomic.AtomicInteger;


public class PointsOfLights 
{
    private ArrayList<IActor> m_lights = new ArrayList<>();
    private IMap<Boolean> m_lightMap = null;

    public PointsOfLights(List<String> lines)
    {
        Function<String, IPoint> parsePoint = str ->
        {
            var newStr = str.replace("<", "").replace(">", "").replace(" ", "");
            var splitted = SplitHelper.CheckSplit(newStr, ",", 2);
            return new Point(Integer.parseInt(splitted[0]), Integer.parseInt(splitted[1]));
        };

        for (String line : lines) 
        {
            var splitted = SplitHelper.CheckSplit(line, " velocity=", 2);
            
            IPoint loc = parsePoint.apply(splitted[0].replace("position=<", ""));
            IPoint vel = parsePoint.apply(splitted[1]);
            m_lights.add(new Actor(loc, vel));
        }
    }

    public IMap<Boolean> GetOrCreateMap()
    {
        if (m_lightMap == null)
        {
            m_lightMap = new Map<Boolean>();

            for (var light : m_lights)
            {
                m_lightMap.setTile(light.GetLocation(), true);
            }
        }

        return m_lightMap;
    }

    public void MoveOneTurn()
    {
        for (var light: m_lights)
        {
            light.MoveOneTurn();
        }
        m_lightMap = null;
    }

    public int TryFindLetter()
    {
        final int howLong = 7;
        final int maxLoop = 100000;
        for (int i = 0; i < maxLoop; i ++)
        {
            MoveOneTurn();
            var length = FindMaxLength();
            if (length >= howLong)
            {
                return i + 1;
            }
        }
        throw new IllegalStateException(String.format("Could not find a letter after %d seconds", maxLoop));
    }

    public int FindMaxLength()
    {
        var lightMap = GetOrCreateMap();
        AtomicInteger maxLength = new AtomicInteger(0);

        lightMap.forEachTile((p, val) ->
        {
            final int limit = 256;
            for (int i = 1; i <= limit; i ++)
            {
                if (!lightMap.contains(new Point(p.getX(), p.getY() + i)))
                {
                    break;
                }
                maxLength.set(Math.max(maxLength.get(), i));
            }
        });

        return maxLength.get();
    }

    @Override
    public String toString()
    {
        var lightMap = GetOrCreateMap();
        StringBuilder sb = new StringBuilder();

        lightMap.forEachSpacePoint((IPoint p) -> 
        {
            if (p.getX() == lightMap.getMin().getX())
            {
                sb.append("\n");
            }
            sb.append(lightMap.contains(p) ? "#" : ".");
        });

        return sb.toString();
    }
}
