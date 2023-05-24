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

        private const int NoReplacement = -1;
        private record Step(Direction.Dir dir, int step, int replacement);
        private string SteptoString(Step step) => $"{step.dir.ToString().First()},{step.step}";
        private string PatternToString(List<Step> pattern) => string.Join(",", pattern.Select(k => SteptoString(k)));

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
                path.Add(new(turnDir, steps, NoReplacement));
            }

            if (AllowLogDetail)
            {
                LogDetail(string.Join(",", path.Select(p => SteptoString(p))));
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

        private int m_fitIndex = 0;
        private bool FitInPatterns(List<Step> path, IReadOnlyList<List<Step>> possiblePatterns, HashSet<List<Step>> selectedPatterns, int maxPatterns)
        {
            var pathToString = (List<Step> p) => 
            {
                StringBuilder sb = new();
                foreach (var step in p)
                {
                    if (step.replacement == NoReplacement)
                    {
                        sb.Append($"{SteptoString(step)},");
                    }
                    else
                    {
                        sb.Append($"[REPLACE {step.replacement}],");
                    }
                }
                return sb.ToString();
            };

            int fitIndex = m_fitIndex ++;
            //LogDetail($"<Fit {fitIndex}> start - {selectedPatterns.Count} selected, path : {pathToString(path)}");
            bool allReplaced = true;
            foreach (var step in path)
            {
                if (step.replacement == NoReplacement)
                {
                    allReplaced = false;
                    break;
                }
            }

            if (allReplaced)
            {
                LogDetail($"All replaced!! {pathToString(path)}");
                return true;
            }

            if (selectedPatterns.Count >= maxPatterns)
            {
                //LogDetail($"<Fit {fitIndex}> returning false for reaching max - {path.Count} paths, {selectedPatterns.Count} selected");
                return false;
            }

            foreach (var pattern in possiblePatterns)
            {
                List<Step> localPatternPath = new();

                int occurence = 0;
                for (int startIndex = 0; startIndex < path.Count; startIndex ++)
                {
                    bool hasMatched = false;

                    if (startIndex <= path.Count - pattern.Count)
                    {
                        hasMatched = true;
                        for (int i = 0; i < pattern.Count; i ++)
                        {
                            if (path[startIndex + i] != pattern[i])
                            {
                                hasMatched = false;
                                break;
                            }
                        }

                        if (hasMatched)
                        {
                            occurence ++;
                            localPatternPath.Add(new Step(Direction.Dir.Up, 0, selectedPatterns.Count));
                            startIndex += pattern.Count - 1;
                        }
                    }

                    if (!hasMatched)
                    {
                        localPatternPath.Add(path[startIndex]);
                    }
                }

                if (occurence > 0)
                {
                    HashSet<List<Step>> localSelectedPatterns = new(selectedPatterns);
                    localSelectedPatterns.Add(pattern);

                    //LogDetail($"<Fit {fitIndex}> Replaced Pattern : [{PatternToString(pattern)}] {occurence} occurrences, so far {localSelectedPatterns.Count} selected, going deeper.");

                    if (FitInPatterns(localPatternPath, possiblePatterns, localSelectedPatterns, maxPatterns))
                    {
                        foreach (var p in localSelectedPatterns)
                        {
                            selectedPatterns.Add(p);
                        }
                        LogDetail($"<Fit {fitIndex}> returning true , path : {pathToString(localPatternPath)}");
                        path.Clear();
                        path.AddRange(localPatternPath);
                        return true;
                    }
                }
            }
            //LogDetail($"<Fit {fitIndex}> returning false - {selectedPatterns.Count} selected {path.Count} paths");
            return false;
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
            const int maxPatterns = 3;
            
            var path = GetPath(m_tileMap);
            Dictionary<List<Step>, int> allPatternsDic = PatternUtil.FindPatterns<Step>(path);
            var possiblePatterns = new List<List<Step>>();

            foreach (var patternPair in allPatternsDic)
            {
                var patternStr = PatternToString(patternPair.Key);

                if (patternStr.Length <= 20)
                {
                    LogDetail($"Possible pattern {patternStr} => {patternPair.Value} times");
                    possiblePatterns.Add(patternPair.Key);
                }
            }

            HashSet<List<Step>> selectedPatterns = new();
            
            var result = FitInPatterns(path, possiblePatterns, selectedPatterns, maxPatterns);
            Debug.Assert(result);
            List<string> inputLines = new();

            // add main program
            inputLines.Add(string.Join(",", path.Select(p => (char)(p.replacement + 'A'))));

            foreach (var pattern in selectedPatterns)
            {
                inputLines.Add(PatternToString(pattern));
            }

            inputLines.Add("n"); // no to visual tracking

            Queue<long> inputQueue = new();

            foreach (var line in inputLines)
            {
                LogDetail($"[Input Line] {line}");
                foreach (char c in line)
                {
                    inputQueue.Enqueue(c);
                }
                inputQueue.Enqueue((long)Tile.Newline);
            }

            arr[0] = 2; // Manipulate int program
            Computer09 com = new(arr);
            com.Run(inputQueue);

            long output = 0;
            while (com.OutputCount > 0)
            {
                output = com.Output;

                if (AllowLogDetail)
                {
                    Console.Write((char)output);
                }
            }

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


