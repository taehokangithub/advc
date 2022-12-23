using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    using Dir = Direction.Dir;
    class Problem22 : Loggable
    {
        enum Tile { Empty, Road, Wall };
        enum CmdType { Go, Turn };
        enum TransitionType { Flat, Cube };
        record Cmd(CmdType type, int length, Dir dir);
        record SquareLink(Square square, Dir orgDir, Dir newDir);

        static Dir GetLinkRotation(SquareLink sql) 
        {
            return Direction.GetRotateDiff(sql.newDir, sql.orgDir);
        }

        class Square
        {
            private static int s_index = 0;
            public int Index { get; } = s_index ++;
            public Dictionary<Dir, SquareLink> Links { get; } = new();
            public Point GlobalLoc { get; init; }
            public int m_length;

            public Square(Point loc, int length)
            {
                GlobalLoc = loc;
                m_length = length;
            }

            public bool IsInSquare(Point p)
            {
                return (p.x >= GlobalLoc.x && p.x < GlobalLoc.x + m_length
                        && p.y >= GlobalLoc.y && p.y < GlobalLoc.y + m_length);
            }

            public Point GetRelativeLoc(Point globalLoc) 
            {
                return globalLoc.SubtractedPoint(GlobalLoc);
            }

            public Point GetGlobalLoc(Point relLoc)
            {
                return relLoc.AddedPoint(GlobalLoc);
            }

            public override string ToString()
            {
                return $"[Sq {Index}{GlobalLoc}]";
            }

        }

        class Cube 
        {
            private int m_cubeSize;
            private int m_maxInclusive;

            private Dictionary<Point, Square> m_findCache = new();
            private List<Square> m_squares { get; } = new();

            private Loggable m_logger;

            public Cube(int length, Loggable logger)
            {
                m_logger = logger;
                m_cubeSize = length;
                m_maxInclusive = m_cubeSize - 1;
            }

            public void AddNewSquare(int x, int y)
            {
                m_squares.Add(new Square(new(x, y), m_cubeSize));
            }
            
            public Square? GetSquareByPoint(Point p)
            {
                if (m_findCache.ContainsKey(p))
                {
                    return m_findCache[p];
                }

                var square = m_squares.FirstOrDefault(s => s.IsInSquare(p));
                if (square != null)
                {
                    m_findCache.Add(p, square);
                }
                
                return square;
            }

            public (Point, Dir) GetTransition(Point srcPoint, Dir dir)
            {
                Square sq = GetSquareByPoint(srcPoint)!;
                SquareLink sql = sq.Links[dir];
                Debug.Assert(dir == sql.orgDir);

                Dir outDir = sql.newDir;
                Point srcRel = sq.GetRelativeLoc(srcPoint);
                Debug.Assert(dir == Dir.Up && srcRel.y == 0
                            || dir == Dir.Left && srcRel.x == 0
                            || dir == Dir.Right && srcRel.x == m_maxInclusive
                            || dir == Dir.Down && srcRel.y == m_maxInclusive);

                Point outRel = srcRel;
                Point outPoint = srcPoint;

                var reverse = (bool isReverse, int x) => isReverse ? m_maxInclusive - x : x;

                if (sql.newDir == sql.orgDir)
                {
                    if (sql.newDir == Dir.Up)
                    {
                        outRel.y = m_maxInclusive;
                    }
                    else if (sql.newDir == Dir.Right)
                    {
                        outRel.x = 0;
                    }
                    else if (sql.newDir == Dir.Left)
                    {
                        outRel.x = m_maxInclusive;
                    }
                    else if (sql.newDir == Dir.Down)
                    {
                        outRel.y = 0;
                    }
                }
                else if (sql.newDir == Direction.Rotate(sql.orgDir, Dir.Down))
                {
                    if (sql.newDir == Dir.Down)
                    {
                        outRel.y = 0;
                        outRel.x = reverse(true, srcRel.x);
                    }
                    else if (sql.newDir == Dir.Up)
                    {
                        outRel.y = m_maxInclusive;
                        outRel.x = reverse(true, srcRel.x);
                    }
                    else if (sql.newDir == Dir.Right)
                    {
                        outRel.x = 0;
                        outRel.y = reverse(true, srcRel.y);
                    }
                    else if (sql.newDir == Dir.Left)
                    {
                        outRel.x = m_maxInclusive;
                        outRel.y = reverse(true, srcRel.y);
                    }
                }
                else 
                {
                    if (sql.newDir == Dir.Up)
                    {
                        outRel.y = m_maxInclusive;
                        outRel.x = reverse(dir == Dir.Left, srcRel.y);
                    }
                    else if (sql.newDir == Dir.Down)
                    {
                        outRel.y = 0;
                        outRel.x = reverse(dir == Dir.Right, srcRel.y);
                    }
                    else if (sql.newDir == Dir.Left)
                    {
                        outRel.x = m_maxInclusive;
                        outRel.y = reverse(dir == Dir.Up, srcRel.x);
                    }
                    else if (sql.newDir == Dir.Right)
                    {
                        outRel.x = 0;
                        outRel.y = reverse(dir == Dir.Down, srcRel.x);
                    }
                }
                Debug.Assert(sql.newDir == Dir.Up && outRel.y == m_maxInclusive
                            || sql.newDir == Dir.Left && outRel.x == m_maxInclusive
                            || sql.newDir == Dir.Right && outRel.x == 0
                            || sql.newDir == Dir.Down && outRel.y == 0);

                outPoint = sql.square.GetGlobalLoc(outRel);

                m_logger.LogDetail($"Transition from {sq} to {sql}, point from {srcPoint} to {outPoint}, rel from {srcRel} to {outRel} ");

                return (outPoint, outDir);
            }

            public void LinkSquares()
            {
                bool hasFoundAll = false;
                const int MaxTry = 1000;
                int cntTry = 0;
                while (!hasFoundAll)
                {
                    if (cntTry ++ == MaxTry)
                    {
                        throw new Exception($"Can't find all links!");
                    }
                    hasFoundAll = true;
                    foreach (var square in m_squares)
                    {
                        foreach (var dir in Enum.GetValues<Dir>())
                        {
                            if (!square.Links.ContainsKey(dir))
                            {
                                var link = FindLink(square, dir);
                                if (link != null)
                                {
                                    square.Links.Add(dir, link);
                                    m_logger.LogDetail($"{square} found link {link.square}, {link}");

                                    Dir opDir = Direction.Rotate(link.newDir, Dir.Down);
                                    if (!link.square.Links.ContainsKey(opDir))
                                    {
                                        Dir opDir2 = Direction.Rotate(link.orgDir, Dir.Down);
                                        SquareLink opLink = new SquareLink(square, opDir, opDir2);
                                        link.square.Links.Add(opDir, opLink);
                                        m_logger.LogDetail($"=> Oplink {link.square} found link {square}, {opLink} ");
                                    }
                                }
                                else
                                {
                                    hasFoundAll= false;
                                }
                            }
                        }
                    }
                }

                m_logger.LogDetail($"===> Found All Links!");
            }

            private SquareLink? FindLink(Square square, Dir dir)
            {
                var checkPoint = square.GlobalLoc.DividedPoint(m_cubeSize).MovedPoint(dir).MultipliedPoint(m_cubeSize);
                var target = GetSquareByPoint(checkPoint);
                m_logger.LogDetail($"    * FindLink : trying from {square} {dir}");
                if (target != null)
                {
                    return new SquareLink(target, dir, dir);
                }

                foreach (var otherDir in Enum.GetValues<Dir>())
                {
                    if (dir == otherDir || dir == Direction.Rotate(otherDir, Dir.Down))
                    {
                        continue;
                    }
                    if (square.Links.TryGetValue(otherDir, out var otherLink))
                    {
                        var otherSquare = otherLink.square;
                        var otherLinkRotation = GetLinkRotation(otherLink);
                        var dirToLookfor = Direction.Rotate(dir, otherLinkRotation);

                        if (otherSquare.Links.TryGetValue(dirToLookfor, out var targetLink))
                        {
                            var targetLinkRotation = GetLinkRotation(targetLink);
                            var newDir = Direction.Rotate(Direction.Rotate(otherDir, otherLinkRotation), targetLinkRotation);

                            m_logger.LogDetail($"        * Found indirect link, from {square} to {otherLink.square} to {targetLink.square}");
                            Debug.Assert(targetLink.square.Index != square.Index);
                            return new SquareLink(targetLink.square, dir, newDir);
                        }
                    }
                }
#if false
                var oppDir = Direction.Rotate(dir, Dir.Down);
                if (square.Links.TryGetValue(oppDir, out var opp1Link))
                {
                    //m_logger.LogDetail($"    Trying circular, {oppDir}, found {opp1Link.square}");
                    if (opp1Link.square.Links.TryGetValue(oppDir, out var opp2Link))
                    {
                        //m_logger.LogDetail($"    Trying circular, {oppDir}, found {opp2Link.square}");
                        if (opp2Link.square.Links.TryGetValue(oppDir, out var targetLink))
                        {
                            m_logger.LogDetail($"        * Found circular link, from {square} to {opp1Link.square} to {opp2Link.square} to {targetLink.square}");
                            Debug.Assert(targetLink.square.Index != square.Index);

                            // 3 times on the opposite direction => linked to the original direction
                            return new SquareLink(targetLink.square, dir, dir);
                        }
                    }
                }
#endif
                return null;
            }
        }
        
        class Map : MapByList<Tile>
        {
            private List<Cmd> m_cmds = new();
            private int m_cubeSize = int.MaxValue;
            private Point m_curLoc;
            private Dir m_curDir = Dir.Right;
            private Loggable m_logger;
            private TransitionType m_transType;
            private Cube m_cube = new(0, new());

#region parser
            public Map(string[] lines, Loggable logger, TransitionType transType)
            {
                m_logger = logger;
                m_transType = transType;

                List<List<Tile>> tiles = new();

                bool hasFoundStartingPoint = false;
                foreach (var line in lines)
                {
                    if (line.Length == 0)
                    {
                        break;
                    }

                    List<Tile> newTiles = new();
                    bool segmentStarted = false;
                    int segmentLength = 0;
                    foreach (char c in line)
                    {
                        Tile t = c == ' ' ? Tile.Empty 
                                : c == '#' ? Tile.Wall 
                                : c == '.' ? Tile.Road 
                                : throw new Exception($"Unknown Tile {c}");

                        newTiles.Add(t);

                        if (t != Tile.Empty)
                        {
                            if (!hasFoundStartingPoint)
                            {
                                hasFoundStartingPoint = true;
                                m_curLoc = new(newTiles.Count - 1, 0);
                            }
                            segmentStarted = true;
                        }

                        segmentLength += segmentStarted ? 1 : 0;
                    }
                    m_cubeSize = Math.Min(m_cubeSize, segmentLength);
                    tiles.Add(newTiles);
                }

                SetMax(tiles.Max(t => t.Count), tiles.Count);
                tiles.ForEach(lineOfTiles => {
                    lineOfTiles.ForEach(t => Add(t));
                    int toAdd = Max.x - lineOfTiles.Count;
                    Enumerable.Range(0, toAdd).ToList().ForEach(a => Add(Tile.Empty));
                });
                CheckAddFinished(throwException: true);

                //Console.WriteLine($"{m_cubSize}, {m_curLoc}");
                //Draw(t => t == Tile.Empty ? '^' : t == Tile.Road ? '.' : '#');

                var cmdLine = new Queue<char>(lines.Last());

                while (cmdLine.Any())
                {
                    List<char> numStr = new();
                    while (cmdLine.Any() && char.IsNumber(cmdLine.First()))
                    {
                        numStr.Add(cmdLine.Dequeue());
                    }

                    int value = int.Parse(string.Join("", numStr));
                    m_cmds.Add(new(CmdType.Go, value, default));
                    
                    if (cmdLine.Any())
                    {
                        char c = cmdLine.Dequeue();
                        Dir dir = c == 'R' ? Dir.Right : c == 'L' ? Dir.Left : throw new Exception($"Unknown dir {c}");
                        m_cmds.Add(new(CmdType.Turn, default, dir));
                    }
                }
            }
#endregion
            
            private void MakeCube()
            {
                m_cube = new(m_cubeSize, m_logger);

                for (int y = 0; y < Max.y; y += m_cubeSize)
                {
                    for (int x = 0; x < Max.x; x += m_cubeSize)
                    {
                        if (GetAt(x, y) != Tile.Empty)
                        {
                            m_cube.AddNewSquare(x, y);
                        }
                    }
                }

                m_cube.LinkSquares();
            }

            private Point FindFlatTransition(Point curLoc, Dir dir)
            {
                Dir oppDir = Direction.Rotate(dir, Dir.Down);
                Point orgLoc = curLoc;
                Point nextLoc = curLoc;

                while (CheckBoundary(nextLoc) && GetAt(nextLoc) != Tile.Empty)
                {
                    curLoc = nextLoc;
                    nextLoc.Move(oppDir);
                }

                if (GetAt(curLoc) == Tile.Wall)
                {
                    curLoc = orgLoc;
                }
                return curLoc;
            }

            private Point FindCubeTransition(Point curLoc, Dir dir)
            {
                var (nextLoc, nextDir) = m_cube.GetTransition(curLoc, dir);

                m_logger.LogDetail($"Found cube tx, from {curLoc} dir {dir} => {nextLoc}, {nextDir}");
                if (!CheckBoundary(nextLoc) || GetAt(nextLoc) == Tile.Wall)
                {
                    nextLoc = curLoc;
                }
                else
                {
                    m_curDir = nextDir;
                }
                
                return nextLoc;
            }

            private Point FindNextPos(Point curLoc, Dir dir)
            {
                var nextLoc = curLoc.MovedPoint(dir);
                Tile nextTile = Tile.Empty;

                if (CheckBoundary(nextLoc))
                {
                    nextTile = GetAt(nextLoc);
                }

                if (nextTile == Tile.Wall)
                {
                    nextLoc = curLoc;
                }
                else if (nextTile == Tile.Empty)
                {  
                    nextLoc = (m_transType == TransitionType.Flat) ? FindFlatTransition(curLoc, dir) 
                            : FindCubeTransition(curLoc, dir);
                }

                m_logger.LogDetail($"Moved from {curLoc}, {dir} => {nextLoc}");
                return nextLoc;
            }
            
            private void Go(int length)
            {
                m_logger.LogDetail($"[GO] {m_curDir} {length}");
                for (int i = 0; i < length; i ++)
                {
                    m_curLoc = FindNextPos(m_curLoc, m_curDir);
                }
            }

            private void Turn(Dir dir)
            {
                m_logger.LogDetail($"[Turn] {m_curDir} => {dir} => {Direction.Rotate(m_curDir, dir)}");
                m_curDir = Direction.Rotate(m_curDir, dir);
            }

            private void RunCommands()
            {
                foreach (var cmd in m_cmds)
                {
                    if (cmd.type == CmdType.Go)
                    {
                        Go(cmd.length);
                    }
                    else 
                    {
                        Turn(cmd.dir);
                    }
                }
            }

            private int GetFacing(Dir dir)
            {
                return dir == Dir.Right ? 0 
                    : dir == Dir.Down ? 1 
                    : dir == Dir.Left ? 2
                    : dir == Dir.Up ? 3 
                    : throw new UnreachableException();
            }

            public int GetPassword()
            {
                if (m_transType == TransitionType.Cube)
                {
                    MakeCube();
                }

                RunCommands();

                m_logger.LogDetail($"Finished at {m_curLoc}, {m_curDir}");

                return (m_curLoc.y + 1) * 1000 + (m_curLoc.x + 1) * 4 + GetFacing(m_curDir);
            }
        }
        public static void Start()
        {
            var textData = File.ReadAllText("data/input22.txt");
            var lines = textData.Split(Environment.NewLine);

            Problem22 prob1 = new() { AllowLogDetail = false };
            Map map1 = new(lines, prob1, TransitionType.Flat);

            Problem22 prob2 = new() { AllowLogDetail = false };
            Map map2 = new(lines, prob2, TransitionType.Cube);

            var ans1 = map1.GetPassword();
            var ans2 = map2.GetPassword();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


