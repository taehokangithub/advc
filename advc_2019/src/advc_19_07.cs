using Advc.Utils.StateUtil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem07Defs
    {
        public static bool m_logDetail = true;
        public static void LogDetail(string str)
        {
            if (m_logDetail)
            {
                Console.WriteLine(str);
            }
        }
    }

    class Thruster
    {
        private Computer05 m_computer;
        public bool IsHalted => m_computer.IsHalted;
        public bool IsBlocked => m_computer.IsBlocked;

        public Thruster(IReadOnlyList<int> instructions)
        {
            m_computer = new Computer05(instructions);
        }

        public int Run(List<int> inputs)
        {
            m_computer.Run(new(inputs));

            Debug.Assert(m_computer.Output.Count == 1);

            return m_computer.Output.First();
        }
    }

    class Amplifier
    {
        private PossibleStates m_possibleStates;
        private IReadOnlyList<int> m_instructions;
        private int m_numThrusters;

        public Amplifier(int minState, int maxState, int numThrusters, IReadOnlyList<int> instructions)
        {
            m_instructions = instructions;
            m_numThrusters = numThrusters;
            m_possibleStates = new PossibleStates(minState, maxState, maxElements: numThrusters);
        }

        private List<Thruster> CreateThrusters()
        {
            var thrusters = new List<Thruster>();
            for (int i = 0; i < m_numThrusters; i++)
            {
                thrusters.Add(new Thruster(m_instructions));
            }
            return thrusters;
        }

        public int RunSequence(IReadOnlyList<int> states)
        {
            var thrusters = CreateThrusters();

            Debug.Assert(states.Count == thrusters.Count);
            Queue<int> stateQueue = new(states);

            int lastOutput = 0;
            int index = 0;
            foreach (var thruster in thrusters)
            {
                var input = lastOutput;
                var state = stateQueue.Dequeue();

                //Problem07Defs.LogDetail($"[Amp] Running thruster [{++index}] state {state} input {input}");
                lastOutput = thruster.Run(new List<int> { state, input });
            }
            //Problem07Defs.LogDetail($"[Amp/sequence] state {string.Join("", states)} => output {lastOutput}");

            return lastOutput;
        }

        public int RunFeedback(IReadOnlyList<int> states)
        {
            var thrusters = CreateThrusters();

            Debug.Assert(states.Count == thrusters.Count);
            Queue<int> stateQueue = new(states);

            int lastOutput = 0;
            int index = 0;

            while (!thrusters.Last().IsHalted)
            {
                foreach (var thruster in thrusters)
                {
                    var inputs = new List<int> {};

                    if (stateQueue.Count > 0)
                    {
                        inputs.Add(stateQueue.Dequeue());
                    }
                    inputs.Add(lastOutput);

                    lastOutput = thruster.Run(inputs);

                    Problem07Defs.LogDetail($"[Amp] thruster [{++index}] inputs {string.Join(",", inputs)} output {lastOutput} isBlocked {thruster.IsBlocked}, isHalted {thruster.IsHalted}");
                }
            }

            Problem07Defs.LogDetail($"[Amp/feedback] state {string.Join("", states)} => output {lastOutput}");

            return lastOutput;
        }

        public int FindBestOutput(bool useFeedback)
        {
            int maxOutput = 0;
            foreach (var state in m_possibleStates.States)
            {
                int output = useFeedback ? RunFeedback(state) : RunSequence(state);
                if (maxOutput < output)
                {
                    maxOutput = output;
                    Problem07Defs.LogDetail($"Found new max {output} at {string.Join("", state)}");
                }
            }

            return maxOutput;
        }
    }

    class Problem07
    {
        private IReadOnlyList<int> m_instructions;

        public Problem07(List<int> instructions)
        {
            m_instructions = instructions;
        }

        public int Solve1()
        {
            Amplifier amp = new(minState: 0, maxState: 4, numThrusters: 5, m_instructions);
            return amp.FindBestOutput(useFeedback: false);
        }

        public int Solve2()
        {
            Amplifier amp = new(minState: 5, maxState: 9, numThrusters: 5, m_instructions);
            return amp.FindBestOutput(useFeedback: true);
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input07.txt");
            //var textData = ("3,23,3,24,1002,24,10,24,1002,23,-1,23,101,5,23,23,1,24,23,23,4,23,99,0,0");
            var textArr = textData.Split(',');
            var intList = textArr.Select(t => int.Parse(t)).ToList();
            var problem = new Problem07(intList);
            var ans1 = problem.Solve1();
            var ans2 = problem.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


