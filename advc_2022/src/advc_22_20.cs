using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem20 : Loggable
    {
        class NumContainer : Loggable
        {
            private LinkedList<long> m_list;
            private LinkedListNode<long> m_zeroNode;
            private int m_nodeCount;
            public NumContainer(List<long> data)
            {
                m_list = new(data);
                m_zeroNode = m_list.Find(0)!;
                m_nodeCount = data.Count;
            }

            public void MultipleMix(int times)
            {
                var initialList = MakeInitialList();
                for (int i = 0; i < times; i ++)
                {
                    MixInternal(initialList);
                }
            }

            public void SingleMix()
            {
                MixInternal(MakeInitialList());
            }

            private IReadOnlyList<LinkedListNode<long>> MakeInitialList()
            {
                List<LinkedListNode<long>> listOfNodes = new();
                for (var node = m_list.First; node != null; node = node.Next)
                {
                    listOfNodes.Add(node);
                }
                return listOfNodes;
            }

            public void MixInternal(IReadOnlyList<LinkedListNode<long>> listOfNodes)
            {
                LogDetail($"Initial List : {string.Join(",", m_list)}");
                
                int maxMove = m_nodeCount - 1;
                foreach (var node in listOfNodes)
                {
                    long addMultiple = 1 + 10 * Math.Abs(node.Value) / m_nodeCount;
                    long moveCnt = (node.Value + addMultiple * maxMove) % maxMove;

                    var addBefore = node.Next;
                    m_list.Remove(node);

                    for (int i = 0; i < moveCnt; i ++)
                    {
                        addBefore = (addBefore == null) ? m_list.First!.Next! : addBefore.Next;
                    }

                    if (addBefore == null)
                    {
                        m_list.AddLast(node);
                    }
                    else
                    {
                        m_list.AddBefore(addBefore, node);
                    }

                    //LogDetail($"After moving {nodeIndex} ({listOfNodes[nodeIndex++].Value.ToString().PadLeft(3)}) {moveCnt} times : {string.Join(",", m_list)}");
                }
            }

            public long GetGroveCoordinates()
            {
                long a = GetAtFrom(m_zeroNode, 1000);
                long b = GetAtFrom(m_zeroNode, 2000);
                long c = GetAtFrom(m_zeroNode, 3000);
                LogDetail($"{a} + {b} + {c}");
                return a + b + c;
            }

            public long GetAtFrom(LinkedListNode<long> from, int offset)
            {
                offset = offset % (m_nodeCount);
                LinkedListNode<long> node = from;
                for (int i = 0; i < offset; i ++)
                {
                    node = node.Next == null ? m_list.First!: node.Next;
                }
                return node.Value;
            }
        }
        public long Solve1(List<long> arr)
        {
            NumContainer nc = new(arr);

            nc.AllowLogDetail = AllowLogDetail = false;
            nc.SingleMix();
            return nc.GetGroveCoordinates();
        }

        public long Solve2(List<long> arr)
        {
            const int numMix = 10;
            const int decryptionKey = 811589153;

            NumContainer nc = new(arr.Select(s => s * decryptionKey).ToList());
            AllowLogDetail = nc.AllowLogDetail = false;
            nc.MultipleMix(numMix);

            return nc.GetGroveCoordinates();
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input20.txt");
            var lines = textData.Split(Environment.NewLine);
            var intList = lines.Select(t => long.Parse(t)).ToList();

            Problem20 prob1 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = prob1.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


