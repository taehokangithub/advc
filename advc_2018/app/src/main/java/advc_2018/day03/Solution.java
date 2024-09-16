package advc_2018.day03;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day03");

    public long solve1(List<String> lines, Claims claims)
    {
        return claims.getMultipleClaimedTiles();
    }

    public long solve2(List<String> lines, Claims claims)
    {
        return claims.getSingleClaimedID();
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");
        var claims = new Claims(lines);

        final var ans1 = solve1(lines, claims);
        final var ans2 = solve2(lines, claims);

        m_helper.answerChecker(ans1, 107043);
        m_helper.answerChecker(ans2, 346);
    }

    @Test
    void testInput()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");
        var claims = new Claims(lines);
        
        m_helper.answerCheckerTestInput(solve1(lines, claims), 4);
        m_helper.answerCheckerTestInput(solve2(lines, claims), 3);
    }
}
