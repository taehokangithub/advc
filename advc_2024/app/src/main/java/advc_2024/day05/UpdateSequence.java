package advc_2024.day05;

import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;

public class UpdateSequence 
{
    private ArrayList<Integer> m_sequence = new ArrayList<>();
    private HashMap<Integer, Integer> m_numberSet = new HashMap<>();

    public UpdateSequence(String line)
    {
        for (var numberStr : line.split(","))
        {
            final int val = Integer.parseInt(numberStr);
            m_numberSet.put(val, m_sequence.size());
            m_sequence.add(val);
        }
    }

    public UpdateSequence(UpdateSequence other)
    {
        for (int number : other.m_numberSet.keySet())
        {
            m_numberSet.put(number, m_sequence.size());
            m_sequence.add(number);
        }
    }

    public boolean hasNumber(int num)
    {
        return m_numberSet.containsKey(num);
    }

    public List<Integer> getSequence()
    {
        return m_sequence;
    }

    public int getMidNumber()
    {
        return m_sequence.get(m_sequence.size() / 2);
    }

    boolean isRuleSatisfied(UpdateRule rule)
    {
        if (hasNumber(rule.getPrev()) && hasNumber(rule.getNext()))
        {
            for (int num : m_sequence)
            {
                if (num == rule.getPrev())
                {
                    return true;
                }
                if (num == rule.getNext())
                {
                    return false;
                }
            }
            
            throw new IllegalStateException(String.format("Both %d and %d not found in the sequence", rule.getPrev(), rule.getNext()));
        }
        return true;
    }

    public UpdateSequence fixSequence(List<UpdateRule> rules)
    {
        UpdateSequence fixed = new UpdateSequence(this);

        for (int i = 0 ; i < 100; i ++)
        {
            boolean isFixed = true;
            for (var rule : rules)
            {
                if (!fixed.isRuleSatisfied(rule))
                {
                    isFixed = false;
                    fixed.exchange(rule.getPrev(), rule.getNext());
                }
            }
            if (isFixed)
            {
                break;
            }
        }
        
        return fixed;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();

        sb.append("[");
        for (int num : m_sequence)
        {
            sb.append(String.format("%d ", num));
        }
        sb.deleteCharAt(sb.length() - 1);
        sb.append("]");

        return sb.toString();
    }

    private void exchange(int prev, int next)
    {
        final int prevIndex = m_numberSet.get(prev);
        final int nextIndex = m_numberSet.get(next);

        m_sequence.set(prevIndex, next);
        m_sequence.set(nextIndex, prev);

        m_numberSet.put(prev, nextIndex);
        m_numberSet.put(next, prevIndex);
    }
}
