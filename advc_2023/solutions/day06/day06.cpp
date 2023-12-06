#include <assert.h>
#include <iostream>
#include <string>
#include <sstream>

#include "../../utils/input_utils.h"
#include "record_container.h"

using namespace std;

namespace advc_2023::day06
{

    static uint64_t part1(const vector<string>& lines)
    {
        RecordContainer rc;
        rc.parse(lines);

        return rc.find_number_of_strategies();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        RecordContainer rc;
        rc.parse(lines);

        return  rc.find_number_of_strategies_p2();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day06/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY06] PART 1 : " << ans1 << endl;
        assert(ans1 == 3316275);

        const auto ans2 = part2(lines);
        cout << "[DAY06] PART 2 : " << ans2 << endl;
        assert(ans2 == 27102791);
    }
}

