package advc_2024.day12;

import java.util.ArrayDeque;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Queue;

import advc_utils.Etc.Logger;
import advc_utils.Etc.Logger.LoggerType;
import advc_utils.Grid.Grid;
import advc_utils.Points.IPoint;
import advc_utils.Points.PointStatic;

class Region
{
    public int area;
    public int perimiter;
    public HashSet<String> outside = new HashSet<>(); // dir, location for being recognised as an outside tile
}

public class Regions 
{
    private Grid<String> m_grid = new Grid<>();
    private HashMap<String, Region> m_regions = new HashMap<>();
    private int m_regionId = 0;
    private boolean m_checkSideOnly = false;
    

    public Regions(List<String> lines, boolean checkSideOnly)
    {
        m_checkSideOnly = checkSideOnly;
        m_grid.initialiseGrid(lines, c -> ("" + c));
        setupRegions();
    }

    public int getAllRegionsCost()
    {
        int sum = 0;

        setupRegionsCost();

        for (var regionItem : m_regions.entrySet())
        {
            final var region = regionItem.getValue();

            final int cost = region.area * region.perimiter;
            sum += cost;
        }
        return sum;
    }

    private void setupRegions()
    {
        var newGrid = new Grid<>(m_grid);
        final String noRegion = ".";

        newGrid.forEach((loc, tile) ->
        {
            if (tile.equals(noRegion))
            {
                return;
            }

            String regionName = tile + m_regionId;
            m_regionId ++;
            m_regions.put(regionName, new Region());

            Queue<IPoint> searchQ = new ArrayDeque<>();

            searchQ.add(loc);
            newGrid.setTile(loc, noRegion);

            while (!searchQ.isEmpty())
            {
                IPoint visitLoc = searchQ.poll();

                newGrid.setTile(visitLoc, noRegion);
                m_grid.setTile(visitLoc, regionName);
                
                for (var dir : PointStatic.Dir4Points)
                {
                    var newPoint = visitLoc.getAdded(dir);
                    if (newGrid.isValid(newPoint) && newGrid.getTile(newPoint).equals(tile))
                    {
                        newGrid.setTile(newPoint, noRegion);
                        searchQ.add(newPoint);
                    }
                }
            }
        });
    }

    private void setupRegionsCost()
    {
        m_grid.forEach((loc, regionName) ->
        {
            final var region = m_regions.get(regionName);

            region.area ++;

            for (var dir : PointStatic.Dir4Points)
            {
                final var newLoc = loc.getAdded(dir);
                if (m_grid.isValid(newLoc))
                {
                    final var otherRegionName = m_grid.getTile(newLoc);
                    if (regionName != otherRegionName)
                    {
                        setPerimiter(region, dir, newLoc);
                    }
                }
                else
                {
                    setPerimiter(region, dir, newLoc);
                }
            }
        });
    }

    private void setPerimiter(Region region, IPoint dir, IPoint location)
    {
        if (m_checkSideOnly)
        {
            region.outside.add(dir.toString() + location.toString());

            for (var sideDir : PointStatic.Dir4Points)
            {
                var newLoc = location.getAdded(sideDir);

                String checkSideString = dir.toString() + newLoc.toString();
                if (region.outside.contains(checkSideString))
                {
                    return; // do not count as perimiter! it's one of the existing "side"
                }
            }
        }
        region.perimiter ++;
    }

}
