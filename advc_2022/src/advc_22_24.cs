using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    using Dir = Direction.Dir;

    class Problem24 : Loggable
    {
        enum Tile { Road, Me, Exit, Up, Down, Left, Right, Double, Tripple, Quadruple };
        record SnowFlake(Point loc, Dir dir);

        static Dir? CharToDir(char c)
        {
            return c == '^' ? Dir.Up 
                 : c == 'v' ? Dir.Down 
                 : c == '<' ? Dir.Left 
                 : c == '>' ? Dir.Right : null;
        }

        static char DirToChar(Dir d)
        {
            return d == Dir.Up ? '^' 
                 : d == Dir.Down ? 'v' 
                 : d == Dir.Left ? '<' 
                 : d == Dir.Right ? '>' : throw new ArgumentException($"Unknown dir {d}");
        }

        static char TileToChar(Tile t)
        {
            return t == Tile.Road ? '.'
                 : t == Tile.Me ? 'M'
                 : t == Tile.Exit ? 'X'
                 : t == Tile.Up ? DirToChar(Dir.Up)
                 : t == Tile.Down ? DirToChar(Dir.Down)
                 : t == Tile.Left ? DirToChar(Dir.Left)
                 : t == Tile.Right ? DirToChar(Dir.Right)
                 : t == Tile.Double ? '2'
                 : t == Tile.Tripple ? '3'
                 : t == Tile.Quadruple ? '4' : throw new ArgumentException($"Unknown tile {t}");
        }
        
        class State
        {
            public Point MyLoc { get; set; }
            public List<SnowFlake> SnowFlakes { get; set; } = new();
            private HashSet<Point> m_snowLocs = new();

            public State() {}
            public State(State other)
            {
                MyLoc = other.MyLoc;
                SnowFlakes = new(other.SnowFlakes);
            }

            public MapByDictionary<Tile> MakeMap()
            {
                MapByDictionary<Tile> map = new();
                map.SetAt(Tile.Me, MyLoc);
                SnowFlakes.ForEach(s => 
                {
                    Tile t = map.GetAt(s.loc);
                    if ((int)t >= (int)Tile.Double)
                    {
                        t = (Tile)((int) t + 1);
                    }
                    else 
                    {
                        t = (Tile)((int)s.dir + (int)Tile.Up);
                    }
                    map.SetAt(t, s.loc);
                });
                return map;
            }

            public void MoveSnowFlakes(BlizzardMap blizzardMap)
            {
                List<SnowFlake> newFlakes = new();
                m_snowLocs.Clear();

                SnowFlakes.ForEach(s =>
                {
                    Point nextLoc = s.loc.MovedPoint(s.dir);
                    if (nextLoc.x < blizzardMap.MinSnowGrid.x)
                    {
                        nextLoc.x = blizzardMap.MaxSnowGrid.x;
                    }
                    else if (nextLoc.x > blizzardMap.MaxSnowGrid.x)
                    {
                        nextLoc.x = blizzardMap.MinSnowGrid.x;
                    }
                    if (nextLoc.y < blizzardMap.MinSnowGrid.y)
                    {
                        nextLoc.y = blizzardMap.MaxSnowGrid.y;
                    }
                    if (nextLoc.y > blizzardMap.MaxSnowGrid.y)
                    {
                        nextLoc.y = blizzardMap.MinSnowGrid.y;
                    }
                    newFlakes.Add(new(nextLoc, s.dir));
                    Debug.Assert(nextLoc.IsInRange(blizzardMap.MinSnowGrid, blizzardMap.MaxSnowGrid));
                    m_snowLocs.Add(nextLoc);
                });
                SnowFlakes = newFlakes;
            }

            public List<Point> FindPossibleLocations(BlizzardMap blizzardMap)
            {
                List<Point> locs = new();
                foreach (Dir dir in Enum.GetValues<Dir>())
                {
                    Point loc = MyLoc.MovedPoint(dir);
                    if (loc.IsInRange(blizzardMap.MinSnowGrid, blizzardMap.MaxSnowGrid) || blizzardMap.EndLoc.Equals(loc))
                    {
                        if (!m_snowLocs.Contains(loc))
                        {
                            locs.Add(loc);
                        }
                    }
                }
                if (!m_snowLocs.Contains(MyLoc))
                {
                    locs.Add(MyLoc);
                }
                return locs;
            }
        }

        class SearchState
        {
            public State State { get; set; } = new();
            public int Steps = 0;
            public BlizzardMap BlizzardMap { get; init; }

            public SearchState(BlizzardMap blizzardMap)
            {
                BlizzardMap = blizzardMap;
            }

            public SearchState(SearchState other)
            {
                BlizzardMap = other.BlizzardMap;
                State = new(other.State);
                Steps = other.Steps;
            }

            public void Draw()
            {
                var map = State.MakeMap();
                map.SetAt(Tile.Exit, BlizzardMap.EndLoc);
                Console.WriteLine($"------ Step {Steps} ------");
                map.Draw(t => TileToChar(t));
                Console.WriteLine("");
            }

            public int AssumedDistance()
            {
                return (int) BlizzardMap.EndLoc.SubtractedPoint(State.MyLoc).ManhattanDistance();
            }

            public int AssumedSteps()
            {
                return AssumedDistance() + Steps;
            }

            public bool HasFinished()
            {
                return AssumedDistance() == 0;
            }

            public List<SearchState> FindNextStates()
            {
                State.MoveSnowFlakes(BlizzardMap);
                var nextLocs = State.FindPossibleLocations(BlizzardMap);

                List<SearchState> nextStates = new();

                foreach (var loc in nextLocs)
                {
                    SearchState newState = new(this);
                    newState.Steps ++;
                    newState.State.MyLoc = loc;
                    nextStates.Add(newState);
                }

                return nextStates;
            }
        }
       
        class BlizzardMap : Loggable
        {
            private SearchState InitialState { get; set; }
            private SearchState FinalState { get; set; }
            public Point EndLoc { get; set; }
            public Point StartLoc { get; set; }
            public Point MinSnowGrid { get; init; }
            public Point MaxSnowGrid { get; init; }

            #region parser
            public BlizzardMap(string[] lines)
            {
                int startXLoc = lines.First().ToList().FindIndex(c => c == '.');
                StartLoc = new(startXLoc, 0);

                int endXLoc = lines.Last().ToList().FindIndex(c => c == '.');
                EndLoc = new(endXLoc, lines.Count() - 1);

                InitialState = new(this);
                InitialState.State.MyLoc = StartLoc;
                FinalState = new(InitialState);

                MinSnowGrid = new(1, 1);
                MaxSnowGrid = new(lines.First().Count() - 2, lines.Count() - 2);

                for (int y = 1; y < lines.Count() - 1; y ++)
                {
                    var line = lines[y];
                    int x = 0;
                    foreach (char c in line)
                    {
                        Dir? dir = CharToDir(c);
                        if (dir != null)
                        {
                            Point loc = new(x, y);
                            SnowFlake snow = new(loc, (Dir)dir);
                            InitialState.State.SnowFlakes.Add(snow);
                        }
                        x ++;
                    }
                }
            }
            #endregion

            class StateContainer
            {
                private SortedSet<SearchState> m_queue = new SortedSet<SearchState>(Comparer<SearchState>.Create((a, b) =>
                {
                    int asDiff = a.AssumedDistance() - b.AssumedDistance();
                    if (asDiff != 0)
                    {
                        return asDiff;
                    }
                    int stepDiff = b.Steps - a.Steps;
                    if (stepDiff != 0)
                    {
                        return stepDiff;
                    }                    
                    return a.State.MyLoc.x - b.State.MyLoc.x;
                }));
                public void Push(SearchState s)
                {
                    m_queue.Add(s);
                }
                public SearchState Pop()
                {
                    var s = m_queue.First();
                    m_queue.Remove(s);
                    return s;
                }
                public SortedSet<SearchState> Q => m_queue;
            }

            record VisitedState(Point loc, int step);

            public int CountSteps()
            {
                AllowLogDetail = true;
                var stateQ = new StateContainer();
                stateQ.Push(InitialState);

                int minSteps = int.MaxValue;
                const int logFrequencey = 3000;
                int logCnt = 0;
                HashSet<VisitedState> visited = new();
                while (stateQ.Q.Any())
                {
                    bool canLog = AllowLogDetail && (logCnt++ % logFrequencey == 0);
                    var state = stateQ.Pop();
                    if (state.AssumedSteps() >= minSteps)
                    {
                        continue;
                    }

                    if (canLog)
                    {
                        LogDetail($"[{logCnt}] Examining {state.State.MyLoc} Step {state.Steps} Remaining {state.AssumedDistance()} => {state.AssumedSteps()} / {minSteps}, qLen {stateQ.Q.Count}");
                        //state.Draw();
                    }
                    
                    if (state.HasFinished())
                    {
                        LogDetail($"Found new answer at step {state.Steps}");
                        FinalState = new(state);
                        minSteps = state.Steps;
                    }

                    var newStates = state.FindNextStates();
                    //if (canLog) LogDetail($"   {newStates.Count} possible next locs");
                    foreach(var newState in newStates)
                    {
                        VisitedState v = new(newState.State.MyLoc, newState.Steps);
                        if (newState.AssumedSteps() >= minSteps)
                        {
                            //if (canLog) LogDetail($"    Discarding {v} {newState.State.MyLoc} Step {newState.Steps} total {newState.AssumedSteps()}");
                            continue;
                        }
                        
                        if (visited.Contains(v))
                        {
                            //if (canLog) LogDetail($"    discarding {v} duplicated");
                            continue;
                        }
                        visited.Add(v);

                        stateQ.Push(newState);
                        //if (canLog) LogDetail($"    Added {v} {newState.State.MyLoc} Step {newState.Steps} total {newState.AssumedSteps()}");
                    }
                }

                return minSteps;
            }

            public int CountStepsBackAndforth()
            {
                int first = CountSteps();
                
                InitialState = new(FinalState);
                InitialState.Steps = 0;
                Point orgEndLoc = EndLoc;
                EndLoc = StartLoc;

                int second = CountSteps();

                InitialState = new(FinalState);
                InitialState.Steps = 0;
                EndLoc = orgEndLoc;

                int third = CountSteps();
                return first + second + third;
            }
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input24.txt");
            var lines = textData.Split(Environment.NewLine);

            var ans1 = 0; //new BlizzardMap(lines).CountSteps();
            var ans2 = new BlizzardMap(lines).CountStepsBackAndforth();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


// 585 high. 507?