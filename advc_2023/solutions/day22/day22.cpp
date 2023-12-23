#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "brick.h"

using namespace std;

namespace advc_2023::day22
{

    static int part1(const vector<string>& lines)
    {
        Brick_map bm;
        bm.parse(lines);

        return bm.count_disintegratable_bricks();
    }

    static int part2(const vector<string>& lines)
    {
        Brick_map bm;
        bm.parse(lines);

        return bm.count_chain_reactions();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day22/data/input_s1.txt") };

        auto ans = part1(lines);
        assert(ans == 5);

        ans = part2(lines);
        assert(ans == 7);
#endif        
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day22/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY22] PART 1 : " << ans1 << endl;
        assert(ans1 == 454);

#ifndef _DEBUG // takes 9 secs in debug
        const auto ans2 = part2(lines);
        cout << "[DAY22] PART 2 : " << ans2 << endl;
        assert(ans2 == 74287);
#endif
    }
}

