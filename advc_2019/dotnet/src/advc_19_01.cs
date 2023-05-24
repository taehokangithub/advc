using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Problem01
    {
        public static int GetFuelSimple(int mass)
        {
            return mass / 3 - 2;
        }
        public static int Solve1(List<int> arr)
        {
            int ans = 0;
            foreach (var mass in arr)
            {
                ans += GetFuelSimple(mass);
            }

            return ans;
        }

        public static int Solve2Internal(int mass)
        {
            int fuel = GetFuelSimple(mass);

            if (fuel > 0)
            {
                fuel += Solve2Internal(fuel);
            }
            else
            {
                fuel = 0;
            }

            return fuel;
        }
        public static int Solve2(List<int> arr)
        {
            int ans = 0;
            foreach (var value in arr)
            {
                ans += Solve2Internal(value);
            }

            return ans;
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input01.txt");
            var textArr = textData.Split(Environment.NewLine);
            var intList = textArr.Select(t => int.Parse(t)).ToList();
            int ans1 = Solve1(intList);
            int ans2 = Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


