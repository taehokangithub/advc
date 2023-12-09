#pragma once

#include <string>
#include <vector>

namespace advc_2023::day09
{
    enum class Dir
    {
        Forward, Backward
    };

    // ------------------------------------------

    class History
    {
    public:
        void add_history_number(int number) { m_numbers.push_back(number); }
        int get_extrapolation(Dir dir) const;

    private:
        std::vector<int> m_numbers;
    };

    // ------------------------------------------

    class History_container
    {
    public:
        void parse(const std::vector<std::string>& lines);
        int64_t get_sum_extrapolation(Dir dir) const;

    private:
        std::vector<History> m_histories;
    };
}