using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Problem0X
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
            var textData = File.ReadAllText("data/input0X.txt");
            var textArr = textData.Split(Environment.NewLine);
            var intList = textArr.Select(t => int.Parse(t)).ToList();

            Problem0X prob1 = new();
            Problem0X prob2 = new();

            int ans1 = prob1.Solve1(intList);
            int ans2 = prob2.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


