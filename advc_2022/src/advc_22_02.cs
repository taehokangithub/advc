using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2022
{
    class Problem02 : Advc.Utils.LogUtils.Loggable
    {
        public enum Shape { Rock, Paper, Scissor }
        public enum Result { Win, Draw, Lose }
        public record Plan(Shape shape, Result result);

        private static Result GetResult(Shape opp, Shape  mine)
        {
            if (opp == mine)
            {
                return Result.Draw;
            }
            if (opp == Shape.Rock)
            {
                return (mine == Shape.Paper) ? Result.Win : Result.Lose;
            }
            else if (opp == Shape.Scissor)
            {
                return (mine == Shape.Rock) ? Result.Win : Result.Lose;
            }
            else if (opp == Shape.Paper)
            {
                return (mine == Shape.Scissor) ? Result.Win : Result.Lose;
            }
            throw new Exception($"Unknown opp shape {opp}");
        }

        private static Shape GetShapeForResult(Shape opp, Result result)
        {
            foreach (var shape in Enum.GetValues(typeof(Shape)).Cast<Shape>())
            {
                if (GetResult(opp, shape) == result)
                {
                    return shape;
                }
            }
            throw new Exception($"Not found a shape for {opp} {result}");
        }

        public static int GetScore(Result result, Shape mine)
        {
            int resultScore = result == Result.Win ? 6 : result == Result.Draw ? 3 : 0;
            int shapeScore = mine == Shape.Rock ? 1 : mine == Shape.Paper ? 2 : 3;
            
            return resultScore + shapeScore;
        }

        public long Solve1(List<List<Shape>> inputList)
        {
            int score = 0;

            foreach (var game in inputList)
            {
                var oppShape = game[0];
                var myShape = game[1];

                var result = GetResult(oppShape, myShape);
                score += GetScore(result, myShape);
            }

            return score;
        }

        public long Solve2(List<Plan> plans)
        {
            int score = 0;

            foreach (var plan in plans)
            {
                var mine = GetShapeForResult(plan.shape, plan.result);
                score += GetScore(plan.result, mine);
            }
            return score;
        }
        
        public static void Start()
        {
            var textData = File.ReadAllText("data/input02.txt");
            var textArr = textData.Split(Environment.NewLine);

            List<List<Shape>> inputList = new();
            List<Plan> intputList2 = new();

            foreach (var line in textArr)
            {
                var val = line.Split(" ");
                var val1 = val[0];
                var val2 = val[1];

                Shape shape1 = (val1 == "A") ? Shape.Rock : val1 == "B" ? Shape.Paper : val1 == "C" ? Shape.Scissor : throw new Exception($"Unknown shape1 {val1}");
                Shape shape2 = (val2 == "X") ? Shape.Rock : val2 == "Y" ? Shape.Paper : val2 == "Z" ? Shape.Scissor : throw new Exception($"Unknown shape2 {val2}");
                Result result = (val2 == "X") ? Result.Lose : val2 == "Y" ? Result.Draw : val2 == "Z" ? Result.Win : throw new Exception($"Unknown result {val2}");

                inputList.Add(new List<Shape>{shape1, shape2});
                intputList2.Add(new Plan(shape1, result));
            }

            var prob = new Problem02();

            var ans1 = prob.Solve1(inputList);
            var ans2 = prob.Solve2(intputList2);;

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


