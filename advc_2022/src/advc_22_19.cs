using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem19 : Loggable
    {
        enum Mineral 
        {
            Ore,
            Clay,
            Obsidian,
            Geode
        }

        private static Mineral GetMineralFromString(string str)
        {
            foreach(Mineral m in Enum.GetValues(typeof(Mineral)).Cast<Mineral>())
            {
                if (str == m.ToString().ToLower())
                {
                    return m;
                }
            }
            throw new ArgumentException($"Unknown mineral name {str}");
        }


        class RobotBlueprint
        {
            public Mineral OutputType { get; set; }
            public Dictionary<Mineral, int> Ingredients = new();

            public override string ToString()
            {
                var ingrStr = string.Join(",", Ingredients.Select(p => $"({p.Key},{p.Value}").ToList());
                return $"[{OutputType}:{ingrStr}]";
            }

            public static RobotBlueprint Parse(string line)
            {
                line = line.Trim();
                Queue<string> words = new(line.Split(" ").ToList());

                GenericUtil.ExpectString(words.Dequeue(), "Each");

                RobotBlueprint rbp = new();
                rbp.OutputType = GetMineralFromString(words.Dequeue());

                GenericUtil.ExpectString(words.Dequeue(), "robot");
                GenericUtil.ExpectString(words.Dequeue(), "costs");

                while (words.Any())
                {
                    int amount = int.Parse(words.Dequeue());
                    Mineral ingr = GetMineralFromString(words.Dequeue());
                    rbp.Ingredients[ingr] = amount;

                    if (words.Any())
                    {
                        GenericUtil.ExpectString(words.Dequeue(), "and");
                    }
                }

                return rbp;
            }
        }

        class BluePrint
        {
            public int Index { get; init; }
            public Dictionary<Mineral, RobotBlueprint> RobotBlueprints { get; set; } = new();

            public BluePrint(int index)
            {
                Index = index;
            }

            public override string ToString()
            {
                StringBuilder sb = new();
                sb.Append($"[BluePrint {Index.ToString().PadLeft(2)}]{Environment.NewLine}");
                foreach (var rb in RobotBlueprints)
                {
                    sb.Append($"     {rb.ToString()}{Environment.NewLine}");
                }

                return sb.ToString();
            }

            public static BluePrint Parse(string line)
            {
                Queue<string> linesQueue = new();
                var first2lines = line.Split(": ");
                string firstLine = first2lines.First();
                int index = int.Parse(GenericUtil.ExpectString(firstLine, "Blueprint "));
                BluePrint bluePrint = new(index);

                var bpLines = first2lines.Last().Split(".");
                foreach(var bpLine in bpLines)
                {
                    if (bpLine.Length == 0)
                    {
                        break;
                    }

                    RobotBlueprint rbp = RobotBlueprint.Parse(bpLine);
                    bluePrint.RobotBlueprints.Add(rbp.OutputType, rbp);
                }
                return bluePrint;
            }
        }

        class BluePrintSet
        {
            public List<BluePrint> BluePrints { get; set; } = new();

            public override string ToString()
            {
                return string.Join(Environment.NewLine, BluePrints);
            }

            public static BluePrintSet Parse(string[] lines)
            {
                BluePrintSet bluePrintSet = new();

                Queue<string> linesQueue = new(lines);

                while (linesQueue.Any())
                {
                    string line = linesQueue.Dequeue();
                    BluePrint bp = BluePrint.Parse(line);
                    bluePrintSet.BluePrints.Add(bp);
                }

                return bluePrintSet;
            }
        }

        private int Solve1(List<int> arr)
        {
            AllowLogDetail = true;

            return 0;
        }

        private int Solve2(List<int> arr)
        {
            AllowLogDetail = true;

            return 0;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input19s.txt");
            var lines = textData.Split(Environment.NewLine);

            BluePrintSet bluePrintSet = BluePrintSet.Parse(lines);
            Console.WriteLine(bluePrintSet);

            var ans1 = 0; //prob1.Solve1(intList);
            var ans2 = 0; //prob1.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


