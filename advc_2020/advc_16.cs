
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc20_16
{
    static bool s_debugWrite = true;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    class Range
    {
        public int Max { get; set; }
        public int Min { get; set; }
        public Range(string str)
        {
            var values = str.Split("-");
            Min = Convert.ToInt32(values[0]);
            Max = Convert.ToInt32(values[1]);
        }

        public bool In(int val)
        {
            return Min <= val && val <= Max;
        }

        public override string ToString()
        {
            return $"({Min},{Max})";
        }
    }

    class InfoType
    {
        List<Range> m_ranges = new List<Range>();
        public string Name { get; }

        public InfoType(string str)
        {
            var values = str.Split(": ");
            Name = values[0];

            var ranges = values[1].Split(" or ");
            m_ranges.Add(new Range(ranges[0]));
            m_ranges.Add(new Range(ranges[1]));
        }

        public bool IsValidValue(int val)
        {
            for (int i = 0; i < m_ranges.Count; i ++)
            {
                if (m_ranges[i].In(val))
                {
                    return true;
                }
            }
            return false;
        }

        public override string ToString()
        {
            return $"[{Name}:{m_ranges[0]},{m_ranges[1]}]";
        }
    };

    class Ticket
    {
        public List<int> Values { get; } = new List<int>();

        public Ticket(string line)
        {
            foreach (string value in line.Split(","))
            {
                Values.Add(Convert.ToInt32(value));
            }
        }

        public Ticket()
        {
        }

        public override string ToString()
        {
            string str = "";

            Values.ForEach((v) =>
            {
                str += v.ToString() + ":";
            });

            str = str.Remove(str.Length - 1);

            return str;
        }        
    };

    class TicketPosGuess
    {
        public int Index { get; } = 0;
        public HashSet<string> PossibleTypes { get; } = new HashSet<string>();
        public HashSet<string> ImpossibleTypes { get; } = new HashSet<string>();

        public TicketPosGuess(int index)
        {
            Index = index;
        }

        public override string ToString()
        {
            string str = $"<pos-{Index.ToString("D2")}> ";

            var list = PossibleTypes.ToList();
            list.Sort();

            foreach(string name in list)
            {
                str += $"[{name}], ";
            }
            str = str.Substring(0, str.Length - 2);
            return str;
        }

        public void Eliminate()
        {
            foreach(string name in ImpossibleTypes)
            {
                PossibleTypes.Remove(name);
            }
        }

        public bool IsCompleted()
        {
            return PossibleTypes.Count == 1;
        }

        public string GetGuessed()
        {
            Debug.Assert(IsCompleted());

            return PossibleTypes.ToList()[0];
        }
    }

    class Solver
    {
        public List<InfoType> InfoTypes { get; } = new List<InfoType>();
        public List<Ticket> NearbyTickets { get; } = new List<Ticket>();
        private List<TicketPosGuess> TicketPosGuesses { get; } = new List<TicketPosGuess>();
        public Ticket YourTicket { get; set; } = new Ticket();

        public override string ToString()
        {
            string str = "";

            InfoTypes.ForEach(info => str += info.ToString() + "\n");

            str += $"Your Ticket :\n{YourTicket.ToString()}\n";

            str += "Nearby Tickets :\n";

            NearbyTickets.ForEach(ticket => str += ticket.ToString() + "\n");

            return str;
        }

        private void InitTicketPosGuesses()
        {
            int numPos = YourTicket.Values.Count;

            for (int i = 0; i < numPos; i ++)
            {
                TicketPosGuesses.Add(new TicketPosGuess(i));
            }
        }

        private void TidyupGuesses()
        {
            foreach (var guess in TicketPosGuesses)
            {
                guess.Eliminate();
            }

            HashSet<int> completed = new HashSet<int>();
            bool hasAnyChange = true;
            while (hasAnyChange)
            {
                hasAnyChange = false;
                foreach (var guess in TicketPosGuesses)
                {
                    if (guess.IsCompleted() && !completed.Contains(guess.Index))
                    {
                        completed.Add(guess.Index);
                        string thisAnswer = guess.GetGuessed();
                        hasAnyChange = true;

                        foreach (var otherGuess in TicketPosGuesses)
                        {
                            if (!completed.Contains(otherGuess.Index))
                            {
                                otherGuess.PossibleTypes.Remove(thisAnswer);
                            }
                        }
                    }
                }
            }
        }

        public void Solve()
        {
            InitTicketPosGuesses();

            int solve1ans = 0;

            for (int ticketIdx = 0; ticketIdx < NearbyTickets.Count; ticketIdx ++)
            {
                bool isValid = false;

                Ticket ticket = NearbyTickets[ticketIdx];

                Dictionary<int, List<InfoType>> possibleTypes = new Dictionary<int, List<InfoType>>();
                Dictionary<int, List<InfoType>> impossibleTypes = new Dictionary<int, List<InfoType>>();

                for (int pos = 0; pos < ticket.Values.Count; pos ++)
                {
                    isValid = false;    // reset validation for each values

                    int val = ticket.Values[pos];
                    possibleTypes[pos] = new List<InfoType>();
                    impossibleTypes[pos] = new List<InfoType>();

                    // see if this value is any fit
                    for (int k = 0; k < InfoTypes.Count; k ++)
                    {
                        var info = InfoTypes[k];
                        if (info.IsValidValue(val))
                        {
                            isValid = true;
                            possibleTypes[pos].Add(info);
                        }
                        else
                        {
                            impossibleTypes[pos].Add(info);
                        }
                    }
                    if (!isValid)
                    {
                        solve1ans += val;
                        break;
                    }
                }

                if (isValid)
                {
                    foreach (var item in possibleTypes)
                    {
                        item.Value.ForEach(info => TicketPosGuesses[item.Key].PossibleTypes.Add(info.Name));
                    }
                    foreach (var item in impossibleTypes)
                    {
                        item.Value.ForEach(info => TicketPosGuesses[item.Key].ImpossibleTypes.Add(info.Name));
                    }
                }
            }

            TidyupGuesses();

            long solve2ans = 1;
            foreach (var guess in TicketPosGuesses)
            {
                debugWrite(guess.ToString());

                if (guess.GetGuessed().StartsWith("departure"))
                {
                    solve2ans *= YourTicket.Values[guess.Index];
                }
            }

            Console.WriteLine($"Solve1 Ans = {solve1ans}");
            Console.WriteLine($"Solve2 Ans = {solve2ans}");
        }

    }

    enum Phase
    {
        Infotypes, YourTicket, NearbyTickets
    };

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Phase p = Phase.Infotypes;

        Solver solver = new Solver();

        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                if (line == "your ticket:")
                {
                    p = Phase.YourTicket;
                }
                else if (line == "nearby tickets:")
                {
                    p = Phase.NearbyTickets;
                }
                else if (p == Phase.Infotypes)
                {
                    solver.InfoTypes.Add(new InfoType(line));
                }
                else if (p == Phase.NearbyTickets)
                {
                    solver.NearbyTickets.Add(new Ticket(line));
                }
                else if (p == Phase.YourTicket)
                {
                    solver.YourTicket = new Ticket(line);
                }
                else
                {
                    Debug.Assert(false, "Shouldn't be here");
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