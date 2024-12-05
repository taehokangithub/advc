package advc_2024.day05;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day05");

    public long solve1(List<String> lines)
    {
        var manager = new UpdateManager(lines);

        return manager.getSatisfiedSequencesMidNumberSum();
    }

    public long solve2(List<String> lines)
    {
        var manager = new UpdateManager(lines);

        return manager.getFixedSequenceMidNumerSum();
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 6951);
        m_helper.answerCheckerDontThrow(solve2(lines), 4121);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 143);
        m_helper.answerCheckerTestInput(solve2(lines), 123);
    }

}
