package advc_2018.day06;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

import org.junit.jupiter.api.Test;
import static org.junit.jupiter.api.Assertions.*;

import java.util.List;

public class SolutionTest 
{   
    private IAdvcHelper m_helper = new AdvcHelper("day06");
    private List<String> m_lines;

    public SolutionTest()
    {
        m_lines = m_helper.readLinesFromFile("input_test.txt");
    }

    @Test
    void testInput()
    {
        m_helper.answerCheckerTestInput(Solution.solve1(m_lines), 17);
        m_helper.answerCheckerTestInput(Solution.solve2(m_lines), 0);
    }

    @Test
    void testParser()
    {
        var map = AreaMapParser.parse(m_lines);
        assertTrue(map.getCount() == 6);
    }
    
}
