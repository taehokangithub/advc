using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Problem04
    {
        private enum DoubleState
        {
            None,
            Started,
            LargerGroup,
            Confirmed,
        }

        HashSet<string> m_solve1Answers = new();

        public int Solve1(List<int> arr)
        {
            int ans = 0;
            for (long i = arr[0]; i <= arr[1]; i ++)
            {
                string str = i.ToString();

                bool hasFoundDouble = false;
                bool isFailed = false;
                
                for (int k = 1; k < str.Length; k ++)
                {
                    char current = str[k];
                    char prev = str[k - 1];

                    if (current < prev)
                    {
                        isFailed = true;
                        break;
                    }
                    else if (current == prev)
                    {
                        hasFoundDouble = true;
                    }
                }

                if (hasFoundDouble && !isFailed)
                {
                    ans ++;
                    m_solve1Answers.Add(str);
                }
            }
            return ans;
        }

        public int Solve2(List<int> arr)
        {
            int ans = 0;
            foreach (var str in m_solve1Answers)
            {
                DoubleState state = DoubleState.None;

                for (int k = 1; k < str.Length; k ++)
                {
                    char current = str[k];
                    char prev = str[k - 1];

                    if (current == prev)
                    {
                        if (state == DoubleState.None)
                        {
                            state = DoubleState.Started;
                        }
                        else if (state == DoubleState.Started)
                        {
                            state = DoubleState.LargerGroup;
                        }
                    }
                    else
                    {
                        if (state == DoubleState.Started)
                        {
                            state = DoubleState.Confirmed;
                            break;
                        }
                        else
                        {
                            state = DoubleState.None;
                        }
                    }
                }

                if (state == DoubleState.Confirmed || state == DoubleState.Started)
                {
                    ans ++;
                }
            }
            return ans;
        }
        public static void Start()
        {
            var textData = "372304-847060";
            var textArr = textData.Split('-');
            var intList = textArr.Select(t => int.Parse(t)).ToList();

            Problem04 prob = new();

            int ans1 = prob.Solve1(intList);
            int ans2 = prob.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


