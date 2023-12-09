#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "history_container.h"

using namespace std;

namespace advc_2023::day09
{
    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day09/input.txt") };

        History_container hc;
        hc.parse(lines);

        const auto ans1 = hc.get_sum_extrapolation(Dir::Forward);
        cout << "[DAY09] PART 1 : " << ans1 << endl;
        assert(ans1 == 1938731307);

        const auto ans2 = hc.get_sum_extrapolation(Dir::Backward);
        cout << "[DAY09] PART 2 : " << ans2 << endl;
        assert(ans2 == 948);
    }
}

