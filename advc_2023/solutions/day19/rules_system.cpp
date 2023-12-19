
#include <assert.h>
#include <iostream>
#include <sstream>

#include "../../utils/input_utils.h"
#include "rules_system.h"

using namespace std;

namespace advc_2023::day19
{
    bool Rule::is_satisfied(const utils::Point& part) const
    {
        switch (m_cond)
        {
        case cond::greater: return part.get(m_part) > m_cond_val;
        case cond::less: return part.get(m_part) < m_cond_val;
        case cond::none: return true;
        }
        assert(false);
        return false;
    }

    // -----------------------------------------------

    void Rule::merge_accept_condition(Accept_condition& accept_condition, sign s) const
    {
        Range& range = accept_condition.ranges[m_part];

        switch (m_cond)
        {
        case cond::greater:
            if (s == sign::positive)
            {
                range.set_start(m_cond_val + 1);
            }
            else
            {
                range.set_end(m_cond_val);
            }
            break;
        case cond::less:
            if (s == sign::positive)
            {
                range.set_end(m_cond_val - 1);
            }
            else
            {
                range.set_start(m_cond_val);
            }
            break;
        case cond::none:
            break;
        default:
            assert(false);
        }
    }

    // -----------------------------------------------

    string Workflow::get_destination(const utils::Point& part) const
    {
        for (const auto& rule : m_rules)
        {
            if (rule.is_satisfied(part))
            {
                return rule.get_destination();
            }
        }

        assert(false);
        return string();
    }

    // -------------------------------------------------

    Rule Workflow::find_rule_to(const string& destination) const
    {
        for (const Rule& rule : m_rules)
        {
            if (rule.get_destination() == destination)
            {
                return rule;
            }
        }
        assert(false);
        return {};
    }

    // -----------------------------------------------

    Accept_condition Rules_system::get_accept_condition(std::shared_ptr<Workflow> workflow, int rule_index) const
    {
        Accept_condition ac;
        string dest_name = ACCEPT_LITERAL;

        while(true)
        {
            int i = 0;
            for (const auto& rule : workflow->get_rules())
            {
                if (rule.get_destination() != dest_name || i < rule_index)
                {
                    rule.merge_accept_condition(ac, Rule::sign::negative);
                }
                else
                {
                    rule.merge_accept_condition(ac, Rule::sign::positive);
                    break;
                }
                i++;
            }

            if (workflow->get_name() == STARTING_LITERAL)
            {
                break;
            }

            dest_name = workflow->get_name();
            rule_index = -1; // do not care about rule_index after the intial 'accepted' workflow. All other nodes have only 1 paraents
            workflow = get_workflow(workflow->get_parent());
        }
        

        return ac;
    }

    // -----------------------------------------------

    bool Rules_system::is_accepted(const utils::Point part) const
    {
        auto workflow = get_workflow(STARTING_LITERAL);

        while (true)
        {
            const string ans = workflow->get_destination(part);

            if (ans == ACCEPT_LITERAL)
            {
                return true;
            }
            if (ans == REJECT_LITERAL)
            {
                return false;
            }

            workflow = get_workflow(ans);
        }
    }

    // ---------------------------------------------

    shared_ptr<Workflow> Rules_system::get_workflow(const string& name) const
    {
        auto ite = m_workmap.find(name);

        if (ite == m_workmap.end())
        {
            assert(false);
            return nullptr;
        }
        return ite->second;
    }

    // ---------------------------------------------

    int Rules_system::get_total_accepted() const
    {
        int sum = 0;
        for (const auto& part : m_parts)
        {
            if (is_accepted(part))
            {
                sum += part.add_all_axis();
            }
        }
        return sum;
    }

    // ---------------------------------------------

    uint64_t Rules_system::get_total_accepted_cases() const
    {
        uint64_t sum = 0;

        for (const auto workflow : m_accepted_workflows)
        {
            int i = 0;
            for (const Rule& rule : workflow->get_rules())
            {
                if (rule.get_destination() == ACCEPT_LITERAL)
                {
                    auto ac = get_accept_condition(workflow, i);

                    sum += ac.get_all_cases();
                }
                i++;
            }
        }

        return sum;
    }
}