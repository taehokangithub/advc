package advc_2018.day06;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day06");

    public long solve1(List<String> lines, AreaMap areaMap)
    {
        return areaMap.getLargestInternalArea();
    }

    public long solve2(List<String> lines, AreaMap areaMap)
    {
        return areaMap.getSafeAreas();
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");

        AreaMap areaMap = new AreaMap(lines);
        areaMap.analyse(10000);
        
        m_helper.answerChecker(solve1(lines, areaMap), 4887);
        m_helper.answerChecker(solve2(lines, areaMap), 34096);
    }

    @Test
    void test()
    {   
        var lines = m_helper.readLinesFromFile("input_test.txt");

        AreaMap areaMap = new AreaMap(lines);
        areaMap.analyse(32);

        m_helper.answerCheckerTestInput(solve1(lines, areaMap), 17);
        m_helper.answerCheckerTestInput(solve2(lines, areaMap), 16);
    }    
}
