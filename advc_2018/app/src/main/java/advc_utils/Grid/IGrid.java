package advc_utils.Grid;

import java.util.List;

import advc_utils.Points.IPoint;

public interface IGrid<T>
{
    // Callback to convert character to a tile (enum)
    interface CharToTileCB<T>
    {
        T getTileFromChar(char c);
    }

    // Callback for looping for each tiles in a grid
    interface ForEachCB<T>
    {
        void forEachPoint(IPoint p, T val);
    }

    // Initialise : read string data from lines, convert it to a Grid, with a char-tile conversion callback
    void initialiseGrid(List<String> lines, CharToTileCB<T> cb);

    // Get the size of the grid - 2d point (x ,y)
    IPoint getSize();

    // Returns if the given point is inside the grid's valid coordinate range (x, y)
    boolean isValid(IPoint p);

    // Getter : returns the tile of the given point (x, y)
    T getTile(IPoint p);

    // Setter
    void setTile(IPoint p, T tile);

    // Returns the string representation of the grid
    String getGridString();

    // Loops for each tile in the grid, calling the given ForEachCB
    void forEach(ForEachCB<T> cb);
}
