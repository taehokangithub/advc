using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem16 : Advc.Utils.Loggable
    {
        const int MaxPhase = 100;
        private IReadOnlyList<int> m_basePattern = new List<int> { 0, 1, 0, -1 };
        
        private List<int> PatternEvolve(IReadOnlyList<int> pattern, int phase)
        {
            List<int> result = new();

            foreach (int p in pattern)
            {
                result.AddRange(Enumerable.Repeat(p, phase));
            }

            //LogDetail($"phase {phase} pattern {string.Join(",",result)}");
            return result;
        }

        private List<char> Phase(List<char> input, IReadOnlyList<int> pattern)
        {
            List<char> output = new();
            for (int i = 0; i < input.Count; i ++)
            {
                int sum = 0;
                var evolvedPattern = PatternEvolve(pattern, i + 1);
                for (int k = 0; k < input.Count; k ++)
                {
                    int patternIndx = ((k % evolvedPattern.Count) + 1) % evolvedPattern.Count;
                    int inputVal = int.Parse(input[k].ToString());
                    int patternVal = evolvedPattern[patternIndx];
                    int val = (inputVal * patternVal) % 10;
                    sum += val;

                    //LogDetail($"[{i}:{k}:{patternIndx}] {inputVal} * {patternVal} => {val}");
                }

                char c = (Math.Abs(sum) % 10).ToString().FirstOrDefault();
                output.Add(c);
            }
            return output;
        }

        public int Solve1(string arr)
        {
            AllowLogDetail = false;
            List<char> signal = arr.ToList();

            for (int i = 0; i < MaxPhase; i ++)
            {
                signal = Phase(signal, m_basePattern);

                LogDetail($"Phase {i} => signal {string.Join("",signal)}");
            }
            

            string outputString = string.Join("",signal.Take(8));
            LogDetail($"output string => {outputString}");
            return int.Parse(outputString);
        }

/*
    0 : 1xxxxxxxxx
    1 : 01xxxxxxxx
    2 : 001xxxxxxx
    3 : 0001xxxxxx
    4 : 00001xxxxx
    5 : 0000011111
    6 : 0000001111
    7 : 0000000111
    8 : 0000000011
    9 : 0000000001
*/

        public int Solve2(string arr)
        {
            AllowLogDetail = false;
            List<char> signal = arr.ToList();
            var globalOffset =  int.Parse(string.Join("",signal.Take(7)));

            LogDetail($"Signal Offset = {globalOffset}");
            Debug.Assert(globalOffset >= arr.Length / 2);

            // first phase - create a transformed signal
            const int multiplier = 10000;
            List<char> transformed = new(3000000);
                       
            int totalSignalLength = signal.Count * multiplier;
            int sumToEnd = 0;
            for (int i = globalOffset; i < totalSignalLength; i ++)
            {   
                char c = signal[i % signal.Count];
                int valInt = GenericUtil.CharToInt(c);
                Debug.Assert(valInt >= 0 && valInt <= 9);
                sumToEnd += valInt;
            }

            LogDetail($"sum To End {sumToEnd} from {globalOffset} to {totalSignalLength}");

            for (int i = globalOffset; i < totalSignalLength; i ++)
            {
                char curDigitChar = GenericUtil.IntTochar(sumToEnd % 10);
                Debug.Assert(curDigitChar >= '0' && curDigitChar <= '9');
                transformed.Add(curDigitChar);

                char c = signal[i % signal.Count];
                int toMinus = GenericUtil.CharToInt(c);
                Debug.Assert(toMinus >= 0 && toMinus <= 9);

                sumToEnd -= toMinus;
                //LogDetail($"sum {sumToEnd} => char {curDigitChar}, minus {toMinus}");
                Debug.Assert(sumToEnd >= 0);
            }

            LogDetail($"[1] {string.Join("",transformed)}");

            for (int i = 1; i < MaxPhase; i ++)
            {
                List<char> newTransformed = new List<char>(transformed.Count);
                int newSum = transformed.Sum(c => GenericUtil.CharToInt(c));

                foreach (char c in transformed)
                {
                    var newVal = GenericUtil.IntTochar(newSum % 10);
                    newTransformed.Add(newVal);
                    newSum -= GenericUtil.CharToInt(c);
                }

                transformed = newTransformed;
                LogDetail($"[{i + 1}] {string.Join("",transformed)}");
            }
            

            string outputString = string.Join("",transformed.Take(8));
            LogDetail($"output string => {outputString}");
            return int.Parse(outputString);            
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input16.txt");
            //var textData = "80871224585914546619083218645595";
            //var textData = "03036732577212944063491565474664";

            Problem16 prob1 = new();

            var ans1 = prob1.Solve1(textData);
            var ans2 = prob1.Solve2(textData);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


