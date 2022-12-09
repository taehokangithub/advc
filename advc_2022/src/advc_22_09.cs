using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem09 : Advc.Utils.Loggable
    {
        public record Cmd(Direction.Dir dir, int val);

        private static List<Cmd> ParseCmd(List<List<string>> arr)
        {
            List<Cmd> cmdList = new();
            foreach(var cmd in arr)
            {
                char dirChar = cmd.First().First();
                int val = int.Parse(cmd.Last());
                Direction.Dir dir;
                switch (dirChar)
                {
                    case 'L' : dir = Direction.Dir.Left; break;
                    case 'R' : dir = Direction.Dir.Right; break;
                    case 'U' : dir = Direction.Dir.Up; break;
                    case 'D' : dir = Direction.Dir.Down; break; 
                    default:
                    throw new Exception($"Unknown dir char {dirChar}");
                }
                var cmdRecord = new Cmd(dir, val);
                cmdList.Add(cmdRecord);
            }
            return cmdList;
        }

        Point GetFollowerPosition(Point headLoc, Point tailLoc)
        {
            Point diff = headLoc.SubtractedPoint(tailLoc);

            if (Math.Abs(diff.x) > 1 || Math.Abs(diff.y) > 1)
            {
                tailLoc.Add(diff.GetBasePoint());
            }

            LogDetail($"Head {headLoc} tail {tailLoc} diff {diff}");
            return tailLoc;
        }

        HashSet<Point> GetTailMovedPoints(List<Cmd> cmdList, int numTails)
        {
            HashSet<Point> movedPoints = new();

            Point headLoc = new();
            List<Point> tailLocs = new();

            for (int i = 0; i < numTails; i ++)
            {
                tailLocs.Add(new());
            }

            foreach (var cmd in cmdList)
            {
                Point moveVec = Direction.DirVectors[(int)cmd.dir];

                for (int i = 0; i < cmd.val; i ++)
                {
                    headLoc.Add(moveVec);

                    for (int t = 0; t < numTails; t ++)
                    {
                        Point prevTail = (t == 0) ? headLoc : tailLocs[t - 1];
                        tailLocs[t] = GetFollowerPosition(prevTail, tailLocs[t]);
                    }
                    movedPoints.Add(tailLocs[numTails - 1]);
                }
            }

            return movedPoints;
        }

        public int Solve1(List<Cmd> cmdList)
        {
            AllowLogDetail = false;

            return GetTailMovedPoints(cmdList, 1).Count;
        }

        public int Solve2(List<Cmd> cmdList)
        {
            AllowLogDetail = false;

            return GetTailMovedPoints(cmdList, 9).Count;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input09.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();
            var cmdList = textArr.Select(t => t.Split(" ").ToList()).ToList();
            var parsedCmdList = ParseCmd(cmdList);
            Problem09 prob1 = new();

            var ans1 = prob1.Solve1(parsedCmdList);
            var ans2 = prob1.Solve2(parsedCmdList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


