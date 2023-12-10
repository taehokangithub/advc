#include <assert.h>
#include <map>

#include "tile_type.h"

using namespace std;

namespace advc_2023::day10::tile_type_utils
{
    static const map<tile_type, vector<utils::Dir>> s_possible_ways{
        { tile_type::vertical,      { utils::Dir::Up, utils::Dir::Down }},
        { tile_type::horizontal,    { utils::Dir::Right, utils::Dir::Left }},
        { tile_type::L_shape,       { utils::Dir::Up, utils::Dir::Right }},
        { tile_type::J_shape,       { utils::Dir::Up, utils::Dir::Left }},
        { tile_type::F_shape,       { utils::Dir::Down, utils::Dir::Right }},
        { tile_type::seven_shape,   { utils::Dir::Down, utils::Dir::Left }},
    };

    static vector<utils::Dir> s_empty_dirs;

    // --------------------------------------------------

    tile_type get_tile_type_from_directions(const vector<utils::Dir>& dirs)
    {
        for (const auto& [tile, tile_dirs] : s_possible_ways)
        {
            if (dirs.front() == tile_dirs.front() || dirs.front() == tile_dirs.back())
            {
                if (dirs.back() == tile_dirs.front() || dirs.back() == tile_dirs.back())
                {
                    return tile;
                }
            }
        }

        assert(false);
        return tile_type::none;
    }

    // ----------------------------------------------------

    vector<utils::Dir> get_possible_directions(const tile_type t)
    {
        assert(t != tile_type::none);

        if (t == tile_type::ground)
        {
            return s_empty_dirs;
        }

        const auto ite = s_possible_ways.find(t);

        assert(ite != s_possible_ways.end());
        assert(ite->second.size() == 2);

        return ite->second;
    }

    // --------------------------------------------------

    tile_type get_tile_type(const char c)
    {
        switch (c)
        {
        case '|': return tile_type::vertical;
        case '-': return tile_type::horizontal;
        case 'L': return tile_type::L_shape;
        case 'J': return tile_type::J_shape;
        case '7': return tile_type::seven_shape;
        case 'F': return tile_type::F_shape;
        case '.': return tile_type::ground;
        case 'S': return tile_type::starting_position;
        }

        assert(false);
        return tile_type::none;
    }

}