using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem14 : Loggable
    {
        public enum Tile 
        {
            Rock,
            Sand,
            StartPoint
        }
        public class CaveMap : Loggable
        {
            private readonly Point m_start = new Point(500, 0);
            private MapByDictionary<Tile> m_map;

            public CaveMap()
            {
                m_map = new();
            }

            public void Parse(string[] lines)
            {
                foreach (var line in lines)
                {
                    List<Point> points = new();
                    foreach (var pointStr in line.Split(" -> "))
                    {
                        var positions = pointStr.Split(",").Select(s => int.Parse(s));
                        Point point = new(positions.First(), positions.Last());

                        points.Add(point);
                    }

                    Point curLoc = points.First();
                    m_map.SetAt(Tile.Rock, curLoc);
                    
                    foreach (var point in points)
                    {
                        while (!curLoc.Equals(point))
                        {
                            var diff = point.SubtractedPoint(curLoc).GetBasePoint();
                            curLoc.Add(diff);

                            m_map.SetAt(Tile.Rock, curLoc);
                        }
                    }
                }
                m_map.SetAt(Tile.StartPoint, m_start);
            }

            public void Draw()
            {
                LogDetail($"Min {m_map.ActualMin} Max {m_map.ActualMax}");

                m_map.ForEach((t, point) =>
                {
                    char c = '.';
                    if (m_map.Contains(point))
                    {
                        var tile = m_map.GetAt(point);
                        c = (tile == Tile.Rock) ? '#' 
                            : (tile == Tile.Sand) ? 'O'
                            : (tile == Tile.StartPoint) ? '+'
                            : throw new InvalidDataException($"Unknown tile {tile}");
                    }
                    
                    Console.Write(c);
                    if (point.x == m_map.ActualMax.x)
                    {
                        Console.WriteLine("");
                    }
                });
            }

            public int ThrowSands(bool hasFloor)
            {
                int floorLine = m_map.ActualMax.y + 2;;

                if (!hasFloor)
                {
                    m_map.SetBoundaryByActualMinMax();
                }

                int cnt = 0;
                while(ThrowOneSand(hasFloor, floorLine))
                {
                    cnt ++;
                    if (AllowLogDetail)
                    {
                        Draw();
                    }
                }
                return cnt;
            }

            private bool ThrowOneSand(bool hasFloor, int floorLine)
            {
                Point curLoc = m_start;
                List<Point> checkPoints = new List<Point> {new Point(0, 1), new Point(-1, 1), new Point(1, 1)};

                bool hasMovedThisTurn = true;

                while (hasMovedThisTurn)
                {
                    hasMovedThisTurn = false;
                    foreach (var p in checkPoints)
                    {
                        Point targetPoint = curLoc.AddedPoint(p);

                        if (hasFloor)
                        {
                            if (targetPoint.y == floorLine)
                            {
                                continue;
                            }
                        }
                        else 
                        {
                            if (!m_map.CheckBoundary(targetPoint))
                            {
                                LogDetail($"Found void at {targetPoint}");
                                return false;
                            }
                        }

                        if (!m_map.Contains(targetPoint))
                        {
                            hasMovedThisTurn = true;
                            curLoc = targetPoint;
                            break;
                        }
                    }
                }

                if (!m_map.Contains(curLoc) || m_map.GetAt(curLoc) == Tile.StartPoint)
                {
                    LogDetail($"Set sand at {curLoc}");
                    m_map.SetAt(Tile.Sand, curLoc);
                    return true;
                }
                
                return false;
            }
        }
                
        public static void Start()
        {
            var textData = File.ReadAllText("data/input14.txt");
            var lines = textData.Split(Environment.NewLine);

            CaveMap map1 = new();
            map1.Parse(lines);
            var ans1 = map1.ThrowSands(hasFloor: false);

            CaveMap map2 = new();
            map2.Parse(lines);
            var ans2 = map2.ThrowSands(hasFloor: true);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


