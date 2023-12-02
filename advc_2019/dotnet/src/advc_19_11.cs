using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem11 : Advc.Utils.Loggable
    {
        enum TurnDir : long
        {
            Left = 0,
            Right = 1
        }

        public int RunRobot(Computer09 com, long startingColour, bool printMap = false)
        {
            Point curPos = new();
            Point curDir = Direction.Up;
            HashSet<Point> visitied = new();
            MapByDictionary<long> colorMap = new();

            visitied.Add(curPos);
            colorMap.SetAt(startingColour, curPos);
            
            while (!com.IsHalted)
            {
                var curCol = colorMap.GetAt(curPos);
                com.Run(curCol);
                Debug.Assert(com.OutputCount == 2);
                var nextCol = com.Output;
                var turnDir = (TurnDir) com.Output;
                var nextDir = turnDir == (long)TurnDir.Left ? Angles.RotateLeft(curDir) : Angles.RotateRight(curDir);
                var nextPos = curPos.AddedPoint(nextDir);

                LogDetail($"curPos {curPos} curDir {curDir} curCol {curCol} nextColr {nextCol} turnDir {turnDir} nextDir {nextDir} nextPos {nextPos}");

                // Paint and record
                colorMap.SetAt(nextCol, curPos);
                visitied.Add(curPos);

                // Move
                curPos = nextPos;
                curDir = nextDir;
            }

            if (printMap)
            {
                Dump(colorMap);
            }
            return visitied.Count;
        }

        public void Dump(MapByDictionary<long> map)
        {
            for (int y = map.ActualMin.y; y <= map.ActualMax.y; y ++)
            {
                for (int x = map.ActualMin.x; x <= map.ActualMax.x; x ++)
                {
                    Console.Write(map.GetAt(x, y) == 1 ? 'X' : ' ');
                }
                Console.WriteLine();
            }
        }

        public int Solve1(List<long> arr)
        {
            Computer09 com = new(arr);
            com.AllowLogDetail = false;
            return RunRobot(com, 0);
        }

        public int Solve2(List<long> arr)
        {
            Computer09 com = new(arr);
            com.AllowLogDetail = false;
            return RunRobot(com, 1, printMap: true);
        }

        public static void Start()
        {
            Point.LogDimension = 2;

            var textData = File.ReadAllText("../data/input11.txt");
            var textArr = textData.Split(",");
            var intList = textArr.Select(t => long.Parse(t)).ToList();

            Problem11 prob1 = new();
            prob1.AllowLogDetail = false;

            var ans1 = prob1.Solve1(intList);
            var ans2 = prob1.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


