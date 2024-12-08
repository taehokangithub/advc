package advc_2024.day08;

import advc_utils.Grid.*;
import java.util.HashMap;
import java.util.List;
import java.util.concurrent.atomic.AtomicInteger;

public class FrequencyManager 
{
    public enum Version { Ver1, Ver2 }    
    IGrid<Character> m_grid = new Grid<>();
    HashMap<Character, Frequency> m_freqs = new HashMap<>();

    public FrequencyManager(List<String> lines)
    {
        m_grid.initialiseGrid(lines, s -> s);

        m_grid.forEach((location, tile) -> 
        {
            if (tile != '.')
            {
                if (!m_freqs.containsKey(tile))
                {
                    m_freqs.put(tile, new Frequency(tile));
                }
                m_freqs.get(tile).m_locations.add(location);
            }
        });
    }

    public int getCountAntinodes(Version v)
    {
        AtomicInteger sum = new AtomicInteger(0);

        IGrid<Character> newGrid = new Grid<>(m_grid);
        
        m_grid.forEach((location, _) ->
        {
            for (var freq : m_freqs.values())
            {
                if (v == Version.Ver1 && freq.isAntinode(location))
                {
                    sum.incrementAndGet();
                    newGrid.setTile(location, '#');
                    return;
                }
                else if (v == Version.Ver2 && freq.isAntinodeV2(location))
                {
                    sum.incrementAndGet();
                    newGrid.setTile(location, '#');
                    return;
                }
            }
        });

        //System.out.println(newGrid.getGridString(c -> c));

        return sum.get();
    }
}
