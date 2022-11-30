using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019_14
{
    class Ingredient
    {
        public const string FUEL = "FUEL";
        public const string ORE = "ORE";
        public string Name { get; private set; }
        public long Amount { get; private set; }

        public Ingredient(string str)
        {
            var elements = str.Split(" ");
            Name = elements[1];
            Amount = int.Parse(elements[0]);
        }

        public Ingredient(string name, long amount)
        {
            Name = name;
            Amount = amount;
        }

        public override string ToString()
        {
            string isFuelMark = (Name == FUEL) ? "*" : "";
            return $"[{isFuelMark}{Name}{isFuelMark}:{Amount}]";
        }
    }

    class Equation
    {
        public List<Ingredient> InputList { get; private set; } = new();
        public Ingredient Output { get; private set; }
        public Equation(string textLine)
        {
            var equation = textLine.Split(" => ");
            var inputIngrList = equation[0].Split(", ");
            var outputIngr = equation[1];

            foreach (var intputIngr in inputIngrList)
            {
                InputList.Add(new(intputIngr));
            }
            Output = new(outputIngr);
        }

        public override string ToString()
        {
            return $"{string.Join(",", InputList)} = {Output}";
        }
    }

    class RemantBank : Advc.Utils.LogUtils.Loggable
    {
        private Dictionary<string, long> RemnantBankMap { get; set; } = new();
        public void Save(string name, long amount)
        {
            if (amount == 0)
            {
                return;
            }
            if (!RemnantBankMap.ContainsKey(name))
            {
                RemnantBankMap[name] = 0;
            }
            RemnantBankMap[name] += amount;

            LogDetail($"[Bank] saving {name} {amount}, balance {RemnantBankMap[name]}");
        }

        public bool Withdraw(string name, long amount)
        {
            if (RemnantBankMap.ContainsKey(name))
            {
                if (RemnantBankMap[name] >= amount)
                {
                    RemnantBankMap[name] -= amount;

                    LogDetail($"[Bank] Using remnant, {name} {amount}, remaining {RemnantBankMap[name]}");
                    return true;
                }
            }
            return false;
        }

        public long CheckBalance(string name)
        {
            return (RemnantBankMap.ContainsKey(name) ? RemnantBankMap[name] : 0);
        }

        public void MultipliBalance(long multiplier)
        {
            foreach (var item in RemnantBankMap)
            {
                RemnantBankMap[item.Key] *= multiplier;
            }
        }

        public long RoughlyTotalBalance()
        {
            long ans = 0;
            foreach (var item in RemnantBankMap)
            {
                ans += RemnantBankMap[item.Key];
            }  
            return ans;
        }

        public void PrintBalance()
        {
            LogDetail($"-----------------------------------");
            foreach (var item in RemnantBankMap)
            {
                LogDetail($"[Bank] balance {item.Key} {item.Value}");
            }
        }
    }

    class Reactions : Advc.Utils.LogUtils.Loggable
    {
        private Dictionary<string, Equation> EquationMap { get; set; } = new();

        public Reactions(string textData)
        {
            var eqList = textData.Split(Environment.NewLine);

            foreach(var equation in eqList)
            {
                var eq = new Equation(equation);
                EquationMap[eq.Output.Name] = eq;
            }
        }

        public long GetRequredOREForFuel()
        {
            RemantBank remnantBank = new();
            remnantBank.AllowLogDetail = AllowLogDetail;
            var fuelEquation = EquationMap[Ingredient.FUEL];
            long ore = GetRequredORE(fuelEquation.Output, remnantBank) / fuelEquation.Output.Amount;
            remnantBank.PrintBalance();
            return (int)ore;
        }

        public long GetFuelsByORE(long ore)
        {
            RemantBank remnantBank = new();
            remnantBank.AllowLogDetail = AllowLogDetail;
            var fuelEquation = EquationMap[Ingredient.FUEL];

            long cnt = 0;
            long oreConsumed = GetRequredORE(fuelEquation.Output, remnantBank);
            long expectedBatches = ore / oreConsumed;
            expectedBatches = (long) (expectedBatches * 0.99);

            
            ore -= oreConsumed * expectedBatches; 
            Console.WriteLine($"expectedBatches {expectedBatches} oreConsumed {oreConsumed} ORE {ore} {oreConsumed * expectedBatches}");
            cnt += expectedBatches;
            remnantBank.MultipliBalance(expectedBatches);
            
            Console.WriteLine($"starting final loop with {ore}, cur cnt {cnt}");

            while (ore > 0)
            {
                oreConsumed = GetRequredORE(fuelEquation.Output, remnantBank);
                if (ore >= oreConsumed)
                {
                    cnt ++;
                    ore -= oreConsumed;
                }
                else
                {
                    Console.WriteLine($"[{cnt}] FIN!! ore {ore} consumed {oreConsumed} balance {remnantBank.RoughlyTotalBalance()}");
                    break;
                }

                if (cnt % 10000 == 0)
                {
                    Console.WriteLine($"[{cnt}] ore {ore} consumed {oreConsumed} balance {remnantBank.RoughlyTotalBalance()}");
                }
            }
            
            return cnt;
        }        

        private long GetRequredORE(Ingredient requiredIngr, RemantBank remnantBank)
        {
            if (requiredIngr.Name == Ingredient.ORE)
            {
                return requiredIngr.Amount; // that's the thing we're looking for
            }
            
            if (remnantBank.Withdraw(requiredIngr.Name, requiredIngr.Amount))
            {
                LogDetail($"[GetRequiredORE] Returning 0 after withdrawing {requiredIngr} from bank");
                return 0; // Used remnant, no cost
            }

            var eq = EquationMap[requiredIngr.Name];
            Debug.Assert(eq != null);

            var getMultipler = (long requiredAmount) => (requiredAmount / eq.Output.Amount) + (requiredAmount % eq.Output.Amount == 0 ? 0 : 1);

            // A has equation : 10 ORE => 10 A
            var multiplier = getMultipler(requiredIngr.Amount);
            var remnamt = eq.Output.Amount * multiplier - requiredIngr.Amount;
            var lastBit = eq.Output.Amount - remnamt;
            long bankBalance = remnantBank.CheckBalance(requiredIngr.Name);

            if (bankBalance >= lastBit)
            {
                LogDetail($"[GetRequiredORE] special withdrawal for renmant {remnamt} last bit {lastBit}, bankBalance {bankBalance}, multiplier {multiplier}");
                remnantBank.Withdraw(requiredIngr.Name, lastBit);
                multiplier --;
                remnamt = 0;
            }

            LogDetail($"[GetRequiredORE] required {requiredIngr}, equation {eq}, multiplier {multiplier} (renmant {remnamt} last bit {lastBit}, bankBalance {bankBalance})");

            if (multiplier == 0)
            {
                LogDetail($"[GetRequiredORE] Returning 0 for 0 multiplier");
                return  0;
            }

            long ore = 0;
            for (int i = 0; i < multiplier; i ++)
            {
                foreach (var ingr in eq.InputList)
                {
                    var amount = GetRequredORE(ingr, remnantBank);
                    ore += amount;

                    if (i > 1000 && (i % 1000 == 0))
                    {
                        LogDetail($"{eq} {ingr} batch {i}/{multiplier} amount {amount} ore {ore}");
                    }
                }
            }
            
            remnantBank.Save(eq.Output.Name, remnamt);
            return ore;
        }

        public void PrintAllEquations()
        {
            foreach (var eqPair in EquationMap)
            {
                Console.WriteLine(eqPair.Value);
            }
        }
    }
}

namespace Advc2019
{

    class Problem14 : Advc.Utils.LogUtils.Loggable
    {
        public long Solve1(Advc2019_14.Reactions reactions)
        {
            reactions.AllowLogDetail = false;
            return reactions.GetRequredOREForFuel();
        }

        public long Solve2(Advc2019_14.Reactions reactions)
        {
            return reactions.GetFuelsByORE(1000000000000);
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input14-s2.txt");
            
            Advc2019_14.Reactions reactions = new(textData);

            Problem14 prob1 = new();

            var ans1 = prob1.Solve1(reactions);
            var ans2 = prob1.Solve2(reactions);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


// 3992600 low