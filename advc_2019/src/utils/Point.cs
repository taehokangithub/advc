
namespace Advc.Utils.MapUtil
{
    public struct Point
    {
        public static Point Dummy = new Point();
        public static int LogDimension { get; set; } = 2;

        public int w, x, y, z;

        public Point()
        {
            w = x = y = z = 0;
        }

        public Point(int _x, int _y, int _z = 0, int _w = 0)
        {
            x = _x;
            y = _y;
            z = _z;
            w = _w;
        }

        public Point(Point p)
        {
            x = p.x;
            y = p.y;
            z = p.z;
            w = p.w;
        }

        public void Add(Point p)
        {
            x += p.x;
            y += p.y;
            z += p.z;
            w += p.w;
        }

        public Point AddedPoint(Point p)
        {
            Point result = new(this);
            result.Add(p);
            return result;
        }

        public void Subtract(Point p)
        {
            x -= p.x;
            y -= p.y;
            z -= p.z;
            w -= p.w;
        }

        public Point SubtractedPoint(Point p)
        {
            Point result = new(this);
            result.Subtract(p);
            return result;
        }

        public void Divide(int val)
        {
            x /= val;
            y /= val;
            z /= val;
            w /= val;
        }

        public Point DividedPoint(int val)
        {
            Point result = new(this);
            result.Divide(val);
            return result;
        }

        public void Multiply(int val)
        {
            x *= val;
            y *= val;
            z *= val;
            w *= val;
        }

        public Point MultipliedPoint(int val)
        {
            Point result = new(this);
            result.Multiply(val);
            return result;
        }

        public bool Equals(Point p)
        {
            return x == p.x 
                && y == p.y 
                && z == p.z 
                && w == p.w;
        }

        public override string ToString()
        {
            switch (LogDimension)
            {
                case 2:
                    return $"[{x}:{y}]";
                case 3:
                    return $"[{x}:{y}:{z}]";
                case 4:
                    return $"[{x}:{y}:{z}:{w}]";
            }
            throw new Exception($"[Point] Unknown dimension {LogDimension}");
        }

        public void UpdateMaxCoordinate(Point p)
        {
            x = Math.Max(x, p.x);
            y = Math.Max(y, p.y);
            z = Math.Max(z, p.z);
            w = Math.Max(w, p.w);
        }

        public void UpdateMinCoordinate(Point p)
        {
            x = Math.Min(x, p.x);
            y = Math.Min(y, p.y);
            z = Math.Min(z, p.z);
            w = Math.Min(w, p.w);
        }
    }

    public static class Direction
    {
        public static readonly Point Up = new Point(0, -1);
        public static readonly Point Down = new Point(0, 1);
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);
    }
}