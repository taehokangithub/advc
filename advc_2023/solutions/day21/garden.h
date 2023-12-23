#include <string>
#include <vector>

#include "../../utils/grid.h"
namespace advc_2023::day21
{
    class Garden
    {
        enum class tile_type : uint8_t { none, garden, rock, visit };
        enum class parity_type : uint8_t { even, odd };
        enum class compare_type : uint8_t { dont_care, greater, less_or_equal };
    public:
        void parse(const std::vector<std::string>& lines);

        uint64_t get_num_visits(int steps) const;
        uint64_t get_num_visits_part2(uint64_t steps) const;

    private:
        void draw(utils::Grid<tile_type>& grid);
        void fill_distance_grid();

        uint64_t count_parity_tiles(uint64_t steps, compare_type compare, parity_type parity) const;

        utils::Grid<tile_type> m_grid;
        utils::Grid<int> m_distance_grid;
        utils::Point m_entry;
    };
}