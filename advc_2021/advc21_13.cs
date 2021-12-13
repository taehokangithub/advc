
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_13
{
    class Point
    {
        public int x = 0;
        public int y = 0;

        public Point(string line)
        {
            string[] split = line.Split(",");
            x = int.Parse(split[0]);
            y = int.Parse(split[1]);
        }

        public Point()
        {
        }

        public Point(int a, int b)
        {
            x = a;
            y = b;
        }

        public void SetMax(Point p)
        {
            x = Math.Max(x, p.x + 1);
            y = Math.Max(y, p.y + 1);
        }
    }

    class Cmd
    {
        public enum FoldType { X, Y };
        public FoldType Type = FoldType.X;
        public int Value = 0;

        public Cmd(string line)
        {
            var values = line.Split(' ')[2].Split('=');
            string typeStr = values[0];
            Value = int.Parse(values[1]);
            Type = (typeStr[0] == 'x') ? FoldType.X : FoldType.Y;
        }
    }

    class Grid
    {
        List<List<bool>> m_grid = new List<List<bool>>();
        List<Point> m_points = new List<Point>();
        Point m_maxPoint = new Point();

        public void SetValue(Point p, bool val)
        {
            try
            {
                m_grid[p.y][p.x] = val;
            }
            catch (Exception)
            {
                Console.WriteLine($"Index out of range {p.x},{p.y}, while max {m_maxPoint.x},{m_maxPoint.y}");
                throw new Exception("Error! Exiting..");
            }
            
        }

        public bool GetValue(Point p)
        {
            return GetValue(p.x, p.y);
        }

        public bool GetValue(int x, int y)
        {
            return  m_grid[y][x];
        }

        public void SetupGrid(List<Point> points)
        {
            m_points = new List<Point>(points);

            foreach (Point p in points)
            {
                m_maxPoint.SetMax(p);
            }

            for (int y = 0; y < m_maxPoint.y; y ++)
            {
                List<bool> list = new List<bool>(new bool[m_maxPoint.x]);
                m_grid.Add(list);
            }

            foreach(Point p in points)
            {
                SetValue(p, true);
            }
        }

        public int CntValues()
        {
            int cnt = 0;
            for (int y = 0; y < m_maxPoint.y; y ++)
            {
                for (int x = 0; x < m_maxPoint.x; x ++)
                {
                    cnt += GetValue(x, y) ? 1 : 0;
                }
            }
            return cnt;
        }

        private Point GetFoldedPoint(Point p, Cmd cmd)
        {
            Point n = new Point(p.x, p.y);

            if (cmd.Type == Cmd.FoldType.Y)
            {
                if (p.y > cmd.Value)
                {
                    n.y = cmd.Value - (p.y - cmd.Value);
                }
            }
            else
            {
                if (p.x > cmd.Value)
                {
                    n.x = cmd.Value - (p.x - cmd.Value);
                }
            }

            return n;
        }

        public Grid Execute(Cmd cmd)
        {
            var newPoints = new List<Point>();

            foreach(Point p in m_points)
            {
                newPoints.Add(GetFoldedPoint(p, cmd));
            }

            Grid grid = new Grid();
            grid.SetupGrid(newPoints);

            return grid;
        }

        public void PrintGrid()
        {
            for (int y = 0; y < m_maxPoint.y; y ++)
            {
                for (int x = 0; x < m_maxPoint.x; x ++)
                {
                    char c = (GetValue(x, y) ? '#' : '.');
                    Console.Write(c);
                }
                Console.WriteLine("");
            }
        }
    }

    class Solver
    {
        List<Point> m_points = new List<Point>();
        List<Cmd> m_commands = new List<Cmd>();
        Point m_maxPoint = new Point();

        public void AddPoint(Point p)
        {
            m_maxPoint.SetMax(p);
            m_points.Add(p);
        }

        public void AddCmd(Cmd c)
        {
            m_commands.Add(c);
        }
        public void Solve1()
        {
            Grid grid = new Grid();
            grid.SetupGrid(m_points);
            Grid newGrid = grid.Execute(m_commands[0]);

            int ans = newGrid.CntValues();

            Console.WriteLine($"Solve part 1 : {ans}");
        }

        public void Solve2()
        {
            Grid grid = new Grid();
            grid.SetupGrid(m_points);

            foreach(Cmd cmd in m_commands)
            {
                grid = grid.Execute(cmd);
            }

            grid.PrintGrid();
        }
    }

	static void SolveMain(string path)
	{
        Console.WriteLine($"Reading File {path}");
		var lines = File.ReadLines(path);

        Solver solver = new Solver();
		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
                if (line[0] == 'f')
                {
                    solver.AddCmd(new Cmd(line));
                }
                else
                {
                    solver.AddPoint(new Point(line));
                }
			}
		}

        solver.Solve1();
        solver.Solve2();
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