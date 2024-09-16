package advc_2018.day09;

public class Marble 
{
    public enum Direction { Left, Right }

    private final int m_value;
    private Marble m_right;
    private Marble m_left;

    public Marble(int val) 
    {
        m_value = val;
        m_right = this;
        m_left = this;
    }

    public int getValue()
    {
        return m_value;
    }

    public void setLink(Direction dir, Marble other)
    {
        if (dir == Direction.Left)
        {
            m_left = other;
        }
        else
        {
            m_right = other;
        }
    }

    public Marble getLinkDirect(Direction dir)
    {
        return (dir == Direction.Left) ? m_left : m_right;
    }

    public Marble getLink(Direction dir, int step)
    {
        var ret = this;
        for (int i = 0; i < step; i ++)
        {
            ret = ret.getLinkDirect(dir);
        }
        return ret;
    }

    public void print()
    {
        System.out.print(String.format("[%d]", m_value));
        for (var marble = this.m_right; marble != this; marble = marble.m_right)
        {
            System.out.print(String.format("-%d", marble.m_value));
        }
        System.out.println("");
    }
}
