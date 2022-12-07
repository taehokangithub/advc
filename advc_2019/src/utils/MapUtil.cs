using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc.Utils
{    
    abstract class MapBase<ValueType>
    {
        private Point? m_min = null;
        private Point? m_max = null;

        // Input Restrictions
        public Point Max => m_max.HasValue ? (Point) m_max : Point.Dummy;
        public Point Min => m_min.HasValue ? (Point) m_min : Point.Dummy;

        // Actual data min/max
        public Point ActualMax = new();
        public Point ActualMin = new();

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

        public virtual bool CheckBoundary(Point p)
        {
            if (m_max.HasValue)
            {
                if (m_max.Value.x < p.x
                    || m_max.Value.y < p.y
                    || m_max.Value.z < p.z
                    || m_max.Value.w < p.w)
                {
                    return false;
                }
            }

            if (m_min.HasValue)
            {
                if (m_min.Value.x > p.x
                    || m_min.Value.y > p.y
                    || m_min.Value.z > p.z
                    || m_min.Value.w > p.w)
                {
                    return false;
                }
            }

            return true;
        }

        public void SetAt(ValueType v, Point p)
        {
            if (!CheckBoundary(p))
            {
                throw new ArgumentException($"[SetAt] Point {p} out of boundary");
            }
            ActualMax.UpdateMaxCoordinate(p);
            ActualMin.UpdateMinCoordinate(p);
            OnSetAt(v, p);
        }

        public ValueType GetAt(Point p)
        {
            if (!CheckBoundary(p))
            {
                throw new ArgumentException($"[GetAt] Point {p} out of boundary");
            }
            return OnGetAt(p);
        }

        protected abstract void OnSetAt(ValueType val, Point p);
        protected abstract ValueType OnGetAt(Point p);
    }

    class MapByDictionary<ValueType> : MapBase<ValueType>
    {
        private Dictionary<Point, ValueType> m_map = new();

        protected override void OnSetAt(ValueType val, Point p)
        {
            m_map[p] = val;
        }

        protected override ValueType OnGetAt(Point p)
        {
            if (m_map.Keys.Contains(p))
            {
                return m_map[p];
            }
            return default(ValueType)!;
        }

        public void ForEach(Action<ValueType, Point> callback)
        {
            for (int z = ActualMin.z; z <= ActualMax.z; z ++)
            {
                for (int y = ActualMin.y; y <= ActualMax.y; y ++)
                {
                    for (int x = ActualMin.x; x <= ActualMax.x; x ++)    
                    {
                        Point p = new(x, y, z);
                        callback(GetAt(p), p);
                    }
                }            
            }
        }

    }

    class MapByList<ValueType> : MapBase<ValueType>
    {
        private List<List<ValueType>> m_map = new();    // [y][x] coordiation (in order to add a line by line)
        private Point m_addPointer = new();

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

            for (int y = 0; y < max.y; y++)
            {
                List<ValueType> list = new();
                m_map.Add(list);

                for (int x = 0; x < max.x; x ++)
                {
                    list.Add(default!);
                }
            }
        }

        public override bool CheckBoundary(Point p)
        {
            return p.x < Max.x && p.y < Max.y && p.x >= 0 && p.y >= 0;
        }

        public Point Add(ValueType val)
        {
            Point curLoc = m_addPointer;
            SetAt(val, m_addPointer);

            m_addPointer.x ++;
            if (m_addPointer.x >= Max.x)
            {
                m_addPointer.x = 0;
                m_addPointer.y++;
            }
            return curLoc;
        }

        // This is for read-only! Collection must not modified while iteration
        public void ForEach(Action<ValueType, Point> callback)
        {
            Point curPos = new();

            foreach (var line in m_map)
            {
                foreach (ValueType val in line)
                {
                    callback(val, curPos);
                    curPos.x ++;
                }
                curPos.y ++;
                curPos.x = 0;
            }
        }

        public bool CheckAddFinished(bool throwException)
        {
            // Pointing to the last available point + (1,0) == (0, max)
            bool ret = m_addPointer.x == 0
                    && m_addPointer.y == Max.y;
            if (throwException && !ret)
            {
                throw new Exception($"{m_addPointer.x} != 0 || {m_addPointer.y} != {Max.y}");
            }
            return ret;
        }

        protected override void OnSetAt(ValueType val, Point p)
        {
            m_map[p.y][p.x] = val;
        }

        protected override ValueType OnGetAt(Point p)
        {
            return m_map[p.y][p.x];
        }
    }
}


