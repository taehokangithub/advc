#include <assert.h>
#include <iostream>


#include "../../utils/input_utils.h"
#include "convert_maps.h"

using namespace std;

namespace advc_2023::day05
{
    static uint64_t part1(const vector<string>& lines)
    {
        Converter_maps maps;
        maps.parse(lines);

        return maps.find_lowest_final_value();
    }

    static uint64_t part2(const vector<string>& lines)
    {
        Converter_maps maps;
        maps.parse(lines);

        return maps.find_lowest_final_value_v2();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day05/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY05] PART 1 : " << ans1 << endl;
        assert(ans1 == 175622908);

        const auto ans2 = part2(lines);
        cout << "[DAY05] PART 2 : " << ans2 << endl;
        assert(ans2 == 5200543);
    }
}
