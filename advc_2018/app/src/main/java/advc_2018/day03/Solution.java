package advc_2018.day03;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines, Claims claims)
    {
        return claims.getMultipleClaimedTiles();
    }

    public static long solve2(List<String> lines, Claims claims)
    {
        return claims.getSingleClaimedID();
    }

    public static void test(IAdvcHelper helper)
    {
        var lines = helper.readLinesFromFile("input2.txt");

        helper.answerCheckerTestInput(solve1(lines, new Claims(lines)), 4);
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day03");

        test(helper);

        var lines = helper.readLinesFromFile("input.txt");
        var claims = new Claims(lines);

        final var ans1 = solve1(lines, claims);
        final var ans2 = solve2(lines, claims);

        helper.answerChecker(ans1, 107043);
        helper.answerChecker(ans2, 346);
    }
}
