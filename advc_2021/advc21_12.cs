
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_12
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    class Cave
    {
        public enum CaveType { Start, Big, Small, End };
        public string Name { get; set; }
        public CaveType caveType { get; set; } 
        public Cave(string name)
        {
            Name = name;
            if (Name == "start")
            {
                caveType = CaveType.Start;
            }
            else if (Name == "end")
            {
                caveType = CaveType.End;
            }
            else if (Name[0] >= 'A' && Name[0] <= 'Z')
            {
                caveType = CaveType.Big;
            }
            else
            {
                caveType = CaveType.Small;
            }
        }
    }

    class Path
    {
        public HashSet<Cave> Visited { get; } = new HashSet<Cave>();
        public List<Cave> PathSoFar { get; }= new List<Cave>();
        public Cave Current { get; }
        public Cave? SpecialTwiceSmallCave { get; set; }
        public Path(Cave current, Path? fromPath)
        {
            Current = current;

            if (fromPath != null)
            {
                foreach(Cave cave in fromPath.Visited)
                {
                    Visited.Add(cave);
                }
                PathSoFar.AddRange(fromPath.PathSoFar);
                SpecialTwiceSmallCave = fromPath.SpecialTwiceSmallCave;
            }

            Visited.Add(current);
            PathSoFar.Add(current);
        }
        public override string ToString()
        {
            string str = "";

            foreach(Cave cave in PathSoFar)
            {
                str += $"[{cave.Name}]";
            }

            return str;
        }

    }

    class PathFinder
    {
        private Dictionary<string, Cave> m_caves = new Dictionary<string, Cave>();
        private Dictionary<string, List<Cave>> m_connection = new Dictionary<string, List<Cave>>();
        private Cave? m_startCave;
        private List<Path> m_finalPaths = new List<Path>();

        public void AddConnection(string cave1Name, string cave2Name)
        {
            Cave cave1 = CreateOrGetCave(cave1Name);
            Cave cave2 = CreateOrGetCave(cave2Name);

            AddConnection(cave1, cave2);
            AddConnection(cave2, cave1);

            debugWrite($"Adding {cave1Name}'s connection to {cave2Name}");
        }

        public void Solve1()
        {
            if (m_startCave == null)
            {
                throw new Exception($"Start cave not found {m_startCave}");
            }

            int ans1 = FindPaths(new Path(m_startCave, null), allowSpecialCase: false);

            Console.WriteLine($"Solve 1 ans = {ans1}");
        }

        public void Solve2()
        {
            if (m_startCave == null)
            {
                throw new Exception($"Start cave not found {m_startCave}");
            }

            int ans2 = FindPaths(new Path(m_startCave, null), allowSpecialCase: true);

            Console.WriteLine($"Solve 2 ans = {ans2}");
        }

        private int FindPaths(Path path, bool allowSpecialCase)
        {
            debugWrite($"Entering cave {path}. Special : {path.SpecialTwiceSmallCave?.Name}");
            List<Cave> nextCaves = m_connection[path.Current.Name];

            foreach (Cave cave in nextCaves)
            {
                Path nextPath = new Path(cave, path);

                if (path.Current == cave)
                {
                    debugWrite($"Discarding parent cave : {nextPath}");
                    continue;
                }                
                if (cave.caveType == Cave.CaveType.Small && path.Visited.Contains(cave))
                {
                    if (!allowSpecialCase || path.SpecialTwiceSmallCave != null)
                    {
                        debugWrite($"Discarding visited small cave : {nextPath}");
                        continue;
                    }

                    nextPath.SpecialTwiceSmallCave = cave;
                    debugWrite($"allowing special small cave {cave.Name} \n that paths {nextPath}");
                }
                if (cave.caveType == Cave.CaveType.Start)
                {
                    debugWrite($"Discarding start cave : {nextPath}");
                    continue;
                }

                if (cave.caveType == Cave.CaveType.End)
                {
                    debugWrite($"Found path : {nextPath}");
                    m_finalPaths.Add(nextPath);
                }
                else
                {
                    FindPaths(nextPath, allowSpecialCase);
                }
            }

            return m_finalPaths.Count;
        }

        private void AddConnection(Cave from, Cave to)
        {
            if (!m_connection.ContainsKey(from.Name))
            {
                m_connection[from.Name] = new List<Cave>();
            }

            m_connection[from.Name].Add(to);
        }
        private Cave CreateOrGetCave(string caveName)
        {
            if (!m_caves.ContainsKey(caveName))
            {
                Cave newCave = m_caves[caveName] = new Cave(caveName);
                if (newCave.caveType == Cave.CaveType.Start)
                {
                    m_startCave = newCave;
                }
            }
            return m_caves[caveName];
        }
    }

	static void SolveMain(string path)
	{
        Console.WriteLine($"Reading File {path}");
		var lines = File.ReadLines(path);

        PathFinder pathFinder1 = new PathFinder();
        PathFinder pathFinder2 = new PathFinder();

		foreach(string line in lines)
		{

			if (line.Length > 0)
			{
                string[] caveNames = line.Split("-");
                pathFinder1.AddConnection(caveNames[0], caveNames[1]);
                pathFinder2.AddConnection(caveNames[0], caveNames[1]);
			}
		}

        pathFinder1.Solve1();
        pathFinder2.Solve2();
	}


	static void Run()
	{
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

		Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        SolveMain($"../../data/{className}_sample2.txt");
		SolveMain($"../../data/{className}.txt");
	}

	static void Main()
	{
		Run();
	}
}