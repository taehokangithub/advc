using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    using Dir = Direction.Dir;

    class Problem23 : Loggable
    {
        class ElvesMap : Loggable
        {
            private record SuggestionInfo(int count, Point fromPoint);
            private MapByDictionary<bool> m_elves = new();
            public ElvesMap(string[] lines)
            {
                int y = 0;
                foreach (var line in lines)
                {
                    int x = 0;
                    foreach (char c in line)
                    {
                        if (c == '#')
                        {
                            m_elves.SetAt(true, x, y);
                        }
                        x ++;
                    }
                    y ++;
                }
            }

            public void Draw()
            {
                m_elves.Draw(v => v ? '#' : '.');
                LogDetail("");
            }

            private Point? MakeSuggestion(Point p, LinkedList<Dir> dirOrder)
            {
                bool hasNeighbour = false;
                for (int y = -1; y <= 1; y ++)
                {
                    for (int x = -1; x <= 1; x ++)
                    {
                        Point target = p.AddedPoint(new(x, y));
                        if (!p.Equals(target) && m_elves.Contains(target) && m_elves.GetAt(target))
                        {
                            hasNeighbour = true;
                            break;
                        }
                    }
                }

                if (!hasNeighbour)
                {
                    return null;
                }

                foreach (var dir in dirOrder)
                {
                    List<Point> searchVectors = new();

                    if (dir == Dir.Up)
                    {
                        searchVectors.Add(new(-1, -1));
                        searchVectors.Add(new(0,-1));
                        searchVectors.Add(new(1, -1));
                    }
                    else if (dir == Dir.Down)
                    {
                        searchVectors.Add(new(-1, 1));
                        searchVectors.Add(new(0, 1));
                        searchVectors.Add(new(1, 1));                        
                    }
                    else if (dir == Dir.Left)
                    {
                        searchVectors.Add(new(-1, -1));
                        searchVectors.Add(new(-1, 0));
                        searchVectors.Add(new(-1, 1));                        
                    }
                    else if (dir == Dir.Right)
                    {
                        searchVectors.Add(new(1, -1));
                        searchVectors.Add(new(1, 0));
                        searchVectors.Add(new(1, 1));                        
                    }

                    bool hasAny = false;

                    foreach (Point vector in searchVectors)
                    {
                        Point searchPoint = p.AddedPoint(vector);
                        if (m_elves.Contains(searchPoint) && m_elves.GetAt(searchPoint))
                        {
                            hasAny = true;
                            break;
                        }
                    }

                    if (!hasAny)
                    {
                        return p.MovedPoint(dir);
                    }
                }
                return null;
            }

            private MapByDictionary<SuggestionInfo> Phase1(LinkedList<Dir> dirOrder)
            {
                MapByDictionary<SuggestionInfo> suggestions = new();

                var addSuggestion = (Point target, Point from) => 
                {
                    int curSuggestions =  (suggestions.Contains(target)) ? suggestions.GetAt(target).count : 0;
                    SuggestionInfo suggestion = new(curSuggestions + 1, from);
                    LogDetail($"  Making suggestion at {target} : {suggestion}");
                    suggestions.SetAt(suggestion, target);
                };

                m_elves.ForEach((v, p) =>
                {
                    if (v)
                    {
                        Point? suggested = MakeSuggestion(p, dirOrder);

                        if (suggested != null)
                        {
                            addSuggestion((Point) suggested, p);
                        }
                    }
                });

                return suggestions;
            }

            private int Phase2(MapByDictionary<SuggestionInfo> suggestions)
            {
                int cnt = 0;
                suggestions.ForEach((v, p) =>
                {
                    if (v != null && v.count == 1)
                    {
                        m_elves.SetAt(true, p);
                        m_elves.SetAt(false, v.fromPoint);
                        cnt ++;
                        LogDetail($"  Adding point at {p}, removed from {v.fromPoint}");
                    }
                });
                return cnt;
            }

            private int DoRound(LinkedList<Dir> dirOrder)
            {
                var suggestions = Phase1(dirOrder);
                int movedCnt = Phase2(suggestions);
                if (AllowLogDetail)
                {
                    Draw();
                }
                Dir firstDir = dirOrder.First();
                dirOrder.RemoveFirst();
                dirOrder.AddLast(firstDir);
                return movedCnt;
            }

            public int GetStallRound()
            {
                LinkedList<Dir> dirOrder = new(new List<Dir>() {Dir.Up, Dir.Down, Dir.Left, Dir.Right});
                bool orgAllowLogDetaul = AllowLogDetail;
                int round = 1;
                while (true)
                {
                    AllowLogDetail = false;
                    int movedCnt = DoRound(dirOrder);

                    AllowLogDetail = orgAllowLogDetaul;
                    LogDetail($"Round {round} movedCnt {movedCnt} min {m_elves.ActualMin}, max {m_elves.ActualMax}");

                    if (movedCnt == 0)
                    {
                        break;
                    }

                    round ++;
                }
                return round;
            }

            public int GetNumEmptyGround(int nRounds)
            {
                LinkedList<Dir> dirOrder = new(new List<Dir>() {Dir.Up, Dir.Down, Dir.Left, Dir.Right});

                LogDetail($"----- Initial State -----");
                Draw();

                for (int i = 0; i < nRounds; i ++)
                {
                    LogDetail($"----- Round {i + 1} -----");
                    LogDetail($"{string.Join(",", dirOrder)}, min {m_elves.ActualMin} max {m_elves.ActualMax}");
                    DoRound(dirOrder);
                }

                var newMap = new MapByDictionary<bool>();

                m_elves.ForEach((v, p) => { if (v) newMap.SetAt(v, p); });

                int emptyCnt = 0;

                newMap.ForEach((v, p) => { if (!v) emptyCnt ++; });

                return emptyCnt;
            }
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input23.txt");
            var lines = textData.Split(Environment.NewLine);

            var map1 = new ElvesMap(lines);
            map1.AllowLogDetail = false;
            var ans1 = map1.GetNumEmptyGround(10);

            var map2 = new ElvesMap(lines);
            map2.AllowLogDetail = true;
            var ans2 = map2.GetStallRound();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


/// 1000 too low