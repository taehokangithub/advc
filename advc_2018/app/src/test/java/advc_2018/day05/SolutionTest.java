package advc_2018.day05;

import org.junit.jupiter.api.Test;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

import static org.junit.jupiter.api.Assertions.*;

public class SolutionTest 
{
    @Test
    void testInput()
    {
        IAdvcHelper helper = new AdvcHelper("day05");
        var lines = helper.readLinesFromFile("input_test.txt");
        
        helper.answerCheckerTestInput(Solution.solve1(lines.getFirst()), 10);
        helper.answerCheckerTestInput(Solution.solve2(lines.getFirst()), 4);
    }
}
