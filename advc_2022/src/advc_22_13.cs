using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem13 : Loggable
    {
        class Signal
        {
            public int Value { get; init; }
            public List<Signal> Signals { get; init; } = new();
            public bool IsList { get; init; } = false;

            public Signal() {}

            public static Signal ValueSignalToListSignal(Signal from)
            {
                Debug.Assert(!from.IsList);
                var newSignal = new Signal{
                    IsList = true
                };

                newSignal.Signals.Add(from);
                return  newSignal;
            }

            public Signal(Queue<char> input)
            {
                if (input.First() == '[')
                {
                    IsList = true;
                    char c = input.Dequeue(); // remove '['

                    if (input.First() == ']')
                    {
                        input.Dequeue();
                    }
                    else
                    {
                        while (c != ']')
                        {
                            Signals.Add(new(input));
                            c = input.Dequeue();
                        }
                    }
                }
                else
                {
                    // value
                    Queue<char> numbers = new();
                    while (char.IsNumber(input.First()))
                    {
                        numbers.Enqueue(input.Dequeue());
                    }
                    var valueStr = string.Join("", numbers.ToList());
                    Value = int.Parse(valueStr);
                }
            }

            public override string ToString()
            {
                if (IsList)
                {
                    return $"[{string.Join(",", Signals)}]";
                }
                return Value.ToString();
            }
        }

        class SignalPair
        {
            public Tuple<Signal, Signal> Pair { get; init; }
            public SignalPair(Queue<string> lines)
            {
                Signal first = new Signal(new Queue<char>(lines.Dequeue().ToCharArray()));
                Signal second = new Signal(new Queue<char>(lines.Dequeue().ToCharArray()));
                Pair = new(first, second);

                if (lines.Any())
                {
                    lines.Dequeue();
                }
            }

            private enum CompareResult { Ordered, Same, NotOrdered }

            private static CompareResult CompareValue(int first, int second)
            {
                return (first > second ? CompareResult.NotOrdered 
                            : first < second ? CompareResult.Ordered
                            : CompareResult.Same);
            }

            private static CompareResult CompareList(Signal first, Signal second)
            {
                Debug.Assert(first.IsList && second.IsList);

                Queue<Signal> firstQ = new(first.Signals);
                Queue<Signal> secondQ = new(second.Signals);

                while (firstQ.Any() && secondQ.Any())
                {
                    Signal firstElement = firstQ.Dequeue();
                    Signal secondElement = secondQ.Dequeue();

                    var result = Compare(firstElement, secondElement);
                    if (result != CompareResult.Same)
                    {
                        return result;
                    }
                }

                return CompareValue(first.Signals.Count, second.Signals.Count);
            }

            private static CompareResult Compare(Signal first, Signal second)
            {
                if (first.IsList && !second.IsList)
                {
                    return Compare(first, Signal.ValueSignalToListSignal(second));
                }
                else if (!first.IsList && second.IsList)
                {
                    return Compare(Signal.ValueSignalToListSignal(first), second);
                }
                else if (!first.IsList && !second.IsList)
                {
                    return CompareValue(first.Value, second.Value);
                }
                else
                {
                    return CompareList(first, second);
                }
            }

            public static int CompareForSort(Signal first, Signal second)
            {
                var result = Compare(first, second);
                return result == CompareResult.Ordered ? -1 :
                        result == CompareResult.Same ? 0 : 1;
            }

            public bool IsOrdered()
            {
                var result = Compare(Pair.Item1,  Pair.Item2);
                Debug.Assert(result != CompareResult.Same);
                return result == CompareResult.Ordered;
            }

        }

        class AllSignals : Loggable
        {
            List<SignalPair> SignalPairs = new();
            List<Signal> Signals = new();

            public AllSignals(string[] data)
            {
                AllowLogDetail = true;
                var lines = new Queue<string>(data);

                while (lines.Any())
                {
                    SignalPairs.Add(new SignalPair(lines));

                    var lastPair = SignalPairs.Last().Pair;
                    Signals.Add(lastPair.Item1);
                    Signals.Add(lastPair.Item2);
                }
            }

            public int GetDecoderKey()
            {
                int ans = 1;

                Signal s1 = new Signal(new Queue<char>("[[2]]"));
                Signal s2 = new Signal(new Queue<char>("[[6]]"));

                Signals.Add(s1);
                Signals.Add(s2);
                Signals.Sort(SignalPair.CompareForSort);

                for (int i = 0; i < Signals.Count; i ++)
                {
                    Signal s = Signals[i];
                    if (s == s1 || s == s2)
                    {
                        LogDetail($"found divider {s} at {i + 1}");
                        ans *= (i + 1);
                    }
                }

                return ans;
            }

            public int CountOrdered()
            {
                int ans = 0;
                
                for (int i = 0; i < SignalPairs.Count; i ++)
                {
                    var pair = SignalPairs[i];
                    if (pair.IsOrdered())
                    {
                        LogDetail($"found ordered at {i + 1}");
                        ans += i + 1;
                    }
                }
                return ans;
            }
        }

        int Solve1(AllSignals allSignals)
        {
            allSignals.AllowLogDetail = false;

            return allSignals.CountOrdered();
        }

        int Solve2(AllSignals allSignals)
        {
            allSignals.AllowLogDetail = false;

            return allSignals.GetDecoderKey();
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input13.txt");
            var textArr = textData.Split(Environment.NewLine);
            
            Problem13 prob1 = new();

            var ans1 = prob1.Solve1(new(textArr));
            var ans2 = prob1.Solve2(new(textArr));

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


