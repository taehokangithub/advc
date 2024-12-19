package advc_2024.day11;

import java.util.HashMap;
import java.util.Collection;
import java.util.LinkedList;

class TurnElement
{
    public long value;
    public long count;
    public TurnElement(long val)
    {
        value = val;
    }
}

class Turn
{
    private HashMap<Long, TurnElement> m_valueCountMap = new HashMap<>();

    public void add(long value, long count)
    {
        if (!m_valueCountMap.containsKey(value))
        {
            m_valueCountMap.put(value, new TurnElement(value));
        }
        var element = m_valueCountMap.get(value);
        element.count += count;
    }

    Collection<TurnElement> getTurnElements()
    {
        return m_valueCountMap.values();
    }
}


public class TurnManager 
{
    private HashMap<Integer, Turn> m_turns = new HashMap<>();

    public TurnManager(LinkedList<Long> firstStep)
    {
        Turn initialTurn = new Turn();

        m_turns.put(0, initialTurn);

        for (var val : firstStep)
        {
            add(0, val, 1);
        }
    }

    public void add(int turn, long value, long count)
    {
        if (!m_turns.containsKey(turn))
        {
            m_turns.put(turn, new Turn());
        }

        m_turns.get(turn).add(value, count);
    }

    public Collection<TurnElement> getTurnElements(int turn)
    {
        if (m_turns.containsKey(turn))
        {
            return m_turns.get(turn).getTurnElements();
        }
        return null;
    }
}
