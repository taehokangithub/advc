#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "modules.h"

using namespace std;

namespace advc_2023::day20
{

    static int part1(const vector<string>& lines)
    {
        Modules_manager mm;
        mm.parse(lines);
        return mm.get_total_signal_score(1000);
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Modules_manager mm;
        mm.parse(lines);
        return mm.get_cnt_button_pressed("rx");
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines1{ utils::get_lines("solutions/day20/data/input_s1.txt") };
        const auto& lines2{ utils::get_lines("solutions/day20/data/input_s2.txt") };

        auto ans = part1(lines1);
        assert(ans == 32000000);

        ans = part1(lines2);
        assert(ans == 11687500);
#endif        
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day20/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY20] PART 1 : " << ans1 << endl;
        assert(ans1 == 898731036);

        const auto ans2 = part2(lines);
        cout << "[DAY20] PART 2 : " << ans2 << endl;
        assert(ans2 == 229414480926893);
    }
}

