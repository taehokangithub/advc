package advc_2018.day00_template;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day00");

    public long solve1(List<String> lines)
    {
        return 0;
    }

    public long solve2(List<String> lines)
    {
        return 0;
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 0);
        m_helper.answerCheckerDontThrow(solve2(lines), 0);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 0);
        m_helper.answerCheckerTestInput(solve2(lines), 0);
    }

}
