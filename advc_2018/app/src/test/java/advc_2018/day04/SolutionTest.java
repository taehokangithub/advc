package advc_2018.day04;

import advc_utils.Etc.AdvcHelper;
import advc_utils.Etc.IAdvcHelper;

import org.junit.jupiter.api.Test;

import static org.junit.jupiter.api.Assertions.*;

public class SolutionTest {
    @Test
    void testGuardRecordParserTest1()
    {
        GuardRawRecord record = new GuardRawRecord("[1518-11-01 00:30] falls asleep");

        assertTrue(record.getId() == 0);
        assertTrue(record.getYear() == 1518);
        assertTrue(record.getMonth() == 11);
        assertTrue(record.getDay() == 1);
        assertTrue(record.getHour() == 0);
        assertTrue(record.getMinute() == 30);
        assertTrue(record.getRecordType() == EGuardRecordType.FallsAsleep);
    }

    @Test
    void testGuardRecordParserTest2()
    {
        GuardRawRecord record = new GuardRawRecord("[2024-09-23 23:47] Guard #1907 begins shift");

        assertTrue(record.getId() == 1907);
        assertTrue(record.getYear() == 2024);
        assertTrue(record.getMonth() == 9);
        assertTrue(record.getDay() == 23);
        assertTrue(record.getHour() == 23);
        assertTrue(record.getMinute() == 47);
        assertTrue(record.getRecordType() == EGuardRecordType.BeginsShift);
    }
}
