#pragma once

#include <string>
#include <vector>

#include "../../utils/grid.h"
#include "../../utils/map.h"
#include "tile_type.h"

namespace advc_2023::day10
{
    enum cluster_type
    {
        none, boundary, inner, outer
    };

    struct Cluster
    {
        utils::Point start_point;
        cluster_type type;
        int tile_count;
    };

    class Pipes
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int get_distance_to_farthest() const;
        int count_inner_tiles() const;

    private:
        tile_type guess_starting_point_type() const;
        Cluster get_cluster(int cluster_index, utils::Point starting_point, utils::Map<int>& cluster_map) const;


        utils::Grid<tile_type> m_grid;
        utils::Point m_starting_pos;
    };
}