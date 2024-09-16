package advc_2018.day01;

import advc_utils.Etc.*;
import java.util.HashSet;
import java.util.List;

public class Solution 
{
    private long parseValue(String str)
    {
        boolean minus = str.charAt(0) == '-';
        return Long.parseLong(str.substring(1)) * (minus ? -1 : 1);
    }

    public long solve1(List<String> lines)
    {
        long ans = 0;
        for (var line : lines)
        {
            ans += parseValue(line);
        }
        return ans;
    }

    public long solve2(List<String> lines)
    {
        long frequency = 0;
        HashSet<Long> visited = new HashSet<>();

        final int maxTry = 10000;

        for (int currentTry = 0; currentTry < maxTry; currentTry ++)
        {
            for (var line : lines)
            {
                frequency += parseValue(line);
                if (visited.contains(frequency))
                {
                    return frequency;
                }
                visited.add(frequency);
            }
        }

        throw new IllegalStateException("Cound not find answer");
    }

    public void run()
    {
        IAdvcHelper helper = new AdvcHelper("day01");

        var lines = helper.readLinesFromFile("input.txt");
        
        helper.answerChecker(solve1(lines), 437);
        helper.answerChecker(solve2(lines), 655);
    }
}
