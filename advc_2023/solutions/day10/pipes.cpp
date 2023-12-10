
#include <iostream>
#include <map>
#include <memory>
#include <queue>

#include "pipes.h"

using namespace std;

constexpr bool DISABLE_DBG_OUT = true;
#define DBG_OUT if (DISABLE_DBG_OUT) {} else cout

namespace advc_2023::day10
{
    string cluster_to_string(cluster_type t)
    {
        switch (t)
        {
        case cluster_type::boundary: return "Boundary";
        case cluster_type::inner: return "Inner";
        case cluster_type::outer: return "Outer";
        case cluster_type::none: return "None";
        }
        assert(false);
        return "";
    }

    // --------------------------------------------------
    void Pipes::parse(const vector<string>& lines)
    {
        const int y_max = (int)lines.size();
        const int x_max = (int)lines.front().size();

        m_starting_pos = { -1, -1 };
        m_grid.set_size(x_max, y_max);

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                const tile_type t = tile_type_utils::get_tile_type(c);
                utils::Point added_point = m_grid.add_element(t);

                if (t == tile_type::starting_position)
                {
                    m_starting_pos = added_point;
                }
            }
        }

        assert(m_grid.is_all_set());
        assert(m_grid.is_valid_point(m_starting_pos));

        m_grid.set(m_starting_pos, guess_starting_point_type());
    }

    // --------------------------------------------------

    tile_type Pipes::guess_starting_point_type() const
    {
        std::vector<utils::Dir> possible_dirs;

        for (const auto& p : utils::Point::s_dir_4)
        {
            if (const auto check_point = m_starting_pos.get_added(p)
                    ; m_grid.is_valid_point(check_point))
            {
                const tile_type t = m_grid.get(check_point);

                for (const auto dir : tile_type_utils::get_possible_directions(t))
                {
                    const auto moved_point = check_point.get_moved(dir);
                    if (moved_point == m_starting_pos)
                    {
                        possible_dirs.push_back(utils::Point::get_opposite_dir(dir));
                    }
                }
            }
        }

        assert(possible_dirs.size() == 2);
        const tile_type tile = tile_type_utils::get_tile_type_from_directions(possible_dirs);
        return tile;
    }

    // --------------------------------------------------

    int Pipes::get_distance_to_farthest() const
    {
        using point_queue = std::vector<utils::Point>;
        utils::Map<bool> visited;
        point_queue visit_q;

        visit_q.push_back(m_starting_pos);

        int steps = 0;

        while (!visit_q.empty())
        {
            point_queue next_queue;

            for (const auto current_point : visit_q)
            {
                visited.set(current_point, true);

                const tile_type current_tile = m_grid.get(current_point);

                for (auto dir : tile_type_utils::get_possible_directions(current_tile))
                {
                    const auto next_point = current_point.get_moved(dir);
                    assert(m_grid.is_valid_point(next_point));

                    if (visited.exists(next_point))
                    {
                        continue;
                    }

                    next_queue.push_back(next_point);
                }
            }

            if (next_queue.empty())
            {
                break; // don't bother increasing steps
            }

            visit_q = std::move(next_queue);
            steps++;
        }

        return steps;
    }

    // --------------------------------------------------

    int Pipes::count_inner_tiles() const
    {
        DBG_OUT << "-------------------------------------" << endl;

        utils::Map<int> cluster_map;
        vector<Cluster> clusters;

        Cluster init_cluster = get_cluster(0, m_starting_pos, cluster_map);
        clusters.push_back(init_cluster);

        for (int y = 0; y < m_grid.get_size().y; y++)
        {
            for (int x = 0; x < m_grid.get_size().x; x++)
            {
                utils::Point p(x, y);
                if (!cluster_map.exists(p))
                {
                    DBG_OUT << "** start exploring cluster from " << p.to_string() << ", index " << clusters.size() << endl;
                    Cluster cluster = get_cluster((int)clusters.size(), p, cluster_map);
                    DBG_OUT << "    ===> cluster from " << p.to_string() << ", type " << cluster_to_string(cluster.type) << ", count " << cluster.tile_count << endl << endl;
                    clusters.push_back(cluster);
                }
            }
        }

        int sum = 0;
        for (const auto& cluster : clusters)
        {
            if (cluster.type == cluster_type::inner)
            {
                sum += cluster.tile_count;
            }
        }
        return sum;
    }
    // --------------------------------------------------

    Cluster Pipes::get_cluster(int cluster_index, utils::Point starting_point, utils::Map<int>& cluster_map) const
    {
        Cluster cluster;
        cluster.start_point = starting_point;
        cluster.tile_count = 1;
        cluster.type = cluster_type::inner; // default inner but try to find out any clue

        if (starting_point == m_starting_pos)
        {
            cluster.type = cluster_type::boundary;
        }

        assert(!cluster_map.exists(starting_point));
        cluster_map.set(starting_point, cluster_index);

        std::queue<utils::Point> visit_queue;

        visit_queue.push(starting_point);

        while (!visit_queue.empty())
        {
            auto point = visit_queue.front();
            visit_queue.pop();

            assert(cluster_map.get(point) == cluster_index);

            // determine type if applicable
            if (cluster.type == cluster_type::inner)
            {
                if (point.x == 0 || point.y == 0 || point.x == m_grid.get_size().x - 1 || point.y == m_grid.get_size().y - 1)
                {
                    cluster.type = cluster_type::outer;
                }
            }

            //cluster_map.set(point, cluster_index);

            const tile_type current_tile = m_grid.get(point);

            const auto dirs = (cluster.type == cluster_type::boundary) 
                ? tile_type_utils::get_possible_directions(current_tile)    // for boundary type, follow only connected tiles
                : utils::Point::s_dirs;                                     // for other types, explore all 4 dirs

            for (auto dir : dirs)
            {
                const auto next_point = point.get_moved(dir);

                if (cluster_map.exists(next_point))
                {
                    continue;
                }
                if (m_grid.is_valid_point(next_point))
                {
                    //DBG_OUT << "[CLUSTER " << cluster_index << ":TYPE " << cluster_to_string(cluster.type) << "] from " << point.to_string() << " to " << next_point.to_string() << endl;
                    visit_queue.push(next_point);
                    cluster_map.set(next_point, cluster_index);
                    cluster.tile_count++;
                }
            }
        }

        if (cluster.type == cluster_type::inner)
        {
            int cnt_horizontal = 0; // count horizontal boundary until going up to the end of the map
            bool is_in_vertical = false;
            tile_type vertical_entrance = tile_type::none;

            for (int y = starting_point.y - 1; y >= 0; y--)
            {
                utils::Point p(starting_point.x, y);

                if (cluster_map.exists(p) && cluster_map.get(p) == 0) // 0th cluster : boundary
                {
                    const auto tile = m_grid.get(p);
                    if (tile == tile_type::horizontal )
                    {
                        assert(is_in_vertical == false);
                        cnt_horizontal++;
                    }
                    else if (tile == tile_type::L_shape || tile == tile_type::J_shape)
                    {
                        assert(is_in_vertical == false);
                        is_in_vertical = true;
                        vertical_entrance = tile;
                    }
                    else if (tile == tile_type::seven_shape || tile == tile_type::F_shape)
                    {
                        assert(is_in_vertical);
                        is_in_vertical = false;
                        if (vertical_entrance == tile_type::L_shape && tile == tile_type::seven_shape
                            || vertical_entrance == tile_type::J_shape && tile == tile_type::F_shape)
                        {
                            cnt_horizontal++;
                        }
                        vertical_entrance = tile_type::none;
                    }
                }
            }

            assert(is_in_vertical == false);

            if (cnt_horizontal % 2 == 0) // if the counter is even, it's an outer
            {
                cluster.type = cluster_type::outer;
            }
        }

        return cluster;
    }

}