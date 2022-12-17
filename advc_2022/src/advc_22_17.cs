using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Rock
    {
        private static int NextId = 0;
        public int Id { get; } = ++NextId;
        public MapByList<bool> Sprite { get; } = new();
        public int Width => Sprite.Max.x;
        public int Height => Sprite.Max.y;

        public Rock(Queue<string> linesQ)
        {
            List<string> lines = new();

            while (linesQ.Any())
            {
                string s = linesQ.Dequeue();
                if (s.Length == 0)
                {
                    break;
                }
                lines.Add(s);
            }

            Sprite.SetMax(lines.First().Count(), lines.Count());
            foreach (var line in lines)
            {
                foreach (var c in line)
                {
                    Sprite.Add(c == '#');
                }
            }
        }
        
        public void Draw()
        {
            Sprite.Draw(v => v ? '*' : '.');
            Console.WriteLine("");
        }
    }

    class WindInput
    {
        private string m_windInputStr;
        private int m_index = 0;
        public int Index => m_index;
        public WindInput(string data)
        {
            m_windInputStr = data;
        }

        public Direction.Dir GetNext()
        {
            char c = m_windInputStr[m_index];
            m_index = (m_index + 1) % m_windInputStr.Length;
            return c == '<' ? Direction.Dir.Left : c == '>' ? Direction.Dir.Right : throw new InvalidDataException($"Unknown wind {c}");
        }
    }

    class RockInput 
    {
        private List<Rock> m_rocks = new();
        private int m_index = 0;
        public int Index => m_index;

        public RockInput(List<Rock> rocks)
        {
            m_rocks = rocks;
        }

        public Rock GetNext()
        {
            Rock rock = m_rocks[m_index];
            m_index = (m_index + 1) % m_rocks.Count;
            return rock;
        }
    }

    class GameMap : Loggable
    {
        private MapByDictionary<bool> m_map = new();
        private int m_highestRockY = 0;
        private readonly Point PlaceGap = new Point(2, -4);
        private WindInput WindInput;
        private RockInput Rocks;

        public GameMap(int width, string windStr, List<Rock> rocks)
        {
            m_map.SetMax(width - 1, -1);
            m_map.SetMin(0, int.MinValue);
            WindInput = new(windStr);
            Rocks = new(rocks);
        }

        private void PlaceAndMoveRock(Rock rock)
        {
            Point loc = GetRockIntialPosition(rock);

            Debug.Assert(CheckCollisionFree(rock, loc));

            var tryMove = (Direction.Dir moveDir) => 
            {
                Point nextLoc = loc.MovedPoint(moveDir);

                if (CheckCollisionFree(rock, nextLoc))
                {
                    loc = nextLoc;
                    return true;
                }
                return false;
            };

            do
            {
                tryMove(WindInput.GetNext());
            } 
            while (tryMove(Direction.Dir.Down));

            rock.Sprite.ForEach((v, p) =>
            {
                if (v)
                {
                    Point toPlace = p.AddedPoint(loc);
                    Debug.Assert(m_map.GetAt(toPlace) == false);
                    m_map.SetAt(true, toPlace);
                    //LogDetail($"    Placing : {loc} + {p} = {toPlace}");
                }
            });

            m_highestRockY = Math.Min(loc.y, m_highestRockY);

            LogDetail($"Placed Rock {rock.Id} at {loc}, now upper bound {m_highestRockY}");
        }

        private Point GetRockIntialPosition(Rock rock)
        {
            Point p = new(0, m_highestRockY - rock.Height + 1);
            p.Add(PlaceGap);

            LogDetail($"Placing Rock {rock.Id}, height {rock.Height} width {rock.Width}, mapY {m_highestRockY} => {p}");
            return p;
        }

        private bool CheckCollisionFree(Rock rock, Point loc)
        {
            bool ret = true;
            LogDetail($"Checking collision at {loc}");
            
            rock.Sprite.ForEach((v, p) => 
            {
                if (ret)
                {
                    Point checkPoint = p.AddedPoint(loc);
                    //LogDetail($"  > {loc} + {p} => {checkPoint}");
                    if (!m_map.CheckBoundary(checkPoint))
                    {
                        LogDetail($"        ==> out of bound");
                        ret = false;
                    }
                    else if (v && m_map.GetAt(checkPoint))
                    {
                        LogDetail($"        ==> collision detected");
                        ret = false;
                    }
                }

            });

            return ret;
        }

        public void Draw()
        {
            if (!AllowLogDetail)
            {
                return;
            }

            for (int y = m_highestRockY; y < 0; y ++)
            {
                string header = $"[{(-y).ToString().PadLeft(4)}]";
                Console.Write(header);

                for (int x = 0; x <= m_map.Max.x; x ++)
                {
                    char c = '.';
                    Point p = new Point(x, y);
                    if (m_map.Contains(p) && m_map.GetAt(p))
                    {
                        c = '#';
                    }
                    Console.Write(c);
                }
                Console.WriteLine("");
            }
        }

        public int GetHighestAfter(int numRocks)
        {
            AllowLogDetail = false;
            for (int i = 0; i < numRocks; i ++)
            {
                Rock rock = Rocks.GetNext();
                PlaceAndMoveRock(rock);
                //Draw();
                LogDetail($"[{i.ToString().PadLeft(4)}] {-m_highestRockY}");
            }
            return -m_highestRockY;
        }

        private string GetUpperView()
        {
            StringBuilder sb = new();
            sb.Append("[");
            for (int i = 0; i <= m_map.Max.x; i ++)
            {
                int depth = 0;

                int y = m_highestRockY;
                Point loc = new(i, y);
                while (loc.y < 0)
                {
                    if (m_map.Contains(loc) && m_map.GetAt(loc))
                    {
                        break;
                    }
                    depth ++;
                    loc.y ++;
                }
                sb.Append($"{depth}:");
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            return sb.ToString();
        }

        private record RepeatState(string upperView, int windIndex, int rockIndex);
        private record StateValues(int loopCount, int height);

        public long FindRepeatPoint(long totalLoops)
        {
            AllowLogDetail = false;
            int cnt = 0;

            Dictionary<RepeatState, StateValues> states = new();

            int repeatStart = 0;
            int heightStart = 0;

            while (true)
            {
                var state = new RepeatState(GetUpperView(), WindInput.Index, Rocks.Index);
                LogDetail($"[{cnt}] {state}");

                if (states.ContainsKey(state))
                {
                    var rv = states[state];
                    repeatStart = rv.loopCount;
                    heightStart = rv.height;
                    break;
                }
                states.Add(state, new StateValues(cnt, m_highestRockY));

                cnt ++;
                Rock rock = Rocks.GetNext();
                PlaceAndMoveRock(rock);
            }

            int freq = cnt - repeatStart;
            int addedHeight = m_highestRockY - heightStart;

            long toGo = (totalLoops - repeatStart);
            long toLoopPattern = toGo / freq;
            long remain = toGo % freq;

            long heightAfterRepeatPattern = heightStart + toLoopPattern * addedHeight;
            long currentHeightSnapshot = m_highestRockY;

            for (int i = 0; i < remain; i ++)
            {
                PlaceAndMoveRock(Rocks.GetNext());
            }

            long heightDiffAfterRemain = m_highestRockY - currentHeightSnapshot;
            long ans = heightAfterRepeatPattern + heightDiffAfterRemain;

            return -ans;
        }
    }

    class Problem17 : Loggable
    {
        public static void Start()
        {
            var rockText = new Queue<string>(File.ReadAllText("data/input17Rocks.txt").Split(Environment.NewLine));
            var windText = File.ReadAllText("data/input17Winds.txt");

            List<Rock> rocks = new();
            while (rockText.Any())
            {
                rocks.Add(new Rock(rockText));
            }

            GameMap gameMap1 = new(7, windText, rocks);
            var ans1 = gameMap1.GetHighestAfter(2022);

            GameMap gameMap2 = new(7, windText, rocks);
            var ans2 = gameMap2.FindRepeatPoint(1000000000000);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}