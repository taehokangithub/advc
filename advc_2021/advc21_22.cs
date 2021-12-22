
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_22
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    const int MINPOS = -50;
    const int MAXPOS = 50;
    static readonly Point MinPoint = new Point(MINPOS, MINPOS, MINPOS);
    static readonly Point MaxPoint = new Point(MAXPOS, MAXPOS, MAXPOS);
    static readonly Cuboid MaxCuboid = new Cuboid(MinPoint, MaxPoint);

    class Point
    {
        public int X { get; set; } = 0;
        public int Y { get; set; } = 0;
        public int Z { get; set; } = 0;
        
        public Point() 
        {
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
            Z = other.Z;
        }

        public Point(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Point operator -(Point a, Point b)
        {
            Point p = new Point();

            p.X = a.X - b.X;
            p.Y = a.Y - b.Y;
            p.Z = a.Z - b.Z;

            return p;            
        }

        public static Point operator +(Point a, Point b)
        {
            Point p = new Point();

            p.X = a.X + b.X;
            p.Y = a.Y + b.Y;
            p.Z = a.Z + b.Z;

            return p;
        }

        public static bool operator <(Point a, Point b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        public static bool operator >(Point a, Point b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }
        public static bool operator <=(Point a, Point b)
        {
            return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
        }

        public static bool operator >=(Point a, Point b)
        {
            return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        }

#if false
        public static bool operator ==(Point a, Point b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Point a, Point b)
        {
            return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }
#endif
        public void SetMax(Point p)
        {
            X = Math.Max(X, p.X);
            Y = Math.Max(Y, p.Y);
            Z = Math.Max(Z, p.Z);
        }

        public void SetMin(Point p)
        {
            X = Math.Min(X, p.X);
            Y = Math.Min(Y, p.Y);
            Z = Math.Min(Z, p.Z);
        }

        public override string ToString()
        {
            return $"[{X},{Y},{Z}]";
        }
    }

    class Range
    {
        public int Start { get; }
        public int End { get; }
        public long Count { get => End - Start + 1; }

        public Range(int a, int b)
        {
            Start = a;
            End = b;
        }

        public Range(Range other)
        {
            Start = other.Start;
            End = other.End;
        }

        public bool IsValid()
        {
            return Start <= End;
        }

        public Range Intersection(Range other)
        {
            Range range = new Range(Math.Max(Start, other.Start), Math.Min(End, other.End));
                        
            return range;
        }

        public static Range CreateIntersection(Range a, Range b)
        {
            return a.Intersection(b);
        }

        public List<Range> Substraction(Range other)
        {
            var result = new List<Range>();

            if (Start < other.Start)
            {
                result.Add(new Range(Start, other.Start - 1));
            }
            if (End > other.End)
            {
                result.Add(new Range(other.End + 1, End));
            }

            return result;
        }
    }

    class Cuboid
    {
        public Point Start { get; }
        public Point End { get; }
        public Range XRange { get; private set; }
        public Range YRange { get; private set; }
        public Range ZRange { get; private set; }
        public long Count { get => XRange.Count * YRange.Count * ZRange.Count; }

        public Cuboid(Point minPoint, Point maxPoint)
        {
            Start = new Point(minPoint);
            End = new Point(maxPoint);
            XRange = new Range(Start.X, End.X);
            YRange = new Range(Start.Y, End.Y);
            ZRange = new Range(Start.Z, End.Z);
        }

        public Cuboid(Range xRange, Range yRange, Range zRange)
        {
            Start = new Point(xRange.Start, yRange.Start, zRange.Start);
            End = new Point(xRange.End, yRange.End, zRange.End);
            XRange = new Range(xRange);
            YRange = new Range(yRange);
            ZRange = new Range(zRange);
        }

        public Cuboid(Cuboid other)
        {
            Start = new Point(other.Start);
            End = new Point(other.End);
            XRange = new Range(Start.X, End.X);
            YRange = new Range(Start.Y, End.Y);
            ZRange = new Range(Start.Z, End.Z);            
        }
        
        public bool IsValid()
        {
            return Start <= End;
        }

        public bool In(Point p)
        {
            return p >= Start && p <= End;
        }

        public override string ToString()
        {
            return $"({Start} - {End})";
        }

        public Cuboid Intersection(Cuboid other)
        {
            Range xRange = Range.CreateIntersection(new Range(Start.X, End.X), new Range(other.Start.X, other.End.X));
            Range yRange = Range.CreateIntersection(new Range(Start.Y, End.Y), new Range(other.Start.Y, other.End.Y));
            Range zRange = Range.CreateIntersection(new Range(Start.Z, End.Z), new Range(other.Start.Z, other.End.Z));

            return new Cuboid(xRange, yRange, zRange);
        }

        public void ForEach(Func<Point, bool> callback) // return : continue operation (false means stop foreach)
        {
            for (int x = Start.X; x <= End.X; x ++)
            {
                for (int y = Start.Y; y <= End.Y; y ++)
                {
                    for (int z = Start.Z; z <= End.Z; z++)
                    {
                        Point p = new Point(x, y, z);
                        if (!callback(p))
                        {
                            return;
                        }
                    }
                }
            }
        }

        public List<Cuboid> Substraction(Cuboid otherOrg)
        {
            List<Cuboid> result = new List<Cuboid>();
            Cuboid other = this.Intersection(otherOrg);

            if (other.IsValid())
            {
                foreach(Range xRange in this.XRange.Substraction(other.XRange))
                {
                    result.Add(new Cuboid(xRange, this.YRange, this.ZRange));
                }

                foreach(Range yRange in this.YRange.Substraction(other.YRange))
                {
                    result.Add(new Cuboid(other.XRange, yRange, this.ZRange));
                }

                foreach(Range zRange in this.ZRange.Substraction(other.ZRange))           
                {
                    result.Add(new Cuboid(other.XRange, other.YRange, zRange));
                }
            }
            else
            {
                result.Add(new Cuboid(this));
            }

            return result;
        }
    }

    class Operation
    {
        public bool OnOff { get; }
        public Cuboid Cuboid { get; }

        public Operation(string line)
        {
            string[] inputs = line.Split(' ');

            OnOff = inputs[0] == "on";

            string[] locStrs = inputs[1].Split(',');
            string[] locRangeX = locStrs[0].Split('=')[1].Split("..");
            string[] locRangeY = locStrs[1].Split('=')[1].Split("..");
            string[] locRangeZ = locStrs[2].Split('=')[1].Split("..");

            Point minPoint = new Point(int.Parse(locRangeX[0]), int.Parse(locRangeY[0]), int.Parse(locRangeZ[0]));
            Point maxPoint = new Point(int.Parse(locRangeX[1]), int.Parse(locRangeY[1]), int.Parse(locRangeZ[1]));

            Cuboid = new Cuboid(minPoint, maxPoint);
        }

        public override string ToString()
        {
            string onOff = OnOff ? "<On>" : "<Off>";
            return $"{onOff}{Cuboid}";
        }
    }

    class RebootSequence
    {
        HashSet<string> m_onCubes = new HashSet<string>();
        public List<Operation> Operations { get; } = new List<Operation>();
        
        List<Cuboid> m_onCuboids = new List<Cuboid>();

        public void RunOperations()
        {
            foreach (Operation operation in Operations)
            {
                RunOperationFancier(operation, MaxCuboid);
            }

            Console.WriteLine($"[Solve 1] Total cubes on : {CountCubesFromCuboids()}");

            foreach (Operation operation in Operations)
            {
                RunOperationFancier(operation, null);
            }

            Console.WriteLine($"[Solve 2] Total cubes on : {CountCubesFromCuboids()}");
        }

        void RunOperationFancier(Operation operation, Cuboid? clipMaxCuboid)
        {
            debugWrite($"\n* Operation {operation}");

            Cuboid validArea = new Cuboid(operation.Cuboid);
            if (clipMaxCuboid != null)
            {
                validArea = validArea.Intersection(clipMaxCuboid);
            }

            if (!validArea.IsValid())
            {
                return;
            }

            if (operation.OnOff)
            {
                List<Cuboid> areasToAdd = new List<Cuboid>();
                areasToAdd.Add(validArea);

                foreach (Cuboid cuboidAlreadyIn in m_onCuboids)
                {
                    List<Cuboid> newAreas = new List<Cuboid>();

                    foreach (Cuboid area in areasToAdd)
                    {
                        newAreas.AddRange(area.Substraction(cuboidAlreadyIn));
                    }

                    areasToAdd = newAreas;
                }

#if false
                foreach (Cuboid area in areasToAdd)
                {
                    debugWrite($"Adding {area} from {validArea}");
                }
#endif                
                m_onCuboids.AddRange(areasToAdd);
            }
            else
            {
                List<Cuboid> newOnCuboids = new List<Cuboid>();
                foreach (Cuboid cuboid in m_onCuboids)
                {
                    List<Cuboid> subtracted = cuboid.Substraction(validArea);
#if false                    
                    foreach (Cuboid area in subtracted)
                    {
                        debugWrite($"Reamining {area} = {cuboid} substracted by {validArea}");
                    }
#endif
                    newOnCuboids.AddRange(subtracted);
                }

#if false
                foreach (Cuboid area in newOnCuboids)
                {
                    debugWrite($"Final Reamining {area}");
                }
#endif                
                m_onCuboids = newOnCuboids;
            }
        }

        long CountCubesFromCuboids()
        {
            long cnt =  0;
            foreach (Cuboid cuboid in m_onCuboids)
            {
                cnt += cuboid.Count;
            }
            return cnt;
        }

        void RunOperation(Operation operation)
        {
            Cuboid validArea = operation.Cuboid.Intersection(MaxCuboid);

            if (!validArea.IsValid())
            {
                debugWrite($"Skipping invalid operation {operation}");
                return;
            }

            if (operation.OnOff)
            {
                validArea.ForEach((point) =>
                {
                    m_onCubes.Add(point.ToString());
                    return true;
                });
            }
            else
            {
                validArea.ForEach((point) =>
                {
                    m_onCubes.Remove(point.ToString());
                    return true;
                });
            }
        }
    }
    
    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        RebootSequence seq = new RebootSequence();

        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                Operation op = new Operation(line);
                seq.Operations.Add(op);
            }
        }

        seq.RunOperations();
    }

    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        SolveMain($"../../data/{className}_sample2.txt");
        SolveMain($"../../data/{className}.txt");
    }
    
    static void Main()
    {
        Run();
    }    
}