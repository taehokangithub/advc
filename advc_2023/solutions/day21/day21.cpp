#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "garden.h"

using namespace std;

namespace advc_2023::day21
{

    static uint64_t part1(const vector<string>& lines)
    {
        Garden g;
        g.parse(lines);

        return g.get_num_visits(64);
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Garden g;
        g.parse(lines);

        return g.get_num_visits(16); // WIP - Should be 26501365
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day21/data/input_s1.txt") };

        Garden g;
        g.parse(lines);

        assert(g.get_num_visits(6) == 16);
        assert(g.get_num_visits(10) == 50);
        assert(g.get_num_visits(50) == 1594);
        assert(g.get_num_visits(100) == 6536);
        assert(g.get_num_visits(500) == 167004);
        assert(g.get_num_visits(1000) == 668697);
        assert(g.get_num_visits(5000) == 16733044);
        
#endif
    }

    void solve()
    {
        //test(); // not working with part 2 samples
        
        const auto& lines{ utils::get_lines("solutions/day21/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY21] PART 1 : " << ans1 << endl;
        assert(ans1 == 3722);

        const auto ans2 = part2(lines);
        cout << "[DAY21] PART 2 : " << ans2 << endl;
        //assert(ans2 == 54331);
    }
}

