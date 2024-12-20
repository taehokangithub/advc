package advc_2024.day14;

import advc_utils.Etc.*;
import advc_utils.Points.IPoint;
import advc_utils.Points.Point;

import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day14");

    private IPoint m_realSize = new Point(101, 103);
    private IPoint m_testSize = new Point(11, 7);

    public long solve1(List<String> lines, IPoint areaSize)
    {
        return new Robots(lines, areaSize).getSafetyFactor(100);
    }

    public long solve2(List<String> lines, IPoint areaSize)
    {
        return new Robots(lines, areaSize).findXMasTree();
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines, m_realSize), 216027840);
        m_helper.answerCheckerDontThrow(solve2(lines, m_realSize), 6876);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines, m_testSize), 12);
    }

}
