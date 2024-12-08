package advc_2024.day08;

import advc_utils.Points.*;
import java.util.ArrayList;
import java.util.List;

public class Frequency
{
    public char m_name;
    public List<IPoint> m_locations = new ArrayList<>();

    public Frequency(char name)
    {
        m_name = name;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();
        sb.append("[" + m_name + " : ");
        for (var loc : m_locations)
        {
            sb.append(loc.toString());
        }
        sb.append("]");
        return sb.toString();
    }

    public boolean isAntinode(IPoint location)
    {
        for (int i = 0; i < m_locations.size() - 1; i ++)
        {
            for (int k = i + 1; k < m_locations.size(); k ++)
            {
                assert(i != k);

                final IPoint p1 = m_locations.get(i);
                final IPoint p2 = m_locations.get(k);

                final var xdiff = p2.getX() - p1.getX();
                final var ydiff = p2.getY() - p1.getY();

                if (location.getX() == p1.getX() - xdiff &&
                    location.getY() == p1.getY() - ydiff)
                {
                    return true;
                }
                if (location.getX() == p2.getX() + xdiff &&
                    location.getY() == p2.getY() + ydiff)
                {
                    return true;
                }
            }
        }
        return false;
    }

    public boolean isAntinodeV2(IPoint location)
    {
        for (int i = 0; i < m_locations.size() - 1; i ++)
        {
            for (int k = i + 1; k < m_locations.size(); k ++)
            {
                assert(i != k);

                final IPoint p1 = m_locations.get(i);
                final IPoint p2 = m_locations.get(k);

                if (location.equals(p1) || location.equals(p2))
                {
                    return true;
                }

                final var xdiff = p2.getX() - p1.getX();
                final var ydiff = p2.getY() - p1.getY();

                assert(xdiff != 0);
                assert(ydiff != 0);                

                final var loc_p2Xdiff = location.getX() - p2.getX();
                final var loc_p2Ydiff = location.getY() - p2.getY();

                final var loc_p1Xdiff = location.getX() - p1.getX();
                final var loc_p1Ydiff = location.getY() - p1.getY();

                if (loc_p1Xdiff == 0 || loc_p1Ydiff == 0 || loc_p2Xdiff == 0 || loc_p2Ydiff == 0)
                {
                    continue;
                }

                if (loc_p2Xdiff % xdiff == 0 && loc_p2Ydiff % ydiff == 0 &&
                        loc_p2Xdiff / xdiff == loc_p2Ydiff / ydiff) 
                {
                    return true;
                }
                if (loc_p1Xdiff % xdiff == 0 && loc_p1Ydiff % ydiff == 0 &&
                            loc_p1Xdiff / xdiff == loc_p1Ydiff / ydiff)
                {
                    return true;                        
                }
            }
        }
        return false;
    }    
}