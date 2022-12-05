using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class CargoStacks : Loggable
    {
        List<Stack<char>> m_cargoStacks = new();
        public CargoStacks(string textData)
        {
            var containerlines = textData.Split(Environment.NewLine);
            List<List<char>> cargoMap = new();
            foreach (var containerline in containerlines)
            {
                cargoMap.Add(new());
                var cargos = containerline.Split(",");

                foreach (var cargo in cargos)
                {
                    char cargoName = cargo[1];
                    cargoMap.Last().Add(cargoName);
                }
            }
            cargoMap.Reverse();

            for (int i = 0; i < cargoMap.First().Count; i ++)
            {
                m_cargoStacks.Add(new());
            }

            foreach (var mapLine in cargoMap)
            {
                for (int i = 0; i < mapLine.Count; i ++)
                {
                    char cargo = mapLine[i];
                    if (cargo != ' ')
                    {
                        //LogDetail($"Pushing {cargo} to {i + 1}");
                        m_cargoStacks[i].Push(cargo);
                    }
                }
            }
        }

        public Stack<char> GetCargoStack(int index)
        {
            Debug.Assert(index > 0);
            return m_cargoStacks[index - 1];
        }

        public List<char> PopCargos(int index, int count)
        {
            Debug.Assert(index > 0);
            List<char> poppedCargos = new();
            var stack = GetCargoStack(index);
            LogDetail($"Popping {count} cargos from {index}, stack count {stack.Count}");
            while (count -- > 0)
            {
                poppedCargos.Add(stack.Pop());
            }
            return poppedCargos;
        }

        public void PushCargos(int index, List<char> cargos)
        {
            Debug.Assert(index > 0);
            var stack = GetCargoStack(index);
            foreach (var c in cargos)
            {
                stack.Push(c);
            }
        }

        public void Draw()
        {
            if (!AllowLogDetail)
            {
                return;
            }
            for (int cargoIndex = 1; cargoIndex <= m_cargoStacks.Count; cargoIndex ++)
            {
                var cargoStack = GetCargoStack(cargoIndex);

                Console.Write($"Stack {cargoIndex} : ");
                foreach (var cargo in cargoStack.ToList())
                {
                    Console.Write($"{cargo},");
                }
                Console.WriteLine("");
            }
        }

        public void Move(CargoMovement move, bool reverse = false)
        {
            LogDetail($"Moving : {move}, reverse {reverse}");
            var cargos = PopCargos(move.From, move.Count);
            if (reverse)
            {
                cargos.Reverse();
            }
            PushCargos(move.To, cargos);
        }

        public string ReadTopCargos()
        {
            StringBuilder sb =  new();
            foreach (var stack in m_cargoStacks)
            {
                sb.Append(stack.First());
            }
            return sb.ToString();
        }
    }

    class CargoMovement
    {
        public int Count { get; set; }
        public int From { get; set; }
        public int To { get; set; }
        public CargoMovement(string line)
        {
            var dataArray = line.Split(" ").Select(c => c.ToString()).ToList();
            Count = int.Parse(dataArray[1]);
            From = int.Parse(dataArray[3]);
            To = int.Parse(dataArray[5]);
        }
        public override string ToString()
        {
            return $"[{From}=>{To}({Count})]";
        }
    }

    class Problem05 : Advc.Utils.Loggable
    {
        public string Solve1(CargoStacks stacks, List<CargoMovement> movements)
        {
            AllowLogDetail = false;
            int index = 0;
            foreach (var move in movements)
            {
                LogDetail($"Movement {++index}");
                stacks.Move(move);
            }

            return stacks.ReadTopCargos();
        }

        public string Solve2(CargoStacks stacks, List<CargoMovement> movements)
        {
            AllowLogDetail = false;
            int index = 0;
            foreach (var move in movements)
            {
                LogDetail($"Movement {++index}");
                stacks.Move(move, reverse: true);
            }

            return stacks.ReadTopCargos();
        }
        
        public static void Start()
        {
            var movementText = File.ReadAllText("data/input05.txt");
            var movementLines = movementText.Split(Environment.NewLine);
            List<CargoMovement> movements = new();
            foreach (var line in movementLines)
            {
                movements.Add(new CargoMovement(line));
            }

            var containerText = File.ReadAllText("data/input05a.txt");
            CargoStacks cargoStacks1 = new(containerText);
            CargoStacks cargoStacks2 = new(containerText);

            Problem05 prob1 = new();
            var ans1 = prob1.Solve1(cargoStacks1, movements);
            var ans2 = prob1.Solve2(cargoStacks2, movements);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


