using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2022
{
    class Problem04 : Advc.Utils.Loggable
    {
        public record Section(int start, int end);

        public long Solve1(List<List<Section>> sectionPairList)
        {
            int cnt = 0;
            foreach (var sectionPair in sectionPairList)
            {
                if (sectionPair.First().end >= sectionPair.Last().end)
                {
                    cnt ++;
                }
                else if (sectionPair.Last().start <= sectionPair.First().start)
                {
                    cnt ++;
                }
            }
            return cnt;
        }

        public long Solve2(List<List<Section>> sectionPairList)
        {
            int cnt = 0;
            foreach (var sectionPair in sectionPairList)
            {
                if (sectionPair.First().end >= sectionPair.Last().start)
                {
                    cnt ++;
                }
            }
            return cnt;            

        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input04.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();

            List<List<Section>> sectionPairList = new();
            foreach (var line in textArr)
            {
                var sectionsText = line.Split(",");
                List<Section> sections = new();
                foreach (var sectionText in sectionsText)
                {
                    var numbers = sectionText.Split("-").Select(s => int.Parse(s));
                    Section s = new Section(numbers.First(), numbers.Last());
                    sections.Add(s);
                }
                sectionPairList.Add(sections.OrderBy(s => s.start).ToList());
            }

            var prob = new Problem04();

            var ans1 = prob.Solve1(sectionPairList);
            var ans2 = prob.Solve2(sectionPairList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


