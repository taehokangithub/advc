package advc_2018.day03;

import advc_utils.Etc.SplitHelper;

class Claim
{
    private int m_x, m_y, m_height, m_width;
    private int m_id;

    public Claim(String line)
    {
        // eg. #1197 @ 743,101: 29x11
        var parts = SplitHelper.CheckSplit(line, " @ ", 2);
        m_id = Integer.parseInt(parts[0].substring(1));

        var strs = SplitHelper.CheckSplit(parts[1], ": ", 2);
        var coordinates = SplitHelper.CheckSplit(strs[0], ",", 2);
        var widthHeight = SplitHelper.CheckSplit(strs[1], "x", 2);

        m_x = Integer.parseInt(coordinates[0]);
        m_y = Integer.parseInt(coordinates[1]);
        m_width = Integer.parseInt(widthHeight[0]);
        m_height = Integer.parseInt(widthHeight[1]);
    }

    @Override
    public String toString()
    {
        return String.format("[<#%d>%d,%d=>%d,%d]", m_id, m_x, m_y, m_height, m_width);
    }
    
    public int getId()
    {
        return m_id;
    }

    public int getX() 
    {
        return m_x;
    }

    public int getY()
    {
        return m_y;
    }

    public int getHeight()
    {
        return m_height;
    }

    public int getWidth()
    {
        return m_width;
    }    
}
