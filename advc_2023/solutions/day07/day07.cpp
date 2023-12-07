#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "bids_set.h"

using namespace std;

namespace advc_2023::day07
{

    static int part1(const vector<string>& lines)
    {
        Bids_set bid_set;
        bid_set.parse(lines, rule_type::NORMAL);
        return bid_set.get_total_winnings();
    }

    static int part2(const vector<string>& lines)
    {
        Bids_set bid_set;
        bid_set.parse(lines, rule_type::JOKER);
        return bid_set.get_total_winnings();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day07/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY07] PART 1 : " << ans1 << endl;
        assert(ans1 == 246912307);

        const auto ans2 = part2(lines);
        cout << "[DAY07] PART 2 : " << ans2 << endl;
        assert(ans2 == 246894760);
    }
}

