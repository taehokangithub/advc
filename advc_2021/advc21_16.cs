
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_16
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    static string s_inputSampleLiteral = "D2FE28";
    static string s_inputSampleSubPacket1 = "38006F45291200";
    static string s_inputSampleSubPacket2 = "EE00D40C823060";
    static string s_inputSample = "8A004A801A8002F478620080001611562C8802118E34C0015000016115A2E0802F182340A0016C880162017C3686B18A3D4780";
    static string s_input = "220D4B80491FE6FBDCDA61F23F1D9B763004A7C128012F9DA88CE27B000B30F4804D49CD515380352100763DC5E8EC000844338B10B667A1E60094B7BE8D600ACE774DF39DD364979F67A9AC0D1802B2A41401354F6BF1DC0627B15EC5CCC01694F5BABFC00964E93C95CF080263F0046741A740A76B704300824926693274BE7CC880267D00464852484A5F74520005D65A1EAD2334A700BA4EA41256E4BBBD8DC0999FC3A97286C20164B4FF14A93FD2947494E683E752E49B2737DF7C4080181973496509A5B9A8D37B7C300434016920D9EAEF16AEC0A4AB7DF5B1C01C933B9AAF19E1818027A00A80021F1FA0E43400043E174638572B984B066401D3E802735A4A9ECE371789685AB3E0E800725333EFFBB4B8D131A9F39ED413A1720058F339EE32052D48EC4E5EC3A6006CC2B4BE6FF3F40017A0E4D522226009CA676A7600980021F1921446700042A23C368B713CC015E007324A38DF30BB30533D001200F3E7AC33A00A4F73149558E7B98A4AACC402660803D1EA1045C1006E2CC668EC200F4568A5104802B7D004A53819327531FE607E118803B260F371D02CAEA3486050004EE3006A1E463858600F46D8531E08010987B1BE251002013445345C600B4F67617400D14F61867B39AA38018F8C05E430163C6004980126005B801CC0417080106005000CB4002D7A801AA0062007BC0019608018A004A002B880057CEF5604016827238DFDCC8048B9AF135802400087C32893120401C8D90463E280513D62991EE5CA543A6B75892CB639D503004F00353100662FC498AA00084C6485B1D25044C0139975D004A5EB5E52AC7233294006867F9EE6BA2115E47D7867458401424E354B36CDAFCAB34CBC2008BF2F2BA5CC646E57D4C62E41279E7F37961ACC015B005A5EFF884CBDFF10F9BFF438C014A007D67AE0529DED3901D9CD50B5C0108B13BAFD6070";

    class BitStream
    {
        Queue<byte> m_buf = new Queue<byte>();
        int m_bitIdx = 0;
        byte m_curByte = 0;

        const int s_maxBits = 8;

        public BitStream(string input)
        {
            for (int i = 0; i < input.Length; i += 2)
            {
                string hex = input.Substring(i, 2);
                int val = Convert.ToInt32(hex, 16);
                m_buf.Enqueue((byte) (val & 0xFF));
            }

            m_curByte = m_buf.Dequeue();
        }

        public BitStream(BitStream parent, int numBits)
        {
            m_bitIdx = parent.m_bitIdx;
            m_curByte = parent.m_curByte;

            int takenBits = s_maxBits - m_bitIdx;
            numBits -= takenBits;
            parent.GetNextBits(takenBits);

            while (numBits >= s_maxBits)
            {
                numBits -= s_maxBits;
                m_buf.Enqueue(parent.m_buf.Dequeue());
            }

            // final byte
            if (numBits > 0)
            {
                m_buf.Enqueue(parent.m_buf.Peek());
                parent.GetNextBits(numBits);
            }
            
        }

        public int GetRemainingBits()
        {
            return (s_maxBits - m_bitIdx) + m_buf.Count() * s_maxBits;
        }

        public override string ToString()
        {
            string ret = "";
            ret += $"[{m_buf.Count()}, at({m_bitIdx}), total({GetRemainingBits()})]";
            return ret;
        }

        public BitStream SpawnSubStream(int numBits)
        {
            BitStream subStream = new BitStream(this, numBits);

            return subStream;
        }

        public int GetNextBits(int n)
        {
            int ret = 0;

            for (int i = n - 1; i >= 0; i --)
            {
                int bit = GetCurrentBit();
                ret |= (bit << i);
            }

            return ret;
        }

        public void DiscardByte()
        {
            m_bitIdx = s_maxBits;
        }

        public bool IsEmpty()
        {
            return m_buf.Count == 0;
        }

        int GetCurrentBit()
        {
            if (m_bitIdx >= s_maxBits)
            {
                m_bitIdx = 0;
                m_curByte = m_buf.Dequeue();
            }
            int ret = (m_curByte & (1 << (8 - m_bitIdx - 1))) == 0 ? 0 : 1;
            m_bitIdx ++;
            return ret;
        }
    }

    class PacketDefs 
    {
        public const int s_versionLength = 3;
        public const int s_typeLength = 3;
        public const int s_literalLength = 4;
        public const int s_literalLastBitLength = 1;
        public const int s_operatorLengthTypeLength = 1;
        public const int s_totalLengthBitsLength = 15;
        public const int s_totalSubPacketBitsLength = 11;

        public enum PacketType 
        {
            SUM = 0,
            PRODUCT = 1,
            MINIMUM = 2,
            MAXIMUM = 3,
            LITERAL = 4,
            GREATOR = 5,
            LESS = 6,
            EQUAL = 7

        }
    }

    class Packet
    {
        byte m_version;
        byte m_packetType;

        List<Packet> m_subPackets = new List<Packet>();
        List<byte> m_literals = new List<byte>();
        ulong m_literalValue = 0;


        public Packet(BitStream stream)
        {
            debugWrite($"Decoding start : {stream}");
            Decode(stream);
        }

        public int GetAllVersions()
        {
            int ret = m_version;

            foreach (Packet packet in m_subPackets)
            {
                ret += packet.GetAllVersions();
            }

            return ret;
        }

        public long Evaluate()
        {
            long result = 0;
            var packetType = (PacketDefs.PacketType) m_packetType;

            switch (packetType)
            {
                case PacketDefs.PacketType.LITERAL:
                    result = (long) m_literalValue;
                    break;

                case PacketDefs.PacketType.SUM : 
                case PacketDefs.PacketType.PRODUCT :
                case PacketDefs.PacketType.MINIMUM :
                case PacketDefs.PacketType.MAXIMUM : 
                    result = EvaluateArithmatic();
                    break;

                case PacketDefs.PacketType.GREATOR : 
                case PacketDefs.PacketType.LESS : 
                case PacketDefs.PacketType.EQUAL :
                    result = EvaluateBoolean();
                    break;

                default:   
                    throw new Exception($"Unhandled packet type {m_packetType}");
            }

            debugWrite($"Evaluation {packetType} = {result}");
            return result;
        }

        long EvaluateArithmatic()
        {
            long result = 0;

            switch ((PacketDefs.PacketType) m_packetType)
            {
                case PacketDefs.PacketType.PRODUCT : result = 1; break;
                case PacketDefs.PacketType.MINIMUM : result = int.MaxValue; break;
            }

            foreach (Packet packet in m_subPackets)
            {
                long subResult = packet.Evaluate();

                switch ((PacketDefs.PacketType) m_packetType)
                {
                    case PacketDefs.PacketType.SUM : result += subResult; break;
                    case PacketDefs.PacketType.PRODUCT : result *= subResult; break;
                    case PacketDefs.PacketType.MINIMUM : result = Math.Min(result, subResult); break;
                    case PacketDefs.PacketType.MAXIMUM : result = Math.Max(result, subResult); break;
                    default:
                        throw new Exception($"Unhandled type {m_packetType}");

                }                
            }

            return result;
        }

        long EvaluateBoolean()
        {
            if (m_subPackets.Count != 2)
            {
                throw new Exception($"Boolean rerquires 2 subPackets, but found {m_subPackets.Count}, type {m_packetType}");
            }

            long a = m_subPackets[0].Evaluate();
            long b = m_subPackets[1].Evaluate();

            switch ((PacketDefs.PacketType) m_packetType)
            {
                case PacketDefs.PacketType.GREATOR : return Convert.ToInt32(a > b);
                case PacketDefs.PacketType.LESS : return Convert.ToInt32(a < b);
                case PacketDefs.PacketType.EQUAL : return Convert.ToInt32(a == b);
            }

            throw new Exception($"Unhandled packet type {m_packetType}");
        }

        void Decode(BitStream stream)
        {
            m_version = (byte)stream.GetNextBits(PacketDefs.s_versionLength);
            m_packetType = (byte)stream.GetNextBits(PacketDefs.s_typeLength);

            debugWrite($"Packet [Ver:{m_version}] [Type:{m_packetType}]");

            if (m_packetType == (byte)PacketDefs.PacketType.LITERAL)
            {
                DecodeLiteralPacket(stream);
            }
            else
            {
                DecodeOperatorPacket(stream);
            }
        }

        void DecodeLiteralPacket(BitStream stream)
        {
            bool isLast = false;
            while (!isLast)
            {
                isLast = !Convert.ToBoolean(stream.GetNextBits(PacketDefs.s_literalLastBitLength));
                byte b = (byte) stream.GetNextBits(PacketDefs.s_literalLength);
                m_literals.Add(b);
            }

            m_literalValue = 0;

            for (int i = 0; i < m_literals.Count; i ++)
            {
                byte l = m_literals[i];
                int bitsToMove = (PacketDefs.s_literalLength * (m_literals.Count - 1 - i));
                ulong currentLiteral = ((ulong)l << bitsToMove);
                m_literalValue += currentLiteral;

                debugWrite($"   => Adding value [{i}:{l}:{bitsToMove}] as {currentLiteral} => {m_literalValue}");
            }

            debugWrite($"   => Literal {m_literalValue}");
        }

        void DecodeOperatorPacket(BitStream stream)
        {
            int lengthType = stream.GetNextBits(PacketDefs.s_operatorLengthTypeLength);

            if (lengthType == 0)
            {
                int totalLength = stream.GetNextBits(PacketDefs.s_totalLengthBitsLength);

                debugWrite($"   ==> Found subStream length {totalLength}");

                BitStream subStream = new BitStream(stream, totalLength);

                while(!subStream.IsEmpty())
                {
                    Packet subPacket = new Packet(subStream);
                    m_subPackets.Add(subPacket);
                }
            }
            else if (lengthType == 1)
            {
                int totalPackets = stream.GetNextBits(PacketDefs.s_totalSubPacketBitsLength);

                debugWrite($"   => Found subStream count {totalPackets}");

                for (int i = 0; i < totalPackets; i ++)
                {
                    Packet subPacket = new Packet(stream);
                    m_subPackets.Add(subPacket);
                }
            }
            else
            {
                throw new Exception($"Length type {lengthType}");
            }
        }
    }

    static void SolvePart1(List<Packet> packets)
    {
        int ans = 0;
        foreach(Packet packet in packets)
        {
            ans += packet.GetAllVersions();
        }

        Console.WriteLine($"Solve 1 ans : {ans}");
    }

    static void SolvePart2(List<Packet> packets)
    {
        long ans = 0;
        foreach(Packet packet in packets)
        {
            long oneAnswer = packet.Evaluate();
            ans += oneAnswer;
        }

        Console.WriteLine($"Solve 2 ans : {ans}");        
    }

    static void SolveMain(string input)
    {
        BitStream stream = new BitStream(input);

        Console.WriteLine($"Decoding {input}");

        List<Packet> packets = new List<Packet>();

        while(!stream.IsEmpty())
        {
            Packet packet = new Packet(stream);
            packets.Add(packet);
            stream.DiscardByte();    // only top-level packets can discard byte?
        }

        SolvePart1(packets);
        SolvePart2(packets);
    }


    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();
        
        SolveMain(s_input); // 61182482683 too low
    }

    static void Main()
    {
        Run();
    }    
}