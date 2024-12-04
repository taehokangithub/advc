package advc_utils.Points;

public final class PointStatic {
    public static final Point[] Dir4Points = { 
        new Point(0, -1),   // up
        new Point(1, 0), // right
        new Point(0, 1), // down
        new Point(-1, 0)   // left
    };

    public static final Point[] Dir8Points = { 
        new Point(-1, -1), new Point(0, -1), new Point(1, -1), 
        new Point(-1, 0), new Point(1, 0), 
        new Point(-1, 1), new Point(0, 1), new Point(1, 1), 
    };

    static public IPoint getDir4Point(IPoint.EDir dir)
    {
        var ret = new Point(PointStatic.Dir4Points[dir.ordinal()]);
        return ret;
    }

    static public IPoint getDir8Point(int index)
    {
        var ret = new Point(PointStatic.Dir8Points[index % Dir8Points.length]);
        return ret;
    }
}
