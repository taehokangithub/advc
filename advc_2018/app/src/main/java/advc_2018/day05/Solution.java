package advc_2018.day05;

import advc_utils.Etc.*;

public class Solution 
{
    public static long solve1(String line)
    {
        return Reduction.reduce(new StringBuilder(line));
    }

    public static long solve2(String line)
    {
        return Reduction.reduceBestStrategy(new StringBuilder(line));
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day05");
        
        var lines = helper.readLinesFromFile("input.txt");
        
        helper.answerChecker(solve1(lines.getFirst()), 10804);
        helper.answerChecker(solve2(lines.getFirst()), 6650);
    }
}
