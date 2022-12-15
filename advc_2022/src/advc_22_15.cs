using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem15 : Loggable
    {
        public enum Tile
        {
            Empty,
            Sensor,
            Beacon,
            Range
        }

        public class Sensor
        {
            private static int UniqueId = 0;
            public int Id { get; set; }
            public Point Location { get; set; }
            public Point ClosestBeacon { get; set; }
            public int Distance { get; set; }
            public Sensor() 
            {
                Id = ++UniqueId;
            }

            public bool IsInRange(Point p)
            {
                return p.SubtractedPoint(Location).ManhattanDistance() <= Distance;
            }
            public override string ToString()
            {
                return $"[[{Id}]{Location},D{Distance}]";
            }
        }

        public class BeaconMap : Loggable
        {
            public List<Sensor> Sensors = new();
            public MapByDictionary<Tile> Map = new();
            public BeaconMap(string[] lines)
            {
                Parse(lines);
            }

            public void Parse(string[] lines)
            {
                var checkAndGetRest = (string str, string checkPart) =>
                {
                    Debug.Assert(str.StartsWith(checkPart));
                    return str.Substring(checkPart.Length);
                };

                var parsePoint = (string str) =>
                {
                    var coords = str.Split(", ");
                    var x = checkAndGetRest(coords.First(), "x=");
                    var y = checkAndGetRest(coords.Last(), "y=");

                    return new Point(int.Parse(x), int.Parse(y));
                };

                Point min = new(int.MaxValue, int.MaxValue);
                Point max = new(int.MinValue, int.MinValue);

                foreach (var line in lines)
                {
                    var parts = line.Split(": ");
                    var sensorCoord = checkAndGetRest(parts.First(), "Sensor at ");
                    var sensorLoc = parsePoint(sensorCoord);
                    var beaconCoord = checkAndGetRest(parts.Last(), "closest beacon is at ");
                    var beaconLoc = parsePoint(beaconCoord);
                    var distance = (int) sensorLoc.SubtractedPoint(beaconLoc).ManhattanDistance();

                    Sensors.Add(new Sensor{
                        Location = sensorLoc,
                        ClosestBeacon = beaconLoc,
                        Distance = distance
                    });
                    
                    Map.SetAt(Tile.Sensor, sensorLoc);
                    Map.SetAt(Tile.Beacon, beaconLoc);

                    min.SetMin(new Point(sensorLoc.x - distance, sensorLoc.y - distance));
                    max.SetMax(new Point(sensorLoc.x + distance, sensorLoc.y + distance));
                }

                Map.SetMax(max);
                Map.SetMin(min);
            }

            public void Draw()
            {
                Map.ForEach((tile, point) =>
                {
                    char c = tile == Tile.Sensor ? 'S' 
                            : tile == Tile.Beacon ? 'B' 
                            : tile == Tile.Range ? '#' 
                            : tile == Tile.Empty ? '.'
                            : throw new InvalidDataException($"Unknown tile {tile}");

                    Console.Write(c);
                    if (point.x == Map.ActualMax.x)
                    {
                        Console.WriteLine("");
                    }
                });
            }

            public void SetBeaconRanges() // This is only for small space, brutal way
            {
                foreach (var sensor in Sensors)
                {
                    int dist = sensor.Distance;
                    int ystart = sensor.Location.y - dist;
                    int yend = sensor.Location.y + dist;

                    LogDetail($"Checking sensor {sensor}, beacon {sensor.ClosestBeacon} distance {dist} from {ystart} to {yend}");

                    int checker = 0;
                    for (int y = ystart; y <= yend; y ++)
                    {
                        int xdiff = y < sensor.Location.y ? (y - ystart) : 
                                    y > sensor.Location.y ? (yend -y) : dist;
                        for (int x = sensor.Location.x - xdiff; x <= sensor.Location.x + xdiff; x ++)
                        {
                            Point p = new(x,  y);

                            if (AllowLogDetail && (++checker) % 10000 == 0)
                            {
                                LogDetail($"    [{checker}] checking {p} between {ystart} and {yend}, xdiff {xdiff}");
                            }
                            if (!Map.Contains(p))
                            {
                                Map.SetAt(Tile.Range, p);
                            }
                        }
                    }
                }
            }

            private void PlaceCandidates(Dictionary<Point, int> candidates, Sensor sensor, int minCoordinate, int maxCoordinate)
            {
                int dist = sensor.Distance + 1;
                int ystart = sensor.Location.y - dist;
                int yend = sensor.Location.y + dist;
                int cnt = 0;

                var addToCandidate = (Point p) =>
                {
                    if (p.x < minCoordinate || p.y < minCoordinate || p.x > maxCoordinate || p.y > maxCoordinate)
                    {
                        return;
                    }
                    foreach (var otherSensor in Sensors)
                    {
                        if (otherSensor != sensor)
                        {
                            if (otherSensor.IsInRange(p))
                            {
                                return;
                            }
                        }
                    }

                    if (!candidates.ContainsKey(p))
                    {
                        candidates[p] = 0;
                    }
                    candidates[p] ++;
                    {
                        LogDetail($"Adding {cnt}th candidate {p} : {candidates[p]} (total {candidates.Count}) for sensor {sensor}");
                    }
                };

                for (int y = ystart; y <= yend; y ++)
                {
                    int xdiff = y < sensor.Location.y ? (y - ystart) : 
                                y > sensor.Location.y ? (yend -y) : dist;
                    
                    if (xdiff == 0)
                    {
                        addToCandidate(new(sensor.Location.x, y));
                    }
                    else
                    {
                        addToCandidate(new(sensor.Location.x - xdiff, y));
                        addToCandidate(new(sensor.Location.x + xdiff, y));
                    }
                }

                LogDetail($"Placed candidates for {sensor.Location}, cnt {candidates.Count}");
            }

            public Point FindMostVotedCandidate(int minCoordinate, int maxCoordinate)
            {
                Dictionary<Point, int> candidates = new();

                foreach (var sensor in Sensors)
                {
                    PlaceCandidates(candidates, sensor, minCoordinate, maxCoordinate);

                    if (candidates.Count == 1)
                    {
                        // then this is just the answer
                        return candidates.First().Key;
                    }
                }

                var orderedCandidates = candidates.OrderByDescending(c => c.Value);

                foreach (var pair in orderedCandidates.Take(10).ToList())
                {
                    LogDetail($"Candidate {pair.Key} : {pair.Value} times");
                }

                return orderedCandidates.First().Key;
            }

            public int GetCountRanges(int y)
            {
                int cnt = 0;
                for (int x = Map.Min.x; x <= Map.Max.x; x ++)
                {
                    Point p = new(x, y);
                    Tile t = Map.GetAt(p);

                    if (t != Tile.Beacon)
                    {
                        foreach (var b in Sensors)
                        {
                            var distance = p.SubtractedPoint(b.Location).ManhattanDistance();
                            if (b.IsInRange(p))
                            {
                                cnt ++;

                                if (AllowLogDetail && cnt % 100000 == 0)
                                {
                                    LogDetail($"checking point {p} against {b.Location} distance {distance} range {b.Distance} => {b.IsInRange(p)} : cnt {cnt}");
                                }

                                break;
                            }
                        }
                    }
                }
                return cnt;
            }

        }

        public int Solve1(BeaconMap map)
        {
            map.AllowLogDetail = AllowLogDetail = false;

            LogDetail($"Map actual from {map.Map.ActualMin} to {map.Map.ActualMax}");
            LogDetail($"Map possible from {map.Map.Min} to {map.Map.Max}");
            return map.GetCountRanges(2000000);
        }

        public long Solve2(BeaconMap map)
        {
            map.AllowLogDetail = AllowLogDetail = false;
            var p = map.FindMostVotedCandidate(0, 4000000);
            return (long)p.x * (long)4000000 + p.y;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input15.txt");
            var textArr = textData.Split(Environment.NewLine);

            BeaconMap map = new(textArr);

            Problem15 prob1 = new();

            var ans1 = prob1.Solve1(map);
            var ans2 = prob1.Solve2(map);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


// 2655411:3166538 = 10621647166538