#include <string>
#include <vector>

#include "../../utils/grid.h"

namespace advc_2023::day14
{
    class Rocks
    {
        enum tile_type
        {
            none, rock, wall, empty
        };

    public:
        void parse(const std::vector<std::string>& lines);
        void tilt(utils::Dir dir);
        void spin(int times);

        int get_total_load() const;

    private:
        void spin_once();
        void draw() const;

        utils::Grid<tile_type>  m_grid;
    };
}