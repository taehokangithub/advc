#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"
#include "components.h"

using namespace std;

namespace advc_2023::day25
{

    static int part1(const vector<string>& lines)
    {
        Components c;
        c.parse(lines);

        return c.get_clusters_signature();
    }

    static void test()
    {
#ifdef _DEBUG        
        const auto& lines{ utils::get_lines("solutions/day25/data/input_s1.txt") };

        const auto ans = part1(lines);
        assert(ans == 54);
#endif
    }

    void solve()
    {
        test();
     
#ifndef _DEBUG // took a minute in debug
        const auto& lines{ utils::get_lines("solutions/day25/data/input.txt") };

        const auto ans1 = part1(lines);
        cout << "[DAY25] PART 1 : " << ans1 << endl;
        assert(ans1 == 548960);
#endif
    }
}

