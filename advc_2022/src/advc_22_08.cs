using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    using Dir = Direction.Dir;

    class Problem08 : Advc.Utils.Loggable
    {
        class Map : MapByList<int>
        {
            public Map(List<string> lines)
            {
                SetMax(lines.First().Length, lines.Count);
                foreach (string line in lines)
                {
                    foreach (char c in line)
                    {
                        Add(GenericUtil.CharToInt(c));
                    }
                }
            }

            public void Draw()
            {
                ForEach((val, p) =>
                {
                    Console.Write(val);
                    if (p.x == Max.x - 1)
                    {
                        Console.WriteLine("");
                    }
                });
            }
        }
        
        private Map m_map;
        private HashSet<Point> m_alreadySeen = new();

        private int CheckScene(Point start,Dir visionDir)
        {
            int cnt = 0;
            int considerHeight = m_map.GetAt(start);

            for (Point visionLoc = start.MovedPoint(visionDir); m_map.CheckBoundary(visionLoc); visionLoc.Move(visionDir))
            {
                cnt ++;
                int curHeight = m_map.GetAt(visionLoc);
                if (m_map.GetAt(visionLoc) >= considerHeight)
                {
                    break;
                }
            }
            return cnt;
        }

        private void CountVisibleInternal(Point start, Dir stepDir, Dir visionDir)
        {
            Point curLoc = start;

            LogDetail($"Start checking from {start}, step {stepDir}, vision {visionDir}");
            while (m_map.CheckBoundary(curLoc))
            {
                Point visionLoc = curLoc;

                LogDetail($"  Start visioning from {visionLoc}");

                int maxHeightSoFar = -1;

                while (m_map.CheckBoundary(visionLoc))
                {
                    int curHeight = m_map.GetAt(visionLoc);
                    if (curHeight > maxHeightSoFar)
                    {
                        LogDetail($"    Added {visionLoc}, height {curHeight} curCount {m_alreadySeen.Count}");
                        m_alreadySeen.Add(visionLoc);
                    }

                    maxHeightSoFar = Math.Max(maxHeightSoFar, curHeight);

                    visionLoc.Move(visionDir);
                }
                
                curLoc.Move(stepDir);
            }
        }
        private int CountVisible()
        {
            m_alreadySeen = new();

            CountVisibleInternal(new Point(0, 0), Dir.Right, Dir.Down);
            CountVisibleInternal(new Point(0, 0), Dir.Down, Dir.Right);
            CountVisibleInternal(new Point(m_map.Max.x - 1, 0), Dir.Down, Dir.Left);
            CountVisibleInternal(new Point(0, m_map.Max.y - 1), Dir.Right, Dir.Up);

            return m_alreadySeen.Count;
        }

        private int MaxScenecScore()
        {
            int max = 0;

            m_map.ForEach((val, point) =>
            {
                int score = 1;

                foreach (Dir dir in Enum.GetValues(typeof(Dir)).Cast<Dir>())
                {
                    score *= CheckScene(point, dir);
                }

                max = Math.Max(max, score);
            });
            return  max;
        }

        public Problem08(List<string> lines)
        {
            m_map = new(lines);
        }
        
        public int Solve1()
        {
            AllowLogDetail = false;
            return CountVisible();
        }

        public int Solve2()
        {
            AllowLogDetail = false;
            return MaxScenecScore();
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input08.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();

            Problem08 prob1 = new(textArr);
            Problem08 prob2 = new(textArr);

            var ans1 = prob1.Solve1();
            var ans2 = prob2.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


