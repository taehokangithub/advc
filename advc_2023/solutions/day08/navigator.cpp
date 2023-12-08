#include <assert.h>
#include <iostream>
#include <numeric>
#include <string>

#include "../../utils/input_utils.h"
#include "navigator.h"

using namespace std;

namespace advc_2023::day08
{
    void Node::set_links(const shared_ptr<Node>& left, const shared_ptr<Node>& right)
    {
        m_left = left;
        m_right = right;
    }

    // ----------------------------------------------------

    shared_ptr<const Node> Node::get_next(char inst) const
    {
        return inst == 'R' ? m_right : inst == 'L' ? m_left : nullptr;
    }

    // ----------------------------------------------------

    shared_ptr<Node> Graph::get_or_create_node(const string& name)
    {
        auto ite = m_nodes.find(name);

        if (ite != m_nodes.end())
        {
            return ite->second;
        }

        auto node = m_nodes[name] = make_shared<Node>(name);
        return node;
    }

    // ----------------------------------------------------

    shared_ptr<Node> Graph::get_node(const string& name) const
    {
        auto ite = m_nodes.find(name);

        if (ite != m_nodes.end())
        {
            assert(ite->second->get_name() == name);
            return ite->second;
        }

        assert(false);
        return nullptr;
    }

    // ----------------------------------------------------

    vector<shared_ptr<const Node>> Graph::get_all_nodes_end_with(char c) const
    {
        vector<shared_ptr<const Node>> node_list;

        for (const auto& [name, node] : m_nodes)
        {
            if (name.back() == c)
            {
                node_list.push_back(node);
            }
        }

        return node_list;
    }

    // ----------------------------------------------------


    void Navigator::parse(const vector<string>& lines)
    {
        m_instruction = lines.front();

        for (int i = 1; i < (int)lines.size(); i++)
        {
            const auto& line = lines[i];

            const auto divided = utils::split(line, " = ");
            assert(divided.size() == 2);

            const auto& name_part = divided.front();
            const auto& dest_part = divided.back();

            const auto destinations = utils::split(dest_part.substr(1, dest_part.length() - 2), ", ");
            assert(destinations.size() == 2);
            assert(destinations.front().length() == 3);
            assert(destinations.back().length() == 3);

            auto node_from = m_graph.get_or_create_node(name_part);
            auto node_left = m_graph.get_or_create_node(destinations.front());
            auto node_right = m_graph.get_or_create_node(destinations.back());

            node_from->set_links(node_left, node_right);
        }
    }

    // ----------------------------------------------------

    int Navigator::get_steps() const
    {
        const std::string START_NODE_NAME = "AAA";
        const std::string END_NODE_NAME = "ZZZ";

        shared_ptr<const Node> node = m_graph.get_node(START_NODE_NAME);

        int steps = 0;

        for ( ;node->get_name() != END_NODE_NAME; steps++)
        {
            const char inst = m_instruction[steps % m_instruction.size()];

            node = node->get_next(inst);

            assert(node);
        }

        return steps;
    }

    // ----------------------------------------------------

    uint64_t Navigator::get_multiple_steps() const
    {
        const char end_node_postfix = 'Z';

        auto nodes = m_graph.get_all_nodes_end_with('A');
        vector<int> each_answers(nodes.size());

        int steps = 0;
        int cnt_arrived = 0;

        for ( ; cnt_arrived < (int)nodes.size(); steps++)
        {
            const char inst = m_instruction[steps % m_instruction.size()];

            for (int i = 0; i < (int)nodes.size(); i ++)
            {
                const auto from_node = nodes[i];
                if (from_node->has_postfix(end_node_postfix))
                {
                    continue; // do not progress if already arrived
                }

                const auto to_node = from_node->get_next(inst);
                if (to_node->has_postfix(end_node_postfix))
                {
                    cnt_arrived++;
                    each_answers[i] = steps + 1;    // steps is 1-based
                }
                
                nodes[i] = to_node;
            }
        }

        // Find LCM of all answers
        uint64_t total_ans = each_answers.front();

        for (int i = 1; i < (int)each_answers.size(); i++)
        {
            const uint64_t ans = each_answers[i];
            total_ans = (total_ans / std::gcd(ans, total_ans)) * ans;
        }

        return total_ans;
    }


}

