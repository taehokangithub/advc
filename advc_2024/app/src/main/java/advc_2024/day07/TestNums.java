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

    public List<Operator> getPossibleOperators(boolean useConcate)
    {
        List<Operator> operators = new ArrayList<Operator>();

        return getPossibleOperatorInternal(operators, useConcate);
    }

    private List<Operator> getPossibleOperatorInternal(List<Operator> operators, boolean useConcate)
    {
        if (m_numbers.size() == operators.size() + 1)
        {   
            long result = m_numbers.get(0);
            for (int i = 0; i < operators.size(); i ++)
            {
                result = calculate(operators.get(i), result, m_numbers.get(i + 1));
            }

            if (result == m_targetNumber)
            {
                return operators;
            }
            return null;
        }
        
        var ans1 = AppendOperatorAndCall(operators, Operator.Add, useConcate);
        if (ans1 != null)
        {
            return ans1;
        }

        var ans2 = AppendOperatorAndCall(operators, Operator.Multi, useConcate);
        if (ans2 != null || !useConcate)
        {
            return ans2;
        }

        return AppendOperatorAndCall(operators, Operator.Concat, useConcate);
    }

    private List<Operator> AppendOperatorAndCall(List<Operator> operators, Operator newOp, boolean useConcate)
    {
        List<Operator> newOperators = new ArrayList<Operator>(operators);
        newOperators.add(newOp);
        return getPossibleOperatorInternal(newOperators, useConcate);
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

