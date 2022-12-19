using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem19 : Loggable
    {
        enum Mineral 
        {
            Ore,
            Clay,
            Obsidian,
            Geode
        }

        private static Mineral GetMineralFromString(string str)
        {
            foreach(Mineral m in Enum.GetValues(typeof(Mineral)).Cast<Mineral>())
            {
                if (str == m.ToString().ToLower())
                {
                    return m;
                }
            }
            throw new ArgumentException($"Unknown mineral name {str}");
        }


        class RobotBlueprint
        {
            public Mineral OutputType { get; set; }
            public Dictionary<Mineral, int> Ingredients = new();

            public override string ToString()
            {
                var ingrStr = string.Join(",", Ingredients.Select(p => $"({p.Key},{p.Value}").ToList());
                return $"[{OutputType}:{ingrStr}]";
            }

            public static RobotBlueprint Parse(string line)
            {
                line = line.Trim();
                Queue<string> words = new(line.Split(" ").ToList());

                GenericUtil.ExpectString(words.Dequeue(), "Each");

                RobotBlueprint rbp = new();
                rbp.OutputType = GetMineralFromString(words.Dequeue());

                GenericUtil.ExpectString(words.Dequeue(), "robot");
                GenericUtil.ExpectString(words.Dequeue(), "costs");

                while (words.Any())
                {
                    int amount = int.Parse(words.Dequeue());
                    Mineral ingr = GetMineralFromString(words.Dequeue());
                    rbp.Ingredients[ingr] = amount;

                    if (words.Any())
                    {
                        GenericUtil.ExpectString(words.Dequeue(), "and");
                    }
                }

                return rbp;
            }
        }

        enum Phase { Decision, Process }

        class State
        {
            private static int s_nextIndex = 0;
            public int Index { get; private set; } = ++ s_nextIndex;
            public Dictionary<Mineral, int> NumRobots { get; set; } = new();
            public Dictionary<Mineral, int> NumMinerals { get; set; } = new();

            public int Minutes { get; set; }
            public Mineral? NextProduction { get; set; }
            public Phase Phase { get; set; } = Phase.Decision;
            public List<State> Path = new();
            public int AssumedMaxGeode { get; set; }
            
            private BluePrint m_bp;

            public State(BluePrint bp) 
            {
                m_bp = bp;
                NumRobots[Mineral.Ore] = 1;

                foreach (Mineral mineral in Enum.GetValues<Mineral>().ToList())
                {
                    NumMinerals[mineral] = 0;
                }

                Minutes = 1;
            }
            public State(State other)
            {
                NumRobots = new(other.NumRobots);
                NumMinerals = new(other.NumMinerals);
                Minutes = other.Minutes;
                NextProduction = other.NextProduction;
                Phase = other.Phase;
                Path = new(other.Path);

                if (other.Path.LastOrDefault()?.ToString() != other.ToString())
                {
                    Path.Add(other);
                }
                m_bp = other.m_bp;
            }

            public override string ToString()
            {
                string nextStr = (NextProduction == null) ? "" : $"/ Next {NextProduction}";
                return $"[State: {Minutes} min ({Phase}) / Resources ({string.Join(",", NumMinerals.Values)}) / Robots ({string.Join(",", NumRobots.Values)}) {nextStr} ";
            }

            public List<Mineral> GetPossibleProductions(BluePrint bp)
            {
                Debug.Assert(Phase == Phase.Decision);
                List<Mineral> possibleProduction = new();

                foreach (var mineral in Enum.GetValues<Mineral>().ToList())
                {
                    var bpRobot = bp.RobotBlueprints[mineral];
                    bool canProduce = true;
                    foreach (Mineral ingr in bpRobot.Ingredients.Keys)
                    {
                        if (!NumRobots.ContainsKey(ingr) || NumRobots[ingr] == 0)
                        {
                            canProduce = false;
                        }
                    }
                    if (canProduce)
                    {
                        possibleProduction.Add(mineral);
                    }
                }

                return possibleProduction;
            }

            public void ProcessMining()
            {
                Debug.Assert(Phase == Phase.Process);

                foreach (var mineral in NumRobots.Keys)
                {
                    NumMinerals[mineral] = NumMinerals[mineral] + NumRobots[mineral];
                }
            }

            public bool PrepareProduction()
            {
                Debug.Assert(Phase == Phase.Process && NextProduction != null);

                Mineral robotType = NextProduction.GetValueOrDefault();
                var bpRobot = m_bp.RobotBlueprints[robotType];

                if (bpRobot.Ingredients.Any(ingr => NumMinerals[ingr.Key] < ingr.Value))
                {
                    return false;
                }
                return true;
            }

            public void ProcessProduction()
            {
                Debug.Assert(Phase == Phase.Process && NextProduction != null);

                Mineral robotType = NextProduction.GetValueOrDefault();
                var bpRobot = m_bp.RobotBlueprints[robotType];

                bpRobot.Ingredients.ToList().ForEach(ingr => 
                {
                    NumMinerals[ingr.Key] -= ingr.Value;
                    Debug.Assert(NumMinerals[ingr.Key] >= 0);
                });

                // Add a robot
                if (!NumRobots.ContainsKey(robotType))
                {
                    NumRobots[robotType] = 0;
                }
                NumRobots[robotType] ++;

                NextProduction = null;     
            }

            public void SetAssumedMaxGeode(int maxMinutes)
            {
                int remaining = maxMinutes - Minutes;

                int geodeRbCnt = NumRobots.ContainsKey(Mineral.Geode) ? NumRobots[Mineral.Geode] : 0;
                int assumedGeode = NumMinerals[Mineral.Geode];

                int curObsidian = NumMinerals[Mineral.Obsidian];
                int curObsiRobot = NumRobots.ContainsKey(Mineral.Obsidian) ? NumRobots[Mineral.Obsidian] : 0;
                int requiredObsidian = m_bp.RobotBlueprints[Mineral.Geode].Ingredients[Mineral.Obsidian];

                int curClay = NumMinerals[Mineral.Clay];
                int curClayRobot = NumRobots.ContainsKey(Mineral.Clay) ? NumRobots[Mineral.Clay] : 0;
                int requiredClay = m_bp.RobotBlueprints[Mineral.Obsidian].Ingredients[Mineral.Clay];

                for (int i = 0; i <= remaining; i ++)
                {
                    assumedGeode += geodeRbCnt;
                    curClay += curClayRobot;
                    curObsidian += curObsiRobot;

                    curClayRobot ++;

                    if (curClay >= requiredClay)
                    {
                        curClay -= requiredClay;
                        curObsiRobot ++;
                    }

                    if (curObsidian >= requiredObsidian)
                    {
                        curObsidian -= requiredObsidian;
                        geodeRbCnt ++;
                    }
                }
                AssumedMaxGeode = assumedGeode;
            }
        }

        class BluePrint : Loggable
        {
            public int Index { get; init; }
            public Dictionary<Mineral, RobotBlueprint> RobotBlueprints { get; set; } = new();

            public BluePrint(int index)
            {
                Index = index;
            }

            public override string ToString()
            {
                StringBuilder sb = new();
                sb.Append($"[BluePrint {Index.ToString().PadLeft(2)}]{Environment.NewLine}");
                foreach (var rb in RobotBlueprints)
                {
                    sb.Append($"     {rb.ToString()}{Environment.NewLine}");
                }
                return sb.ToString();
            }

            public static BluePrint Parse(string line)
            {
                Queue<string> linesQueue = new();
                var first2lines = line.Split(": ");
                string firstLine = first2lines.First();
                int index = int.Parse(GenericUtil.ExpectString(firstLine, "Blueprint "));
                BluePrint bluePrint = new(index);

                var bpLines = first2lines.Last().Split(".");
                foreach(var bpLine in bpLines)
                {
                    if (bpLine.Length == 0)
                    {
                        break;
                    }

                    RobotBlueprint rbp = RobotBlueprint.Parse(bpLine);
                    bluePrint.RobotBlueprints.Add(rbp.OutputType, rbp);
                }
                return bluePrint;
            }

            private List<State> GetNextStatesForPossibleProduction(State state)
            {
                List<State> nextStates = new();
                var possibleProduction = state.GetPossibleProductions(this);

                foreach (var robotType in possibleProduction)
                {
                    var newState = new State(state);
                    newState.NextProduction = robotType;
                    newState.Phase = Phase.Process;
                    nextStates.Add(newState);
                }

                return nextStates;
            }

            public int FindBestResult(int minutes)
            {
                State initialState = new(this);
                //Queue<State> stateQ = new();

                SortedSet<State> stateQ = new SortedSet<State>(Comparer<State>.Create((a,b) => 
                {
                    if (a.NumMinerals[Mineral.Geode] != b.NumMinerals[Mineral.Geode])
                    {
                        return b.NumMinerals[Mineral.Geode] - a.NumMinerals[Mineral.Geode];
                    }

                    if (a.AssumedMaxGeode != b.AssumedMaxGeode)
                    {
                        return b.AssumedMaxGeode - a.AssumedMaxGeode;
                    }

                    if (a.Minutes != b.Minutes)
                    {
                        return b.Minutes - a.Minutes;
                    }

                    return a.Index - b.Index;
                }));

                var addToStateQ = (State s) => 
                {
                    s.SetAssumedMaxGeode(minutes);
                    stateQ.Add(s);
                };

                addToStateQ(initialState);

                int bestResult = 0;
                const int logFreq = 10000;
                int logIndex = 0;
                int totalDiscarded = 0;
                while (stateQ.Any())
                {
                    ++logIndex;
                    var LogTrace = (string str) => { if ((logIndex) % logFreq == 0) LogDetail($"[{this.Index}/{logIndex}] {str}"); };

                    var state = stateQ.First();
                    stateQ.Remove(state);

                    if (state.AssumedMaxGeode <= bestResult)
                    {
                        LogTrace($" ==> discrading {state} for assumed {state.AssumedMaxGeode} <= {bestResult}");
                        totalDiscarded ++;
                        continue;
                    }

                    LogTrace($"<Q {stateQ.Count}/Best {bestResult}/Discard {totalDiscarded}> {state}");

                    if (state.Phase == Phase.Decision)
                    {
                        if (state.NextProduction == null)
                        {
                            GetNextStatesForPossibleProduction(state).ForEach(s => 
                            {
                                addToStateQ(s);
                                //LogTrace($"     : Enqueing {state}, cur count {stateQ.Count}");
                            });
                        }
                        else
                        {
                            state.Phase = Phase.Process;
                        }
                    }

                    if (state.Phase == Phase.Process)
                    {
                        bool prepared = state.PrepareProduction();
                        state.ProcessMining();
                        if (prepared)
                        {
                            state.ProcessProduction();
                        }
                        
                        // Check geode
                        int geode = state.NumMinerals[Mineral.Geode];

                        if (geode > bestResult)
                        {
                            LogTrace($" =====> Found a new best {geode} over {bestResult} at state {state}");
                            foreach (var s in state.Path)
                            {
                                LogTrace($" =====> prev : {s}");
                            }

                            bestResult = geode;
                        }
                        
                        if (state.Minutes >= minutes)
                        {
                            LogTrace($" =====> Finished : {state}");
                        }
                        else
                        {
                            var newState = new State(state);
                            newState.Phase = Phase.Decision;
                            newState.Minutes ++;
                            addToStateQ(newState);
                        }
                    }
                }

                return bestResult;
            }
        }

        class BluePrintSet : Loggable
        {
            private List<BluePrint> BluePrints { get; set; } = new();

            public override string ToString()
            {
                return string.Join(Environment.NewLine, BluePrints);
            }

            public static BluePrintSet Parse(string[] lines)
            {
                BluePrintSet bluePrintSet = new();

                Queue<string> linesQueue = new(lines);

                while (linesQueue.Any())
                {
                    string line = linesQueue.Dequeue();
                    BluePrint bp = BluePrint.Parse(line);
                    bluePrintSet.BluePrints.Add(bp);
                }

                return bluePrintSet;
            }

            public int FindQualityLevel(int minutes)
            {
                AllowLogDetail = false;
                int ans = 0;
                foreach (var bp in BluePrints)
                {
                    bp.AllowLogDetail = AllowLogDetail;
                    int bpResult = bp.FindBestResult(minutes);
                    LogDetail($"[BP {bp.Index}] Found best result {bpResult}");
                    ans += (bpResult * bp.Index);
                }
                return ans;
            }
            
            public int FindMultipliedBestResults(int maxBluePrints, int minutes)
            {
                AllowLogDetail = false;
                int ans = 1;
                for (int i = 0; i < maxBluePrints; i ++)
                {
                    var bp = BluePrints[i];
                    bp.AllowLogDetail = AllowLogDetail;
                    int bpResult = bp.FindBestResult(minutes);
                    LogDetail($"[BP {bp.Index}] Found best result {bpResult}");
                    ans *= bpResult;
                }
                return ans;
            }
        }

        public static void Start()
        {
            var textData = File.ReadAllText("data/input19.txt");
            var lines = textData.Split(Environment.NewLine);

            BluePrintSet bluePrintSet = BluePrintSet.Parse(lines);
            //Console.WriteLine(bluePrintSet);

            var ans1 = bluePrintSet.FindQualityLevel(24);
            var ans2 = bluePrintSet.FindMultipliedBestResults(3, 32);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


// p1. 1589 / elap 121305.235 / 85180.499 with sorted set / 9858.964 with clay assumed / 5438.587 with cached assumption
// p2. 29348 / elap 741763.008 / elap 43815.182 with clay assumed / 18050.621 with cached assumption