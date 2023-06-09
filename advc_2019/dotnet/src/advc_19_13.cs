using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Advc2019
{
    class Problem13 : Advc.Utils.Loggable
    {
        enum Tile 
        {
            Empty = 0,
            Wall = 1,
            Block = 2,
            Paddle = 3,
            Ball = 4,
        }

        public int Solve1(List<int> arr)
        {
            AllowLogDetail = false;
            Computer09 com = new(arr);

            com.Run(0);

            int ans = 0;
            while (com.OutputCount > 0)
            {
                var x = com.Output;
                var y = com.Output;
                var tile = (Tile) com.Output;

                if (tile == Tile.Block)
                {   
                    ans ++;
                }

                LogDetail($"{x} {y} {tile}");
            }

            return ans;
        }

        public int Solve2(List<int> arr)
        {
            AllowLogDetail = false;

            arr[0] = 2;

            Computer09 com = new(arr);

            long ballX = 0;
            long paddleX = 0;
            int score = 0;
            while (!com.IsHalted)
            {
                int joystick = (ballX > paddleX) ? 1 : (ballX < paddleX) ? -1 : 0;

                LogDetail($"Joystick {joystick} ball {ballX} paddle {paddleX}");
                com.Run(joystick);

                while (com.OutputCount > 0)
                {
                    var x = com.Output;
                    var y = com.Output;
                    var tile = (Tile) com.Output;

                    if (x < 0)
                    {
                        score = (int)tile;
                        LogDetail($"score : {score}  ({x} {y})");
                    }
                    
                    if (tile == Tile.Ball)
                    {
                        ballX = x;
                        LogDetail($"{x} {y} : {tile}");
                    }
                    else if (tile == Tile.Paddle)
                    {
                        paddleX = x;
                        LogDetail($"{x} {y} : {tile}");
                    }
                }
            }

            return score;
        }

        public static void Start()
        {
            var textData = File.ReadAllText("../data/input13.txt");
            var textArr = textData.Split(",");
            var intList = textArr.Select(t => int.Parse(t)).ToList();

            Problem13 prob1 = new();
            Problem13 prob2 = new();

            var ans1 = prob1.Solve1(intList);
            var ans2 = prob2.Solve2(intList);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


