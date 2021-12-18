
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;

class Advc21_18
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    class StringStream
    {
        string m_str;
        int m_pos = 0;

        public StringStream(string str)
        {
            m_str = str;
        }

        public string GetString(int length)
        {
            Debug.Assert(m_str.Length >= m_pos + length);
            string ret = m_str.Substring(m_pos, length);

            m_pos += length;
            return ret;
        }

        public char GetChar()
        {
            return GetString(1)[0];
        }
    }

    class Pair
    {
        enum PairDir { Left, Right };

        const char s_openBracket = '[';
        const char s_closeBracket = ']';
        const char s_comma = ',';
        const int s_reduceDepth = 5;
        const int s_splitThreshold = 10;
        List<Pair> m_pairs = new List<Pair>();
        int m_value = -1;
        int m_depth = 1;
        Pair? m_parent;

        public Pair(StringStream line, Pair? parent = null)
        {
            m_parent = parent;
            char ch = line.GetChar();

            if (ch == s_openBracket)
            {
                Pair left = new Pair(line, this);
                ch = line.GetChar();
                Debug.Assert(ch == s_comma);
                Pair right = new Pair(line, this);
                ch = line.GetChar();
                Debug.Assert(ch == s_closeBracket);

                m_pairs.Add(left);
                m_pairs.Add(right);
            }
            else
            {
                m_value = int.Parse(ch.ToString());
            }
        }

        public Pair(Pair parent)
        {
            m_parent = parent;
            m_pairs = new List<Pair>(parent.m_pairs);

            m_value = parent.m_value;
        }

        public Pair(int value, Pair parent)
        {
            m_parent = parent;
            m_value = value;
        }

        public int GetMagnitude()
        {
            if (IsNumber())
            {
                return m_value;
            }
            else 
            {
                return 3 * GetChild(PairDir.Left).GetMagnitude() + 2 * GetChild(PairDir.Right).GetMagnitude();
            }
        }

        public Pair Add(Pair rhs)
        {
            Pair newLeft = new Pair(this);
            rhs.m_parent = this;

            m_pairs.Clear();
            m_pairs.Add(newLeft);
            m_pairs.Add(rhs);

            DoActions();
            return this;
        }
        static bool s_printDepth = false;
        public override string ToString()
        {
            if (m_pairs.Count > 0)
            {
                if (s_printDepth)
                {
                    return s_openBracket + $"({m_depth})" + GetChild(PairDir.Left).ToString() + s_comma + GetChild(PairDir.Right).ToString() + s_closeBracket;
                }
                return s_openBracket + GetChild(PairDir.Left).ToString() + s_comma + GetChild(PairDir.Right).ToString() + s_closeBracket;
            }
            else
            {
                return m_value.ToString();
            }
        }

        private bool IsNumber()
        {
            return m_pairs.Count == 0;
        }

        private Pair GetChild(PairDir dir)
        {
            Debug.Assert(m_pairs.Count == 2, $"Get child for {this}");
            switch (dir)
            {
                case PairDir.Left:
                    return m_pairs[0];
                case PairDir.Right:
                    return m_pairs[1];
            }
            throw new Exception($"Unknown dir {dir}");
        }

        private PairDir GetChildDir(Pair child)
        {
            foreach(PairDir dir in Enum.GetValues(typeof(PairDir)))
            {
                if (GetChild(dir) == child)
                {
                    return dir;
                }
            }
            Debug.Assert(false, $"{this} could not fiind child {child}");
            return PairDir.Left;
        }

        private void AdjustHierarchy()
        {
            foreach (Pair child in m_pairs)
            {
                child.m_parent = this;
                child.m_depth = this.m_depth + 1;
                //debugWrite($"Setting depth {child.m_depth} for {child}");
                child.AdjustHierarchy();
            }
        }
        private void DoActions()
        {
            if (m_pairs.Count == 0)
            {
                return;
            }

            while(true)
            {
                AdjustHierarchy();
                debugWrite($"Current main : {this}");
                if (ReduceAction())
                {
                    continue;
                }
                if (!SplitAction())
                {
                    break;
                }
            }
        }

        private bool ReduceAction()
        {
            if (IsNumber())
            {
                return false;
            }

            bool hasHappend = false;

            if (m_depth == s_reduceDepth)
            {
                debugWrite($"Explosion : {m_depth}:{this}");

                foreach(PairDir dir in Enum.GetValues(typeof(PairDir)))
                {
                    Pair child = GetChild(dir);
                    Debug.Assert(child.IsNumber(), $"Child is not number, this : {m_depth}:{this}, child : {child.m_depth}:{child}");
                    Debug.Assert(m_parent != null, $"No parent : {this}");

                    m_parent?.ExplodeUp(dir, child.m_value, this);
                }

                m_pairs.Clear();
                m_value = 0;
                hasHappend = true;
            }
            else
            {
                hasHappend = GetChild(PairDir.Left).ReduceAction() || GetChild(PairDir.Right).ReduceAction();
            }

            return hasHappend;
        }

        private bool SplitAction()
        {
            bool hasHappend = false;

            if (IsNumber())
            {
                if (m_value >= s_splitThreshold)
                {
                    m_pairs.Add(new Pair(m_value / 2, this));
                    m_pairs.Add(new Pair((int)Math.Ceiling((float)m_value / 2), this));

                    debugWrite($"[Split] {m_value} to {m_depth}:{this}");
                    hasHappend = true;
                }
            }
            else
            {
                hasHappend = GetChild(PairDir.Left).SplitAction() || GetChild(PairDir.Right).SplitAction();
            }

            return hasHappend;
        }

        private void ExplodeUp(PairDir dir, int value, Pair child)
        {
            //debugWrite($"Explosion delivered up from {child} to {this}");
            PairDir fromDir = GetChildDir(child);

            if (fromDir == dir)
            {
                m_parent?.ExplodeUp(dir, value, this);
            }
            else
            {
                GetChild(dir).ExplodeDown(fromDir, value);
            }
        }

        private void ExplodeDown(PairDir dir, int value)
        {
            //debugWrite($"Explosion delivered down to {this}");

            if (IsNumber())
            {
                m_value += value;
                debugWrite($"Explosion affected at {m_depth}:{this} => {value}");
            }
            else
            {
                GetChild(dir).ExplodeDown(dir, value);
            }
        }
    }

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Pair? mainPair = null;
        List<string> pairLines = new List<string>();

        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                Pair pair = new Pair(new StringStream(line));

                if (mainPair == null)
                {
                    debugWrite($"Created : {pair}");
                    mainPair = pair;
                }
                else
                {
                    mainPair.Add(pair);
                    debugWrite($"Added : {pair}");
                }

                pairLines.Add(line);
            }
        }

        Console.WriteLine(mainPair);
        Console.WriteLine($"Solve 1 : {mainPair?.GetMagnitude()}");

        int solve2ans = 0;
        for (int i = 0; i < pairLines.Count - 1; i ++)
        {
            for (int k = i + 1; k < pairLines.Count; k ++)
            {
                Pair pair1 = new Pair(new StringStream(pairLines[i]));
                Pair pair2 = new Pair(new StringStream(pairLines[k]));
                Pair pair3 = new Pair(new StringStream(pairLines[k]));
                Pair pair4 = new Pair(new StringStream(pairLines[i]));

                solve2ans = Math.Max(solve2ans, pair1.Add(pair2).GetMagnitude());
                solve2ans = Math.Max(solve2ans, pair3.Add(pair4).GetMagnitude());
            }
        }

        Console.WriteLine($"Solve 2 : {solve2ans}");
    }


    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        SolveMain($"../../data/{className}.txt");
    }
    
    static void Main()
    {
        Run();
    }    
}