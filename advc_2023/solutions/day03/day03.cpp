#include <assert.h>
#include <iostream>

#include "../../utils/grid.h"
#include "gear.h"

using namespace std;

namespace advc_2023::day03
{

	static int part1(const vector<string>& lines)
	{
        const auto grid = create_grid(lines);
        const auto numbers = extract_numbers(grid);

        int sum = 0;
        for (const auto& number : numbers)
        {
            if (number.part_symbol != NO_PART)
            {
                sum += number.value;
            }
        }
        return sum;
	}

	static int part2(const vector<string>& lines)
	{
        const auto grid = create_grid(lines);
        const auto numbers = extract_numbers(grid);
        const auto gears = extract_gears(numbers);

        int sum = 0;
        for (const auto& [_, gear] : gears)
        {
            if (gear.numbers.size() == 2)
            {
                sum += (gear.numbers.front().value * gear.numbers.back().value);
            }
        }
        return sum;
	}

	void solve()
	{
		const auto& lines{ utils::get_lines("solutions/day03/input.txt") };
		
		const int ans1 = part1(lines);
		cout << "[DAY03] PART 1 : " << ans1 << endl;
		assert(ans1 == 539637);

		const int ans2 = part2(lines);
		cout << "[DAY03] PART 2 : " << ans2 << endl;
		assert(ans2 == 82818007);
	}
}

