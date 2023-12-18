#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "dig_plan.h"

using namespace std;

namespace advc_2023::day18
{

    static uint64_t part1(const vector<string>& lines)
    {
        Dig_plan dp;
        dp.parse(lines);
        
        return dp.get_cubic_meters();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Dig_plan dp;
        dp.parse(lines);
        dp.swap_instructions();
        return  dp.get_cubic_meters();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day18/data/input_s1.txt") };
        auto ans = part1(lines);
        assert(ans == 62);

        ans = part2(lines);
        assert(ans == 9524'0814'4115);
#endif        
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day18/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY18] PART 1 : " << ans1 << endl;
        assert(ans1 == 74074);

        const auto ans2 = part2(lines);
        cout << "[DAY18] PART 2 : " << ans2 << endl;
        assert(ans2 == 112074045986829);
    }
}

