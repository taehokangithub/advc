package advc_2018.day05;

import advc_utils.Etc.*;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day05");
    
    public long solve1(String line)
    {
        return Reduction.reduce(new StringBuilder(line));
    }

    public long solve2(String line)
    {
        return Reduction.reduceBestStrategy(new StringBuilder(line));
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerChecker(solve1(lines.getFirst()), 10804);
        m_helper.answerChecker(solve2(lines.getFirst()), 6650);
    }

    @Test
    void testInput()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");
        
        m_helper.answerCheckerTestInput(solve1(lines.getFirst()), 10);
        m_helper.answerCheckerTestInput(solve2(lines.getFirst()), 4);
    }    
}
