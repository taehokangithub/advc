#include <queue>
#include <string>
#include <vector>

#include "../../utils/actor.h"
#include "../../utils/grid.h"

namespace advc_2023::day17
{
    struct Search
    {
        uint16_t heat_loss{ 0 };
        uint8_t straight{ 0 };
        uint8_t x{ 0 };
        uint8_t y{ 0 };
        utils::Dir dir{};

        std::string to_string() const;
        int get_unique_id() const;
    };

    // ---------------------------------------------------------

    struct Compare
    {
        bool operator()(const Search& a, const Search& b)
        {
            return a.heat_loss > b.heat_loss;
        }
    };

    using search_queue = std::priority_queue<Search, std::vector<Search>, Compare>;

    // ---------------------------------------------------------

    class Heat_map
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int get_min_heat_loss(int min_steps, int max_steps) const;

    private:
        utils::Grid<uint8_t> m_grid;
    };
}