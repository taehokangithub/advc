package advc_2018.day03;

import advc_utils.Map.IMap;
import advc_utils.Map.Map;
import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

import java.util.ArrayList;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

public class Claims 
{
    private List<Claim> m_claims = new ArrayList<>();
    private IMap<Integer> m_map = new Map<Integer>();

    public Claims(List<String> lines)
    {
        for (var line : lines)
        {
            m_claims.add(new Claim(line));
        }
        applyClaims();
    }

    public void applyClaims()
    {
        for (var claim : m_claims)
        {
            for (int x = claim.getX(); x < claim.getX() + claim.getWidth(); x++)
            {
                for (int y = claim.getY(); y < claim.getY() + claim.getHeight(); y++)
                {
                    IPoint p = new Point(x, y);
                    Integer curVal = 0;

                    if (m_map.contains(p))
                    {
                        curVal = m_map.getTile(p);
                    }

                    curVal ++;
                    m_map.setTile(p, curVal);
                }
            }
        }
    }

    public int getMultipleClaimedTiles()
    {
        AtomicInteger count = new AtomicInteger(0);
        m_map.forEach((IPoint p, Integer val) -> {
            if (val > 1)
            {
                count.incrementAndGet();
            }
        });
        return count.get();
    }

    public int getSingleClaimedID()
    {
        for (var claim : m_claims)
        {
            boolean success = true;
            for (int x = claim.getX(); x < claim.getX() + claim.getWidth(); x++)
            {
                for (int y = claim.getY(); y < claim.getY() + claim.getHeight(); y++)
                {
                    IPoint p = new Point(x, y);
                    if (m_map.getTile(p) > 1)
                    {
                        success = false;
                        break;
                    }
                }
                if (!success)
                {
                    break;
                }
            }

            if (success)
            {
                return claim.getId();
            }
        }

        throw new IllegalStateException("Could not find an answer");
    }
}
