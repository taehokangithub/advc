package advc_2018.day04;

import java.util.ArrayList;
import java.util.List;
import java.util.HashMap;

public class RecordManager 
{
    private HashMap<Integer, ArrayList<GuardRawRecord>> m_rawData = new HashMap<>();
    private HashMap<Integer, Guard> m_guards = new HashMap<>();

    public RecordManager(List<String> lines)
    {
        lines.sort(java.util.Comparator.naturalOrder());
        
        int currentId = 0;

        for (var line : lines)
        {
            var record = new GuardRawRecord(line);

            {
                int id = record.getId();
                currentId = (id == 0) ? currentId : id;
            }

            if (currentId == 0)
            {
                throw new IllegalArgumentException("No id detected at" + line);
            }

            record.setId(currentId);

            var guardRecordList = getOrCreateGuardRecordList(currentId);

            guardRecordList.add(record);
        }

        createGuardData();
    }

    public int getMostLikelySleepGuarByCount()
    {
        Guard guard = getMostSleptGuardByTotalMinute();
        final int mostSleptMinute = guard.getMostSleptMinute();

        return mostSleptMinute * guard.getId();
    }

    public int getMostLikelySleepGuardByMinute()
    {
        Guard guard = getMostSleptGuardBySpecificMinute();

        return guard.getId() * guard.getMostSleptMinute();
    }

    private Guard getMostSleptGuardByTotalMinute()
    {
        Guard mostSleptGuard = null;

        for (Guard guard : m_guards.values())
        {
            if (mostSleptGuard == null || mostSleptGuard.getTotalSleepMinutes() < guard.getTotalSleepMinutes())
            {
                mostSleptGuard = guard;
            }
        }

        return mostSleptGuard;
    }

    private Guard getMostSleptGuardBySpecificMinute()
    {
        Guard mostSleptGuard = null;

        for (Guard guard : m_guards.values())
        {
            if (mostSleptGuard == null || mostSleptGuard.getMostSleepCount() < guard.getMostSleepCount())
            {
                mostSleptGuard = guard;
            }
        }

        return mostSleptGuard;
    }

    private List<GuardRawRecord> getOrCreateGuardRecordList(int id)
    {
        var guardRecordList = m_rawData.get(id);

        if (guardRecordList == null)
        {
            guardRecordList = new ArrayList<>();
            m_rawData.put(id, guardRecordList);
        }

        return guardRecordList;
    }

    private void createGuardData()
    {
        for (var item : m_rawData.entrySet())
        {
            final int id = item.getKey();
            ArrayList<GuardRawRecord> list = item.getValue();

            GuardRawRecord fallAsleepRecord = null;

            final Guard guard = new Guard(id);
            m_guards.put(id, guard);

            for (int i = 0; i < list.size(); i ++)
            {
                GuardRawRecord record = list.get(i);
                
                if (record.getId() != id)
                {
                    throw new IllegalArgumentException("Current id " + id + ", but the record id " + record.getId());
                }

                final EGuardRecordType recordType = record.getRecordType();

                if (fallAsleepRecord == null)
                {
                    if (recordType == EGuardRecordType.FallsAsleep)
                    {
                        fallAsleepRecord = record;
                    }
                    else if (recordType == EGuardRecordType.WakesUp)
                    {
                        throw new IllegalArgumentException("Expecting fall asleep, found wakeup " + record);
                    }
                }
                else
                {
                    if (recordType == EGuardRecordType.WakesUp)
                    {
                        guard.addSleepRecord(fallAsleepRecord, record);
                        fallAsleepRecord = null;
                    }
                    else if (recordType == EGuardRecordType.FallsAsleep)
                    {
                        throw new IllegalArgumentException("Expecting wakeup, found fall asleep " + record);
                    }
                }
            }
        }
    }
}
