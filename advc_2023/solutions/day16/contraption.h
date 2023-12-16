#include <unordered_set>
#include <string>
#include <vector>

#include "../../utils/actor.h"
#include "../../utils/grid.h"
#include "../../utils/map.h"

namespace advc_2023::day16
{
    using beam_vector = std::vector<utils::Actor>;
    using beam_type = utils::Actor;

    struct Search
    {
        utils::Map<bool> energised;
        beam_vector beams;
        std::unordered_set<uint32_t> visited;

        static uint32_t get_unique_id(const beam_type& beam);
    };

    class Contraption
    {
        enum tile_type
        {
            none,
            empty,
            mirror_up,
            mirror_down,
            pass_horizontal,
            pass_vertical
        };

    public:
        void parse(const std::vector<std::string>& lines);
        int count_energised() const;
        int count_max_energised() const;

    private:
        beam_vector handle_reflection(beam_type& beam) const;
        bool handle_beams_one_tick(Search& data) const;
        int count_energised(const beam_type& starting_beam) const;
        

        utils::Grid<tile_type> m_grid;
    };
}