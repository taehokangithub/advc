#include <string>
#include <vector>

#include "../../utils/grid.h"
namespace advc_2023::day21
{
    class Garden
    {
        enum class tile_type { none, garden, rock, visit, visit_candidate };
    public:
        void parse(const std::vector<std::string>& lines);

        uint64_t get_num_visits(int steps) const;

    private:
        static void process_one_step(utils::Grid<tile_type>& grid);
        static void draw(utils::Grid<tile_type>& grid);

        utils::Grid<tile_type> m_grid;
        //utils::Point m_entry;
    };
}