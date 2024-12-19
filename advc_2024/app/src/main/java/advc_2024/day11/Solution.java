package advc_2024.day11;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day11");

    public long solve1(List<String> lines)
    {
        return new Stones(lines.get(0)).getNumStonesAfterBlink(25);
    }

    public long solve2(List<String> lines)
    {
        return new Stones(lines.get(0)).getNumStonesAfterBlink(75);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 194482);
        m_helper.answerCheckerDontThrow(solve2(lines), 232454623677743l);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 55312);
        //m_helper.answerCheckerTestInput(solve2(lines), 0);
    }

}
