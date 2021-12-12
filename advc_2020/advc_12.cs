
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

class Advc20_12
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }
	
	enum CommandType
	{
		Right, Left, East, West, South, North, Forward
	}

	enum Dir
	{
		East, South, West, North
	}

	const int s_maxDir = 4;
	const int s_oneDirAngle = 90;

	static readonly List<char> CommandLiteral = new List<char> {'R','L','E','W','S','N','F'};

	private static Dir Rotate(int angle, Dir fromDir)
	{
		int curDir = (int) fromDir;
		int movement = (Math.Abs(angle) / s_oneDirAngle) % s_maxDir;

		if (angle < 0)
		{
			movement = s_maxDir - movement;
		}

		Dir newDir = (Dir) ((curDir + movement) % s_maxDir);

		debugWrite($"Rotate from {fromDir} by {angle}, now {newDir}");
		return newDir;
	}

	class Point
	{
		public int x { get; set; } = 0;
		public int y { get; set; } = 0;
		public override string ToString()
		{
			return $"[{x}:{y}]";
		}

		public void Move(Dir dir, int dist)
		{
			switch (dir)
			{
				case Dir.East : x += dist; break;
				case Dir.West : x -= dist; break;
				case Dir.North : y -= dist; break;
				case Dir.South : y += dist; break;
			}

			debugWrite($"Moving {dir} by {dist}, now {this}");			
		}

		public void Rotate(int angle)
		{
			Dir dir = Advc20_12.Rotate(angle, Dir.North);

			int newX = x;
			int newY = y;

			switch(dir)
			{
				case Dir.East: newX = -y; newY = x; break; 
				case Dir.West: newX = y; newY = -x; break;
				case Dir.North: break;
				case Dir.South: newX = -x; newY = -y; break;
				default:
					break;
			}

			x = newX;
			y = newY;
		}
	}

	class Command
	{
		public CommandType Type { get; }
		public int Value { get; }

		public Command(string line)
		{
			int commandIndex = CommandLiteral.IndexOf(line[0]);

			if (commandIndex < 0)
			{
				throw new Exception($"Unknown command {line[0]}");
			}

			Type = (CommandType) commandIndex;
			Value = int.Parse(line.Substring(1));
		}

		public override string ToString()
		{
			return $"<{Type}/{Value}>";
		}
	}
	class Ship
	{
		private List<Command> m_commands = new List<Command>();
		private Dir m_dir = Dir.East;
		private Point m_location = new Point();
		private Point m_wayPoint = new Point{x = 10, y = -1};

		public void AddCommand(string line)
		{
			m_commands.Add(new Command(line));
		}

		private void MoveShip(Dir dir, int dist)
		{
			m_location.Move(dir, dist);
		}

		private void MoveShipToWaypoint(int dist)
		{
			m_location.x += m_wayPoint.x * dist;
			m_location.y += m_wayPoint.y * dist;
		}

		private void RotateShip(int angle)
		{
			m_dir = Rotate(angle, m_dir);
		}

		private void ExecuteCommand(Command command)
		{
			debugWrite($"Executing command {command}, cur location {m_location}");
			switch (command.Type)
			{
				case CommandType.East: MoveShip(Dir.East, command.Value); break;
				case CommandType.West: MoveShip(Dir.West, command.Value); break;
				case CommandType.North: MoveShip(Dir.North, command.Value); break;
				case CommandType.South: MoveShip(Dir.South, command.Value); break;
				case CommandType.Forward: MoveShip(m_dir, command.Value); break;
				case CommandType.Right: RotateShip(command.Value); break;
				case CommandType.Left: RotateShip(-command.Value); break;
			}
		}

		private void ExecuteCommandPart2(Command command)
		{
			debugWrite($"Executing command part 2{command}, cur location {m_location}");
			switch (command.Type)
			{
				case CommandType.East: m_wayPoint.Move(Dir.East, command.Value); break;
				case CommandType.West: m_wayPoint.Move(Dir.West, command.Value); break;
				case CommandType.North: m_wayPoint.Move(Dir.North, command.Value); break;
				case CommandType.South: m_wayPoint.Move(Dir.South, command.Value); break;
				case CommandType.Forward: MoveShipToWaypoint(command.Value); break;
				case CommandType.Right: m_wayPoint.Rotate(command.Value); break;
				case CommandType.Left: m_wayPoint.Rotate(-command.Value); break;
			}		
		}

		public void Run()
		{
			foreach(var command in m_commands)
			{
				ExecuteCommand(command);
			}

			int ans1 = Math.Abs(m_location.x) + Math.Abs(m_location.y);
			Console.WriteLine($"Part 1 Current location {m_location}, ans {ans1} ");
		}

		public void RunPart2()
		{
			foreach(var command in m_commands)
			{
				ExecuteCommandPart2(command);
			}

			int ans2 = Math.Abs(m_location.x) + Math.Abs(m_location.y);
			Console.WriteLine($"Part 2 Current location {m_location}, ans {ans2} ");
		}
	}

    static void solve(string path)
    {
        var lines = File.ReadLines(path);

		Ship ship1 = new Ship();
		Ship ship2 = new Ship();
        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
				ship1.AddCommand(line);
				ship2.AddCommand(line);
			}
        }

		ship1.Run();
		ship2.RunPart2();
    }

    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        solve($"../../data/{className}_sample.txt");
        solve($"../../data/{className}.txt");
    }

    static void Main()
    {
        Run();
    }    
}