#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "heat_map.h"

using namespace std;

namespace advc_2023::day17
{

    static int part1(const vector<string>& lines)
    {
        Heat_map heat_map;
        heat_map.parse(lines);
        return heat_map.get_min_heat_loss(0, 3);
    }

    static int part2(const vector<string>& lines)
    {
        Heat_map heat_map;
        heat_map.parse(lines);
        return heat_map.get_min_heat_loss(4, 10);
    }

    static void test()
    {
#ifdef _DEBUG
        const auto& lines{ utils::get_lines("solutions/day17/data/input_s1.txt") };
        auto ans = part1(lines);
        assert(ans == 102);

        ans = part2(lines);
        assert(ans == 94);

        const auto& lines_s2{ utils::get_lines("solutions/day17/data/input_s2.txt") };
        ans = part2(lines_s2);
        assert(ans == 71);
#endif
    }

    void solve()
    {
        test();

#ifndef _DEBUG // takes 8+ secs in debug
        const auto& lines{ utils::get_lines("solutions/day17/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY17] PART 1 : " << ans1 << endl;
        assert(ans1 == 845);

        const auto ans2 = part2(lines);
        cout << "[DAY17] PART 2 : " << ans2 << endl;
        assert(ans2 == 993);
#endif
    }
}

