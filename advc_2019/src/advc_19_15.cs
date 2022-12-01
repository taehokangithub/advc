using Advc.Utils.MapUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem15 : Advc.Utils.LogUtils.Loggable
    {
        public enum Dir 
        {
            North = 1,
            South = 2,
            West = 3,
            East = 4, 
        }

        public enum Tile
        {
            Wall = 0,
            OkGo = 1,
            Oxygen = 2
        }

        public class TileMap : MapByDictionary<Tile>
        {
            private int ProcessTurnAndGetNonOxygenTiles()
            {
                int remainingCount = 0;
                Queue<Point> convertToOxygenQueue = new();

                ForEach((tile, point) =>
                {
                    if (tile == Tile.Oxygen)
                    {
                        foreach (var dirVector in Direction.DirVectors)
                        {
                            var target = point.AddedPoint(dirVector);

                            var targetTile = GetAt(target);

                            if (targetTile == Tile.OkGo)
                            {
                                convertToOxygenQueue.Enqueue(target);
                            }
                        }
                    }
                });

                foreach (var target in convertToOxygenQueue)
                {
                    SetAt(Tile.Oxygen, target);
                }

                ForEach ((tile, point) => 
                {
                    if (tile == Tile.OkGo)
                    {
                        remainingCount ++;
                    }
                });

                return remainingCount;
            }

            public int CountOxygenSpread(Advc.Utils.LogUtils.Loggable log)
            {
                int turn = 1;
                int remainingCount = 0;
                for ( ;; turn++)
                {
                    int newCount = ProcessTurnAndGetNonOxygenTiles();
                    if (newCount == remainingCount)
                    {
                        log.LogDetail($"Turn [{turn}] remaining [{remainingCount}] - no change break");
                        break;
                    }
                    remainingCount = newCount;
                    if (remainingCount == 0)
                    {
                        log.LogDetail($"Turn [{turn}] finished!");
                        break;
                    }
                }

                return turn;
            }

            public void Draw()
            {
                for (int y = ActualMin.y; y <= ActualMax.y; y ++)
                {
                    for (int x = ActualMin.x; x <= ActualMax.x; x ++)    
                    {
                        Tile t = GetAt(x, y);
                        char c = (t == Tile.Wall ? '#' : t == Tile.Oxygen ? 'O' : ' ');
                        Console.Write(c);
                    }
                    Console.WriteLine("");
                }
            }
        }

        class BFSState
        {
            public List<Dir> Path { get; set; } = new();
            public HashSet<Point> Visited = new();
            public Point Location;
            public Computer09? Computer { get; set; }
        }

        private static void MovePoint(ref Point point, Dir dir)
        {
            switch (dir)
            {
                case Dir.East : point.Add(Direction.Right); break;
                case Dir.West : point.Add(Direction.Left); break;
                case Dir.North : point.Add(Direction.Up); break;
                case Dir.South : point.Add(Direction.Down); break;
                default: throw new ArgumentException($"Unknown dir {dir}");
            }
        }

        public TileMap DrawMap(Computer09 com)
        {
            TileMap map = new();

            Queue<BFSState> stateQueue = new();

            var initialState = new BFSState{
                Computer = new(com)
            };

            initialState.Visited.Add(initialState.Location);
            stateQueue.Enqueue(initialState);

            while (stateQueue.Count > 0)
            {
                var state = stateQueue.Dequeue();

                foreach (var dir in Enum.GetValues(typeof(Dir)).Cast<Dir>())
                {
                    Point target = state.Location;
                    MovePoint(ref target, dir);

                    if (state.Visited.Contains(target))
                    {
                        continue;
                    }

                    Computer09 localComputer = new(state.Computer!);
                    localComputer.Run((long)dir);

                    Debug.Assert(localComputer.OutputCount == 1);
                    
                    var output = (Tile) localComputer.Output;
                    map.SetAt(output, target);

                    if (output != Tile.Wall)
                    {
                        BFSState newState = new BFSState{
                            Path = new(state.Path),
                            Location = target,
                            Visited = new(state.Visited),
                            Computer = new(localComputer)
                        };

                        newState.Path.Add(dir);
                        newState.Visited.Add(target);
                        stateQueue.Enqueue(newState);
                    }
                };
            }
            return map;
        }

        public int BFS(Computer09 com)
        {
            Queue<BFSState> stateQueue = new();

            var initialState = new BFSState{
                Computer = com
            };
            initialState.Visited.Add(initialState.Location);
            stateQueue.Enqueue(initialState);

            while (stateQueue.Count > 0)
            {
                var state = stateQueue.Dequeue();

                LogDetail($"Examining state at {state.Location}, visited {state.Visited.Count}, path {state.Path.Count}");

                foreach (var dir in Enum.GetValues(typeof(Dir)).Cast<Dir>())
                {
                    Point target = state.Location;
                    MovePoint(ref target, dir);

                    if (state.Visited.Contains(target))
                    {
                        continue;
                    }

                    // Run a local computer and get the result
                    Computer09 localComputer = new(state.Computer!);
                    localComputer.Run((long)dir);

                    Debug.Assert(localComputer.OutputCount == 1);
                    
                    var output = (Tile) localComputer.Output;

                    if (output == Tile.Wall)
                    {
                        LogDetail($"Can't go from {state.Location} to {dir}, hit the wall");
                        continue;
                    }

                    BFSState newState = new BFSState{
                        Path = new(state.Path),
                        Location = target,
                        Visited = new(state.Visited),
                        Computer = new(localComputer)
                    };

                    newState.Path.Add(dir);
                    newState.Visited.Add(target);

                    if (output == Tile.Oxygen)
                    {
                        LogDetail($"Going from {state.Location} to {dir}, found Oxygen at {target}");
                        return newState.Path.Count;
                    }

                    stateQueue.Enqueue(newState);
                    LogDetail($"Going from {state.Location} to {dir}, at {target}");
                };
            }
            return -1;
        }

        public long Solve1(List<long> arr)
        {
            AllowLogDetail = false;
            Computer09 com = new(arr);
            return BFS(com);
        }

        public long Solve2(List<long> arr)
        {
            AllowLogDetail = true;
            Computer09 com = new(arr);
            var map = DrawMap(com);
            map.Draw();
            
            return map.CountOxygenSpread(this);
        }

        public static void Start()
        {
            var textData = File.ReadAllText("data/input15.txt");
            var textArr = textData.Split(",");
            var intList = textArr.Select(t => long.Parse(t)).ToList();

            Problem15 prob1 = new();
            Problem15 prob2 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = prob2.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


