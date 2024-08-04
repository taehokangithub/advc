package advc_2018.day04;

import advc_utils.Etc.*;
import java.util.List;

public class Solution 
{
    public static long solve1(List<String> lines, RecordManager manager)
    {
        return manager.getMostLikelySleepGuarByCount();
    }

    public static long solve2(List<String> lines, RecordManager manager)
    {
        return manager.getMostLikelySleepGuardByMinute();
    }

    public static void run()
    {
        IAdvcHelper helper = new AdvcHelper("day04");

        var lines = helper.readLinesFromFile("input.txt");
        var manager = new RecordManager(lines);

        helper.answerChecker(solve1(lines, manager), 38813);
        helper.answerChecker(solve2(lines, manager), 141071);
    }
}
