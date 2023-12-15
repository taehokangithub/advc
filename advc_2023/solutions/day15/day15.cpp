#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "hash.h"

using namespace std;

namespace advc_2023::day15
{

    static int part1(const string& line)
    {
        Hash hash;
        hash.parse(line);
        return hash.get_sum_hash();
    }

    static int part2(const string& line)
    {
        Hash hash;
        hash.parse(line);
        return hash.get_focusing_power();
    }

    static void test()
    {
        const string line = "rn=1,cm-,qp=3,cm=2,qp-,pc=4,ot=9,ab=5,pc-,pc=6,ot=7";
        auto ans = part1(line);
        assert(ans == 1320);

        ans = part2(line);
        assert(ans == 145);
    }

    void solve()
    {
        test();

        const auto& lines{ utils::get_lines("solutions/day15/data/input.txt") };

        const auto ans1 = part1(lines.front());
        cout << "[DAY15] PART 1 : " << ans1 << endl;
        assert(ans1 == 520500);

        const auto ans2 = part2(lines.front());
        cout << "[DAY15] PART 2 : " << ans2 << endl;
        assert(ans2 == 213097);
    }
}

