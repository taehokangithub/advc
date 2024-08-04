package advc_2018.day04;

import advc_utils.Etc.SplitHelper;

enum EGuardRecordType 
{
    BeginsShift,
    FallsAsleep,
    WakesUp,
}

class GuardRawRecord 
{
    private int m_id;
    private int m_year;
    private int m_month;
    private int m_day;
    private int m_hour;
    private int m_minute;
    private EGuardRecordType m_type;

    public GuardRawRecord(String line)
    {
        var strParts = SplitHelper.CheckSplit(line.substring(1), "] ", 2);
        var dateTimeParts = SplitHelper.CheckSplit(strParts[0], " ", 2);
        var dateParts = SplitHelper.CheckSplit(dateTimeParts[0], "-", 3);
        var timeParts = SplitHelper.CheckSplit(dateTimeParts[1], ":", 2);

        m_year = Integer.parseInt(dateParts[0]);
        m_month = Integer.parseInt(dateParts[1]);
        m_day = Integer.parseInt(dateParts[2]);

        m_hour = Integer.parseInt(timeParts[0]);
        m_minute = Integer.parseInt(timeParts[1]);

        final String typeStr = strParts[1];
        final char firstCharOfType = typeStr.charAt(0);

        if (firstCharOfType == 'G')
        {
            m_type = EGuardRecordType.BeginsShift;
            var shiftParts = SplitHelper.CheckSplit(typeStr, " ", 4);
            final String idString = shiftParts[1].substring(1);

            m_id = Integer.parseInt(idString);
        }
        else if (firstCharOfType == 'f')
        {
            m_type = EGuardRecordType.FallsAsleep;
        }
        else if (firstCharOfType == 'w')
        {
            m_type = EGuardRecordType.WakesUp;
        }
        else
        {
            throw new IllegalArgumentException(String.format("Unknown type string %s", typeStr));
        }
    }

    @Override
    public String toString()
    {
        return String.format("[%d/%d/%d %d:%d] %d - %s", m_year, m_month, m_day, m_hour, m_minute, m_id, m_type);
    }

    public void setId(int id)
    {
        m_id = id;
    }

    public int getId() 
    {
        return m_id;
    }

    public int getYear()
    {
        return m_year;
    }

    public int getMonth()
    {
        return m_month;
    }

    public int getDay()
    {
        return m_day;
    }

    public int getHour()
    {
        return m_hour;
    }

    public int getMinute()
    {
        return m_minute;
    }

    public EGuardRecordType getRecordType()
    {
        return m_type;
    }
}
