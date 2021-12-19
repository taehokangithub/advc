
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_19
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    enum AXIS 
    {
        ZR = -3, YR = -2,XR = -1,
        X = 1, Y = 2, Z = 3
    }

    const int NUM_ROTATIONS = 24;
    const int NUM_AXIS = 3;

    static readonly int [,] ROTATIONS = new int[NUM_ROTATIONS, NUM_AXIS] {{1, 2, 3}, {2, 1, -3}, {-2, 3, -1}, {3, 2, -1}, {3, 1, 2}, {-3, 2, 1}, 
                                    {-3, 1, -2}, {-2, 1, 3}, {1, -3, 2}, {1, -2, -3}, {-3, -1, 2}, {2, -1, 3}, 
                                    {2, -3, -1}, {-2, -1, -3}, {-1, -2, 3}, {-3, -2, -1}, {-1, 3, 2}, {2, 3, 1},
                                    {1, 3, -2}, {-1, 2, -3}, {3, -2, 1}, {3, -1, -2}, {-2, -3, 1}, {-1, -3, -2}};

    class Point
    {
        public int x = 0;
        public int y = 0;
        public int z = 0;

        public Point()
        {

        }

        public Point(Point p)
        {
            x = p.x;
            y = p.y;
            z = p.z;
        }

        public Point Rotate(int rotationIndx)
        {
            AXIS rotatedX = (AXIS) ROTATIONS[rotationIndx, 0];
            AXIS rotatedY = (AXIS) ROTATIONS[rotationIndx, 1];
            AXIS rotatedZ = (AXIS) ROTATIONS[rotationIndx, 2];

            Point newPoint = new Point(this);

            newPoint.x = Get(rotatedX);
            newPoint.y = Get(rotatedY);
            newPoint.z = Get(rotatedZ);

            return newPoint;
        }

        public int Get(AXIS axis)
        {
            switch (axis)
            {
                case AXIS.X: return x;
                case AXIS.Y: return y;
                case AXIS.Z: return z;
                case AXIS.XR: return -x;
                case AXIS.YR: return -y;
                case AXIS.ZR: return -z;
            }
            throw new Exception($"Unknown axis {axis}");
        }

        public static Point operator -(Point a, Point b)
        {
            Point p = new Point();

            p.x = a.x - b.x;
            p.y = a.y - b.y;
            p.z = a.z - b.z;

            return p;            
        }

        public static Point operator +(Point a, Point b)
        {
            Point p = new Point();

            p.x = a.x + b.x;
            p.y = a.y + b.y;
            p.z = a.z + b.z;

            return p;            
        }

        public bool Equals(Point other)
        {
            return x == other.x && y == other.y && z == other.z;
        }

        public override string ToString()
        {
            return $"[{x}:{y}:{z}]";
        }

        public int ManhattanDistance(Point p)
        {
            return Math.Abs(p.x - x) + Math.Abs(p.y - y) + Math.Abs(p.z - z);
        }
            
    }

    class Scanner
    {
        const int REQUIRED_ALIGNS = 12;

        List<Point> m_points = new List<Point>();

        public bool Aligned { get; set; } = false; // if aligned to scanner 0 (always true for scanner 0)
        public int Index { get; }

        public Point Position { get; set; } = new Point();

        public Scanner(int index)
        {
            Index = index;
        }

        public void AddPoint(string line)
        {
            string[] strs = line.Split(",");

            Point p = new Point();
            p.x = Convert.ToInt32(strs[0]);
            p.y = Convert.ToInt32(strs[1]);
            p.z = Convert.ToInt32(strs[2]);

            m_points.Add(p);
        }

        public override string ToString()
        {
            string str = $"=== Scanner {Index} ===\n";
            
            foreach (Point p in m_points)
            {
                str += $"{p}\n";
            }

            str += "\n";

            return str;
        }

        private bool TryAlignInteral(Scanner other, Point pivot, int rotation)
        {
           for (int i = 0; i < other.m_points.Count; i ++)
            {
                // let's assume scanner 0 point 0 is the same with scanner other point i
                Point otherPoint = other.m_points[i];
                Point rotatedOtherPoint = otherPoint.Rotate(rotation);
                Point relPos = pivot - rotatedOtherPoint;    // this is assumed position of Scanner i. Now prove it

                int numAlignedPoints = 1;    // one point already aligned
                for (int myPointIndex = 1; myPointIndex < this.m_points.Count; myPointIndex ++)
                {
                    Point myPoint = this.m_points[myPointIndex];
                    Point? foundOtherPoint = other.FindRelativePoint(rotation, relPos, myPoint);

                    if (foundOtherPoint != null)
                    {
                        numAlignedPoints ++;
                    }
                }

                if (numAlignedPoints >= REQUIRED_ALIGNS)
                {
                    other.Transform(rotation, relPos);
                    Console.WriteLine($"* Scanner {Index} and {other.Index}:point{i} have aligned with total {numAlignedPoints} points at rotation{rotation}");

                    return true;
                }
            }
            return false;
        }

        public bool TryAlign(Scanner other)
        {
            Debug.Assert(Aligned, $"Scanner {Index} is not aligned");

            foreach (Point pivot in m_points)
            {
                for (int rotation = 0; rotation < NUM_ROTATIONS; rotation ++)
                {
                    if (TryAlignInteral(other, pivot, rotation))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        void Transform(int rotation, Point relPoint)
        {
            for (int i = 0; i < m_points.Count; i ++)
            {
                m_points[i] = m_points[i].Rotate(rotation) + relPoint;
            }
            Position = relPoint;
            Aligned = true;
        }

        Point? FindRelativePoint(int rotation, Point relPos, Point targetPos)
        {
            foreach (Point p in m_points)
            {
                if ((p.Rotate(rotation) + relPos).Equals(targetPos))
                {
                    return p;
                }
            }
            return null;
        }

        public List<Point> GetPoints() 
        {
            return m_points;
        }
    }

    class Solver
    {
        List<Scanner> m_scanners = new List<Scanner>();

        public Scanner GetNewScanner()
        {
            Scanner scanner = new Scanner(m_scanners.Count);
            m_scanners.Add(scanner);
            return scanner;
        }

        public void Solve()
        {
            Scanner rootScanner = m_scanners[0];
            rootScanner.Aligned = true;
            bool hasAnyChange = true;
            HashSet<Scanner> aligned = new HashSet<Scanner>();
            
            while(hasAnyChange)
            {
                hasAnyChange = false;
                foreach (Scanner scanner in m_scanners)
                {
                    if (scanner.Aligned && !aligned.Contains(scanner))
                    {
                        aligned.Add(scanner);
                        hasAnyChange = true;

                        foreach (Scanner other in m_scanners)
                        {
                            if (other != scanner && !other.Aligned)
                            {
                                if (scanner.TryAlign(other))
                                {
                                    Console.WriteLine($"Scanner {scanner.Index} and {other.Index} have been aligned");
                                    
                                }
                            }
                        }
                        
                    }
                }
            }

            HashSet<string> m_allPoints = new HashSet<string>();
            int maxDist = 0;

            foreach (Scanner scanner in m_scanners)
            {
                foreach(Point point in scanner.GetPoints())
                {
                    m_allPoints.Add(point.ToString());
                }

                foreach(Scanner other in m_scanners)
                {
                    if (other.Index != scanner.Index)
                    {
                        maxDist = Math.Max(maxDist, scanner.Position.ManhattanDistance(other.Position));
                    }
                }
            }

            Console.WriteLine($"Solve1 ans {m_allPoints.Count}");
            Console.WriteLine($"Solve2 ans {maxDist}");
        }
    }

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Solver solver = new Solver();
        Scanner? currentScanner = null;

        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                if (line.Substring(0, 3) == "---")
                {
                    currentScanner = solver.GetNewScanner();
                }
                else
                {
                    if (currentScanner == null)
                    {
                        throw new Exception("No current scanner");
                    }
                    currentScanner.AddPoint(line);
                }
            }
        }

        solver.Solve();
    }


    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        SolveMain($"../../data/{className}.txt");
    }
    
    static void Main()
    {
        Run();
    }    
}