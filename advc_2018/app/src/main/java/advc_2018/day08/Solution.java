package advc_2018.day08;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day08");

    public long solve1(List<String> lines)
    {
        var poptree = new Poptree(lines.getFirst());
        return poptree.getSumMetadata();
    }

    public long solve2(List<String> lines)
    {
        var poptree = new Poptree(lines.getFirst());
        return poptree.getMetaValue();
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerChecker(solve1(lines), 48155);
        m_helper.answerChecker(solve2(lines), 40292);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 138);
        m_helper.answerCheckerTestInput(solve2(lines), 66);
    }
}

