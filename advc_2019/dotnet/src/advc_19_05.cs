using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Computer05
    {
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
            Halt = 99
        }

        private List<int> m_memory;
        private int m_excutionPtr = 0;
        private bool m_isHalted = false;
        private bool m_isBlockedForInput = false;
        private Queue<int> m_inputs = new();
        private Queue<int> m_output = new();

        public int Output => m_output.Dequeue();
        public int OutputCount => m_output.Count;
        public bool IsHalted => m_isHalted;
        public bool IsBlocked => m_isBlockedForInput;
        public bool AllowLogDetail { get; set; }

        public Computer05(IReadOnlyList<int> initalMemory) 
        {
            m_memory = new(initalMemory);
        }

        public void Run(Queue<int> inputs)
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

        public int GetAt(int index)
        {
            if (index >= m_memory.Count)
            {
                throw new ArgumentException($"[GetAt] Memory out of index {index} size {m_memory.Count}");
            }
            return m_memory[index];
        }

        public void SetAt(int index, int val)
        {
            if (index >= m_memory.Count)
            {
                throw new ArgumentException($"[GetAt] Memory out of index {index} size {m_memory.Count}");
            }
            m_memory[index] = val;
        }

        public void Restore(int noun, int verb)
        {
            SetAt(1, noun);
            SetAt(2, verb);
        }

        public void Dump()
        {
            Console.WriteLine($"[Dump] {string.Join(",", m_memory)}");
        }

        private void RunInstruction()
        {
            LogDetail($"[RunInstruction] Reading opcode from {m_excutionPtr}");
            string instruction = Fetch().ToString();

            string opcodeStr = instruction.Length >= 2 ? instruction.Substring(instruction.Length - 2) : instruction;
            string paramModeStr = instruction.Length > 2 ? instruction.Substring(0, instruction.Length - 2) : "";

            Opcode opcode = (Opcode) int.Parse(opcodeStr);
            Stack<char> paramMode = new(paramModeStr.ToArray());
            var fetchParam = () => {
                char paramModeChar = paramMode.Count > 0 ? paramMode.Pop() : '0';
                return paramModeChar == '1' ? Fetch() : FetchIndirect();
            };

            switch (opcode)
            {
                case Opcode.Add: 
                case Opcode.Multi:
                case Opcode.Lessthan:
                case Opcode.Equals:
                {
                    int op1 = fetchParam();
                    int op2 = fetchParam();
                    int dst = Fetch();
                    int val = (opcode == Opcode.Add) ? (op1 + op2) 
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
                    int val = m_inputs.Dequeue();
                    int dst = Fetch();
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} {val} at {dst}");
                    SetAt(dst, val);
                    break;
                }

                case Opcode.Output:
                {
                    int val = fetchParam();
                    m_output.Enqueue(val);
                    LogDetail($"[Inst] [{m_excutionPtr}] ({paramModeStr}) {(Opcode)opcode} {val}");
                    break;
                }

                case Opcode.JumpIfFalse:
                case Opcode.JumpIfTrue:
                {
                    int op1 = fetchParam();
                    int op2 = fetchParam();
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

        private int Fetch()
        {
            //LogDetail($"[Fetch] {m_excutionPtr} => {m_memory[m_excutionPtr]}");
            return m_memory[m_excutionPtr++];
        }

        private int FetchIndirect()
        {
            return GetAt(Fetch());
        }

        public void LogDetail(string str)
        {
            if (AllowLogDetail)
            {
                Console.WriteLine(str);
            }
        }
        
        public string Solve1()
        {
            Run(new(new[] { 1 }));

            return string.Join(',', m_output);
        }

        public string Solve2()
        {
            Run(new(new[] { 5 }));

            return string.Join(',', m_output);
        }
    }

    class Problem05
    {
        public static void Start() 
        {
            var textData = File.ReadAllText("../data/input05.txt");
            var textArr = textData.Split(',');
            var intList = textArr.Select(t => int.Parse(t)).ToList();
            var problem1 = new Computer05(intList);
            var problem2 = new Computer05(intList);
            var ans1 = problem1.Solve1();
            var ans2 = problem2.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


