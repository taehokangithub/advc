#pragma once

#include <string>
#include <vector>

namespace advc_2023::day06
{
    class RaceRecord
    {
    public:
        RaceRecord() {}
        RaceRecord(uint64_t time, uint64_t distance) : m_time(time), m_distance(distance) {}

        bool check_record_breakable(uint64_t push_time) const;

        uint64_t find_number_of_strategies_brutal_force() const;
        uint64_t find_fist_success() const;
        uint64_t find_last_success() const;

    private:
        uint64_t m_time {};
        uint64_t m_distance {};
    };

    // ----------------------------------------------------------

    class RecordContainer
    {
    public:
        void parse(const std::vector<std::string>& lines);
        uint64_t find_number_of_strategies() const;
        uint64_t find_number_of_strategies_p2() const;

    private:

        std::vector<RaceRecord> m_records;
        RaceRecord m_big_record;
    };
}