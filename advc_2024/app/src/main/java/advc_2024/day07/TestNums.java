package advc_2024.day07;

import java.util.ArrayList;
import java.util.List;

import advc_utils.Etc.SplitHelper;

enum Operator { Add, Multi, Concat };

public class TestNums {

    private long m_targetNumber;
    private List<Integer> m_numbers = new ArrayList<Integer>();

    public TestNums(String line)
    {
        var partStrs = SplitHelper.CheckSplit(line, ": ", 2);
        m_targetNumber = Long.parseLong(partStrs[0]);
        for (String numStr : partStrs[1].split(" "))
        {
            m_numbers.add(Integer.parseInt(numStr));
        }
    }

    public long getTargetNumber() 
    {
        return m_targetNumber;
    }

    @Override 
    public String toString()
    {
        StringBuilder sb = new StringBuilder();

        sb.append("[<" + m_targetNumber + "> ");
        for (var num : m_numbers)
        {
            sb.append("" + num + " ");
        }
        sb.deleteCharAt(sb.length() - 1);
        sb.append("]");
        return sb.toString();
    }

    public String toOpString(List<Operator> operators)
    {
        StringBuilder sb = new StringBuilder();

        sb.append("[<" + m_targetNumber + "> " + m_numbers.get(0) + " ");

        for (int i = 1; i < m_numbers.size(); i ++)
        {
            if (i <= operators.size())
            {
                sb.append(getOperatorString(operators.get(i - 1)));
            }
            else
            {
                sb.append(" ");
            }
            sb.append(" " + m_numbers.get(i) + " ");
        }

        sb.deleteCharAt(sb.length() - 1);
        sb.append("]");
        return sb.toString();        
    }

    public boolean hasPossibleOperators(boolean useConcate)
    {
        List<Operator> operators = new ArrayList<Operator>();

        return hasPossibleOperatorInternal(operators, useConcate);
    }

    private boolean hasPossibleOperatorInternal(List<Operator> operators, boolean useConcate)
    {
        if (m_numbers.size() == operators.size() + 1)
        {   
            long result = m_numbers.get(0);
            for (int i = 0; i < operators.size(); i ++)
            {
                result = calculate(operators.get(i), result, m_numbers.get(i + 1));
            }

            return result == m_targetNumber;
        }
        
        operators.add(Operator.Add);
        var ans1 = hasPossibleOperatorInternal(operators, useConcate);

        if (ans1)
        {
            return ans1;
        }

        operators.set(operators.size() - 1, Operator.Multi);
        var ans2 = hasPossibleOperatorInternal(operators, useConcate);

        if (ans2 || !useConcate)
        {
            operators.removeLast();
            return ans2;
        }

        operators.set(operators.size() - 1, Operator.Concat);
        var ans3 = hasPossibleOperatorInternal(operators, useConcate);
        operators.removeLast();

        return ans3;
    }

    private long calculate(Operator op, long num1, long num2)
    {
        switch(op)
        {
            case Operator.Add : return num1 + num2;
            case Operator.Multi : return num1 * num2;
            case Operator.Concat : return Long.parseLong("" + num1 + "" + num2);
        }
        throw new IllegalArgumentException();
    }

    private String getOperatorString(Operator op) 
    {
        switch(op)
        {
            case Operator.Add : return "+";
            case Operator.Multi : return "*";
            case Operator.Concat : return "|";
        }
        throw new IllegalArgumentException();
    }
}

