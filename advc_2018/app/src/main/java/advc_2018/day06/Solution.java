package advc_2018.day06;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines)
    {
        AreaMap areaMap = new AreaMap(lines);
        return areaMap.getLargestInternalArea();
    }

    public static long solve2(List<String> lines)
    {
        return 0;
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day06");
        
        var lines = helper.readLinesFromFile("input.txt");
        
        helper.answerCheckerDontThrow(solve1(lines), 0);
        helper.answerCheckerDontThrow(solve2(lines), 0);
    }
}
