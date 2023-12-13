#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "patterns.h"

using namespace std;

namespace advc_2023::day13
{

    static int part1(const vector<string>& lines)
    {
        Patterns p;
        p.parse(lines);

        return p.find_mirrior_number();
    }

    static int part2(const vector<string>& lines)
    {
        Patterns p;
        p.parse(lines);

        return p.find_smudged_mirror_number();
    }

    static void test()
    {
        const auto& lines{ utils::get_lines("solutions/day13/data/input_s1.txt", utils::line_option::dont_skip_empty_line) };

        auto ans = part1(lines);
        assert(ans == 405);

        ans = part2(lines);
        assert(ans == 400);
    }

    void solve()
    {
        test();

        const auto& lines{ utils::get_lines("solutions/day13/data/input.txt", utils::line_option::dont_skip_empty_line) };

        const auto ans1 = part1(lines);
        cout << "[DAY13] PART 1 : " << ans1 << endl;
        assert(ans1 == 37113);

        const auto ans2 = part2(lines);
        cout << "[DAY13] PART 2 : " << ans2 << endl;
        assert(ans2 == 30449);
    }
}

