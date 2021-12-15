
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

/*
*  It takes around 10 minutes.... 
*  It should have been solved using Dijkstra algorithm or something similar, but I've gone a hard way and too late to change
*/
class Advc21_15
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
        Dictionary<Point, int> m_visited = new Dictionary<Point, int>();
        Dictionary<Point, int> m_risks = new Dictionary<Point, int>();
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

        public void PrintGrid()
        {
            Console.WriteLine($"Grid {m_max.X} * {m_max.Y}");

            foreach (var line in m_points)
            {
                foreach(Point p in line)
                {
                    if (m_visited.Keys.Contains(p))
                    {
                        Console.Write($"[{p.Val}]");
                    }
                    else
                    {
                        Console.Write($" {p.Val} ");
                    }
                    
                }
                Console.WriteLine("");
            }
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

        int getRiskAt(Point? p)
        {
            if (p == null)
            {
                return int.MaxValue;
            }
            if (!m_risks.ContainsKey(p))
            {
                m_risks[p] = 0;
            }
            return m_risks[p];
        }

        void setRiskAt(Point p, int val)
        {
            m_risks[p] = val;
        }

        bool isExit(Point p)
        {
            return p.X == m_max.X - 1 && p.Y == m_max.Y - 1;
        }

        int solveInternal(Point? p)
        {
            if (p == null)
            {
                return int.MaxValue;
            }
            if (isExit(p))
            {
                return p.Val;   // exit
            }

            int savedRisk = getRiskAt(p);

            if (savedRisk > 0)
            {
                return savedRisk;
            }

            debugWrite($"{p}");
            int riskRight = solveInternal(getPointAt(p.X + 1, p.Y));
            int riskBottom = solveInternal(getPointAt(p.X, p.Y + 1));

            int risk = Math.Min(riskRight, riskBottom) + p.Val;

            debugWrite($"{p} Right {riskRight} Bottom {riskBottom} Myval {p.Val} MyRisk {risk}");

            setRiskAt(p, risk);

            return risk;
        }

        int ManhattanDistance(Point p)
        {
            return (m_max.X - p.X - 1) + (m_max.Y - p.Y - 1);
        }

        struct QPoint
        {
            public Point point;
            public int val;
            public QPoint(Point p, int v) 
            {
                point = p;
                val = v;
            }
        };

        int traverse(Point start, int answerSoFar)
        {
            Stack<QPoint> stack = new Stack<QPoint>();

            stack.Push(new QPoint(start, 0));

            int loopcnt = 0;
    
            while(stack.Count > 0)
            {
                loopcnt ++;
                bool printDetail = (loopcnt % 10000000 == 0);

                QPoint qp = stack.Pop();
                Point p = qp.point;
                int sumSoFar = qp.val + p.Val;

                if (isExit(p))
                {
                    Console.WriteLine($"Found candidate {sumSoFar - start.Val}");
                    answerSoFar = Math.Min(sumSoFar, answerSoFar);
                    continue;
                }

                if (printDetail)
                {
                    int previousP = (m_visited.ContainsKey(p) ? m_visited[p] : -1);
                    Console.WriteLine($"[{loopcnt}] Visiting {p}, sumsofar {sumSoFar} / {previousP}, stackSize {stack.Count}");
                }
                
                m_visited[p] = sumSoFar;

                int minRisk = int.MaxValue;

                List<Point> tovisit = new List<Point>();
                foreach(Dir dir in s_dirs)
                {
                    Point? nextPoint = getPointAtDir(p, dir);

                    if (nextPoint == null)
                    {
                        continue;
                    }

                    int nextSumSoFar = sumSoFar + nextPoint.Val;

                    if (nextSumSoFar > (answerSoFar - ManhattanDistance(nextPoint)))
                    {
                        continue;
                    }

                    if (!m_visited.ContainsKey(nextPoint) || m_visited[nextPoint] > nextSumSoFar)
                    {
                        tovisit.Add(nextPoint);
                        minRisk = Math.Min(getRiskAt(nextPoint), minRisk);
                    }
                }

                tovisit.Sort((a, b) => getRiskAt(a) - getRiskAt(b));
                tovisit.Reverse();

                foreach(Point visitP in tovisit)
                {
                    int risk = getRiskAt(visitP);
                    if (printDetail)
                    {
                        Console.WriteLine($"Enqueueing {visitP}, risk {risk}");
                    }
                    
                    stack.Push(new QPoint(visitP, sumSoFar));
                }
            }
            return answerSoFar;
        }

        public void Solve()
        {
            Point? start = getPointAt(0, 0);

            if (start != null)
            {
                int initialSolve = solveInternal(start);

                Console.WriteLine($"Initial Solve {initialSolve - start.Val}");

                int ans = traverse(start, initialSolve);

                Console.WriteLine($"Answer {ans - start.Val}");
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