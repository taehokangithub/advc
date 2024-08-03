package advc_2018.day00_template;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines)
    {
        return 0;
    }

    public static long solve2(List<String> lines)
    {
        return 0;
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day00");
        
        var lines = helper.readLinesFromFile("input.txt");
        
        helper.answerCheckerDontThrow(solve1(lines), 0);
        helper.answerCheckerDontThrow(solve2(lines), 0);
    }
}
