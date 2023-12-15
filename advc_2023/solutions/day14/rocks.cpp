#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "rocks.h"

using namespace std;

namespace advc_2023::day14
{
    void Rocks::parse(const vector<string>& lines)
    {
        m_grid.set_size((int)lines.front().size(), (int)lines.size());

        for (const string& line : lines)
        {
            for (const char c : line)
            {
                tile_type tt = tile_type::none;
                switch (c)
                {
                case 'O': tt = tile_type::rock; break;
                case '#': tt = tile_type::wall; break;
                case '.': tt = tile_type::empty; break;
                default: assert(false);
                }
                assert(tt != tile_type::none);
                m_grid.add_element(tt);
            }
        }
    }

    // ----------------------------------------------------

    void Rocks::draw() const
    {
        cout << " --------------------------------- " << endl;

        for (int y = 0; y < m_grid.get_size().y; y++)
        {
            for (int x = 0; x < m_grid.get_size().x; x++)
            {
                auto tt = m_grid.get(x, y);
                char c = ' ';
                switch (tt)
                {
                case tile_type::empty: c = '.'; break;
                case tile_type::rock: c = 'O'; break;
                case tile_type::wall: c = '#'; break;
                default: assert(false);
                }
                cout << c;
            }
            cout << endl;
        }
    }

    // ----------------------------------------------------

    void Rocks::tilt(utils::Dir dir)
    {
        int start_x = 0, start_y = 0;
        int step_x = 1, step_y = 1;
        int end_x = m_grid.get_size().x;
        int end_y = m_grid.get_size().y;

        switch (dir)
        {
        case utils::Dir::Up: break;
        case utils::Dir::Left: break;
        case utils::Dir::Down: 
            start_y = end_y - 1;
            end_y = -1;
            step_y = -1;
            break;
        case utils::Dir::Right:
            start_x = end_x - 1;
            end_x = -1;
            step_x = -1;
        }


        for (int y = start_y; y != end_y; y += step_y)
        {
            for (int x = start_x; x != end_x; x += step_x)
            {
                assert(m_grid.is_valid_point(x, y));
                if (m_grid.get(x, y) == tile_type::rock)
                {
                    utils::Point org_pos(x, y);
                    utils::Point cur_pos(x, y);

                    while (true)
                    {
                        utils::Point next_pos = cur_pos.get_moved(dir);

                        if (!m_grid.is_valid_point(next_pos) || m_grid.get(next_pos) != tile_type::empty)
                        {
                            break;
                        }
                        cur_pos = next_pos;
                    }

                    m_grid.set(org_pos, tile_type::empty);
                    m_grid.set(cur_pos, tile_type::rock);
                }
            }
        }
    }

    // ----------------------------------------------------

    void Rocks::spin_once()
    {
        tilt(utils::Dir::Up);
        tilt(utils::Dir::Left);
        tilt(utils::Dir::Down);
        tilt(utils::Dir::Right);
    }


    // ----------------------------------------------------

    void Rocks::spin(int times)
    {
        // Inexplicably I've found it's repeating itself in every 7 spins after 'enough' amount of time
        constexpr int repeat = 7; 
        constexpr int enough_times = 1000;

        vector<int> records;
        int repeat_start = 0;

        for (int i = 0; i < enough_times; i ++)
        {
            spin_once();
            const int load = get_total_load();
            records.push_back(load);

            if (records.size() > repeat && records[i] == records[i - repeat])
            {
                repeat_start = i + 1;
                break;
            }
        }

        assert(repeat_start >= repeat);
        
        const int required_spins = (times - repeat_start) % repeat;

        for (int i = 0; i < required_spins; i++)
        {
            spin_once();
        }
    }

    // ----------------------------------------------------

    int Rocks::get_total_load() const
    {
        const int max_y = m_grid.get_size().y;
        const int max_x = m_grid.get_size().x;
        int sum = 0;

        for (int y = max_y - 1; y >= 0; y--)
        {
            const int load = max_y - y;
            for (int x = 0; x < max_x; x++)
            {
                if (m_grid.get(x, y) == tile_type::rock)
                {
                    sum += load;
                }
            }
        }
        return sum;
    }

}