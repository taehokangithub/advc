
#include <map>
#include <string>

#include "../../utils/input_utils.h"

namespace advc_2023::day03
{
    const char NO_PART{ '.' };
    struct Number
    {
        int y{ 0 };
        int x_start{ 0 };
        int x_end{ 0 }; // inclusive
        int value{ 0 };
        char part_symbol{ NO_PART };
        utils::Point gear_location;
    };

    struct Gear
    {
        utils::Point location;
        std::vector<Number> numbers;
    };

    utils::Grid<char> create_grid(const std::vector<std::string>& lines);

    std::vector<Number> extract_numbers(const utils::Grid<char>& grid);

    std::map<std::string, Gear> extract_gears(const std::vector<Number>& numbers);
}