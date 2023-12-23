#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "hiking.h"

using namespace std;

namespace advc_2023::day23
{

    static int part1(const vector<string>& lines)
    {
        Hiking hiking;
        hiking.parse(lines);

        return hiking.get_longest_path();
    }

    static int part2(const vector<string>& lines)
    {
        Hiking hiking;
        hiking.parse(lines);

        return hiking.get_longest_path_ignore_slope();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day23/data/input_s1.txt") };

        auto ans = part1(lines);
        assert(ans == 94);

        ans = part2(lines);
        assert(ans == 154);
#endif        
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day23/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY23] PART 1 : " << ans1 << endl;
        assert(ans1 == 2414);

#ifndef _DEBUG // takes 24s in debug, 3s in release
        const auto ans2 = part2(lines);
        cout << "[DAY23] PART 2 : " << ans2 << endl;
        assert(ans2 == 6598);
#endif
    }
}

