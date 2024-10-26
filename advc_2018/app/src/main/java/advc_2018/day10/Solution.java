package advc_2018.day10;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day10");

    public long solve1(List<String> lines)
    {
        var pol = new PointsOfLights(lines);
        pol.TryFindLetter();    // This will print the letter answers
        return 0;
    }

    public long solve2(List<String> lines)
    {
        return 0;
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");

        var pol = new PointsOfLights(lines);
        final int seconds = pol.TryFindLetter();    // This will print the letter answers

        m_helper.answerChecker(0, 0);
        System.out.println(pol.toString());
        m_helper.answerChecker(seconds, 10274);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        var pol = new PointsOfLights(lines);
        final int seconds = pol.TryFindLetter();    // This will print the letter answers
        m_helper.answerCheckerTestInput(seconds, 3);
    }

}
