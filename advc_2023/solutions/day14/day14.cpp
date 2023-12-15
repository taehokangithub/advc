#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "rocks.h"

using namespace std;

namespace advc_2023::day14
{

    static int part1(const vector<string>& lines)
    {
        Rocks rocks;
        rocks.parse(lines);
        rocks.tilt(utils::Dir::Up);

        return rocks.get_total_load();
    }

    static int part2(const vector<string>& lines)
    {
        Rocks rocks;
        rocks.parse(lines);
        rocks.spin(1000000000);

        return rocks.get_total_load();
    }

    static void test()
    {
        const auto& lines{ utils::get_lines("solutions/day14/data/input_s1.txt") };

        auto ans = part1(lines);
        assert(ans == 136);

        ans = part2(lines);
        assert(ans = 64);
    }

    void solve()
    {
        test();

        const auto& lines{ utils::get_lines("solutions/day14/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY14] PART 1 : " << ans1 << endl;
        assert(ans1 == 109665);

        const auto ans2 = part2(lines);
        cout << "[DAY14] PART 2 : " << ans2 << endl;
        assert(ans2 == 96061);
    }
}

