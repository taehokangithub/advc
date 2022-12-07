using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem06 : Advc.Utils.Loggable
    {
        public int Solve(string textData, int sigLengthToFind)
        {
            Queue<char> recvQ = new();
            for (int i = 0; i < textData.Length; i ++)
            {
                recvQ.Enqueue(textData[i]);
                if (recvQ.Count > sigLengthToFind)
                {
                    recvQ.Dequeue();
                }
                if (recvQ.ToHashSet().Count == sigLengthToFind)
                {
                    return i + 1;               
                }
            }
            return 0;
        }

        public static void Start()
        {
            var textData = File.ReadAllText("data/input06.txt");

            Problem06 prob1 = new();

            var ans1 = prob1.Solve(textData, 4);
            var ans2 = prob1.Solve(textData, 14);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


