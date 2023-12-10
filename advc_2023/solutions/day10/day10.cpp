#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "pipes.h"

using namespace std;

namespace advc_2023::day10
{

    static int part1(const vector<string>& lines)
    {
        Pipes pipes;
        pipes.parse(lines);

        return pipes.get_distance_to_farthest();
    }

    static int part2(const vector<string>& lines)
    {
        Pipes pipes;
        pipes.parse(lines);

        return pipes.count_inner_tiles();
    }

    static void test()
    {
        auto ans = part1(utils::get_lines("solutions/day10/data/input_s1.txt"));
        assert(ans == 4);

        ans = part1(utils::get_lines("solutions/day10/data/input_s2.txt"));
        assert(ans == 8);

        ans = part2(utils::get_lines("solutions/day10/data/input_s1_p2.txt"));
        assert(ans == 4);

        ans = part2(utils::get_lines("solutions/day10/data/input_s2_p2.txt"));
        assert(ans == 8);

        ans = part2(utils::get_lines("solutions/day10/data/input_s3_p2.txt"));
        assert(ans == 10);
}

    void solve()
    {
        test();

        const auto& lines{ utils::get_lines("solutions/day10/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY10] PART 1 : " << ans1 << endl;
        assert(ans1 == 6690);

        const auto ans2 = part2(lines);
        cout << "[DAY10] PART 2 : " << ans2 << endl;
        assert(ans2 == 525);
    }
}

