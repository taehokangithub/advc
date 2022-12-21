using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem21 : Loggable
    {
        enum Operation {
            Const, Add, Sub, Mul, Div, Equals
        }

        class MonkeyOperation 
        {
            public string Name { get; init; }
            public Operation Operation { get; set; }
            public long Value { get; set; }
            public List<string> OperandNames { get; } = new();
            private Monkeys m_monkeys;
            private MonkeyOperation FirstOperand => m_monkeys.MonkeyOperations[OperandNames.First()];
            private MonkeyOperation SecondOperand => m_monkeys.MonkeyOperations[OperandNames.Last()];
            public MonkeyOperation(string line, Monkeys monkeys)
            {
                m_monkeys = monkeys;

                string[] parts = line.Split(": ");
                Name = parts.First();

                if (long.TryParse(parts.Last(), out var value))
                {
                    Operation = Operation.Const;
                    Value = value;
                }
                else 
                {
                    var opPart = parts.Last().Split(" ").ToArray();
                    Debug.Assert(opPart.Count() == 3);
                    OperandNames.Add(opPart.First());
                    OperandNames.Add(opPart.Last());
                    switch (opPart[1].First())
                    {
                        case '*' : Operation = Operation.Mul; break; 
                        case '/' : Operation = Operation.Div; break; 
                        case '+' : Operation = Operation.Add; break; 
                        case '-' : Operation = Operation.Sub; break; 
                        default: throw new InvalidDataException($"Unknown op char from {opPart}");
                    }
                }
            }

            public long GetValue()
            {
                if (Operation == Operation.Const)
                {
                    return Value;
                }
                var value1 = FirstOperand.GetValue();
                var value2 = SecondOperand.GetValue();

                switch (Operation)
                {
                    case Operation.Add : return value1 + value2;
                    case Operation.Sub: return value1 - value2;
                    case Operation.Mul: return value1 * value2;
                    case Operation.Div: return value1 / value2;
                }
                throw new UnreachableException();
            }

            public bool HasChild(string name)
            {
                if (Name == name)
                {
                    return true;
                }
                if (Operation == Operation.Const)
                {
                    return false;
                }
                return FirstOperand.HasChild(name) || SecondOperand.HasChild(name);
            }

            public long GetExpectedValue(string name, long targetValue)
            {
                if (name == Name)
                {
                    return targetValue;
                }

                if (Operation == Operation.Const)
                {
                    throw new InvalidOperationException($"Can't expect value for a const monkey {Name}");
                }

                var left = FirstOperand;
                var right = SecondOperand;
                bool isLeft = left.HasChild(name);

                var withTarget = isLeft ? left : right;
                var withoutTarget = isLeft ? right : left;
                var withoutTargetValue = withoutTarget.GetValue();
                long newTarget = 0;

                switch (Operation)
                {
                    case Operation.Const : throw new UnreachableException();
                    case Operation.Add : newTarget = targetValue - withoutTargetValue; break;
                    case Operation.Sub : newTarget = isLeft ? (targetValue + withoutTargetValue)
                                                            : (withoutTargetValue - targetValue); break;
                    case Operation.Mul : newTarget = targetValue / withoutTargetValue; break;
                    case Operation.Div : newTarget = isLeft ? (targetValue * withoutTargetValue) 
                                                            : (withoutTargetValue / targetValue); break;
                    case Operation.Equals : newTarget = withoutTargetValue; break;
                }

                return withTarget.GetExpectedValue(name, newTarget);
            }

        }

        class Monkeys
        {
            const string NameRoot = "root";
            const string NameHuman = "humn";

            public Dictionary<string, MonkeyOperation> MonkeyOperations = new();
            public Monkeys(string[] lines)
            {
                lines.ToList().ForEach(line => 
                {
                    MonkeyOperation monkey = new(line, this);
                    MonkeyOperations[monkey.Name] = monkey;
                });
            }

            public long GetRootValue()
            {
                return MonkeyOperations[NameRoot].GetValue();
            }

            public long GetHumnValue()
            {
                var root = MonkeyOperations[NameRoot];
                root.Operation = Operation.Equals;
                return root.GetExpectedValue(NameHuman, 0);
            }
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input21.txt");
            var lines = textData.Split(Environment.NewLine);

            Monkeys monkeys = new(lines);

            var ans1 = monkeys.GetRootValue();
            var ans2 = monkeys.GetHumnValue();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


