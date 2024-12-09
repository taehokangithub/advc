package advc_2024.day07;

import java.util.ArrayList;
import java.util.List;

public class TestNumsManager 
{
    private List<TestNums> m_testNums = new ArrayList<>();

    public TestNumsManager(List<String> lines)
    {
        for (var line : lines)
        {
            m_testNums.add(new TestNums(line));
        }
    }

    public long getTotalCalibrationResult(boolean useConcate)
    {
        long sum = 0;
        for (var testNums : m_testNums)
        {
            if (testNums.hasPossibleOperators(useConcate))
            {
                sum += testNums.getTargetNumber();
            }
        }
        return sum;
    }
    
}
