#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "galaxy.h"

using namespace std;

namespace advc_2023::day11
{

    static int64_t part1(const vector<string>& lines)
    {
        Galaxy galaxy;
        galaxy.parse(lines);
        return galaxy.get_expansion_distances(2);
    }

    static int64_t part2(const vector<string>& lines)
    {
        Galaxy galaxy;
        galaxy.parse(lines);
        return galaxy.get_expansion_distances(1000000);
    }

    static void test()
    {
        const auto& lines = utils::get_lines("solutions/day11/data/input_s1.txt");
        auto ans = part1(lines);
        assert(ans == 374);

        Galaxy galaxy;
        galaxy.parse(lines);

        ans = galaxy.get_expansion_distances(10);
        assert(ans == 1030);

        ans = galaxy.get_expansion_distances(100);
        assert(ans == 8410);
    }

    void solve()
    {
        test();

        const auto& lines{ utils::get_lines("solutions/day11/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY11] PART 1 : " << ans1 << endl;
        assert(ans1 == 9599070);

        const auto ans2 = part2(lines);
        cout << "[DAY11] PART 2 : " << ans2 << endl;
        assert(ans2 == 842645913794);
    }
}

