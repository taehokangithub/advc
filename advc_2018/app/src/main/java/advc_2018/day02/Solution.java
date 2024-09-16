package advc_2018.day02;

import advc_utils.Etc.*;
import java.util.HashMap;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day02");

    public long solve1(List<String> lines)
    {
        long cnt2 = 0;
        long cnt3 = 0;

        for (var line : lines)
        {
            HashMap<Character, Integer> cntMap = new HashMap<>();
            for (var c : line.toCharArray())
            {
                cntMap.merge(c, 1, Integer::sum);
            }

            boolean has2 = false;
            boolean has3 = false;
            for (var item : cntMap.entrySet())
            {
                if (!has2 && item.getValue() == 2)
                {
                    cnt2 ++;
                    has2 = true;
                }
                else if (!has3 && item.getValue() == 3)
                {
                    cnt3 ++;
                    has3 = true;
                }
            }
        }
        return cnt2 * cnt3;
    }

    public String solve2(List<String> lines)
    {
        String mostMatch = "";
        int mostCount = 0;

        for (var line : lines)
        {
            for (var target : lines)
            {
                if (line.equals(target))
                {
                    continue;
                }
                int matchCnt = 0;
                String match = "";
                for (int i = 0; i < line.length(); i ++)
                {
                    if (line.charAt(i) == target.charAt(i))
                    {
                        matchCnt ++;
                        match += line.charAt(i);
                    }
                }
                if (matchCnt > mostCount)
                {
                    //System.out.println(String.format("[%d] %s vs %s => %d match, %s", matchCnt, line, target, matchCnt, match));
                    mostMatch = match;
                    mostCount = matchCnt;
                }
            }
        }
        return mostMatch;
    }

    public void run()
    {
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerChecker(solve1(lines), 4712);
        m_helper.answerChecker(solve2(lines), "lufjygedpvfbhftxiwnaorzmq");
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve2(lines), "fgij");
    }
}
