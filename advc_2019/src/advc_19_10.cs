using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Problem10 : Advc.Utils.Loggable
    {
        private MapByList<bool> m_map = new();
        private HashSet<Point> m_stars = new();

        public Problem10(string data)
        {
            var lines = data.Split(Environment.NewLine);

            m_map.SetMax(lines.First().Length, lines.Count());

            LogDetail($"Setting max {m_map.Max}");

            foreach (var line in lines)
            {
                foreach (char c in line)
                {
                    bool isStar = (c == '#');
                    m_map.Add(isStar);
                }
            }

            m_map.ForEach((val, p) => {
                if (val)
                {
                    m_stars.Add(p);
                }
            });
        }

        public void Print()
        {
            m_map.ForEach((val, p) => 
            {
                if (p.x == 0)
                {
                    Console.WriteLine("");
                }
                Console.Write(val ? '#' : '.');
            });
        }

        private HashSet<Point> GetVisibleStarsFrom(Point startPoint, HashSet<Point> targetStars)
        {
            HashSet<Point> visibleStars = new();
            HashSet<Point> remainingStars = new (targetStars);

            remainingStars.Remove(startPoint);

            foreach (Point targetPoint in m_stars)
            {
                if (remainingStars.Contains(targetPoint))
                {
                    Point diff = targetPoint.SubtractedPoint(startPoint);
                    int gcd = Math.Abs(GenericMath.GetGCD(diff.x, diff.y));
                    diff.Divide(gcd);

                    Point searchPoint = new(startPoint);
                    bool hasViewedFirstStar = false;
                    do
                    {
                        searchPoint.Add(diff);
                        if (remainingStars.Contains(searchPoint))
                        {
                            if (!hasViewedFirstStar)
                            {
                                hasViewedFirstStar = true;
                                visibleStars.Add(searchPoint);
                                //LogDetail($"From {startPoint} {searchPoint} is the first sight by diff {diff}. Adding as {visibleStars.Count}th visible star. Remaining {remainingStars.Count} stars");
                            }
                            else
                            {
                                //LogDetail($"From {startPoint} discarding {searchPoint} by diff {diff}. remainingStars has {remainingStars.Count} stars");
                            }
                            remainingStars.Remove(searchPoint);
                        }
                    }
                    while (m_map.CheckBoundary(searchPoint));
                }
            }

            return visibleStars;
        }

        private int CountVisibleStarsFrom(Point startPoint)
        {
            HashSet<Point> visibleStars = GetVisibleStarsFrom(startPoint, m_stars);
            return visibleStars.Count;
        }

        private List<Point> GetSortedVisibleStarsByDegree(Point startPoint, HashSet<Point> targetStars)
        {
            var set = GetVisibleStarsFrom(startPoint, targetStars);

            return set.ToList().OrderBy(s => Angles.ToDegree(s.SubtractedPoint(startPoint), 90f)).ToList();
        }

        public Point StationLocation { get; private set; }
        public int Solve1()
        {
            AllowLogDetail = false;
            Point ansPoint = new();
            int maxStars = 0;

            foreach (var star in m_stars)
            {
                int stars = CountVisibleStarsFrom(star);
                if (maxStars < stars)
                {
                    maxStars = stars;
                    ansPoint = star;
                }
            };

            LogDetail($"Best point {ansPoint} with visible stars {maxStars}");
            StationLocation = ansPoint;
            return maxStars;
        }

        public int Solve2()
        {
            AllowLogDetail = false;
            int toVaporise = 200;
            HashSet<Point> remainingStars = new(m_stars);

            LogDetail($"[Solve2] started - target stars {m_stars.Count}, station at {StationLocation}");

            while(toVaporise >= 0)
            {
                var sortedStars = GetSortedVisibleStarsByDegree(StationLocation, remainingStars);
                LogDetail($"[Batch] found {sortedStars.Count} stars vaporised (to vaporise {toVaporise}) remaining stars {remainingStars.Count}");

                if (AllowLogDetail)
                {
                    Console.Write("Sorted stars =>");
                    foreach (var point in sortedStars)
                    {
                        Console.Write($"{point}");
                    }
                    Console.WriteLine("");
                }

                if (sortedStars.Count >= toVaporise)
                {
                    var star = sortedStars[toVaporise - 1];
                    LogDetail($"Found answer => {star}");
                    return star.x * 100 + star.y;
                }

                foreach (var removeStar in sortedStars)
                {
                    remainingStars.Remove(removeStar);
                }

                toVaporise -= sortedStars.Count;
                LogDetail($"not found in this batch, toVaporise {toVaporise} remaiing stars {remainingStars.Count}");
            }

            throw new Exception("Unreachable code");
       }

        public static void Start()
        {
            var textData = File.ReadAllText("data/input10.txt");

            Problem10 prob = new(textData);

            var ans1 = prob.Solve1();
            var ans2 = prob.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


