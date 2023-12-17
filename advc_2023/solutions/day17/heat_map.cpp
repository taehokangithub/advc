#include <assert.h>
#include <iostream>
#include <sstream>
#include <unordered_set>

#include "../../utils/input_utils.h"
#include "heat_map.h"

using namespace std;

namespace advc_2023::day17
{
    void Heat_map::parse(const vector<string>& lines)
    {
        m_grid.set_size((int)lines.front().size(), (int)lines.size());

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                assert(c >= '0' && c <= '9');
                m_grid.add_element(c - '0');
            }
        }

        assert(m_grid.is_all_set());
    }

    // -------------------------------------------------

    std::string Search::to_string() const
    {
        stringstream ss;
        const utils::Point cur_loc(x, y);

        ss << cur_loc.to_string() << " " << utils::Point::get_dir_name(dir) << " ==> " << heat_loss << " (st " << (int) straight << ")";
        return ss.str();
    }

    // -------------------------------------------------

    int Search::get_unique_id() const
    {
        return (static_cast<int>(dir)) + (x << 8) + (y << 16) + (straight << 24);
    }
    // -------------------------------------------------

    int Heat_map::get_min_heat_loss(const int min_steps, const int max_steps) const
    {
        const utils::Point exit_point(m_grid.get_size().x - 1, m_grid.get_size().y - 1);
        search_queue q;

        // Create default searches
        Search begin;
        begin.dir = utils::Dir::Right;
        q.push(begin);

        begin.dir = utils::Dir::Down;
        q.push(begin);

        unordered_set<int> visited;

        const auto add_to_queue = [&q, this, min_steps, max_steps](const Search& s, utils::Dir rotate_dir)
            {
                if (s.straight < min_steps && rotate_dir != utils::Dir::Up)
                {
                    return;
                }

                const int straight = (rotate_dir == utils::Dir::Up ? (s.straight + 1) : 1);

                if (straight > max_steps)
                {
                    return;
                }

                utils::Point loc(s.x, s.y);

                const auto new_dir = utils::Point::get_rotated_dir(s.dir, rotate_dir);
                const auto new_loc = loc.get_moved(new_dir);

                if (!m_grid.is_valid_point(new_loc))
                {
                    return;
                }

                Search new_search;
                new_search.straight = straight;
                new_search.dir = new_dir;
                new_search.x = new_loc.x;
                new_search.y = new_loc.y;
                new_search.heat_loss = m_grid.get(new_loc) + s.heat_loss;
                q.push(new_search);
            };

        while (!q.empty())
        {
            Search s = q.top();
            q.pop();

            const auto unique_id = s.get_unique_id();

            if (visited.find(unique_id) != visited.end())
            {
                continue;
            }

            visited.insert(unique_id);

            //cout << s.to_string() << ", q " << q.size() << endl;
            
            if (utils::Point(s.x, s.y) == exit_point && s.straight >= min_steps)
            {
                return s.heat_loss;
            }

            add_to_queue(s, utils::Dir::Up);
            add_to_queue(s, utils::Dir::Left);
            add_to_queue(s, utils::Dir::Right);
        }

        assert(false);
        return 0;
    }
}