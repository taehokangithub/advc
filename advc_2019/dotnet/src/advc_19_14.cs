using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

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
            return $"[{Name}:{Amount}]";
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

    class RemantBank : Advc.Utils.Loggable
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

        public void FromString(string str)
        {
            RemnantBankMap.Clear();

            if (str.Length == 0)
            {
                return;
            }
            var items = str.Split(",");

            foreach (var item in items)
            {
                var keyVal = item.Split(":");

                try
                {
                    RemnantBankMap[keyVal[0]] = long.Parse(keyVal[1]);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"[{item}] from [{str}] caused an exception ====> {e}");
                    throw;
                }
            }

            LogDetail($"from string : {this}");
        }

        public override string ToString()
        {
            StringBuilder sb = new();

            foreach (var item in RemnantBankMap)
            {
                if (item.Value > 0)
                {
                    sb.Append($"{item.Key}:{item.Value},");
                }
            }
            if (sb.Length > 0)
            {
                sb.Length--;
            }
            return sb.ToString();
        }
    }

    class IntermediateRemnantBankStateMap
    {
        public class StateOut
        {
            public string RemnantBankState { get; set; } = string.Empty;
            public long CosumedORE { get; set; } = 0;
            public static StateOut Dummy = new();
        }
        public Dictionary<string, StateOut> Dict { get; init; } = new();
        public long TotalTry { get; private set; } = 0;
        public long TotalHit { get; private set; } = 0;

        public void AddState(Ingredient ingredient, string inputStateRemnant, string outputState, long consumedORE)
        {
            if (ingredient.Name != Ingredient.FUEL)
            {
                return;
            }

            Dict.Add(
              inputStateRemnant
            , new StateOut {
                RemnantBankState = outputState,
                CosumedORE = consumedORE
            });

            if (Dict.Count % 10000 == 0)
            {
                Console.WriteLine($"[{Reactions.m_cnt}] AddState - {Dict.Count} states, hit ratio {TotalHit} / {TotalTry} = {(float)TotalHit/TotalTry}");
            }
        }

        public bool TryGetState(Ingredient ingredient, string inputStateRemnant, out StateOut outputState)
        {
            if (ingredient.Name != Ingredient.FUEL)
            {
                outputState = StateOut.Dummy;
                return false;
            }
            TotalTry ++;

            if (Dict.ContainsKey(inputStateRemnant))
            {
                outputState = Dict[inputStateRemnant];
                TotalHit ++;
                return true;
            }

            outputState = StateOut.Dummy;
            return false;
        }
    }

    class Reactions : Advc.Utils.Loggable
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
            long ore = GetRequredORE(fuelEquation.Output, remnantBank, new()) / fuelEquation.Output.Amount;
            return (int)ore;
        }

        public static int m_cnt = 0;
        public long GetFuelsByORE(long ore)
        {
            RemantBank remnantBank = new();
            remnantBank.AllowLogDetail = AllowLogDetail;
            var fuelEquation = EquationMap[Ingredient.FUEL];
            IntermediateRemnantBankStateMap dpMap = new();

            int cnt = 0;
            long oreConsumed = 0;
              while (ore > 0)
            {
                m_cnt = cnt;

                oreConsumed = GetRequredORE(fuelEquation.Output, remnantBank, dpMap);
                if (ore >= oreConsumed)
                {
                    cnt ++;
                    ore -= oreConsumed;
                }
                else
                {
                    Console.WriteLine($"[{cnt}] FIN!! ore {ore} consumed {oreConsumed} {dpMap.Dict.Count} states : {remnantBank.ToString()}");
                    break;
                }
                if (cnt % 1000 == 0)
                {
                    Console.WriteLine($"[{cnt}] ore {ore} consumed {oreConsumed} {dpMap.Dict.Count} states : {remnantBank.ToString()}");
                }
            }
            return cnt;
        }

        private long GetRequredORE(Ingredient requiredIngr, RemantBank remnantBank, IntermediateRemnantBankStateMap dpMap)
        {
            if (requiredIngr.Name == Ingredient.ORE)
            {
                return requiredIngr.Amount; // that's the thing we're looking for
            }

            string initialRemnantState = string.Empty;
#if false
            if (requiredIngr.Name == Ingredient.FUEL)
            {
                initialRemnantState = remnantBank.ToString();

                if (dpMap.TryGetState(requiredIngr, initialRemnantState, out var outputState))
                {
                    remnantBank.FromString(outputState.RemnantBankState);
                    return outputState.CosumedORE;
                }
            }
#endif
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

            long consumedORE = 0;
            {
                foreach (var ingr in eq.InputList)
                {
                    Ingredient multipliedIngr = new(ingr.Name, ingr.Amount * multiplier);
                    var amount = GetRequredORE(multipliedIngr, remnantBank, dpMap);
                    consumedORE += amount;
                }
            }

            remnantBank.Save(eq.Output.Name, remnamt);
#if false
            if (requiredIngr.Name == Ingredient.FUEL)
            {
                dpMap.AddState(requiredIngr, initialRemnantState, remnantBank.ToString(), consumedORE);
            }
#endif
            return consumedORE;
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

    class Problem14 : Advc.Utils.Loggable
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
            var textData = File.ReadAllText("../data/input14.txt");

            Advc2019_14.Reactions reactions = new(textData);

            Problem14 prob1 = new();

            var ans1 = prob1.Solve1(reactions);
            var ans2 = prob1.Solve2(reactions);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}