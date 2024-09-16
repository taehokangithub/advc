package advc_2018.day04;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day04");

    public long solve1(List<String> lines, RecordManager manager)
    {
        return manager.getMostLikelySleepGuarByCount();
    }

    public long solve2(List<String> lines, RecordManager manager)
    {
        return manager.getMostLikelySleepGuardByMinute();
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");
        var manager = new RecordManager(lines);

        m_helper.answerChecker(solve1(lines, manager), 38813);
        m_helper.answerChecker(solve2(lines, manager), 141071);
    }

    @Test
    void testInput()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");
        var manager = new RecordManager(lines);

        m_helper.answerCheckerTestInput(solve1(lines, manager), 240);
        m_helper.answerCheckerTestInput(solve2(lines, manager), 4455);
    }    
}
