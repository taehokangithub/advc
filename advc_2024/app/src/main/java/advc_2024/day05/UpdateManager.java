package advc_2024.day05;

import advc_utils.Etc.*;
import advc_utils.Graphs.Graph;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;

public class UpdateManager 
{
    private List<UpdateRule> m_rules = new ArrayList<UpdateRule>();
    private List<UpdateSequence> m_sequences = new ArrayList<UpdateSequence>();

    public UpdateManager(List<String> lines)
    {
        enum Ephase { rule, sequence };

        Ephase phase = Ephase.rule;

        for (var line : lines)
        {
            if (line.length() == 0)
            {
                assert phase == Ephase.rule;
                phase = Ephase.sequence;
            }
            else if (phase == Ephase.rule)
            {
                m_rules.add(new UpdateRule(line));
            }
            else if (phase == Ephase.sequence)
            {
                m_sequences.add(new UpdateSequence(line));
            }
            else
            {
                throw new IllegalStateException();
            }
        }
    }

    public List<UpdateSequence> findSatisfiedSequences()
    {
        List<UpdateSequence> seqs = new ArrayList<UpdateSequence>();

        for (var seq : m_sequences)
        {
            boolean isSatisfied = true;
            for (var rule : m_rules)
            {
                if (!seq.isRuleSatisfied(rule))
                {
                    isSatisfied = false;
                    break;
                }
            }

            if (isSatisfied)
            {
                seqs.add(seq);
            }
        }

        return seqs;
    }

    public int getSatisfiedSequencesMidNumberSum()
    {
        int sum = 0;

        var rules = findSatisfiedSequences();

        for (var rule : rules)
        {
            sum += rule.getMidNumber();
        }

        return sum;
    }

    public int getFixedSequenceMidNumerSum()
    {
        int sum = 0;

        for (var seq : m_sequences)
        {
            for (var rule : m_rules)
            {
                if (!seq.isRuleSatisfied(rule))
                {
                    var fixed = seq.fixSequence(m_rules);
                    sum += fixed.getMidNumber();
                    break;
                }
            }
        }

        return sum;
    }

    @Override
    public String toString()
    {
        StringBuilder sb = new StringBuilder();

        sb.append("------ RULES ------\n");
        for (var rule : m_rules)
        {
            sb.append(rule.toString());
            sb.append("\n");
        }

        sb.append("------ SEQUENCES ------\n");
        for (var seq : m_sequences)
        {
            sb.append(seq.toString());
            sb.append("\n");
        }

        return sb.toString();
    }
}
