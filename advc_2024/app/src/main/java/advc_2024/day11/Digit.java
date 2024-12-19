package advc_2024.day11;

import java.util.ArrayList;
import java.util.LinkedList;

import advc_utils.Etc.Logger;
import advc_utils.Etc.Logger.LoggerType;

public class Digit 
{
    private int m_value;
    private ArrayList<LinkedList<Long>> m_steps;
    private Logger m_Logger = new Logger(LoggerType.DummyLogger);

    public Digit(int val)
    {
        m_value = val;

        m_steps = new ArrayList<>();
        var step = new LinkedList<Long>();
        step.add(Long.valueOf(val));
        m_steps.add(step);

        for (int i = 0; i < 15; i ++)
        {
            LinkedList<Long> nextStep = new LinkedList<>(step);
            Stones.applyRule(nextStep);

            m_Logger.log("[%d] %s", m_value, getStepString(nextStep));

            m_steps.add(nextStep);

            int num_broken = 0;
            for (var v : nextStep)
            {
                if (v < 10)
                {
                    num_broken ++;
                }
            }

            if (num_broken > 1)
            {
                break;
            }

            step = nextStep;
        }
    }

    public int getValue() 
    {
        return m_value;
    }

    public int getNumSteps()
    {
        return m_steps.size() - 1;
    }

    public int getNumStonesAt(int index)
    {
        return m_steps.get(index).size();
    }

    public LinkedList<Long> getStepAt(int index)
    {
        return m_steps.get(index);
    }

    public LinkedList<Long> getFinalStep()
    {
        return m_steps.get(m_steps.size() - 1);
    }
    
    public String getDebugString()
    {
        StringBuilder sb = new StringBuilder();

        sb.append("Num " + m_value + "\n");

        int stepNum = 0;
        for (var step : m_steps)
        {
            sb.append("[" + stepNum + "] : ");
            sb.append(getStepString(step));
            stepNum ++;
        }

        return sb.toString();
    }

    public String getStepString(LinkedList<Long> step)
    {
        StringBuilder sb = new StringBuilder();
        
        for (var val : step)
        {
            sb.append("" + val + " ");
        }
        sb.append("\n");

        return sb.toString();
    }
}
