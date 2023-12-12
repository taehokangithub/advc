#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "condition_recorder.h"

using namespace std;

namespace advc_2023::day12
{

    static uint64_t part1(const vector<string>& lines)
    {
        Condition_recorder cr;
        cr.parse(lines);

        return cr.get_possible_counts();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Condition_recorder cr;
        cr.parse(lines);
        cr.unfold();

        return cr.get_possible_counts();
    }

    static void test()
    {
        const auto& lines = utils::get_lines("solutions/day12/data/input_s1.txt");
        auto ans = part1(lines);
        assert(ans == 21);

        ans = part2(lines);
        assert(ans == 525152);
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day12/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY12] PART 1 : " << ans1 << endl;
        assert(ans1 == 7922);

        const auto ans2 = part2(lines);
        cout << "[DAY12] PART 2 : " << ans2 << endl;
        assert(ans2 == 18093821750095);
    }
}

