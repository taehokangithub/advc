#include <algorithm>
#include <assert.h>
#include <iostream>
#include <sstream>
#include <queue>
#include <unordered_set>

#include "../../utils/input_utils.h"
#include "components.h"

using namespace std;

namespace advc_2023::day25
{
    string Edge::to_string() const
    {
        stringstream ss;
        ss << "[" << node1->name << "-" << node2->name << "]";
        return ss.str();
    }

    // -------------------------------------------------------------------------------

    void Components::parse(const vector<string>& lines)
    {
        for (const auto& line : lines)
        {
            const auto split = utils::split(line, ": ");
            assert(split.size() == 2);

            auto left_node = get_or_create_node(split.front());

            const auto targets = utils::split(split.back(), " ");

            for (const auto& target : targets)
            {
                auto right_node = get_or_create_node(target);
                auto edge = make_shared<Edge>();
                edge->node1 = left_node;
                edge->node2 = right_node;

                left_node->edges.push_back(edge);
                right_node->edges.push_back(edge);
            }
        }
    }

    // -------------------------------------------------------------------------------

    shared_ptr<Node> Components::get_or_create_node(const string& name)
    {
        if (m_nodes.find(name) != m_nodes.end())
        {
            return m_nodes[name];
        }
        auto new_node = m_nodes[name] = make_shared<Node>();
        new_node->name = name;
        new_node->index = node_index++;

        return new_node;
    }

    // -------------------------------------------------------------------------------

    shared_ptr<const Edge> Components::get_most_visited_edge(const vector<shared_ptr<const Edge>>& disabled) const
    {
        unordered_map<shared_ptr<const Edge>, int> edge_visit_count;
        vector<bool> visited_nodes(m_nodes.size());

        for (const auto& [name, start_node] : m_nodes)
        {
            for (int i = 0; i < (int)visited_nodes.size(); i++)
            {
                visited_nodes[i] = false;
            }

            queue<shared_ptr<const Node>> q;

            q.push(start_node);

            while (!q.empty())
            {
                const auto node = q.front(); q.pop();

                for (const auto edge : node->edges)
                {
                    if (std::find(disabled.begin(), disabled.end(), edge) == disabled.end())
                    {
                        const auto& other_node = edge->get_other(node);
                        if (!visited_nodes[other_node->index])
                        {
                            visited_nodes[other_node->index] = true;
                            q.push(other_node);
                            edge_visit_count[edge]++;
                        }
                    }
                }
            }
        }

        int high_count = 0;
        shared_ptr<const Edge> most_visited;

        for (const auto& [edge, count] : edge_visit_count)
        {
            if (count > high_count)
            {
                high_count = count;
                most_visited = edge;
            }
        }

        //cout << " most visitied " << most_visited->to_string() << " : " << high_count << endl << endl;

        return most_visited;
    }

    // -------------------------------------------------------------------------------

    int Components::get_cluster_count(shared_ptr<const Node> start_node, const vector<shared_ptr<const Edge>>& disabled) const
    {
        unordered_set<shared_ptr<const Node>> visited_nodes;
        queue<shared_ptr<const Node>> q;

        q.push(start_node);

        while (!q.empty())
        {
            const auto node = q.front(); q.pop();

            for (const auto edge : node->edges)
            {
                if (std::find(disabled.begin(), disabled.end(), edge) == disabled.end())
                {
                    const auto& other_node = edge->get_other(node);
                    if (visited_nodes.find(other_node) == visited_nodes.end())
                    {
                        visited_nodes.insert(other_node);
                        q.push(other_node);
                    }
                }
            }
        }
        return (int)visited_nodes.size();
    }

    // -------------------------------------------------------------------------------

    int Components::get_clusters_signature() const
    {
        vector<shared_ptr<const Edge>> disabled;
        for (int i = 0; i < 3; i++)
        {
            const auto most_visited = get_most_visited_edge(disabled);
            disabled.push_back(most_visited);
        }

        const int one_cluster_count = get_cluster_count(m_nodes.begin()->second, disabled);

        const int other_cluster_count = (int)m_nodes.size() - one_cluster_count;

        return one_cluster_count * other_cluster_count;
    }


}