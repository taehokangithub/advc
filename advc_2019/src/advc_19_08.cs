using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advc2019
{
    class Layer
    {
        public enum Pixel
        {
            Black = 0,
            White = 1,
            Transparent = 2
        }

        private MapByList<Pixel> m_map = new();
        public IReadOnlyDictionary<Pixel, int> CntPerDigit { get; private set; }

        public Layer(int width, int height, Queue<char> data)
        {
            m_map.SetMax(width, height);

            Dictionary<Pixel, int> cntPerDigit = new();

            for (int h = 0; h < height; h ++)
            {
                for (int w = 0; w < width; w ++)    
                {
                    Pixel v = (Pixel) int.Parse(data.Dequeue().ToString());
                    m_map.SetAt(v, w, h);

                    if (!cntPerDigit.ContainsKey(v))
                    {
                        cntPerDigit[v] = 0;
                    }
                    cntPerDigit[v] ++;
                }
            }

            CntPerDigit = cntPerDigit;
        }

        public Pixel GetAt(int x, int y)
        {
            return  m_map.GetAt(x, y);
        }

        public void Merge(Layer other)
        {
            for (int y = 0; y < m_map.Max.y; y ++)
            {
                for (int x = 0; x < m_map.Max.x; x ++)    
                {
                    var myVal = m_map.GetAt(x, y);
                    var otherVal = other.GetAt(x, y);

                    if (myVal == Pixel.Transparent)
                    {
                        m_map.SetAt(otherVal, x, y);
                    }
                }
            }
        }

        public void Print()
        {
            Console.WriteLine($"-----------Print Layer------------");
            m_map.ForEach((pixel, p) =>
            {
                if (p.x == 0)
                {
                    Console.WriteLine("");
                }
                char c = (pixel == Pixel.White ? 'O'
                        : pixel == Pixel.Black ? ' '
                        : '?');
                Console.Write(c);
            });
        }
    }
    class Problem08
    {
        const int Width = 25;
        const int Height = 6;

        public static int Solve1(string str)
        {
            Queue<char> data = new(str.ToCharArray());

            int minZeroCount = int.MaxValue;
            int ans = 0;

            while (data.Count > 0)
            {
                Layer layer = new(Width, Height, data);
                if (layer.CntPerDigit[Layer.Pixel.Black] < minZeroCount)
                {
                    minZeroCount = layer.CntPerDigit[Layer.Pixel.Black];
                    ans = layer.CntPerDigit[Layer.Pixel.White] * layer.CntPerDigit[Layer.Pixel.Transparent];
                }
            }

            return ans;
        }

        public static int Solve2(string str)
        {
            Layer? baseLayer = null;

            Queue<char> data = new(str.ToCharArray());

            while (data.Count > 0)
            {
                Layer layer = new(Width, Height, data);
                
                if (baseLayer == null)
                {
                    baseLayer = layer;
                }
                else
                {
                    baseLayer.Merge(layer);
                }
            }

            baseLayer!.Print();
            return 0;
        }

        public static void Start()
        {
            var textData = File.ReadAllText("data/input08.txt");

            int ans1 = Problem08.Solve1(textData);
            int ans2 = Problem08.Solve2(textData);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


