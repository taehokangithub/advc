using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem06
    {
        private NamedTree m_tree = new();

        public Problem06(List<List<string>> pairs)
        {
            m_tree.AllowLogDetail = false;
            foreach (var pair in pairs)
            {
                m_tree.AddNode(pair[0], pair[1]);
            }
        }

        public int Solve1()
        {
            m_tree.Root.Depth = 0;

            m_tree.DFS(
                preCallback: null,
                postCallback: (node) => {
                    node.Value = node.Depth;
                    foreach (var child in node.Children)
                    {
                        node.Value += child.Value;
                    }
            });

            return m_tree.Root.Value;
        }

        public int Solve2()
        {
            const string fromName = "YOU";
            const string toName = "SAN";

            var fromNode = m_tree.GetNodeByName(fromName);
            var toNode = m_tree.GetNodeByName(toName);

            m_tree.TransformRoot(fromNode.Parent!);
            return toNode.Parent!.Depth;
        }

        public static void Start()
        {
            var textData = File.ReadAllText("../data/input06.txt");
            var textArr = textData.Split(Environment.NewLine);
            var pairList = textArr.Select(t => t.Split(')').ToList()).ToList();

            Problem06 prob = new(pairList);

            var ans1 = prob.Solve1();
            var ans2 = prob.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


