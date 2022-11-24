using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc.Utils.MapUtil
{
    public struct Point
    {
        public static int s_dimension = 3;

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

        public void Add(Point p)
        {
            x += p.x;
            y += p.y;
            z += p.z;
            w += p.w;
        }

        public override string ToString()
        {
            switch (s_dimension)
            {
                case 2:
                    return $"[{x}:{y}]";
                case 3:
                    return $"[{x}:{y}:{z}]";
                case 4:
                    return $"[{x}:{y}:{z}:{w}]";
            }
            throw new Exception($"[Point] Unknown dimension {s_dimension}");
        }
    }

    public static class Direction
    {
        public static readonly Point Up = new Point(0, -1);
        public static readonly Point Down = new Point(0, 1);
        public static readonly Point Left = new Point(-1, 0);
        public static readonly Point Right = new Point(1, 0);
    }

    public interface IMap<ValueType>
    {
        void SetMax(int x, int y, int z = 0, int w = 0);
        void SetMax(Point p);
        void SetMin(int x, int y, int z = 0, int w = 0);
        void SetMin(Point p);

        void SetAt(ValueType v, int x, int y, int z = 0, int w = 0);
        void SetAt(ValueType v, Point p);

        ValueType GetAt(int x, int y, int z = 0, int w = 0);
        ValueType GetAt(Point p);
    }

    abstract class MapBase<ValueType>
    {
        private Point? m_min = null;
        private Point? m_max = null;

        public virtual void SetMax(Point max)
        {
            m_max = max;
        }

        public virtual void SetMin(Point min)
        {
            m_min = min;
        }

        public void SetMax(int x, int y, int z = 0, int w = 0)
        {
            SetMax(new Point(x, y, z, w));
        }

        public void SetMin(int x, int y, int z = 0, int w = 0)
        {
            SetMin(new Point(x, y, z, w));
        }

        public void SetAt(ValueType v, int x, int y, int z = 0, int w = 0)
        {
            SetAt(v, new Point(x, y, z, w));
        }

        public ValueType GetAt(int x, int y, int z = 0, int w = 0)
        {
            return GetAt(new Point(x, y, z, w));
        }

        public void SetAt(ValueType v, Point p)
        {
            CheckBoundary(p);
            OnSetAt(v, p);
        }

        public ValueType GetAt(Point p)
        {
            CheckBoundary(p);
            return OnGetAt(p);
        }

        private void CheckBoundary(Point p)
        {
            if (m_max.HasValue)
            {
                if (m_max.Value.x < p.x
                    || m_max.Value.y < p.y
                    || m_max.Value.z < p.z
                    || m_max.Value.w < p.w)
                {
                    throw new ArgumentException($"Point {p} out of max boundary {m_max.Value}");
                }
            }

            if (m_min.HasValue)
            {
                if (m_min.Value.x > p.x
                    || m_min.Value.y > p.y
                    || m_min.Value.z > p.z
                    || m_min.Value.w > p.w)
                {
                    throw new ArgumentException($"Point {p} out of max boundary {m_min.Value}");
                }
            }
        }

        protected abstract void OnSetAt(ValueType v, Point p);
        protected abstract ValueType OnGetAt(Point p);
    }

    class MapByDictionary<ValueType> : MapBase<ValueType>, IMap<ValueType>
    {
        private Dictionary<Point, ValueType> m_map = new();

        protected override void OnSetAt(ValueType v, Point p)
        {
            m_map[p] = v;
        }

        protected override ValueType OnGetAt(Point p)
        {
            if (m_map.Keys.Contains(p))
            {
                return m_map[p];
            }
            return default(ValueType)!;
        }
    }

    class MapByList<ValueType> : MapBase<ValueType>, IMap<ValueType>
    {
        public List<List<ValueType>> m_map = new();

        public override void SetMin(Point min)
        {
            throw new InvalidOperationException("Can't set min boundary for MapByList");
        }

        public override void SetMax(Point max)
        {
            base.SetMax(max);

            if (max.z > 0 || max.w > 0)
            {
                throw new ArgumentException("MapByList does not support 3D/4D");
            }

            for (int x = 0; x < max.x; x ++)
            {
                List<ValueType> list = new();
                m_map.Add(list);

                for (int y = 0; y < max.y; y++)
                {
                    list.Add(default!);
                }
            }
        }

        protected override void OnSetAt(ValueType v, Point p)
        {
            m_map[p.x][p.y] = v;
        }

        protected override ValueType OnGetAt(Point p)
        {
            return m_map[p.x][p.y];
        }
    }
}


