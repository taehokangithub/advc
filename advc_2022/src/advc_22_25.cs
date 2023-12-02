using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem25 : Loggable
    {
        class Converter5 : Loggable
        {
            public long ConvertToBase10(string base5)
            {
                long val = 0;
                long mul = 1;
                foreach (char c in base5.ToCharArray().Reverse())
                {
                    long operand = (c == '0') ? 0 :
                                  (c == '1') ? 1 :
                                  (c == '2') ? 2 :
                                  (c == '-') ? -1 :
                                  (c == '=') ? -2 : throw new InvalidDataException($"Unknown char {c}");
                    val += operand * mul;
                    mul *= 5;
                }
                Debug.Assert(val > 0, $"Invalid value {val} converting from {base5}");
                return val;
            }

            public string ConvertToBase5(long val)
            {
                long orgVal = val;
                Queue<char> base5Reversed = new();
                for (long div = 5; val > 0; div *= 5)
                {
                    long prevDiv = div / 5;
                    long valReminder = val % div;
                    long v = valReminder / prevDiv;
                    val -= valReminder;

                    Debug.Assert(v >= 0 && v <= 4, $"value {v} out of range (0-4)");
                    base5Reversed.Enqueue(GenericUtil.IntTochar((int) v));
                }

                List<char> newBase5Reversed = new();

                LogDetail($"Base 5 rev {string.Join("", base5Reversed.ToList())}");

                int toUpper = 0;
                while (base5Reversed.Any())
                {
                    var c = base5Reversed.Dequeue();
                    int cv = GenericUtil.CharToInt(c) + toUpper;
                    toUpper = 0;

                    if (cv <= 2)
                    {
                        newBase5Reversed.Add(GenericUtil.IntTochar(cv));
                    }
                    else 
                    {
                        char newC = cv == 5 ? '0' 
                                  : cv == 4 ? '-' 
                                  : cv == 3 ? '=' : throw new InvalidOperationException($"Value {cv} should be between 3 and 5");
                        newBase5Reversed.Add(newC);

                        if (!base5Reversed.Any())
                        {
                            base5Reversed.Enqueue('1');
                        }
                        else
                        {
                            toUpper = 1;
                        }
                    }
                }
               
                newBase5Reversed.Reverse();
                LogDetail($"New Base 5  {string.Join("", newBase5Reversed)}");

                var ans = string.Join("", newBase5Reversed);
                long valToTest = ConvertToBase10(ans);
                Debug.Assert(valToTest == orgVal, $"{valToTest} != {orgVal}");

                return ans;
            }

            public string Solve1(string[] lines)
            {
                AllowLogDetail = false;
                long ans = 0;
                foreach (var line in lines)
                {
                    var val = ConvertToBase10(line);
                    LogDetail($"Converted {line} to {val}");
                    ans += val;
                }
                LogDetail($"Total value {ans}");
                return ConvertToBase5(ans);
            }

            public string Solve2(string[] lines)
            {
                return "All 50 stars received!";
            }
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input25.txt");
            var lines = textData.Split(Environment.NewLine);

            var ans1 = new Converter5().Solve1(lines);
            var ans2 = new Converter5().Solve2(lines);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


