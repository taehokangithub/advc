#include <assert.h>
#include <string>
#include <sstream>

#include "../../utils/input_utils.h"
#include "record_container.h"

using namespace std;

namespace advc_2023::day06
{

    // ------------------------------------------------------------------

    bool RaceRecord::check_record_breakable(uint64_t push_time) const
    {
        return push_time * (m_time - push_time) > m_distance;
    }

    // ------------------------------------------------------------------

    uint64_t RaceRecord::find_number_of_strategies_brutal_force() const
    {
        uint64_t cnt = 0;
        for (uint64_t i = 1; i < m_time; i++)
        {
            if (check_record_breakable(i))
            {
                cnt++;
            }
        }
        return cnt;
    }

    // ------------------------------------------------------------------

    uint64_t RaceRecord::find_fist_success() const
    {
        for (uint64_t i = 1; i < m_time; i++)
        {
            if (check_record_breakable(i))
            {
                return i;
            }
        }
        assert(false);
        return 0;
    }

    // ------------------------------------------------------------------

    uint64_t RaceRecord::find_last_success() const
    {
        for (uint64_t i = m_time - 1; i > 0; i--)
        {
            if (check_record_breakable(i))
            {
                return i;
            }
        }
        assert(false);
        return 0;
    }

    // ------------------------------------------------------------------

    void RecordContainer::parse(const vector<string>& lines)
    {
        const auto times_str = utils::split(lines.front(), " ");
        const auto distances_str = utils::split(lines.back(), " ");

        for (int i = 1; i < (int)times_str.size(); i++)
        {
            uint64_t time = stoi(times_str[i]);
            uint64_t distance = stoi(distances_str[i]);
            m_records.push_back(RaceRecord(time, distance));
        }

        // parse big_record
        const auto parse_internal = [](const string& line) -> uint64_t
            {
                const auto divided = utils::split(line, ": ");
                assert((int)divided.size() == 2);

                const auto elements = utils::split(divided.back(), " ");

                stringstream ss;

                for (const string& element : elements)
                {
                    ss << element;
                }

                return stoull(ss.str());
            };

        const uint64_t time = parse_internal(lines.front());
        const uint64_t distance = parse_internal(lines.back());

        m_big_record = { time, distance };
    }

    // ------------------------------------------------------------------

    uint64_t RecordContainer::find_number_of_strategies() const
    {
        uint64_t ans = 1;

        for (const auto& record : m_records)
        {
            const uint64_t cnt = record.find_number_of_strategies_brutal_force();

            if (cnt > 0)
            {
                ans *= cnt;
            }
        }
        return ans;
    }

    // ------------------------------------------------------------------

    uint64_t RecordContainer::find_number_of_strategies_p2() const
    {
        const auto first_success = m_big_record.find_fist_success();
        const auto last_success = m_big_record.find_last_success();

        return last_success - first_success + 1;
    }

}

