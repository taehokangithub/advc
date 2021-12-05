
using System;
using System.Collections.Generic;
using System.IO;


class Advc20_05
{
	struct Point 
	{
		public int x, y;
		public Point(int a, int b) 
		{ 
			x = a; 
			y = b; 
		}
		public override string ToString()
		{
			return "(" + x + "," + y + ")";
		}		
	}

	struct Line
	{
		public Point start, end;
		public Line(Point a, Point b) 
		{ 
			start = a; 
			end = b; 
		}
		public override string ToString()
		{
			return "[" + start + " -> " + end + "]";
		}
	}
	class Grid
	{
		private List<List<int>> m_grid;
		public Grid(int x, int y)
		{
			m_grid = new List<List<int>>(new List<int>[y + 1]);

			for (int i = 0; i < m_grid.Count; i ++)
			{
				m_grid[i] = new List<int>(new int[x + 1]);
			}
		}
		public void DrawLine(Line line)
		{
			int getStep(int a, int b) => (a == b ? 0 : (a < b ? 1 : -1));
			int xStep = getStep(line.start.x, line.end.x);
			int yStep = getStep(line.start.y, line.end.y);
			int xMax = Math.Max(line.start.x, line.end.x);
			int yMax = Math.Max(line.start.y, line.end.y);
			int xMin = Math.Min(line.start.x, line.end.x);
			int yMin = Math.Min(line.start.y, line.end.y);			

			//Console.WriteLine($"Drawing {line}, xStep {xStep}, yStep {yStep}, xMax {xMax}, yMax {yMax}");

			for (int x = line.start.x, y = line.start.y; 
				x <= xMax && y <= yMax && x >= xMin && y >= yMin;
				x += xStep, y += yStep)
			{
				
				try
				{
					//Console.WriteLine($"-- Point {x},{y} ");
					m_grid[y][x] ++;
				}
				catch(Exception e)
				{
					Console.WriteLine($"Failed to DrawLine() x {x} y {y}. {e}");
					Environment.Exit(0);
				}
				
			}
		}

		public void PrintGrid()
		{
			foreach (var line in m_grid)
			{
				foreach (int val in line)
				{
					Console.Write(val);
				}
				Console.Write("\n");
			}
		}

		public int GetCountOver2()
		{
			int cnt = 0;
			foreach (var line in m_grid)
			{
				foreach (int val in line)
				{
					if (val > 1)
					{
						cnt ++;
					}
				}
			}
			return cnt;
		}

	}

	class Solver
	{
		private enum SolveType 
		{
			EXCLUDE_DIAGONAL,
			INCLUDE_DIAGONAL
		}
				
		private List<Line> m_lines = new List<Line>();
		private Grid m_grid = new Grid(0,0);
		private int m_maxX = 0;
		private int m_maxY = 0;

		public void AddLine(Line line)
		{
			m_maxX = Math.Max(Math.Max(line.start.x, line.end.x), m_maxX);
			m_maxY = Math.Max(Math.Max(line.start.y, line.end.y), m_maxY);
			m_lines.Add(line);
		}
		public void BuildGrid()
		{
			m_grid = new Grid(m_maxX, m_maxY);
		}

		public void PrintAllLines()
		{
			foreach(var line in m_lines)
			{
				Console.WriteLine(line);
			}
		}

		private int Solve(SolveType solveType)
		{
			BuildGrid();

			foreach (Line line in m_lines)
			{
				if (solveType == SolveType.EXCLUDE_DIAGONAL)
				{
					if (line.start.x == line.end.x || line.start.y == line.end.y)
					{
						m_grid.DrawLine(line);
					}
				}
				else
				{
					m_grid.DrawLine(line);
				}
			}

			return m_grid.GetCountOver2();
		}
		public void SolvePart1()
		{
			int ans = Solve(SolveType.EXCLUDE_DIAGONAL);
			Console.WriteLine($"Part 1 answer = {ans}");
		}
		public void SolvePart2()
		{
			int ans = Solve(SolveType.INCLUDE_DIAGONAL);
			Console.WriteLine($"Part 2 answer = {ans}");
			m_grid.PrintGrid();
		}
	}

	static Solver ReadInput(string path)
	{
		var lines = File.ReadLines(path);

		Solver solver = new Solver();

		foreach(string line in lines)
		{
			//Console.WriteLine(line);
			if (line.Length > 0)
			{
				List<Point> pList = new List<Point>();

				foreach(string pointString in line.Split(" -> "))
				{
					string[] coord = pointString.Split(",");
					pList.Add(new Point(Int32.Parse(coord[0]), Int32.Parse(coord[1])));
				}

				solver.AddLine(new Line(pList[0], pList[1]));
			}
		}

		return solver;
	}

	static void Main()
	{
		Console.WriteLine("Starting Advc21_05");
		Solver solver = ReadInput("../../data/advc21_05.txt");

		solver.SolvePart1();
		solver.SolvePart2();
		
	}
}