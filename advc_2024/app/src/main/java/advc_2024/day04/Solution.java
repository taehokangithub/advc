package advc_2024.day04;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day04");

    public long solve1(List<String> lines)
    {
        return new WordSearch(lines).findAllWord("XMAS");
    }

    public long solve2(List<String> lines)
    {
        return new WordSearch(lines).findAllXWords("MAS");
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 2530);
        m_helper.answerCheckerDontThrow(solve2(lines), 1921);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 18);
        m_helper.answerCheckerTestInput(solve2(lines), 9);
    }

}
