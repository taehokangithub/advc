
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_21
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    const int s_numRollsPerRound = 3;

    class Player
    {
        public int Loc { get; set; }
        public int Score { get; set; } = 0;
        public long WinCases { get; set; } = 0;

        public Player(int loc)
        {
            Loc = loc;
        }

        public Player(Player other)
        {
            Loc = other.Loc;
            Score = other.Score;
            WinCases = other.WinCases;
        }

        public override string ToString()
        {
            return $"[Loc<{Loc}> Score<{Score}> Win<{WinCases}>]";
        }
    }

    class Board
    {
        const int s_maxLoc = 10;
        List<Player> m_players = new List<Player>();
        int m_curPlayer = 0;
        int m_targetScore;
        string m_path = "";

        public Board(int a, int b, int targetScore)
        {
            m_players.Add(new Player(a));
            m_players.Add(new Player(b));

            m_targetScore = targetScore;
        }

        public Board(Board other)
        {
            other.m_players.ForEach(p => m_players.Add(new Player(p)));

            m_targetScore = other.m_targetScore;
            m_curPlayer = other.m_curPlayer;
            m_path = other.m_path;
        }

        public bool GiveDice(int dice)  // it's the sum of 3 rolls already!
        {
            m_path += dice.ToString();

            Player player = m_players[m_curPlayer];
            player.Loc = ((player.Loc + dice - 1) % s_maxLoc) + 1;
            player.Score += player.Loc;

            int playerIndexToLog = m_curPlayer + 1;
            
            debugWrite($"Player[{m_curPlayer}] dice {dice} Loc {player.Loc} Score {player.Score}");

            m_curPlayer = (m_curPlayer + 1) % 2;

            if (player.Score >= m_targetScore)
            {
                player.WinCases = 1;
                return true;
            }
            return false;
        }

        public int GetWinnderIndex()
        {
            for (int i = 0; i < m_players.Count; i ++)
            {
                if (m_players[i].Score >= m_targetScore)
                {
                    return i;
                }
            }

            throw new Exception($"No winner");
        }

        public int GetWinnerIndexPart2()
        {
            return (m_players[0].WinCases > m_players[1].WinCases ? 0 : 1);
        }

        public int GetLoserScore()
        {
            int loseIndex = GetWinnderIndex() == 1 ? 0 : 1;

            return m_players[loseIndex].Score;
        }

        public long GetPlayerWinStat(int index)
        {
            return m_players[index].WinCases;
        }

        public void AddStats(Board other, long cases)
        {
            for (int i = 0; i < m_players.Count; i ++)
            {
                m_players[i].WinCases += other.m_players[i].WinCases * cases;
                debugWrite($"[{m_path}] adding player {i} wincase by {other.m_path} {other.m_players[i].WinCases} * {cases} = {m_players[i].WinCases}");
            }
        }

        public string RecursionInfo()
        {
            return $"[{m_path}] {m_players[0]}, {m_players[1]}";
        }

        public string ResultMapKey() // to be stored as a key in the result map
        {
            return $"[{m_curPlayer}/{m_players[0].Loc}:{m_players[0].Score}:{m_players[1].Loc}:{m_players[1].Score}]";
        }
    }

    class Sovle2
    {
        
        const int s_targetScore = 21;
        readonly Dictionary<int, long> m_predefinedCases = new Dictionary<int, long>()  // cases per 3-dice results (3,4,5,6,7,8,9) for each reound
        {
            [3] = 1, [4] = 3, [5] = 6, [6] = 7, [7] = 6, [8] = 3, [9] = 1
        };

        Dictionary<string, Board> m_resultMap = new Dictionary<string, Board>();

        public void Solve(int a, int b)
        {
            Board board = new Board(a, b, s_targetScore);

            Search(board);

            Console.WriteLine($"Solve2 answer = {board.GetPlayerWinStat(0)}, {board.GetPlayerWinStat(1)}, Winner {board.GetWinnerIndexPart2()}");
        }

        void Search(Board board, int depth = 0)
        {
            string resultMapKey = board.ResultMapKey();

            if (m_resultMap.ContainsKey(resultMapKey))
            {
                Board resultBoard = m_resultMap[resultMapKey];

                board.AddStats(resultBoard, 1);

                return;
            }

            debugWrite($"[{depth}] starting : {board.RecursionInfo()}");

            Dictionary<Board, long> newBoards = new Dictionary<Board, long>();

            long dbgTotalCases = 0;
            foreach (var item in m_predefinedCases)
            {
                int dice = item.Key;
                long cases = item.Value;

                dbgTotalCases += cases;

                Board nextBoard = new Board(board);
                newBoards[nextBoard] = cases;

                if (!nextBoard.GiveDice(dice))
                {
                    Search(nextBoard, depth + 1);
                }
            }

            foreach (var item in newBoards)
            {
                board.AddStats(item.Key, item.Value);
            }

            m_resultMap[resultMapKey] = board;

            debugWrite($"[{depth}] finishing : {dbgTotalCases} cases, saving {resultMapKey} {board.RecursionInfo()}");
        }
    }

    class Solve1
    {
        const int s_minDice = 1;
        const int s_maxDice = 100;
        const int s_maxScore = 1000;
        int m_curDice = s_minDice;

        public void Solve(int a, int b)
        {
            Board board = new Board(a, b, s_maxScore);

            int numRoll = 0;
            int sumDice = 0;
            do
            {
                sumDice = 0;
                for (int i = 0; i < s_numRollsPerRound; i ++)
                {
                    numRoll ++;
                    sumDice += GetDice();
                }
            }
            while(!board.GiveDice(sumDice));

            Console.WriteLine($"Solve1 answer : {numRoll * board.GetLoserScore()}");
        }

        int GetDice()
        {
            int ret = m_curDice;

            m_curDice ++;

            if (m_curDice > s_maxDice)
            {
                m_curDice = s_minDice;
            }

            return ret;
        }
    }

    static void SolveMain(int a, int b)
    {
        Solve1 solve1 = new Solve1();

        solve1.Solve(a, b);

        Sovle2 solve2 = new Sovle2();
        
        solve2.Solve(a, b);
    }


    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain(4, 8);
        SolveMain(8, 9);
    }
    
    static void Main()
    {
        Run();
    }    
}