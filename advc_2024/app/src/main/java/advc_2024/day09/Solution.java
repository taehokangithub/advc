package advc_2024.day09;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

import advc_2024.day09.DiskSpace.Version;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day09");

    public long solve1(List<String> lines)
    {
        return new DiskSpace(lines).getDefragmentedChecksum(Version.ver1);
    }

    public long solve2(List<String> lines)
    {
        return new DiskSpace(lines).getDefragmentedChecksum(Version.ver2);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 6401092019345l);
        m_helper.answerCheckerDontThrow(solve2(lines), 6431472344710l);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 1928);
        m_helper.answerCheckerTestInput(solve2(lines), 2858);
    }

}
