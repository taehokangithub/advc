using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem11 : Loggable
    {
        public record MonkeyAction(int throwTo, long item);

        public class Monkey : Loggable
        {
            public static int DivideBy { get; set; } = 3;
            public static int MaxItemValue { get; set; } = int.MaxValue;
            public enum Operations { Add, Multi, Square }
            public int Id { get; init; }
            public Queue<long> Items { get; } = new();
            public Operations Operation { get; init; }
            public int OperationValue { get; init; }
            public int TestDivisable { get; init; }
            public Dictionary<bool, int> ThrowTo { get; } = new();
            public long TotalInspected { get; set; } = 0;

            public Monkey(Queue<string> lines)
            {
                AllowLogDetail = false;
                var getLineExcept = (string expected) => 
                {
                    string str = lines.Dequeue().Trim();
                    Debug.Assert(str.StartsWith(expected));
                    string ret = str.Substring(expected.Length);
                    return ret;
                };

                Id = int.Parse(getLineExcept("Monkey ").First().ToString());
                Items = new (getLineExcept("Starting items: ").Split(", ").Select(s =>long.Parse(s)).ToList());

                string opLine = getLineExcept("Operation: new = old ");
                char opChar = opLine.First();
                string opValStr = opLine.Split(" ").Last();
                Operation = (opValStr == "old") ? Operations.Square : (opChar == '+') ? Operations.Add : (opChar == '*') ? Operations.Multi : throw new InvalidDataException($"Unknown operation {opChar}");
                OperationValue = (Operation == Operations.Square) ? 0 : int.Parse(opValStr);
                
                TestDivisable = int.Parse(getLineExcept("Test: divisible by "));
                ThrowTo[true] = int.Parse(getLineExcept("If true: throw to monkey "));
                ThrowTo[false] = int.Parse(getLineExcept("If false: throw to monkey "));

                if (lines.Count > 0)
                {
                    lines.Dequeue();
                }
            }

            public List<MonkeyAction> RunRound()
            {
                List<MonkeyAction> actions = new();

                while (Items.Any())
                {
                    var item = Items.Dequeue();
                    var val = (Operation == Operations.Add ? (item + OperationValue)
                                : Operation == Operations.Multi ? (item * OperationValue)
                                : Operation == Operations.Square ? (item * item) : throw new InvalidDataException($"Unknown operation {Operation}"));

                    val /= DivideBy;
                    val %= MaxItemValue;
                    
                    int throwTo = ThrowTo[val % TestDivisable == 0];
                    MonkeyAction action = new(throwTo, val);
                    actions.Add(action);
                    LogDetail($"Monkey {Id} {action}");
                    TotalInspected ++;
                }
                return actions;
            }

            public override string ToString()
            {
                return $"{ItemsToString()} {Operation} {OperationValue} / TestDiv {TestDivisable} true {ThrowTo[true]} false {ThrowTo[false]} ";
            }

            public string ItemsToString()
            {
                return $"[Monkye {Id} (Total {TotalInspected})] / Items {string.Join(", ", Items.ToList())} ";
            }

            public static List<Monkey> Parse(string[] lines)
            {
                Queue<string> linesQueue = new(lines);

                List<Monkey> monkeys = new();

                while (linesQueue.Count > 0)
                {
                    monkeys.Add(new(linesQueue));
                }

                return monkeys;
            }
        }

        public long DoRounds(List<Monkey> monkeys, int times)
        {
            for (int i = 0; i < times; i ++)
            {
                LogDetail($"----- Round {i + 1}");
                foreach (var monkey in monkeys)
                {
                    var actions = monkey.RunRound();

                    foreach (var action in actions)
                    {
                        monkeys[action.throwTo].Items.Enqueue(action.item);
                    }
                }

                if (AllowLogDetail)
                {
                    foreach (var monkey in monkeys)
                    {
                        LogDetail(monkey.ItemsToString());
                    }
                }
            }

            var topMonkyes = monkeys.OrderByDescending(m => m.TotalInspected).ToList();
            return topMonkyes[0].TotalInspected * topMonkyes[1].TotalInspected;            
        }

        public long Solve1(List<Monkey> monkeys)
        {
            AllowLogDetail = false;
            Monkey.DivideBy = 3;
            return DoRounds(monkeys, 20);
        }

        public long Solve2(List<Monkey> monkeys)
        {
            AllowLogDetail = false;
            Monkey.DivideBy = 1;

            int maxItemValue = 1;
            foreach (var monkey in monkeys)
            {
                maxItemValue *= monkey.TestDivisable;
            }
            Monkey.MaxItemValue = maxItemValue;
            LogDetail($"max val = {maxItemValue}");
            return DoRounds(monkeys, 10000);
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input11.txt");
            var lines = textData.Split(Environment.NewLine);

            Problem11 prob1 = new();

            var ans1 = prob1.Solve1(Monkey.Parse(lines));
            var ans2 = prob1.Solve2(Monkey.Parse(lines));

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


// 2713310158 2713310158
// 20709554856  high 