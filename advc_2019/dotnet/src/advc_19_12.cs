using Advc.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Advc2019
{
    class Star : MovingObject
    {
        private static int s_index = 0;
        public int Index { get; set; }
        public Star(int x, int y, int z)
        {
            Index = s_index ++;
            Position = new Point(x, y, z);
        }
    }

    class Problem12: Advc.Utils.Loggable
    {
        private List<Star> m_stars = new();

        public Problem12()
        {
#if false // test 
            m_stars.Add(new Star(-1, 0, 2));
            m_stars.Add(new Star(2, -10, -7));
            m_stars.Add(new Star(4, -8, 8));
            m_stars.Add(new Star(3, 5, -1));
#else
            m_stars.Add(new Star(-14, -4, -11));
            m_stars.Add(new Star(-9, 6, -7));
            m_stars.Add(new Star(4, 1, 4));
            m_stars.Add(new Star(2, -14, -9));
#endif
        }

        private void ProcessStep()
        {
            CalculateVelocity();
            MoveAll();
        }

        private void CalculateVelocity()
        {
            foreach (var star in m_stars)
            {
                foreach (var target in m_stars)
                {
                    if (star.Index >= target.Index)
                    {
                        continue;
                    }
                    
                    Point starVelChange = new();

                    starVelChange.x = (star.Position.x > target.Position.x) ? -1 : (star.Position.x < target.Position.x) ? 1 : 0;
                    starVelChange.y = (star.Position.y > target.Position.y) ? -1 : (star.Position.y < target.Position.y) ? 1 : 0;
                    starVelChange.z = (star.Position.z > target.Position.z) ? -1 : (star.Position.z < target.Position.z) ? 1 : 0;

                    Point targetVelChange = starVelChange.MultipliedPoint(-1);

                    star.Velocity.Add(starVelChange);
                    target.Velocity.Add(targetVelChange);

                    //LogDetail($"[{star.Index}] vs [{target.Index}], starVelChange {starVelChange} => {star.Velocity}, targetVelChange {targetVelChange} => {target.Velocity}");
                }
            }
        }

        private void MoveAll()
        {
            foreach (var star in m_stars)
            {
                star.Move();
            }
        }

        private long CalculatePower()
        {
            long sum = 0;
            foreach (var star in m_stars)
            {
                long posPower = star.Position.ManhattanDistance();
                long velPower = star.Velocity.ManhattanDistance();

                sum += posPower * velPower;
                this.LogDetail($"[{star.Index}] Pos {star.Position} Vel {star.Velocity} posPwr {posPower} velPwr {velPower} total {sum}");
            }
            return sum;
        }

        private string SnapShotAxis(Point.Axis axis, bool findOnlyZeroVel = true)
        {
            StringBuilder sbPos = new();
            StringBuilder sbVel = new();
            foreach (var star in m_stars)
            {
                int p = star.Position.GetAxisValue(axis);
                int v = star.Velocity.GetAxisValue(axis);

                if (findOnlyZeroVel && v != 0)
                {
                    return ""; // return proper snapshop only when velocity is 0
                }

                sbPos.Append($"[{p}]");
                sbVel.Append($"[{v}]");
            }
            return sbPos.ToString() + sbVel.ToString();
        }

        public long Solve1()
        {
            AllowLogDetail = false;
            const int steps = 1000;

            for (int i = 0; i < steps; i ++)
            {
                ProcessStep();
            }

            return CalculatePower();
        }

        public long Solve2()
        {
            AllowLogDetail = false;
            long steps = 0;
            Dictionary<Point.Axis, long> axisAnswers = new();
            Dictionary<Point.Axis, string> axisStates = new();

            var handleEachAxisState = (Point.Axis axis) =>
            {
                if (!axisAnswers.ContainsKey(axis))
                {
                    string curState = SnapShotAxis(axis);

                    if (curState.Length != 0)
                    {
                        if (!axisStates.ContainsKey(axis))
                        {
                            axisStates[axis] = curState; // store initial state
                        }
                        else if (axisStates[axis].Contains(curState))
                        {
                            axisAnswers[axis] = steps;
                            LogDetail($"Found for {axis} => {steps} steps, {curState}");
                        }
                    }

                    const int logSteps = 1000000;
                    if (steps % logSteps == 0)
                    {
                        LogDetail($"[{steps}] {axis} state x {SnapShotAxis(axis, false)}");
                    }
                }
            };

            while (true)
            {
                handleEachAxisState(Point.Axis.x);
                handleEachAxisState(Point.Axis.y);
                handleEachAxisState(Point.Axis.z);

                if (axisAnswers.Count == 3)
                {
                    break;
                }

                ProcessStep();
                steps ++;                
            }

            var xa = axisAnswers[Point.Axis.x];
            var xb = axisAnswers[Point.Axis.y];
            var xz = axisAnswers[Point.Axis.z];
            LogDetail($"{xa} {xb} {xz}");

            var gcdAB = GenericMath.GetGCD((int)xa, (int)xb);
            var ansAB = xa / gcdAB * xb;

            var gcdABC = GenericMath.GetGCD((int)ansAB, (int)xz);
            var ans = ansAB / gcdABC * xz;

            LogDetail($"gcdAB {gcdAB} ansAB {ansAB} gcdABC {gcdABC} ans {ans}");
            return ans;
        }

        public static void Start()
        {
            Point.LogDimension = 3;
            Problem12 prob1 = new();
            Problem12 prob2 = new();

            var ans1 = prob1.Solve1();
            var ans2 = prob2.Solve2();

            Console.WriteLine($"ans = {ans1}, {ans2}");
        }
    }
}


