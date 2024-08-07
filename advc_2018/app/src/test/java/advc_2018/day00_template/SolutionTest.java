package advc_2018.day00_template;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

import org.junit.jupiter.api.Test;

public class SolutionTest 
{
    @Test
    void testInput()
    {
        IAdvcHelper helper = new AdvcHelper("day00");
        var lines = helper.readLinesFromFile("input_test.txt");
        
        helper.answerCheckerTestInput(Solution.solve1(lines), 0);
        helper.answerCheckerTestInput(Solution.solve2(lines), 0);
    }
}
