using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2022
{
    class Problem18 : Loggable
    {
        class CubeMap : Loggable
        {
            private HashSet<Point> m_cubeSet;
            private Point m_minCubePos = Point.MaxPoint;
            private Point m_maxCubePos = Point.MinPoint;
            private HashSet<Point> m_exposed = new();
            private HashSet<Point> m_trapped = new();

            public CubeMap(List<Point> cubes)
            {
                m_cubeSet = new(cubes);

                foreach (var cube in cubes)
                {
                    m_minCubePos.SetMin(cube);
                    m_maxCubePos.SetMax(cube);
                }

                LogDetail($"min {this.m_minCubePos} max {this.m_maxCubePos}");
            }

            private bool IsExposedSimple(Point p, out bool isExpoed)
            {
                if (m_exposed.Contains(p))
                {
                    isExpoed = true;
                    return true;
                }
                if (m_trapped.Contains(p))
                {
                    isExpoed = false;
                    return true;
                }

                if (p.x > m_maxCubePos.x || p.y > m_maxCubePos.y || p.z > m_maxCubePos.z
                    || p.x < m_minCubePos.x || p.y < m_minCubePos.y || p.z < m_minCubePos.z)
                {
                    isExpoed = true;
                    return true;
                }

                isExpoed = default;
                return false;
            }

            private bool IsExposed(Point p)
            {
                if (m_cubeSet.Contains(p))
                {
                    return false;
                }
                if (IsExposedSimple(p, out var isExposed))
                {
                    return isExposed;
                }

                HashSet<Point> visited = new();
                Queue<Point> searchQ = new();
                searchQ.Enqueue(p);
                visited.Add(p);

                LogDetail($"[IsExposed] examining {p}");
                while (searchQ.Any())
                {
                    p = searchQ.Dequeue();
                    foreach (Point dir in Direction.DirVectors3D)
                    {
                        Point target = p.AddedPoint(dir);
                        if (m_cubeSet.Contains(target) || visited.Contains(target))
                        {
                            continue;
                        }
                        visited.Add(target);
                        if (IsExposedSimple(target, out isExposed))
                        {
                            searchQ.Clear();
                            break;
                        }
                        searchQ.Enqueue(target);
                        LogDetail($"     [IsExposed] enqueue {target}, queuesize {searchQ.Count}"); 
                    }
                }

                if (isExposed)
                {
                    m_exposed = m_exposed.Concat(visited).ToHashSet();
                    LogDetail($"     Found exposed. Added {visited.Count} total {m_exposed.Count}");
                }
                else
                {
                    m_trapped = m_trapped.Concat(visited).ToHashSet();
                    LogDetail($"     Found trapped. Added {visited.Count} total {m_trapped.Count}");
                }
                return isExposed;
            }

            public int CountNonAdjacent()
            {
                return m_cubeSet.Sum(c => 
                    Direction.DirVectors3D.Count(d => !m_cubeSet.Contains(d.AddedPoint(c)))
                );
            }

            public int CountOutsideExposed()
            {
                return m_cubeSet.Sum(c => 
                    Direction.DirVectors3D.Count(d => IsExposed(d.AddedPoint(c)))
                );
            }
        }

        private int Solve1(CubeMap cubMap)
        {
            cubMap.AllowLogDetail = false;

            return cubMap.CountNonAdjacent();
        }

        private int Solve2(CubeMap cubMap)
        {
            cubMap.AllowLogDetail = false;

            return cubMap.CountOutsideExposed();
        }
        
        public static void Start()
        {
            Point.LogDimension = 3;
            var textData = File.ReadAllText("data/input18.txt");
            var lines = textData.Split(Environment.NewLine);

            List<Point> cubes = new();
            foreach (var line in lines)
            {
                var coords = line.Split(",").Select(c => int.Parse(c)).ToArray();
                cubes.Add(new(coords[0], coords[1], coords[2]));
            }

            CubeMap cubeMap = new(cubes);

            Problem18 prob1 = new();

            var ans1 = prob1.Solve1(cubeMap);
            var ans2 = prob1.Solve2(cubeMap);

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }

    }
}


