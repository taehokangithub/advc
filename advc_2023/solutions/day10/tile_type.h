#pragma once

#include <vector>

#include "../../utils/point.h"

namespace advc_2023::day10
{
    enum class tile_type
    {
        none,
        vertical,
        horizontal,
        ground,
        starting_position,
        L_shape,
        J_shape,
        seven_shape,
        F_shape,
    };

    namespace tile_type_utils
    {
        tile_type get_tile_type(const char c);

        tile_type get_tile_type_from_directions(const std::vector<utils::Dir>& dirs);

        std::vector<utils::Dir> get_possible_directions(const tile_type t);
    }
}