#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"

using namespace std;

namespace advc_2023::day14
{

    static int part1(const vector<string>& lines)
    {
        return 0;
    }

    static int part2(const vector<string>& lines)
    {
        return 0;
    }

    static void test()
    {
        const auto& lines{ utils::get_lines("solutions/day14/data/input_s1.txt") };
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day14/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY14] PART 1 : " << ans1 << endl;
        //assert(ans1 == 0);

        const auto ans2 = part2(lines);
        cout << "[DAY14] PART 2 : " << ans2 << endl;
        //assert(ans2 == 54331);
    }
}

