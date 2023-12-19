#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "rules_system.h"

using namespace std;

namespace advc_2023::day19
{
    static utils::Axis get_part_axis(const char c)
    {
        switch (c)
        {
        case 'x': return utils::Axis::X;
        case 'm': return utils::Axis::Y;
        case 'a': return utils::Axis::Z;
        case 's': return utils::Axis::W;
        default: assert(false);
        }
        return {};
    }

    // ---------------------------------------------------

    void Rule::parse(const std::string& line)
    {
        const auto split = utils::split(line, ":");

        if (split.size() == 1)
        {
            m_destination = line;
        }
        else
        {
            assert(split.size() == 2);

            const auto& left = split.front();
            m_destination = split.back();

            const char part = left[0];
            const char cond = left[1];
            m_cond_val = stoi(left.substr(2));
            m_part = get_part_axis(part);

            switch (cond)
            {
            case '<': m_cond = cond::less; break;
            case '>': m_cond = cond::greater; break;
            default: assert(false);
            }
        }
    }

    // -----------------------------------------------

    void Workflow::parse(const std::string& line)
    {
        const auto split = utils::split(line, "{");
        assert(split.size() == 2);

        m_name = split.front();

        auto rule_string = split.back();
        rule_string.pop_back();

        const auto split_rules = utils::split(rule_string, ",");

        for (const auto& split_rule : split_rules)
        {
            m_rules.push_back({});
            m_rules.back().parse(split_rule);
        }
    }

    // -----------------------------------------------

    void Rules_system::parse(const vector<string>& lines)
    {
        int i = 0;
        for (const string& line : lines)
        {
            i++;
            if (line.empty())
            {
                break;
            }
            auto workflow = make_shared<Workflow>();

            workflow->parse(line);
            m_workmap[workflow->get_name()] = workflow;
        }

        for (const auto& [name, workflow] : m_workmap)
        {
            bool already_added = false;

            for (const auto& rule : workflow->get_rules())
            {
                const auto& rule_dst = rule.get_destination();

                if (rule_dst == ACCEPT_LITERAL)
                {
                    if (!already_added)
                    {
                        m_accepted_workflows.push_back(workflow);
                        already_added = true;
                    }
                }
                else if (rule_dst == REJECT_LITERAL)
                {
                    // do nothing 
                }
                else
                {
                    const auto child = m_workmap.find(rule_dst)->second;

                    assert(child->get_parent().empty());
                    child->set_parent(name);
                }
            }
        }
        
        for (string line; i < (int)lines.size(); i++)
        {
            line = lines[i];

            assert(line.front() == '{');
            assert(line.back() == '}');

            const string content = line.substr(1, line.size() - 2);
            const auto split = utils::split(content, ",");
            assert(split.size() == 4);

            utils::Point p{};

            for (const string& part : split)
            {
                const auto split_part = utils::split(part, "=");
                assert(split_part.size() == 2);

                const utils::Axis axis = get_part_axis(split_part.front().front());
                const int val = stoi(split_part.back());

                p.set(axis, val);
            }

            m_parts.push_back(p);
        }
    }
}