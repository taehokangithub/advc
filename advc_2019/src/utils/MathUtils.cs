using Advc.Utils.MapUtil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc.Utils.MathUtil
{
    class GenericMath
    {
        public static int GetGCD(int a, int b)
        {
            if (a >= b)
            {
                return GetGCDInternal(a, b);
            }
            return GetGCDInternal(b, a);
        }

        private static int GetGCDInternal(int a, int b)
        {
            if (b == 0)
            {
                return a;
            }
            return GetGCDInternal(b, a % b);
        }
    }

    class Angles
    {
        public static double Rad2Deg(double rad)
        {
            return rad * 180f / Math.PI;
        }

        // if rotate == 0, point (1,0) is the origin (degree 0)
        public static double ToDegree(Point p, double rotate)
        {
           var standard = (Rad2Deg(MathF.Atan2(p.y, p.x)) + 360.01f) % 360f;
           return (standard + 90f) % 360f;
        }

        // Beware this coordinate system has reverted left/right directinon
        // because y axis is reverted (Up == -1, Down == 1)
        public static Point RotateRight(Point p)
        {
            return new(-p.y, p.x);
        }

        public static Point RotateLeft(Point p)
        {
            return new(p.y, -p.x);
        }

    }
}