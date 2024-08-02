package advc_utils.Map;

import advc_utils.Points.*;

public interface IMap<T>
{
    // Callback for looping for each tiles in a grid
    interface ForEachCB<T>
    {
        void forEachPoint(IPoint p, T val);
    }

    // Returns if the map contains the given point
    boolean contains(IPoint p);

    // Getter : returns the tile of the given point (x, y)
    T getTile(IPoint p);

    // Setter
    void setTile(IPoint p, T tile);

    // min/max coordinate getters
    IPoint getMin();
    IPoint getMax();

    // Loops for each points stored in the map, calling the given ForEachCB
    void forEach(ForEachCB<T> cb);
}
