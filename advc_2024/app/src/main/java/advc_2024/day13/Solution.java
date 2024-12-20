package advc_2024.day13;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day13");

    public long solve1(List<String> lines)
    {
        return new Prizes(lines).getAllPrizes();
    }

    public long solve2(List<String> lines)
    {
        return new Prizes(lines).AddScale(10000000000000l).getAllPrizes();
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 29598);
        m_helper.answerCheckerDontThrow(solve2(lines), 93217456941970l);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 480);
        //m_helper.answerCheckerTestInput(solve2(lines), 0);
    }

}
