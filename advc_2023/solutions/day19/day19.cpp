#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "rules_system.h"

using namespace std;

namespace advc_2023::day19
{

    static int part1(const vector<string>& lines)
    {
        Rules_system r;
        r.parse(lines);

        return r.get_total_accepted();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Rules_system r;
        r.parse(lines);

        return r.get_total_accepted_cases();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day19/data/input_s1.txt", utils::line_option::dont_skip_empty_line) };

        uint64_t ans = part1(lines);
        assert(ans == 19114);

        ans = part2(lines);
        assert(ans == 167409079868000); 
#endif        
    }

    void solve()
    {
        test();
        
        const auto& lines{ utils::get_lines("solutions/day19/data/input.txt", utils::line_option::dont_skip_empty_line) };

        const auto ans1 = part1(lines);
        cout << "[DAY19] PART 1 : " << ans1 << endl;
        assert(ans1 == 449531);

        const auto ans2 = part2(lines);
        cout << "[DAY19] PART 2 : " << ans2 << endl;
        assert(ans2 == 122756210763577);
    }
}

