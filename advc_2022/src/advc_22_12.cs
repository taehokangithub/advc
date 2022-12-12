using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem12 : Loggable
    {
        public record Cell(char height, bool IsDestination);

        public class HeightMap : MapByList<Cell>
        {
            public Point StartPoint { get; init; }
            public Point EndPoint { get; init; }
            public HeightMap(string[] lines)
            {
                SetMax(lines.First().Count(), lines.Count());

                foreach (var line in lines)
                {
                    foreach (var c in line)
                    {
                        bool IsDestination = false;
                        char height = c;
                        if (height == 'S')
                        {
                            StartPoint = m_addPointer;
                            height = 'a';
                        }
                        else if (height == 'E')
                        {
                            height = 'z';
                            IsDestination = true;
                            EndPoint = m_addPointer;
                        }
                        Add(new(height, IsDestination));
                    }
                }
            }

            public string CellToString(Cell cell)
            {
                char c = (cell.IsDestination) ? 'E' : cell.height;
                return $"{c}";
            }

            public void Draw()
            {
                ForEach((cell, point) => {
                    if (point.Equals(StartPoint))
                    {
                        Console.Write('S');
                    }
                    else
                    {
                        Console.Write(CellToString(cell));
                    }
                    if (point.x == Max.x - 1)
                    {
                        Console.WriteLine("");
                    }
                });
            }
        
            public record VisitState(Point curLoc, HashSet<Point> visited, List<Point> path);

            public string StateToString(VisitState state, bool showFullPath = true)
            {
                string pathString = showFullPath ? string.Join(",", state.path) : "";
                return $"[{state.curLoc}/{state.path.Count}:{pathString}]";
            }

            public enum FindCondition { Endpoint, HeightA }

            public int FindDestination(Loggable logger, Point startPoint, FindCondition condition)
            {
                var searchVectors = new List<Point> {Direction.Right, Direction.Down, Direction.Up, Direction.Left};
                Queue<VisitState> toVisit = new();
                Dictionary<Point, int> shortestToPoint = new();

                toVisit.Enqueue(new(startPoint, new(), new()));
                VisitState ansState = toVisit.First();

                while (toVisit.Any())
                {
                    VisitState state = toVisit.Dequeue();
                    state.visited.Add(state.curLoc);
                    state.path.Add(state.curLoc);

                    Cell curCell = GetAt(state.curLoc);
                    if (condition == FindCondition.Endpoint && curCell.IsDestination
                        || condition == FindCondition.HeightA && curCell.height == 'a')
                    {
                        if (ansState.path.Count == 1 || state.path.Count < ansState.path.Count)
                        {
                            ansState = state;
                            logger.LogDetail($"Found candidate {StateToString(state, showFullPath: true)} Queue {toVisit.Count}");
                            continue;
                        }
                    }

                    foreach (var dirPoint in searchVectors)
                    {
                        var targetPoint = state.curLoc.AddedPoint(dirPoint);
                        if (CheckBoundary(targetPoint) && !state.visited.Contains(targetPoint))
                        {
                            Cell targetCell = GetAt(targetPoint);
                            if (condition == FindCondition.Endpoint && targetCell.height <= (curCell.height + 1)
                                || condition == FindCondition.HeightA && targetCell.height + 1 >= curCell.height)
                            {
                                int steps = state.path.Count + 1;

                                if (shortestToPoint.TryGetValue(targetPoint, out var prevSteps))
                                {
                                    if (prevSteps <= steps)
                                    {
                                        continue;
                                    }
                                }

                                shortestToPoint[targetPoint] = steps;

                                VisitState newState = new(targetPoint, new(state.visited), new(state.path));
                                toVisit.Enqueue(newState);
                            }
                        }
                    }
                }

                return ansState.path.Count - 1;
            }
        }

        public int Solve1(HeightMap map)
        {
            AllowLogDetail = false;

            return map.FindDestination(this, map.StartPoint, HeightMap.FindCondition.Endpoint);
        }

        public int Solve2(HeightMap map)
        {
            AllowLogDetail = false;

            return map.FindDestination(this,  map.EndPoint, HeightMap.FindCondition.HeightA);
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input12.txt");
            var lines = textData.Split(Environment.NewLine);
            var map = new HeightMap(lines);
            
            Problem12 prob = new();

            var ans1 = prob.Solve1(map);
            var ans2 = prob.Solve2(map);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


