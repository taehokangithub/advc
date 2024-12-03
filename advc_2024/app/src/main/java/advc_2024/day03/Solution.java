package advc_2024.day03;

import advc_utils.Etc.*;

import java.util.ArrayList;
import java.util.List;
import java.util.regex.MatchResult;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import org.junit.jupiter.api.Test;

class MatchingResult
{
    public String matchedString;
    public int location;

    public MatchingResult(String matchedString, int location) {
        this.matchedString = matchedString;
        this.location = location;
    }
}

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day03");

    private final String RegMul = "mul\\(\\d*,\\d*\\)";
    private final String RegDo = "do\\(\\)";
    private final String RegDont = "don't\\(\\)";

    private List<MatchingResult> getAllMatchings(String line, String regex)
    {
        List<MatchingResult> result = new ArrayList<>();

        Matcher matcher = Pattern.compile(regex).matcher(line);

        while(matcher.find())
        {
            result.add(new MatchingResult(matcher.group(), matcher.start()));
        }

        return result;
    }

    private int calculateMul(String line)
    {
        var stripped = line.substring(4, line.length() - 1);
        var values = SplitHelper.CheckSplit(stripped, ",", 2);
        return Integer.parseInt(values[0]) * Integer.parseInt(values[1]);
    }

    private String matchesToString(List<MatchingResult> matches)
    {
        StringBuilder sb = new StringBuilder();
        sb.append("[");

        for (var match : matches)
        {
            sb.append(match.location);
            sb.append(" ");
        }

        sb.deleteCharAt(sb.length() - 1);
        sb.append("]");
        return sb.toString();
    }

    private int findNearestAppearance(int location, List<MatchingResult> allMatches)
    {
        int ret = 0;

        for (var match : allMatches)
        {
            if (match.location < location)
            {
                ret = match.location;
            }
            else 
            {
                break;
            }
        }

        return ret;
    }

    private boolean doOrDont(int location, List<MatchingResult> allDos, List<MatchingResult> allDonts, boolean defaultOp)
    {
        final int nearestDo = findNearestAppearance(location, allDos);
        final int nearestDont = findNearestAppearance(location, allDonts);

        if (nearestDo == nearestDont && nearestDo == 0)
        {
            return defaultOp;
        }
        return nearestDo > nearestDont;
    }

    public long solve1(List<String> lines)
    {
        int ans = 0;
        for (var line : lines)
        {
            var allMuls = getAllMatchings(line, RegMul);
            for (var mul : allMuls)
            {
                ans += calculateMul(mul.matchedString);
            }
        }
        
        return ans;
    }

    public long solve2(List<String> lines)
    {
        int ans = 0;
        boolean defaultOp = true;

        for (var line : lines)
        {
            var allMuls = getAllMatchings(line, RegMul);
            var allDos = getAllMatchings(line, RegDo);
            var allDonts = getAllMatchings(line, RegDont);

            for (var mul : allMuls)
            {
                defaultOp = doOrDont(mul.location, allDos, allDonts, defaultOp);
                if (defaultOp)
                {
                    ans += calculateMul(mul.matchedString);
                }
            }
        }
        
        return ans;
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 179571322);
        m_helper.answerCheckerDontThrow(solve2(lines), 103811193);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 161);
        m_helper.answerCheckerTestInput(solve2(lines), 48);
    }

}
