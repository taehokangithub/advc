
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

/*
*  Retry with Dijkstra, which takes less than a minute. Much better and clean....
*/
class Advc21_15_Dijkstra
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }
    
    class Point
    {
        public int X = 0;
        public int Y = 0;
        public int Val = 0;

        const int MaxVal = 9;

        public Point() 
        {
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
            Val = other.Val;
        }

        public void IncreaseOnMultiple(int amount)
        {
            Val = ((Val + amount- 1) % MaxVal) + 1;
        }

        public Point(int x, int y, int val)
        {
            X = x;
            Y = y;
            Val = val;
        }

        public void SetMax(Point p)
        {
            X = Math.Max(X, p.X + 1);
            Y = Math.Max(Y, p.Y + 1);
        }

        public override string ToString()
        {
            return $"[{X}:{Y}={Val}]";
        }
    }

    class Grid
    {
        enum Dir
        {
            Left, Right, Bottom, Up
        };

        readonly List<Dir> s_dirs = new List<Dir> {Dir.Left, Dir.Right, Dir.Bottom, Dir.Up};
        List<List<Point>> m_points = new List<List<Point>>();
        Point m_max = new Point();

        public void AddLine(string line)
        {
            int curY = m_points.Count();
            var curLine = new List<Point>();
            m_points.Add(curLine);

            foreach(char c in line)
            {
                curLine.Add(new Point(curLine.Count, curY, c - '0'));
            };

            m_max.X = curLine.Count;
            m_max.Y = m_points.Count;
        }

        public void Multiple(int m)
        {
            List<List<Point>> newGrid1 = new List<List<Point>>();

            // horizontal expansion

            foreach(var line in m_points)
            {
                List<Point> newLine = new List<Point>();

                for (int i = 0; i < m; i ++)
                {
                    foreach(Point p in line)
                    {
                        Point newP = new Point(p.X + i * m_max.X, p.Y, p.Val);
                        newP.IncreaseOnMultiple(i);
                        newLine.Add(newP);
                    }
                }

                newGrid1.Add(newLine);
            }

            List<List<Point>> newGrid2 = new List<List<Point>>();

            // vertical expansion
            for (int i = 0; i < m; i ++)
            {
                foreach(var line in newGrid1)
                {
                    List<Point> newLine = new List<Point>();
                    foreach(Point p in line)
                    {
                        Point newP = new Point(p.X, p.Y + i * m_max.Y, p.Val);
                        newP.IncreaseOnMultiple(i);
                        newLine.Add(newP);                        
                    }
                    newGrid2.Add(newLine);
                }
            }

            m_points = newGrid2;

            m_max.X *= m;
            m_max.Y *= m;
        }


        Point? getPointAt(int x, int y)
        {
            if (x < m_max.X && y < m_max.Y && x >= 0 && y >= 0)
            {
                return m_points[y][x];
            }
            return null;
        }

        Point? getPointAtDir(Point p, Dir dir)
        {
            int x = p.X; 
            int y = p.Y;

            switch(dir)
            {
                case Dir.Left: x --; break;
                case Dir.Right:  x ++; break;
                case Dir.Bottom: y ++; break;
                case Dir.Up: y --; break;
            }

            return getPointAt(x,  y);
        }

        bool isExit(Point p)
        {
            return p.X == m_max.X - 1 && p.Y == m_max.Y - 1;
        }

        Point GetNearestPoint(Dictionary<Point, int> points, out int distance)
        {
            var myList = points.ToList();

            myList.Sort((a, b) => a.Value - b.Value);

            Point p = myList[0].Key;
            distance = myList[0].Value;

            points.Remove(p);

            return p;
        }
        int Dikjstra(Point start)
        {
            Dictionary<Point, int> visited = new Dictionary<Point, int>();
            Dictionary<Point, int> toVisit = new Dictionary<Point, int>();
            toVisit[start] = 0;

            while (toVisit.Count > 0)
            {
                int curDist;
                Point p = GetNearestPoint(toVisit, out curDist);
                visited[p] = curDist;

                if (isExit(p))
                {
                    return curDist;
                }

                foreach(Dir dir in s_dirs)
                {
                    Point? nextPoint = getPointAtDir(p, dir);

                    if (nextPoint != null && !visited.Keys.Contains(nextPoint))
                    {
                        int oldDistNext = (toVisit.Keys.Contains(nextPoint) ? toVisit[nextPoint] : int.MaxValue);
                        int newDistNext = curDist + nextPoint.Val;

                        toVisit[nextPoint] = Math.Min(oldDistNext, newDistNext);
                    }
                }
            }
            
            return 0;
        }

        public void Solve()
        {
            Point? start = getPointAt(0, 0);

            if (start != null)
            {
                int ans = Dikjstra(start);

                Console.WriteLine($"Answer {ans}");
            }

        }
    }

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Grid grid1 = new Grid();
        Grid grid2 = new Grid();

        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                grid1.AddLine(line);
                grid2.AddLine(line);
            }
        }

        grid1.Solve();
        grid2.Multiple(5);
        grid2.Solve();
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