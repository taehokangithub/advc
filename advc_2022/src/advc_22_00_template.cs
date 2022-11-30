using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2022
{
    class Problem00 : Advc.Utils.LogUtils.Loggable
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
            var textData = File.ReadAllText("data/input00.txt");
            var textArr = textData.Split(Environment.NewLine);
            var intList = textArr.Select(t => int.Parse(t)).ToList();

            Problem00 prob1 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = 0;

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


