
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Advc20_14
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

	static bool hasBit(ulong value, int bit)
	{
		return (value & ((ulong)1 << bit)) != 0;
	}

	static ulong setBit(ulong value, int bit, bool hasBit)
	{
		ulong baseValue = ((ulong)1 << bit);
		if (hasBit)
		{
			return value | baseValue;
		}
		return value & ~baseValue;
	}
	
	static string toBinary(ulong value, int len)
	{
		return Convert.ToString((long)value, 2).PadLeft(len, '0');
	}

	class Cell
	{
		const int s_bits = 36;
		const ulong s_mask = 0XFFFFFFFFF;
		ulong m_value = 0;

		public void SetValue(ulong value, string mask)
		{
			if (mask.Length != s_bits)
			{
				throw new Exception($"Wrong mask length {mask.Length} : {mask}");
			}
			m_value = 0;
			for (int i = 0; i < s_bits; i ++)
			{
				int shift = s_bits - 1 - i;
				char maskChar = mask[i];
				bool bit;

				if (maskChar == '1')
				{
					bit = true;
				}
				else if (maskChar == '0')
				{
					bit = false;
				}
				else
				{
					bit = hasBit(value, shift);
				}

				m_value = setBit(m_value, shift, bit);
			}

			debugWrite($"mem : {this}");
		}

		public void SetValueUnchanged(ulong value)
		{
			m_value = value;
		}

		public override string ToString()
		{
			return toBinary(m_value, s_bits);
		}

		public ulong GetValue()
		{
			return m_value & s_mask;
		}
	}

	class Memory
	{
		Dictionary<ulong, Cell> m_memory = new Dictionary<ulong, Cell>();
		string m_mask = "";

		bool m_isNewDecoder = false;

		public Memory(bool isNewDecoder = false)
		{
			m_isNewDecoder = isNewDecoder;
		}

		public override string ToString()
		{
			string str = "";

			foreach (var item in m_memory)
			{	
				str += $"mem[{item.Key}] = {item.Value}\n";
			}

			return str;
		}

		public void SetLine(string line)
		{
			string[] parts = line.Split(" = ");

			if (parts[0] == "mask")
			{
				m_mask = parts[1]; 
			}
			else if (parts[0].Substring(0, 3) == "mem")
			{
				string addrStr = parts[0].Substring(4, parts[0].Length - 5);
				ulong addr = Convert.ToUInt64(addrStr);
				ulong value = Convert.ToUInt64(parts[1]);

				if (m_isNewDecoder)
				{
					SetValueForNewDecoder(addr, value);
				}
				else
				{
					GetCell(addr).SetValue(value, m_mask);
				}
			}
		}

		void SetValueForNewDecoder(ulong addr, ulong value)
		{
			var addresses = DecodeAddresses(addr, m_mask);

			foreach(ulong decodedAddr in addresses)
			{
				GetCell(decodedAddr).SetValueUnchanged(value);
			}
		}

		Cell GetCell(ulong addr)
		{
			if (!m_memory.ContainsKey(addr))
			{
				m_memory[addr] = new Cell();
			}

			return m_memory[addr];
		}

		List<ulong> DecodeAddresses(ulong addr, string mask)
		{
			List<ulong> possibleAddrs = new List<ulong>();
			List<int> floatings = new List<int>();

			ulong baseValue = 0;
			for (int i = 0; i < mask.Length; i ++)
			{
				int shift = mask.Length - 1 - i;

				char maskChar = mask[i];

				if (maskChar == '0')
				{
					baseValue = setBit(baseValue, shift, hasBit(addr, shift));
				}
				else if (maskChar == '1')
				{
					baseValue = setBit(baseValue, shift, true);
				}
				else
				{
					floatings.Add(shift);
				}
			}

			debugWrite($"Decoding \n   => {toBinary(addr, mask.Length)} : original Value\n   =? {mask} : mask\n   => {toBinary(baseValue, mask.Length)} : baseValue");

			ulong floatGen = (ulong) Math.Pow(2, floatings.Count);

			for (ulong i = 0; i < floatGen; i ++)
			{
				ulong thisValue = baseValue;

				for (int bitIdx = 0; bitIdx < floatings.Count; bitIdx ++)
				{
					int bitPos = floatings[bitIdx];

					thisValue = setBit(thisValue, bitPos, hasBit(i, bitIdx));
				}

				debugWrite($"   => {toBinary(thisValue, mask.Length)}, i {i}/{floatGen}, floatings {floatings.Count}");
				possibleAddrs.Add(thisValue);
			}

			return possibleAddrs;
		}

		public void GetSum()
		{
			ulong sum = 0;
			foreach (var item in m_memory)
			{
				sum += item.Value.GetValue();
			}
			Console.WriteLine($"Ans {(m_isNewDecoder ? 2 : 1)} = {sum}");
		}
	}
    static void solve(string path)
    {
        var lines = File.ReadLines(path);

		Memory memory1 = new Memory();
		Memory memory2 = new Memory(isNewDecoder: true);
        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
				memory1.SetLine(line);
				memory2.SetLine(line);
			}
        }

		debugWrite(memory.ToString());
		memory1.GetSum();
		memory2.GetSum();
    }

    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        solve($"../../data/{className}.txt");
    }

    static void Main()
    {
        Run();
    }    
}