#include <string>
#include <vector>

#include "../../utils/map.h"

namespace advc_2023::day18
{
    enum cell_type 
    {
        none,
        external,
        internal,
        edge,
    };

    // --------------------------------------------------------

    struct Cell
    {
        cell_type   type{ cell_type::none };
        std::string colour;
    };

    // --------------------------------------------------------

    struct Dig_instruction
    {
        utils::Dir dir;
        int distance;
        std::string colour;
    };

    // --------------------------------------------------------

    struct Horizontal_line
    {
        int y;
        int x_start;
        int x_end;

        bool is_valid() const { return x_start < x_end; }
        void invalidate() { x_end = x_start; }
        std::string to_string() const;
    };

    // --------------------------------------------------------

    class Dig_plan
    {
    public:
        void parse(const std::vector<std::string>& lines);
        void swap_instructions();
        uint64_t get_cubic_meters() const;

    private:
        void setup_horizontal_lines() const;

        std::vector<Dig_instruction> m_instructions;
        mutable std::vector<Horizontal_line> m_lines;
    };
}