using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem18 : Advc.Utils.Loggable
    {
        enum Tile {
            Empty = '.',

            Wall = '#',
            Entrance = '@',
        }

        class Map : MapByList<Tile>
        {
            public HashSet<Tile> Keys { get; } = new();
            public Point Entrance { get; set; }

            public Map(List<string> textArray, Loggable logger)
            {
                SetMax(textArray.First().Length, textArray.Count);

                foreach (var line in textArray)
                {
                    foreach (char c in line)
                    {
                        Tile t = (Tile) c;
                        var curLoc = Add(t);
                        if (t == Tile.Entrance)
                        {
                            Entrance = curLoc;
                        }
                        else if (t != Tile.Empty && t != Tile.Wall && Char.IsLower(c))
                        {
                            Keys.Add(t);
                        }
                    }
                }

                logger.LogDetail($"{Keys.Count} Keys : {string.Join(",", Keys.Select(k => (char)k))}");
            }

            public void Draw()
            {
                ForEach((tile, loc) =>
                {
                    Console.Write((char)tile);
                    if (loc.x == Max.x - 1)
                    {
                        Console.WriteLine("");
                    }
                });
            }
        }
        class State
        {
            public HashSet<Tile> UnlockedKeys { get; } = new();
            public Point CurLoc { get; set; }
            public int Steps { get; set; }

            public State(Map map)
            {
                CurLoc = map.Entrance;
            }

            public State() {}
            public State(State other)
            {
                UnlockedKeys = new(other.UnlockedKeys);
                CurLoc = other.CurLoc;
                Steps = other.Steps;
            }

            public override string ToString()
            {
                return $"[{CurLoc}/{Steps}] <UL {UnlockedKeys.Count} : {UnlockedStateString()}>";
            }

            public string UnlockedStateString()
            {
                var sortedKeys = UnlockedKeys.Select(t => (char)t).OrderBy(s => s);
                return string.Join(",", sortedKeys);
            }

            public string StateWithoutStepString()
            {
                return $"[{CurLoc}] <Unlocked {UnlockedStateString()}>";
            }

            public bool IsTerminal(Map map)
            {
                return UnlockedKeys.Count == map.Keys.Count;
            }
        }

        private List<State> FindNextPossibleStates(Map map, State incomingState, State currentCandidate, IReadOnlyDictionary<string, int> shortestToState)
        {
            bool logState = AllowLogDetail;
            AllowLogDetail = false;
            HashSet<Point> visited = new();
            Stack<State> searchQ = new();
            List<State> result = new();

            searchQ.Push(new(incomingState));
            visited.Add(incomingState.CurLoc);

            while (searchQ.Count > 0)
            {
                var state = searchQ.Pop();
                if (state.Steps >= currentCandidate.Steps)
                {
                    continue;
                }

                string StateWithoutStepString = state.StateWithoutStepString();
                if (shortestToState.ContainsKey(StateWithoutStepString) 
                    && shortestToState[StateWithoutStepString] < state.Steps)
                {
                    LogDetail($"discarding {state} {shortestToState[StateWithoutStepString]} <= {state.Steps}");
                    continue;
                }
                LogDetail($"[FindingPath] begin state {state}, QLen {searchQ.Count}");

                foreach (var dir in Enum.GetValues(typeof(Direction.Dir)).Cast<Direction.Dir>())
                {
                    Point searchPoint = state.CurLoc.MovedPoint(dir);
                    if (!visited.Contains(searchPoint))
                    {
                        visited.Add(searchPoint);
                        Tile t = map.GetAt(searchPoint);
                        char c = (char)t;

                        LogDetail($"[FindingPath] {state} searching at {searchPoint}, tile {c}");
                        if (t == Tile.Wall)
                        {
                            continue;
                        }

                        if (t != Tile.Empty && t != Tile.Entrance)
                        {
                            Debug.Assert(Char.IsLetter(c));
                            if (Char.IsLower(c)) // key
                            {
                                if (!state.UnlockedKeys.Contains(t))
                                {
                                    State resultState = new(state);
                                    resultState.CurLoc = searchPoint;
                                    resultState.Steps ++;
                                    resultState.UnlockedKeys.Add(t);                     
                                    result.Add(resultState);  // found a new key
                                    continue;
                                }
                            }
                            else // door
                            {
                                var key = (Tile)Char.ToLower(c);
                                if (!state.UnlockedKeys.Contains(key))
                                {
                                    continue;
                                }
                            }
                        }
                        State newState = new(state);
                        newState.CurLoc = searchPoint;
                        newState.Steps ++;
                        searchQ.Push(newState);
                    }
                }
            }
            AllowLogDetail = logState;
            return result;
        }

        private State FindTerminalState(Map map)
        {
            Queue<State> searchQ = new();
            Dictionary<string, int> shortedToState = new();
            searchQ.Enqueue(new(map));

            State candidate = new State{ Steps = int.MaxValue };

            while (searchQ.Count > 0)
            {
                var state = searchQ.Dequeue();

                var nextStates = FindNextPossibleStates(map, state, candidate, shortedToState);

                foreach (var nextState in nextStates) 
                {
                    if (nextState.IsTerminal(map))
                    {
                        LogDetail($"Found terminal state {nextState}");
                        candidate = nextState;
                    }

                    string stateWithoutSteps = nextState.StateWithoutStepString();
    #if true
                    if (shortedToState.ContainsKey(stateWithoutSteps))
                    {
                        if (shortedToState[stateWithoutSteps] <= nextState.Steps)
                        {
                            continue;
                        }
                    }
    #endif
                    shortedToState[stateWithoutSteps] = nextState.Steps;
                    searchQ.Enqueue(nextState);

                    if (nextState.Steps % 100 == 0)
                    {
                        LogDetail($"Adding new state : {nextState}, Qlen {searchQ.Count}");
                    }
                    
                };
            }

            return candidate;
        }

        public int Solve1(List<string> textArr)
        {
            AllowLogDetail = true;
            Map map = new(textArr, this);
            var state = FindTerminalState(map);

            return state.Steps;
        }

        public int Solve2(List<int> arr)
        {
            return 0;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input18.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();
            
            Problem18 prob1 = new();

            var ans1 = prob1.Solve1(textArr);
            var ans2 = 0;

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


