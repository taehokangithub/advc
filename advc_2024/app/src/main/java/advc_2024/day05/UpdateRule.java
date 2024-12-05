package advc_2024.day05;

import advc_utils.Etc.SplitHelper;

public class UpdateRule {
    private int m_prev;
    private int m_next;

    public UpdateRule(String line)
    {
        var numbersStr = SplitHelper.CheckSplit(line, "\\|", 2);

        m_prev = Integer.parseInt(numbersStr[0]);
        m_next = Integer.parseInt(numbersStr[1]);
    }

    public int getPrev()
    {
        return m_prev;
    }

    public int getNext()
    {
        return m_next;
    }

    @Override
    public String toString()
    {
        return String.format("[%d/%d]", m_prev, m_next);
    }

}
