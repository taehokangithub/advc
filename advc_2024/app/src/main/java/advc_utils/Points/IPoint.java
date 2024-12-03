package advc_utils.Points;

public interface IPoint 
{
    public enum EAxis { X, Y, Z, W }
    public enum EDir { UP, RIGHT, DOWN, LEFT }

    // Generic getter for the given axis
    long getAxisValue(EAxis axis);

    // Generic setter for the given axis and value
    void setAxisValue(EAxis axis, long val);

    // Number of axis in this point, representing 2d, 3d or 4d
    int getNumAxis();

    // Point addition
    void add(IPoint other);
    IPoint getAdded(IPoint other);

    // Point substraction
    void sub(IPoint other);
    IPoint getSubbed(IPoint other);

    // Point multiplication
    void multiply(long val);
    IPoint getMultiplied(long val);

    // Point movement for the given direction
    void move(EDir dir);
    void move(EDir dir, long distance);
    IPoint getMoved(EDir dir);
    IPoint getMoved(EDir dir, long distance);

    // Point rotation for the given direction, only for 2D
    void rotate(EDir dir);
    IPoint getRotated(EDir dir);

    // Manhattan distance : sum of all axis values
    long getManhattanDistance();
    long getManhattanDistance(IPoint other);

    // Min/max setter : compare all axis values of this and other, and sets the min or max value
    // This is usefule when setting the min/max coordination of all the given points
    void setMax(IPoint other);
    void setMin(IPoint other);

    // Default getters for IPoint interface
    default long getX() { return getAxisValue(EAxis.X); }
    default long getY() { return getAxisValue(EAxis.Y); }
    default long getZ() { return getAxisValue(EAxis.Z); }
    default long getW() { return getAxisValue(EAxis.W); }    
}
