#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "navigator.h"

using namespace std;

namespace advc_2023::day08
{

    static int part1(const vector<string>& lines)
    {
        Navigator nav;
        nav.parse(lines);

        return nav.get_steps();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Navigator nav;
        nav.parse(lines);

        return nav.get_multiple_steps();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day08/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY08] PART 1 : " << ans1 << endl;
        assert(ans1 == 11911);

        const auto ans2 = part2(lines);
        cout << "[DAY08] PART 2 : " << ans2 << endl;
        assert(ans2 == 10151663816849);
    }
}

