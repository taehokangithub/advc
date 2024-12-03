package advc_2024.day01;

import advc_utils.Etc.*;
import java.util.List;
import java.util.ArrayList;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day01");

    private List<List<Integer>> getTwoLists(List<String> lines)
    {
        List<Integer> list1 = new ArrayList<>();
        List<Integer> list2 = new ArrayList<>();
        List<List<Integer>> allLists = List.of(list1, list2);

        for (String line : lines) 
        {
            var words = SplitHelper.CheckSplit(line, "   ", 2);
            int val1 = Integer.parseInt(words[0]);
            int val2 = Integer.parseInt(words[1]);
            list1.add(val1);
            list2.add(val2);
        }

        list1.sort(null);
        list2.sort(null);

        return allLists;
    }

    public long solve1(List<String> lines)
    {
        var lists = getTwoLists(lines);

        final var list1 = lists.get(0);
        final var list2 = lists.get(1);

        int sumDiff = 0;
        for (int i = 0; i < list1.size(); i ++) 
        {
            sumDiff += Math.abs(list1.get(i) - list2.get(i));
        }
        return sumDiff;
    }

    public long solve2(List<String> lines)
    {
        var lists = getTwoLists(lines);

        final var list1 = lists.get(0);
        final var list2 = lists.get(1);

        int list2_index = 0;
        long sum = 0;
        for (int val1 : list1) 
        {
            for (int i = list2_index; i < list2.size(); i ++) 
            {
                final int val2 = list2.get(i);
                if (val1 == val2) 
                {
                    sum += val1;
                }
                else if (val1 < val2)
                {
                    break; // proceed to next val1
                }
                else 
                {
                    list2_index = i;
                }
            }
        }
        return sum;
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 3246517);
        m_helper.answerCheckerDontThrow(solve2(lines), 29379307);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 11);
        m_helper.answerCheckerTestInput(solve2(lines), 31);
    }

}
