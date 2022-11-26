using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Computer09
    {
        private enum FetchMode
        {
            Indirect = 0,
            Direct = 1,
            Relative = 2
        }
        private enum Opcode 
        {
            Add = 1,
            Multi = 2,
            Input = 3,
            Output = 4,
            JumpIfTrue = 5,
            JumpIfFalse = 6,
            Lessthan = 7,
            Equals = 8,
            SetRelativeBase = 9,
            Halt = 99
        }

        private Dictionary<long, long> m_memory = new();
        private long m_excutionPtr = 0;
        private long m_relativeBase = 0;
        private bool m_isHalted = false;
        private bool m_isBlockedForInput = false;
        private Queue<long> m_inputs = new();
        private Queue<long> m_output = new();

        public long Output => m_output.Dequeue();
        public int OutputCount => m_output.Count;
        public bool IsHalted => m_isHalted;
        public bool IsBlocked => m_isBlockedForInput;
        public bool AllowLogDetail { get; set; } = true;

        public Computer09(IReadOnlyList<long> initalMemory) 
        {
            long address = 0;
            foreach (var val in initalMemory)
            {
                m_memory.Add(address++, val);
            }
        }

        public void Run(long input)
        {
            Queue<long> inputs = new();
            inputs.Enqueue(input);
            Run(inputs);
        }
        public void Run(Queue<long> inputs)
        {
            m_inputs = inputs;
            m_isBlockedForInput = false;
            LogDetail($"Program started with input {string.Join(",",inputs)} at {m_excutionPtr}");

            while (!m_isHalted && !m_isBlockedForInput)
            {
                RunInstruction();
            }
            LogDetail($"Program running finished");
        }

        public long GetAt(long index)
        {
            return m_memory[index];
        }

        public void SetAt(long index, long val)
        {
            m_memory[index] = val;
        }

        public void Dump()
        {
            foreach (var pair in m_memory)
            {
                Console.WriteLine($"[{pair.Key}] {pair.Value}");
            }
        }

        private void RunInstruction()
        {
            LogDetail($"[RunInstruction] Reading opcode from {m_excutionPtr}");
            string instruction = Fetch().ToString();

            string opcodeStr = instruction.Length >= 2 ? instruction.Substring(instruction.Length - 2) : instruction;
            string paramModeStr = instruction.Length > 2 ? instruction.Substring(0, instruction.Length - 2) : "";

            Opcode opcode = (Opcode) int.Parse(opcodeStr);
            Stack<char> paramMode = new(paramModeStr.ToArray());

            var getNextParamMode = () => {
                char paramModeChar = paramMode.Count > 0 ? paramMode.Pop() : '0';
                return (FetchMode) int.Parse(paramModeChar.ToString());
            };
            
            var fetchParam = () => {
                return FetchParameter(getNextParamMode());
            };

            var fetchDest = () => {
                return FetchDest(getNextParamMode());
            };

            switch (opcode)
            {
                case Opcode.Add: 
                case Opcode.Multi:
                case Opcode.Lessthan:
                case Opcode.Equals:
                {
                    var op1 = fetchParam();
                    var op2 = fetchParam();
                    var dst = fetchDest();
                    var val = (opcode == Opcode.Add) ? (op1 + op2) 
                            : (opcode == Opcode.Multi) ? (op1 * op2)
                            : (opcode == Opcode.Equals) ? (op1 == op2 ? 1 : 0)
                            : (opcode == Opcode.Lessthan) ? (op1 < op2 ? 1 : 0)
                            : throw new Exception($"Unknown opcode {opcode}");

                    LogDetail($"[Inst] [{m_excutionPtr}] {(Opcode)opcode} ({paramModeStr}) {op1} {op2} => {val} at {dst}");
                    SetAt(dst, val);
                    break; 
                }

                case Opcode.Input:
                {
                    if (m_inputs.Count == 0)
                    { 
                        m_isBlockedForInput = true;
                        m_excutionPtr--;
                        break;
                    }
                    var val = m_inputs.Dequeue();
                    var dst = fetchDest();
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} {val} at {dst}");
                    SetAt(dst, val);
                    break;
                }

                case Opcode.Output:
                {
                    var val = fetchParam();
                    m_output.Enqueue(val);
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} {val}");
                    break;
                }

                case Opcode.JumpIfFalse:
                case Opcode.JumpIfTrue:
                {
                    var op1 = fetchParam();
                    var op2 = fetchParam();
                    bool shouldJump = (opcode == Opcode.JumpIfFalse ? (op1 == 0) 
                                    : (opcode == Opcode.JumpIfTrue ? (op1 != 0)
                                    : throw new Exception($"Unknown opcode for jump : {opcode}")));
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} : {shouldJump}");
                    if (shouldJump)
                    {
                        m_excutionPtr = op2;
                    }
                    break;
                }

                case Opcode.SetRelativeBase:
                {
                    var op = fetchParam();
                    m_relativeBase += op;
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} : {m_relativeBase}");
                    break;
                }

                case Opcode.Halt:
                {
                    LogDetail($"[Inst] Halting program");
                    m_isHalted = true;
                    break;
                }
                default:
                    throw new InvalidDataException($"[RunInstruction] unknown opcode {instruction}, at {m_excutionPtr}");
            }
        }

        private long FetchParameter(FetchMode fetchMode)
        {
            switch (fetchMode)
            {
                case FetchMode.Indirect: return FetchIndirect();
                case FetchMode.Direct: return Fetch();
                case FetchMode.Relative: return FetchRelative();
            }

            throw new ArgumentException($"Unknown fetch mode {fetchMode}");
        }

        private long FetchDest(FetchMode fetchMode)
        {
            switch (fetchMode)
            {
                case FetchMode.Direct: throw new InvalidOperationException("Destination can't be direct mode");
                case FetchMode.Indirect: return Fetch();
                case FetchMode.Relative: return Fetch() + m_relativeBase;
            }

            throw new ArgumentException($"Unknown fetch mode {fetchMode}");
        }

        private long Fetch()
        {
            LogDetail($"[Fetch] {m_excutionPtr} => {m_memory[m_excutionPtr]}");
            return m_memory[m_excutionPtr++];
        }

        private long FetchIndirect()
        {
            return GetAt(Fetch());
        }

        private long FetchRelative()
        {
            var orgVal = Fetch();
            var targetAddr = orgVal + m_relativeBase;
            var result = GetAt(targetAddr);
            LogDetail($"[FetchRelative] ({orgVal} + {m_relativeBase}) = {targetAddr} => {result}");
            return result;
        }

        public void LogDetail(string str)
        {
            if (AllowLogDetail)
            {
                Console.WriteLine(str);
            }
        }
    }

    class Problem09
    {
        public static long RunComputerByValue(IReadOnlyList<long> initialMemory, int input)
        {
            var computer = new Computer09(initialMemory);

            computer.AllowLogDetail = false;

            computer.Run(input);

            long ans = 0;

            while (computer.OutputCount > 0)
            {
                ans = computer.Output;
                Console.WriteLine($"[Solve] output {ans}");
            }

            return ans;
        }
        public static long Solve1(IReadOnlyList<long> initialMemory)
        {
            return RunComputerByValue(initialMemory, 1);
        }
        public static long Solve2(IReadOnlyList<long> initialMemory)
        {
            return RunComputerByValue(initialMemory, 2);
        }

        public static void Start() 
        {
            var textData = File.ReadAllText("data/input09.txt");
            var textArr = textData.Split(',');
            var intList = textArr.Select(t => long.Parse(t)).ToList();
            long ans1 = Solve1(intList);
            long ans2 = Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}
