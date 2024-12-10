package advc_2024.day10;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

import advc_2024.day10.Trail.Version;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day10");

    public long solve1(List<String> lines)
    {
        return new Trail(lines).getTotalTrailheadsScores(Version.ver1);
    }

    public long solve2(List<String> lines)
    {
        return new Trail(lines).getTotalTrailheadsScores(Version.ver2);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 659);
        m_helper.answerCheckerDontThrow(solve2(lines), 1463);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 36);
        m_helper.answerCheckerTestInput(solve2(lines), 81);
    }
}
