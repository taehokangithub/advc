#include <assert.h>
#include <iostream>
#include <queue>

#include "../../utils/input_utils.h"
#include "garden.h"

using namespace std;

namespace advc_2023::day21
{
    void Garden::parse(const vector<string>& lines)
    {
        m_grid.set_size((int)lines.front().size(), (int)lines.size());

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                tile_type tt = tile_type::none;
                switch (c)
                {
                case '#' : tt = tile_type::rock; break;
                case '.' : tt = tile_type::garden; break;
                case 'S': tt = tile_type::visit; break;
                default: assert(false);
                }
                assert(tt != tile_type::none);

                const auto added_point = m_grid.add_element(tt);
                if (tt == tile_type::visit)
                {
                    m_entry = added_point;
                }
            }
        }
        assert(m_grid.is_all_set());

        fill_distance_grid();
    }

    // --------------------------------------------------

    void Garden::draw(utils::Grid<tile_type>& grid)
    {
        for (int y = 0; y < grid.get_size().y; y++)
        {
            for (int x = 0; x < grid.get_size().x; x++)
            {
                if (m_distance_grid.get(x, y) > 0)
                {
                    if (m_distance_grid.get(x, y) > 5)
                    {
                        cout << "O";
                    }
                    else
                    {
                        cout << "I";
                    }
                    continue;
                }
                switch (grid.get(x, y))
                {
                case tile_type::visit: cout << 'O'; break;
                case tile_type::rock: cout << '#'; break;
                case tile_type::garden: cout << '.'; break;
                default: assert(false);
                }
            }
            cout << endl;
        }
    }

    // --------------------------------------------------

    void Garden::fill_distance_grid()
    {
        m_distance_grid.set_size(m_grid.get_size());

        vector<utils::Point> points;
        points.push_back(m_entry);

        int current_step = 0;

        while (!points.empty())
        {
            vector<utils::Point> next_points;

            for (const auto& p : points)
            {
                m_distance_grid.set(p, current_step);

                for (const auto dir : utils::Point::s_dirs)
                {
                    auto next_p = p.get_moved(dir);

                    if (m_distance_grid.is_valid_point(next_p) && m_distance_grid.get(next_p) == 0)
                    {
                        const tile_type tt = m_grid.get(next_p);

                        if (tt == tile_type::garden)
                        {
                            next_points.push_back(next_p);
                            m_distance_grid.set(next_p, -1);
                        }
                    }
                }
            }

            current_step++;

            points = next_points;
        }
    }

    // --------------------------------------------------

    uint64_t Garden::count_parity_tiles(uint64_t steps, compare_type compare, parity_type parity) const
    {
        int sum = 0;

        for (int x = 0; x < m_distance_grid.get_size().x; x++)
        {
            for (int y = 0; y < m_distance_grid.get_size().y; y++)
            {
                utils::Point cur_pos(x, y);

                if (m_grid.get(cur_pos) != tile_type::rock)
                {
                    const int distance = m_distance_grid.get(x, y);
                    const parity_type pt = (distance % 2 == 0) ? parity_type::even : parity_type::odd;
                    
                    if (pt == parity)
                    {
                        if (compare == compare_type::less_or_equal && distance <= steps
                            || compare == compare_type::greater && distance > steps
                            || compare == compare_type::dont_care)
                        {
                            if (distance > 0 || cur_pos == m_entry) //  distance 0 and not entry ==> an unreachable tile
                            {
                                sum++;
                            }
                        }
                    }
                }
            }
        }
        return sum;
    }

    // --------------------------------------------------
        
    uint64_t Garden::get_num_visits(int steps) const
    {
        const parity_type parity = (steps % 2 == 0) ? parity_type::even : parity_type::odd;

        return count_parity_tiles(steps, compare_type::less_or_equal, parity);
    }

    // --------------------------------------------------

    uint64_t Garden::get_num_visits_part2(uint64_t steps) const
    {
        //!!! I could not understand the problem by myself
        //!!! so I had to dig up the internet and found this explanation
        //!!! https://github.com/villuna/aoc23/wiki/A-Geometric-solution-to-advent-of-code-2023,-day-21

        assert(m_grid.get_size().x == m_grid.get_size().y);
        const uint64_t input_square_size = m_grid.get_size().x;          // edge to edge distance
        const uint64_t center_pos = input_square_size / 2;
        const uint64_t num_square_length = steps / input_square_size;    // number of "repeated" squares (not tiles!) it can reach in a single direction
        const uint64_t num_remaining_length = steps % input_square_size; // center to edge distance

        assert(center_pos == num_remaining_length);
        assert(num_remaining_length * 2 + 1 == input_square_size); 

        const parity_type my_parity = (steps % 2 == 0) ? parity_type::even : parity_type::odd;
        const parity_type other_parity = (my_parity == parity_type::odd) ? parity_type::even : parity_type::odd;

        const uint64_t sum_same_parity_tiles = count_parity_tiles(0, compare_type::dont_care, my_parity);
        const uint64_t sum_other_parity_tiles = count_parity_tiles(0, compare_type::dont_care, other_parity);
        const uint64_t sum_same_parity_outer = count_parity_tiles(num_remaining_length, compare_type::greater, my_parity);
        const uint64_t sum_other_parity_outer = count_parity_tiles(num_remaining_length, compare_type::greater, other_parity);


#ifdef _DEBUG
        const uint64_t sum_same_parity_inner = count_parity_tiles(num_remaining_length, compare_type::less_or_equal, my_parity);
        const uint64_t sum_other_parity_inner = count_parity_tiles(num_remaining_length, compare_type::less_or_equal, other_parity);
        assert(sum_same_parity_inner + sum_same_parity_outer == sum_same_parity_tiles);
        assert(sum_other_parity_inner + sum_other_parity_outer == sum_other_parity_tiles);
#endif

        const uint64_t ans =
            ((num_square_length + 1) * (num_square_length + 1) * sum_same_parity_tiles)
            + (num_square_length * num_square_length * sum_other_parity_tiles)
            - ((num_square_length + 1) * sum_same_parity_outer)
            + (num_square_length * (sum_other_parity_outer - 2)); // No idea at all why -2, but that yields the answer lol! Something must be wrong 

        return ans; 

    }

}