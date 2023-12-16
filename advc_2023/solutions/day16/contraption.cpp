#include <assert.h>
#include <iostream>

#include "../../utils/input_utils.h"
#include "contraption.h"

using namespace std;

constexpr bool DISABLE_DBG_OUT = true;
#define DBG_OUT if (DISABLE_DBG_OUT) {} else cout

namespace advc_2023::day16
{
    void Contraption::parse(const vector<string>& lines)
    {
        m_grid.set_size((int)lines.front().size(), (int)lines.size());

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                tile_type tt = tile_type::none;

                switch (c)
                {
                case '.': tt = tile_type::empty; break;
                case '|': tt = tile_type::pass_vertical; break;
                case '-': tt = tile_type::pass_horizontal; break;
                case '/': tt = tile_type::mirror_up; break;
                case '\\': tt = tile_type::mirror_down; break;
                default: assert(false);
                }

                m_grid.add_element(tt);
            }
        }

        assert(m_grid.is_all_set());
    }


    // -----------------------------------------------------------------

    beam_vector Contraption::handle_reflection(beam_type& beam) const
    {
        beam_vector bv;

        const tile_type tt = m_grid.get(beam.location);

        switch (tt)
        {
        case tile_type::mirror_up:
            switch (beam.dir)
            {
            case utils::Dir::Right: beam.dir = utils::Dir::Up; break;
            case utils::Dir::Left: beam.dir = utils::Dir::Down; break;
            case utils::Dir::Up: beam.dir = utils::Dir::Right; break;
            case utils::Dir::Down: beam.dir = utils::Dir::Left; break;
            default: assert(false);
            }
            break;

        case tile_type::mirror_down:
            switch (beam.dir)
            {
            case utils::Dir::Right: beam.dir = utils::Dir::Down; break;
            case utils::Dir::Left: beam.dir = utils::Dir::Up; break;
            case utils::Dir::Up: beam.dir = utils::Dir::Left; break;
            case utils::Dir::Down: beam.dir = utils::Dir::Right; break;
            default: assert(false);
            }
            break;

        case tile_type::pass_horizontal:
            if (beam.dir == utils::Dir::Down || beam.dir == utils::Dir::Up)
            {
                beam.dir = utils::Dir::Left;
                auto new_beam = beam_type(beam);
                new_beam.dir = utils::Dir::Right;
                bv.push_back(new_beam);
            }
            break;

        case tile_type::pass_vertical:
            if (beam.dir == utils::Dir::Left || beam.dir == utils::Dir::Right)
            {
                beam.dir = utils::Dir::Up;
                auto new_beam = beam_type(beam);
                new_beam.dir = utils::Dir::Down;
                bv.push_back(new_beam);
            }
            break;

        case tile_type::empty:
            break;

        default:
            assert(false);
        }

        return bv;
    }

    // -----------------------------------------------------------------

    uint32_t Search::get_unique_id(const beam_type& beam)
    {
        const auto x = static_cast<uint8_t>(beam.location.x);
        const auto y = static_cast<uint8_t>(beam.location.y);
        const auto dir = static_cast<uint8_t>(beam.dir);

        return (x << 16) + (y << 8) + dir;
    }

    // -----------------------------------------------------------------


    bool Contraption::handle_beams_one_tick(Search& data) const
    {
        beam_vector new_beams;
        bool has_anything_moved = false;

        for (auto& beam : data.beams)
        {
            if (m_grid.is_valid_point(beam.location))
            {
                const auto unique_id = data.get_unique_id(beam);
                if (data.visited.find(unique_id) == data.visited.end())
                {
                    data.visited.insert(unique_id);
                    has_anything_moved = true;
                    data.energised.set(beam.location, true);

                    auto new_beam = handle_reflection(beam);

                    if (new_beam.size())
                    {
                        assert(new_beam.size() == 1);

                        beam_type& nb = new_beam.front();
                        nb.move();

                        new_beams.push_back(nb);
                    }

                    beam.move();
                }
            }
        }

        data.beams.insert(data.beams.begin(), new_beams.begin(), new_beams.end());
        return has_anything_moved;
    }

    // -----------------------------------------------------------------

    int Contraption::count_energised(const beam_type& starting_beam) const
    {
        Search data;
        data.beams.push_back(beam_type(starting_beam));

        while (handle_beams_one_tick(data));

        return (int)data.energised.get_data().size();
    }

    // -----------------------------------------------------------------

    int Contraption::count_energised() const
    {
        const auto starting_beam = beam_type(utils::Point(0, 0), utils::Dir::Right);

        return count_energised(starting_beam);
    }

    // -----------------------------------------------------------------

    int Contraption::count_max_energised() const
    {
        int ret = 0;
        const utils::Point& size = m_grid.get_size();

        for (int x = 0; x < size.x; x++)
        {
            const auto starting_beam1 = beam_type(utils::Point(x, 0), utils::Dir::Down);
            const auto starting_beam2 = beam_type(utils::Point(x, size.y - 1), utils::Dir::Up);

            auto ans = count_energised(starting_beam1);
            DBG_OUT << starting_beam1.to_string() << " => " << ans << endl;
            ret = std::max(ans, ret);

            ans = count_energised(starting_beam2);
            DBG_OUT << starting_beam2.to_string() << " => " << ans << endl;
            ret = std::max(ans, ret);
        }

        for (int y = 0; y < size.y; y++)
        {
            const auto starting_beam1 = beam_type(utils::Point(0, y), utils::Dir::Right);
            const auto starting_beam2 = beam_type(utils::Point(size.x - 1, y), utils::Dir::Left);

            auto ans = count_energised(starting_beam1);
            DBG_OUT << starting_beam1.to_string() << " => " << ans << endl;
            ret = std::max(ans, ret);

            ans = count_energised(starting_beam2);
            DBG_OUT << starting_beam2.to_string() << " => " << ans << endl;
            ret = std::max(ans, ret);
        }
        
        return ret;
    }

}