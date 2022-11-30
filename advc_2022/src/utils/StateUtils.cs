using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Advc.Utils.StateUtil
{
    class PossibleStates
    {
        private List<IReadOnlyList<int>> m_states = new();
        private int m_minState;
        private int m_maxState;
        private int m_maxElements;
        public IReadOnlyList<IReadOnlyList<int>> States => m_states;
        public static bool AllowLogDetail { get; set; } = false;

        public PossibleStates(int minState, int maxState, int maxElements)
        {
            m_maxElements = maxElements;
            m_minState = minState;
            m_maxState = maxState;

            AddState(new());
        }

        private void LogDetail(string str)
        {
            if (AllowLogDetail)
            {
                Console.WriteLine(str);
            }
        }

        private void AddState(List<int> partialState)
        {
            if (partialState.Count == m_maxElements)
            {
                m_states.Add(partialState);
                LogDetail($"Adding state {string.Join("", partialState)}");
                return;
            }

            for (int i = m_minState; i <= m_maxState; i++)
            {
                if (partialState.Contains(i))
                {
                    continue;
                }
                var newList = new List<int>(partialState);
                newList.Add(i);
                AddState(newList);
            }
        }
    }

}