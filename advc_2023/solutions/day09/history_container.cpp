
#include <assert.h>
#include <iostream>
#include <sstream>

#include "../../utils/input_utils.h"
#include "history_container.h"

using namespace std;

namespace advc_2023::day09
{
    using Numbers = vector<int>;

    static bool all_zero_checker(const Numbers& numbers)
    {
        for (const int n : numbers)
        {
            if (n != 0)
            {
                return false;
            }
        }
        return true;
    }

    // --------------------------------------------------------

    static string numbers_to_string(const Numbers& numbers)
    {
        stringstream ss;
        ss << "[";
        for (int i = 0; i < (int)numbers.size(); i++)
        {
            ss << numbers[i];
            if (i < (int)numbers.size() - 1)
            {
                ss << ",";
            }
        }
        ss << "]";
        return ss.str();
    }

    // --------------------------------------------------------

    int History::get_extrapolation(Dir dir) const
    {
        vector<Numbers> diffs;

        diffs.push_back(m_numbers);

        while (!all_zero_checker(diffs.back()))
        {
            const auto& last_numbers = diffs.back();

            Numbers new_numbers;

            for (int i = 0; i < (int)last_numbers.size() - 1; i++)
            {
                new_numbers.push_back(last_numbers[i + 1] - last_numbers[i]);
            }

            diffs.push_back(std::move(new_numbers));
        }

        int extrapolated = 0;
        for (auto ite = diffs.rbegin(); ite != diffs.rend(); ite ++)
        {
            if (dir == Dir::Forward)
            {
                extrapolated += ite->back();
            }
            else
            {
                extrapolated = ite->front() - extrapolated;
            }
        }

        return extrapolated;
    }

    // ----------------------------------------------------------------

    void History_container::parse(const vector<string>& lines)
    {
        for (const string& line : lines)
        {
            const auto& num_strs = utils::split(line, " ");

            History history;

            for (const string& num_str : num_strs)
            {
                history.add_history_number(stoi(num_str));
            }

            m_histories.push_back(std::move(history));
        }
    }

    // ------------------------------------------------------------

    int64_t History_container::get_sum_extrapolation(Dir dir) const
    {
        int64_t ret = 0;

        for (const auto history : m_histories)
        {
            ret += history.get_extrapolation(dir);
        }

        return ret;
    }

}