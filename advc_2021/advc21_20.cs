
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;


class Advc21_20
{
    static bool s_debugWrite = false;

    static void debugWrite(string s)
    {
        if (s_debugWrite)
        {
            Console.WriteLine(s);
        }
    }

    static Point[] DIRPOINTS = new Point[] { new Point(-1, -1), new Point(0, -1), new Point(1, -1) 
                                            , new Point(-1, 0), new Point(0, 0), new Point(1, 0)
                                            , new Point(-1, 1), new Point(0, 1), new Point(1, 1) };
    
    static bool DecodeChar(char c)
    {
        Debug.Assert(c == '#' || c == '.');
        return c == '#';
    }

    class Point
    {
        public int X = 0;
        public int Y = 0;
        public bool Val = false;
        
        public Point() 
        {
        }

        public Point(Point other)
        {
            X = other.X;
            Y = other.Y;
            Val = other.Val;
        }

        public Point(int x, int y, bool val = false)
        {
            X = x;
            Y = y;
            Val = val;
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

        public void SetMax(int x, int y)
        {
            X = Math.Max(X, x);
            Y = Math.Max(Y, y);
        }

        public override string ToString()
        {
            return $"[{X},{Y}:{Val}]";
        }
    }

    class Filter
    {
        List<bool> m_filter = new List<bool>();

        public Filter(string line)
        {
            foreach (char c in line)
            {
                m_filter.Add(DecodeChar(c));
            }
        }

        public bool GetValAt(int index) 
        {
            return m_filter[index];
        }
    }

    class Image
    {
        List<List<Point>> m_pixels = new List<List<Point>>();
        public Point Origin { get; } = new Point();
        public Point Max { get; } = new Point();

        public void AddLine(string line)
        {
            List<Point> points = new List<Point>();

            int x = 0;
            int y = m_pixels.Count();

            foreach (char c in line)
            {   
                points.Add(new Point(x ++, y, DecodeChar(c)));
            }

            m_pixels.Add(points);

            Max.SetMax(points.Count, m_pixels.Count);
        }

        public void AddPixel(int x, int y, bool value)
        {
            if (m_pixels.Count == 0)
            {
                m_pixels.Add(new List<Point>());
            }

            List<Point> curLine = m_pixels[m_pixels.Count - 1];
            int curX = curLine.Count;

            if (curX >= Max.X)   // new Line
            {
                curX = 0;
                curLine = new List<Point>();
                m_pixels.Add(curLine);
            }

            int curY = m_pixels.Count - 1;
            Debug.Assert(x == curX && y == curY, $"{x},{y} != {curX},{curY}");

            Point curPoint = new Point(curX, curY, value);
            curLine.Add(curPoint);
        }

        public int GetLightPixelCount()
        {
            int cnt = 0;
            foreach (var points in m_pixels)
            {
                foreach (Point p in points)
                {
                    if (p.Val)
                    {
                        cnt ++;
                    }
                }
            }
            return cnt;
        }

        public override string ToString()
        {
            string str = "";
            foreach (var points in m_pixels)
            {
                foreach (Point p in points)
                {
                    str += (p.Val ? '#' : '.');
                }
                str += "\n";
            }
            return str;
        }

        public void Transform(int x, int y)
        {
            Origin.X = x;
            Origin.Y = y;
        }

        public void SetSize(int x, int y)
        {
            Max.X = x;
            Max.Y = y;
        }

        public bool GetPixelAt(Point p)
        {
            p = p + Origin;

            if (p.X >= 0 && p.X < Max.X && p.Y >= 0 && p.Y < Max.Y)
            {
                return m_pixels[p.Y][p.X].Val;
            }
            return false;
        }
    }

    class ImageProcessor
    {
        Filter m_filter;
        Image m_image;

        public ImageProcessor(Filter filter, Image image)
        {
            m_filter = filter; 
            m_image = image;
        }

        public void Process()
        {
            int solve1ans = 0;
            int solve2ans = 0;

            for (int i = 0; i < 25; i ++)
            {
                ProcessInternal(10);    // Give enough space to contain "infinite" area : outside pixels are all lit (m_filter[0] == true)
                debugWrite(m_image.ToString());

                ProcessInternal(-6);    // Reduce the area not to contain outside lit pixels
                debugWrite(m_image.ToString());

                if (solve1ans == 0)
                {
                    solve1ans = m_image.GetLightPixelCount();
                }
            }

            solve2ans = m_image.GetLightPixelCount();

            Console.WriteLine($"Solve1 Ans = {solve1ans}");
            Console.WriteLine($"Solve2 Ans = {solve2ans}");
        }

        void ProcessInternal(int growSize)
        {
            int transAmount = 0 - (growSize / 2);
            m_image.Transform(transAmount, transAmount);
            Image newImage = new Image();
            newImage.SetSize(m_image.Max.X + growSize, m_image.Max.Y + growSize);

            for (int y = 0; y < newImage.Max.Y; y ++)
            {
                for (int x = 0; x < newImage.Max.X; x ++)
                {
                    int index = GetFilterIndexFromImage(m_image, x, y);
                    bool val = m_filter.GetValAt(index);
                    newImage.AddPixel(x, y, val);
                }
            }

            m_image = newImage;
        }

        int GetFilterIndexFromImage(Image image, int x, int y)
        {
            Point point = new Point(x, y);

            int bitPos = DIRPOINTS.Length;
            int index = 0;

            string dbgStr = "[";
            foreach (Point dirPoint in DIRPOINTS)
            {
                int value = image.GetPixelAt(point + dirPoint) ? 1 : 0;
                index += (value << (--bitPos));

                dbgStr += (value > 0 ? '#' : '.') + (bitPos % 3 == 0 ? "]\n[" : "") ;
            }

            dbgStr = dbgStr.Substring(0, dbgStr.Length - 1);
            debugWrite($"Processing {x},{y} : \n{dbgStr}");
            debugWrite($"{Convert.ToString(index, 2).PadLeft(DIRPOINTS.Length, '0')} ({index}) : {m_filter.GetValAt(index)}\n");

            return index;
        }

    }

    static void SolveMain(string path)
    {
        Console.WriteLine($"Reading File {path}");
        var lines = File.ReadLines(path);

        Filter? filter = null;
        Image image = new Image();
        foreach(string line in lines)
        {
            if (line.Length > 0)
            {
                if (filter == null)
                {
                    filter = new Filter(line);
                }
                else
                {
                    image.AddLine(line);
                }
            }
        }

        if (filter == null)
        {
            throw new Exception("No filter found");
        }

        ImageProcessor imageProcessor = new ImageProcessor(filter, image);
        imageProcessor.Process();
    }


    static void Run()
    {
        var classType = new StackFrame().GetMethod()?.DeclaringType;
        string className = classType != null? classType.ToString() : "Advc";

        Console.WriteLine($"Starting {className}");
        className.ToString().ToLower();

        SolveMain($"../../data/{className}_sample.txt");
        SolveMain($"../../data/{className}.txt"); // 5686 high
    }
    
    static void Main()
    {
        Run();
    }    
}