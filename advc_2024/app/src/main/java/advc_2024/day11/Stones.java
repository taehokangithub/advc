package advc_2024.day11;

import java.util.ArrayList;
import java.util.Collections;
import java.util.HashMap;
import java.util.LinkedList;
import java.util.List;

import advc_utils.Etc.Logger;
import advc_utils.Etc.Logger.LoggerType;

public class Stones 
{
    private LinkedList<Long> m_stones = new LinkedList<>();
    private Logger m_logger = new Logger(LoggerType.DummyLogger);
    private HashMap<Long, Digit> m_digits = new HashMap<>();

    public Stones(String line)
    {
        var numStrs = line.split(" ");

        for (String numStr : numStrs)
        {
            final long num = Integer.parseInt(numStr);
            m_stones.add(num);
        }

        for (int i = 0 ; i < 10; i ++)
        {
            final var digit = new Digit(i);
            m_digits.put(Long.valueOf(i), digit);
            m_logger.log("Digit %d ---------\n%s", i, digit.getDebugString());
        }
    }

    public static void applyRule(LinkedList<Long> list)
    {
        final int numMulti = 2024;
        var it = list.listIterator();

        while (it.hasNext())
        {
            final long value = it.next();
            if (value == 0)
            {
                it.set(1l);
            }
            else
            {
                String numStr = "" + value;
                final int len = numStr.length();
                if (len % 2 == 0)
                {
                    final long num1 = Long.parseLong(numStr.substring(0, len / 2));
                    final long num2 = Long.parseLong(numStr.substring(len / 2, len));
                    it.set(num1);
                    it.add(num2);
                }
                else
                {
                    it.set(value * numMulti);
                }
            }
        }
    }

    public int getNumStonesAfterBlink_old(int numBlink)
    {
        for (int i = 0; i < numBlink; i ++)
        {
            m_logger.log("[%d] %d", i, m_stones.size());
            applyRule(m_stones);
        }
        Collections.sort(m_stones);
        m_logger.log("Origianl answer %s", getStonesString());
        return m_stones.size();
    }

    public long getNumStonesAfterBlink(int numBlink)
    {
        TurnManager turnManager = new TurnManager(m_stones);
        long sum = 0;

        for (int t = 0; t <= numBlink; t ++)
        {
            m_logger.log("\nTurn %d -------------------------------", t);

            var turnElements = turnManager.getTurnElements(t);
            
            if (turnElements != null)
            {
                List<TurnElement> turnElementList = new ArrayList<>(turnElements);
                Collections.sort(turnElementList, (o1, o2) -> Long.compare(o1.value, o2.value) );
                for (var element : turnElementList)
                {
                    m_logger.log("  Element %d, count %d", element.value, element.count);

                    if (t == numBlink)
                    {
                        sum += element.count;
                        m_logger.log("     final turn, adding count %d, sum %d", element.count, sum);
                        continue;
                    }

                    if (m_digits.containsKey(element.value))
                    {
                        final Digit d = m_digits.get(element.value);
                        final int digitSteps = d.getNumSteps();
                        final int nextTurn = t + digitSteps;

                        m_logger.log("     digit found : %d, steps %d. next turn %d", element.value, digitSteps, nextTurn);

                        if (nextTurn >= numBlink)
                        {
                            final int remainingSteps = numBlink - t;
                            final long addToSum = d.getNumStonesAt(remainingSteps) * element.count;
                            sum += addToSum;

                            m_logger.log("         => final case, nextTurn %d >= max %d, remaining %d, numElement %d, adding %d, sum %d"
                                    , nextTurn, numBlink, remainingSteps, d.getNumStonesAt(remainingSteps)
                                    , addToSum, sum);
                        }
                        else 
                        {
                            for (var v : d.getFinalStep())
                            {
                                m_logger.log("      extracting digit to turn %d, value %d, count %d", nextTurn, v, element.count);
                                turnManager.add(nextTurn, v, element.count);
                            }
                        }
                    }
                    else 
                    {
                        LinkedList<Long> newStep = new LinkedList<>();
                        newStep.add(element.value);
                        applyRule(newStep);
                        for (var v : newStep)
                        {
                            m_logger.log("     digit not found for %d, adding to turn %d, value %d", element.value, t + 1, v);
                            turnManager.add(t + 1, v, element.count);
                        }
                    }
                }
            }
        }

        return sum;
    }

    private String getStonesString()
    {
        StringBuilder sb = new StringBuilder();
        for (long value : m_stones)
        {
            sb.append("" + value + " ");
        }
        sb.deleteCharAt(sb.length() - 1);
        return sb.toString();
    }
}
