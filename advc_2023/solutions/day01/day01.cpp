#include <assert.h>
#include <iostream>
#include <string>

#include "../../utils/input_utils.h"

using namespace std;

namespace advc_2023::day01
{
    const vector<string> digit_strings = {
        "one", "two", "three", "four", "five", "six", "seven", "eight", "nine"
    };

    struct Matching_result
    {
        bool matched;
        int value;
        int length;
    };

    static Matching_result find_matching_str(const string& str, int index)
    {
        string search_target = str.substr(index);

        for (int i = 0; i < (int)digit_strings.size(); i++)
        {
            const auto& digit_str = digit_strings[i];

            if (search_target.find(digit_str) == 0)
            {
                return { true, i + 1, (int)digit_str.length() };
            }
        }

        return { false, 0, 0 };
    }


    static int solve_internal(const vector<string>& lines, bool check_string)
    {
        int sum = 0;
        for (const auto& line : lines)
        {
            vector<int> digits;

            for (int i = 0; i < (int)line.size(); i++)
            {

                const char c = line[i];
                if (isdigit(c))
                {
                    digits.push_back(c - '0');
                }
                else if (check_string)
                {
                    const auto& match_result = find_matching_str(line, i);
                    if (match_result.matched)
                    {
                        digits.push_back(match_result.value);
                    }
                }
            }
            sum += digits.front() * 10 + digits.back();
        }
        return sum;
    }

    void solve()
    {
        const auto& lines{ utils::get_lines("solutions/day01/input.txt") };

        const int ans1 = solve_internal(lines, false);
        cout << "[DAY01] PART 1 : " << ans1 << endl;
        assert(ans1 == 54331);

        const int ans2 = solve_internal(lines, true);
        cout << "[DAY01] PART 2 : " << ans2 << endl;
        assert(ans2 == 54518);

    }
}

