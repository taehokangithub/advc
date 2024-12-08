package advc_utils.Grid;

import java.util.List;
import java.util.Map;

import advc_utils.Points.IPoint;

public interface IGrid<T>
{
    // Callback to convert character to a tile (enum)
    interface CharToTileCB<T>
    {
        T getTileFromChar(char c);
    }

    interface TileToCharCB<T>
    {
        char getCharFromTile(T tile);
    }

    // Callback for looping for each tiles in a grid
    interface ForEachCB<T>
    {
        void forEachPoint(IPoint p, T val);
    }

    interface ForEachRawCB<T>
    {
        void forEachPoint(long x, long y, T val);
    }

    // Initialise : read string data from lines, convert it to a Grid, with a char-tile conversion callback
    void initialiseGrid(List<String> lines, CharToTileCB<T> cb);

    // Initialise an empty grid
    void initialiseGrid(IPoint size, T defaultValue);

    Map<T, Character> getTileToCharMap();

    // Get the size of the grid - 2d point (x ,y)
    IPoint getSize();

    // Returns if the given point is inside the grid's valid coordinate range (x, y)
    boolean isValid(IPoint p);
    boolean isValidFast(long x, long y);

    // Getter : returns the tile of the given point (x, y)
    T getTile(IPoint p);
    T getTileFast(long x, long y);

    // Setter
    void setTile(IPoint p, T tile);
    void setTileFast(long x, long y, T tile);

    // Returns the string representation of the grid
    String getGridString();
    String getGridString(TileToCharCB<T> cb);

    // Loops for each tile in the grid, calling the given ForEachCB
    void forEach(ForEachCB<T> cb);
    void forEachRaw(ForEachRawCB<T> cb);
}
