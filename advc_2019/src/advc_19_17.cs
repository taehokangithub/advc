using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2019
{
    using TileMap = MapByList<Problem17.Tile>;

    class Problem17 : Advc.Utils.Loggable
    {
        public enum Tile
        {
            Invalid = 0,
            Empty = (int)'.',
            Scaffold = (int)'#',
            Rup = (int)'^',
            Rright = (int)'>',
            Rleft = (int)'<',
            Rdown = (int) 'v',
            Newline = 10,
        }

        private record Step(Direction.Dir dir, int step);
        private string StepString(Step step) => $"{step.dir.ToString().First()}{step.step}";
      
        private TileMap m_tileMap = new();

        private void DrawMap(TileMap map)
        {
            for (int y = 0; y < map.Max.y; y ++)
            {
                StringBuilder sb = new();
                for (int x = 0; x < map.Max.x; x++)
                {   
                    var tile = map.GetAt(x, y);
                    sb.Append((char)tile);
                }
                Console.WriteLine(sb.ToString());
            }
        }

        private List<Point> GetIntersections(TileMap map)
        {
            List<Point> result = new();
            map.ForEach((tile, point) =>
            {
                if (tile == Tile.Scaffold)
                {
                    foreach (Point dirVector in Direction.DirVectors)
                    {
                        Point target = point.AddedPoint(dirVector);

                        if (map.CheckBoundary(target))
                        {
                            var targetTile = map.GetAt(target);
                            if (targetTile != Tile.Scaffold)
                            {
                                return;
                            }
                        }
                    }
                    LogDetail($"Found intersection {point}");
                    result.Add(point);
                }
            });
            return result;
        } 

        private (Point, Direction.Dir) GetRobot(TileMap map)
        {
            var ret = (Point.Dummy, Direction.Dir.Down);

            map.ForEach((tile, point) =>
            {
                switch (tile)
                {
                    case Tile.Rup : ret = (point, Direction.Dir.Up); return;
                    case Tile.Rdown : ret = (point, Direction.Dir.Down); return;
                    case Tile.Rleft : ret = (point, Direction.Dir.Left); return;
                    case Tile.Rright : ret = (point, Direction.Dir.Right); return;
                }
            });
            
            Debug.Assert(!ret.Item1.Equals(Point.Dummy));
            return ret;
        }

        private List<Step> GetPath(TileMap map)
        {
            List<Step> path = new();
            var (curLoc, curDir) = GetRobot(map);

            var canGoDir = (Point p, Direction.Dir d) =>
            {
                var target = p.MovedPoint(d);
                return map.CheckBoundary(target) && map.GetAt(target) == Tile.Scaffold;
            };

            var examineDirs = new List<Direction.Dir> {Direction.Dir.Left, Direction.Dir.Right};

            while (true)
            {
                LogDetail($"Checking {curLoc}, {curDir}");
                Debug.Assert(!canGoDir(curLoc, curDir));

                var turnDir = Direction.Dir.Up;
                var curDirVector = Direction.GetDirVector(curDir);

                bool hasFound = false;
                foreach (var edir in examineDirs)
                {
                    if (canGoDir(curLoc, Direction.Rotate(curDir, edir)))
                    {
                        turnDir = edir;
                        hasFound = true;
                        break;
                    }
                }

                if (!hasFound)
                {
                    LogDetail($"Find last point {curLoc}");
                    break;
                }

                curDir = Direction.Rotate(curDir, turnDir);
                int steps = 0;

                while (canGoDir(curLoc, curDir))
                {
                    curLoc.Move(curDir);
                    steps ++;
                }

                LogDetail($"Goind dir {curDir} (turned {turnDir}), {steps} steps");
                path.Add(new(turnDir, steps));
            }

            if (AllowLogDetail)
            {
                LogDetail(string.Join(",", path.Select(p => StepString(p))));
            }
            return path;
        }

        private TileMap GenerateMap(Computer09 com)
        {
            com.Run(0);
            LogDetail($"[Computer] output count = {com.OutputCount}");

            List<List<char>> lines = new();
            lines.Add(new());

            while (com.OutputCount > 0)
            {
                char c = (char)com.Output;

                if ((Tile)c == Tile.Newline)
                {
                    if (lines.Last().Count > 0)
                    {
                        lines.Add(new());
                    }
                    else 
                    {
                        lines.Remove(lines.Last()); // remove empty line
                    }
                }
                else
                {
                    lines.Last().Add(c);
                }
            }

            TileMap map = new();
            map.SetMax(lines.First().Count, lines.Count);

            foreach (var line in lines)
            {
                foreach (char c in line)
                {
                    Tile t = (Tile)c;
                    map.Add(t);
                }
            }

            map.CheckAddFinished(throwException: true);

            if (AllowLogDetail)
            {
                DrawMap(map);
            }
            return map;
        }

        public int Solve1(List<long> arr)
        {
            AllowLogDetail = false;
            Computer09 com = new(arr);
            m_tileMap = GenerateMap(com);
            var intersections = GetIntersections(m_tileMap);
            return intersections.Sum(p => p.x * p.y);
        }

        public int Solve2(List<long> arr)
        {
            AllowLogDetail = false;
            var path = GetPath(m_tileMap);
            var patterns = PatternUtil.FindPatterns<Step>(path);

            foreach (var pattern in patterns)
            {
                var patternStrs = pattern.Key.Select(k => $"{k.dir.ToString().First()},{k.step}");
                var totalStr = string.Join(",", patternStrs);

                if (totalStr.Length <= 20)
                {
                    LogDetail($"{totalStr} => {pattern.Value} times");
                }
            }

            /*
                No idea how to select 3 patterns to replace all individual steps
                So I did manually here 

                R12,L8,R12,R8,R6,R6,R8,R12,L8,R12,R8,R6,R6,R8,R8,L8,R8,R4,R4,R8,L8,R8,R4,R4,R8,R6,R6,R8,R8,L8,R8,R4,R4,R8,R6,R6,R8,R12,L8,R12

                B,A,B,A,C,C,A,C,A,B
                A R8,R6,R6,R8
                B R12,L8,R12
                C R8,L8,R8,R4,R4

                R,8,R,6,R,6,R,8
                R,12,L,8,R,12
                R,8,L,8,R,8,R,4,R,4
            */

            List<string> inputLines = new List<string> {
                "B,A,B,A,C,C,A,C,A,B",
                "R,8,R,6,R,6,R,8",
                "R,12,L,8,R,12",
                "R,8,L,8,R,8,R,4,R,4",
                "n"
            };

            Queue<long> inputQueue = new();

            foreach (var line in inputLines)
            {
                foreach (char c in line)
                {
                    inputQueue.Enqueue(c);
                }
                inputQueue.Enqueue((long)Tile.Newline);
            }

            arr[0] = 2; // Manipulate int program
            Computer09 com = new(arr);
            com.Run(inputQueue);

            LogDetail($"Computer output starts here ======");
            long output = 0;
            while (com.OutputCount > 0)
            {
                output = com.Output;

                if (AllowLogDetail)
                {
                    Console.Write((char)output);
                }
            }
            LogDetail($"Computer output finished ======");
            return (int)output;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input17.txt");
            var textArr = textData.Split(",");
            var intList = textArr.Select(t => long.Parse(t)).ToList();

            Problem17 prob1 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = prob1.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


