#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "hailstone.h"

using namespace std;

namespace advc_2023::day24
{

    static int part1(const vector<string>& lines, uint64_t from, uint64_t to)
    {
        Hailstones hs;
        hs.parse(lines);

        return hs.count_all_intersections_2d(from, to);
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Hailstones hs;
        hs.parse(lines);

        return hs.get_passer_initial_position();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day24/data/input_s1.txt") };

        auto ans1 = part1(lines,7, 27);
        assert(ans1 == 2);

        auto ans2 = part2(lines);
        assert(ans2 == 47);
#endif
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day24/data/input.txt") };

        const auto ans1 = part1(lines, 200000000000000, 400000000000000);
        cout << "[DAY24] PART 1 : " << ans1 << endl;
        assert(ans1 == 27732);

        const auto ans2 = part2(lines);
        cout << "[DAY24] PART 2 : " << ans2 << endl;
        assert(ans2 == 641619849766168);
    }
}

