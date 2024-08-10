package advc_2018.day07;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

public class SolutionTest 
{
    IAdvcHelper m_helper;
    List<String> m_lines;

    @BeforeEach
    void testInit()
    {
        m_helper = new AdvcHelper("day07");
        m_lines = m_helper.readLinesFromFile("input_test.txt");
    }

    @Test
    void testInput()
    {
        m_helper.answerCheckerTestInput(Solution.solve1(m_lines), "CABDFE");
        m_helper.answerCheckerTestInput(Solution.solve2(m_lines, 0, 2), 15);
    }

    @Test
    void testPars()
    {
        JobGraph jg = new JobGraph(m_lines);
        var graph = jg.get();

        assertTrue(graph.getNodes().size() == 6);
    }
}
