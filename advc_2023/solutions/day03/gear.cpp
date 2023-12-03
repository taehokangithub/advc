#include <assert.h>
#include <iostream>

#include "../../utils/grid.h"
#include "gear.h"

using namespace std;

namespace advc_2023::day03
{
    // ----------------------------------------------------------------

    utils::Grid<char> create_grid(const vector<string>& lines)
    {
        utils::Point size;
        size.y = (int)lines.size();
        size.x = (int)lines.front().size();

        utils::Grid<char> grid;
        grid.set_size(size);

        for (const auto& line : lines)
        {
            for (const char c : line)
            {
                grid.add_element(c);
            }
        }

        assert(grid.is_all_set());
        return grid;
    }

    // ----------------------------------------------------------------

    vector<Number> extract_numbers(const utils::Grid<char>& grid)
    {
        vector<Number> numbers;
        const auto& size = grid.get_size();

        Number number;
        for (int y = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++)
            {
                const char c = grid.get(x, y);
                if (std::isdigit(c))
                {
                    if (number.value == 0)
                    {
                        number.x_start = x;
                        number.y = y;
                    }
                    const int digit = c - '0';
                    number.value = number.value * 10 + digit;
                    number.x_end = x;
                }
                else
                {
                    if (number.value != 0)
                    {
                        numbers.push_back(number);
                        number.value = 0;
                    }
                }
            }
        }

        for (auto& number : numbers)
        {
            for (int y = number.y - 1; y <= number.y + 1; y++)
            {
                for (int x = number.x_start - 1; x <= number.x_end + 1; x++)
                {
                    if (grid.is_valid_point(x, y))
                    {
                        const char c = grid.get(x, y);
                        if (!std::isdigit(c) && c != NO_PART)
                        {
                            number.part_symbol = c;
                            number.gear_location = utils::Point(x, y);
                        }
                    }
                }
            }
        }

        return numbers;
    }

    // ----------------------------------------------------------------

    map<string, Gear> extract_gears(const vector<Number>& numbers)
    {
        map<string, Gear> gear_map;

        for (auto& number : numbers)
        {
            if (number.part_symbol != NO_PART)
            {
                const string loc_str = number.gear_location.to_string();

                auto& gear = gear_map[loc_str];

                gear.numbers.push_back(number);
            }
        }

        return gear_map;
    }

}