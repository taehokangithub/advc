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

        return g.get_num_visits_part2(26501365);
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day21/data/input_s1.txt") };

        Garden g;
        g.parse(lines);

        assert(g.get_num_visits(6) == 16);
#endif
    }

    void solve()
    {
        test(); // not working with part 2 samples
        
        const auto& lines{ utils::get_lines("solutions/day21/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY21] PART 1 : " << ans1 << endl;
        assert(ans1 == 3722);

        //!!! I could not understand the problem by myself
        //!!! so I had to dig up the internet and found this explanation
        //!!! https://github.com/villuna/aoc23/wiki/A-Geometric-solution-to-advent-of-code-2023,-day-21
        
        const auto ans2 = part2(lines);
        cout << "[DAY21] PART 2 : " << ans2 << endl;
        assert(ans2 == 614864614526014);
    }
}

