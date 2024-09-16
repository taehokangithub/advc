package advc_2018.day08;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines)
    {
        var poptree = new Poptree(lines.getFirst());
        return poptree.getSumMetadata();
    }

    public static long solve2(List<String> lines)
    {
        var poptree = new Poptree(lines.getFirst());
        return poptree.getMetaValue();
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day08");
        
        var lines = helper.readLinesFromFile("input.txt");
        
        helper.answerChecker(solve1(lines), 48155);
        helper.answerChecker(solve2(lines), 40292);
    }
}
