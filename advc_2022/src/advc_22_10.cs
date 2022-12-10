using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem10 : Advc.Utils.Loggable
    {
        public enum Operator {
            Invalid, Noop, Addx
        }

        public record Instruction(Operator op = Operator.Invalid, int val = 0);
        public static readonly Instruction NilInstruction = new();

        public class Computer : Advc.Utils.Loggable
        {
            public Instruction CurInst { get; set; } = NilInstruction;
            public int Reg { get; set; } = 1;
            public int Cycle { get; set; } = 1;

            private Queue<Instruction> Program { get; init; }
            private int SkipCycle { get; set; }

            public Computer(List<Instruction> insts)
            {
                Program = new(insts);
            }

            public bool IsRunning()
            {
                return SkipCycle > 0 || Program.Count > 0;
            }

            public void RunCycle()
            {
                Cycle ++;
                if (SkipCycle > 0)
                {
                    SkipCycle --;
                    if (SkipCycle == 0)
                    {
                        if (CurInst.op == Operator.Addx)
                        {
                            Reg += CurInst.val;
                        }
                    }
                }
                else
                {
                    CurInst = Program.Dequeue();
                    if (CurInst.op == Operator.Addx)
                    {
                        SkipCycle = 1;
                    }
                }

                LogDetail($"Cycle {Cycle} Reg {Reg} CurInst{CurInst.ToString()} SkipCycle {SkipCycle}");
            }
        }

        List<int> GetSignalStrenghts(List<Instruction> insts)
        {
            Computer com = new(insts);
            com.AllowLogDetail = false;

            List<int> strengths = new();
            strengths.Add(com.Reg);
            
            while (com.IsRunning())
            {
                LogDetail($"cycle {com.Cycle} reg {com.Reg} signal {com.Reg * com.Cycle}");
                strengths.Add(com.Reg * com.Cycle);
                com.RunCycle();
            }

            return strengths;
        }

        void DrawCRT(List<Instruction> insts)
        {
            const int lineLength = 40; 
            Computer com = new(insts);
            com.AllowLogDetail = false;

            while (com.IsRunning())
            {
                int xPos = (com.Cycle - 1) % lineLength;
                char c = '.';
                if (xPos >= com.Reg - 1 && xPos <= com.Reg + 1)
                {
                    c = '#';
                }
                Console.Write(c);

                if (xPos == (lineLength - 1))
                {
                    Console.WriteLine("");
                }
                com.RunCycle();
            }
        }        

        public int Solve1(List<Instruction> insts)
        {
            AllowLogDetail = false;
            var sigSamples = new List<int>{ 20, 60, 100, 140, 180, 220 };
            
            var list = GetSignalStrenghts(insts);

            int sum = 0;
            foreach (var sample in sigSamples)
            {
                var val = list[sample];
                sum += val;
                LogDetail($"{sample} => {val} => {sum}");
            }

            return sum;
        }

        public int Solve2(List<Instruction> insts)
        {
            AllowLogDetail = false;
            DrawCRT(insts);
            return 0;
        }
        

        public static List<Instruction> ParseInstructions(string[] lines)
        {
            List<Instruction> insts = new();
            foreach (var line in lines)
            {
                var operands = line.Split(" ");
                var cmdStr = operands[0];
                Operator op = Operator.Invalid;
                int val = 0;

                if (cmdStr == "noop")
                {
                    op = Operator.Noop;
                }
                else if (cmdStr == "addx")
                {
                    op = Operator.Addx;
                    val = int.Parse(operands[1]);
                }

                Instruction inst = new(op, val);
                insts.Add(inst);
            }
            return insts;
        }
               
        public static void Start()
        {
            var textData = File.ReadAllText("data/input10.txt");
            var lines = textData.Split(Environment.NewLine);
            var insts = ParseInstructions(lines);
            Problem10 prob1 = new();

            var ans1 = prob1.Solve1(insts);
            var ans2 = prob1.Solve2(insts);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


