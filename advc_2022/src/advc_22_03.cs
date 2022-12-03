using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2022
{
    class RucksackGroup 
    {
        const int GroupSize = 3;
        List<string> m_lines = new();

        public bool AddLine(string line)
        {
            if (m_lines.Count < GroupSize)
            {
                m_lines.Add(line);
                return true;
            }
            return false;
        }

        public char FindBadge()
        {
            List<HashSet<char>> hashes = m_lines.Select(line => line.ToHashSet()).ToList();
            
            HashSet<char> intersection = hashes.First();

            foreach (var hash in hashes)
            {
                if (hash != intersection)
                {
                    intersection.IntersectWith(hash);
                }
            }

            Debug.Assert(intersection.Count == 1);
            return intersection.First();
        }
    }

    class Problem03 : Advc.Utils.Loggable
    {
        private int GetPriority(char c)
        {
            if (char.IsLower(c))
            {
                return c - 'a' + 1;
            }
            return c - 'A' + 27;
        }

        public long Solve1(List<string> lines)
        {
            List<char> shared = new();

            foreach (var line in lines)
            {
                string r1 = line.Substring(0, line.Length / 2);
                string r2 = line.Substring(line.Length / 2);

                HashSet<char> hashSet = r1.ToHashSet();
                hashSet.IntersectWith(r2.ToHashSet());

                Debug.Assert(hashSet.Count == 1);
                shared.Add(hashSet.First());
            }

            return shared.Sum(c => GetPriority(c));
        }

        public long Solve2(List<string> lines)
        {
            List<RucksackGroup> groups = new List<RucksackGroup>{new()};

            foreach (var line in lines)
            {
                if (!groups.Last().AddLine(line))
                {
                    groups.Add(new());
                    groups.Last().AddLine(line);
                }
            }

            LogDetail($"Found {groups.Count} groups");

            var badges = groups.Select(g => g.FindBadge());
            return badges.Sum(b => GetPriority(b));
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input03.txt");
            var textArr = textData.Split(Environment.NewLine).ToList();

            var prob = new Problem03();

            var ans1 = prob.Solve1(textArr);
            var ans2 = prob.Solve2(textArr);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


