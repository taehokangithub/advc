package advc_2018.day09;

import advc_utils.Etc.*;

import org.junit.jupiter.api.Test;

public class Solution 
{
    private IAdvcHelper m_helper = new AdvcHelper("day09");

    public void run()
    {        
        m_helper.answerChecker(new MarblesSolver().getHighScore(419, 71052), 412117);
        m_helper.answerChecker(new MarblesSolver().getHighScore(419, 7105200), 3444129546L);
    }

    @Test
    public void test()
    {
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(9, 25), 32);
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(10, 1618), 8317);
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(13, 7999), 146373);
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(17, 1104), 2764);
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(21, 6111), 54718);
        m_helper.answerCheckerTestInput(new MarblesSolver().getHighScore(30, 5807), 37305);
    }

}
