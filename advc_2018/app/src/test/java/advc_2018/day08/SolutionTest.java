package advc_2018.day08;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;
import org.junit.jupiter.api.Test;

public class SolutionTest 
{
    @Test
    void testParsse()
    {
        IAdvcHelper helper = new AdvcHelper("day08");
        var lines = helper.readLinesFromFile("input_test.txt");

        helper.answerCheckerTestInput(Solution.solve1(lines), 138);
        helper.answerCheckerTestInput(Solution.solve2(lines), 66);
    }
}
