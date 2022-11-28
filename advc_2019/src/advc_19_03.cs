using Advc.Utils.MapUtil;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class WireMap
    {
        private List<List<string>> m_ops = new();
        private MapByDictionary<int> m_map = new();
        private MapByDictionary<int> m_mapForStep = new();

        public int MinDist { get; private set; } = int.MaxValue;
        public int MinSteps { get; private set; } = int.MaxValue;

        public WireMap(List<string> arr1, List<string> arr2)
        {
            m_ops.Add(new List<string>(arr1));
            m_ops.Add(new List<string>(arr2));
        }

        public void RunOps()
        {
            const int detectSumValue = 3; // id 1 + 2
            int id = 0;
            foreach (var ops in m_ops)
            {
                id++;

                int steps = 0;

                Point curPos = new Point();

                foreach (var op in ops)
                {
                    RunOneOp(ref curPos, op, id, detectSumValue, ref steps);
                }
            }
        }

        private void RunOneOp(ref Point curPos, string op, int id, int detectSumValue, ref int steps)
        {
            char command = op[0];
            string operand = op.Substring(1);
            int operandValue = int.Parse(operand);

            for (int i = 0; i < operandValue; i ++)
            {
                MovePos(ref curPos, command);
                steps++;

                int curValue = m_map.GetAt(curPos);
                int newValue = curValue + id;
                m_map.SetAt(newValue, curPos);

                if (m_mapForStep.GetAt(curPos) == 0)
                {
                    m_mapForStep.SetAt(steps, curPos);
                }

                if (newValue == detectSumValue)
                {
                    int dist = Math.Abs(curPos.x) + Math.Abs(curPos.y);
                    //Console.WriteLine($"[Found1] {curPos} dist {dist} MinDist {MinDist}");
                    MinDist = Math.Min(MinDist, dist);

                    int sumSteps = steps + m_mapForStep.GetAt(curPos);
                    //Console.WriteLine($"[Found2] {curPos} steps {sumSteps} MinSteps {MinSteps}");
                    MinSteps = Math.Min(MinSteps, sumSteps);
                }
            }

        }

        private void MovePos(ref Point curPos, char command)
        {
            switch (command)
            {
                case 'U': curPos.Add(Direction.Up); break;
                case 'D': curPos.Add(Direction.Down); break;
                case 'R': curPos.Add(Direction.Right); break;
                case 'L': curPos.Add(Direction.Left); break;
                default:
                    throw new ArgumentException($"[RunOneOp] unknown command {command}");
            }
        }
        
    }

    class Problem03
    {
        public static int Solve1(List<string> arr1, List<string> arr2)
        {
            Point.LogDimension = 2;

            var wireMap = new WireMap(arr1, arr2);
            wireMap.RunOps();
            return wireMap.MinDist;
        }

        public static int Solve2(List<string> arr1, List<string> arr2)
        {
            var wireMap = new WireMap(arr1, arr2);
            wireMap.RunOps();
            return wireMap.MinSteps;
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input03.txt");
            var textArrays = textData.Split(Environment.NewLine);

            var arr1 = textArrays[0].Split(",").ToList();
            var arr2 = textArrays[1].Split(",").ToList();

            int ans1 = Solve1(arr1, arr2);
            int ans2 = Solve2(arr1, arr2);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


