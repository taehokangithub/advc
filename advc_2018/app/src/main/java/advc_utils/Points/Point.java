package advc_utils.Points;

import static org.junit.jupiter.api.Assertions.*;

public class Point implements IPoint
{
    private int m_numAxis = 0;
    public long x, y, z, w;

    public Point() {}

    public Point(long _x, long _y) 
    {
        x = _x;
        y = _y;
        m_numAxis = 2;
    }

    public Point(long _x, long _y, long _z) 
    {
        this(_x, _y);
        z = _z;
        m_numAxis = 3;
    }

    public Point(long _x, long _y, long _z, long _w)
    {
        this(_x, _y, _z);
        w = _w;
        m_numAxis = 4;
    }

    public Point(String str)
    {
        String trimmed = str.substring(1, str.length() - 1);
        var intStrings = trimmed.split(",");
        m_numAxis = 0;
        var axisValues = EAxis.values();
        
        for (String element : intStrings)
        {
            Long value = Long.parseLong(element);
            setAxisValue(axisValues[m_numAxis], value);
            m_numAxis ++;
        }
    }

    public Point(IPoint other)
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, other.getAxisValue(axis));
        }
        m_numAxis = other.getNumAxis();
    }

    public Point(Point other)
    {
        this((IPoint)other);
    }

    public Point(Long defaultValue)
    {
        this(defaultValue, defaultValue, defaultValue, defaultValue);
    }

    @Override
    public int getNumAxis() 
    {
        return this.m_numAxis;
    }

    @Override
    public String toString()
    {
        StringBuffer sb = new StringBuffer();
        sb.append("[");

        var axisArray = EAxis.values();

        for (int i = 0; i < m_numAxis; i ++)
        {
            if (i != 0)
            {
                sb.append(",");
            }
            sb.append(Long.toString(getAxisValueLong(axisArray[i])));
        }

        sb.append("]");
        return sb.toString();
    }


    @Override
    public boolean equals(Object obj)
    {
        if (obj == this) 
        {
            return true;
        }
        if (obj == null || getClass() != obj.getClass())
        {
            return false;
        }
        var other = (IPoint) obj;
        for (var axis : EAxis.values())
        {
            if (getAxisValue(axis) != other.getAxisValue(axis))
            {
                return false;
            }
        }
        return true;
    }

    @Override
    public long getAxisValueLong(EAxis axis)
    {
        switch(axis)
        {
            case EAxis.X : return x;
            case EAxis.Y : return y;
            case EAxis.Z : return z;
            case EAxis.W : return w;
        }
        assert false;
        return 0;
    }

    @Override
    public void setAxisValue(EAxis axis, long val)
    {
        switch (axis)
        {
            case EAxis.X : x = val; break;
            case EAxis.Y : y = val; break;
            case EAxis.Z : z = val; break; 
            case EAxis.W : w = val; break;            
            default:
                assert false;
        }
    }

    @Override
    public void add(IPoint other)
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, getAxisValue(axis) + other.getAxisValue(axis));
        }
    }

    @Override
    public IPoint getAdded(IPoint other)
    {
        IPoint ret = new Point(this);
        ret.add(other);
        return ret;
    }

    @Override
    public void sub(IPoint other)
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, getAxisValue(axis) - other.getAxisValue(axis));
        }
    }

    @Override
    public IPoint getSubbed(IPoint other)
    {
        IPoint ret = new Point(this);
        ret.sub(other);
        return ret;
    }

    @Override
    public void multiply(long val)
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, getAxisValue(axis) * val);
        }
    }

    @Override
    public IPoint getMultiplied(long val)
    {
        IPoint ret = new Point(this);
        ret.multiply(val);
        return ret;
    }

    @Override
    public void move(EDir dir) 
    {
        move(dir, 1);
    }

    @Override
    public IPoint getMoved(EDir dir) {
        return getMoved(dir, 1);
    }

    @Override
    public void move(EDir dir, long distance) 
    {
        var movePoint = PointStatic.getDir4Point(dir).getMultiplied(distance);
        add(movePoint);
    }

    @Override
    public IPoint getMoved(EDir dir, long distance) {
        IPoint ret = new Point(this);
        ret.move(dir, distance);
        return ret;
    }

    @Override
    public void rotate(EDir dir) {
        assertTrue(getNumAxis() == 2);

        switch (dir)
        {
        case EDir.UP: 
            break;

        case EDir.RIGHT:
            var tmp = y;
            y = x;
            x = -tmp;
            break;

        case EDir.LEFT: 
            tmp = x;
            x = y;
            y = -tmp;
            break;

        case EDir.DOWN:
            x = -x; 
            y = -y; 
            break;

        default:
            assert(false);
        }
    }

    @Override
    public IPoint getRotated(EDir dir) {
        IPoint ret = new Point(this);
        ret.rotate(dir);
        return ret;
    }

    @Override
    public long getManhattanDistance() {
        long sum = 0;
        for (var axis : EAxis.values())
        {
            sum += getAxisValue(axis);
        }
        return sum;
    }

    @Override
    public void setMax(IPoint other) 
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, Math.max(getAxisValue(axis), other.getAxisValue(axis)));
        }
    }

    @Override
    public void setMin(IPoint other) 
    {
        for (var axis : EAxis.values())
        {
            setAxisValue(axis, Math.min(getAxisValue(axis), other.getAxisValue(axis)));
        }        
    }
}
