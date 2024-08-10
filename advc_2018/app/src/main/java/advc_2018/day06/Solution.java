package advc_2018.day06;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines, AreaMap areaMap)
    {
        return areaMap.getLargestInternalArea();
    }

    public static long solve2(List<String> lines, AreaMap areaMap)
    {
        return areaMap.getSafeAreas();
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day06");
        
        var lines = helper.readLinesFromFile("input.txt");
        AreaMap areaMap = new AreaMap(lines);
        areaMap.analyse(10000);
        
        helper.answerChecker(solve1(lines, areaMap), 4887);
        helper.answerChecker(solve2(lines, areaMap), 34096);
    }
}
