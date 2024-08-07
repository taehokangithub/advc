package advc_utils.Map;

import advc_utils.Points.*;
import java.util.Collection;

public interface IMap<T>
{
    // Callback for looping for each tiles in a grid
    interface ForEachTileCB<T>
    {
        void forEachTile(IPoint p, T val);
    }

    interface ForEachSpacePointCB
    {
        void forEachSpacePoint(IPoint p);
    }

    // Returns if the map contains the given point
    boolean contains(IPoint p);

    // Returns if the given point is on the boundary of the map
    boolean isBoundary(IPoint p);

    // Count of registered points
    int getCount();

    // Getter : returns the tile of the given point (x, y)
    T getTile(IPoint p);

    // Setter
    void setTile(IPoint p, T tile);

    // Returns all registered points
    Collection<IPoint> getTilePoints();

    // min/max coordinate getters
    IPoint getMin();
    IPoint getMax();

    // Loops for each tiles stored in the map, calling the given ForEachCB
    void forEachTile(ForEachTileCB<T> cb);

    // Loops for each points in the virutal space of the map
    void forEachSpacePoint(ForEachSpacePointCB cb);

}
