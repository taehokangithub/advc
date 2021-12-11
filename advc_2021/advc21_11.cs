
using System;
using System.Collections.Generic;
using System.IO;

class Advc21_11
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
        public int X { get; set; }
        public int Y { get; set; }
        public int Val { get; set; }

        public void increase() 
        {
            Val ++;

            if (Val >= 10)
                Val = 0;
        }

        public override string ToString()
        {
            return $"[{X}:{Y}={Val}]";
        }
    }

    class Grid
    {
        private enum Dir { Up, Down, Left, Right };

        private List<List<Point>> m_points = new List<List<Point>>();
        private int m_maxX = 0;
        private int m_maxY = 0;
        private HashSet<Point> m_flashed = new HashSet<Point>();
        private int m_totalFlash = 0;

        public void addLine(List<int> numbers)
        {
            int curY = m_points.Count();
            var curLine = new List<Point>();
            m_points.Add(curLine);

            numbers.ForEach(n =>
            {
                curLine.Add(new Point{ X = curLine.Count, Y = curY, Val = n});
            });

            m_maxX = Math.Max(m_maxX, curLine.Count);
            m_maxY = Math.Max(m_maxY, m_points.Count);

            debugWrite($"added line at {curY}, {numbers.Count}");
        }

        public override string ToString()
        {
            string str = "-------------------------\n";
            m_points.ForEach(line =>
            {
                line.ForEach(p =>
                {
                    str += p.Val.ToString() + " ";
                });
                str += "\n";
            });
            return str;
        }

        private void RunRound()
        {
            for (int y = 0; y < m_maxY; y ++)
            {
                for (int x = 0; x < m_maxX; x ++)
                {
                    GetPointAt(x, y)?.increase();
                }
            }

            m_flashed.Clear();

            for (int y = 0; y < m_maxY; y ++)
            {
                for (int x = 0; x < m_maxX; x ++)
                {
                    Point? p = GetPointAt(x, y);
                    if (p != null && p.Val == 0)
                    {
                        Flash(p);
                    }
                }
            }
        }

        private void Flash(Point p)
        {
            if (m_flashed.Contains(p))
            {
                return;
            }

            m_flashed.Add(p);
            m_totalFlash ++;
            debugWrite($"Flashing {p}");
            IncreaseAndFlash(GetPointAt(p.X - 1, p.Y - 1));
            IncreaseAndFlash(GetPointAt(p.X - 1, p.Y - 0));
            IncreaseAndFlash(GetPointAt(p.X - 1, p.Y + 1));
            IncreaseAndFlash(GetPointAt(p.X - 0, p.Y - 1));
            IncreaseAndFlash(GetPointAt(p.X - 0, p.Y + 1));
            IncreaseAndFlash(GetPointAt(p.X + 1, p.Y - 1));
            IncreaseAndFlash(GetPointAt(p.X + 1, p.Y - 0));
            IncreaseAndFlash(GetPointAt(p.X + 1, p.Y + 1));
        }

        private void IncreaseAndFlash(Point? p)
        {
            if (p != null && p.Val != 0)
            {
                debugWrite($"Increasing {p}");
                p.increase();

                if (p.Val == 0)
                {
                    Flash(p);
                }
            }
        }

        private Point? GetPointAt(int x, int y)
        {
            if (x >= 0 && x < m_maxX && y >= 0 && y < m_maxY)
            {
                return m_points[y][x];
            }
            return null;
        }

        public void SolvePart1(int numRound)
        {
            for (int i = 0; i < numRound; i ++)
            {
                RunRound();
                debugWrite($"After round {i + 1} \n{this}");
            }
            Console.WriteLine(ToString());
            Console.WriteLine($"Total flashes {m_totalFlash}");
        }

        public void SolvePart2()
        {
            int round = 0;

            while (m_flashed.Count != m_maxX * m_maxY)
            {
                RunRound();
                round ++;
            }

            Console.WriteLine($"Synchro round {round}");
        }

    }
    
	static void solve(string path)
	{
		var lines = File.ReadLines(path);

        Grid grid1 = new Grid();
        Grid grid2 = new Grid();
		foreach(string line in lines)
		{
			if (line.Length > 0)
			{
                var numbers = new List<int>();
                foreach (char c in line)
                {
                    int n = int.Parse(c.ToString());
                    numbers.Add(n);
                }
                grid1.addLine(new List<int>(numbers));
                grid2.addLine(new List<int>(numbers));
			}
		}

        grid1.SolvePart1(100);
        grid2.SolvePart2();
	}

	static void Run()
	{
		Console.WriteLine("Starting Advc21_11");

        solve("../../data/advc21_11_sample.txt");
		solve("../../data/advc21_11.txt");
	}

	static void Main()
	{
		Run();
	}    
}