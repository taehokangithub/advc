package advc_2024.day08;

import advc_2024.day08.FrequencyManager.Version;
import advc_utils.Etc.*;

import java.util.List;
import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day08");

    public long solve1(List<String> lines)
    {
        return new FrequencyManager(lines).getCountAntinodes(Version.Ver1);
    }

    public long solve2(List<String> lines)
    {
        return new FrequencyManager(lines).getCountAntinodes(Version.Ver2);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 280);
        m_helper.answerCheckerDontThrow(solve2(lines), 958);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 14);
        m_helper.answerCheckerTestInput(solve2(lines), 34);
    }

}
