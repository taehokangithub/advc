using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem0X : Advc.Utils.Loggable
    {
        public int Solve1(List<int> arr)
        {
            return 0;
        }

        public int Solve2(List<int> arr)
        {
            return 0;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("../data/input0X.txt");
            var textArr = textData.Split(Environment.NewLine);
            var intList = textArr.Select(t => int.Parse(t)).ToList();

            Problem0X prob1 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = 0;

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


