#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "games.h"

namespace advc_2023::day02
{
    static int part1(const vector<string>& lines)
    {
        Games games;
        games.parse(lines);

        Filter_type filter;
        filter[Colour::blue] = 14;
        filter[Colour::green] = 13;
        filter[Colour::red] = 12;

        return games.get_sum_of_passed_games(filter);
    }

    static int part2(const vector<string>& lines)
    {
        Games games;
        games.parse(lines);

        return games.get_sum_of_powers();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day02/input.txt") };

        const int ans1 = part1(lines);
        cout << "[DAY02] PART 1 : " << ans1 << endl;
        assert(ans1 == 2169);

        const int ans2 = part2(lines);
        cout << "[DAY02] PART 2 : " << ans2 << endl;
        assert(ans2 == 60948);
    }
}

