package advc_2024.day06;

import advc_utils.Etc.*;
import java.util.List;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day06");

    public long solve1(List<String> lines)
    {
        Maze maze = new Maze(lines);
        return maze.getTotalGuardTiles();
    }

    public long solve2(List<String> lines)
    {
        Maze maze = new Maze(lines);
        return maze.findObstructionPositions();
    }

    public void run()
    {        
        var lines = m_helper.readLinesFromFile("input.txt");
        
        m_helper.answerCheckerDontThrow(solve1(lines), 5162);
        m_helper.answerCheckerDontThrow(solve2(lines), 1909);
    }

    @Test
    public void test()
    {
        var lines = m_helper.readLinesFromFile("input_test.txt");

        m_helper.answerCheckerTestInput(solve1(lines), 41);
        m_helper.answerCheckerTestInput(solve2(lines), 6);
    }

}
