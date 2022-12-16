using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class State 
    {
        public List<GraphNode> Nodes { get; set; }
        public HashSet<GraphNode> Opened = new();
        public int Index { get; private set; } = 0;
        public bool IsFirst => Index == 0;
        public bool IsLast => Index == Nodes.Count - 1;
        public GraphNode Node => Nodes[Index];

        public override string ToString()
        {
            string nodeNames = string.Join("/", Nodes.Select(s => s.Name));
            string opened = string.Join("/", Opened.Select(n => n.Name));
            return $"[{nodeNames}:open({opened})]";
        }

        public State(List<GraphNode> nodes)
        {
            Nodes = new(nodes);
        }

        public State(State other)
        {
            Nodes = new (other.Nodes);
            Opened = new(other.Opened);
            Index = other.Index;
        }

        public void IncreaseIndex()
        {
            Index = (Index + 1) % Nodes.Count;
        }

        public void SetNode(GraphNode node)
        {
            Nodes[Index] = node;
        }
    }

    class Stat
    {
        public int MinutesLeft { get; set; } = 0;
        public long ReleasedPressure { get; set; } = 0;

        public override string ToString()
        {
            return $"[{this.MinutesLeft} left, {this.ReleasedPressure} released]";
        }

        public Stat() {}
        public Stat(Stat other)
        {
            MinutesLeft = other.MinutesLeft;
            ReleasedPressure = other.ReleasedPressure;
        }
    }

    class SearchState
    {
        public State State { get; set; }
        public Stat CurStat { get; set; } = new();
        public HashSet<string> Visited = new();
        public List<string> PathRecord = new(); // for debugging

        public override string ToString()
        {
            string path = $"{string.Join("-", PathRecord)}";
            return $"<{State}{CurStat}(path {path.Length})>";
        }

        public SearchState(GraphNode node, int minutes, int cntNodes)
        {
            List<GraphNode> nodes = new();
            for (int i = 0; i < cntNodes; i ++)
            {
                nodes.Add(node);
            }
            State = new State(nodes);
            CurStat.MinutesLeft = minutes;
        }

        private SearchState(SearchState other) 
        {
            State = new (other.State);
            CurStat = new (other.CurStat);
            Visited = new (other.Visited);
            PathRecord = new (other.PathRecord);
        }

        public bool CanOpen()
        {
            return State.Node.Value > 0 && !State.Opened.Contains(State.Node);
        }

        public bool CanVisit(GraphNode node)
        {
            State newState = new(State);
            newState.SetNode(node);
            return !Visited.Contains(newState.ToString());
        }

        private void AddVisited()
        {
            Debug.Assert(!Visited.Contains(State.ToString()));

            Visited.Add(State.ToString());
            PathRecord.Add(State.Node.Name);
        }

        private void DecreaseMinuteIfNeeded()
        {
            if (State.IsFirst)
            {
                CurStat.MinutesLeft --;
            }
        }

        public long GetAssumedPressure(NamedGraph map)
        {
            long assumedPressure = 0;
            int minutesLeft = CurStat.MinutesLeft;
            foreach (var (_, node) in map.Nodes.OrderByDescending(n => n.Value.Value))
            {
                if (!State.Opened.Contains(node))
                {
                    assumedPressure += node.Value * minutesLeft;
                    minutesLeft -= 2;
                }
                if (node.Value == 0)
                {
                    break;
                }
            }
            return assumedPressure + CurStat.ReleasedPressure;
        }
        public static SearchState NewSearchByOpenValve(SearchState other)
        {
            var searchState = new SearchState(other);

            var state = searchState.State;
            Debug.Assert(!state.Opened.Contains(state.Node));
            state.Opened.Add(state.Node);

            searchState.DecreaseMinuteIfNeeded();
            var curStat = searchState.CurStat;
            curStat.ReleasedPressure += curStat.MinutesLeft * state.Node.Value;
            Debug.Assert(curStat.ReleasedPressure > 0);

            searchState.AddVisited();
            state.IncreaseIndex();
            return searchState;
        }

        public static SearchState NewSearchByVisit(SearchState other, GraphNode moveTo)
        {
            var searchState = new SearchState(other);

            var state = searchState.State;
            Debug.Assert(state.Node.Path.Contains(moveTo));
            state.SetNode(moveTo);

            searchState.DecreaseMinuteIfNeeded();
            searchState.AddVisited();
            
            state.IncreaseIndex();
            return searchState;
        }
    }

    class ShortestRecord
    {
        private Dictionary<string, Stat> m_records = new();
        private NamedGraph m_map;

        public long RecordPressureSoFar = 0;

        public ShortestRecord(NamedGraph map)
        {
            m_map = map;
        }

        public bool CheckAndAddRecord(SearchState searchState)
        {
            string stateStr = searchState.State.ToString();

            if (m_records.TryGetValue(stateStr, out var stat))
            {
                if (stat.MinutesLeft >= searchState.CurStat.MinutesLeft
                    && stat.ReleasedPressure >= searchState.CurStat.ReleasedPressure)
                {
                    return false;
                }
            }

            // assume all opened immediately
            if (searchState.GetAssumedPressure(m_map) < RecordPressureSoFar)
            {
                return false; // Can't reach the record-so-far even if all the remaining valves get opened right now
            }

            m_records[stateStr] = searchState.CurStat;

            RecordPressureSoFar = Math.Max(RecordPressureSoFar, searchState.CurStat.ReleasedPressure);
            return true;
        }
    }

    class ValveMap : Loggable
    {
        NamedGraph m_grp = new();

        public void Parse(string[] lines)
        {
            var checkAndGetRest = (string str, string checkPart) =>
            {
                Debug.Assert(str.StartsWith(checkPart));
                return str.Substring(checkPart.Length);
            };

            foreach (var line in lines)
            {
                var parts = line.Split("; ");
                var vparts = parts.First().Split(" has flow rate=");
                string valveName = checkAndGetRest(vparts.First(), "Valve ");
                int rate = int.Parse(vparts.Last());
                var tunnelPart = parts.Last();
                tunnelPart = tunnelPart.Replace("tunnels", "tunnel").Replace("leads", "lead").Replace("valves", "valve");
                var targetsPart = checkAndGetRest(tunnelPart, "tunnel lead to valve ");
                string[] targets = targetsPart.Split(", ");

                foreach (var target in targets)
                {
                    m_grp.AddPath(valveName, target);
                }

                var valve = m_grp.GetNodeByName(valveName);
                valve.Value = rate;
            }
        }

        public long FindMostReleasedPressure(int minute, int cntNodes)
        {
            GraphNode startNode = m_grp.GetNodeByName("AA");
            SearchState searchState = new(startNode, minute, cntNodes);

            Queue<SearchState> queue = new();
            queue.Enqueue(searchState);

            ShortestRecord sr = new(m_grp);

            bool orgLogsDetail = AllowLogDetail;
            const int logFrequency = 10000;
            int logCnt = 1;
            while (queue.Any())
            {
                AllowLogDetail = logCnt++ % logFrequency == (logFrequency - 1);
                searchState = queue.Dequeue();
                Debug.Assert(searchState.State.Opened.Count == 0 || searchState.CurStat.ReleasedPressure > 0);
                long assumed = searchState.GetAssumedPressure(m_grp);

                if (assumed < sr.RecordPressureSoFar)
                {
                    continue;
                }

                if (searchState.CurStat.MinutesLeft <= 0)
                {
                    continue;
                }

                if (searchState.State.Opened.Count == m_grp.Nodes.Count)
                {
                    continue;
                }

                LogDetail($"[{logCnt}] Examining {searchState}, Q {queue.Count}, record {assumed}/{sr.RecordPressureSoFar}");

                for (int i = 0; i < cntNodes; i ++)
                {
                    if (searchState.CanOpen())
                    {
                        var newSearch = SearchState.NewSearchByOpenValve(searchState);
                        
                        if (sr.CheckAndAddRecord(newSearch))
                        {
                            queue.Enqueue(newSearch);
                            LogDetail($"[{logCnt}] Opening {newSearch.State.Node.Name} => {newSearch}");
                        }
                    }

                    foreach (var target in searchState.State.Node.Path)
                    {
                        if (searchState.CanVisit(target))
                        {
                            var newSearch = SearchState.NewSearchByVisit(searchState, target);

                            if (sr.CheckAndAddRecord(newSearch))
                            {
                                queue.Enqueue(newSearch);
                                LogDetail($"[{logCnt}] Visiting {newSearch.State.Node.Name} => {newSearch}");
                            }
                        }
                    }
                }
            }

            AllowLogDetail = orgLogsDetail;
            return sr.RecordPressureSoFar;
        }
    }
    class Problem16 : Loggable
    {
        public long Solve1(ValveMap vm)
        {
            vm.AllowLogDetail = false;

            return vm.FindMostReleasedPressure(30, 1);
        }

        public long Solve2(ValveMap vm)
        {
            vm.AllowLogDetail = true;

            return vm.FindMostReleasedPressure(26, 2);
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input16.txt");
            var textArr = textData.Split(Environment.NewLine);

            ValveMap vm = new();
            vm.Parse(textArr);
            Problem16 prob1 = new();

            var ans1 = prob1.Solve1(vm);
            var ans2 = prob1.Solve2(vm);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


