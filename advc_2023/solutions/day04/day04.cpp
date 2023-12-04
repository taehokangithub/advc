#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "cards.h"

using namespace std;

namespace advc_2023::day04
{
    static int part1(const vector<string>& lines)
    {
        CardDeck deck;
        deck.parse(lines);

        return deck.get_total_points();
    }

    static int part2(const vector<string>& lines)
    {
        CardDeck deck;
        deck.parse(lines);

        return deck.get_total_scratch_cards();
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day04/input.txt") };

        const int ans1 = part1(lines);
        cout << "[DAY04] PART 1 : " << ans1 << endl;
        assert(ans1 == 21105);

        const int ans2 = part2(lines);
        cout << "[DAY04] PART 2 : " << ans2 << endl;
        assert(ans2 == 5329815);
    }
}

