
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.IO;


class Advc20_11
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    class Grid 
    {
        enum SeatType 
        {
            FLOOR, EMPTY, OCCUPIED, OUT_OF_BOUND
        };

        static readonly List<Vector2> s_dir = new List<Vector2> {
            new Vector2(-1, -1), new Vector2(0, -1), new Vector2(1, -1),
            new Vector2(-1, 0), new Vector2(1, 0),
            new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 1)
        };

        private List<List<SeatType>> m_grid = new List<List<SeatType>>();
        private int max_x = 0;
        private int max_y = 0;
        private int m_curRound = 0;
        private int m_currentOccupied = 0;

        public void AddLine(string line)
        {
            List<SeatType> seats = new List<SeatType>();

            foreach (char c in line)
            {
                SeatType seat = SeatType.OUT_OF_BOUND;

                switch (c)
                {
                    case '.' :  seat = SeatType.FLOOR; break;
                    case 'L' :  seat = SeatType.EMPTY; break;
                }

                seats.Add(seat);
            }

            m_grid.Add(seats);

            max_x = seats.Count();
            max_y = m_grid.Count;
        }

        public override string ToString()
        {
            string ret = "----------------------------------\n";

            foreach(var line in m_grid)
            {
                foreach(SeatType seat in line)
                {
                    switch (seat)
                    {
                        case SeatType.FLOOR : ret += '.'; break;
                        case SeatType.EMPTY : ret += 'L'; break;
                        case SeatType.OCCUPIED : ret += '#'; break;
                        case SeatType.OUT_OF_BOUND :
                            throw new Exception($"Unknown seat type {seat}");
                    }
                }
                ret += "\n";
            }
            return ret;
        }

        private class PendingSet
        {
            public int x { get; set; }
            public int y { get; set; }
            public SeatType seat { get; set; }
        }

        private List<PendingSet> m_pendingSet = new List<PendingSet>();

        private void PendingSetAt(int x, int y, SeatType seat)
        {
            PendingSet set = new PendingSet{x = x, y = y, seat = seat};
            m_pendingSet.Add(set);
        }

        private void ProcessPendingSet()
        {
            foreach (var set in m_pendingSet)
            {
                m_grid[set.y][set.x] = set.seat;
            }

            m_pendingSet.Clear();
        }
        
        private SeatType GetAt(int x, int y)
        {
            if (x >= 0 && x < max_x && y >= 0 && y < max_y)
            {
                return  m_grid[y][x];
            }
            return SeatType.OUT_OF_BOUND;
        }

        private SeatType SeeAt(int x, int y, Vector2 dir, bool seeDirection)
        {
            SeatType seat;

            do
            {
                x += (int) dir.X;
                y += (int) dir.Y;
                seat = GetAt(x, y);
            }
            while (seat == SeatType.FLOOR && seeDirection);

            return seat;
        }

        private bool DoRound(int tolerance, bool seeDirection)
        {
            bool hasChanged = false;
            m_curRound ++;
            m_currentOccupied = 0;

            for (int y = 0; y < max_y; y ++)
            {
                for (int x = 0; x < max_x; x ++)
                {
                    SeatType curSeat = GetAt(x, y);

                    if (curSeat == SeatType.FLOOR)
                    {
                        continue;
                    }
                    if (curSeat == SeatType.OUT_OF_BOUND)
                    {
                        throw new Exception("Found out of bound");
                    }

                    int cntOccupied = 0;

                    foreach(Vector2 dir in s_dir)
                    {
                        SeatType seat = SeeAt(x, y, dir, seeDirection);

                        if (seat == SeatType.OCCUPIED)
                        {
                            cntOccupied ++;
                        }
                    }
                    if (cntOccupied == 0 && curSeat == SeatType.EMPTY)
                    {
                        hasChanged = true;
                        curSeat = SeatType.OCCUPIED;
                    }
                    else if (cntOccupied >= tolerance && curSeat == SeatType.OCCUPIED)
                    {
                        hasChanged = true;
                        curSeat = SeatType.EMPTY;
                    }

                    if (curSeat == SeatType.OCCUPIED)
                    {
                        m_currentOccupied ++;
                    }

                    if (hasChanged)
                    {
                        PendingSetAt(x, y, curSeat);
                    }
                }
            }
            ProcessPendingSet();

            debugWrite($"Round {m_curRound}, occupied {m_currentOccupied}");
            debugWrite(this.ToString());            
            return hasChanged;
        }

        public void Solve1()
        {
            while(DoRound(tolerance: 4, seeDirection: false));

            Console.WriteLine($"Solve1 ans {m_currentOccupied}");
        }

        public void Solve2()
        {
            while(DoRound(tolerance: 5, seeDirection: true));

            Console.WriteLine($"Solve2 ans {m_currentOccupied}");            
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

        grid1.Solve1();
        grid2.Solve2();
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