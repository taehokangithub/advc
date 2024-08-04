package advc_2018.day03;

import org.junit.jupiter.api.Test;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

public class SolutionTest {
    @Test
    void testInput()
    {
        IAdvcHelper helper = new AdvcHelper("day03");
        var lines = helper.readLinesFromFile("input_test.txt");
        
        helper.answerCheckerTestInput(Solution.solve1(lines, new Claims(lines)), 4);
    }
}
