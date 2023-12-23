#include <assert.h>
#include <iostream>
#include <queue>
#include <unordered_set>

#include "../../utils/input_utils.h"
#include "graph.h"
#include "hiking.h"

using namespace std;

namespace advc_2023::day23
{
    // ---------------------------------------------------------
    Graph::Graph(const utils::Grid<tile_type>& grid, const utils::Point& enter_loc, const utils::Point& exit_loc) : m_grid(grid)
    {
        m_enter_node = make_shared<Node>();
        m_enter_node->loc = enter_loc;

        std::unordered_set<shared_ptr<Node>> q;
        q.insert(m_enter_node);

        while (!q.empty())
        {
            auto node = *q.begin(); q.erase(node);

            //cout << "examining node " << node->loc.to_string() << ", qsize " << q.size() << endl;

            for (const auto dir : utils::Point::s_dirs)
            {
                auto nextpos = node->loc.get_moved(dir);

                if (m_grid.is_valid_point(nextpos) && m_grid.get(nextpos) != tile_type::forest)
                {
                    auto next_node = find_next_node(node, dir);

                    if (next_node->edges.empty() && q.find(next_node) == q.end())
                    {
                        //cout << "   ==> adding a new node " << next_node->loc.to_string() << endl;
                        q.insert(next_node);
                    }
                }
            }
        }

        m_exit_node = m_nodes[get_point_fingerprint(exit_loc)];
        assert(m_exit_node);
    }

    // ---------------------------------------------------------

    std::shared_ptr<Node> Graph::get_or_create_node(const utils::Point& p)
    {
        const auto point_fingerprint = get_point_fingerprint(p);
        if (auto ite = m_nodes.find(point_fingerprint); ite != m_nodes.end())
        {
            return  ite->second;
        }

        static int node_id = 0;
        auto node = make_shared<Node>();
        node->loc = p;
        node->id = (++node_id);
        m_nodes[point_fingerprint] = node;
        return node;
    }

    // ---------------------------------------------------------

    std::shared_ptr<Node> Graph::find_next_node(std::shared_ptr<Node> node, utils::Dir dir)
    {
        auto curpos = node->loc;
        uint16_t distance = 0;
        unordered_set<int> visited;

        while (true)
        {
            curpos.move(dir);
            visited.insert(get_point_fingerprint(curpos));
            distance++;

            assert(m_grid.is_valid_point(curpos));

            int cnt_possible_way{ 0 };

            for (const auto next_dir : utils::Point::s_dirs)
            {
                auto nextpos = curpos.get_moved(next_dir);

                if (nextpos == node->loc || visited.find(get_point_fingerprint(nextpos)) != visited.end())
                {
                    continue;
                }

                if (!m_grid.is_valid_point(nextpos))
                {
                    break;
                }

                if (m_grid.get(nextpos) != tile_type::forest)
                {
                    cnt_possible_way++;
                    dir = next_dir;
                }
            }
            if (cnt_possible_way != 1)
            {
                break;
            }
        }

        assert(curpos != node->loc);
        auto next_node = get_or_create_node(curpos);

        node->edges.push_back({ next_node, distance });

        //cout << " === edge from " << node->loc.to_string() << " to " << next_node->loc.to_string() << " dist " << distance << endl;

        return next_node;
    }

    // ---------------------------------------------------------

    int Graph::find_longest_path() const
    {
        struct search
        {
            uint16_t steps{};
            shared_ptr<Node> cur_node;
            uint64_t visited{};
        };

        const auto visited_value = [](const shared_ptr<Node>& n) { return ((uint64_t)1 << n->id); };
        const auto set_visited = [visited_value](search& s) { s.visited |= visited_value(s.cur_node); };
        const auto get_visited = [visited_value](const search& s, const shared_ptr<Node>& n) { return (s.visited & visited_value(n)) > 0; };

        queue<search> q;
        
        q.push({ 0, m_enter_node });

        uint16_t longest_path = 0;

        while (!q.empty())
        {
            auto s = q.front(); q.pop();

            assert(s.cur_node->id < 64);
            set_visited(s);

            for (const auto& edge : s.cur_node->edges)
            {
                if (get_visited(s, edge.target))
                {
                    continue;
                }
                
                if (edge.target == m_exit_node)
                {
                    const auto  new_steps = edge.distance + s.steps;
                    if (longest_path < new_steps)
                    {
                        //cout << "found exit at step " << new_steps << ", visited " << s.visited << " nodes out of " << m_nodes.size() << " qsize " << q.size() << endl;
                    }

                    longest_path = std::max(longest_path, (uint16_t)new_steps);
                }
                else
                {
                    q.push(search{ static_cast<uint16_t>(s.steps + edge.distance), edge.target, s.visited });
                }
            }
        }

        return longest_path;
    }

}