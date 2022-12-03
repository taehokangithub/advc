using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2022
{
    class Problem01 : Advc.Utils.Loggable
    {
        public static long Solve1(List<List<int>> intLists)
        {
            return intLists.Select(l => l.Sum()).OrderByDescending(v => v).First();
        }

        public static long Solve2(List<List<int>> intLists)
        {
            var sumsOrdered = intLists.Select(l => l.Sum()).OrderByDescending(p => p).ToList();

            long ans = 0;
            
            for (int i = 0; i < 3; i ++)
            {
                ans += sumsOrdered[i];
            }

            return ans;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input01.txt");
            var textArr = textData.Split(Environment.NewLine);

            List<List<int>> intLists = new();
            intLists.Add(new());

            foreach (var line in textArr)
            {
                if (line.Length == 0)
                {
                    intLists.Add(new());
                }
                else
                {
                    intLists.Last().Add(int.Parse(line));
                }
            }

            var ans1 = Problem01.Solve1(intLists);
            var ans2 = Problem01.Solve2(intLists);;

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


