#include <string>
#include <vector>

#include "../../utils/grid.h"

namespace advc_2023::day11
{
    struct Expansion
    {
        utils::Axis axis;
        short       location;
        short       left;
        short       right;
    };

    class Galaxy
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int64_t get_expansion_distances(int n) const;

    private:
        int64_t get_base_distances() const;
        std::vector<Expansion> get_expansions() const;

        int64_t m_base_distance;
        utils::Grid<int> m_grid;
        std::vector<utils::Point> m_stars;
        std::vector<Expansion> m_expansions;
    };
}