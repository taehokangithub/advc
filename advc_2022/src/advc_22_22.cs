using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem22 : Loggable
    {
        enum Tile { Empty, Road, Wall };
        enum Transition { Flat, Cube };
        class Map : MapByList<Tile>
        {
            public Point CurLoc { get; private set; } = new Point(0, 0);
            public Direction.Dir CurDir { get; private set; } = Direction.Dir.Right;
            public Transition Trans { get; set; } = Transition.Flat;
            private Dictionary<(Point, Direction.Dir), Point> m_transition = new();
            private Loggable m_logger;

#region constructor 
            public Map(string[] linesArr, Loggable logger)
            {
                m_logger = logger;
                List<string> lines = new(linesArr);
                lines.RemoveRange(lines.Count - 2, 2);
                SetMax(lines.Max(l => l.Length), lines.Count);
                bool hasFoundStartingPosition = false;
                foreach (var line in lines)
                {
                    if (line.Length == 0)
                    {
                        break;
                    }

                    int numAdded = 0;
                    foreach (char c in line)
                    {
                        Tile t = (c == ' ') ? Tile.Empty :
                                (c == '#') ? Tile.Wall :
                                (c == '.') ? Tile.Road : throw new Exception("Unknown tile {c}");
                        
                        if (!hasFoundStartingPosition && t == Tile.Road)
                        {
                            hasFoundStartingPosition = true;
                            CurLoc = m_addPointer;
                        }
                        Add(t);
                        numAdded ++;
                    }

                    for (int i = numAdded; i < Max.x; i ++)
                    {
                        Add(Tile.Empty);
                    }
                }

                //Draw(t => t == Tile.Empty ? '.' : t == Tile.Road ? '+' : '#');
            }
#endregion
            
            private Point FindFlatTransition(Point curLoc, Direction.Dir dir)
            {
                Direction.Dir opposite = Direction.Rotate(dir, Direction.Dir.Down);
                Point rememberCurLoc = curLoc;
                var nextLoc = curLoc;
                while (CheckBoundary(nextLoc) && GetAt(nextLoc) != Tile.Empty)
                {
                    curLoc = nextLoc;
                    nextLoc = curLoc.MovedPoint(opposite);
                }
                nextLoc = curLoc;
                m_logger.LogDetail($"     Found transition from {rememberCurLoc} to {nextLoc}");
                if (GetAt(nextLoc) == Tile.Wall)
                {
                    nextLoc = rememberCurLoc;
                    m_logger.LogDetail($"     That's a wall! Fall back to {rememberCurLoc}");
                }
                return nextLoc;   
            }

            private Point FindCubeTransition(Point curLoc, Direction.Dir dir)
            {
                return FindFlatTransition(curLoc, dir);
            }

            private Point FindNextLoc(Point curLoc, Direction.Dir dir)
            {
                //m_logger.LogDetail($"Moving from {curLoc}, to {dir}");
                var nextLoc = curLoc.MovedPoint(dir);
                Tile t = CheckBoundary(nextLoc) ? GetAt(nextLoc) : Tile.Empty;

                if (t == Tile.Wall)
                {
                    //m_logger.LogDetail($"     Found wall, stopped at {curLoc}");
                    nextLoc = curLoc;
                }
                else if (t == Tile.Empty)
                {
                    if (m_transition.ContainsKey((curLoc, dir)))
                    {
                        nextLoc = m_transition[(curLoc, dir)];
                    }
                    else
                    {
                        nextLoc = (Trans == Transition.Flat) ? FindFlatTransition(curLoc, dir)
                                    : (Trans == Transition.Cube) ? FindCubeTransition(curLoc, dir)
                                    : throw new Exception($"Unknown transition {Trans}");
                        m_transition.Add((curLoc, dir), nextLoc);
                    }
                }

                Debug.Assert(GetAt(nextLoc) == Tile.Road);
                return nextLoc;
            }

            public void Go(int length)
            {
                for (int i = 0; i < length; i ++)
                {
                    CurLoc = FindNextLoc(CurLoc, CurDir);
                }
                m_logger.LogDetail($"Moved {length} times to {CurDir} => {CurLoc}");
            }

            public void Turn(Direction.Dir dir)
            {
                Debug.Assert(dir == Direction.Dir.Left || dir == Direction.Dir.Right);
                CurDir = Direction.Rotate(CurDir, dir);
                m_logger.LogDetail($"Turned {dir} => {CurDir}");
            }
        }

        class Control : Loggable
        {
            enum CmdType { Go, Turn };
            record Cmd(CmdType type, int length, Direction.Dir dir);

            private Map m_map;
            private List<Cmd> m_commands = new();
#region constructor
            public Control(string[] lines)
            {
                m_map = new(lines, this);

                var line = new Queue<char>(lines.Last().ToCharArray());

                while (line.Any())
                {
                    List<char> numChars = new();
                    while (line.Any() && char.IsNumber(line.First()))
                    {
                       numChars.Add(line.Dequeue());
                    }
                    int num = int.Parse(string.Join("", numChars));
                    m_commands.Add(new(CmdType.Go, num, default));

                    if (line.Any())
                    {
                        char d = line.Dequeue();
                        Direction.Dir dir = (d == 'R') ? Direction.Dir.Right 
                                            : (d == 'L') ? Direction.Dir.Left
                                            : throw new Exception("Unknown dir {d}");
                        m_commands.Add(new(CmdType.Turn, default, dir));
                    }
                }
            }
#endregion    
            private void DoCommands()
            {
                foreach (var cmd in m_commands)
                {
                    if (cmd.type == CmdType.Go)
                    {
                        m_map.Go(cmd.length);
                    }
                    else if (cmd.type == CmdType.Turn)
                    {
                        m_map.Turn(cmd.dir);
                    }
                    else 
                    {
                        throw new Exception($"Unknown cmd tpe {cmd.type}");
                    }
                }
            }

            private long GetPasswordFromLocAndDir(Point loc, Direction.Dir dir)
            {
                int facing = 0;
                switch (dir) 
                {
                    case Direction.Dir.Up: facing = 3; break;
                    case Direction.Dir.Left : facing = 2; break;
                    case Direction.Dir.Down : facing = 1; break;
                    case Direction.Dir.Right : facing = 0; break;
                }

                return 1000 * (loc.y + 1) + 4 * (loc.x + 1) + (facing);
            }

            public long GetPassword()
            {
                AllowLogDetail = false;
                DoCommands();
                LogDetail($"loc {m_map.CurLoc} facing {m_map.CurDir}");

                return GetPasswordFromLocAndDir(m_map.CurLoc, m_map.CurDir);
            }

            public long GetCubePassword()
            {
                AllowLogDetail = true;
                m_map.Trans = Transition.Cube;
                DoCommands();
                LogDetail($"loc {m_map.CurLoc} facing {m_map.CurDir}");

                return GetPasswordFromLocAndDir(m_map.CurLoc, m_map.CurDir);                
            }

        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input22.txt");
            var lines = textData.Split(Environment.NewLine);

            var ans1 = new Control(lines).GetPassword();
            var ans2 = new Control(lines).GetCubePassword();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


