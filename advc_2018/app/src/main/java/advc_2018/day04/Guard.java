package advc_2018.day04;

import java.util.ArrayList;

class Guard 
{
    private class SleepRecord
    {
        public GuardRawRecord start;
        public GuardRawRecord end;

        public int getTotalMinutes()
        {
            return end.getMinute() - start.getMinute();
        }

        public boolean didSleepAt(int minute)
        {
            return (minute >= start.getMinute() && minute < end.getMinute());
        }
    }

    private int m_id;
    private int m_totalSleepMinutes = -1;
    private int m_mostSleepMinute = 0;
    private int m_mostSleepCount = 0;
    private boolean m_hasInitialised = false;

    private ArrayList<SleepRecord> m_sleepRecords = new ArrayList<>();

    public Guard(int id)
    {
        m_id = id;
    }

    public int getId()
    {
        return m_id;
    }

    public void addSleepRecord(GuardRawRecord start, GuardRawRecord end)
    {
        if (start.getRecordType() != EGuardRecordType.FallsAsleep)
        {
            throw new IllegalArgumentException("Expected fall asleep, but found " + start);
        }
        if (end.getRecordType() != EGuardRecordType.WakesUp)
        {
            throw new IllegalArgumentException("Expected wakeup, but found " + end);
        }

        var sleepRecord = new SleepRecord();
        sleepRecord.start = start;
        sleepRecord.end = end;

        m_sleepRecords.add(sleepRecord);
    }

    public int getTotalSleepMinutes()
    {
        intialise();
        return m_totalSleepMinutes;
    }

    public int getMostSleptMinute()
    {
        intialise();
        return m_mostSleepMinute;
    }

    public int getMostSleepCount()
    {
        intialise();
        return m_mostSleepCount;
    }

    // ---------------------------------

    private void setMostSleepData()
    {
        for (int minute = 0; minute < 60; minute ++)
        {
            int cnt = 0;
            for (var sleepRecord : m_sleepRecords)
            {
                if (sleepRecord.didSleepAt(minute))
                {
                    cnt ++;
                }
            }
            if (cnt > m_mostSleepCount)
            {
                m_mostSleepMinute = minute;
                m_mostSleepCount = cnt;
            }
        }
    }

    private void setTotalSleepMinutes()
    {
        m_totalSleepMinutes = 0;

        for (var sleepRecord : m_sleepRecords)
        {
            m_totalSleepMinutes += sleepRecord.getTotalMinutes();
        }
    }

    private void intialise()
    {
        if (m_hasInitialised == false)
        {
            setMostSleepData();
            setTotalSleepMinutes();
        }
        m_hasInitialised = true;
    }
}
