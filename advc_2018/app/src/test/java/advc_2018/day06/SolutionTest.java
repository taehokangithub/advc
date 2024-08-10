package advc_2018.day06;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

import org.junit.jupiter.api.Test;

public class SolutionTest 
{   
    private IAdvcHelper m_helper = new AdvcHelper("day06");

    public SolutionTest()
    {
        
    }

    @Test
    void testInput()
    {   
        var lines = m_helper.readLinesFromFile("input_test.txt");

        AreaMap areaMap = new AreaMap(lines);
        areaMap.analyse(32);

        m_helper.answerCheckerTestInput(Solution.solve1(lines, areaMap), 17);
        m_helper.answerCheckerTestInput(Solution.solve2(lines, areaMap), 16);
    }
   
}
