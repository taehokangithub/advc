#include <assert.h>
#include <iostream>

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

                m_grid.add_element(tt);
#if 0
                auto added_point = m_grid.add_element(tt);

                if (tt == tile_type::visit)
                {
                    m_entry = added_point;
                }
#endif
            }
        }
        assert(m_grid.is_all_set());
    }

    // --------------------------------------------------

    void Garden::draw(utils::Grid<tile_type>& grid)
    {
        for (int y = 0; y < grid.get_size().y; y++)
        {
            for (int x = 0; x < grid.get_size().x; x++)
            {
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

    void Garden::process_one_step(utils::Grid<tile_type>& grid)
    {
        for (int y = 0; y < grid.get_size().y; y++)
        {
            for (int x = 0; x < grid.get_size().x; x++)
            {
                const utils::Point p(x, y);
                if (grid.get(p) == tile_type::visit)
                {
                    for (const auto dir : utils::Point::s_dirs)
                    {
                        auto next_p = p.get_moved(dir);
                        if (grid.is_valid_point(next_p) && grid.get(next_p) != tile_type::rock)
                        {
                            grid.set(next_p, tile_type::visit_candidate);
                        }
                    }
                }
            }
        }

        for (int y = 0; y < grid.get_size().y; y++)
        {
            for (int x = 0; x < grid.get_size().x; x++)
            {
                const utils::Point p(x, y);
                const tile_type tt = grid.get(p);
                if (tt == tile_type::visit)
                {
                    grid.set(p, tile_type::garden);
                }
                else if (tt == tile_type::visit_candidate)
                {
                    grid.set(p, tile_type::visit);
                }
            }
        }
    }


    // --------------------------------------------------

    uint64_t Garden::get_num_visits(int steps) const
    {
        auto grid = m_grid;

        for (int i = 0; i < steps; i++)
        {
            process_one_step(grid);

            //draw(grid);
        }

        int sum = 0;
        for (int y = 0; y < m_grid.get_size().y; y++)
        {
            for (int x = 0; x < m_grid.get_size().x; x++)
            {
                if (grid.get(x, y) == tile_type::visit)
                {
                    sum++;
                }
            }
        }

        return sum;
    }

}