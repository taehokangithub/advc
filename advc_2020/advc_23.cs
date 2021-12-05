#pragma warning disable CS8625
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.IO;

class Advc20_06
{
	const string s_input = "193467258";
	const string s_testInput = "389125467";
	const int s_chunkSize = 3;

	class Node
	{
		public int Val { get; private set; }
		public Node Next { get; set; } = null;

		public Node(int val)
		{
			Val = val;
		}
		public override string ToString()
		{
			return "[" + Val.ToString() + "]";
		}
	}

	class Chunk
	{
		public Node Head{ get;  private set; }
		public Node Tail{ get; private set; }


		public Chunk(Node head)
		{
			Head = head;

			for (int i = 0 ; i < s_chunkSize - 1; i ++)
			{
				head = head.Next;
			}

			Tail = head;
		}

		public Node CutTail()
		{
			Node ret = Tail.Next;
			Tail.Next = null;
			return ret;
		}

		public void AttachAfter(Node node)
		{
			Tail.Next = node.Next;
			node.Next = Head;
			//Console.WriteLine($"Attaching : {node} {Head}");
		}

		public int FindPossibleDestination(int val)
		{
			bool found;

			do
			{
				found = false;
				for (Node p = Head; p != null; p = p.Next)
				{
					if (p.Val == val)
					{
						found = true;
						val = CircularList.DecreaseVal(val);
						break;
					}
				}
			}
			while(found);

			return val;
		}
	}

	class CircularList
	{
		private Node Head{ get; set; } = null;
		private Node Tail{ get; set; } = null;

		private static int m_minVal = Int32.MaxValue;
		private static int m_maxVal = Int32.MinValue;
		private int m_count = 0;
		private Dictionary<int,Node> m_nodeMap = new Dictionary<int, Node>();

		public void AddNode(int val)
		{
			Node newNode = new Node(val);
			m_maxVal = Math.Max(m_maxVal, val);
			m_minVal = Math.Min(m_minVal, val);
			m_count ++;

			m_nodeMap[val] = newNode;

			if (m_count % 100000 == 0)
			{
				Console.WriteLine($"Adding node {val}");
			}

			if (Head != null)
			{
				Tail.Next = newNode;
				Tail = newNode;
				newNode.Next = Head;
			}
			else
			{
				Tail = Head = newNode.Next = newNode;
			}
		}
		public void PrintAll()
		{
			Console.Write("Circular List : ");
			Console.Write(Head);

			for (Node p = Head.Next; p != null && p != Head; p = p.Next)
			{
				Console.Write("->" + p);
			}

			Console.WriteLine("\n");
		}

		public void PrintAfter(int val)
		{
			Console.Write("Circular List After 1 : ");

			Node from = FindNode(val);

			for (Node p = from.Next; p != null && p != from; p = p.Next)
			{
				Console.Write(p.Val);
			}

			Console.Write("\n");
		}

		public static int DecreaseVal(int val)
		{
			val --;
			if (val < m_minVal)
			{
				val = m_maxVal;
			}
			return val;
		}

		private Node FindNode(int val)
		{
			return m_nodeMap[val];
		}

		public void Move()
		{
			Chunk chunk = new Chunk(Head.Next);
			Head.Next = chunk.CutTail();

			int destVal = chunk.FindPossibleDestination(CircularList.DecreaseVal(Head.Val));

			Node destNode = FindNode(destVal);
			
			chunk.AttachAfter(destNode);
			Head = Head.Next;
		}

		public void SolvePart1()
		{
			int moves = 100;
			for (int i = 0; i < moves; i ++)
			{
				Move();
				PrintAll();
			}

			PrintAfter(1);
		}

		public void SolvePart2()
		{
			const int totalNodes = 1000000;
			const int totalMoves = 10000000;

			while (m_count < totalNodes)
			{
				AddNode(m_maxVal + 1);
			}

			Console.WriteLine($"Added {m_count} nodes");

			for (int i = 0; i < totalMoves; i ++)
			{
				Move();

				if ((i + 1) % 100000 == 0)
				{
					Console.WriteLine($"Move {i + 1}");
				}
			}

			Console.WriteLine($"Finished {totalMoves} moves");

			Node one = FindNode(1);

			Node oneNext = one.Next;
			Node oneNextNext = one.Next.Next;

			Console.WriteLine($"{oneNext.Val} * {oneNextNext.Val} = {(long)oneNext.Val * oneNextNext.Val}");
		}
		
	}

	static void Run()
	{
		Console.WriteLine("Starting Advc20_23");

		CircularList list1 = new CircularList();
		CircularList list2 = new CircularList();

		foreach (char c in s_input)
		{
			int val = c - '0';
			list1.AddNode(val);
			list2.AddNode(val);
		}

		list1.SolvePart1();
		list2.SolvePart2();
	}

	static void Main()
	{
		Run();
	}
}