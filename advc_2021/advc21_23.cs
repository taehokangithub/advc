
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_23
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    enum Dir { Up, Down, Left, Right};

    static readonly List<Point> DIRPOINTS = new List<Point>() {new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0)};
    static readonly List<Dir> LEFTRIGHT = new List<Dir>() {Dir.Left, Dir.Right};
    static readonly Dictionary<int, Beast.BeastType> BEASTPOS = new Dictionary<int, Beast.BeastType>() { 
            [3] = Beast.BeastType.A, 
            [5] = Beast.BeastType.B,
            [7] = Beast.BeastType.C,
            [9] = Beast.BeastType.D
    };

    static readonly Dictionary<Beast.BeastType, int> COSTS = new Dictionary<Beast.BeastType, int> {
        [Beast.BeastType.A] = 1,
        [Beast.BeastType.B] = 10,
        [Beast.BeastType.C] = 100,
        [Beast.BeastType.D] = 1000
    };
    
    class Point
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        
        public Point() 
        {
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
        }

        public Point(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Point operator -(Point a, Point b)
        {
            Point p = new Point();

            p.X = a.X - b.X;
            p.Y = a.Y - b.Y;

            return p;            
        }

        public static Point operator +(Point a, Point b)
        {
            Point p = new Point();

            p.X = a.X + b.X;
            p.Y = a.Y + b.Y;

            return p;
        }

        public static bool operator <(Point a, Point b)
        {
            return a.X < b.X && a.Y < b.Y;
        }

        public static bool operator >(Point a, Point b)
        {
            return a.X > b.X && a.Y > b.Y;
        }
        public static bool operator <=(Point a, Point b)
        {
            return a.X <= b.X && a.Y <= b.Y;
        }

        public static bool operator >=(Point a, Point b)
        {
            return a.X >= b.X && a.Y >= b.Y;
        }

        public void SetMax(Point p)
        {
            X = Math.Max(X, p.X);
            Y = Math.Max(Y, p.Y);
        }

        public void SetMin(Point p)
        {
            X = Math.Min(X, p.X);
            Y = Math.Min(Y, p.Y);
        }

        public override string ToString()
        {
            return $"[{X},{Y}]";
        }

        public Point GetDirPoint(Dir dir)
        {
            return this + DIRPOINTS[(int)dir];
        }

        public int MapDistance(Point p)
        {
            int hallY = 1;  // hardcoding :( 

            int ans = Math.Abs(p.Y - hallY) + Math.Abs(Y - hallY);
            ans += Math.Abs(p.X - X);
            return ans;
        }
    }

    class Cell
    {
        public enum CellType
        {
            Room, Hallway, Entrance
        }

        public CellType Type{ get; set; }
        public Beast.BeastType RoomFor{ get; }
        public Point Loc{ get; set; } = new Point();

        public Cell(CellType type, Point loc)
        {
            Type = type;
            Loc = new Point(loc);
            RoomFor = BEASTPOS.ContainsKey(loc.X) ? BEASTPOS[loc.X] : Beast.BeastType.None;
        }

        public bool IsMovable()
        {
            return Type != CellType.Entrance;
        }
    }

    class Beast
    {
        public enum BeastType { A, B, C, D, None };
        public BeastType Type { get; }
        public Point Loc{ get; set; } = new Point();
        public bool Fixed { get; set; } = false;
        public Beast(BeastType bt, Point loc)
        {
            Type = bt;
            Loc = new Point(loc);
        }
        public Beast(Beast other)
        {
            Type = other.Type;
            Loc = new Point(other.Loc);
            Fixed = other.Fixed;
        }
        public static BeastType GetBeastType(char c)
        {
            foreach (BeastType bt in Enum.GetValues(typeof(BeastType)))
            {
                if (bt.ToString()[0] == c)
                {
                    Debug.Assert(bt != BeastType.None);
                    return bt;
                }
            }
            throw new Exception($"Unknown beast char {c}");
        }

        public override string ToString()
        {
            return $"<{Type}:{Loc}>";
        }
    }

    class SolveState
    {
        public Dictionary<string, Beast> Beasts = new Dictionary<string, Beast>();
        public long Cost { get; set; } = 0;

        public SolveState(Dictionary<string, Beast> beasts)
        {
            foreach (var item in beasts)
            {
                Beasts[item.Key] = new Beast(item.Value);
            }
        }

        public SolveState(SolveState other) : this(other.Beasts)
        {
            Cost = other.Cost;
        }

        public void MoveBeastLocation(Point from, Point to)
        {
            Beast myBeast = GetBeast(from);
            Beasts.Remove(from.ToString());
            Beasts[to.ToString()] = myBeast;
            myBeast.Loc = to;
            Cost += COSTS[myBeast.Type] * from.MapDistance(to);
        }

        public bool HasBeast(Point p)
        {
            return Beasts.ContainsKey(p.ToString());
        }

        public Beast GetBeast(Point p)
        {
            return Beasts[p.ToString()];
        }

        public string BeastPosRepresentation()
        {
            string str = "";

            foreach (string key in Beasts.Keys.OrderBy(p => p))
            {
                str += Beasts[key].ToString();
            }

            return str;
        }
    }

    class SolveStateList
    {
        public List<SolveState> Answer = new List<SolveState>();

        public SolveState Last { get => Answer[Answer.Count - 1]; }
        public long Cost { get => Last.Cost; }
        
        //HashSet<string> m_previousStates = new HashSet<string>();
        Dictionary<string, long> m_previousStateCost = new Dictionary<string, long>();

        public SolveStateList(SolveState state)
        {
            AddNewState(state);
        }
        public SolveStateList(SolveStateList list)
        {
            foreach(var state in list.Answer)
            {
                bool ret = AddNewState(state);

                if (!ret)
                {
                    Console.WriteLine($"something's wrong");
                    foreach(var s in list.Answer)
                    {
                        Console.WriteLine(s.BeastPosRepresentation());
                    }   
                }
                Debug.Assert(ret, $"{state.BeastPosRepresentation()}" );
            }
        }

        public bool AddNewState(SolveState state)
        {
            string str = state.BeastPosRepresentation();
            if (m_previousStateCost.ContainsKey(str))
            {
                if (m_previousStateCost[str] < state.Cost)
                {
                    Console.WriteLine($"dup state {str}");
                    return false;
                }
            }
            m_previousStateCost[str] = state.Cost;
            Answer.Add(new SolveState(state));
            return true;
        }

        public void PrintAllStates()
        {
            foreach (string str in m_previousStateCost.Keys)
            {
                Console.WriteLine($"  previous : {str}");
            }
        }
    }

    class Board
    {
        Dictionary<string, Beast> m_beasts = new Dictionary<string, Beast>();
        Dictionary<string, Cell>  m_cells = new Dictionary<string, Cell>();
        HashSet<string> m_savedFailures = new HashSet<string>();
        Point m_max = new Point();
        SolveStateList? m_possibleAnswer = null;

        public void Parse(string line)
        {
            Point curPos = new Point(m_max);
            curPos.X = 0;

            foreach (char c in line)
            {
                if (c == '.')
                {
                    Cell cell = new Cell(Cell.CellType.Hallway, curPos);
                    m_cells[curPos.ToString()] = cell;
                }
                else if (char.IsLetter(c))
                {
                    Cell cell = new Cell(Cell.CellType.Room, curPos);
                    m_cells[curPos.ToString()] = cell;

                    Beast beast = new Beast(Beast.GetBeastType(c), curPos);
                    m_beasts[curPos.ToString()] = beast;
                }
                curPos.X ++;
            }

            m_max.X = Math.Max(curPos.X, m_max.X);
            m_max.Y ++;
        }

        public void DisableUnmovableHallways()
        {
            foreach (Cell cell in m_cells.Values)
            {
                if (cell.Type == Cell.CellType.Hallway)
                {
                    Point down = cell.Loc.GetDirPoint(Dir.Down);

                    if (HasCell(down))
                    {
                        if (GetCell(down).Type == Cell.CellType.Room)
                        {
                            cell.Type = Cell.CellType.Entrance;
                        }
                        else
                        {
                            throw new Exception($"is it possible to have {GetCell(down).Type} under hallway at {cell.Loc} ?");
                        }
                    }
                }
            }
        }

        public void Solve()
        {
            SolveStateList initialList = new SolveStateList(new SolveState(m_beasts));
            SolveInternal(initialList);

            if (m_possibleAnswer != null)
            {
                PrintSolveStateList(m_possibleAnswer);
                Console.WriteLine($"Answer = {m_possibleAnswer.Cost}");
                return;
            }

            throw new Exception("Failed to find answer");
        }

        bool SolveInternal(SolveStateList stateList)
        {
            SolveState state = stateList.Last;

            if (m_savedFailures.Contains(state.BeastPosRepresentation()))
            {
                return false;
            }

            Console.WriteLine($"\nSolveInternal, cur moves [{stateList.Answer.Count}]");
            PrintSolveState(stateList.Last);

            if (m_possibleAnswer != null && stateList.Cost >= m_possibleAnswer.Cost)
            {
                //Console.WriteLine($"[Move {stateList.Answer.Count}] Cost overfow {stateList.Cost}\n{state.BeastPosRepresentation()}");
                return false;
            }

            if (CheckAnswerAndSave(stateList))
            {
                return true; // This is the final state
            }

            bool hasDoneAnything = false;
            bool ret = false;
            foreach (Beast beast in state.Beasts.Values)
            {
                if (beast.Fixed)
                {
                    continue;
                }

                List<Point> possibleMoves = GetPossibleMovement(state, beast);

                foreach (Point point in possibleMoves)
                {
                    
                    SolveStateList newStateList = new SolveStateList(stateList);
                    SolveState newState = new SolveState(newStateList.Last);

                    newState.MoveBeastLocation(beast.Loc, point);

                    Console.WriteLine($"   Trying to add new possible  move for beast {beast.Type} to {point}");
                    if (newStateList.AddNewState(newState))
                    {
                        hasDoneAnything = true;
                        if (SolveInternal(newStateList))
                        {
                            ret = true;
                        }
                    }
                }
            }

            if (!hasDoneAnything)
            {
                //Console.WriteLine($"[Move {stateList.Answer.Count}] Not found any further moves");
            }

            if (!ret)
            {
                m_savedFailures.Add(state.BeastPosRepresentation());
            }

            return ret;
        }
        bool CheckAnswerAndSave(SolveStateList stateList)
        {
            SolveState state = stateList.Last;
            bool isAnswer = true;
            foreach (Beast beast in state.Beasts.Values)
            {
                if (beast.Fixed)
                {
                    continue;
                }
                Cell cell = GetCell(beast.Loc);

                if (cell.Type == Cell.CellType.Room && cell.RoomFor == beast.Type)
                {
                    Point down = beast.Loc.GetDirPoint(Dir.Down);
                    if (!HasCell(down) || (state.HasBeast(down) && state.GetBeast(down).Fixed))
                    {
                        beast.Fixed = true;
                        //Console.WriteLine($"** [Move {stateList.Answer.Count}] Found beast type {beast.Type} at cell {cell.Loc}");
                    }
                    continue;
                }
                isAnswer = false;
            }

            if (isAnswer)
            {
                Console.WriteLine($"!! Found answer at move {stateList.Answer.Count} Cost {stateList.Cost}");
                m_possibleAnswer = stateList;
            }

            return isAnswer;
        }

        List<Point> GetPossibleMovementFromHallway(SolveState state, Beast beast)
        {
            Debug.Assert(GetCell(beast.Loc).Type == Cell.CellType.Hallway);

            List<Point> ans = new List<Point>();

            foreach (Dir dir in LEFTRIGHT)
            {
                Point curPos = beast.Loc.GetDirPoint(dir);

                while(HasCell(curPos) && state.HasBeast(curPos) == false)
                {
                    if (GetCell(curPos).Type == Cell.CellType.Entrance)
                    {
                        Point go = curPos.GetDirPoint(Dir.Down);
                        
                        while(HasCell(go) && !state.HasBeast(go) && GetCell(go).RoomFor == beast.Type)
                        {
                            Point furtherGo = go.GetDirPoint(Dir.Down);

                            if (!HasCell(furtherGo) || (state.HasBeast(furtherGo) && state.GetBeast(furtherGo).Fixed))
                            {
                                ans.Add(go);
                                break;
                            }
                            go = furtherGo;
                        }
                    }
                    curPos = curPos.GetDirPoint(dir);
                }
            }

            return ans;
        }

        List<Point> GetPossibleMovementFromRoom(SolveState state, Beast beast)
        {
            Debug.Assert(GetCell(beast.Loc).Type == Cell.CellType.Room);

            List<Point> ans = new List<Point>();

            Point curPos = new Point(beast.Loc).GetDirPoint(Dir.Up);

            bool isMovable = true;

            while (HasCell(curPos) && GetCell(curPos).IsMovable())
            {
                if (state.HasBeast(curPos))
                {
                    isMovable = false;
                    break;
                }
                curPos = curPos.GetDirPoint(Dir.Up);
            }

            if (isMovable)
            {
                foreach (Dir dir in LEFTRIGHT)
                {
                    Point go = curPos.GetDirPoint(dir);

                    while(HasCell(go) && state.HasBeast(go) == false)
                    {
                        if (GetCell(go).IsMovable())
                        {
                            ans.Add(go);
                        }
                        else
                        {
                            // Go down
                            Point godown = go.GetDirPoint(Dir.Down);
                            
                            while(HasCell(godown) && !state.HasBeast(godown) && GetCell(godown).RoomFor == beast.Type)
                            {
                                Point furtherGo = godown.GetDirPoint(Dir.Down);

                                if (!HasCell(furtherGo) || (state.HasBeast(furtherGo) && state.GetBeast(furtherGo).Fixed))
                                {
                                    Console.WriteLine($" found downside {godown}");
                                    ans.Add(godown);
                                    break;
                                }
                                godown = furtherGo;
                            }                            
                        }
                        go = go.GetDirPoint(dir);
                    }
                }
            }
            return ans;
        }

        List<Point> GetPossibleMovement(SolveState state, Beast beast)
        {
            Cell mycell = GetCell(beast.Loc);

            if (mycell.Type == Cell.CellType.Room)
            {
                return GetPossibleMovementFromRoom(state, beast);
            }
            else if (mycell.Type == Cell.CellType.Hallway)
            {
                return GetPossibleMovementFromHallway(state, beast);
            }

            throw new Exception($"Invalid cell type for beast, type {mycell.Type}, Loc {beast.Loc}");
        }


        bool HasCell(Point p)
        {
            return m_cells.ContainsKey(p.ToString());
        }

        Cell GetCell(Point p)
        {
            return m_cells[p.ToString()];
        }

        public void PrintSolveState(SolveState state)
        {
            Console.WriteLine($"----- COST {state.Cost} -----");

            for (int y = 0; y < m_max.Y; y ++)
            {
                for (int x = 0; x < m_max.X; x ++)
                {
                    Point p = new Point(x, y);

                    if (state.HasBeast(p))
                    {
                        Console.Write(state.GetBeast(p).Type.ToString());
                    }
                    else if (HasCell(p))
                    {
                        if (GetCell(p).IsMovable())
                            Console.Write('.');
                        else 
                            Console.Write('-');
                    }
                    else
                    {
                        Console.Write('#');
                    }
                }
                Console.Write('\n');
            }
        }

        public void PrintSolveStateList(SolveStateList list)
        {
            foreach (SolveState state in list.Answer)
            {
                PrintSolveState(state);
            }
        }
    }

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Board board = new Board();
        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                board.Parse(line);
            }
        }
        board.DisableUnmovableHallways();
        board.Solve();
    }
    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        //SolveMain($"../../data/{className}.txt");
    }
    
    static void Main()
    {
        Run();
    }    
}