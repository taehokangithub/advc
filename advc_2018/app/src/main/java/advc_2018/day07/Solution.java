package advc_2018.day07;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static String solve1(List<String> lines)
    {
        var g = new JobGraph(lines);
        return g.getFullPath();
    }

    public static long solve2(List<String> lines, int baseSeconds, int numWorkers)
    {
        var g = new JobGraph(lines);
        return g.getTotalSeconds(baseSeconds, numWorkers);
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day07");
        var lines = helper.readLinesFromFile("input.txt");

        helper.answerChecker(solve1(lines), "GNJOCHKSWTFMXLYDZABIREPVUQ");
        helper.answerChecker(solve2(lines, 60, 5), 886);
        // 887 too high
        // 876 low
    }
}
