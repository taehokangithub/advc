#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "contraption.h"

using namespace std;

namespace advc_2023::day16
{

    static int part1(const vector<string>& lines)
    {
        Contraption c;
        c.parse(lines);
        return c.count_energised();
    }

    static int part2(const vector<string>& lines)
    {
        Contraption c;
        c.parse(lines);
        return c.count_max_energised();
    }

    static void test()
    {
        const auto& lines{ utils::get_lines("solutions/day16/data/input_s1.txt") };

        auto ans = part1(lines);
        assert(ans == 46);

        ans = part2(lines);
        assert(ans == 51);
    }

    void solve()
    {
        test();
        const auto& lines{ utils::get_lines("solutions/day16/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY16] PART 1 : " << ans1 << endl;
        assert(ans1 == 8021);

#ifndef _DEBUG // takes 30+ seconds in debug
        const auto ans2 = part2(lines);
        cout << "[DAY16] PART 2 : " << ans2 << endl;
        assert(ans2 == 8216);
#endif
    }
}

