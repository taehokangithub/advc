package advc_2024.day07;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day07");

    public long solve1(List<String> lines)
    {
        return new TestNumsManager(lines).getTotalCalibrationResult(false);
    }

    public long solve2(List<String> lines)
    {
        return new TestNumsManager(lines).getTotalCalibrationResult(true);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 5837374519342L);
        m_helper.answerCheckerDontThrow(solve2(lines), 492383931650959L);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 3749);
        m_helper.answerCheckerTestInput(solve2(lines), 11387);
    }

}
