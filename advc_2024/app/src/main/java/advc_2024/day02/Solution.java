package advc_2024.day02;

import advc_utils.Etc.*;

import java.util.ArrayList;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day02");

    private List<List<Integer>> getReports(List<String> lines)
    {
        List<List<Integer>> reports = new ArrayList<>();

        for (String line : lines) 
        {
            var valueStrs = line.split(" ");
            var values = new ArrayList<Integer>();
            reports.add(values);

            for (String valStr : valueStrs)
            {
                values.add(Integer.parseInt(valStr));
            }
        }

        return reports;
    }

    private boolean isSafe(List<Integer> report, boolean tolerance)
    {
        int prevVal = report.get(0);
        int prefDiff = 0;

        for (int i = 1; i < report.size(); i ++)
        {
            final int curVal = report.get(i);
            final int diff = curVal - prevVal;

            if (diff == 0 || Math.abs(diff) > 3 || (diff * prefDiff < 0))
            {
                if (tolerance)
                {
                    tolerance = false;
                }
                else
                {
                    return false;
                }
            }
            else 
            {
                prevVal = curVal;
                prefDiff = diff;
            }

        }
        return true;
    }

    private List<Integer> removeElement(List<Integer> srcList, int at)
    {
        List<Integer> removed = new ArrayList();

        for (int i = 0; i < srcList.size(); i ++)
        {
            if (i != at)
            {
                removed.add(srcList.get(i));
            }
        }

        return removed;
    }

    private long countSafe(List<String> lines, boolean tolerance)
    {
        var reports = getReports(lines);
        int numSafe = 0;

        for (var report : reports)
        {
            if (isSafe(report, tolerance))
            {
                numSafe ++;
            }
            else if (tolerance)
            {
                for (int i = 0; i < report.size(); i ++)
                {
                    var examine = removeElement(report, i);
                    if (isSafe(examine, false))
                    {
                        numSafe ++;
                        break;
                    }
                }
            }

        }
        return numSafe;
    }

    public long solve1(List<String> lines)
    {
        return countSafe(lines, false);
    }

    public long solve2(List<String> lines)
    {
        return countSafe(lines, true);
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 510);
        m_helper.answerCheckerDontThrow(solve2(lines), 553);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 2);
        m_helper.answerCheckerTestInput(solve2(lines), 4);
    }

}
