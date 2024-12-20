package advc_2024.day12;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day12");

    public long solve1(List<String> lines)
    {
        return new Regions(lines, false).getAllRegionsCost();
    }

    public long solve2(List<String> lines)
    {
        return new Regions(lines, true).getAllRegionsCost();
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 1363682);
        m_helper.answerCheckerDontThrow(solve2(lines), 787680);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 1930);
        m_helper.answerCheckerTestInput(solve2(lines), 1206);
    }

}
