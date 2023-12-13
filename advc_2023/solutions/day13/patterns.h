#include <string>
#include <vector>

#include "../../utils/grid.h"

namespace advc_2023::day13
{
    enum class tile_type { none, rock, ash };
    enum class mirror_type { none, horizontal, vertical };

    // --------------------------------------------------------------

    class Pattern
    {
    public:
        Pattern(const std::vector<std::string>& lines);

        void set_mirror_point();

        std::shared_ptr<Pattern> get_smudged_pattern() const;
        int get_pattern_score() const;

    private:
        bool check_mirror(mirror_type mtype, int pos) const;
        bool compare_two_lines(mirror_type mtype, int pos1, int pos2) const;

        utils::Grid<tile_type> m_grid;
        mirror_type m_mirror_type{ mirror_type::none };
        int m_mirror_pos{ 0 }; // count of before mirror
    };

    // --------------------------------------------------------------

    class Patterns
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int find_mirrior_number() const;
        int find_smudged_mirror_number() const;

    private:
        std::vector<std::shared_ptr<Pattern>> m_patterns;
    };
}